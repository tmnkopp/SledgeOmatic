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
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
namespace SOM
{
    class Program
    { 
        static void Main(string[] args)
        { 
            ServiceProvider serviceProvider = RegisterServices(args);
            IConfiguration config = serviceProvider.GetService<IConfiguration>();
            Serilog.ILogger logger = serviceProvider.GetService<Serilog.ILogger>();
            ISomContext somContext;//= serviceProvider.GetService<ISomContext>();
            IParseProcessor parseProcessor;//= serviceProvider.GetService<IParseProcessor>();
            ICompileProcessor compileProcessor;// = serviceProvider.GetService<ICompileProcessor>();
            IConfigProcessor configProcessor = serviceProvider.GetService<IConfigProcessor>();
   
            var exit = Parser.Default.ParseArguments<CompileOptions, ParseOptions, ConfigOptions>(args)
                .MapResult(
                (CompileOptions o) => { 
                    logger.Information("{o}", JsonConvert.SerializeObject(o));
                    somContext = serviceProvider.GetService<ISomContext>();
                    somContext.Options = o;
                    compileProcessor = serviceProvider.GetService<ICompileProcessor>();
                    compileProcessor.Process(somContext);
                    return 0; 
                },
                (ParseOptions o) => { 
                    logger.Information("{o}", JsonConvert.SerializeObject(o)); 
                    somContext = serviceProvider.GetService<ISomContext>();
                    somContext.Options = o;
                    parseProcessor = serviceProvider.GetService<IParseProcessor>();
                    parseProcessor.Process(somContext);
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

            string basepath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);  // Environment.CurrentDirectory;
            while (basepath.Contains("\\bin"))
                basepath = Directory.GetParent(basepath).ToString(); 
            basepath = basepath + @"\";
             
            IConfiguration configuration = new ConfigurationBuilder()
                  .SetBasePath(basepath)
                  .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                  .AddEnvironmentVariables()
                  .AddCommandLine(args)
                  .Build();

            if (string.IsNullOrWhiteSpace(configuration.GetSection("AppSettings:BasePath").Value))
            {
                configuration.GetSection("AppSettings:BasePath").Value = basepath;
            }

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
            services.AddSingleton<ISomContext, SomContext>(); 
            services.AddTransient<ICompileProcessor, CompileProcessor>(); 
            services.AddTransient<IParseProcessor, ParseProcessor>(); 
            services.AddTransient<IConfigProcessor, ConfigProcessor>(); 
            return services.BuildServiceProvider();
        } 
    } 
}
