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
using System.IO;

namespace SOM.Procedures
{
    #region Base
    public abstract class BaseKeyValReplacer : BaseCompiler
    {
        protected Dictionary<string, string> KeyVals { get; set; }
        protected string Source { get; set; }
        public BaseKeyValReplacer()
        {
        }
        protected void PopulateKeyVals(ISomContext somContext)
        {
            string src = ""; 
            if (Source.ToLower().EndsWith(".json"))
            {
                Source = Source.Replace("~", somContext.BasePath);
                using (TextReader tr = File.OpenText(Source))
                    src = tr.ReadToEnd();
                this.KeyVals = JsonConvert.DeserializeObject<Dictionary<string, string>>(src);
            }
            if (Source.ToLower().EndsWith(".sql"))
            {
                Source = Source.Replace("~", somContext.BasePath);
                using (TextReader tr = File.OpenText(Source))
                    src = tr.ReadToEnd();
                KeyValDBReader dbreader = new KeyValDBReader(src);
                dbreader.ExecuteRead();
                this.KeyVals = dbreader.Data;
            }
            if (this.IsValidJson(Source))
            {
                this.KeyVals = JsonConvert.DeserializeObject<Dictionary<string, string>>(Source);
            }
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
                    Console.WriteLine($"JsonReaderException: {jex.Message}");
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
        public virtual string Compile(ISomContext somContext)
        {
            string content = somContext.Content;
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
            this.Source = Source;
        }
        public KeyValReplacer(Dictionary<string, string> Dict)
        {
            base.KeyVals = Dict;
        } 
    } 
}
