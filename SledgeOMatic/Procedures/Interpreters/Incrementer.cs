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
    public class Incrementer : BaseCompiler, ICompilable
    {

        #region PROPS  
        public int Amount { get; set; } 
        public string Pattern { get; set; } = @"";
        #endregion
        #region FIELDS 
        #endregion

        #region CTOR
        public Incrementer()
        {

        }
        [CompilableCtorMeta()]
        public Incrementer(int Amount, string Pattern)
        {
            this.Amount = (int)Convert.ToInt32(Amount);
            this.Pattern = Pattern; 
        }

        #endregion

        #region METHODS

        public string Compile(ISomContext somContext)
        {
            string content = somContext.Content;
            StringBuilder result = new StringBuilder();
            var lines = Regex.Split(content, $"\r|\n");
            foreach (var line in ParseLines(content))
            {
                if (Regex.IsMatch(line, $@"(som!\w+|\w+!som)"))
                {
                    result.AppendLine(line);
                    continue;
                }
                string target = line;
                string pattern = "([^\\d]|^)(" + this.Pattern + ")([^\\d]|$)";
                if (Regex.IsMatch(target, pattern))
                {
                    target = Regex.Replace(target, pattern,
                        m =>
                        {
                            int nextint = (this.Amount) + Convert.ToInt32(m.Groups[2].Value) + 0;
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

        #endregion

    }
}
