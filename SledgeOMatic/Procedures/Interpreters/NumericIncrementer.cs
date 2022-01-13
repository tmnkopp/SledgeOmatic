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
        public NumericIncrementer(int IncrementBase, int IncrementSeed, string NumericPattern)
        {
            _base = IncrementBase;
            _seed = IncrementSeed;
            _incrementPattern = NumericPattern;
        } 
        public string Compile(string content)
        {
            StringBuilder result = new StringBuilder();
            var lines = Regex.Split(content, $"\r|\n");
            foreach (var line in lines)
            {
                string target = line;
                string pattern = "([^\\d])(" + _incrementPattern + ")([^\\d])";
                if (Regex.IsMatch(target, pattern))
                { 
                    target = Regex.Replace(target, pattern,
                        m => {
                            int nextint =(_seed - _base) + Convert.ToInt32(m.Groups[2].Value) + 1; 
                            return $"{m.Groups[1].Value}{nextint}{m.Groups[3].Value}";
                        }
                        , RegexOptions.Singleline);
                };
                target = Regex.Replace(target, $"\r|\n", "");
                result.AppendLine(target);
            }
            return result.ToString();
        }
    }
}
