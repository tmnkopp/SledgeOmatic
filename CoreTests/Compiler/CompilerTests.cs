using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SOM;
using SOM.Compilers; 
using SOM.Data;
using SOM.Extentions;
using SOM.IO;
using SOM.Models;
using SOM.Procedures;
using SOMAPI.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
namespace CoreTests
{ 
    [TestClass]
    public class CompilerTests
    { 
        [TestMethod]
        public void ModelCompile_Compiles()
        {
            Cache.Write("");
            var config = new TestServices().Configuration;
            var mock = new Mock<ILogger<CompilerTests>>();
            ISomContext somContext = new SomContext(config, mock.Object);
            somContext.Content = "\nPREFIX\n{0} : {0}\nPOSTFIX\n";
            ModelCompile compiler = new ModelCompile("aspnet_Users", ".*");
            string content = compiler.Compile(somContext);
            Cache.WriteLine(content);
            Cache.Inspect();
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
                    var builder = new ConfigurationBuilder()
                        .SetBasePath(@"C:\Users\Tim\source\repos\SledgeOMatic\SledgeOMatic\")
                        .AddJsonFile($"appsettings.json", optional: false);
                    _config = builder.Build();
                }

                return _config;
            }
        }
    }
}


