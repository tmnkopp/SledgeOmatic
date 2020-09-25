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
    public class ModuloCompile : ICompiler
    {  
        private StringBuilder result = new StringBuilder();
        public ModuloCompile()
        {  
        }
        public string Compile(string content)
        {
            string matchpattern = "";
            Match match = Regex.Match(content, @".*\[%:(\d) format:(.*).*\].*");
            if (match.Success)
            {
                GroupCollection groups = match.Groups;
                matchpattern = groups[0].Value.TrimTrailingNewline();
                string[] lines = content.Split(new[] { matchpattern }, StringSplitOptions.None);
                int modval = Convert.ToInt32(groups[1].Value.Replace("%", "")); 
                int index = 0;
                foreach (var line in lines)
                {
                    index++;
                    string append = ((index % modval == 0) && (index <= lines.Length)) ? groups[2].Value : "";
                    if (line != "")
                        result.AppendFormat("{0}{1}", line, append) ; 
                } 
            } 
            return result.ToString().Replace(matchpattern,"").TrimTrailingNewline();
        }  
    }
} 