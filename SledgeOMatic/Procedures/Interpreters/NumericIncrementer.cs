using System;
using System.Text;
using System.Text.RegularExpressions;

namespace SOM.Procedures
{
    public class NumericIncrementer : BaseCompiler, ICompilable
    {
        #region PROPS  
        public int From { get; set; }
        public int To { get; set; }
        public string Pattern { get; set; }
        #endregion

        #region CTOR 
        public NumericIncrementer()
        {

        }
        [CompilableCtorMeta()]
        public NumericIncrementer(object From, object To, string Pattern)
        {
            this.From = (int)Convert.ToInt32(From);
            this.To = (int)Convert.ToInt32(To);
            this.Pattern = Pattern;
        } 
        #endregion

        #region METHODS

        public string Compile(ISomContext somContext)
        {
            string content = somContext.Content;
            StringBuilder result = new StringBuilder();
            somContext.Logger.Debug("{o}", new { NumericPattern=this.Pattern });
            foreach (var line in base.ParseLines(content))
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
                            int nextint = (this.To - this.From) + Convert.ToInt32(m.Groups[2].Value) + 0;
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
