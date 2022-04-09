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
        #region FIELDS

        private bool verbose = true;

        #endregion

        #region CTOR

        public SomTagInterpreter(string Options)
        {
            verbose = Options.Contains("-v");
        }

        #endregion

        #region METHODS

        public string Compile(ISomContext somContext)
        {
            string content = somContext.Content;

            for (int i = 12; i > 0; i -= 4)
            { 
                var parsed = new SomDocParser(i).Parse(somContext); 
                foreach (var pr in parsed)
                {
                    Type typ = Type.GetType($"{pr.CommandType.FullName}, SOM");
                    CompilableCtorMeta ccm = GetCompilableCtorMeta(pr.CommandType);
                    ConstructorInfo ctor = GetConstructorInfo(typ);

                    var parseItem = pr.Parsed;
                    var oparms = pr.Parms(ctor.GetParameters());

                    somContext.Logger.Information("SomTagInterpreter: Indent{i} {o} {p}", i, pr.CommandType.FullName, string.Join(", ", oparms.ToArray()));
                     
                    ICompilable obj = (ICompilable)Activator.CreateInstance(typ, oparms.ToArray());
                    somContext.Content = parseItem;
                    parseItem = obj.Compile(somContext);
                    parseItem = RemoveTags(parseItem); 
                    content = content.Replace(pr.Parsed, parseItem);
                    somContext.Content = content;
                    if (pr.Options.Verbose)
                        somContext.Cache.Write(content);
                }
            }
            return somContext.Content;
        }
        public string RemoveTags(string tagged)
        {
            StringBuilder sb = new StringBuilder();
            var lines = Regex.Split(tagged, @"[\n\r]");
            foreach (var line in lines)
            {
                if (!Regex.IsMatch(line, $@"(som!\w+|\w+!som)") && !string.IsNullOrWhiteSpace(line))
                    sb.AppendLine(line);
            }
            return sb.ToString();
        }
        public static ConstructorInfo GetConstructorInfo(Type typ)
        {
            return (from con in typ.GetConstructors()
                    let attr = (CompilableCtorMeta[])con.GetCustomAttributes(typeof(CompilableCtorMeta), false)
                    from a in attr
                    where a.Invokable
                    select con).FirstOrDefault();
        }
        public static CompilableCtorMeta GetCompilableCtorMeta(Type CommandType)
        {
            return (from cons in CommandType.GetConstructors()
                    let attr = (CompilableCtorMeta[])cons.GetCustomAttributes(typeof(CompilableCtorMeta), false)
                    from a in attr
                    where a.Invokable
                    select a).FirstOrDefault();
        }

        #endregion

    }
}
