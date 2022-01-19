using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using CommandLine;

namespace SOM.Procedures
{
    public class CommandParseResult { 
        public string[] Args {
            get
            {
                MatchCollection mc = Regex.Matches(RawOptions.Split("\n")[0], $@"-\w [^-]*");
                return (from m in mc 
                        select m.Value.Trim().Replace("\\n", "\n").Replace("\\t", "\t") ?? "{0}").ToArray();
            }
        } 
        public SomParseOptions Options  { 
            get
            {
                return new CommandLine
                .Parser(with => with.HelpWriter = null)
                .ParseArguments<SomParseOptions>(Args)
                .MapResult(o => o, o => default(SomParseOptions));
            } 
        }
        public List<object> Parms(ParameterInfo[] parms) {
            var oparms = new List<object>();
            SomParseOptions o = (SomParseOptions)this.Options;
            var props = o.GetType().GetProperties();
            oparms = (from a in o.ParamParsed select a.ToString().Trim()).ToList<object>();
    
            for (int p = 0; p < parms.Count(); p++)
            {
                if (p < oparms.Count)
                    oparms[p] = Convert.ChangeType(oparms[p], parms[p].ParameterType);
                else
                    oparms.Add(null);
            }

            return oparms;
        }
        public CommandParseResult()
        { 
        } 
        public string Parsed { get; set; } 
        public string CommandName { get; set; } 
        public Type CommandType { get; set; } 
        public Match Prefix { get; set; } 
        public Match Postfix { get; set; } 
        public string RawOptions { get; set; } 
    }
}
