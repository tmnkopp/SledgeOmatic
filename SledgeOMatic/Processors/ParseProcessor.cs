﻿using Microsoft.Extensions.Configuration;
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
            string configFile = $"{configPath}{o.ConfigFile}.yaml".Replace(@"\\", @"\");
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
    
            DirectoryParser parser = new DirectoryParser();
            parser.Directories.AddRange(dfd.Directories); 
            parser.Parser = (IParser<string>)Activator.CreateInstance(ptype, dfd.ParseTypeArgs.ToArray());
            parser.Parser.ParseMode = o.ParseMode;
            parser.FileFilter = dfd.FileFilter;
            parser.ResultFormat = dfd.ResultFormat;
            if (string.IsNullOrWhiteSpace(dfd.Dest)) {
                parser.Inspect(); 
            }else{ 
                dfd.Dest = dfd.Dest.Replace("~", configPath);
                parser.ParseToFile(dfd.Dest);
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
