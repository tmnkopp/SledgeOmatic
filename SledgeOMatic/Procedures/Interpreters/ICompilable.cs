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
        public bool Invokable { get; set; }
    }
    public interface ICompilable
    {
         string Compile(string content);
    } 
}
