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
        public void YamlProvider_Provides()
        {
            Cache.Write("");
            Compiler compiler = new Compiler();
            compiler.CompileMode = CompileMode.Cache;

            Assembly assmsom = Assembly.LoadFrom(@"C:\Users\Tim\source\repos\SledgeOMatic\SledgeOMatic\obj\Debug\netcoreapp3.1\SOM.dll");
            var yaml = new YamlStream();
            using (TextReader tr = File.OpenText(@"C:\Users\Tim\source\repos\SledgeOMatic\CoreTests\unittest.yaml"))
                yaml.Load(tr);
            var root = (YamlMappingNode)yaml.Documents[0].RootNode; 

            foreach (var rootitem in root.Children)
            { 
                PropertyInfo pi = compiler.GetType().GetProperty(rootitem.Key.ToString());
                if (pi != null) {
                    if (pi.PropertyType.FullName.Contains("List"))
                    {
                        foreach (var propitems in ((YamlSequenceNode)rootitem.Value).Children)
                        {
                            List<object> oparms = new List<object>();
                            string stype = "";
                            foreach (var prop in (YamlMappingNode)propitems)
                            {
                                stype = prop.Key.ToString();
                                foreach (var parm in ((YamlSequenceNode)prop.Value).Children)
                                {
                                    oparms.Add(parm.ToString());
                                }
                            }
                            var typ = assmsom.GetTypes().Where(t => t.Name == stype && typeof(ICompilable).IsAssignableFrom(t)).FirstOrDefault();
                            Type gtyp = Type.GetType($"{typ.FullName}, SOM");
                            var obj = Activator.CreateInstance(gtyp, oparms.ToArray());
                            pi.PropertyType.GetMethod("Add").Invoke(pi.GetValue(compiler), new object[] { obj });
                        }
                    }
                    else
                    {
                        pi.SetValue(compiler, rootitem.Value.ToString(), null);
                    }
                }
                MethodInfo mi = compiler.GetType().GetMethod(rootitem.Key.ToString());
                if (mi != null)
                {
                    mi.Invoke(compiler, new object[] {  });
                } 
            }
            //compiler.Compile();
            Cache.Inspect();  
        }
        [TestMethod]
        public void typeProvider_Provides()
        {
            ISchemaProvider schema = new SchemaProvider("aspnet_Membership");
            var ssm = new SomSchemaInterpreter(schema)
            {
                SchemaItemPredicate = app => true,
                SchemaItemProjector = (app) =>
                {
                    app.StringFormatter = (i, f) => f.Replace("{0}", i.Name).Replace("{1}", i.DataType);
                    app.DataType = Regex.Replace(app.DataType, "(.*unique.*)", "int");
                    return app;
                }
            };

            var t = Assm.GetTypes().Where(t => t.Name.Contains("SomSchemaInterpreter") && typeof(ICompilable).IsAssignableFrom(t))
                .FirstOrDefault() ;

            var type = Type.GetType($"{t.FullName}, {t.Namespace}");
            Assert.IsNotNull(type);
        }

        [TestMethod]
        public void yamlProvider_Provides()
        {
            var yml = @"    
  FileFilter: 'fileFilter'
  Source: 'src'
  Dest: 'dest'
            ";
            
            var cc = new CompilationConfig() { FileFilter = "ff", Source = "src", Dest="dest" };
            var c = new Compile() { CompilationConfig = cc };
            var serializer = new SerializerBuilder() 
                .Build();

            var yaml = serializer.Serialize(c);
            System.Console.WriteLine(yaml); 
            var deserializer = new DeserializerBuilder() 
                .Build();
    
            var t = deserializer.Deserialize<CompilationConfig>(yml);
            Assert.IsNotNull(t);
        }
    }
    [Serializable]
    public class Compile { 
        public CompilationConfig CompilationConfig { get; set; }
    }
    [Serializable]
    public class CompilationConfig { 
        public string FileFilter { get; set; }
        public string Source { get; set; }
        public string Dest { get; set; }
    }
}


