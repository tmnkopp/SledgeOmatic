using SOM.Extentions;
using SOM.Parsers;
using SOM.Procedures ; 
using SOM.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SOM.Procedures
{
    public class RangeExtractor : BaseParser, IParser
    { 
        private string _extractTarget;
        private string _fromWhere;
        private string _toWhere; 
         
        public RangeExtractor( string ExtractTarget, string FromWhere, string ToWhere)
        { 
            _extractTarget = ExtractTarget;
            _fromWhere = FromWhere;
            _toWhere = (ToWhere!= "") ? ToWhere : "~~!__!~~"; 
        }
        public IEnumerable<string> Parse(string content)
        {
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
    }
}
