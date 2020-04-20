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
using System.Reflection;

namespace SOM.Procedures

{
    public class ModelItemCompiler : ICompiler
    { 
        AppModel _AppModel=new AppModel();
        string _modelname = "";
        string _format = "";
        public ModelItemCompiler(string ModelName, string Format)
        {
            _modelname = ModelName;
            _format = Format;
        }
        public string Compile(string content)
        {
            AppModel _AppModel = JsonConvert.DeserializeObject<AppModel>(_modelname);
            StringBuilder _result = new StringBuilder(); 
            foreach (AppModelItem item in _AppModel.AppModelItems)
            {
                StringBuilder _formatted = new StringBuilder();  
               _formatted.Append(_format.Replace("${Name}", item.Name));
               _formatted.Append(_format.Replace("${DataType}", item.DataType)); 
               _result.Append($"{_formatted.ToString()}\n");
            }
            return _result.ToString();
        }
    }
    public class ModelCompile :  ICompiler, Injectable
    {
        private string _InjectableExpression;

        public string InjectableExpression
        {
            get { return _InjectableExpression; }
            set { _InjectableExpression = value; }
        }  
        BaseTypeEnumerator<AppModelItem> _ModelEnumerator;
        string _ModelName; 
        public ModelCompile(string ModelName )
        {
            _ModelName = ModelName; 
            this._ModelEnumerator = DeriveModelEnumerator(ModelName);
            InjectableExpression = $"[ModelCompile -{this._ModelName}]";
        }
  
        public string Compile(string content)
        {  
            AppModel appModel = new AppModel()  {
                ModelName = _ModelName, 
                AppModelItems =new List<AppModelItem>()
            };
            foreach (AppModelItem item in _ModelEnumerator.Items) {
                appModel.AppModelItems.Add(item); 
            }
            string json = JsonConvert.SerializeObject(appModel, Formatting.None);
            return content.Replace(  this.InjectableExpression , $"{json}"  );
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
