using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SOM.Procedures;

namespace UnitTests
{
    [TestClass]
    public class ParseTests
    {
        [TestMethod]
        public void LineExtractor_Extracts()
        {
            string parseme = "111\n222\n333\n-target-\n444\n555\n666\n";
            LineExtractor extract = new LineExtractor("-target-", 2); 
            string actual = extract.Parse(parseme);
            string expected = "222\n333\n-target-\n444\n555";
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void BlockExtractor_Extracts()
        {
            string parseme = "1\n2\n3\n-target-\n1\n2\n3\n-target-\n4\n5\n3";
            BlockExtractor extract = new BlockExtractor("-target-", "2", "1");
            string actual = extract.Parse(parseme);

            Assert.AreNotEqual("", actual);
        } 
    }
}
