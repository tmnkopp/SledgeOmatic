﻿using Newtonsoft.Json;
using SOM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOM.Procedures
{
    public interface ITypeFormatter<T>
    { 
        string Format(T item);
    }
    public class DefaulTypeFormatter: ITypeFormatter<AppModelItem>
    { 
        public string Format(AppModelItem item)
        {
            return JsonConvert.SerializeObject(item, Formatting.None); 
        }
    }

}
