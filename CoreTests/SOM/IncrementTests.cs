using System;
using System.Collections.Generic;
using SOM.Data;
using SOM.Procedures;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SOM;
using SOM.IO; 
using System.Text;
using System.Text.RegularExpressions;
using SOM.Extentions;

namespace UnitTests
{
    [TestClass]
    public class IncrementTests
    {  
        [TestMethod]
        public void Regex_Increments()
        {
            string search = "-123- 1234 12345 12 aaa";
            var matched = Regex.Match(search, @"(?<=\D|^|\s)(\d{2})(?=\D|$|\s)");
            if (matched.Success)
            {
                var g = matched.Groups[0];
            }
            Assert.IsTrue(matched.Success);
        } 
        public static string test = @"FOO
""1000
""1001
""2001 ""2002 ""2003 
BAR";

    }
}
