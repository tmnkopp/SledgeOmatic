﻿ using CommandLine;
using CommandLine.Text;
using Microsoft.Extensions.Configuration;
using Serilog;
using Newtonsoft.Json;
using SOM.Compilers;
using SOM.IO;
using SOM.Parsers;
using SOM.Procedures;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using SOM.Core;
using System.Reflection.Metadata.Ecma335;

namespace SOM 
{
    public interface ICompileProcessor
    {
        void Process(ISomContext somContext);
    }
    public class CompileProcessor : ICompileProcessor
    {
        private readonly ICompiler compiler;
        private readonly ISomContext somContext;
        private IConfiguration config;
        private ILogger logger;  
        public CompileProcessor(
              ICompiler compiler
            , ISomContext somContext
        )
        {
            this.somContext = somContext;
            this.compiler = compiler;
            this.config = this.somContext.Config;
            this.logger = this.somContext.Logger; 
        }
        public void Process(ISomContext somContext) 
        {
            string basePath = config.GetSection("AppSettings:BasePath").Value;
            string configFile = $"{basePath}{somContext.Options.Path.Replace(@".yaml", @"")}.yaml".Replace(@"\\", @"\");
     
            logger.Information("{o}", configFile); 
             
            string raw = File.ReadAllText(configFile);
            var deser = new DeserializerBuilder().WithNamingConvention(PascalCaseNamingConvention.Instance).Build();
            var def = deser.Deserialize<CompileDefinition>(raw);
 
            def.ContentCompilers.Where(c => c.Skip?.Trim() != "true").ToList().ForEach(c =>
            { 
                var typ = AssmTypes().Where(t => t.Name == c.CompilerType && typeof(ICompilable).IsAssignableFrom(t)).FirstOrDefault();
                ICompilable obj = GenericFactory<ICompilable>.Create(typ.FullName, c.Params);
                compiler.ContentCompilers.Add(obj); 
            });
            def.FilenameCompilers.Where(c => c.Skip?.Trim() != "true").ToList().ForEach(c =>
            {
                var typ = AssmTypes().Where(t => t.Name == c.CompilerType && typeof(ICompilable).IsAssignableFrom(t)).FirstOrDefault();
                ICompilable obj = GenericFactory<ICompilable>.Create(typ.FullName, c.Params);
                compiler.FilenameCompilers.Add(obj);
            }); 
            def.Compilations.ForEach(c =>
            { 
                compiler.FileFilter = c.FileFilter;
                compiler.Source = (c.Source ?? "~").Replace("~", somContext.BasePath);
                compiler.Dest = (c.Dest ?? "~").Replace("~", somContext.BasePath); 
                compiler.Compile(); 
            });
            if (somContext.Options.Mode != SomMode.Commit) somContext.Cache.Inspect();
           
        } 
        private Type[] AssmTypes() {
            Assembly assm = Assembly.GetExecutingAssembly();
            return assm.GetTypes();
        }
    }
}
