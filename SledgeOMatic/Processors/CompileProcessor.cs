 using CommandLine;
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
            string configPath = config.GetSection("AppSettings:CompileConfig").Value ?? "~";
            string basePath = config.GetSection("AppSettings:BasePath").Value;
            string configFile = somContext.Options.Path;
            if (!string.IsNullOrEmpty(configFile.ToString()))  {
                configPath = configPath.Replace("~", basePath); 
                if (!configFile.Contains(":")) configFile = $"{configPath}{configFile}";
                configFile = configFile.Replace(@"\\", @"\");
                configFile = (configFile.Contains(".yaml")) ? configFile : $"{configFile}.yaml"; 
            }
            logger.Information("{o}", configFile); 
             
            string raw = File.ReadAllText(configFile);
            var deser = new DeserializerBuilder().WithNamingConvention(PascalCaseNamingConvention.Instance).Build();
            var def = deser.Deserialize<CompileDefinition>(raw);
 
            def.ContentCompilers.ForEach(c =>
            {
                var typ = AssmTypes().Where(t => t.Name == c.CompilerType && typeof(ICompilable).IsAssignableFrom(t)).FirstOrDefault(); 
                ICompilable obj = (ICompilable)Activator.CreateInstance(typ, c.Args.ToArray());
                compiler.ContentCompilers.Add(obj); 
            });
            def.FilenameCompilers.ForEach(c =>
            {
                var typ = AssmTypes().Where(t => t.Name == c.CompilerType && typeof(ICompilable).IsAssignableFrom(t)).FirstOrDefault();
                ICompilable obj = (ICompilable)Activator.CreateInstance(typ, c.Args.ToArray());
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
