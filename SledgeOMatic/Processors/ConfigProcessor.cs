using CommandLine;
using CommandLine.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SOM.Compilers; 
using SOM.Procedures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SOM 
{
    public interface IConfigProcessor
    {
        void Process(ConfigOptions o);
    }
    public class ConfigProcessor: IConfigProcessor
    {
        private readonly ICompiler compiler;
        private readonly IConfiguration config;
        private readonly ILogger logger;
        public ConfigProcessor(
              ICompiler compiler
            , IConfiguration config
            , ILogger logger
            )
        {
            this.compiler = compiler;
            this.config = config;
            this.logger = logger;
        }
        public void Process(ConfigOptions o) {
            Console.Clear();
            var cnt = 0;
            Console.Write($"{string.Join("\n", from t in types() let c = cnt++ select $"({c}) {t}")}\n");
            logger.LogInformation($"{o}", config.GetSection("AppSettings:CompileConfig").Value); 
        }  
        private static List<Type> types()
        { 
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => typeof(ICompilable).IsAssignableFrom(p) && p.IsClass).ToList();
            return types;
        } 
    }
}
