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
using System.IO;
using SOM.Data;

namespace SOM.Procedures
{
    
    public class RangeExtractor : BaseParser, IParser<string>
    { 
        private string _extractPattern;
        private string _fromWhere;
        private string _toWhere; 
         
        public RangeExtractor(string ExtractPattern, string FromWhere, string ToWhere)
        { 
            _extractPattern = ExtractPattern;
            _fromWhere = FromWhere;
            _toWhere = (ToWhere!= "") ? ToWhere : "~~~~"; 
        }
        public virtual IEnumerable<string> Parse(string content)
        {
            base.Content = Content;
            if (this.ParseMode == ParseMode.Debug)
                Console.WriteLine($"content.Contains(_fromWhere) {content.Contains(_fromWhere)}");

            if (content.Contains(_fromWhere) && Regex.Match(content, _extractPattern).Success)
            {
                string[] FromSplits = content.Split(new[] { _fromWhere }, StringSplitOptions.None);
                foreach (string FromSplit in FromSplits)
                {
                    int matchPos = Regex.Match(FromSplit, _extractPattern).Index;
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
