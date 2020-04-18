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
using System.Text.RegularExpressions;

namespace SOM.Procedures
{ 
    public class ModelCompile :  ICompiler, Injectable
    {
        private string _InjectableExpression;

        public string InjectableExpression
        {
            get { return _InjectableExpression; }
            set { _InjectableExpression = value; }
        } 
        ITypeFormatter<AppModelItem> _TypeFormatter;
        BaseTypeEnumerator<AppModelItem> _ModelEnumerator;
        string _ModelName;
         
        public ModelCompile(string ModelName, string TypeFormatter)
        {
            _ModelName = ModelName;
            _TypeFormatter = (ITypeFormatter<AppModelItem>)Invoker.InvokeProcedure(TypeFormatter);
            this._ModelEnumerator = DeriveModelEnumerator(ModelName);
            InjectableExpression = $"[ModelCompile -{this._ModelName} -{this._TypeFormatter.GetType().Name}]";
        }
  
        public string Compile(string content)
        { 
            StringBuilder _FormattedModelItem = new StringBuilder();
            foreach (AppModelItem item in _ModelEnumerator.Items) 
                _FormattedModelItem.Append($"{_TypeFormatter.Format(item)}\n");
             
            return content.Replace(  this.InjectableExpression , _FormattedModelItem.ToString()  );
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
