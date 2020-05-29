using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SOM.Extentions;
using SOM.Procedures.Parsers;

namespace SOM.Procedures
{
    public class LineExtractor : ICompiler, IParseStrategy
    {
        private string _extractTarget;
        private int _numberOfLines = 4; 
        public LineExtractor(string ExtractTarget, int NumberOfLines )
        {
            _extractTarget = ExtractTarget;
            _numberOfLines = NumberOfLines; 
        }
        public string Compile(string content)
        {
            StringBuilder _result = new StringBuilder();
            foreach (string item in Parse(content))
                _result.Append($"{item}"); 
            return _result.ToString().TrimTrailingNewline();
        }

        public IEnumerable<string> Parse(string content)
        {
            StringBuilder _result = new StringBuilder();
            content = content.Replace("\r", "\n").Replace("\n\n", "\n");
            content = $"{new string('\n', _numberOfLines)}{content}{new string('\n', _numberOfLines)}";
            string[] lines = content.Split('\n');
            int findingCnt = 0;
            for (int lineIndex = _numberOfLines; lineIndex < lines.Length - _numberOfLines; lineIndex++)
            {
                Match match = Regex.Match(lines[lineIndex], _extractTarget);
                if (match.Success)
                {
                    _result.Clear();
                    findingCnt++; 
                    for (int takeIndex = lineIndex - _numberOfLines; takeIndex <= lineIndex + _numberOfLines; takeIndex++)
                    {
                        if (takeIndex < lines.Length && takeIndex > 0)
                        {
                            _result.Append($"{lines[takeIndex]} [LN {takeIndex.ToString()}]\n"); 
                        }
                    }
                    yield return _result.ToString();
                }
            }
        }
    }
}
