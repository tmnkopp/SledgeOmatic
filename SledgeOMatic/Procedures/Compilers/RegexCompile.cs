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
    public abstract class BaseRegexCompile : ICompiler
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
            return JsonConvert.SerializeObject(  this.KeyVals  );
        }
    }
    public class SqlRegexCompile : BaseRegexCompile 
    { 
        public SqlRegexCompile(string SqlFile)
        {
            IReader r = new FileReader(SqlFile);
            KeyValDBReader dbreader = new KeyValDBReader(r.Read());
            dbreader.ExecuteRead();
            this.KeyVals = dbreader.Data; 
        } 
    }
    public class JsonRegexCompile : BaseRegexCompile 
    { 
        public JsonRegexCompile(string json)
        { 
            this.KeyVals = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        } 
    }
    public class  RegexCompile : BaseRegexCompile
    {
        public RegexCompile(string Expression, string Replacement)
        {
            this.KeyVals.Add(Expression, Replacement);
        }
    }
}
