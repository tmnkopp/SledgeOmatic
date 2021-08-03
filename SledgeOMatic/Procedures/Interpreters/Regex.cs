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
        public RegexReplacer(string Source) : base(Source) { }
        public override string Compile(string content)
        {
            foreach (var KeyValItem in KeyVals)
            {
                content = Regex.Replace(content, KeyValItem.Key, KeyValItem.Value);
            }
            return content.TrimTrailingNewline();
        }
    }

}
