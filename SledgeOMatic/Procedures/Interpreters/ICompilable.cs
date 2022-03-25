 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOM.Procedures
{
    [AttributeUsage(AttributeTargets.Constructor)]
    public class CompilableCtorMeta : Attribute
    {
        public bool Invokable { get; set; } = true;
        public string CommandMapper { get; set; } 
    }
    public interface ICompilable
    {
         string Compile(ISomContext somContext);
    }
    public abstract class BaseCompiler
    {
        protected virtual IEnumerable<string> ParseLines(string content){
            content = content.Replace($"\r", $"\n");
            content = content.Replace($"\n\n", $"\n");
            foreach (string line in content.Split($"\n")){
                yield return line;
            }
        }
    }
}
