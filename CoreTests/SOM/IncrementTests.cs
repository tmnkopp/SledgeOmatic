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
        public void Increment_Increments()
        {
            Incrementer compiler = new Incrementer("[\"|,](\\d{4})[^\\d]", 1000); 
            string actual = compiler.Interpret(test);
            string expected = @"FOO
""2000
""2001
""3001 ""3002 ""3003 
BAR";
            Assert.AreEqual(expected, actual);
        } 

        public static string test = @"FOO
""1000
""1001
""2001 ""2002 ""2003 
BAR";

    }
}
