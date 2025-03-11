using System;
using System.Text;
using System.Text.RegularExpressions;
using SOM.Core;
namespace SOM.Procedures
{
    public class NumericIncrementer : BaseCompiler, ICompilable
    {
        #region PROPS   
        [InlineParam(Alias ="PrevSeed")]
        public int From { get; set; }
        [InlineParam(Alias = "NextSeed")]
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
            foreach (var line in base.ParseLines(content))
            {
                if (Regex.IsMatch(line, $@"(som!\w+|\w+!som)"))
                {
                    result.AppendLine(line);
                    continue;
                }
                string oldLine = line;
                string pattern = "([^\\d]|^)(" + this.Pattern + ")([^\\d]|$)";
                string newLine = "";
                var match = Regex.Match(oldLine, pattern);
                if(!match.Success) {
                    oldLine = Regex.Replace(oldLine, $@"\n|\r", "");
                    result.AppendLine($"{oldLine}");
                }else{
                    while (match.Success)
                    {
                        int pos = match.Index;
                        int newValue = (this.To - this.From) + Convert.ToInt32(match.Groups[2].Value);
                        newLine = newLine + oldLine.Substring(0, pos) + $"{match.Groups[1].Value}{newValue.ToString()}{match.Groups[3].Value}";
                        int matchLength = pos + match.Value.Length;
                        oldLine = oldLine.Substring(matchLength, oldLine.Length - matchLength);
                        match = Regex.Match(oldLine, pattern);
                    };
                    if (!string.IsNullOrWhiteSpace(oldLine)) newLine += oldLine;
                    newLine = Regex.Replace(newLine, $@"\n|\r", ""); 
                    result.AppendLine($"{newLine}");
                }   
            }
            return result.ToString();
        }

        #endregion
    }
}
