using System;
using System.Collections.Generic;
using System.Text;

namespace SOM.Core
{
    [AttributeUsage(AttributeTargets.Property)]
    public class InlineParam: Attribute
    {
        public string Alias { get; set; }
    }
}
