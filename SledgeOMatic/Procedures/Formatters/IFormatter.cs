using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOM.Formatters
{
    public interface ITypeFormatter<T>
    { 
        string Format(T item);
    }

}
