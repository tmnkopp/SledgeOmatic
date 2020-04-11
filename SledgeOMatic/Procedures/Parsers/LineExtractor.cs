using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOM.Extentions;
namespace SOM.Procedures
{
    public class LineExtractor : IProcedure
    {
        private string _extractTarget;
        private int _numberOfLines = 4;
        private bool _verbose = false;
        public LineExtractor(string ExtractTarget, int NumberOfLines, bool Verbose)
        {
            _extractTarget = ExtractTarget;
            _numberOfLines = NumberOfLines;
            _verbose = Verbose;
        }
        public LineExtractor(string ExtractTarget, int NumberOfLines )
        {
            _extractTarget = ExtractTarget;
            _numberOfLines = NumberOfLines;
            _verbose = false;
        }
        public string Execute(string content)
        {
            StringBuilder result = new StringBuilder(); 

            //content = content.RemoveEmptyLines();
            content = $"{new string('\n', _numberOfLines)}{content}{new string('\n', _numberOfLines)}";
            string[] lines = content.Split('\n');
            int findingCnt = 0;
            for (int lineIndex = _numberOfLines; lineIndex < lines.Length - _numberOfLines; lineIndex++)
            {
                if (lines[lineIndex].Contains(_extractTarget))
                {
                    findingCnt++;
                    if (_verbose)
                        result.Append($"\n[{findingCnt.ToString()}:{lineIndex}]\n");
                    for (int takeIndex = lineIndex - _numberOfLines; takeIndex <= lineIndex + _numberOfLines; takeIndex++)
                    { 
                        if (takeIndex < lines.Length && takeIndex > 0 ) {
                            if (_verbose) 
                                result.Append($"[{takeIndex.ToString()}]: {lines[takeIndex]}\n");
                            else
                                result.Append($"{lines[takeIndex]}\n");
                        }
                            
                    }
                    
                }
            } 
            return result.ToString().TrimTrailingNewline();
        }
    }
}
