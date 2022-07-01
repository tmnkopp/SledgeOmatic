using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SOM.Extentions;
using SOM.Parsers;
using SOM.Procedures ;

namespace SOM.Procedures
{
    public class LineExtractor : IParser<string>
    {
        private string _extractTarget;
        private int _numberOfLines = 4; 
        
        public LineExtractor(string ExtractTarget, int NumberOfLines )
        {
            _extractTarget = ExtractTarget;
            _numberOfLines = NumberOfLines; 
        }  
        public IEnumerable<string> Parse(ISomContext somContext){ 
            string content = somContext.Content;
            content = content.Replace("\r", "\n");
            content = content.Replace("\n\n", "\n");
            content = $"{new string('\n', _numberOfLines)}{content}{new string('\n', _numberOfLines)}";
            Match match = Regex.Match(content, _extractTarget);
            if(!match.Success)
                yield return "";

            string[] lines = content.Split('\n'); 
            for (int i = _numberOfLines; i < lines.Length; i++)
            {
                match = Regex.Match(lines[i], _extractTarget);
                if (match.Success)
                {
                    StringBuilder result = new StringBuilder();
                    for (int j = i - _numberOfLines; j < i + _numberOfLines; j++)
                    {
                        if (somContext.Options.Mode == SomMode.Debug){
                            result.AppendLine($"{lines[j]} [LN {j}]");
                        }    
                        else
                            result.AppendLine(lines[j]);
                    }
                    yield return result.ToString().TrimTrailingNewline();
                }
            }
            yield return "";  
        }   
    }
}
