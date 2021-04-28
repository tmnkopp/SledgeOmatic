using SOM.Procedures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOM.Extentions;
using System.Text.RegularExpressions;

namespace SOM.Procedures
{
    public class PatternIndexer : ICompilable
    {
        private int seed = 0;
        private int _reset = 1;
        private string Pattern = "[index]";
        public PatternIndexer(string Pattern, int Seed)
        {
            this.seed = Seed;
            this.Pattern = Pattern;
        }
        public string Compile(string content)
        {
            StringBuilder result = new StringBuilder();
            string[] lines = content.Split('\n'); 
            foreach (var line in lines) {
                string rslt = Regex.Replace(line, this.Pattern, (Match m) => (
                //"FOO"
                m.Groups[0].Value.Replace(m.Groups[1].Value, (this.seed++).ToString())
                ));
                result.AppendFormat("{0}\n", rslt);
            }
            return result.ToString().TrimTrailingNewline();
        }
       
    }
}
