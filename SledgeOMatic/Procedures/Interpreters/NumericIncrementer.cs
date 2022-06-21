using System;
using System.Text;
using System.Text.RegularExpressions;

namespace SOM.Procedures
{
    public class NumericIncrementer : BaseCompiler, ICompilable
    {
        #region FIELDS

        private int _base = 0;
        private int _seed = 0;
        private string _incrementPattern = "";

        #endregion
        
        #region CTOR

        [CompilableCtorMeta()]
        public NumericIncrementer(object Seed, object Base, string NumericPattern)
        {
            _seed = (int)Convert.ToInt32(Seed);
            _base = (int)Convert.ToInt32(Base);
            _incrementPattern = NumericPattern;
        }

        #endregion

        #region METHODS

        public string Compile(ISomContext somContext)
        {
            string content = somContext.Content;
            StringBuilder result = new StringBuilder(); 
            foreach (var line in base.ParseLines(content))
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
                        m =>
                        {
                            int nextint = (_base - _seed) + Convert.ToInt32(m.Groups[2].Value) + 0;
                            return $"{m.Groups[1].Value}{nextint}{m.Groups[3].Value}";
                        }
                        , RegexOptions.Singleline);
                };
                target = Regex.Replace(target, $@"\n|\r", "");
                result.AppendLine($"{target}");
            }
            return result.ToString();
        }

        #endregion
    }
}
