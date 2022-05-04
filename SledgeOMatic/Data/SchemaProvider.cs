using Microsoft.Extensions.Configuration;
using SOM.Data;
using SOM.Models;
using SOM.Procedures;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SOM.Data
{
    public interface ISchemaProvider
    {
        AppModel Model { get; }
        List<AppModelItem> AppModelItems { get; } 
        AppModel GetModel(string ModelName);
        void LoadModel(string ModelName);
        IEnumerable<string> GetTables(string Filter);
    }
    public class SchemaProvider : ISchemaProvider
    {
        private readonly IConfiguration _config; 
        public SchemaProvider(
            IConfiguration configuration)
        {
            _config = configuration; 
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
            _AppModelItems = new TableEnumerator(ModelName, _config).Items;
            _model = new AppModel()
            {
                ModelName = ModelName,
                AppModelItems = _AppModelItems
            }; 
        }
        public IEnumerable<string> GetTables(string Filter)
        {
            List<string> tables = new List<string>();
            string sql = $" SELECT CONVERT(NVARCHAR(5), ROW_NUMBER() OVER (ORDER BY TABLE_NAME))  K, TABLE_NAME V FROM INFORMATION_SCHEMA.TABLES ";
            if (Filter != "")  sql += $" WHERE TABLE_NAME LIKE '%{Filter}%' ";
             
            using (SqlConnection conn = new SqlConnection(_config.GetSection("ConnectionStrings")["default"]))
            {
                conn.Open();
                SqlCommand oCmd = new SqlCommand(sql, conn);
                SqlDataReader oReader; 
                using (oReader = oCmd.ExecuteReader())
                {
                    while (oReader.Read())
                        tables.Add(oReader["V"].ToString()); 
                }
            } 
            return tables.ToList();
        }
    }
}
