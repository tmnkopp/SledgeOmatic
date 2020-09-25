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
using System.IO; 
namespace SOM.Procedures 
{
    public class ModelTemplateCompile : BaseModelCompiler, ICompiler 
    { 
        public ModelTemplateCompile()
        { 
        }
        public string Compile(string content)
        {
            StringBuilder result = new StringBuilder();
            string[] lines = content.Split('\n');
            foreach (var line in lines)
            {
                Match match = Regex.Match(line, @".*\[model:(?<model>.*) path:(?<path>.*).*\]");
                if (match.Success)
                {
                    GroupCollection groups = match.Groups;
                    string model = groups["model"].Value;
                    string path = groups["path"].Value.Replace("~", AppSettings.BasePath);
                    
                    string template = File.ReadAllText(path); 
                    foreach (var item in base.GetModelItems(model))
                    {
                        result.Append(template.Replace("$0", item.Name).Replace("$1", item.DataType));
                    }
                }  else  {
                    result.Append(line);
                } 
            }
            return result.ToString();
        }
    } 
    public abstract class BaseModelCompiler
    {  
        protected BaseTypeEnumerator<AppModelItem> _AppModelItems; 
        public BaseModelCompiler()
        {  
        }  
        protected IEnumerable<AppModelItem> GetModelItems(string ModelName)
        {
            BaseTypeEnumerator<AppModelItem> itemEnumerator;
            if (ModelName.Contains("."))
                itemEnumerator = new TypeEnumerator(Type.GetType(ModelName));
            else
                itemEnumerator = new TableEnumerator(ModelName);

            foreach (var item in itemEnumerator.Enumerate())
            {
                yield return item;
            } 
        } 
    }

    public class ModelItemCompiler : ICompiler
    {
        AppModel _AppModel = new AppModel();
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


}
