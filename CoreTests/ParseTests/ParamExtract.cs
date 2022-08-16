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
        public void ParamExtractor_extractor()
        {
            ISomContext somContext = new SomContext(config, logger, cache);
            somContext.Content = @" 
                    -p /p:name=value /p:name1=value  --> 
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
            ISomContext somContext = new SomContext(config, logger, cache);

            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("Prop", "vale");

            ICompilable obj = new CompilableFactory().Create(typeof(Insert));
            Console.Write(new { obj });
        } 
    }
    public class CompilableFactory{
 
        private ICompilable obj;
        public CompilableFactory()
        {
        }
        public ICompilable Create(Type type)
        {
            this.obj = (ICompilable)Activator.CreateInstance(type); 
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
