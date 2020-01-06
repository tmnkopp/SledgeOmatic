using SOM.Procedures;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SOM.IO;
using SOM.Data;

namespace SOM.Procedures
{
    public class RegexCompile : IProcedure
    {
        private Dictionary<string, string> _dict;
        public RegexCompile(string sqlFile)
        {
            IReader r = new FileReader(sqlFile);
            KeyValDBReader dbreader = new KeyValDBReader(r.Read());
            dbreader.ExecuteRead();
            this._dict = dbreader.Data;
          
        }
        public RegexCompile(Dictionary<string, string> dict)
        { 
            this._dict = dict;

        }
        public   string Execute(string compileme)
        {
            StringBuilder result = new StringBuilder();
            string[] lines = compileme.Split('\n'); 
            foreach (var line in lines)
            {
                bool matched = false;
                foreach (var item in _dict)
                {
                    string pattern = item.Key;
                    Match match = Regex.Match(line, pattern);
                    if (match.Success )  {
                        string replacewith = item.Value.Replace("$1", match.Value);
                        result.AppendFormat("{0}\n",line.Replace(match.Value, replacewith));
                        matched = true;
                    } 
                }
                if (!matched)
                {
                    result.AppendFormat("{0}\n", line);
                }
            }
            return result.ToString(); 
        }
        
        public override string ToString()
        {
            return $"{base.ToString()} -#{_dict.ToString()}";
        }
    }
}
