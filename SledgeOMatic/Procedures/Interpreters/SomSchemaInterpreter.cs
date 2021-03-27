using SOM.Data;
using SOM.Extentions;
using SOM.IO;
using SOM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
namespace SOM.Procedures
{
    public class SomSchemaInterpreter : ICompilable
    {
        private ISchemaProvider _SchemaProvider;
        private string _InitModel; 
        public SomSchemaInterpreter(
            ISchemaProvider SchemaProvider )
        {
            _SchemaProvider = SchemaProvider;
            _InitModel= SchemaProvider.Model.ModelName;
        }
        private Func<AppModelItem, AppModelItem> schemaItemProjector = app => app;
        public Func<AppModelItem, AppModelItem> SchemaItemProjector
        {
            set { schemaItemProjector = value; }
        } 
        Func<AppModelItem, bool> schemaItemFilter = item => item != null;
        public Func<AppModelItem, bool> SchemaItemFilter
        {
            set { schemaItemFilter = value; }
        }
        public string Compile(string content)
        {
            IParser<CommandParseResult> _Parser = new SomTagParser(ParseTag.SCHEMA); 
            foreach (var parseresult in _Parser.Parse(content))
            {
                StringBuilder result = new StringBuilder(); 
                IEnumerable<AppModelItem>_AppModelItems = _SchemaProvider
                    .GetModel(parseresult.Arguments.Model.ToValidSchemaName() ?? _InitModel)
                    .AppModelItems.Select(schemaItemProjector)
                    .Where(schemaItemFilter).AsEnumerable(); 

                if (parseresult.Arguments.Template != null)  { 
                    foreach (AppModelItem item in _AppModelItems) { 
                        string path = item.ToStringFormat(parseresult.Arguments.Template);
                        if (_Parser.ParseMode == ParseMode.Debug) Cache.Debug($"\n{path}");
                        string fmt = item.ToStringFormat(Reader.Read(path));  
                        result.Append(fmt); 
                    } 
                }
                else { 
                    foreach (AppModelItem item in _AppModelItems) {
                        result.Append(item.ToStringFormat(parseresult.Arguments.Format ?? "{0}"));
                    }   
                }
                content = content.Replace(parseresult.Result, result.ToString());
            }
            return content; 
        } 
    }
}
