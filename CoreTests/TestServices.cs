using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

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
                    var path = Environment.GetEnvironmentVariable("som", EnvironmentVariableTarget.User);
                    var builder = new ConfigurationBuilder()
                        .SetBasePath(@$"{path}")
                        .AddJsonFile($"appsettings.json", optional: false);
                    _config = builder.Build();
                }

                return _config;
            }
        }
    }
}


