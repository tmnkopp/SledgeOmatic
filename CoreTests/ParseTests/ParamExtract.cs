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
using Newtonsoft.Json;

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
        public void FactoryCreates()
        {
            ICompilable obj;
            obj = GenericFactory<ICompilable>.Create("SOM.Procedures.Insert");
            var ser = JsonConvert.SerializeObject(obj);
            Console.Write(ser);
            obj = GenericFactory<ICompilable>.Create("SOM.Procedures.Insert", @"  
                -p /p Prop /p Prop1 /p Prop2 
            ");
            ser = JsonConvert.SerializeObject(obj);
            Console.Write(ser);
            obj = GenericFactory<ICompilable>.Create("SOM.Procedures.Insert", @"   
                -p /p:Prop=value /p:Prop1=value1    
            ");
            ser = JsonConvert.SerializeObject(obj);
            Console.Write(ser);
        } 
    } 
}
