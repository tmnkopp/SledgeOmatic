using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
namespace SOM.Procedures.Data {
    public class ccCustomConverter : IColumnConverter
    { 
        Func<DBColumnDefinition, string> _ColumnConverter; 
        public ccCustomConverter(Func<DBColumnDefinition,string> ColumnConverter )
        {
            _ColumnConverter = ColumnConverter;
        }
        public string Convert(DBColumnDefinition dbColumnDefinition)
        {
            return _ColumnConverter(dbColumnDefinition);
        }
    }
    public class ccCSHARPModel : IColumnConverter
    { 
        public string Convert(DBColumnDefinition dbColumnDefinition)
        {
            string format = "public {1} {0} {{ get; set; }}";
            switch (dbColumnDefinition.DATA_TYPE.ToLower())
            {
                case "int":
                    return string.Format(format, dbColumnDefinition.COLUMN_NAME, "int");
                case "datetime":
                    return string.Format(format, dbColumnDefinition.COLUMN_NAME, "DateTime");
                default:
                    return string.Format(format, dbColumnDefinition.COLUMN_NAME, "string");
            }
        }
    }
    public class ccNgInput : IColumnConverter
    { 
        public string Convert(DBColumnDefinition dbColumnDefinition)
        {
            string format = "<input type=\"text\" id=\"{1}\" formControlName =\"{1}\" class=\"form-control\" />";
            switch (dbColumnDefinition.DATA_TYPE.ToLower())
            {
                case "int":
                    return string.Format(format, dbColumnDefinition.COLUMN_NAME, "int");
                case "datetime":
                    return string.Format(format, dbColumnDefinition.COLUMN_NAME, "DateTime");
                default:
                    return string.Format(format, dbColumnDefinition.COLUMN_NAME, "string");
            }
        }
    }
    public class ccNGInterface : IColumnConverter
    {
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
