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
    public class ProcessStartTests
    {
        [TestMethod]
        public void VSCode_VsCodes()
        { 
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = ConfigurationManager.AppSettings["CodeViewer"].ToString();
            startInfo.Arguments = $"cd {Environment.GetEnvironmentVariable("repo")}";  
            Process process = new Process();
            process.StartInfo = startInfo;
            process.Start(); 
            Assert.IsNotNull(process);
        }

        [TestMethod]
        public void ProcessStarter_Starts()
        { 
            var startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }; 
            var process = new Process { StartInfo = startInfo }; 
            process.Start(); 
            process.StandardInput.WriteLine(@"mkdir C:\_som\_src\_compile\BOD\compiled"); 
            process.StandardInput.WriteLine("exit");

            process.WaitForExit();

        }
        [TestMethod]
        public void Powershell_Powershells()
        {

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = @"powershell.exe";
            startInfo.Arguments = @"& 'D:\PS\test.ps1'";
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;

            Process process = new Process();
            process.StartInfo = startInfo;
            process.Start();

            Assert.IsNotNull(Cache.Read());
        }

        [TestMethod]
        public void Cache_Writes()
        {
            Cache.Write("");
            for (var i = 0; i < 100; i++)
            {
                bool IsFizz = (i % 2 == 0);
                bool IsBuzz = (i % 4 == 0);
                if (IsFizz && IsBuzz)
                {
                    Cache.WriteLine("FizzBuzz");
                }
                else if(IsFizz)
                {
                    Cache.WriteLine("Fizz");
                }
                else if (IsBuzz)
                {
                    Cache.WriteLine("Buzz");
                }
                else
                {
                    Cache.WriteLine($"{i}");
                }
            } 
            Cache.Inspect();
        }
        interface ILogger{
            void Log(string message);
        }
        class CacheLogger : ILogger{
            public void Log(string message){
                Cache.WriteLine($"{message}");
            }
        }
        static class RangeProvider{ 
            public static IEnumerable<int> Provide(){
                for (var i = 0; i < 100; i++){
                    yield return i;
                }  
            }
        }
    }
}
 