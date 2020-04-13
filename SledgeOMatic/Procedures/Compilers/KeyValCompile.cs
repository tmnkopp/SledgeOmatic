using SOM.Data;
using SOM.IO;
using SOM.Procedures;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SOM.Compilers
{
    public abstract class BaseKeyValCompile : ICompiler
    { 
        public Dictionary<string, string> Dict { get; set; } 
        public virtual string Compile(string compileme)
        {
            foreach (var item in Dict) { 
                    compileme = compileme.Replace(item.Key, item.Value); 
            }
            return compileme;        
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this.Dict, Formatting.Indented);
        }
    }
    public class JsonCompile : BaseKeyValCompile, ICompiler {
         
        public JsonCompile(string json)
        {
            base.Dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        }
    }
    public class SqlKeyValCompile : BaseKeyValCompile, ICompiler  {
        private string _sqlFileParam = "";  
        public SqlKeyValCompile(string sqlFile)
        { 
            IReader r = new FileReader(sqlFile);
            KeyValDBReader dbreader = new KeyValDBReader(r.Read()); 
            dbreader.ExecuteRead(); 
            base.Dict = dbreader.Data;
        }  
    } 
}
