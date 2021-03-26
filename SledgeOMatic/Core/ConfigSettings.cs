using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOM 
{
    public interface IAppSettings
    {
        string BasePath { get; }
        string DestDir { get; }
        string SourceDir { get; }
    } 
    public class ConfigSettings : IAppSettings
    {
        private readonly IConfiguration config;
        public ConfigSettings(IConfiguration config)
        {
            this.config = config;
        }
        public string BasePath => config.GetSection("AppSettings")["BasePath"];
        public string SourceDir => config.GetSection("AppSettings")["SourceDir"];
        public string DestDir => config.GetSection("AppSettings")["DestDir"];
    }
}
