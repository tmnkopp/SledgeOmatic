using SOM.IO;
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
    public class PathInterpreter : ICompilable
    {  
        public string Compile(string content)
        {
            StringBuilder result = new StringBuilder();
            string[] lines = content.Split('\n'); 
            foreach (var line in lines)
            {
                string pattern = "som:\\w:.+\\:som";
                Match match = Regex.Match(line, pattern);
                if (match.Success)
                {
                    string filename = match.Value.Replace("som:", "").Replace(":som", "");
                    FileReader r = new FileReader(filename); 
                    result.AppendFormat("{0}", line.Replace(match.Value, r.Read()));
                }   else  {
                    result.AppendFormat("{0}\n", line);
                } 
            }
            return result.ToString();       
        } 
    }
}
