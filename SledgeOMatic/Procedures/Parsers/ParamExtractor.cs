using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SOM.Procedures
{
    public class ParamExtractor : IParser<KeyValuePair<string, string>>
    {
        public ParamExtractor()
        { 
        } 
        public IEnumerable<KeyValuePair<string, string>> Parse(ISomContext somContext)
        {
            string content = somContext.Content.Replace("\r", "");
            var m = Regex.Match(content, $"(/p:.*)\n");
            if (m.Success) { 
                foreach (var item in Regex.Split(m.Groups[1].Value, $"/p:")) {
                    if (item.Contains("="))
                        yield return new KeyValuePair<string, string>(item.Split("=")[0], item.Split("=")[1]);
                }
            } 
        }
    }
}
