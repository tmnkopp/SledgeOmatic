using System;
using System.Collections.Generic;
using SOM.Data;
using SOM.Procedures;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SOM;
using SOM.IO;
using SOM.Procedures;
using Newtonsoft.Json;
using System.Text;
using System.Text.RegularExpressions;
using SOM.Extentions;

namespace UnitTests
{
    [TestClass]
    public class ProcedureTests
    {
        [TestMethod]
        public void RegexReplacer_Replaces()
        {
            var rep = new RegexReplacer("c:\\_som\\_src\\_compile\\unittest\\regex.json"); 
            string expected = $"123\nline\n123\n3333333\nline\n";
            string act = rep.Compile(expected);  
            Assert.AreEqual(expected, act);
        } 
    }
    public class RegexReplacer : KeyValReplacer, ICompilable
    {
        public RegexReplacer(string Source) : base(Source) { }
        public override string Compile(string content)
        {  
            foreach (var KeyValItem in KeyVals)
            { 
                content = Regex.Replace(content, KeyValItem.Key, KeyValItem.Value);
            }
            return content.TrimTrailingNewline();
        } 
    }
}
