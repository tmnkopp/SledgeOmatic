using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOM.Models
{
    public enum AppModelType
    {
        DbTable,
        Class
    }
    [JsonObject(NamingStrategyType = typeof(DefaultNamingStrategy))]
    public class AppModel
    {
        [JsonProperty("AppModelId")]
        public int AppModelId { get; set; }
        public string ModelName { get; set; }
        public AppModelType AppModelType { get; set; }
        public virtual ICollection<AppModelItem> AppModelItems { get; set; }
    }
    [JsonObject(NamingStrategyType = typeof(DefaultNamingStrategy))]
    public class AppModelItem
    {
        [JsonProperty("AppModelItemId")]
        public int AppModelItemId { get; set; }
        public string Name { get; set; }
        public string DataType { get; set; }
        public int OrdinalPosition { get; set; }
        public bool IsNullable { get; set; }
        public int? MaxLen { get; set; } 
        public bool IsPkey()
        {
            return this.OrdinalPosition == 1;
        }
    }
}
