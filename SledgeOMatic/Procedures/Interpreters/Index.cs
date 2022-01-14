using SOM.Procedures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOM.Extentions;
namespace SOM.Procedures
{
   
    public class Indexer : ICompilable
    {
        private int _seed = 0;
        private int _reset = 1;
        private string _indexName = "[index]";
 
        public string Compile(string content)
        {
            StringBuilder result = new StringBuilder();
            string[] lines = content.Split('\n');
            int index = _seed; 
            foreach (var line in lines) {
                if (line.Contains(_indexName))
                    index++; 
                result.AppendFormat("{0}\n", line.Replace(""+ _indexName + "", ReSetter(index).ToString()));
            }
            return result.ToString().TrimTrailingNewline();
        }
        private int ReSetter(int index) {
            if (_reset <= 1)
                return index;
            return (_seed+1) + ((index) % _reset);
        } 
    }
}
