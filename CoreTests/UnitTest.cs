using Microsoft.VisualStudio.TestTools.UnitTesting; 
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Dynamic;
using System.Linq;
using static System.Net.WebRequestMethods;
using System.Data.SqlClient;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Extensions.Configuration;
using SOM;

namespace CoreTests
{  
    [TestClass]
    public class Bootstrapper
    { 
        public Bootstrapper()
        { 
        }
        [TestMethod]
        public void DBConnector()
        {
            string _filePath = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory);
            _filePath = Regex.Match(_filePath, "(.*SledgeOmatic\\\\).*").Groups[1].Value + "SledgeOMatic\\Tasks\\keys.sql"; 
            string sql = System.IO.File.ReadAllText(_filePath);

            var _config = new TestServices().Configuration;
            var dt = new DataTableProvider(_config).Provide(sql);
            var dict = new Dictionary<string, string>();
            foreach (DataColumn c in dt.Columns){ 
                dict.Add(c.ColumnName, dt.Rows[0][c].ToString());
            }
            var ser = JsonConvert.SerializeObject(dict);
            dynamic d = (dynamic)ser; 
            Console.Write(ser);
        } 
    }

   

}
