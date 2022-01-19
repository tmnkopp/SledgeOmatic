using CommandLine;
using SOM.Data;
using SOM.IO;
using SOM.Procedures;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace SOM.Procedures
{

    public class SomTagParser : BaseParser, IParser<CommandParseResult> 
    {  
        private string _parsetag = "";
        public SomTagParser(string ParseTag) {
            _parsetag = ParseTag;
        } 
        public IEnumerable<CommandParseResult> Parse(string content)
        {
            content = $"\n{content}\n";
            MatchCollection mc = Regex.Matches(content, @"\n.*som!"+ _parsetag + "\\s?(.*)", RegexOptions.IgnoreCase);
            foreach (Match prefix in mc)
            { 
                string match = content.Substring(prefix.Index, content.Length - prefix.Index);
                Match postfix = Regex.Match(match, @""+ _parsetag + "!som.*", RegexOptions.IgnoreCase);
                string result = match.Substring(0, postfix.Index + postfix.Length);
                var cpr = new CommandParseResult();
                cpr.Parsed = result;
                cpr.RawOptions = prefix.Groups[1].Value;
                cpr.Prefix = prefix;
                cpr.Postfix = postfix; 
                yield return cpr;
            }  
        } 
    }
}
