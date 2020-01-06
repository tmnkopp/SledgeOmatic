using SOM.Procedures;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
namespace SOM.Procedures
{
    public class RegexCompile : IProcedure
    {
        private Dictionary<string, string> _dict;
        public RegexCompile(Dictionary<string, string> Dict)
        {
            _dict = Dict;
        }
        public string Execute(string compileme)
        {
            StringBuilder result = new StringBuilder();
            string[] lines = compileme.Split('\n'); 
            foreach (var line in lines)
            {
                foreach (var item in _dict)
                {
                    string pattern = item.Key;
                    Match match = Regex.Match(line, pattern);
                    if (match.Success)  {
                        string replacewith = item.Value.Replace("$1", match.Value);
                        result.AppendFormat("{0}\n",line.Replace(match.Value, replacewith));
                    } else {
                        result.AppendFormat("{0}\n",line);
                    }
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
