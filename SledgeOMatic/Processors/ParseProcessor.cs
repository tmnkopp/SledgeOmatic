using Microsoft.Extensions.Configuration;
using Serilog;
using SOM.Compilers;
using SOM.Parsers;
using SOM.Procedures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace SOM 
{
    public interface IParseProcessor
    {
        void Process(ISomContext somContext);
    }
    public class ParseProcessor: IParseProcessor
    {
        private readonly IDirectoryParser parser;
        private readonly IConfiguration config;
        private readonly ILogger logger;
        public ParseProcessor(
              IDirectoryParser parser
            , IConfiguration config
            , ILogger logger
            )
        {
            this.parser = parser;
            this.config = config;
            this.logger = logger;
        }
        public void Process(ISomContext somContext) {
            string basePath = config.GetSection("AppSettings:BasePath").Value;
            string configPath = (config.GetSection("AppSettings:ParseConfig").Value ?? "~").Replace("~", basePath);
            string configFile = $"{configPath}{somContext.Options.Path}.yaml".Replace(@"\\", @"\");
            logger.Information("{o}", configFile);

            string raw = File.ReadAllText(configFile);
            var deser = new DeserializerBuilder().WithNamingConvention(PascalCaseNamingConvention.Instance).Build();
            var dfd = deser.Deserialize<DirectoryParseDefinition>(raw); 
            Type ptype = (from t in this.IParserTypes where t.Name == dfd.ParseType select t).FirstOrDefault();
            var ctor_params = ptype.GetConstructors()[0].GetParameters();
            for (int i = 0; i < ctor_params.Count(); i++)
            {
                var val = Convert.ChangeType(dfd.ParseTypeArgs[i], ctor_params[i].ParameterType);
                dfd.ParseTypeArgs[i] = val;
            } 
            parser.Directories.AddRange(dfd.Directories); 
            parser.Parser = (IParser<string>)Activator.CreateInstance(ptype, dfd.ParseTypeArgs.ToArray()); 
            parser.FileFilter = dfd.FileFilter;
            parser.ResultFormat = dfd.ResultFormat;
            if (string.IsNullOrWhiteSpace(dfd.Dest)) {
                parser.ParseDirectory();
                somContext.Cache.Write(parser.ToString());
                somContext.Cache.Inspect();
            }
            else{
                parser.ParseDirectory();
                dfd.Dest = dfd.Dest.Replace("~", somContext.BasePath);
                using (StreamWriter w = File.AppendText($"{dfd.Dest}")) { }
                File.WriteAllText($"{dfd.Dest}", parser.ToString(), Encoding.Unicode); 
            }    
        }  
        private List<Type> IParserTypes{ 
            get{ 
                return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => typeof(IParser<string>).IsAssignableFrom(p) && p.IsClass).ToList();
            }
        } 
    }
}
