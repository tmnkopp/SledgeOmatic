using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOM.Procedures
{
    public class CustomCompile: IProcedure
    {
        private string Customize(string result) {
            result = result.Replace("~", "");
            result = string.Format("{1}{0}{2}", result, "",""); 
            return result;
        }

        public string Execute(string compileme)
        {
            StringBuilder result = new StringBuilder();
            string[] lines = compileme.Split('\n'); 
            foreach (var line in lines) 
                result.AppendFormat("{0}\n", Customize(line));
          
            return result.ToString();
        }
    }
}
