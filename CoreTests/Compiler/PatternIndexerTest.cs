using Microsoft.VisualStudio.TestTools.UnitTesting;
using SOM.IO;
using SOM.Procedures;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreTests
{
    [TestClass]
    public class PatternIndexerTest
    {
        [TestMethod]
        public void PatternIndexer_Indexes()
        { 
            var s = Reader.Read(@"D:\dev\CyberScope\CyberScopeBranch\CSwebdev\database\DB_Update7.34_BOD_2021.sql");
            ICompilable comp = new PatternIndexer("QGroup, (\\d{1,2}),",  1); 
            var c = comp.Compile(s);
            Cache.Inspect(c); 

        }
    }

}
