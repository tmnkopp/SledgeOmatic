using System;
using System.Collections.Generic;
using SOM.Data;
using SOM.Procedures;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SOM;
using SOM.IO;
using SOM.Procedures;
namespace UnitTests
{
    [TestClass]
    public class ProcedureTests
    { 
 
        [TestMethod]
        public void RegExExpectedResult()
        { 
            string parseme = "[failed]\n[11111]\n[failed][11111]\n[11111]";
            SqlRegexInterpreter extract = new SqlRegexInterpreter($"{Placeholder.Basepath}_regextest.sql");
            string actual = extract.Interpret(parseme);
            string expected = "[passed]\n[passed]\n[passed][passed]\n[passed]";
            Assert.AreEqual(expected, actual);
        } 
 
    }
}
