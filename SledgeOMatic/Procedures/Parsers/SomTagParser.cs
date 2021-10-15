using CommandLine;
using SOM.Data;
using SOM.IO;
using SOM.Procedures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SOM.Procedures
{
    public enum ParseTag { 
        SCHEMA
    }
    public class SomTagParser : BaseParser, IParser<CommandParseResult> 
    {  
        private string _parsetag = "";
        public SomTagParser(ParseTag ParseTag) {
            _parsetag = ParseTag.ToString().ToLower();
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
                yield return new CommandParseResult(result , prefix.Groups[1].Value);
            }  
        } 
    }
}
