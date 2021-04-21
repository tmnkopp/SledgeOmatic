using CommandLine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SOM;
using SOM.Compilers;
using SOM.Data;
using SOM.Extentions;
using SOM.IO;
using SOM.Models;
using SOM.Procedures;
using SOMAPI.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
namespace CoreTests
{
    [TestClass]
    public class FormCompilerTests
    {
        //ShowRow.aspx
        [TestMethod]
        public void ShowRow_Compiles()
        { 
            Cache.Write("");
            List<metric> ret = new DataProvider().GetData();
            foreach (var item in ret)
            {
                string src = "";
                using (TextReader tr = System.IO.File.OpenText(@"C:\_som\_src\ShowRow.aspx"))
                    src = tr.ReadToEnd();
                string id = item.IDTEXT.Replace(".", "_");
                if (id == "") id = "0" + item.SortPos;
                Cache.Append(string.Format(src, $"{id}" ));
            }
            Cache.Inspect();
         
        }


        [TestMethod]
        public void BODForm_Compiles()
        {
            List<metric> ret = new DataProvider().GetData();
            Cache.Write("");
            foreach (var item in ret)
            {
                string src = "";
                using (TextReader tr = System.IO.File.OpenText(@"C:\_som\_src\" + item.QuestionTypeCode + ".aspx"))
                    src = tr.ReadToEnd();
                string id = item.IDTEXT.Replace(".", "_");
                if (id == "") id = "0" + item.SortPos;

                string annote = $"IDTEXT:{id.Replace("_", ".")}\n\tQPK:{item.QPK}\n\tQTEXT:{item.QTEXT}"; 
                Cache.Append(string.Format(src, item.QPK, id, item.FK_PickList, item.QTEXT, annote, ""));
            }
            Cache.Inspect();
            Assert.IsNotNull(ret);
        } 
    }
 
    public class metric{
        public string QPK { get; set; }
        public string QuestionTypeCode { get; set; }
        public string FK_PickList { get; set; }
        public string IDTEXT { get; set; }
        public string SortPos { get; set; }
        public string QTEXT { get; set; }
    }
    public class DataProvider { 
        public List<metric> GetData() {
            var con = ConfigurationManager.ConnectionStrings["CSLITE"].ToString();
            SqlDataReader oReader;
            List<metric> ret = new List<metric>();
            using (SqlConnection myConnection = new SqlConnection(con))
            {
                SqlCommand oCmd = new SqlCommand(" SELECT QPK, QuestionTypeCode, FK_PickList, IDTEXT, SortPos, QTEXT FROM vw_MetricsCompositeKeys WHERE PK_FORM = '2021-A-HVA' AND SECTION_COUNT = 1", myConnection);
                myConnection.Open();
                using (oReader = oCmd.ExecuteReader())
                    while (oReader.Read())
                    {
                        ret.Add(new metric() {  
                            QPK=oReader["QPK"].ToString()
                            , QuestionTypeCode=oReader["QuestionTypeCode"].ToString()
                            , FK_PickList = oReader["FK_PickList"].ToString() 
                            , IDTEXT=oReader["IDTEXT"].ToString() 
                            , SortPos=oReader["SortPos"].ToString() 
                            , QTEXT = oReader["QTEXT"].ToString() 
                        }); 
                    }
                myConnection.Close();
            } 
            return ret;
        }
    }
}


