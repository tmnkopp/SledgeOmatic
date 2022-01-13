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
using System.Data.SqlClient;
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
        //
        [TestMethod]
        public void Form_Refactors()
        {
            int fnum = 7;
            Cache.Write(""); 
            Compiler compiler = new Compiler();
            compiler.Source = @"D:\dev\CyberScope\CyberScopeBranch\CSwebdev\code\CyberScope\EINSTEIN\2021\";
            compiler.Dest = @"D:\dev\CyberScope\CyberScopeBranch\CSwebdev\code\CyberScope\EINSTEIN\2021\";
            compiler.FileFilter = "*_8.aspx*";
            compiler.CompileMode = CompileMode.Commit; 
            compiler.ContentPreFormatter = (c) => {
                c = Regex.Replace(c, $"2021_A_EINSTEIN_(\\d)", $"2021_A_EINSTEIN_{fnum}");
                return c;
            };
            compiler.FileNameFormatter = (c) =>
            {
                c = Regex.Replace(c, $"2021_A_EINSTEIN_(\\d)", $"2021_A_EINSTEIN_{fnum}"); 
                return c;
            };
  
            compiler.Compile();
            Cache.CacheEdit();
            Assert.IsNotNull(Cache.Read());
        }
        [TestMethod]
        public void Form_Compiles()
        {
            Cache.Write("");
            ISchemaProvider schema = new SchemaProvider("EinsteinUnannounced");

            Compiler compiler = new Compiler();
            compiler.Source = @"D:\dev\CyberScope\CyberScopeBranch\CSwebdev\database\Sprocs\";
            compiler.Dest = "C:\\temp\\";
            compiler.CompileMode = CompileMode.Cache;
            compiler.FileFilter = "EinsteinUnannounced_CRUD.sql";
            compiler.ContentPreFormatter = (c) => {
                return c.Replace("som:", "som!schema").Replace(":som", "schema!som");
            };
            compiler.ContentCompilers.Add(
                new SomSchemaInterpreter(schema) { 
                    SchemaItemProjector = (appModelItem) => { 
                        appModelItem.DataType = Regex.Replace(appModelItem.DataType, $"(.*bit.*)", "int");
                        return appModelItem;
                    },
                    SchemaItemPredicate = (mi) => { 
                        return !Regex.IsMatch(mi.Name, $"K_|UserId|isActive") ;
                    }
                });
            compiler.Compile();
            Cache.CacheEdit();
            Assert.IsNotNull(Cache.Read());
        }
         
        [TestMethod]
        public void SAOP2020_Compiles()
        {
            Compiler compiler = new Compiler();
            compiler.Source = @"c:\_som\_src\_compile\SAOP\src\";
            compiler.CompileMode = CompileMode.Commit;  
            compiler.ContentCompilers.Add(new KeyValReplacer(@"C:\_som\_src\_compile\saop\pre-compile.json"));
            compiler.ContentCompilers.Add(new NumericKeyReplacer(@"c:\_som\_src\_compile\SAOP\keyval.json"));
            compiler.ContentCompilers.Add(new KeyValReplacer(@"c:\_som\_src\_compile\SAOP\post-compile.json")); 
            compiler.FilenameCompilers.Add(new KeyValReplacer(@"c:\_som\_src\_compile\SAOP\post-compile.json")); 
            compiler.Dest = @"D:\dev\CyberScope\CyberScopeBranch\CSwebdev\code\CyberScope\FismaForms\2021"; 
            compiler.Compile("*aspx*");
            compiler.Dest = @"D:\dev\CyberScope\CyberScopeBranch\CSwebdev\database";
            compiler.Compile("DBUpdate*");
            Cache.Inspect();
        }

        private void Compiler_OnPreCompile(object sender, CompilerEventArgs e)
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void SAOP_Compiles()
        {
            Compiler compiler = new Compiler();
            compiler.Source = @"c:\_som\_src\_compile\SAOP\src\";
            compiler.CompileMode = CompileMode.Commit;
            compiler.ContentCompilers.Add(new KeyValReplacer(@"c:\_som\_src\_compile\SAOP\pre-compile.json"));
            compiler.ContentCompilers.Add(new NumericKeyReplacer(@"c:\_som\_src\_compile\SAOP\keyval.sql"));
            compiler.ContentCompilers.Add(new KeyValReplacer(@"c:\_som\_src\_compile\SAOP\post-compile.json"));
            compiler.FilenameCompilers.Add(new KeyValReplacer(@"c:\_som\_src\_compile\SAOP\post-compile.json"));
            compiler.Dest = @"D:\dev\CyberScope\CyberScopeBranch\CSwebdev\database\";
            compiler.FileFilter = "*DBUpdate*";
            compiler.Compile();  
            //Cache.Inspect();
        }


        public string replacer(string s) { 
            s = s.Replace($"SolarWindNetwork", $"SWNPOC");  
            s = s.Replace($"wte", $"txt");  
            return s; 
        } 
        [TestMethod]
        public void SW_Compiles()
        {
            Compiler compiler = new Compiler();
            compiler.Source = @"D:\dev\CyberScope\CyberScopeBranch\CSwebdev\code\CyberScope\CustomControls\";
            compiler.CompileMode = CompileMode.Commit;
            compiler.ContentPostFormatter = (n) => (n = replacer(n));
            compiler.FileNameFormatter = (n) => (n = replacer(n));
            compiler.FileFilter = "*CBPOC*";
            compiler.Dest = @"D:\dev\CyberScope\CyberScopeBranch\CSwebdev\code\CyberScope\CustomControls\";
            compiler.Compile();
            Cache.Inspect();
        }
        [TestMethod]
        public void SWN_Compiles()
        {
            Compiler compiler = new Compiler();  
            compiler.CompileMode = CompileMode.Commit;
            compiler.ContentPostFormatter = (s) =>
            {
                s = s.Replace($"SolarWindNetwork", $"SWNPOC");
                s = s.Replace($"SolarWindNetwork", $"SWNPOC");
                return s;
            };
            compiler.FileNameFormatter = (s) => (s=s.Replace($"SolarWindNetwork", $"SWNPOC"));
            compiler.Source = @"D:\dev\CyberScope\CyberScopeBranch\CSwebdev\database\sprocs\"; 
            compiler.FileFilter = "*SolarWindNetwork_CRUD*";
            compiler.Dest = @"D:\dev\CyberScope\CyberScopeBranch\CSwebdev\database\sprocs\";
            compiler.Compile();
            Cache.Inspect();
        }


        [TestMethod]
        public void BOD_Compiles()
        {
            Compiler compiler = new Compiler();
            compiler.Source = @"c:\_som\_src\_compile\BOD\";
            compiler.CompileMode = CompileMode.Cache;
            compiler.ContentCompilers.Add(new NumericKeyReplacer(@"c:\_som\_src\_compile\BOD\pre-compile.json"));
            compiler.ContentCompilers.Add(new NumericKeyReplacer(@"c:\_som\_src\_compile\BOD\keyval.sql"));
            compiler.ContentCompilers.Add(new KeyValReplacer(@"c:\_som\_src\_compile\BOD\post-compile.json"));
            compiler.FilenameCompilers.Add(new KeyValReplacer(@"c:\_som\_src\_compile\BOD\post-compile.json")); 
            compiler.FileFilter = "*DB_Update*sql";
            compiler.Dest = @"D:\dev\CyberScope\CyberScopeBranch\CSwebdev\database"; 
            compiler.Compile();
            compiler.Dest = @"D:\dev\CyberScope\CyberScope-v-7-34\CSwebdev\code\CyberScope\FismaForms\2021\"; //
            compiler.Compile("*_IG_*aspx*"); //
            Cache.Inspect();
        }

        [TestMethod]
        public void IG_Compiles()
        {
            Compiler compiler = new Compiler();
            compiler.Source = @"D:\dev\CyberScope\CyberScopeBranch\CSwebdev\code\CyberScope\FismaForms\2021";
            compiler.Dest = @"D:\dev\CyberScope\CyberScopeBranch\CSwebdev\code\CyberScope\FismaForms\2022";
            compiler.CompileMode = CompileMode.Cache;
            compiler.ContentCompilers.Add(new NumericIncrementer(22421, 30000, @"\d{5}")); 
            compiler.FileNameFormatter = (n) => (n.Replace("2021_", "2022_"));
            compiler.ContentPostFormatter = (n) => (n.Replace("2021_", "2022_"));

            compiler.Compile("*_IG_*aspx*"); 
            Cache.CacheEdit(); 
        }

        [TestMethod]
        public void Compiler_Compiles()
        {
            Compiler compiler = new Compiler();
            compiler.Source = @"C:\_som\_src\_compile\IG";
            compiler.Dest = @"C:\_som\_src\_compile\IG\_compiled";
            compiler.CompileMode = CompileMode.Commit;
            compiler.ContentCompilers.Add(new KeyValReplacer(@"C:\_som\_src\_compile\IG\post-compile.json"));
            compiler.FileNameFormatter = (n) => (n.Replace("IG_1", "IG_1A"));
            compiler.ContentPostFormatter = (n) => (n.Replace("IG_1", "IG_1A"));
            compiler.Compile("*2020_A_IG_1*aspx*");
            compiler.FileNameFormatter = (n) => (n.Replace("IG_1", "IG_1B"));
            compiler.ContentPostFormatter = (n) => (n.Replace("IG_1", "IG_1B"));
            compiler.Compile("*2020_A_IG_1*aspx*");
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
                    SchemaItemPredicate = app => true,
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


