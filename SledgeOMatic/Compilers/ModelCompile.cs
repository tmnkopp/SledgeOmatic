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
using SOM.Procedures.Data;

namespace SOM.Compilers
{

    public class ModelCompile : BaseModelCompile 
    {
        public ModelCompile(string ModelName)
        {
            this.ModelEnumerator = DeriveEnumerator(ModelName);
        }
        public ModelCompile(string ModelName, IPropFormatter PropFormatter) 
        {
            this.ModelEnumerator = DeriveEnumerator(ModelName);
            this.PropFormatter = PropFormatter; 
        }
        private IModelEnumerator DeriveEnumerator(string ModelName) {
            if (ModelName.Contains("."))
                return new TypeEnumerator(Type.GetType(ModelName));
            else
                return new TableEnumerator(ModelName);
        }
    }
    public class TypeModelCompiler : BaseModelCompile
    {
        public TypeModelCompiler(string type, IPropFormatter PropFormatter)
            : base(PropFormatter, new TypeEnumerator(Type.GetType(type)))   {   }
    }
    public class TableModelCompiler : BaseModelCompile
    {
        public TableModelCompiler(string ModelName, IPropFormatter PropFormatter)
            : base(PropFormatter, new TableEnumerator(ModelName))   {   }
    }
    public abstract class BaseModelCompile  {
     
        public IPropFormatter PropFormatter;
        public IModelEnumerator ModelEnumerator;
        public BaseModelCompile()
        { 
        } 
        public BaseModelCompile(IPropFormatter PropFormatter, IModelEnumerator ModelEnumerator)
        {
            this.ModelEnumerator = ModelEnumerator;
            this.PropFormatter = PropFormatter;
        }
        public string Compile()
        {
            StringBuilder _result = new StringBuilder();
            foreach (PropDefinition prop in ModelEnumerator.Items())
            {
                if (PropFormatter != null)
                    _result.Append(PropFormatter.Format(prop));
                else
                    _result.Append(prop); 
            } 
            return  _result.ToString();
        }
    } 
}
