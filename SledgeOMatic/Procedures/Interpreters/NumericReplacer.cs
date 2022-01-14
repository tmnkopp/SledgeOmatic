using System.Text;
using System.Text.RegularExpressions;
namespace SOM.Procedures
{ 
    public class NumericReplacer : KeyValReplacer
    { 
        public NumericReplacer(string Source) : base(Source) {  } 
        public override string Compile(string content)
        {
            StringBuilder result = new StringBuilder(); 
            foreach (var contentline in content.Split("\n"))
            {
                string line = contentline;
                foreach (var item in KeyVals)
                {
                    int cnt = 0;
                    string pattern = "([^\\d]|^)(" + item.Key + ")([^\\d]|$)"; 
                    while (Regex.IsMatch(line, pattern))
                    {
                        line = Regex.Replace(line, pattern,  
                            m => $"{m.Groups[1].Value}{item.Value}{m.Groups[3].Value}"  
                            , RegexOptions.Singleline);

                        if (cnt++ > 5)
                            break;
                    }; 
                }
                line = Regex.Replace(line, $"\r|\n", "");
                result.AppendLine(line);
            } 
            return result.ToString();
        }
    } 
}
