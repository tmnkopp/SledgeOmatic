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

namespace CoreTests
{ 
    [TestClass]
    public class Bootstrapper
    {
        [TestMethod]
        public void DestDirCreated()
        {
            //  dynamic d = new ExpandoObject(); 
            //  d.id = "id";
            //  d.qtext = "123";  
            //  ((IDictionary<string, object>)d)["this"] = "tahat";
     
            ParseContext context = new ParseContext();
            DataTable ws = new SheetProvider(context).Sheet;
            ws = new BOD1802QuestionnairePicklistParser(context).Parse(ws);
            ws = new DataTableIndexer(context).Parse(ws);
            
            string s = JsonConvert.SerializeObject(ws);
            Console.Write(s);
        }
    }
    public class DataTableIndexer : IDataTableParser
    {
        public DataTableIndexer(ParseContext context)
        { 
        }
        public DataTable Parse(DataTable ws)
        {
            index[] indexes = new[] { new index (){ name="PKEY", on="ID", @using="gap", gapsize = 50, start=1000 }}; 
            foreach (index index in indexes)
            { 
                if (!ws.Columns.Contains(index.name)) ws.Columns.Add(index.name, typeof(int));
                string priv = ws.Rows[0][index.on].ToString();
                int idx = index.start; 
                for (int irow = 0; irow < ws.Rows.Count; irow++)
                {  
                    if (priv != ws.Rows[irow][index.on].ToString())
                    {
                        priv = ws.Rows[irow][index.name].ToString();
                        if (index.gapsize > 0)  {
                            idx += index.gapsize;
                        }
                        else if(index.@using.Contains("reset"))  {
                            idx = 0;
                        }   
                    } 
                    idx += 1;
                    ws.Rows[irow][index.name] = idx-1;
                } 
            }
            return ws;
        } 
        private class index{
            public string name { get; set; }
            public string on { get; set; }
            public string @using { get; set; } 
            public int start { get; set; }
            public int gapsize { get; set; } = 0; 
        }
    }
    public class BOD1802QuestionnairePicklistParser : IDataTableParser
    {
        private int irow = 0;
        private int destrowcnt = 1;
        private DataTable ws;
        private ParseContext context;
        public BOD1802QuestionnairePicklistParser(ParseContext context)
        {
            this.context = context;
        }
        public DataTable Parse(DataTable ws)
        {
            ws = new BOD1802QuestionnaireParser(context).Parse(ws);
            DataTable dest = new DataTable();
            dest.Columns.Add("row", typeof(int));
            dest.Columns.Add("id", typeof(string));
            dest.Columns.Add("text", typeof(string)); 
            dest.Columns.Add("code", typeof(string));  
            dest.Columns.Add("sort", typeof(int));  
            for (irow = 0; irow < ws.Rows.Count; irow++)
            {
                if (((List<string>)ws.Rows[irow]["selections"]).Count <=1) continue;
                int sort = 0;
                foreach (string item in ((List<string>)ws.Rows[irow]["selections"]))
                {
                    dest.Rows.Add(destrowcnt++, ws.Rows[irow]["id"].ToString(), item, _get_item_code(item), sort++);
                } 
            } 
            return dest;
        } 
        private string _get_item_code(string s){
            s = Regex.Replace(s.ToUpper(), @$"[^A-Z0-9]", "");
            s = Regex.Replace(s, @$"[AEIOU]", "");
            s = s.Replace(" ", "_");
            if (Regex.IsMatch(s, "^\\d")) s = $"N{s}";
            if (s.Length > 20)
                s = s.Substring(0, 20);
            return s;
        }
    }
    public class BOD1802QuestionnaireParser : IDataTableParser
    {
        private int irow = 0; 
        private DataTable ws; 
        public BOD1802QuestionnaireParser(ParseContext context)
        { 
        }
        public DataTable Parse(DataTable ws)
        { 
            this.ws = ws;
            DataTable dest = new DataTable();  
            dest.Columns.Add("row", typeof(string));
            dest.Columns.Add("id", typeof(string));
            dest.Columns.Add("text", typeof(string));
            dest.Columns.Add("selections", typeof(List<string>));
            dest.Columns.Add("type", typeof(string)); 
            List<string> selections = new List<string>();  
            for (irow = 0; irow < ws.Rows.Count; irow++)
            { 
                if (Regex.Match(ws.Rows[irow][0].ToString(), $@"CQ\d.*", RegexOptions.IgnoreCase).Success)
                { 
                    selections = new List<string>();
                    selections.Add(ws.Rows[irow][3].ToString()); 
                    dest.Rows.Add(dest.Rows.Count + 1, ws.Rows[irow][0].ToString(), ws.Rows[irow][1].ToString(), selections, _get_type());
                }
                else{
                    if (dest.Rows.Count > 0)
                        ((List<string>)dest.Rows[dest.Rows.Count - 1]["selections"]).Add(ws.Rows[irow][3].ToString().Trim());   
                }
            }
            
            return dest;
        }
        private string _get_type()
        {
            if (Regex.Match(ws.Rows[irow][3].ToString().ToLower(), $@".*date.*").Success)
                return "DATE";
            return (Regex.Match(ws.Rows[irow][3].ToString(), $@".*free text.*", RegexOptions.IgnoreCase).Success) ? "TXT" : "PICK";
        } 
    }
    public interface IDataTableParser {
        DataTable Parse(DataTable ws);
    }
   
    public class SheetProvider{
        private DataTable _ws;
        public DataTable Sheet { 
            get{
                if (_ws == null)
                    _ws = getData();
                return _ws;
            }  
        } 
        public SheetProvider(ParseContext context)
        { 
        } 
        private DataTable getData(){
            var fileName = "C:\\Users\\Tim\\Documents\\1802\\Questionnaire\\Custodian.xlsx";
            var connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0}; Extended Properties=Excel 12.0;", fileName);
            var adapter = new OleDbDataAdapter("SELECT * FROM [Custodian Questionnaire$]", connectionString);
            var ds = new DataSet();
            adapter.Fill(ds, "1");
            _ws = ds.Tables["1"];
            return _ws;
        }
    }
    public class ParseContext{
        public string Source { get; set; } = "C:\\Users\\Tim\\Documents\\1802\\Questionnaire\\Custodian.xlsx";
        public string Sheeet { get; set; } = "Custodian Questionnaire";
        public List<string> Templates { get; set; }
        public List<object> Indexes { get; set; }
        public DataTable ws { get; set; }
    }
}
