using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOM.Procedures.Formatters
{
    public interface IWrapStrategy
    { 
        string Wrap(string content);
    }
}
