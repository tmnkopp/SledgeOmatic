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
using SOM.Parsers;
using System.Linq;

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
            List<string> dirs = new List<string>();
            dirs.AddRange(new string[] { "111", "222" });
            List<object> args = new List<object>();
            args.AddRange(new object[] { "111", "222" });

            DirectoryParseDefinition dfd = new DirectoryParseDefinition() ;
            dfd.Directories = dirs;
            dfd.FileFilter = ".*"; 
            dfd.ParseType = "RangeExtractor";
            dfd.ParseTypeArgs = args;

            var ser = new SerializerBuilder()
                .WithNamingConvention(PascalCaseNamingConvention.Instance)
                .Build();
            string yaml = ser.Serialize(dfd);
            File.WriteAllText(@"c:\_som\parse\dfd.yaml", yaml);

            string raw = File.ReadAllText(@"c:\_som\parse\dfd.yaml");
            var deser = new DeserializerBuilder().WithNamingConvention(PascalCaseNamingConvention.Instance).Build();
            dfd = deser.Deserialize<DirectoryParseDefinition>(raw); 
  
        }
        [TestMethod]
        public void PathParser_Parses()
        {
            string path = @"D:\dev\CyberScope\CyberScopeBranch\CSwebdev\code\CyberScope\FismaForms\2022\";
            string ff = (from p in path.Split(@"\").Reverse() select p).FirstOrDefault();

            path = @"D:\dev\CyberScope\CyberScopeBranch\CSwebdev\code\CyberScope\FismaForms\2022\*.*";
            ff = (from p in path.Split(@"\").Reverse() select p).FirstOrDefault();
        }
    }
 
}
