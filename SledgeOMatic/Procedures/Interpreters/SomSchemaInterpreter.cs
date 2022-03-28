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
        #region FIELDS

        private ISchemaProvider _SchemaProvider;
        private string _InitModel;

        #endregion

        #region PROPS

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

        #endregion

        #region CTOR

        public SomSchemaInterpreter(
            ISchemaProvider SchemaProvider)
        {
            _SchemaProvider = SchemaProvider;
            _InitModel = SchemaProvider.Model.ModelName;
        }

        #endregion

        #region METHODS

        public string Compile(ISomContext somContext)
        {
            string content = somContext.Content;
            IParser<CommandParseResult> _Parser = new SomTagParser("SCHEMA");
            foreach (var parseresult in _Parser.Parse(content))
            {
                StringBuilder result = new StringBuilder();
                IEnumerable<AppModelItem> _AppModelItems = _SchemaProvider
                    .GetModel(parseresult.Options.Model.ToValidSchemaName() ?? _InitModel)
                    .AppModelItems.Select(schemaItemProjector)
                    .Where(schemaItemPredicate).AsEnumerable();

                if (parseresult.Options.Template != null)
                {
                    foreach (AppModelItem item in _AppModelItems)
                    {
                        string path = item.ToStringFormat(parseresult.Options.Template);
                        path = path.Replace("{1}", item.DataType);
                        if (_Parser.ParseMode == ParseMode.Debug) Cache.Debug($"\n{path}");
                        string fmt = item.ToStringFormat(Reader.Read(path));
                        result.Append(fmt);
                    }
                }
                else
                {
                    foreach (AppModelItem item in _AppModelItems)
                    {
                        result.Append(item.ToStringFormat(parseresult.Options.Format ?? "{0}"));
                    }
                }
                content = content.Replace(parseresult.Parsed, result.ToString());
            }
            return content;
        }

        #endregion
    }
}
