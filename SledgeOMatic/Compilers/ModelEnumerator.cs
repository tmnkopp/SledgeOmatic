using SOM.Models;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOM.Compilers
{
    public interface IModelEnumerator {
        IEnumerable<AppModelItem> Items();
    }
    public class TypeEnumerator: IModelEnumerator
    {
        private Type _type ;
        public TypeEnumerator(Type type)
        {
            _type = type;
        }
        public IEnumerable<AppModelItem> Items() {
            int cnt = 0;
            foreach (var prop in _type.GetProperties())
            { 
                yield return new AppModelItem() {
                    Name= prop.Name,
                    DataType= prop.PropertyType.ToString(),
                    OrdinalPosition = (cnt++)
                };
            }
        }
    }
    public class TableEnumerator : IModelEnumerator
    {
        private string _tablename;
        public TableEnumerator(string TableName)
        {
            _tablename=TableName;
        }
        public IEnumerable<AppModelItem> Items()
        {
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
                        yield return GetAppModelItem(oReader); 
                    } 
                    myConnection.Close();
                }
            } 
        } 
        private AppModelItem GetAppModelItem(SqlDataReader oReader) {
            AppModelItem _AppModelItem = new AppModelItem()
            {
                Name = oReader["COLUMN_NAME"].ToString(),
                DataType = oReader["DATA_TYPE"].ToString(),
                OrdinalPosition = Convert.ToInt32(oReader["ORDINAL_POSITION"])
            };
            if (oReader["CHARACTER_MAXIMUM_LENGTH"] != DBNull.Value)
                _AppModelItem.MaxLen = Convert.ToInt32(oReader["CHARACTER_MAXIMUM_LENGTH"]);


            return _AppModelItem;
        } 
    }
}
