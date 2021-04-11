using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace SOM.Compilers
{
    public class CompilerEventArgs : EventArgs
    {
        public CompilerEventArgs(string Src, string Dest)
        {
            this.Source = Src;
            this.Dest = Dest;
        }
        public bool IsSuccessful { get; set; }
        public string Dest { get; set; } 
        public string Source { get; set; } 
        public string CompiledFileName { get; set; } 
        public string ContentCompiled { get; set; } 
        public FileInfo File { get; set; } 
    } 
}
