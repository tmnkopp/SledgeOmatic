using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOM.Data
{ 
    public static class DataExtensions
    {
        public static bool HasColumn(this SqlDataReader dr, string columnName)
        {
            var fieldNames = Enumerable.Range(0, dr.FieldCount).Select(i => dr.GetName(i)).ToArray();
            return fieldNames.Contains(columnName);
        }
    }
    public abstract class DBReader<T>
    {
        public virtual T Data { get; set; }
        public abstract void UnloadReader();
        
        public SqlDataReader oReader { get; set; } 
        public virtual void ExecuteSql(string sSql)
        {  
            var con = ConfigurationManager.ConnectionStrings["default"].ToString();
            using (SqlConnection myConnection = new SqlConnection(con))
            {
                SqlCommand oCmd = new SqlCommand(sSql, myConnection);
                myConnection.Open(); 
                using (oReader = oCmd.ExecuteReader()) 
                    while (oReader.Read()) 
                        UnloadReader(); 
                    myConnection.Close(); 
            }
        } 

    }
    public interface IDBReader {
        void ExecuteRead();
    }

    public class KeyValDBReader : DBReader<Dictionary<string, string>>, IDBReader
    {
        private string _sql;
        private Dictionary<string, string> data = new Dictionary<string, string>();  
        public override Dictionary<string, string> Data
        {
            get { return data; }
            set { data = value; }
        } 
        public KeyValDBReader(string sSql)
        {
            _sql = sSql;
        }  
        public override void UnloadReader()
        { 
            try
            {
                Data.Add(base.oReader[0].ToString(), base.oReader[1].ToString());
            }
            catch (Exception)//just skip dupes
            {
            } 
        } 
        public void ExecuteRead()
        {
            base.ExecuteSql(_sql);
        }
    }
}
 