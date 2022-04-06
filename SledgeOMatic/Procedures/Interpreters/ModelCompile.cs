using SOM.Data;
using SOM.Extentions;
using SOM.IO;
using SOM.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SOM.Procedures
{
    public class ModelCompile : BaseCompiler, ICompilable
    {
        #region FIELDS

        private ISchemaProvider _SchemaProvider;
        private string _format;
        private string _modelName;
        private string _predpattern;

        #endregion

        #region CTOR

        [CompilableCtorMeta()]
        public ModelCompile(string Model, string PredPattern)
        {
            this._modelName = Model;
            this._predpattern = PredPattern ?? ".*";
        }

        #endregion

        #region METHODS

        public string Compile(ISomContext somContext)
        {
            string content = somContext.Content;
            StringBuilder result = new StringBuilder();

            _SchemaProvider = new SchemaProvider(somContext.Config);
            _SchemaProvider.LoadModel(this._modelName);

            var lines = (from s in Regex.Split(content, $@"\r|\n")
                         where !string.IsNullOrWhiteSpace(s)
                         select s).ToList();

            var prefix = lines[0];
            var postfix = lines[lines.Count() - 1];

            _format = string.Join(' ', lines).Replace(prefix, "").Replace(postfix, "");
            _format = Regex.Replace(_format, @"\s\/n", "\n");
            _format = Regex.Replace(_format, @"\s\/t", "\t");

            IEnumerable<AppModelItem> _AppModelItems = _SchemaProvider
                .AppModelItems.Select(i => i)
                .Where(i => Regex.IsMatch(i.Name, _predpattern)).AsEnumerable();

            if (Regex.IsMatch(_format, $@"^.*\w:\\"))
            {
                foreach (AppModelItem item in _AppModelItems)
                {
                    string path = item.ToStringFormat(_format);
                    path = path.Replace("{1}", item.DataType);
                    path = path.Replace("~", somContext.BasePath); 
                    string rte = "";
                    using (TextReader tr = File.OpenText(path))
                        rte = tr.ReadToEnd();
                    string fmt = item.ToStringFormat(rte);
                    result.Append(fmt);
                }
            }
            else
            {
                foreach (AppModelItem item in _AppModelItems)
                {
                    _format = (string.IsNullOrWhiteSpace(_format)) ? "{0}" : _format;
                    result.Append(item.ToStringFormat(_format));
                }
            }
            content = prefix + "\n" + result.ToString() + "\n" + postfix;
            return content;
        }

        #endregion
    }
}
