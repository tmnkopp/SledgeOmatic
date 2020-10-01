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
    public class RegExCompilerTests
    { 
        [TestMethod]
        public void ModuloCompile_Pass()
        {
            string content = "aa[%:2 format:-]\nbb[%:2 format:-]\ncc[%:2 format:-]";
            ModuloInterpreter comp = new ModuloInterpreter();
            string actual = comp.Interpret(content);
            string expected = "aa\nbb-\ncc";
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void Group1RegEx_Pass()
        {
            string content = "12345_FOO_67890";
            ReplaceGroup1ByRegex comp = new ReplaceGroup1ByRegex();
            string actual = comp.Interpret(content);
            string expected = "12345_BAR_67890";
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void ReplaceByRegEx_Pass()
        {
            string content = "12345_FOO_67890";
            ReplaceByRegex comp = new ReplaceByRegex();
            string actual = comp.Interpret(content);
            string expected = "12345_BAR_67890";
            Assert.AreEqual(expected, actual);
        }
        class ReplaceGroup1ByRegex : BaseRegexInterpreter {
            public ReplaceGroup1ByRegex() { 
                this.KeyVals.Add(".*_(\\w*)_.*", "BAR");
            }
            public override string Interpret(string content)  {
                return base.Interpret(content);
            }
        }
        class ReplaceByRegex : BaseRegexInterpreter {
            public ReplaceByRegex(){
                this.KeyVals.Add("FOO", "BAR"); 
            }
            public override string Interpret(string content)  {
                return base.Interpret(content);
            }
        }
        [TestMethod]
        public void SqlRegexCompile_Pass()
        { 
            string parseme = "[failed]\n[11111]";
            SqlRegexInterpreter extract = new SqlRegexInterpreter($"{Placeholder.Basepath}_regextest.sql");
            string actual = extract.Interpret(parseme);
            string expected = "[passed]\n[passed]";
            Assert.AreEqual(expected, actual);
        } 
    }
}
