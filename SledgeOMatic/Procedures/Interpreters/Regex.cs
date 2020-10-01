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
    public abstract class BaseRegexInterpreter 
    {
        public Dictionary<string, string> KeyVals = new Dictionary<string, string>();
        public virtual string Interpret(string content)
        {
            StringBuilder result = new StringBuilder();
            string replacementContent = content;
            foreach (var KeyValItem in KeyVals)
            { 
                Match match = Regex.Match(content, KeyValItem.Key); 
                while (match.Success)
                { 
                    string targetReplacement = match.Groups[0].Value;   
                    replacementContent = replacementContent.Replace(targetReplacement, KeyValItem.Value);

                    content = content.Remove(0, match.Index + match.Length);
                    match = Regex.Match(content, KeyValItem.Key);
                }
            } 
            return replacementContent.TrimTrailingNewline(); 
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(  this.KeyVals  );
        }
    }
    public class SqlRegexInterpreter : BaseRegexInterpreter , IInterpreter
    { 
        public SqlRegexInterpreter(string SqlFile)
        {
            IReader r = new FileReader(SqlFile);
            KeyValDBReader dbreader = new KeyValDBReader(r.Read());
            dbreader.ExecuteRead();
            this.KeyVals = dbreader.Data; 
        } 
    } 
    public class RegexInterpreter : BaseRegexInterpreter, IInterpreter
    {
        public RegexInterpreter(string Expression, string Replacement)
        {
            this.KeyVals.Add(Expression, Replacement);
        }
        public RegexInterpreter(string JSON)
        {
            this.KeyVals = JsonConvert.DeserializeObject<Dictionary<string, string>>(JSON);
        }
        public RegexInterpreter(Dictionary<string, string> KeyVals)
        {
            this.KeyVals = KeyVals;
        }
    }
}
