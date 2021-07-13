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
using SOM.Parsers;

namespace UnitTests
{ 
    [TestClass]
    public class ParserTests
    { 
        [TestMethod]
        public void LineExtractor_Extracts()
        { 
            DirectoryParser parser = new DirectoryParser();
            parser.Directory = "D:\\dev\\CyberScope\\CyberScopeBranch\\CSwebdev\\code\\CyberScope\\CustomControls\\*.*";
            parser.Parser = new LineExtractor("InPlace", 5);
            parser.Parser.ParseMode = ParseMode.Verbose; 
            parser.Inspect(); 
            Assert.IsNotNull(parser);
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
    } 
}
