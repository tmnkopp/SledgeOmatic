using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SOM.Procedures
{
    public class SomDocParser : BaseParser, IParser<CommandParseResult>
    {
        private int indent = 0;
        public SomDocParser( int Indent )
        {
            this.indent = Indent; 
        }
        public IEnumerable<CommandParseResult> Parse(string content)
        {
            StringBuilder sb = new StringBuilder();
            content = $"\n{content}\n"; 
 
            string indent = @"^ {" + this.indent + "}";
            string pattern = @"som!(\w+) ?(.+)\n";
            string regex = $"{indent}{pattern}";
            var mc = Regex.Matches(content, regex, RegexOptions.Multiline);
            foreach (Match prefix in mc)
            {
                if (prefix?.Groups.Count > 0)
                {
                    var CommandName = prefix.Groups[1].Value; 
                    string matchedContent = content.Substring(prefix.Index, content.Length - prefix.Index);
                    string postRegex = indent + CommandName + "!som.*";
                    Match postfix = Regex.Match(matchedContent, postRegex, RegexOptions.Multiline);
                    string RawParsed = matchedContent.Substring(0, postfix.Index + postfix.Length);

                    var cpr = new CommandParseResult();
                    cpr.Parsed = RawParsed;
                    cpr.RawOptions = prefix.Groups[2].Value;
                    cpr.CommandName = CommandName;
                    cpr.CommandType = (from c in _compilables where c.Name == CommandName select c).FirstOrDefault();
                    cpr.Prefix = prefix;
                    cpr.Postfix = postfix;
                    yield return cpr;
                }
            }
           
        }
        private List<Type> _compilables { get; set; } = CompilableProvider();
        public static List<Type> CompilableProvider()
        {
            List<Type> compilables = (from assm in AppDomain.CurrentDomain.GetAssemblies()
                                      where assm.FullName.Contains(AppDomain.CurrentDomain.FriendlyName)
                                      from t in assm.GetTypes()
                                      where typeof(ICompilable).IsAssignableFrom(t) && t.IsClass 
                                      select t).ToList();

            compilables = (from comp in compilables
                           from ctor in comp.GetConstructors()
                           let attr = (CompilableCtorMeta[])ctor.GetCustomAttributes(typeof(CompilableCtorMeta), false)
                           from a in attr
                           where a.Invokable
                           select comp
                           ).ToList();
            return compilables;
        }
    }
}
