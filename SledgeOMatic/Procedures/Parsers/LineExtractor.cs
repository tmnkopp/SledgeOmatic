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
            content = content.Replace("\r", "\n").Replace("\n\n", "\n");
            content = $"{new string('\n', _numberOfLines)}{content}{new string('\n', _numberOfLines)}";
            string[] lines = content.Split('\n');
            int findingCnt = 0;
            for (int lineIndex = _numberOfLines; lineIndex < lines.Length - _numberOfLines; lineIndex++)
            { 
                Match match = Regex.Match(lines[lineIndex], _extractTarget); 
                if (match.Success)
                {
                    StringBuilder result = new StringBuilder();
                    findingCnt++;
                    int cursor = lineIndex;
                    for (int takeIndex = lineIndex - _numberOfLines; takeIndex <= lineIndex + _numberOfLines; takeIndex++)
                    {
                        if (takeIndex < lines.Length && takeIndex > 0)
                        { 
                            if (somContext.Options.Mode == SomMode.Cache)
                                result.Append( lines[takeIndex] + "\n"  );
                            else
                                result.Append($"{lines[takeIndex]} [LN {takeIndex.ToString()}]\n");  
                        } 
                        cursor = takeIndex;
                    }
                    lineIndex = cursor + 1;
                    yield return result.ToString().TrimTrailingNewline();
                }
            } 
        }   
    }
}
