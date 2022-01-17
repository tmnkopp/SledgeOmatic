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
    public class RegexReplacer : KeyValReplacer, ICompilable
    {
        [CompilableCtorMeta()]
        public RegexReplacer(string Source) : base(Source) { }
        public override string Compile(string content)
        {
            StringBuilder result = new StringBuilder();
            foreach (var line in content.Split("\n"))   {
                string replacement = line;
                foreach (var item in KeyVals) { 
                    var match = Regex.Match(replacement, item.Key);
                    if (match.Success)
                    { 
                        replacement = Regex.Replace(
                              replacement
                            , item.Key
                            , m => {
                                if (m.Groups.Count > 2)
                                    return $"{ m.Groups[1]}{item.Value}{m.Groups[3]}";
                                else
                                    return item.Value;
                                }
                            , RegexOptions.Singleline); 
                    } 
                }
                result.Append(replacement);
            }
            return result.ToString(); 
        }
    } 
}
