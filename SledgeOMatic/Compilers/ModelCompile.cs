using SOM.Data;
using SOM.IO;
using SOM.Procedures;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using SOM.Models;

namespace SOM.Compilers
{ 
    public  class PropListFormatter : BasePropListFormatter 
    {
        public PropListFormatter(string ModelName)
        {
            this.ModelEnumerator = this.DeriveModelEnumerator(ModelName);
        }
        public PropListFormatter(string ModelName, IModelItemWrapper PropFormatter): this(ModelName)
        { 
            this.PropFormatter = PropFormatter; 
        } 
    }

    public abstract class BasePropListFormatter  {
        public string ModelName = "";
        public IModelItemWrapper PropFormatter;
        public IModelEnumerator ModelEnumerator;
        public BasePropListFormatter()
        { 
        } 
        public BasePropListFormatter(IModelItemWrapper PropFormatter, IModelEnumerator ModelEnumerator)
        {
            this.ModelEnumerator = ModelEnumerator;
            this.PropFormatter = PropFormatter;
        }
        public IModelEnumerator DeriveModelEnumerator(string ModelName)
        {
            this.ModelName = ModelName;
            if (ModelName.Contains("."))
                return new TypeEnumerator(Type.GetType(ModelName));
            else
                return new TableEnumerator(ModelName);
        }
        public string Format()
        {
            StringBuilder _result = new StringBuilder();
            foreach (AppModelItem prop in ModelEnumerator.Items())
            {
                if (PropFormatter != null)
                    _result.Append(PropFormatter.Format(prop));
                else
                    _result.Append(prop); 
            } 
            return  _result.ToString();
        }
    }
    public class TypePropFormatter : BasePropListFormatter
    {
        public TypePropFormatter(string type, IModelItemWrapper PropFormatter)
            : base(PropFormatter, new TypeEnumerator(Type.GetType(type))) { }
    }
    public class TablePropFormatter : BasePropListFormatter
    {
        public TablePropFormatter(string ModelName, IModelItemWrapper PropFormatter)
            : base(PropFormatter, new TableEnumerator(ModelName)) { }
    }
}
