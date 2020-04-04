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
    public class DBColumnConverter: IProcedure
    {  
        private StringBuilder _result = new StringBuilder();
        private IColumnConvertStrategy _ConvertStrategy;
        private Func<string, string, string> _ContentReplacement;
        public DBColumnConverter(string TableName, IColumnConvertStrategy ConversionStrategy)
        {
            _tablename = TableName;
            _ConvertStrategy = ConversionStrategy; 
        }
        public string Execute(string content)
        {
            string result = IterateColumns();
            //content = _ContentReplacement(content, result);
            return result;
        }

        #region Props 
        private string _entityName = "";
        public string EntityName
        {
            get
            {
                if (_entityName == "") { _entityName = _tablename; }
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
        #endregion
        public string IterateColumns()
        {
            var con = ConfigurationManager.ConnectionStrings["default"].ToString();
            using (SqlConnection myConnection = new SqlConnection(con))
            {
                SqlCommand oCmd = new SqlCommand($"SELECT COLUMN_NAME, DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME  = '{_tablename}'", myConnection);
                SqlDataReader oReader;
                try
                {
                    myConnection.Open();
                }
                catch (Exception ex)
                {
                    throw;
                    Console.Write(ex.InnerException);
                    Console.Write(ex.Message);
               
                    
                }
                
                using (oReader = oCmd.ExecuteReader())
                { 
                    while (oReader.Read())
                    { 
                        _result.AppendFormat("{0}{1}", "\n\t", _ConvertStrategy.Convert(
                             new DBColumnDefinition
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
                _wrapper = _ConvertStrategy.Wrap; 
            string ret = _wrapper;
            ret = ret.Replace("[entityname]", EntityName);
            ret = ret.Replace("[fields]", _result.ToString());
            return ret;
        }


    } 
}
 