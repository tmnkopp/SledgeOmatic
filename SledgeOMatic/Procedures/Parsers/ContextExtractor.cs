using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOM.Extentions;
namespace SOM.Procedures
{
    public class ContextExtractor : ICompiler
    {
        private string _extractTarget;
        private int _fromWhere;
        private int _toWhere;
        public ContextExtractor(string ExtractTarget, int FromWhere, int ToWhere)
        {
            _extractTarget = ExtractTarget;
            _fromWhere = FromWhere;
            _toWhere = ToWhere;
        }
        public string Compile(string content)
        {
            StringBuilder result = new StringBuilder();
            string[] postFixes = $"~{content}".Split(new[] { _extractTarget }, StringSplitOptions.None);
            for (int i = 1; i < postFixes.Length; i++)//skip prefix content index
            {
                string postfix = postFixes[i];
                if (_toWhere > postfix.Length)
                    _toWhere = postfix.Length;
                postfix = postfix.Substring(0, _toWhere);
                postFixes[i] = postfix;
            }
            _extractTarget = _extractTarget.ReverseString();
            content = content.ReverseString();
            string[] preFixes = $"~{content}".Split(new[] { _extractTarget }, StringSplitOptions.None);
            for (int i = 1; i < preFixes.Length; i++)//skip prefix content index
            {
                string prefix = preFixes[i];
                if (_fromWhere > prefix.Length)
                    _fromWhere = prefix.Length;
                prefix = prefix.Substring(0, _fromWhere);
                preFixes[i] = prefix.ReverseString();
            }
            _extractTarget = _extractTarget.ReverseString();
            content = content.ReverseString();
             
            for (int i = 1; i < preFixes.Length; i++)//skip prefix content index
            {
                result.AppendFormat("{1}{0}{2}\n", _extractTarget, preFixes[i], postFixes[i]); 
            }

            return result.ToString();
        }
    }
}
