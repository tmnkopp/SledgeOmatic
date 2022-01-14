using SOM.Data;
using SOM.Extentions;
using SOM.IO;
using SOM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
namespace SOM.Procedures
{ 
    public class SomTagInterpreter : ICompilable
    { 
        public SomTagInterpreter()
        { 
        } 
        public string Compile(string content)
        {
            StringBuilder sb = new StringBuilder();
            var compilables = (from assm in AppDomain.CurrentDomain.GetAssemblies()
                                where assm.FullName.Contains(AppDomain.CurrentDomain.FriendlyName)
                                from t in assm.GetTypes()
                                where typeof(ICompilable).IsAssignableFrom(t) && t.IsClass 
                                select t).ToList();

            compilables.ForEach(c =>
            {
                Type typ = Type.GetType($"{c.FullName}, SOM");
                IParser<CommandParseResult> _Parser = new SomTagParser(c.Name);
                var parsed = _Parser.Parse(content);
                foreach (var parseresult in parsed)
                {
                    var parseItem = parseresult.Parsed;
                    var opts = parseresult.Options<SomParseArguments>();
                    var oparms = (from a in opts.Args select a.ToString().Trim()).ToList<object>();
                    
                    var parms = typ.GetConstructors()
                        .Where(c => c.GetParameters().Count() == oparms.Count())
                        .FirstOrDefault().GetParameters().ToList();

                    for (int i = 0; i < parms.Count(); i++) 
                        oparms[i] = Convert.ChangeType(oparms[i], parms[i].ParameterType);

                    parseItem = parseItem.Replace(parseresult.RawOptions, "~RawOptions~");
                    ICompilable obj = (ICompilable)Activator.CreateInstance(typ, oparms.ToArray());
                    parseItem = obj.Compile(parseItem);
                    parseItem = parseItem.Replace("~RawOptions~", parseresult.RawOptions);
                    if (!opts.Verbose)
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
