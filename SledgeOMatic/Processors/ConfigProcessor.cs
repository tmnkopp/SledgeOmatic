using CommandLine;
using CommandLine.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog;
using SOM.Compilers; 
using SOM.Procedures;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace SOM 
{
    public interface IConfigProcessor
    {
        void Process(ConfigOptions o);
    }
    public class ConfigProcessor: IConfigProcessor
    { 
        private readonly IConfiguration config;
        private readonly Serilog.ILogger logger;
        public ConfigProcessor( 
             IConfiguration config
            , Serilog.ILogger logger
            )
        { 
            this.config = config;
            this.logger = logger;
        }
        public void Process(ConfigOptions o) {
          
            logger.Information($"Bootstrap: {o.Bootstrap}"); 
            logger.Information($"PK_FORM: {o.PK_FORM}"); 

            var splitArgs = o.Args?.Split(new[] { "/p" }, StringSplitOptions.None) ?? new string[]{ };
            logger.Information($"\n{string.Join("\n", from t in splitArgs where !string.IsNullOrWhiteSpace(t) select t)}");
             
            var AppSettings = config.GetSection("AppSettings").GetChildren();
            foreach (var item in AppSettings) logger.Information("{k} {v}", item.Key, item.Value);
            StringBuilder sb = new StringBuilder();

            try
            {
                string _filePath = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory);
                _filePath = Regex.Match(_filePath, "(.*SledgeOmatic\\\\).*").Groups[1].Value + "SledgeOMatic\\Tasks\\keys.sql";
                if( !string.IsNullOrEmpty(config.GetSection("AppSettings")["ConfigSqlPath"]) ){
                    _filePath = config.GetSection("AppSettings")["ConfigSqlPath"];
                }
                string sql = System.IO.File.ReadAllText(_filePath);
                if (!string.IsNullOrEmpty(o.PK_FORM))
                {
                    sql = Regex.Replace(sql, @"\d{4}-\w{1,4}-\w{1,8}", o.PK_FORM);
                    sql = sql.Replace(@"DC_", $"_{o.PK_FORM.Replace("-", "")}_");
                }
                var provider = new DataTableProvider(config, logger);
                var dt = provider.Provide(sql);
                var ser = JsonConvert.SerializeObject(dt);
                ser = ser.Replace(",", "\n,");
                sb.AppendLine(ser);
            }
            catch (Exception ex)
            {

                logger.Error(ex, "keys.sql file error");
            }
              
            logger.Information(sb.ToString()); 
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
