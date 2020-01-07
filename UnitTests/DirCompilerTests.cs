using System;
using System.Collections.Generic;
using SOM.Data;
using SOM.Procedures;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SOM;
using System.IO;

namespace UnitTests
{
    [TestClass]
    public class DirCompilerTests
    {
        [TestMethod]
        public void DirRenamer_Renames_UnitTestPlaceholder()
        { 
            IProcedure sproc = new SqlKeyValCompile($"{Placeholder.Basepath}unittest.sql") ;
            DirRenamer dirRenamer = new DirRenamer(AppSettings.DestDir, sproc);
            dirRenamer.Rename();

            DirectoryInfo DI = new DirectoryInfo($"{AppSettings.DestDir}"); 
            Assert.IsTrue(DI.Exists);
        }

        [TestMethod]
        public void DirCompileToDestDirExist()
        {
            List<IProcedure> sprocs = new List<IProcedure>();
            sprocs.Add(new SqlKeyValCompile($"{Placeholder.Basepath}unittest.sql"));
            DirCompiler dirCompile = new DirCompiler(sprocs);
            dirCompile.Compile();

            DirectoryInfo DI = new DirectoryInfo($"{dirCompile.DestDir}"); 
            Assert.IsTrue(DI.Exists);
        } 
    }
}
