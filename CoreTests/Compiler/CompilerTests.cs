using CommandLine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SOM;
using SOM.Compilers;
using SOM.Data;
using SOM.Extentions;
using SOM.IO;
using SOM.Models;
using SOM.Procedures;
using SOMAPI.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;

namespace CoreTests
{ 
    [TestClass]
    public class CompilerTests
    { 
        [TestMethod]
        public void IG_Compiles()
        { 
            Compiler compiler = new Compiler();
            compiler.OnPreCompile += (s, a) =>
            {
                System.IO.Directory.CreateDirectory(@"D:\dev\CyberScope\CyberScope-v-7-34\CSwebdev\code\CyberScope\FismaForms\2021");
            };
            compiler.Source = @"C:\_som\_src\_compile\IG";
            compiler.Dest = @"C:\_som\_src\_compile\IG\compiled";
            compiler.CompileMode = CompileMode.Cache; 
            compiler.ContentCompilers.Add(new NumericKeyReplacer(@"C:\_som\_src\_compile\IG\keyval.sql"));
            compiler.ContentCompilers.Add(new KeyValReplacer(@"C:\_som\_src\_compile\IG\post-compile.json"));
            compiler.FileNameFormatter = (n) => (n.Replace("IG_1", "IG_1A"));
            compiler.ContentFormatter = (n) => (n.Replace("IG_1", "IG_1A"));
            compiler.FileFilter = "*_IG_*aspx*";
            compiler.Compile();
            compiler.FileFilter = "*DB_Update*sql"; 
            compiler.Compile();
            compiler.FileFilter = "*frmVal*";
            compiler.Compile();
            Cache.Inspect();
            Assert.IsNotNull(Cache.Read());
        }
        [TestMethod]
        public void BOD_Compiles()
        {
            string DestRoot = @"D:\dev\CyberScope\CyberScope-v-7-34\CSwebdev\";
            Compiler compiler = new Compiler(); 
            compiler.OnPreCompile += (s, a) =>
            {
                System.IO.Directory.CreateDirectory(DestRoot + @"code\CyberScope\HVA\2021");
            }; 
            compiler.Source = @"c:\_som\_src\_compile\BOD\"; 
            compiler.CompileMode = CompileMode.Commit;
            compiler.ContentCompilers.Add(new KeyValReplacer(@"c:\_som\_src\_compile\BOD\pre-compile.json"));
            compiler.ContentCompilers.Add(new NumericKeyReplacer(@"c:\_som\_src\_compile\BOD\keyval.sql"));
            compiler.FileNameFormatter = (n) => (n.Replace("2020", "2021"));
            compiler.ContentFormatter = (n) => (n.Replace("2020", "2021"));
            compiler.FileFilter = "*frmVal*";
            compiler.Dest = DestRoot + @"database\Sprocs";
            compiler.Compile();
            compiler.FileFilter = "*DB_Update*sql";
            compiler.Dest = DestRoot + @"database";
            compiler.Compile(); 
            compiler.Source = @"D:\dev\CyberScope\CyberScopeBranch\CSwebdev\code\CyberScope\HVA\2020";
            compiler.Dest = DestRoot +  @"code\CyberScope\HVA\2021"; 
            compiler.FileFilter = "*aspx*";
            compiler.Compile();
            Cache.Inspect();
        }

        [TestMethod]
        public void Schema_Compiles()
        {
            Cache.Write("");
            ISchemaProvider schema = new SchemaProvider("aspnet_Membership");

            Compiler compiler = new Compiler();
            compiler.Source = "C:\\_som\\T\\";
            compiler.Dest = "C:\\_som\\T\\";
            compiler.CompileMode = CompileMode.Commit;
            compiler.FileFilter = "unittest.html";
            compiler.FileNameFormatter = (n) => (n.Replace("unittest", "unittest_compiled"));
            compiler.ContentCompilers.Add(
                new SomSchemaInterpreter(schema)
                {
                    SchemaItemFilter = app => true,
                    SchemaItemProjector = (app) =>
                    {
                        app.StringFormatter = (i, f) => f.Replace("{0}", i.Name).Replace("{1}", i.DataType);
                        app.DataType = Regex.Replace(app.DataType, "(.*unique.*)", "int");
                        return app;
                    }
                });
            compiler.Compile();

            compiler.Source = compiler.Dest;
            compiler.OnCompiled += (s, a) =>
            {
                ProcessStartInfo startinfo = new ProcessStartInfo();
                startinfo.FileName = System.Environment.GetEnvironmentVariable("bom");
                startinfo.UseShellExecute = true;
                startinfo.Arguments = @"exe -t TestAutomator";
                Process p = Process.Start(startinfo);
            };
            compiler.ContentCompilers.Clear();
            compiler.ContentCompilers.Add(new KeyValReplacer(@"c:\\_som\\_src\replace.json"));
            compiler.ContentCompilers.Add(new Incrementer("(?<!\\d)\\d{3}(?!\\d)", 250));
            compiler.Compile();

            Assert.IsNotNull(Cache.Read());
        }

        [TestMethod]
        public void RMA_Compiles()
        { 
            Compiler compiler = new Compiler();
            compiler.Source = "c:\\_som\\_src\\_compile";
            compiler.Dest = "c:\\_som\\_src\\_compile\\_compiled";
            compiler.CompileMode = CompileMode.Commit; 
            compiler.ContentCompilers.Add(new KeyValReplacer($"{compiler.Source}\\replace.json"));
            compiler.ContentCompilers.Add(new NumericKeyReplacer($"{compiler.Source}\\keyval.sql"));
            compiler.FileNameFormatter = (n) => (n.Replace("Q1", "Q2"));  
            compiler.FileFilter = "*aspx*";
            compiler.Compile(); 
            Assert.IsNotNull(Cache.Read());
        } 
        [TestMethod]
        public void CIO_Compiles()
        { 
            Compiler compiler = new Compiler(); 
            compiler.Source = "c:\\_som\\_src\\_compile";
            compiler.Dest = "c:\\_som\\_src\\_compile\\_compiled";
            compiler.CompileMode = CompileMode.Cache; 
            compiler.ContentCompilers.Add(new KeyValReplacer($"{compiler.Source}\\replace.json"));
            compiler.ContentCompilers.Add(new NumericKeyReplacer($"{compiler.Source}\\keyval.sql"));
            compiler.FileNameFormatter = (n) => (n.Replace("Q1", "Q2"));
            compiler.FileFilter = "*DB_Update*sql";
            compiler.Compile();   
            compiler.FileFilter = "*frmVal*";
            compiler.Compile();
            compiler.FileFilter = "*aspx*"; 
            compiler.Compile();  
            Assert.IsNotNull(Cache.Read());
        } 
    } 
}


