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
    public class TypeModelCompiler : BaseModelCompile, IProcedure
    {
        IModelEnumerator _ModelEnumerator;
        IColumnConverter _ColumnConverter;
        Func<string, string, string> _ContentIntegrator;
        public TypeModelCompiler(string type, IColumnConverter ColumnConverter, Func<string, string, string> ContentIntegrator)
            : base(ColumnConverter, new TypeEnumerator(Type.GetType(type)))
        {
            _ContentIntegrator = ContentIntegrator;
        }
        public string Execute(string content)
        {
            return _ContentIntegrator(content, base.Compile());
        }
    }
    public class TableModelCompiler : BaseModelCompile, IProcedure
    {
        IModelEnumerator _ModelEnumerator;
        IColumnConverter _ColumnConverter;
        Func<string, string, string> _ContentIntegrator;
        public TableModelCompiler(string ModelName, IColumnConverter ColumnConverter, Func<string, string, string> ContentIntegrator)
            : base(ColumnConverter, new TableEnumerator(ModelName))
        {
            _ContentIntegrator = ContentIntegrator;
        }
        public string Execute(string content)
        {
            return _ContentIntegrator(content, base.Compile());
        }
    }
    public class ModelCompile : BaseModelCompile, IProcedure
    {
        IModelEnumerator _ModelEnumerator;
        IColumnConverter _ColumnConverter;
        Func<string, string, string> _ContentIntegrator;
        public ModelCompile(IModelEnumerator ModelEnumerator, IColumnConverter ColumnConverter, Func<string, string, string> ContentIntegrator)
        : base(ColumnConverter, ModelEnumerator)
        {  
            _ContentIntegrator = ContentIntegrator;
        }
        public string Execute(string content)
        {
            return _ContentIntegrator(content, base.Compile()); 
        } 
    }
   
    public abstract class BaseModelCompile  {
     
        public IColumnConverter ColumnConverter;
        public IModelEnumerator ModelEnumerator; 

        public BaseModelCompile(IColumnConverter ColumnConverter, IModelEnumerator ModelEnumerator)
        {
            this.ModelEnumerator = ModelEnumerator;
            this.ColumnConverter = ColumnConverter;
        }
        public string Compile()
        {
            StringBuilder _result = new StringBuilder();
            foreach (PropDefinition prop in ModelEnumerator.Items())
            {
                _result.Append( ColumnConverter.Convert(  prop  ));
            } 
            return _result.ToString();
        }
    } 
}
