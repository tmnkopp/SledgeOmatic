using System;
using System.Collections.Generic;
using SOM.Data;
using SOM.Procedures;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class ProcedureTests
    {
        [TestMethod]
        public void RegExExpectedResult()
        { 
            string parseme = "[failed]\n[11111]";
            RegexCompile extract = new RegexCompile("[dir]regextest.sql");
            string actual = extract.Execute(parseme);
            string expected = "[passed]\n[passed]\n";
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void ContextExtractReturnsExpectedResult()
        {
            string parseme = "1-2-3-4-5-6-7-8-9-01-2-3-4-5-6-7-8-9-0";
            ContextExtractor extract = new ContextExtractor("4-5-6", 1, 3);
            string actual = extract.Execute(parseme);
            string expected = "-4-5-6-7-\n-4-5-6-7-\n";
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void KeyValDBReaderNotNull()
        {
            SqlKeyValCompile KVCompile = new SqlKeyValCompile("[dir]unittest.sql");
            //Dictionary<string, string> dict = KVCompile.Data;
            string actual = KVCompile.Execute("[UNITTEST]");
            string expected = "passed";
            Assert.AreEqual(expected, actual);
        }
    }
}
