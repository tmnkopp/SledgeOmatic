using SOM.Procedures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SOM.Extentions;
using System.Text.RegularExpressions;
using System.Text;

namespace SOM.Procedures
{
    public class Incrementer : ICompilable
    {
        private int _amount = 0; 
        private string _pattern = @"\d";
        [CompilableCtorMeta()]
        public Incrementer(int Amount, string Pattern)
        { 
            _amount = (int)Convert.ToInt32(Amount);
            _pattern = Pattern;
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
                string pattern = "([^\\d]|^)(" + _pattern + ")([^\\d]|$)";
                if (Regex.IsMatch(target, pattern))
                {
                    target = Regex.Replace(target, pattern,
                        m => {
                            int nextint = (_amount) + Convert.ToInt32(m.Groups[2].Value) + 0;
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
