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
    public class Indexer : BaseCompiler, ICompilable
    {

        #region PROPS  
        public int Seed { get; set; }
        public int Reset { get; set; }
        public string Pattern { get; set; } = @"";
        #endregion

        #region FIELDS 
        #endregion

        #region CTOR
        public Indexer()
        {

        }
        [CompilableCtorMeta()]
        public Indexer(object Seed, object Reset, string IndexPattern)
        {
            this.Pattern = IndexPattern;
            this.Seed = (int)Convert.ToInt32(Seed);
            this.Reset = (int)Convert.ToInt32(Reset);
        }

        #endregion

        #region METHODS

        public virtual string Compile(ISomContext somContext)
        {
            string content = somContext.Content;
            StringBuilder result = new StringBuilder();
            int index = this.Seed;
            foreach (var line in base.ParseLines(content))
            {
                if (Regex.IsMatch(line, $@"(som!\w+|\w+!som)"))
                {
                    result.AppendLine(line);
                    continue;
                }
                var m = Regex.Match(line, this.Pattern);
                if (m.Success)
                {
                    var val = ReSetter(index).ToString();
                    if (m.Groups.Count == 1) 
                        result.AppendFormat("{0}\n", line.Replace(m.Value, val));
                    if (m.Groups.Count == 2){
                        val = m.Groups[0].Value.Replace(m.Groups[1].Value, val);
                        result.AppendFormat("{0}\n", line.Replace(m.Value, val));
                    }
                    if (m.Groups.Count == 4 ) 
                        result.AppendFormat("{0}\n", line.Replace(m.Groups[0].Value, $"{m.Groups[1].Value}{val}{m.Groups[3].Value}"));
                 
                    index++; 
                }
                else
                { 
                    result.AppendFormat("{0}\n", line);
                } 
            }
            return result.ToString().TrimTrailingNewline();
        }
        private int ReSetter(int index)
        {
            if (this.Reset <= 1)
                return index;
            return (this.Seed + 1) + ((index) % this.Reset);
        }

        #endregion
    }
}
