using SOM.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SOM.Procedures 
{
    public class ExpressionExtractor : IParser<string>
    {
        private List<string> _patterns = new List<string>();
        public ExpressionExtractor(string Patterns)
        {
            _patterns = Patterns.Split(",").ToList();
        }
        public IEnumerable<string> Parse(ISomContext somContext)
        {
            string content = somContext.Content;
            content = content.Replace("\r", "\n").Replace("\n\n", "\n");
            string[] lines = content.Split('\n');
            int linecnt = 0;
            foreach (string line in lines)
            {
                linecnt++;
                foreach (string pattern in _patterns)
                {
                    Match match = Regex.Match(line, pattern);
                    if (match.Success)
                    {
                        StringBuilder result = new StringBuilder();
                        if (somContext.Options.Mode == SomMode.Cache)
                            result.Append(line + "\n");
                        else
                            result.Append($"{line}[LN {linecnt.ToString()}]\n");
                        yield return result.ToString().TrimTrailingNewline();
                    }
                }
            }
        }
    }
}
