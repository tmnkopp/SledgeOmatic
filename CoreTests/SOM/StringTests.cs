using System;
using System.Collections.Generic;
using SOM.Data;
using SOM.Procedures;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SOM;
using SOM.IO;
using SOM.Extentions;

namespace UnitTests
{
    [TestClass]
    public class StringTests
    { 
        [TestMethod]
        public void Removes_White_And_Newline()
        {
          
            string expected= $"line\nline\nline\nline\nline\n";
            string content = $"line\nline\n\r\r\n\nline\nline\n\n\nline\n";
            Cache.Write($"{content}\n--------------------\n{expected}");
            content = content
                .RemoveEmptyLines();
            Cache.CacheEdit();
            Assert.AreEqual(expected, content);
        } 
    }
} 