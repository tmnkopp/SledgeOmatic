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

namespace SOM.Procedures.Data
{ 
    public class EntityMapper
    {  
        private StringBuilder result = new StringBuilder(); 
        private string _entityName = ""; 
        public string EntityName
        {
            get {
                if (_entityName == "")  { _entityName = _tablename;  }
                return _entityName;
            }
            set { _entityName = value; }
        } 
        private string _tablename = ""; 
        public string TableName
        {
            get { return _tablename; }
            set { _tablename = value; }
        }
        private string _wrapper = "";
        public string Wrapper
        {
            get { return _wrapper; }
            set { _wrapper = value; }
        }
         
        public EntityMapper(string TableName)
        { 
            _tablename = TableName;
        } 
        public string MapFields(IFieldMapStrategy fieldMapStrategy)
        {
            var con = ConfigurationManager.ConnectionStrings["default"].ToString();
            using (SqlConnection myConnection = new SqlConnection(con))
            {
                SqlCommand oCmd = new SqlCommand($"SELECT COLUMN_NAME, DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME  = '{_tablename}'", myConnection);
                SqlDataReader oReader;
                myConnection.Open();
                using (oReader = oCmd.ExecuteReader())
                { 
                    while (oReader.Read())
                    { 
                        result.AppendFormat("{0}{1}", "\n\t", fieldMapStrategy.Execute(
                             new SchemaField
                             {
                                 COLUMN_NAME = oReader["COLUMN_NAME"].ToString(),
                                 DATA_TYPE = oReader["DATA_TYPE"].ToString()
                             }
                        )); 
                    }
                    myConnection.Close();
                }
            }
            if (_wrapper == "") 
                _wrapper = fieldMapStrategy.Wrap; 
            string ret = _wrapper;
            ret = ret.Replace("[entityname]", EntityName);
            ret = ret.Replace("[fields]", result.ToString());
            return ret;
        }
    } 
}
 