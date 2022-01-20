using CommandLine;
using CommandLine.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
        void Process(ParseOptions o);
    }
    public class ParseProcessor: IParseProcessor
    {
        private readonly ICompiler compiler;
        private readonly IConfiguration config;
        private readonly ILogger logger;
        public ParseProcessor(
              ICompiler compiler
            , IConfiguration config
            , ILogger logger
            )
        {
            this.compiler = compiler;
            this.config = config;
            this.logger = logger;
        }
        public void Process(ParseOptions o) {
            Console.Clear(); 
            string configPath = config.GetSection("AppSettings:ParseConfig").Value;
            if (!string.IsNullOrEmpty(o.Path.ToString()))
            {
                string envar = Environment.GetEnvironmentVariable("som", EnvironmentVariableTarget.User).ToLower().Replace("som.exe", "");
                o.Path = o.Path.Replace("~", envar);
                if (!o.Path.Contains(":")) o.Path = $"{envar}{o.Path}";
                if (!o.Path.EndsWith(".yaml")) o.Path += ".yaml"; 
                configPath = o.Path.Replace(@"\\", @"\");
            }
            logger.LogInformation("{o}", configPath);

            string raw = File.ReadAllText(configPath);
            var deser = new DeserializerBuilder().WithNamingConvention(PascalCaseNamingConvention.Instance).Build();
            var dfd = deser.Deserialize<DirectoryParseDefinition>(raw); 
            Type ptype = (from t in types() where t.Name == dfd.ParseType select t).FirstOrDefault();
            Type type = Type.GetType($"{ptype.FullName}, {ptype.FullName.Split('.')[0]}");
   
            DirectoryParser parser = new DirectoryParser();
            parser.Directories.AddRange(dfd.Directories); 
            parser.Parser = (IParser<string>)Activator.CreateInstance(type, dfd.ParseTypeArgs.ToArray());
            parser.Parser.ParseMode = o.ParseMode;
            parser.FileFilter = dfd.FileFilter;
            parser.Inspect(); 
             
        }
        private static string getFilter()
        {
            Console.WriteLine("Filter: ");
            var f = Console.ReadLine();
            return (string.IsNullOrEmpty(f)) ? "*.*" : f;
        }
        /*  utils  */
        private static List<string> dirs()
        {
            return new ConsoleSettingsProvider().Provide().Paths;
        }
        private static List<Type> types()
        {
            var type = typeof(IParser<string>);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && p.IsClass).ToList();
            return types;
        } 
    }
}
