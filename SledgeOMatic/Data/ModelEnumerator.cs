using Newtonsoft.Json;
using SOM.Models;
using SOM.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace SOM.Procedures
{
    public abstract class BaseTypeEnumerator<T>
    {
        public List<T> Items { get => this.Enumerate().ToList() ;   }
        public abstract IEnumerable<T> Enumerate();
        public override string ToString()
        {
            return JsonConvert.SerializeObject(
                this.Enumerate().ToList()
            );
        }
    }

    public class TypeEnumerator: BaseTypeEnumerator<AppModelItem>
    {
        private Type _type ;
        public TypeEnumerator(Type type)
        {
            _type = type;
        }
        public override IEnumerable<AppModelItem> Enumerate() {
            int cnt = 0;
            foreach (var prop in _type.GetProperties())
            {
                AppModelItem _item = new AppModelItem()
                {
                    Name = prop.Name,
                    DataType = prop.PropertyType.ToString(),
                    OrdinalPosition = (cnt++)
                };
                yield return _item; 
            }
        }
    }
   
    public class TableEnumerator : BaseTypeEnumerator<AppModelItem>
    {
        private string _tablename;
        private readonly IConfiguration _config;
        public TableEnumerator(string TableName,
            IConfiguration configuration)
        {
            _config = configuration;
            _tablename =TableName;
        }
        public override IEnumerable<AppModelItem> Enumerate()
        {
            //var con = _config.GetSection("ConnectionStrings")["default"];
            var con = ConfigurationManager.ConnectionStrings["default"].ToString();
            using (SqlConnection myConnection = new SqlConnection(con))
            {
                string sql = $"SELECT COLUMN_NAME, DATA_TYPE, ORDINAL_POSITION, CHARACTER_MAXIMUM_LENGTH FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME  = '{_tablename}'";
                SqlCommand oCmd = new SqlCommand(sql, myConnection);
                SqlDataReader oReader;
                myConnection.Open();
                using (oReader = oCmd.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        AppModelItem _AppModelItem = GetAppModelItem(oReader);
                        yield return GetAppModelItem(oReader); 
                    } 
                    myConnection.Close();
                }
            } 
        } 
        private AppModelItem GetAppModelItem(SqlDataReader sdr) {
            AppModelItem _AppModelItem = new AppModelItem()
            {
                Name = sdr["COLUMN_NAME"].ToString(),
                DataType = sdr["DATA_TYPE"].ToString(), 
                OrdinalPosition = Convert.ToInt32(sdr["ORDINAL_POSITION"])
            }; 
            if (sdr["CHARACTER_MAXIMUM_LENGTH"] != DBNull.Value)
                _AppModelItem.MaxLen = Convert.ToInt32(sdr["CHARACTER_MAXIMUM_LENGTH"]);
            if (sdr.HasColumn("ControlType"))
                if (sdr["ControlType"] != DBNull.Value)
                    _AppModelItem.ControlType = sdr["ControlType"].ToString();

            return _AppModelItem;
        } 
  
    }
}
