using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SOM.IO;
using SOM.Procedures;
using SOM.Extentions;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;

namespace UnitTests
{
     
    [TestClass]
    public class ParseTests
    { 
        [TestMethod]
        public void LineExtractor_Extracts()
        {
            string content = "111\n222\n333\n-target-\n444\n555\n666\n111\n222\n333\n-target-\n444\n555\n666\n";
            LineExtractor parser = new LineExtractor("-target-", 2);
            StringBuilder result = new StringBuilder();
            foreach (var item in parser.Parse(content))
                result.Append(item);
            string expected = "222\n333\n-target-\n444\n555";
            Assert.AreEqual(expected, result.ToString());
        }

        [TestMethod]
        public void BlockExtractor_Extracts()
        {
            string content = "1\n2\n3\n-target-\n1\n2\n3\n-target-\n1\n5\n3";
            StringBuilder result = new StringBuilder();
            RangeExtractor parser = new RangeExtractor("-target-", "2", "1"); 
            foreach (var item in parser.Parse(content))
                result.Append(item);
            Assert.AreEqual("2\n3\n-target-\n12\n3\n-target-\n1", result.ToString());
        }
        [TestMethod]
        public void BlockParser_Parses()
        {
            string content = "1\n2\n3\n<-target->\n1\n2\n3\n<-target->\n1\n5\n3";
            StringBuilder result = new StringBuilder();
            RangeExtractor parser = new RangeExtractor("-target-", "<", ">"); 
            foreach (var item in parser.Parse(content))
                result.Append(item);
            Assert.AreEqual("<-target-><-target->", result.ToString());
        }

        [TestMethod]
        public void YAML_Parses()
        {

            var serializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
            var yaml = serializer.Serialize(tasks);

            string content = "1\n2\n3\n<-target->\n1\n2\n3\n<-target->\n1\n5\n3";
            StringBuilder result = new StringBuilder();
            RangeExtractor parser = new RangeExtractor("-target-", "<", ">");
            foreach (var item in parser.Parse(content))
                result.Append(item);
            Assert.AreEqual("<-target-><-target->", result.ToString());
        }
    }
 
}
