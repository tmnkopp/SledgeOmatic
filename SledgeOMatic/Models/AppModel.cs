using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOM.Models
{
    public enum SchemaModelType { DbTable,  Class }
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class AppModel
    {
        [JsonProperty("AppModelId")]
        public int AppModelId { get; set; }
        public string ModelName { get; set; }
        public SchemaModelType AppModelType { get; set; } 
        public virtual ICollection<AppModelItem> AppModelItems { get; set; }
    }
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class AppModelItem
    {
        [JsonProperty("AppModelItemId")]
        public int AppModelItemId { get; set; }
        public string Name { get; set; }
        public string DataType { get; set; }
        public string ControlType { get; set; }
        public string ControlPrefix { get; set; }
        public int OrdinalPosition { get; set; }
        public bool IsNullable { get; set; }
        public int? MaxLen { get; set; } 
        public bool IsPkey()
        {
            return this.OrdinalPosition == 1;
        }
        public string ToStringFormat(string Format) {
            return _ToStringFormatter(this, Format); 
        }
        private Func<AppModelItem, string, string> _ToStringFormatter = (i, f) => {
            return f.Replace("{0}", i.Name)
            .Replace("{1}", i.DataType)
            .Replace("{2}", i.ControlType)
            .Replace("{3}", i.MaxLen?.ToString() ?? "255");
        };
        public Func<AppModelItem, string, string> StringFormatter 
        {
            set { _ToStringFormatter = value; }
        }
    }   
}
