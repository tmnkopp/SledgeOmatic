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
            string content = "aa%3bb%3cc%3dd%3ee%3ff%3gg";
            ModuloCompile comp = new ModuloCompile("><");
            string actual = comp.Compile(content);
            string expected = "aabbcc><ddeeff><gg";
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void Group1RegEx_Pass()
        {
            string content = "12345_FOO_67890";
            ReplaceGroup1ByRegex comp = new ReplaceGroup1ByRegex();
            string actual = comp.Compile(content);
            string expected = "12345_BAR_67890";
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void ReplaceByRegEx_Pass()
        {
            string content = "12345_FOO_67890";
            ReplaceByRegex comp = new ReplaceByRegex();
            string actual = comp.Compile(content);
            string expected = "12345_BAR_67890";
            Assert.AreEqual(expected, actual);
        }
        class ReplaceGroup1ByRegex : BaseRegexCompile {
            public ReplaceGroup1ByRegex() { 
                this.KeyVals.Add(".*_(\\w*)_.*", "BAR");
            }
            public override string Compile(string content)  {
                return base.Compile(content);
            }
        }
        class ReplaceByRegex : BaseRegexCompile {
            public ReplaceByRegex(){
                this.KeyVals.Add("FOO", "BAR"); 
            }
            public override string Compile(string content)  {
                return base.Compile(content);
            }
        }
        [TestMethod]
        public void SqlRegexCompile_Pass()
        { 
            string parseme = "[failed]\n[11111]";
            SqlRegexCompile extract = new SqlRegexCompile($"{Placeholder.Basepath}_regextest.sql");
            string actual = extract.Compile(parseme);
            string expected = "[passed]\n[passed]\n";
            Assert.AreEqual(expected, actual);
        } 
    }
}
