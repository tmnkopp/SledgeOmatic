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
    public abstract class RegexCompile : ICompiler
    {
        public Dictionary<string, string> KeyVals = new Dictionary<string, string>();
        public virtual string Compile(string compileme)
        {
            StringBuilder result = new StringBuilder();
            string[] lines = compileme.Split('\n');
            foreach (var line in lines)
            {
                bool matched = false;
                foreach (var item in KeyVals)
                {
                    string pattern = item.Key;
                    Match match = Regex.Match(line, pattern);
                    if (match.Success)
                    {
                        string replacewith = item.Value.Replace("$1", match.Value);
                        result.AppendFormat("{0}\n", line.Replace(match.Value, replacewith));
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
            return $"{base.ToString()} -{KeyVals.ToString()}";
        }
    }
    public class SqlRegexCompile : RegexCompile 
    { 
        public SqlRegexCompile(string SqlFile)
        {
            IReader r = new FileReader(SqlFile);
            KeyValDBReader dbreader = new KeyValDBReader(r.Read());
            dbreader.ExecuteRead();
            this.KeyVals = dbreader.Data; 
        }  
        public override string ToString()
        {
            return $"{base.ToString()} -#{KeyVals.ToString()}";
        }
    }
    public class KeyValRegexCompile : RegexCompile 
    { 
        public KeyValRegexCompile(Dictionary<string, string> Dict)
        { 
            this.KeyVals = Dict;
        }
        public override string ToString()
        {
            return $"{base.ToString()} -#{KeyVals.ToString()}";
        }
    }
}
