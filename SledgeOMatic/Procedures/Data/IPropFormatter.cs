using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOM.Procedures.Data
{
    public interface IPropFormatter
    { 
        string Format(PropDefinition PropertyDefinition);
    }
    public class Propformatter : IPropFormatter
    {
        Func<PropDefinition, string> _FormatMethod;
        public Propformatter(Func<PropDefinition, string> FormatMethod)
        {
            _FormatMethod = FormatMethod;
        }
        public string Format(PropDefinition propDef)
        {
            return _FormatMethod(propDef);
        }
    }
}
