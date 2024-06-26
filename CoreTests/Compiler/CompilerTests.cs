﻿using CommandLine;
using Serilog;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SOM;
using SOM.Compilers;
using SOM.IO;
using SOM.Procedures;
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace CoreTests
{
    [TestClass]
    public class CompilerTests
    { 
        [TestMethod]
        public void ModelCompile_Compiles()
        { 
            var som = Environment.GetEnvironmentVariable("som", EnvironmentVariableTarget.User);
            var config = new TestServices().Configuration;
            var logger = new Mock<ILogger>().Object;
            var cache = new CacheService(config, logger);
            ISomContext somContext = new SomContext(config, logger, cache);
            var compiler = new Compiler(somContext);
            compiler.Source = som;
            compiler.FileFilter = "*.som"; 
            compiler.ContentCompilers.Add(new ModelCompile("aspnet_Users", ".*")); 
            compiler.Compile();
            cache.Inspect();
            Assert.IsNotNull(compiler); 
        }
   
        [TestMethod]
        public void Regex_Compiles()
        {
            var som = Environment.GetEnvironmentVariable("som", EnvironmentVariableTarget.User);
            var config = new TestServices().Configuration;
            var logger = new Mock<ILogger>().Object;
            var cache = new CacheService(config, logger);
            ISomContext somContext = new SomContext(config, logger, cache);
            var compiler = new Compiler(somContext);
            compiler.Source = som;
            compiler.FileFilter = "*.som";
            compiler.ContentCompilers.Add(new RegexReplacer("{\"FOO\":\"BAR\"}"));
            compiler.ContentCompilers.Add(new NumericIncrementer(1000, 2000, @"\d{4}"));
            compiler.Compile();
            cache.Inspect();
            Assert.IsNotNull(compiler);
        }
        [TestMethod]
        public void Inserter_Inserts()
        {
            string readall = File.ReadAllText(@"D:\dev\CyberScope\CyberScopeBranch\CSwebdev\code\CyberScope\FismaForms\2022\2022_A_IG_0.aspx");
            var config = new TestServices().Configuration;
            var logger = new Mock<ILogger>().Object;
            var cache = new CacheService(config, logger);
            ISomContext somContext = new SomContext(config, logger, cache) { Content = readall }; 
            //var obj = new Insert(@"(<tr>.*?SectionHead.*?<\/tr>)", @"\n\n\n{1}\n\n\n");
            var obj = new Insert(@"(<tr>.*?SectionHead.*?)(<\/tr>)", @" [ {1} ] [ {2} ] ");
            somContext.Content = obj.Compile(somContext);
            
            //obj = new Inserter(@"using SOM\.Procedures", "FOO");
            //result = obj.Compile(somContext);
            Assert.IsNotNull(somContext.Content);
        }
    } 
}


