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
    public class InjectableCompile : ICompiler
    {  
        private StringBuilder result = new StringBuilder();
        private List<string> Injectables; 
        public InjectableCompile()
        {
            Injectables = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                .Where(x => typeof(Injectable).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(x => x.Name).ToList(); 
        }
        public string Compile(string content)
        { 
            string[] lines = content.Split('\n'); 
            foreach (var line in lines)
            {
                InjectContent(line); 
            }
            return result.ToString();       
        }
        private string InjectContent(string line) {

            foreach (string Injectable in Injectables)
            {
                string pattern = $"\\[{Injectable} .*\\]";
                Match match = Regex.Match(line, pattern);
                if (match.Success)
                {
                    // filename = match.Value.Replace("[", "").Replace("]", "");
                    //Invoker.Invoke
                    //result.AppendFormat("{0}", line.Replace(match.Value, r.Read()));
                }
                else
                {
                    result.AppendFormat("{0}\n", line);
                }
            }
            return "";
        }
        public override string ToString()
        {
            return $"{base.ToString()}";
        }
    }
}
