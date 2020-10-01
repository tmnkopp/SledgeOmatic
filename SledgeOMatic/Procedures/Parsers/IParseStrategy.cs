using SOM.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOM.Procedures
{
    public interface IParseStrategy
    { 
        string Parse(string content);
        ParseResultMode ParseResultMode { get; set; }
    }
}
