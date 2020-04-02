using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOM.Procedures.Data
{
    public class SchemaField
    {
        public string COLUMN_NAME { get; set; }
        public string DATA_TYPE { get; set; }
        public bool Nullable { get; set; }
        public int MaxLen { get; set; }
    }
}
