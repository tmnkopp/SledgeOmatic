using SOM.Parsers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SOM.Extentions;

namespace SOM.Procedures
{ 
    public interface IParser<R>
    {
        IEnumerable<R> Parse(ISomContext somContext); 
    }   
}
