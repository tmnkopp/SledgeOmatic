using SOM.Data;
using SOM.IO;
using SOM.Procedures;
using Newtonsoft.Json;
using System;
using System.Collections.Generic; 
using Newtonsoft.Json.Linq; 
using System.IO;
using System.Data.SqlClient;

namespace SOM.Procedures
{ 
    public class KeyValReplacer : BaseCompiler, ICompilable
    {
        public string Source { get; set; }
        public KeyValReplacer()
        {

        }
        public KeyValReplacer(string Source)
        {
            this.Source = Source; 
        } 
        protected Dictionary<string, string> PopulateKeyVals(ISomContext somContext)
        {
            string src = "";
            Dictionary<string, string> KeyVals = new Dictionary<string, string>();
            if (Source.ToLower().EndsWith(".json"))
            {
                Source = Source.Replace("~", somContext.BasePath);
                using (TextReader tr = File.OpenText(Source))
                    src = tr.ReadToEnd();
                KeyVals = JsonConvert.DeserializeObject<Dictionary<string, string>>(src);
                return KeyVals;
            }
            if (Source.ToLower().EndsWith(".sql"))
            {
                Source = Source.Replace("~", somContext.BasePath);
                using (TextReader tr = File.OpenText(Source))
                    src = tr.ReadToEnd();

                var con = somContext.Config.GetSection("ConnectionStrings")["default"];
                using (SqlConnection conn = new SqlConnection(con))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(src, conn); 
                    using (SqlDataReader read = cmd.ExecuteReader())
                        while (read.Read()){
                            if (!KeyVals.ContainsKey(read[0].ToString())) 
                                KeyVals.Add(read[0].ToString(), read[1].ToString()); 
                        }        
                }
                return KeyVals;
            }
            if (this.IsValidJson(Source))
            {
                KeyVals = JsonConvert.DeserializeObject<Dictionary<string, string>>(Source);
            }
            return KeyVals;
        }
        protected bool IsValidJson(string strInput)
        {
            strInput = strInput.Trim(); 
            try
            {
                var obj = JToken.Parse(strInput);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }  
        }
        public virtual string Compile(ISomContext somContext)
        {
            string content = somContext.Content;
            var keyvals = PopulateKeyVals(somContext);
            foreach (var item in keyvals)
            {
                content = content.Replace(item.Key, item.Value);
            }
            return content;
        }
    } 
}
