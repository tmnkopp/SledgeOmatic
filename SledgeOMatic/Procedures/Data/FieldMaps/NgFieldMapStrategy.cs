using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
namespace SOM.Procedures.Data {  
    public class NgFieldMapStrategy : IFieldMapStrategy
    {
        string _wrap = "interface [entityname] {\n [fields] \n}";
        public string Wrap { get => _wrap; set => _wrap = value; } 
        public string Execute(SchemaField schemaField)
        {
            string format = "{0}: {1};";
            switch (schemaField.DATA_TYPE)
            {
                case "int":
                    return string.Format(format, schemaField.COLUMN_NAME, "number");
                default:
                    return string.Format(format, schemaField.COLUMN_NAME, "string");
            }
        }
    }
}
