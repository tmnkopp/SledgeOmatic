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
            string content = "aa som:%2 -f :som\nbb som:%2 -f :som\ncc som:%2 -f :som";
            ModuloInterpreter comp = new ModuloInterpreter();
            string actual = comp.Interpret(content);
            string expected = "aa\nbb-\ncc";
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void Group1RegEx_Pass()
        {
            string content = "12345_FOO_67890\n1-FOO-0";
            ReplaceGroup1ByRegex comp = new ReplaceGroup1ByRegex();
            comp.KeyVals.Add(".*(-\\w*-).*", "-QUX-");
            comp.KeyVals.Add(".*_(\\w*)_.*", "BAR"); 
            string actual = comp.Interpret(content);
            string expected = "12345_BAR_67890\n1-QUX-0";
            Assert.AreEqual(expected, actual);
        }
        class ReplaceGroup1ByRegex : BaseRegexInterpreter
        {
            public ReplaceGroup1ByRegex()
            { 
            }
            public override string Interpret(string content)
            {
                return base.Interpret(content);
            }
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
