using System;
using System.Text;
using System.Text.RegularExpressions;

namespace SOM.Procedures
{
    public class NumericIncrementer : BaseCompiler, ICompilable
    {
        #region PROPS  
        public int Seed { get; set; }
        public int Base { get; set; }
        public string NumericPattern { get; set; }
        #endregion

        #region CTOR 
        public NumericIncrementer()
        {

        }
        [CompilableCtorMeta()]
        public NumericIncrementer(object Seed, object Base, string NumericPattern)
        {
            this.Seed = (int)Convert.ToInt32(Seed);
            this.Base = (int)Convert.ToInt32(Base);
            this.NumericPattern = NumericPattern;
        } 
        #endregion

        #region METHODS

        public string Compile(ISomContext somContext)
        {
            string content = somContext.Content;
            StringBuilder result = new StringBuilder();
            somContext.Logger.Information("{o}", new { NumericPattern=this.NumericPattern });
            foreach (var line in base.ParseLines(content))
            {
                if (Regex.IsMatch(line, $@"(som!\w+|\w+!som)"))
                {
                    result.AppendLine(line);
                    continue;
                }
                string target = line;
                string pattern = "([^\\d]|^)(" + this.NumericPattern + ")([^\\d]|$)";
                if (Regex.IsMatch(target, pattern))
                {
                    target = Regex.Replace(target, pattern,
                        m =>
                        {
                            int nextint = (this.Base - this.Seed) + Convert.ToInt32(m.Groups[2].Value) + 0;
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
