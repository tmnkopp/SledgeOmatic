using SOM.Extentions;
using SOM.Procedures.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SOM.Procedures
{
    public class BlockExtractor : ICompiler, IParseStrategy
    {
        StringBuilder result = new StringBuilder();
        private string _extractTarget;
        private string _fromWhere;
        private string _toWhere;
        private string _id;
        public string ID
        {
            get {
                if (string.IsNullOrEmpty(_id))
                    _id = this.GetHashCode().ToString();
                return _id; 
            }
            set { _id = value; }
        }

        public BlockExtractor( string ExtractTarget, string FromWhere, string ToWhere)
        { 
            _extractTarget = ExtractTarget;
            _fromWhere = FromWhere;
            _toWhere = ToWhere; 
        }

        public string Compile(string content)
        {
            foreach (string item in Parse(content))
                result.Append($"{item}");
            return result.ToString().TrimTrailingNewline();
        }

        public IEnumerable<string> Parse(string content)
        {
            StringBuilder result = new StringBuilder();

            if (content.Contains(_fromWhere) && Regex.Match(content, _extractTarget).Success)
            {
                string[] FromSplits = content.Split(new[] { _fromWhere }, StringSplitOptions.None);
                foreach (string FromSplit in FromSplits)
                {
                    int matchPos = Regex.Match(FromSplit, _extractTarget).Index;
                    if (matchPos > 0)
                    {
                        int toPos = FromSplit.IndexOf(_toWhere);
                        toPos = (toPos < 0) ? FromSplit.Length : toPos + _toWhere.Length;

                        if (toPos > FromSplit.Length)
                            toPos = FromSplit.Length;

                        yield return string.Format("{0}{1}", _fromWhere, FromSplit.Substring(0, toPos));
                  
                    }
                }
            }
        }
         
        public override string ToString()
        {
            return $"{base.ToString()}-{ID.ToString()}";
        }
    }
}
