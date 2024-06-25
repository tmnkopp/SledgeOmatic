using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace CoreTests
{
    public class TestServices
    {
        private IConfiguration _config;
        public TestServices()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton<IConfiguration>(Configuration);
        }
        public IConfiguration Configuration
        {
            get
            {
                if (_config == null)
                {
                    string basepath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);  // Environment.CurrentDirectory;
                    while (basepath.Contains("\\bin"))
                        basepath = Directory.GetParent(basepath).ToString();
                    basepath = basepath + @"\";
                     
                    var builder = new ConfigurationBuilder()
                        .SetBasePath(@$"{basepath}")
                        .AddJsonFile($"appsettings.json", optional: false);
                    _config = builder.Build();
                }

                return _config;
            }
        }
    }
}


