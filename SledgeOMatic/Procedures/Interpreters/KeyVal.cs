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
    public abstract class BaseKeyValInterpreter : IInterpreter
    { 
        public Dictionary<string, string> KeyVals { get; set; } 
        public virtual string Interpret(string content)
        {
            foreach (var item in KeyVals) { 
                    content = content.Replace(item.Key, item.Value); 
            }
            return content;        
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this.KeyVals, Formatting.Indented);
        }
    } 
    public class KeyValInterpreter : BaseKeyValInterpreter, IInterpreter
    {
        public KeyValInterpreter(Dictionary<string, string> Dict)
        {
            this.KeyVals = Dict; 
        }
        public KeyValInterpreter(string json)
        {
            if (json != "")
                this.KeyVals = JsonConvert.DeserializeObject<Dictionary<string, string>>(json); 
        }
    }
    public class SqlKeyValInterpreter : BaseKeyValInterpreter, IInterpreter  { 
        public SqlKeyValInterpreter(string sqlFile)
        { 
            IReader r = new FileReader(sqlFile);
            KeyValDBReader dbreader = new KeyValDBReader(r.Read()); 
            dbreader.ExecuteRead(); 
            base.KeyVals = dbreader.Data;
        }  
    } 
}
