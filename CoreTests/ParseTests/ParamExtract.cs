using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SOM.IO;
using SOM.Procedures; 
using SOM;
using Moq;
using Serilog;
using System.Linq;
using SOM.Core;
using System.Reflection;
using System.Text.RegularExpressions;

namespace CoreTests
{ 
    [TestClass]
    public class ParamExtract
    {
        string som;
        Microsoft.Extensions.Configuration.IConfiguration config;
        ILogger logger;
        ICacheService cache;
        public ParamExtract()
        {
            som = Environment.GetEnvironmentVariable("som", EnvironmentVariableTarget.User);
            config = new TestServices().Configuration;
            logger = new Mock<ILogger>().Object;
            cache = new CacheService(config, logger);
        }
        [TestMethod]
        public void ParamExtractor_extracts()
        {
            ISomContext somContext = new SomContext(config, logger, cache);
            somContext.Content = @" 
                    -p /p:Prop=value /p:Prop1=value1   
            ";
            somContext.Content = @" 
                    -p /p Prop /p Prop1 
            ";
            var parser = new ParamExtractor();
            var parms = (from kvp in parser.Parse(somContext) select kvp).ToDictionary(i=>i.Key, i=>i.Value); 
            foreach (var kvp in parms)
            {
                var item = kvp;
            } 
        }
        [TestMethod]
        public void Field_extractor()
        {
            ICompilable obj;
            obj = new CompilableFactory().Create(typeof(Insert), " -p /p Prop /p Prop1 ");
            Console.Write(new { obj });
            obj = new CompilableFactory().Create(typeof(Insert), "  -p /p:Prop=value /p:Prop1=value1   ");
            Console.Write(new { obj });
        } 
    }
    public class CompilableFactory{
 
        private ICompilable obj;
        public CompilableFactory()
        {
        }
        public ICompilable Create(Type type, string paramString)
        {
            paramString = paramString.Split("\n")[0];
            var m = Regex.Match(paramString, $"(/p:.*)\n");
            if (m.Success)
            { 
                var props = new Dictionary<string, string>();
                foreach (var item in Regex.Split(m.Groups[1].Value, $"/p:"))
                {
                    if (item.Contains("="))
                        props.Add(item.Split("=")[0], item.Split("=")[1]); 
                }

                this.obj = (ICompilable)Activator.CreateInstance(type);

                (from p in obj.GetType().GetProperties()
                 where props.ContainsKey(p.Name)
                 select p).ToList().ForEach(p => {
                     p.SetValue(obj, props[p.Name], null);
                 });
            }

            m = Regex.Match(paramString, $@"(/p\s.*)\n");
            if (m.Success)
            {
                var oparms = new List<object>();
                foreach (var item in Regex.Split(m.Groups[1].Value, $"/p\\s"))
                {
                    if (!string.IsNullOrWhiteSpace(item))
                        oparms.Add(item);
                }
                this.obj = (ICompilable)Activator.CreateInstance(type, oparms.ToArray());
            }
            return obj;
        }
        public ICompilable Create(Type type, ParameterInfo[] oparms)
        {
            this.obj = (ICompilable)Activator.CreateInstance(type, oparms.ToArray());
            return obj;
        }
        public ICompilable Create(Type type, Dictionary<string, string> props)
        { 
            this.obj = (ICompilable)Activator.CreateInstance(type);
            (from p in obj.GetType().GetProperties() 
             where props.ContainsKey(p.Name) select p).ToList().ForEach(p=>{
                p.SetValue(obj, props[p.Name], null);
            }); 
            return obj;
        } 
    }
}
