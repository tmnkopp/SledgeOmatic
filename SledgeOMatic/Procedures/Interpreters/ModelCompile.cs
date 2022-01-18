using SOM.Data;
using SOM.Extentions;
using SOM.IO;
using SOM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SOM.Procedures
{
    public class ModelCompile : ICompilable
    {
        private ISchemaProvider _SchemaProvider; 
        private string _modelname; 
        private string _format;  
        private string _predpattern;  

        [CompilableCtorMeta()]
        public ModelCompile(string Model, string Format, string PredPattern )
        { 
            _SchemaProvider = new SchemaProvider(Model);
            _modelname = _SchemaProvider.Model.ModelName;
            _format = Format;
            _format = Regex.Replace(_format, @"\/n\s?", "\n") ;
            _format = Regex.Replace(_format, @"\/t\s?", "\t") ;
            _predpattern = PredPattern ?? ".*";
        } 
        public string Compile(string content)
        { 
            StringBuilder result = new StringBuilder(); 
            IEnumerable<AppModelItem>_AppModelItems = _SchemaProvider
                .GetModel(_modelname)
                .AppModelItems.Select(i => i)
                .Where(i => Regex.IsMatch(i.Name, _predpattern)).AsEnumerable(); 

            if (Regex.IsMatch(_format, $@"^\w:"))  { 
                foreach (AppModelItem item in _AppModelItems) { 
                    string path = item.ToStringFormat(_format);
                    path = path.Replace("{1}", item.DataType);  
                    string fmt = item.ToStringFormat(Reader.Read(path));  
                    result.Append(fmt); 
                } 
            }
            else { 
                foreach (AppModelItem item in _AppModelItems) {
                    result.Append(item.ToStringFormat(_format ?? "{0}"));
                }   
            }
            content = content.Replace("/r", result.ToString());
            return content; 
        } 
    }
}
