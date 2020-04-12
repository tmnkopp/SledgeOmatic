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
using SOM.Formatters; 

namespace SOM.Compilers
{

    public class ModelCompiler :  ICompiler
    {
        ITypeFormatter<AppModelItem> _TypeFormatter;
        BaseTypeEnumerator<AppModelItem> _ModelEnumerator;
        string _ModelName; 
        public ModelCompiler(string ModelName, string TypeFormatter ) 
        {
            _ModelName = ModelName; 
            _TypeFormatter = (ITypeFormatter<AppModelItem>)Invoker.Invoke(TypeFormatter); 
            this._ModelEnumerator = DeriveModelEnumerator(ModelName);
        }
        public string Compile(string content)
        { 
            StringBuilder _result = new StringBuilder();
            foreach (AppModelItem item in _ModelEnumerator.Items) 
                _result.Append(_TypeFormatter.Format(item));
           
            return content.Replace($"[ModelCompile -{_ModelName} -{_TypeFormatter.GetType().Name}]", _result.ToString());
        }

        private BaseTypeEnumerator<AppModelItem> DeriveModelEnumerator(string ModelName)
        { 
            if (ModelName.Contains("."))
                return new TypeEnumerator(Type.GetType(_ModelName));
            else
                return new TableEnumerator(_ModelName);
        }


    }
     
}
