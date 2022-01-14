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
            var compilables = (from assm in AppDomain.CurrentDomain.GetAssemblies()
                                         where assm.FullName.Contains(AppDomain.CurrentDomain.FriendlyName)
                                         from t in assm.GetTypes()
                                         where typeof(ICompilable).IsAssignableFrom(t) && t.IsClass 
                                         select t).ToList();
            compilables.ForEach(c =>
            {
                IParser<CommandParseResult> _Parser = new SomTagParser(c.Name);
                foreach (var parseresult in _Parser.Parse(content))
                {
                    var args = parseresult.Options<SomParseArguments>().Args;
                    var oparms = (from a in args select a.ToString().Trim()).ToList<object>();
    
                    Type typ = Type.GetType($"{c.FullName}, SOM");
                    int cnt = 0;
                    typ.GetConstructors()
                        .Where(c=> c.GetParameters().Count() == oparms.Count())
                        .FirstOrDefault()
                        .GetParameters()
                        .ToList()
                        .ForEach(parm => {
                            oparms[cnt] = Convert.ChangeType(oparms[cnt], parm.ParameterType);
                            cnt++;
                    });

                    content = content.Replace(parseresult.RawOptions, "~RawOptions~");
                    ICompilable obj = (ICompilable)Activator.CreateInstance(typ, oparms.ToArray());
                    content = obj.Compile(content);
                    content = content.Replace("~RawOptions~", parseresult.RawOptions);
                }
            }); 
            return content; 
        } 
    }
}
