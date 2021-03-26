using Microsoft.Extensions.Configuration;
using SOM.Data;
using SOM.Models;
using SOM.Procedures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SOM.Data
{
    public interface ISchemaProvider
    {
        AppModel Model { get; }
        List<AppModelItem> AppModelItems { get; }
        IEnumerable<string> GetTables(string Filter);
        AppModel GetModel(string ModelName); 
    }
    public class SchemaProvider : ISchemaProvider
    {
        private readonly IConfiguration _config;
        private string con;
        public SchemaProvider(
            IConfiguration configuration)
        {
            _config = configuration; 
        }
        public SchemaProvider(string ModelName)
        {
             LoadModel(ModelName);
        }
        AppModel _model;
        public AppModel Model { get { return _model; } }
        public AppModel GetModel (string ModelName)
        {
            LoadModel(ModelName);
            return _model;
        }
        List<AppModelItem> _AppModelItems = new List<AppModelItem>();
        public List<AppModelItem> AppModelItems
        {
            get { return this._AppModelItems; }
        }
        public void LoadModel(string ModelName)
        {
            if (ModelName.Contains("."))  {
                _AppModelItems = new TypeEnumerator(Type.GetType(ModelName)).Items;
                ModelName = ModelName.Split(".")[ModelName.Split(".").Length - 1];
            }
            else
                _AppModelItems = new TableEnumerator(ModelName, _config).Items;
            _model = new AppModel()  {
                ModelName = ModelName,
                AppModelItems = _AppModelItems
            };
        }
        public IEnumerable<string> GetTables(string Filter)
        {
            string sql = $" SELECT CONVERT(NVARCHAR(5), ROW_NUMBER() OVER (ORDER BY TABLE_NAME))  K, TABLE_NAME V FROM INFORMATION_SCHEMA.TABLES ";
            if (Filter != "")
                sql += $" WHERE TABLE_NAME LIKE '%{Filter}%' ";
            KeyValDBReader reader = new KeyValDBReader(sql);
            reader.ExecuteRead();
            return reader.Data.Values.ToList();
        }
    }
}
