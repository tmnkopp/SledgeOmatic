using System;
using System.Collections.Generic;
using System.Text;

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
    } 
}
