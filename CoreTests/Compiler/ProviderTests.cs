 
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Serilog;
using SOM; 
using SOM.IO; 
using SOM.Procedures; 
using System;
using System.Collections.Generic; 
using System.IO;
using System.Linq; 
using YamlDotNet.Serialization; 
namespace CoreTests
{
    public static class Assm {
        public static IEnumerable<Type> GetTypes()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(assm => assm.GetTypes());
        }
    } 
    [TestClass]
    public class ProviderTests
    {

        [TestMethod]
        public void KeyValReplacer_Provides()
        {
            string readall = @"
                1-1-1
                1-2-1.1
            ";
            var config = new TestServices().Configuration;
            var logger = new Mock<ILogger>().Object;
            var cache = new CacheService(config, logger);
            ISomContext somContext = new SomContext(config, logger, cache) { Content = readall };
            var obj = new KeyValReplacer(@"C:\Users\Tim\Documents\SQL Server Management Studio\SQLQuery1.sql");
            somContext.Content = obj.Compile(somContext); 
            Assert.IsNotNull(somContext.Content);
    }

    [TestMethod]
        public void yamlProvider_Provides()
        {
            var yml = @"    
    - FileFilter: 'fileFilter'
      Source: 'src'
      Dest: 'dest'
    - FileFilter: 'fileFilter'
      Source: 'src'
      Dest: 'dest'
            ";
            List<CompilationConfig> lst = new List<CompilationConfig>();
            var cc = new CompilationConfig() { FileFilter = "ff", Source = "src", Dest="dest" };
            lst.Add(cc);
            var serializer = new SerializerBuilder().Build();

            var yaml = serializer.Serialize(lst);
            System.Console.WriteLine(yaml); 
            var deserializer = new DeserializerBuilder().Build(); 
            var t = deserializer.Deserialize<List<CompilationConfig>>(yml);
            string s = t[1].Dest;
            Assert.AreEqual("dest", s);
        }
    } 
    [Serializable]
    public class CompilationConfig { 
        public string FileFilter { get; set; }
        public string Source { get; set; }
        public string Dest { get; set; }
    }
}


