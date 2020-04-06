using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
namespace SOM.Procedures.Data {
    public class ccCustomConverter : IColumnConverter
    { 
        Func<PropDefinition, string> _ColumnConverter; 
        public ccCustomConverter(Func<PropDefinition,string> ColumnConverter )
        {
            _ColumnConverter = ColumnConverter;
        }
        public string Convert(PropDefinition propDef)
        {
            return _ColumnConverter(propDef);
        }
    }
    public class ccCSHARPModel : IColumnConverter
    { 
        public string Convert(PropDefinition propDef)
        {
            string format = "public {1} {0} {{ get; set; }}";
            if (propDef.DATA_TYPE.ToLower().Contains("int")) 
                return string.Format(format, propDef.NAME, "int"); 
            if (propDef.DATA_TYPE.ToLower().Contains("date")) 
                return string.Format(format, propDef.NAME, "DateTime"); 
            return string.Format(format, propDef.NAME, "string");
        }
    }
    public class ccNgInput : IColumnConverter
    { 
        public string Convert(PropDefinition propDef)
        {
            string format = "<input type=\"text\" id=\"{0}\" formControlName =\"{0}\" class=\"form-control\" />";

            if (propDef.DATA_TYPE.ToLower().Contains("int")) {
                return string.Format(format, propDef.NAME, "int");
            } 
            if (propDef.DATA_TYPE.ToLower().Contains("date"))
                return string.Format(format, propDef.NAME, "DateTime");
            return string.Format(format, propDef.NAME, "string");
        }
    }
    public class ccNGInterface : IColumnConverter
    {
        public string Convert(PropDefinition propDef)
        {
            string format = "{0}: {1};";
            if (propDef.DATA_TYPE.ToLower().Contains("int"))
                return string.Format(format, propDef.NAME, "number"); 
            return string.Format(format, propDef.NAME, "string"); 
        }
    }
}
