using CommandLine;
using CommandLine.Text;
using Microsoft.Extensions.Configuration;
using Serilog;
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
        private readonly IConfiguration config;
        private readonly ILogger logger;
        public ConfigProcessor( 
             IConfiguration config
            , ILogger logger
            )
        { 
            this.config = config;
            this.logger = logger;
        }
        public void Process(ConfigOptions o) {
         
            string basepath = Environment.GetEnvironmentVariable("som", EnvironmentVariableTarget.User);

            logger.Information($"basepath: {basepath}");
            logger.Information($"Bootstrap: {o.Bootstrap}");
            logger.Information($"Args: {o.Args}");
            logger.Information($"Path: {o.Path}");

            var splitArgs = o.Args?.Split(new[] { "/p" }, StringSplitOptions.None);
            logger.Information($"\n{string.Join("\n", from t in splitArgs where !string.IsNullOrWhiteSpace(t) select t)}");
             
            var AppSettings = config.GetSection("AppSettings").GetChildren();
            foreach (var item in AppSettings) logger.Information("{k} {v}", item.Key, item.Value);

            if (o.Bootstrap) SomBootstrapper.Run(o); 
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
