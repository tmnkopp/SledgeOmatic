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
    public class RegexInterpreter :  ICompilable
    {
        public Dictionary<string, string> KeyVals = new Dictionary<string, string>();
        public RegexInterpreter(string Expression, string Replacement)
        {
            this.KeyVals.Add(Expression, Replacement);
        }
        public RegexInterpreter(string JSON)
        {
            this.KeyVals = JsonConvert.DeserializeObject<Dictionary<string, string>>(JSON);
        }  
        public string Compile(string content)
        {
            StringBuilder result = new StringBuilder();
            string replacementContent = content;
            foreach (var KeyValItem in KeyVals)
            { 
                Match match = Regex.Match(content, KeyValItem.Key);
                while (match.Success)
                {
                    string matched = match.Groups[0].Value;
                    string replacement = KeyValItem.Value;
                    if (match.Groups.Count > 1)  
                        replacementContent = replacementContent.Replace(match.Groups[1].Value, replacement);
                    else
                        replacementContent = replacementContent.Replace(match.Groups[0].Value, replacement);

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

}
