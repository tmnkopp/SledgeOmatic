using CommandLine;
using CommandLine.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SOM.Compilers;
using SOM.Parsers;
using SOM.Procedures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace SOM 
{
    public interface IParseProcessor
    {
        void Process(ParseOptions o);
    }
    public class ParseProcessor: IParseProcessor
    {
        private readonly ICompiler compiler;
        private readonly IConfiguration config;
        private readonly ILogger logger;
        public ParseProcessor(
              ICompiler compiler
            , IConfiguration config
            , ILogger logger
            )
        {
            this.compiler = compiler;
            this.config = config;
            this.logger = logger;
        }
        public void Process(ParseOptions o) {
            Console.Clear(); 
            string configPath = config.GetSection("AppSettings:ParseConfig").Value;
            if (!string.IsNullOrEmpty(o.ConfigFile.ToString()))
            {
                string envar = Environment.GetEnvironmentVariable("som", EnvironmentVariableTarget.User).ToLower().Replace("som.exe", "");
                o.ConfigFile = o.ConfigFile.Replace("~", envar);
                if (!o.ConfigFile.Contains(":")) o.ConfigFile = $"{envar}{o.ConfigFile}";
                if (!o.ConfigFile.EndsWith(".yaml")) o.ConfigFile += ".yaml"; 
                configPath = o.ConfigFile.Replace(@"\\", @"\");
            }
            logger.LogInformation("{o}", configPath);

            string raw = File.ReadAllText(configPath);
            var deser = new DeserializerBuilder().WithNamingConvention(PascalCaseNamingConvention.Instance).Build();
            var dfd = deser.Deserialize<DirectoryParseDefinition>(raw);
            // ParseMode pm = (ParseMode)Enum.Parse(typeof(ParseMode), dfd.ParseMode);
            Type ptype = (from t in types() where t.Name == dfd.ParseType select t).FirstOrDefault();
            Type type = Type.GetType($"{ptype.FullName}, {ptype.FullName.Split('.')[0]}");
  

            DirectoryParser parser = new DirectoryParser();
            parser.Directories.AddRange(dfd.Directories); 
            parser.Parser = (IParser<string>)Activator.CreateInstance(type, dfd.ParseTypeArgs.ToArray());
            parser.Parser.ParseMode = o.ParseMode;
            parser.FileFilter = dfd.FileFilter;
            parser.Inspect();


            //  var cnt = 0;
            //  if (o.Dir == "")
            //  {
            //      Console.Write($"{string.Join("", from d in dirs() let c = cnt++ select $"\n({c}) {d}")}\ndir:");
            //      o.Dir = dirs().ElementAt(Convert.ToInt32(Console.ReadLine()));
            //  }
            //  
            //  if (!o.Dir.Contains("*"))
            //      o.Dir += getFilter();
            //  
            //  if (o.Verbose) Console.WriteLine($"dir: {o.Dir}");
            //  if (o.ParseMode != ParseMode.Default)
            //  {
            //      cnt = 0;
            //      Console.Write($"{string.Join("", from t in Enum.GetNames(typeof(ParseMode)) let counter = cnt++ select $" | ({counter.ToString()}){t}") }\n:");
            //      o.ParseMode = ((ParseMode)Convert.ToInt32(Console.ReadLine()));
            //  }
            //  if (o.Verbose) Console.WriteLine($"{o.ParseMode}");
            //  
            //  cnt = 0;
            //  Console.Write($"{string.Join("", from t in types() let c = cnt++ select $" | ({c}) {t}")}\ntyp:");
            //  Type tparser = types().ElementAt(Convert.ToInt32(Console.ReadLine()));
            //  if (o.Verbose) Console.WriteLine($"{tparser.FullName}");
            //   
            //  Type type = Type.GetType($"{tparser.FullName}, {tparser.FullName.Split('.')[0]}");
            //  ParameterInfo[] PI = type.GetConstructors()[0].GetParameters();
            //  List<object> oparms = new List<object>();
            //  foreach (ParameterInfo parm in PI)
            //  {
            //      Console.Write($"{parm.Name} ({parm.ParameterType.Name}):");
            //      var item = Console.ReadLine();
            //      if (parm.ParameterType.Name.Contains("Int"))
            //          oparms.Add(Convert.ToInt32(item));
            //      else
            //          oparms.Add(item);
            //  }
            //  
            //  DirectoryParser parser = new DirectoryParser(o.Dir);
            //  parser.Parser = (IParser<string>)Activator.CreateInstance(type, oparms.ToArray());
            //  parser.Parser.ParseMode = o.ParseMode;
            //  parser.PathExcludePattern = o.ConfigFileExcludePattern;
            //  parser.Inspect();
        }
        private static string getFilter()
        {
            Console.WriteLine("Filter: ");
            var f = Console.ReadLine();
            return (string.IsNullOrEmpty(f)) ? "*.*" : f;
        }
        /*  utils  */
        private static List<string> dirs()
        {
            return new ConsoleSettingsProvider().Provide().Paths;
        }
        private static List<Type> types()
        {
            var type = typeof(IParser<string>);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && p.IsClass).ToList();
            return types;
        } 
    }
}
