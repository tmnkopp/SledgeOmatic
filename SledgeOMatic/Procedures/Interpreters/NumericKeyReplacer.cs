using System.Text;
using System.Text.RegularExpressions;
namespace SOM.Procedures
{ 
    public class NumericKeyReplacer : KeyValReplacer
    { 
        public NumericKeyReplacer(string Source) : base(Source) {  } 
        public override string Compile(string content)
        {
            StringBuilder result = new StringBuilder(); 
            foreach (var line in content.Split("\n"))
            {
                string target = line;
                foreach (var item in KeyVals)
                { 
                    string pattern = "([^\\d])(" + item.Key + ")([^\\d])"; 
                    if (Regex.IsMatch(target, pattern))
                    {
                        target = Regex.Replace(target, pattern,  
                            m => $"{m.Groups[1].Value}{item.Value}{m.Groups[3].Value}"  
                            , RegexOptions.Singleline); 
                    }; 
                }
                target = Regex.Replace(target, "\r|\n", "");
                result.AppendLine(target);
            } 
            return result.ToString();
        }
    } 
}
