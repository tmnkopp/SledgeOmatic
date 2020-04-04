using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
namespace SOM.Procedures.Data {  
    public class NGMapper : IColumnConvertStrategy
    {
        string _wrap = "interface [entityname] {\n [fields] \n}";
        public string Wrap { get => _wrap; set => _wrap = value; }
        public NGMapper()
        {

        }
        public NGMapper( string Wrapper)
        {
            _wrap = Wrapper;
        }
        public string Convert(DBColumnDefinition dbColumnDefinition)
        {
            string format = "{0}: {1};";
            switch (dbColumnDefinition.DATA_TYPE)
            {
                case "int":
                    return string.Format(format, dbColumnDefinition.COLUMN_NAME, "number");
                default:
                    return string.Format(format, dbColumnDefinition.COLUMN_NAME, "string");
            }
        }
    }
}
