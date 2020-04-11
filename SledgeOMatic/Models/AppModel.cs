using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOM.Models
{
    public enum AppModelType {
        DbTable,
        Class
    }
    public class AppModel
    {
        public int AppModelId { get; set; }
        public string Name { get; set; }
        public AppModelType AppModelType { get; set; }
        public ICollection<AppModelItem> AppModelItems { get; set;}
    }
    public class AppModelItem
    {
        public int AppModelItemId { get; set; }
        public string Name { get; set; }
        public string DataType { get; set; }
        public int OrdinalPosition { get; set; }
        public bool IsNullable { get; set; } 
        public int? MaxLen { get; set; }
        public AppModel AppModel { get; set; }
        public bool IsPkey()
        {
            return this.OrdinalPosition == 1;
        }
    }
}
