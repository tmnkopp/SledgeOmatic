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
using SOM.Extentions;

namespace SOM.Procedures
{
    public abstract class BaseRegexCompile : ICompiler
    {
        public Dictionary<string, string> KeyVals = new Dictionary<string, string>();
        public virtual string Compile(string content)
        {
            StringBuilder result = new StringBuilder();
            string[] lines = content.Split('\n');
            foreach (var line in lines)
            {
                bool matched = false;
                foreach (var KeyValItem in KeyVals)
                { 
                    Match match = Regex.Match(line, KeyValItem.Key);
                    if (match.Success)
                    {
                        matched = true;
                        string matchval = match.Groups[0].Value;
                        if (match.Groups.Count > 1) 
                            matchval = match.Groups[1].Value;
                      
                        string replacewith = KeyValItem.Value.Replace("$0", matchval); 
                        result.AppendFormat("{0}\n", line.Replace(matchval, replacewith)); 
                    }
                }
                if (!matched)
                {
                    result.AppendFormat("{0}\n", line);
                }
            }
            return result.ToString().TrimTrailingNewline();
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
    public class RegexCompile : BaseRegexCompile
    {
        public RegexCompile(string Expression, string Replacement)
        {
            this.KeyVals.Add(Expression, Replacement);
        }
    }
}
