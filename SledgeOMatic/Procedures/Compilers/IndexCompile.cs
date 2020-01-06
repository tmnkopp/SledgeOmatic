using SOM.Procedures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOM.Procedures
{
    public class IndexCompile : IProcedure
    {
        private int _seed = 0;
        private int _reset = 1;
        private string _indexName = "[index]";

        public IndexCompile(int Seed, int Reset, string IndexName)
        {
            _seed = Seed-1;
            _reset = Reset;
            _indexName = IndexName;
        }
        public string Execute(string compileme)
        {
            StringBuilder result = new StringBuilder();
            string[] lines = compileme.Split('\n');
            int index = _seed; 
            foreach (var line in lines) {
                if (line.Contains(_indexName))
                    index++; 
                result.AppendFormat("{0}\n", line.Replace(""+ _indexName + "", ReSetter(index).ToString()));
            }
            return result.ToString();
        }
        private int ReSetter(int index) {
            if (_reset <= 1)
                return index;
            return (_seed+1) + ((index) % _reset);
        }
        public override string ToString()
        {
            return $"{base.ToString()} -{(_seed + 1).ToString()} -{_reset.ToString()} -{_indexName.ToString()}";
        }
    }
}
