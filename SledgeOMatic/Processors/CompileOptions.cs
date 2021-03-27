using CommandLine;
using CommandLine.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
using YamlDotNet.RepresentationModel;

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
            compiler.CompileMode = o.CompileMode;
            compiler.OnCompiled += (o, e) =>
            {
                Process p = new Process();
                p.StartInfo.FileName = config.GetSection("AppSettings:CodeViewer").Value;
                p.StartInfo.Arguments = $"cd {e.Dest}";
                p.Start();
            }; 
            Assembly assmsom = Assembly.GetExecutingAssembly();
            var yaml = new YamlStream();
            using (TextReader tr = File.OpenText(config.GetSection("AppSettings:CompileConfig").Value))
                yaml.Load(tr);
            var root = (YamlMappingNode)yaml.Documents[0].RootNode;

            foreach (var rootitem in root.Children)
            {
                PropertyInfo pi = compiler.GetType().GetProperty(rootitem.Key.ToString());
                if (pi != null)
                { 
                    if (pi.PropertyType.FullName.Contains("List`1"))
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
                    mi.Invoke(compiler, new object[] { });
                }
            } 
            if (o.CompileMode != CompileMode.Commit) Cache.Inspect(); 
        } 
    }
}
