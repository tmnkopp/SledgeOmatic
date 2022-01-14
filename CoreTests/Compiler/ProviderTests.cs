using CommandLine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
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
            var serializer = new SerializerBuilder() 
                .Build();

            var yaml = serializer.Serialize(lst);
            System.Console.WriteLine(yaml); 
            var deserializer = new DeserializerBuilder() 
                .Build();
    
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


