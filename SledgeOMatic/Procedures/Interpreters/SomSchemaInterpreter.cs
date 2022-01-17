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
        Func<AppModelItem, bool> schemaItemPredicate = item => item != null;
        public Func<AppModelItem, bool> SchemaItemPredicate
        {
            set { schemaItemPredicate = value; }
        }
        public string Compile(string content)
        {
            IParser<CommandParseResult> _Parser = new SomTagParser("SCHEMA"); 
            foreach (var parseresult in _Parser.Parse(content))
            {
                StringBuilder result = new StringBuilder(); 
                IEnumerable<AppModelItem>_AppModelItems = _SchemaProvider
                    .GetModel(parseresult.Options().Model.ToValidSchemaName() ?? _InitModel)
                    .AppModelItems.Select(schemaItemProjector)
                    .Where(schemaItemPredicate).AsEnumerable(); 

                if (parseresult.Options().Template != null)  { 
                    foreach (AppModelItem item in _AppModelItems) { 
                        string path = item.ToStringFormat(parseresult.Options().Template);
                        path = path.Replace("{1}", item.DataType); 
                        if (_Parser.ParseMode == ParseMode.Debug) Cache.Debug($"\n{path}");
                        string fmt = item.ToStringFormat(Reader.Read(path));  
                        result.Append(fmt); 
                    } 
                }
                else { 
                    foreach (AppModelItem item in _AppModelItems) {
                        result.Append(item.ToStringFormat(parseresult.Options().Format ?? "{0}"));
                    }   
                }
                content = content.Replace(parseresult.Parsed, result.ToString());
            }
            return content; 
        } 
    }
}
