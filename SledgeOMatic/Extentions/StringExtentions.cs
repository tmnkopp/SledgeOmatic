using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOM.Extentions
{ 
    public static class StringExtentions
    {
        public static string RemoveAsChars(this string stringParam, string removeChars)
        {
            var chars = removeChars.ToCharArray();
            foreach (var ch in chars) 
                stringParam = stringParam.Replace(ch.ToString(),""); 
            return stringParam;
        }
        public static string ReverseString(this string input)
        {
            char[] array = input.ToCharArray();
            Array.Reverse(array);
            return new String(array);
        }
        public static string ReplaceWhitespace(this string input, string ReplaceWith)
        {
            do
            {
                input = input.Replace(" ", ReplaceWith);
            } while (input.Contains(" ")); 
            return input;
        }
        public static string RemoveWhiteAndBreaks(this string input)
        {
            do
            {
                input = input.Replace(" ", "");
            } while (input.Contains(" "));
            input = input.Replace("\n", "").Replace("\r", "");
            return input;
        }
        public static string CleanHTML(this string input) {
            string output = input;

            string[] removables = new string[] { "\t", "\r", "\n" };
            foreach (string removable in removables)
                output = output.Replace(removable, "");

            do
            {
                output = output.Replace("  ", " ");
            } while (output.Contains("  "));

            output = output.Replace("> <", "><");
            string[] tags = new string[] { "p", "li", "b", "em", "strong" };
            foreach (string tag in tags) {  
                output = output.Replace($"<{tag}></{tag}>", ""); 
            }
            
            return output; 
        }
        public static string RemoveFormattingHTML(this string input)
        {
            string output = input; 
            output = output.Replace("> <", "><");
            string[] tags = new string[] { "p", "li", "ol", "ul", "b", "em", "strong" };
            foreach (string tag in tags)
            {
                output = output.Replace($"<{tag}>", "");
                output = output.Replace($"</{tag}>", "");
            }

            return output;
        }
    }
}
