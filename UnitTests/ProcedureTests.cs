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
        //DECLARE @KVTABLE
         
        [TestMethod]
        public void ModelTemplateCompile()
        {  
            ModelTemplateCompile compiler = new ModelTemplateCompile();
            string compiled = compiler.Compile("[model:AuditLog template:C:\\_som\\_src\\model\\templateProp.cs]");
            Cache.Write(compiled);
            Cache.CacheEdit();
            Assert.IsNotNull(compiled);
        }

        [TestMethod]
        public void Path_Compiles_To_Output()
        { 
            FileReader f = new FileReader(AppSettings.FileIn);
            PathCompile compiler = new PathCompile();
            string compiled = compiler.Compile(f.Read());
            Cache.Write(compiled);
            Cache.CacheEdit();
            Assert.IsNotNull(compiled);
        }
        [TestMethod]
        public void RegExExpectedResult()
        { 
            string parseme = "[failed]\n[11111]";
            SqlRegexCompile extract = new SqlRegexCompile($"{Placeholder.Basepath}_regextest.sql");
            string actual = extract.Compile(parseme);
            string expected = "[passed]\n[passed]\n";
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void ContextExtractReturnsExpectedResult()
        {
            string parseme = "1-2-3-4-5-6-7-8-9-01-2-3-4-5-6-7-8-9-0";
            ContextExtractor extract = new ContextExtractor("4-5-6", 1, 3);
            string actual = extract.Compile(parseme);
            string expected = "-4-5-6-7-\n-4-5-6-7-\n";
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void LineExtractReturnsExpectedResult()
        {
            string parseme = "111\n222\n333\n-target-\n444\n555\n666\n";
            LineExtractor extract = new LineExtractor("-target-", 2);
            string actual = extract.Compile(parseme);
            string expected = "222\n333\n-target-\n444\n555";
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void BlockExact()
        {
            string parseme = "1\n2\n3\n-target-\n1\n2\n3\n-target-\n4\n5\n3";
            BlockExtractor extract = new BlockExtractor("-target-", "2", "1");
            string actual = extract.Compile(parseme);
 
            Assert.AreNotEqual("", actual);
        }
        [TestMethod]
        public void LineExtractExact()
        {
            string parseme = "111\n222\n333\n-target-\n444\n555\n666\n";
            LineExtractor extract = new LineExtractor("-target-", 0);
            string actual = extract.Compile(parseme);
            string expected = "-target-";
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void KeyValDBReaderNotNull()
        {
            SqlKeyValCompile KVCompile = new SqlKeyValCompile( $"{Placeholder.Basepath}_unittest.sql"); 
            string actual = KVCompile.Compile("[UNITTEST]");
            string expected = "passed";
            Assert.AreEqual(expected, actual);
        }
    }
}
