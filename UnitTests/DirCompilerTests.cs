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
        public void DirCompileToDestDirExist()
        {
            List<IProcedure> sprocs = new List<IProcedure>();
            sprocs.Add(new SqlKeyValCompile($"{Placeholder.Basepath}unittest.sql"));

            DirCompiler dirCompile = new DirCompiler(AppSettings.SourceDir);
            dirCompile.Compile(sprocs);
            dirCompile.Rename(sprocs);

            DirectoryInfo DI = new DirectoryInfo($"{dirCompile.DestDir}"); 
            Assert.IsTrue(DI.Exists);
        } 
    }
}
