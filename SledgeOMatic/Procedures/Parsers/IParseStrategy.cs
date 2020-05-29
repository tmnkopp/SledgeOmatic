using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOM.Procedures.Parsers
{
    public interface IParseStrategy
    {
        IEnumerable<string> Parse(string content);
    }
}
