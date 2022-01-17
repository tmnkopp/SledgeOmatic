using SOM.Procedures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SOM.Extentions;
using System.Text.RegularExpressions;

namespace SOM.Procedures
{
    public class Incrementer : ICompilable
    {
        private int _increment = 0;
        private string _pattern = "";
        public Incrementer(string Pattern, int Increment)
        {
            _pattern = Pattern;
            _increment = Increment;
        }
        public string Compile(string content)
        { 
            Match match = Regex.Match(content, _pattern);
            string replacementContent = content;
            while (match.Success)
            {
                string targetReplace = match.Value;
                string numTarget = match.Groups[0].Value;
                if (match.Groups.Count > 1)
                    numTarget = match.Groups[1].Value;

                int replaceNum = Convert.ToInt32(numTarget) + _increment;
                string replacement = targetReplace.Replace(numTarget, replaceNum.ToString());
                replacementContent = replacementContent.Replace(targetReplace, replacement);

                content = content.Remove(0, match.Index + match.Length);
                match = Regex.Match(content, _pattern);
            } 
            return replacementContent.TrimTrailingNewline();
        }
    }
}
