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
    public class PatternReplacer : KeyValReplacer
    {
        public PatternReplacer(string Source) : base(Source) { }
        public override string Compile(string content)
        {
            StringBuilder result = new StringBuilder();
            foreach (var line in content.Split("\n"))
            {
                string target = line;
                foreach (var item in KeyVals)
                {
                    string pattern = item.Key;
                    target = Regex.Replace(target, pattern,
                        m => m.Groups[1].Value
                         + item.Value
                         + m.Groups[3].Value
                        , RegexOptions.Singleline);
                }
                result.Append(target);
            }
            return result.ToString();
        }
    }
}
