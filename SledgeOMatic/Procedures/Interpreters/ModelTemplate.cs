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
using Microsoft.Extensions.Configuration;

namespace SOM.Procedures 
{
    public class ModelTemplateInterpreter : BaseModelInterpreter, IInterpreter 
    {
        private readonly IConfiguration _config;
        public ModelTemplateInterpreter(IConfiguration configuration)
        {
            _config = configuration;
        }
        public string Interpret(string content)
        {
            StringBuilder result = new StringBuilder();
            string[] lines = content.Split('\n');
            foreach (var line in lines)
            {
                Match match = Regex.Match(line, @".*som: -m (?<model>.*) -p (?<path>.*).*:som");
                if (match.Success)
                {
                    GroupCollection groups = match.Groups;
                    string model = groups["model"].Value;
                    string path = groups["path"].Value.Replace("~", AppSettings.BasePath);
                    
                    string template = File.ReadAllText(path); 
                    foreach (var modelItem in base.GetModelItems(model, _config))
                    {
                        result.Append(template.Replace("$0", modelItem.Name).Replace("$1", modelItem.DataType));
                    }
                }  else  {
                    result.Append(line);
                } 
            }
            return result.ToString();
        }
    } 
    public abstract class BaseModelInterpreter
    {  
        protected BaseTypeEnumerator<AppModelItem> _AppModelItems;  
        protected IEnumerable<AppModelItem> GetModelItems(string ModelName, IConfiguration _config)
        {
            BaseTypeEnumerator<AppModelItem> itemEnumerator;
            if (ModelName.Contains("."))
                itemEnumerator = new TypeEnumerator(Type.GetType(ModelName));
            else
                itemEnumerator = new TableEnumerator(ModelName, _config);

            foreach (var item in itemEnumerator.Enumerate())
            {
                yield return item;
            } 
        } 
    }

    public class ModelItemCompiler : IInterpreter
    {
        AppModel _AppModel = new AppModel();
        string _modelname = "";
        string _format = "";
        public ModelItemCompiler(string ModelName, string Format)
        {
            _modelname = ModelName;
            _format = Format;
        }
        public string Interpret(string content)
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
