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
            for (int i = 12; i > 0; i -= 4)
            {
                IParser<CommandParseResult> _Parser = new SomDocParser(i);
                var parsed = _Parser.Parse(content);
                foreach (var pr in parsed)
                {
                    Type typ = Type.GetType($"{pr.CommandType.FullName}, SOM");
                    CompilableCtorMeta ccm = GetCompilableCtorMeta(pr.CommandType);
                    ConstructorInfo ctor = GetConstructorInfo(typ);

                    var parseItem = pr.Parsed;
                    var oparms = pr.Parms();
                    var parms = ctor.GetParameters().ToList();
                    for (int p = 0; p < parms.Count(); p++)
                    {
                        if (p < oparms.Count)
                            oparms[p] = Convert.ChangeType(oparms[p], parms[p].ParameterType);
                        else
                            oparms.Add(null);
                    }
                    ICompilable obj = (ICompilable)Activator.CreateInstance(typ, oparms.ToArray());
                    parseItem = obj.Compile(parseItem);
                    parseItem = RemoveTags(parseItem);
                    content = content.Replace(pr.Parsed, parseItem);
                }
            } 
            return content;
        }
        public string RemoveTags(string tagged) {
            StringBuilder sb = new StringBuilder();
            var lines = Regex.Split(tagged, @"[\n|\r]");
            foreach (var line in lines)
            {
                if (!Regex.IsMatch(line, @"(som!|!som)") && !string.IsNullOrWhiteSpace(line)) 
                    sb.AppendLine(line); 
            }
            return sb.ToString();
        }
        public static ConstructorInfo GetConstructorInfo(Type typ)
        {
            return (from con in typ.GetConstructors()
                    let attr = (CompilableCtorMeta[])con.GetCustomAttributes(typeof(CompilableCtorMeta), false)
                    from a in attr where a.Invokable
                    select con).FirstOrDefault();
        }
        public static CompilableCtorMeta GetCompilableCtorMeta(Type CommandType) {
            return (from cons in CommandType.GetConstructors()
                    let attr = (CompilableCtorMeta[])cons.GetCustomAttributes(typeof(CompilableCtorMeta), false)
                    from a in attr where a.Invokable
                    select a).FirstOrDefault(); 
        } 
    }
}
