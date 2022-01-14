using SOM.Data;
using SOM.IO;
using SOM.Procedures;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SOM.Extentions;
namespace SOM.Procedures
{
    #region Base
    public abstract class BaseKeyValReplacer 
    {
        protected Dictionary<string, string> KeyVals { get; set; }
        public BaseKeyValReplacer()
        {
        }
        protected bool IsValidJson(string strInput)
        {
            if (string.IsNullOrWhiteSpace(strInput)) { return false; }
            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
                (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
            {
                try
                {
                    var obj = JToken.Parse(strInput);
                    return true;
                }
                catch (JsonReaderException jex)
                {
                    Console.WriteLine(jex.Message);
                    return false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public virtual string Compile(string content)
        {
            foreach (var item in KeyVals)
            {
                content = content.Replace(item.Key, item.Value);
            }
            return content;
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this.KeyVals, Formatting.Indented);
        }
    } 
    #endregion

    public class KeyValReplacer : BaseKeyValReplacer, ICompilable
    {
        public KeyValReplacer()
        { 
        } 
        public KeyValReplacer(string Source)
        {
            if (Source.EndsWith(".json"))
            {
                base.KeyVals = JsonConvert.DeserializeObject<Dictionary<string, string>>(Reader.Read(Source));
            }
            if (Source.EndsWith(".sql"))
            {
                IReader reader = new FileReader(Source);
                string sql = reader.Read();
                KeyValDBReader dbreader = new KeyValDBReader(sql);
                dbreader.ExecuteRead();
                base.KeyVals = dbreader.Data;
            }
            if (base.IsValidJson(Source))
            {
                base.KeyVals = JsonConvert.DeserializeObject<Dictionary<string, string>>(Source);
            }
        }
        public KeyValReplacer(Dictionary<string, string> Dict)
        {
            base.KeyVals = Dict;
        }
        public override string Compile(string content)
        {
            return base.Compile(content);
        }
    } 
}
