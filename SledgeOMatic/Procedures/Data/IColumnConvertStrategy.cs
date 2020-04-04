using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOM.Procedures.Data
{
    public interface IColumnConvertStrategy
    {
        string Wrap  { get; set;  }
        string Convert(DBColumnDefinition schemaField);
    }
}
