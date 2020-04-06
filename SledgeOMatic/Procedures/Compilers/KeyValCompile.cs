using SOM.Data;
using SOM.IO;
using SOM.Procedures;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SOM.Procedures
{
    public abstract class KeyValCompile : IProcedure
    { 
        public Dictionary<string, string> Dict { get; set; } 
        public virtual string Execute(string compileme)
        {
            foreach (var item in Dict) { 
                    compileme = compileme.Replace(item.Key, item.Value); 
            }
            return compileme;        
        }
        public override string ToString()
        {
            return $"{base.ToString()} -{Dict.ToString()}";
        }
    }
    public class JsonCompile : KeyValCompile, IProcedure {
         
        public JsonCompile(string json)
        {
            base.Dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        }
    }
    public class SqlKeyValCompile : KeyValCompile, IProcedure  {
        private string _sqlFileParam = "";  
        public SqlKeyValCompile(string sqlFile)
        { 
            IReader r = new FileReader(sqlFile);
            KeyValDBReader dbreader = new KeyValDBReader(r.Read()); 
            dbreader.ExecuteRead(); 
            base.Dict = dbreader.Data;
        }
        public override string Execute(string compile)
        {
            return base.Execute(compile);
        }
        public override string ToString()
        {
            return $"SqlKeyValCompile -{_sqlFileParam.ToString()}";
        }
    } 
}
