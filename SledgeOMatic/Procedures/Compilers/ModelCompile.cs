using SOM.Data;
using SOM.IO;
using SOM.Procedures;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using SOM.Procedures.Data;

namespace SOM.Procedures
{
    public class ModelCompile : BaseModelCompile, IProcedure
    {
        string _tablename = "";
        IColumnConverter _ColumnConverter;
        Func<string, string, string> _ContentIntegrationMethod;
        public ModelCompile(string Tablename, IColumnConverter ColumnConverter, Func<string, string, string> ContentIntegrationMethod)
        : base( Tablename, ColumnConverter)
        {
            _tablename = TableName;
            _ColumnConverter = ColumnConverter;
            _ContentIntegrationMethod = ContentIntegrationMethod;
        }
        public string Execute(string content)
        {
            return _ContentIntegrationMethod(content, base.Compile()); 
        } 
    }
   
    public abstract class BaseModelCompile  {
     
        public IColumnConverter ColumnConverter;
        public string TableName { get; set; } 

        public BaseModelCompile(string Tablename, IColumnConverter ColumnConverter)
        {
            this.TableName = Tablename;
            this.ColumnConverter = ColumnConverter;
        }
        public string Compile()
        { 
            StringBuilder _result = new StringBuilder();
            var con = ConfigurationManager.ConnectionStrings["default"].ToString();
            using (SqlConnection myConnection = new SqlConnection(con))
            {
                SqlCommand oCmd = new SqlCommand($"SELECT COLUMN_NAME, DATA_TYPE, ORDINAL_POSITION FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME  = '{TableName}'", myConnection);
                SqlDataReader oReader;
                myConnection.Open();
                using (oReader = oCmd.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        _result.AppendFormat("{0}{1}", "\n\t", ColumnConverter.Convert(
                             new DBColumnDefinition
                             {
                                 COLUMN_NAME = oReader["COLUMN_NAME"].ToString(),
                                 DATA_TYPE = oReader["DATA_TYPE"].ToString(),
                                 ORDINAL_POSITION = Convert.ToInt32(oReader["ORDINAL_POSITION"])
                             }
                        ));
                    }

                    myConnection.Close();
                }
            }
            return _result.ToString();
        }
    } 
}
