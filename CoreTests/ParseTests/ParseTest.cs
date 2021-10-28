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
            Cache.Write("");
            StringBuilder sb = new StringBuilder();
            DirectoryParser parser = new DirectoryParser(); 
            parser.Parser = new LineExtractor("KEY_SES_CAUSER", 1);
            parser.Parser.ParseMode = ParseMode.Verbose;
            parser.PathExcludePattern = @"Archive|bin\\|obj\\"; 
            //parser.ParseDirectory("D:\\dev\\CyberScope\\CyberScopeBranch\\CSwebdev\\database\\*.sql");
            parser.ParseDirectory(@"D:\dev\CyberScope\CyberScopeBranch\CSwebdev\code\CyberScope\*");
            sb.Append(parser.ToString()); 
  
            Cache.Write(sb.ToString());
            Cache.CacheEdit();
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
 