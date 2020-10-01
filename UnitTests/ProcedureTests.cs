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
        public void Incrementer_Increments()
        {
            Incrementer compiler = new Incrementer("[\"|,](\\d{4}).*", 1000); 
            string actual = compiler.Interpret(",1000ASDF\n,1001\n,1002\n\"1003");
            string expected = ",2000ASDF\n,2001\n,2002\n\"2003"; 
            Assert.AreEqual(expected, actual);
        }
                 
        [TestMethod]
        public void ModelTemplateCompile()
        {  
            ModelTemplateInterpreter compiler = new ModelTemplateInterpreter();
            string compiled = compiler.Interpret("[model:AuditLog path:C:\\_som\\_src\\model\\template.txt]");
            Cache.Write(compiled);
            Cache.CacheEdit();
            Assert.IsNotNull(compiled);
        } 
        [TestMethod]
        public void RegExExpectedResult()
        { 
            string parseme = "[failed]\n[11111]\n[failed][11111]\n[11111]";
            SqlRegexInterpreter extract = new SqlRegexInterpreter($"{Placeholder.Basepath}_regextest.sql");
            string actual = extract.Interpret(parseme);
            string expected = "[passed]\n[passed]\n[passed][passed]\n[passed]";
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void ContextExtractReturnsExpectedResult()
        {
            string parseme = "1-2-3-4-5-6-7-8-9-01-2-3-4-5-6-7-8-9-0";
            ContextExtractor extract = new ContextExtractor("4-5-6", 1, 3);
            string actual = extract.Parse(parseme);
            string expected = "-4-5-6-7-\n-4-5-6-7-\n";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void KeyValDBReaderNotNull()
        {
            SqlKeyValInterpreter KVCompile = new SqlKeyValInterpreter( $"{Placeholder.Basepath}_unittest.sql"); 
            string actual = KVCompile.Interpret("[UNITTEST]");
            string expected = "passed";
            Assert.AreEqual(expected, actual);
        }
    }
}
