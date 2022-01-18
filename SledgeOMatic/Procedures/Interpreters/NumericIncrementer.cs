using System;
using System.Text;
using System.Text.RegularExpressions;

namespace SOM.Procedures
{
    public class NumericIncrementer : ICompilable
    {
        private int _base = 0;
        private int _seed = 0;
        private string _incrementPattern = "";
        [CompilableCtorMeta()]
        public NumericIncrementer(object IncrementBase, object IncrementSeed, string NumericPattern)
        {
            _base = (int)Convert.ToInt32(IncrementBase);
            _seed = (int)Convert.ToInt32(IncrementSeed);
            _incrementPattern = NumericPattern;
        } 
        public string Compile(string content)
        {
            StringBuilder result = new StringBuilder();
            var lines = Regex.Split(content, $"\r|\n");
            foreach (var line in lines)
            {
                if (Regex.IsMatch(line, $@"(som!\w+|\w+!som)"))
                {
                    result.AppendLine(line);
                    continue;
                }
                string target = line;
                string pattern = "([^\\d]|^)(" + _incrementPattern + ")([^\\d]|$)";
                if (Regex.IsMatch(target, pattern))
                { 
                    target = Regex.Replace(target, pattern,
                        m => {
                            int nextint =(_seed - _base) + Convert.ToInt32(m.Groups[2].Value) + 0; 
                            return $"{m.Groups[1].Value}{nextint}{m.Groups[3].Value}";
                        }
                        , RegexOptions.Singleline);
                };
                target = Regex.Replace(target, $"\r|\n", "");
                if (!string.IsNullOrWhiteSpace(target)) 
                    result.AppendLine(target);  
            }
            return result.ToString();
        }
    }
}
