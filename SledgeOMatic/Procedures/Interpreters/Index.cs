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
        #region FIELDS

        private int _seed = 0;
        private int _reset = 0;
        private string _pattern = @"";

        #endregion

        #region CTOR

        [CompilableCtorMeta()]
        public Indexer(object Seed, object Reset, string IndexPattern)
        {
            _pattern = IndexPattern;
            _seed = (int)Convert.ToInt32(Seed);
            _reset = (int)Convert.ToInt32(Reset);
        }

        #endregion

        #region METHODS

        public virtual string Compile(ISomContext somContext)
        {
            string content = somContext.Content;
            StringBuilder result = new StringBuilder();
            int index = _seed;
            foreach (var line in base.ParseLines(content))
            {
                if (Regex.IsMatch(line, $@"(som!\w+|\w+!som)"))
                {
                    result.AppendLine(line);
                    continue;
                }
                var m = Regex.Match(line, _pattern);
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
            if (_reset <= 1)
                return index;
            return (_seed + 1) + ((index) % _reset);
        }

        #endregion
    }
}
