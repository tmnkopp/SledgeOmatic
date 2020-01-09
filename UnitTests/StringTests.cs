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
            FileReader f = new FileReader(@"D:\dev\CyberScope\CyberScopeBranch\CSwebdev\code\CyberScope\RMA\2020\2020_Q1_RMA_0.aspx");
            string content = f.Read(); 
            content = content
                .RemoveEmptyLines();
            
            Cache.Write(content);
            Cache.CacheEdit();
            Assert.IsNotNull(content);
        } 
    }
}





/* 
                .RemoveDuplicate(" ")  
     
*/
