using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SOM;
using SOM.Compilers;
using SOM.IO; 
using SOM.Procedures;
using System;

namespace CoreTests
{ 
    [TestClass]
    public class CompilerTests
    { 
        [TestMethod]
        public void ModelCompile_Compiles()
        { 
            var som = Environment.GetEnvironmentVariable("som", EnvironmentVariableTarget.User);
            var config = new TestServices().Configuration;
            var logger = new Mock<ILogger>().Object;
            var cache = new CacheService(config, logger);
            ISomContext somContext = new SomContext(config, logger, cache);
            var compiler = new Compiler(somContext);
            compiler.Source = som;
            compiler.FileFilter = "*.som"; 
            compiler.ContentCompilers.Add(new ModelCompile("aspnet_Users", ".*")); 
            compiler.Compile();
            cache.Inspect();
            Assert.IsNotNull(compiler); 
        }
        [TestMethod] 
        public void Regex_Compiles()
        {
            var som = Environment.GetEnvironmentVariable("som", EnvironmentVariableTarget.User);
            var config = new TestServices().Configuration;
            var logger = new Mock<ILogger>().Object;
            var cache = new CacheService(config, logger);
            ISomContext somContext = new SomContext(config, logger, cache);
            var compiler = new Compiler(somContext);
            compiler.Source = som;
            compiler.FileFilter = "*.som"; 
            compiler.ContentCompilers.Add(new RegexReplacer("{\"FOO\":\"BAR\"}"));
            compiler.ContentCompilers.Add(new NumericIncrementer(1000, 2000,  @"\d{4}"));
            compiler.Compile();
            cache.Inspect();
            Assert.IsNotNull(compiler);
        }
    }
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


