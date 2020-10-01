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
    public class LineExtractor : IParseStrategy
    {
        private string _extractTarget;
        private int _numberOfLines = 4; 
        public LineExtractor(string ExtractTarget, int NumberOfLines )
        {
            _extractTarget = ExtractTarget;
            _numberOfLines = NumberOfLines; 
        }
        private ParseResultMode _ParseResultMode = Parsers.ParseResultMode.Default;
        public ParseResultMode ParseResultMode
        {
            get { return _ParseResultMode; }
            set { _ParseResultMode = value; }
        }
        public string Parse(string content)
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
                    //_result.Clear();
                    findingCnt++;
                    int cursor = lineIndex;
                    for (int takeIndex = lineIndex - _numberOfLines; takeIndex <= lineIndex + _numberOfLines; takeIndex++)
                    {
                        if (takeIndex < lines.Length && takeIndex > 0)
                        {
                            if (ParseResultMode == ParseResultMode.Verbose)  {
                                _result.Append($"{lines[takeIndex]} [LN {takeIndex.ToString()}]\n");
                            } else {
                                _result.Append($"{lines[takeIndex]}\n");
                            }    
                        }
                        cursor = takeIndex;
                    }
                    lineIndex = cursor + 1;  
                }
            }
            return _result.ToString().TrimTrailingNewline();
        }   
    }
}
