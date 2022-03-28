using SOM.Procedures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOM.Extentions;
using System.Text.RegularExpressions;

namespace SOM.Procedures
{

    public class PatternIndexer : BaseCompiler, ICompilable
    {
        #region FIELDS

        private int seed = 0;
        private string Pattern = "(index)";

        #endregion

        #region CTOR

        [CompilableCtorMeta()]
        public PatternIndexer(string Pattern, int Seed)
        {
            this.seed = Seed;
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
                    result.AppendFormat("{0}\n", line);
                    continue;
                }
                var rslt = line;
                if (Regex.IsMatch(rslt, this.Pattern))
                {
                    rslt = Regex.Replace(rslt, this.Pattern,
                        (Match m) => (
                            m.Groups[0].Value.Replace(m.Groups[1].Value, (this.seed++).ToString())
                    ));
                }
                result.AppendFormat("{0}\n", rslt);
            }
            return result.ToString().TrimTrailingNewline();
        }

        #endregion
    }
}
