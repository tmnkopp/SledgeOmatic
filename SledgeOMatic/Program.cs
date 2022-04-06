using System;
using Microsoft.Extensions.Configuration; 
using CommandLine; 
using Microsoft.Extensions.DependencyInjection; 
using Microsoft.Extensions.Logging; 
using Newtonsoft.Json;
using SOM.Compilers;
using Serilog;
using System.IO;
using SOM.Parsers;
using SOM.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace SOM
{
    class Program
    { 
        static void Main(string[] args)
        { 
            ServiceProvider serviceProvider = RegisterServices(args);
            IConfiguration config = serviceProvider.GetService<IConfiguration>();
            Serilog.ILogger logger = serviceProvider.GetService<Serilog.ILogger>();
            ISomContext somContext = serviceProvider.GetService<ISomContext>();  
            IParseProcessor parseProcessor = serviceProvider.GetService<IParseProcessor>();
            ICompileProcessor compileProcessor = serviceProvider.GetService<ICompileProcessor>();
            IConfigProcessor configProcessor = serviceProvider.GetService<IConfigProcessor>();
  
            var exit = Parser.Default.ParseArguments<CompileOptions, ParseOptions, ConfigOptions>(args)
                .MapResult(
                (CompileOptions o) => { 
                    logger.Information("{o}", JsonConvert.SerializeObject(o)); 
                    compileProcessor.Process(o);
                    return 0; 
                },
                (ParseOptions o) => { 
                    logger.Information("{o}", JsonConvert.SerializeObject(o));
                    parseProcessor.Process(o);
                    return 0; 
                },
                (ConfigOptions o) => {
                    logger.Information("{o}", JsonConvert.SerializeObject(o));
                    configProcessor.Process(o);
                    return 0;
                },
                errs => 1);
        }
        private static ServiceProvider RegisterServices(string[] args)
        {
            SomBootstrapper.Run();
            string basepath = Environment.GetEnvironmentVariable("som", EnvironmentVariableTarget.User);
             
            IConfiguration configuration = new ConfigurationBuilder()
                  .SetBasePath(basepath)
                  .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                  .AddEnvironmentVariables()
                  .AddCommandLine(args)
                  .Build();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            var services = new ServiceCollection();
            services.AddLogging(cfg => cfg.AddSerilog());
           
            Log.Information($"Information: {basepath}"); 
            services.AddSingleton<Serilog.ILogger>(Log.Logger);
            services.AddSingleton(configuration); 
            services.AddTransient<ICompiler, Compiler>(); 
            services.AddTransient<IDirectoryParser, DirectoryParser>(); 
            services.AddSingleton<ICacheService, CacheService>(); 
            services.AddTransient<ISomContext, SomContext>(); 
            services.AddTransient<ICompileProcessor, CompileProcessor>(); 
            services.AddTransient<IParseProcessor, ParseProcessor>(); 
            services.AddTransient<IConfigProcessor, ConfigProcessor>(); 
            return services.BuildServiceProvider();
        } 
    } 
}
