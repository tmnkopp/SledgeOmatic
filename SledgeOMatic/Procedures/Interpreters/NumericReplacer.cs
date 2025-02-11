﻿using System.Text;
using System.Text.RegularExpressions;
namespace SOM.Procedures
{ 
    public class NumericReplacer : KeyValReplacer
    { 
        #region CTOR 
        public NumericReplacer()
        { 
        }
        [CompilableCtorMeta()]
        public NumericReplacer(string Source) : base(Source)
        {
        }

        #endregion

        #region METHODS

        public override string Compile(ISomContext somContext)
        { 
            string content = somContext.Content;
            StringBuilder result = new StringBuilder();
            var keyvals = base.PopulateKeyVals(somContext);
              
            foreach (var contentline in base.ParseLines(content))
            {
                string line = contentline;
                if (Regex.IsMatch(line, $@"(som!\w+|\w+!som)"))
                {
                    result.AppendLine(line);
                    continue;
                } 
                foreach (var item in keyvals)
                {
                    int cnt = 0;
                    string pattern = "([^\\d]|^)(" + item.Key + ")([^\\d]|$)";
                    while (Regex.IsMatch(line, pattern))
                    {
                        line = Regex.Replace(line, pattern,
                            m => $"{m.Groups[1].Value}{item.Value}{m.Groups[3].Value}"
                            , RegexOptions.Singleline);
                        if (cnt++ > 5)
                            break;
                    };
                } 
                line = Regex.Replace(line, $"\r|\n", "");
                result.AppendLine(line);
            }
            return result.ToString();
        }

        #endregion
    } 
}
