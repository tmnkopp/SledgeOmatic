using SOM.Data;
using SOM.Extentions;
using SOM.IO;
using SOM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace SOM.Procedures
{ 
    public class SomTagInterpreter : ICompilable
    { 
        private string _ICompilablePattern = "";
        public SomTagInterpreter(string ICompilablePattern)
        {
            _ICompilablePattern = (string.IsNullOrEmpty(ICompilablePattern)) ? ".*" : _ICompilablePattern;
        } 
        public string Compile(string content)
        {
            StringBuilder sb = new StringBuilder();
            List<Type> compilables = (from assm in AppDomain.CurrentDomain.GetAssemblies()
                                where assm.FullName.Contains(AppDomain.CurrentDomain.FriendlyName)
                                from t in assm.GetTypes()
                                where typeof(ICompilable).IsAssignableFrom(t) && t.IsClass 
                                && Regex.IsMatch(t.Name, this._ICompilablePattern)
                                select t).ToList();

            compilables = (from comp in compilables
                           from ctor in comp.GetConstructors()
                           let attr = (CompilableCtorMeta[])ctor.GetCustomAttributes(typeof(CompilableCtorMeta), false)
                           from a in attr where a.Invokable
                           select comp
                           ).ToList();


            compilables.ForEach(c =>
            {
                Type typ = Type.GetType($"{c.FullName}, SOM");

                CompilableCtorMeta ccm = (from cons in c.GetConstructors()
                               let attr = (CompilableCtorMeta[])cons.GetCustomAttributes(typeof(CompilableCtorMeta), false)
                               from a in attr where a.Invokable
                               select a
                               ).FirstOrDefault();

                ConstructorInfo ctor = (from con in typ.GetConstructors()
                                        let attr = (CompilableCtorMeta[])con.GetCustomAttributes(typeof(CompilableCtorMeta), false)
                                        from a in attr
                                        where a.Invokable
                                        select con
                                ).FirstOrDefault();

                IParser<CommandParseResult> _Parser = new SomTagParser(c.Name);
                var parsed = _Parser.Parse(content);
                foreach (var parseresult in parsed)
                { 
                    var parseItem = parseresult.Parsed; 
                    var oparms = parseresult.Parms();
                     
                    var parms = ctor.GetParameters().ToList();

                    for (int i = 0; i < parms.Count(); i++) {
                        if (i < oparms.Count)  {
                            oparms[i] = Convert.ChangeType(oparms[i], parms[i].ParameterType);
                        }  else {
                            oparms.Add(null);
                        }
                    } 
                    parseItem = parseItem.Replace(parseresult.RawOptions, "~optionsraw~");
                    ICompilable obj = (ICompilable)Activator.CreateInstance(typ, oparms.ToArray());
                    parseItem = obj.Compile(parseItem);
                    parseItem = parseItem.Replace("~optionsraw~", parseresult.RawOptions);
                    if (parseresult.Options().Verbose)
                    {
                        parseItem = parseItem.Replace(parseresult.Prefix, "");
                        parseItem = parseItem.Replace(parseresult.Postfix, "");
                    }
                    content = content.Replace(parseresult.Parsed, parseItem); 
                } 
            });

            return content; 
        } 
    }
}
