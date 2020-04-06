using SOM.Procedures.Data;
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
        IEnumerable<PropDefinition> Items();
    }
    public class TypeEnumerator: IModelEnumerator
    {
        private Type _type ;
        public TypeEnumerator(Type type)
        {
            _type = type;
        }
        public IEnumerable<PropDefinition> Items() {
            int cnt = 0;
            foreach (var prop in _type.GetProperties())
            { 
                yield return new PropDefinition() {
                    NAME = prop.Name,
                    DATA_TYPE= prop.PropertyType.ToString(),
                    ORDINAL_POSITION= (cnt++)
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
        public IEnumerable<PropDefinition> Items()
        {
            var con = ConfigurationManager.ConnectionStrings["default"].ToString();
            using (SqlConnection myConnection = new SqlConnection(con))
            {
                SqlCommand oCmd = new SqlCommand($"SELECT COLUMN_NAME, DATA_TYPE, ORDINAL_POSITION FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME  = '{_tablename}'", myConnection);
                SqlDataReader oReader;
                myConnection.Open();
                using (oReader = oCmd.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        yield return new PropDefinition()
                        {
                            NAME = oReader["COLUMN_NAME"].ToString(),
                            DATA_TYPE = oReader["DATA_TYPE"].ToString(),
                            ORDINAL_POSITION = Convert.ToInt32(oReader["ORDINAL_POSITION"])
                        };
                    } 
                    myConnection.Close();
                }
            } 
        }
    }
}
