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
        private string _indexName = "[index]";

        #endregion

        #region CTOR

        [CompilableCtorMeta()]
        public Indexer(int Seed, int Reset, string IndexName)
        {
            _indexName = IndexName;
            _seed = Seed;
            _reset = Reset;
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
                if (line.Contains(_indexName))
                    index++;
                result.AppendFormat("{0}\n", line.Replace("" + _indexName + "", ReSetter(index).ToString()));
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
