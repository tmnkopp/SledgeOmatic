using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOM.Procedures.Data
{
    public interface IColumnConverter
    { 
        string Convert(PropDefinition columnDefinition);
    }
}
