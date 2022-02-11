using System;
using Microsoft.Extensions.Configuration;
using System.IO;
using CommandLine;
using CommandLine.Text;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;
using System.Configuration;
using Newtonsoft.Json;
using SOM.Compilers;
using System.Threading.Tasks;
using System.Threading;

namespace SOM
{
    class Program
    {  
        static void Main(string[] args)
        { 
            ServiceProvider serviceProvider = RegisterServices(args);
            IConfiguration config = serviceProvider.GetService<IConfiguration>();
            IAppSettings appSettings = serviceProvider.GetService<IAppSettings>();
            ICompiler compiler = serviceProvider.GetService<ICompiler>();
            IParseProcessor parseProcessor = serviceProvider.GetService<IParseProcessor>();
            ICompileProcessor compileProcessor = serviceProvider.GetService<ICompileProcessor>();
            IConfigProcessor configProcessor = serviceProvider.GetService<IConfigProcessor>();
            ILogger logger = serviceProvider.GetService<ILogger<Program>>();

            var exit = Parser.Default.ParseArguments<CompileOptions, ParseOptions, ConfigOptions>(args)
                .MapResult(
                (CompileOptions o) => { 
                    logger.LogInformation("{o}", JsonConvert.SerializeObject(o)); 
                    compileProcessor.Process(o);
                    return 0; 
                },
                (ParseOptions o) => { 
                    logger.LogInformation("{o}", JsonConvert.SerializeObject(o));
                    parseProcessor.Process(o);
                    return 0; 
                },
                (ConfigOptions o) => {
                    logger.LogInformation("{o}", JsonConvert.SerializeObject(o));
                    configProcessor.Process(o);
                    return 0;
                },
                errs => 1);
        }
        private static ServiceProvider RegisterServices(string[] args)
        {
            string envar = Environment.GetEnvironmentVariable("som", EnvironmentVariableTarget.User);
            if (string.IsNullOrEmpty(envar))
            {
                Environment.SetEnvironmentVariable("som", "c:\\_som\\", EnvironmentVariableTarget.User);
                envar = Environment.GetEnvironmentVariable("som", EnvironmentVariableTarget.User);
            } 
            var basepath = envar.ToLower().Replace("som.exe", "");
            Console.Write($"SetBasePath: {basepath}");
            IConfiguration configuration = new ConfigurationBuilder()
                  .SetBasePath(basepath)
                  .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                  .AddEnvironmentVariables()
                  .AddCommandLine(args)
                  .Build();
 
            var services = new ServiceCollection();
            services.AddLogging(cfg => cfg.AddConsole());
            services.AddSingleton<ILogger>(svc => svc.GetRequiredService<ILogger<Program>>());
            services.AddSingleton(configuration);
            services.AddTransient<IAppSettings, ConfigSettings>(); 
            services.AddTransient<ICompiler, Compiler>(); 
            services.AddTransient<ICompileProcessor, CompileProcessor>(); 
            services.AddTransient<IParseProcessor, ParseProcessor>(); 
            services.AddTransient<IConfigProcessor, ConfigProcessor>(); 
            return services.BuildServiceProvider();
        } 
    } 
}
