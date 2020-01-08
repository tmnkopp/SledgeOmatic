using System;
using System.Collections.Generic;
using SOM.Data;
using SOM.Procedures;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SOM;
using SOM.IO;

namespace UnitTests
{
    [TestClass]
    public class ProcedureTests
    {
        //DECLARE @KVTABLE
        [TestMethod]
        public void Path_Compiles_To_Output()
        { 
            FileReader f = new FileReader(AppSettings.FileIn);
            PathCompile compiler = new PathCompile();
            string compiled = compiler.Execute(f.Read());
            Cache.Write(compiled);
            Cache.CacheEdit();
            Assert.IsNotNull(compiled);
        }
        [TestMethod]
        public void RegExExpectedResult()
        {
            
            string parseme = "[failed]\n[11111]";
            SqlRegexCompile extract = new SqlRegexCompile($"{Placeholder.Basepath}_regextest.sql");
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
            SqlKeyValCompile KVCompile = new SqlKeyValCompile( $"{Placeholder.Basepath}_unittest.sql"); 
            string actual = KVCompile.Execute("[UNITTEST]");
            string expected = "passed";
            Assert.AreEqual(expected, actual);
        }
    }
}
