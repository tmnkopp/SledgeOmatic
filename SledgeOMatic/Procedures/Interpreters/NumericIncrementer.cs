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
        public NumericIncrementer(object Seed, object Base, string NumericPattern)
        {
            _seed = (int)Convert.ToInt32(Seed);
            _base = (int)Convert.ToInt32(Base);
            _incrementPattern = NumericPattern;
        } 
        public string Compile(string content)
        {
            StringBuilder result = new StringBuilder();
            content = content.Replace($"\r", $"\n");
            content = content.Replace($"\n\n", $"\n"); 
            foreach (var line in content.Split($"\n"))
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
                            int nextint =(_base - _seed) + Convert.ToInt32(m.Groups[2].Value) + 0; 
                            return $"{m.Groups[1].Value}{nextint}{m.Groups[3].Value}";
                        }
                        , RegexOptions.Singleline);
                }; 
                result.AppendLine($"{target}");
            } 
            return result.ToString();
        }
    }
}
