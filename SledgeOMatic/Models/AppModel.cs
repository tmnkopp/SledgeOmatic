using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOM.Models
{
 
    [JsonObject(NamingStrategyType = typeof(DefaultNamingStrategy))]
    public class ParseResult
    { 
        public int ParseResultId { get; set; }
        public string Expression { get; set; }
        public string Source { get; set; } 
        public ICollection<ParseResultItem> ParseResultItems { get; set;}
    }
    [JsonObject(NamingStrategyType = typeof(DefaultNamingStrategy))]
    public class ParseResultItem
    { 
        public int ParseResultItemId { get; set; }
        public string Location { get; set; }
        public string Content { get; set; } 
        public ParseResult ParseResult { get; set; } 
    }
}
