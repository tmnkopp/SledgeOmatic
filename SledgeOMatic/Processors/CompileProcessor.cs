using CommandLine;
using CommandLine.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SOM.Compilers;
using SOM.IO;
using SOM.Parsers;
using SOM.Procedures;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;

namespace SOM 
{
    public interface ICompileProcessor
    {
        void Process(CompileOptions o);
    }
    public class CompileProcessor : ICompileProcessor
    {
        private readonly ICompiler compiler;
        private readonly IConfiguration config;
        private readonly ILogger logger; 
        public CompileProcessor(
              ICompiler compiler
            , IConfiguration config
            , ILogger logger
        )
        {
            this.compiler = compiler;
            this.config = config;
            this.logger = logger;
        }
        public void Process(CompileOptions o) 
        {
            string configPath = config.GetSection("AppSettings:CompileConfig").Value; 
            if (!string.IsNullOrEmpty(o.Path.ToString()))  {
                string envar = Environment.GetEnvironmentVariable("som", EnvironmentVariableTarget.User).ToLower().Replace("som.exe", "");
                o.Path = o.Path.Replace("~", envar); 
                if (!o.Path.Contains(":"))
                    o.Path = $"{envar}{o.Path}";
                if (!o.Path.EndsWith(".yaml"))
                    o.Path += ".yaml";
                o.Path = o.Path.Replace(@"\\", @"\");
                configPath = o.Path; 
            }
            logger.LogInformation("{o}", configPath);
            compiler.CompileMode = o.CompileMode; 
            var yaml = new YamlStream();
            using (TextReader tr = File.OpenText(configPath))
                yaml.Load(tr);
            var root = (YamlMappingNode)yaml.Documents[0].RootNode;

            List<object> oparms;
            foreach (var rootitem in root.Children)  {
                PropertyInfo pi = compiler.GetType().GetProperty(rootitem.Key.ToString());
                if (pi != null)  { 
                    if (pi.PropertyType.FullName.Contains("List`1"))  { 
                        foreach (var propitems in ((YamlSequenceNode)rootitem.Value).Children)
                        {
                            string stype = ((YamlMappingNode)propitems).FirstOrDefault().Key.ToString();
                            oparms = GetParms((YamlMappingNode)propitems);

                            var typ = Assm().GetTypes().Where(t => t.Name == stype && typeof(ICompilable).IsAssignableFrom(t)).FirstOrDefault();
                            Type gtyp = Type.GetType($"{typ.FullName}, SOM");
                            var obj = Activator.CreateInstance(gtyp, oparms.ToArray());
                            pi.PropertyType.GetMethod("Add").Invoke(pi.GetValue(compiler), new object[] { obj });
                        }
                    }
                    else  {
                        pi.SetValue(compiler, rootitem.Value.ToString(), null);
                    }
                }
                MethodInfo[] methods = compiler.GetType().GetMethods().Where(m => m.Name.ToLower() == rootitem.Key.ToString().ToLower()).ToArray();
                if (methods.Count() > 0)
                {
                    oparms = new List<object>();
                    foreach (var parmValue in ((YamlSequenceNode)rootitem.Value).Children)
                        oparms.Add(parmValue.ToString()); 

                    foreach (MethodInfo m in methods) 
                        if (m.Name == rootitem.Key.ToString() && m.GetParameters().Count() == oparms.Count()) 
                            m.Invoke(compiler, oparms.ToArray());  
                } 
                if (Regex.IsMatch(rootitem.Key.ToString().ToLower(), $"compilation"))
                { 
                    ((YamlSequenceNode)rootitem.Value).Children.ToList().ForEach(ncomp => {
                        ((YamlMappingNode)ncomp).Children.ToList().ForEach(nprop => {
                            var propinfo = compiler.GetType().GetProperties().Where(p => p.Name == nprop.Key.ToString()).FirstOrDefault();
                            propinfo?.SetValue(compiler, nprop.Value.ToString(), null);
                        });
                        compiler.Compile();
                    }); 
                }
            } 
            if (o.CompileMode != CompileMode.Commit) Cache.Inspect(); 
        }
        private List<object> GetParms(YamlMappingNode propitems)
        {
            List<object> oparms = new List<object>();
            string stype = "";
            foreach (var prop in (YamlMappingNode)propitems)
            {
                stype = prop.Key.ToString();
                if (prop.Value.GetType() == typeof(YamlSequenceNode))
                {
                    foreach (var parm in ((YamlSequenceNode)prop.Value).Children)
                        oparms.Add(parm.ToString());
                }
                if (prop.Value.GetType() == typeof(YamlScalarNode))
                {
                    oparms.Add(prop.Value.ToString());
                }
            }
            return oparms;
        }
        private Assembly Assm() {
            Assembly assmsom = Assembly.GetExecutingAssembly();
            return assmsom;
        }
    }
}
