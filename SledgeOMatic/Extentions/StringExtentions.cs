﻿using SOM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        public static string RemoveDuplicate(this string input, string Remove)
        {
            do  {
                input = input.Replace($"{Remove}{Remove}", $"{Remove}");
            } while (input.Contains($"{Remove}{Remove}"));
            return input;
        }
        public static string ReplaceWhitespace(this string input, string ReplaceWith)
        {
            do  {
                input = input.Replace(" ", ReplaceWith);
            } while (input.Contains(" ")); 
            return input;
        } 
        public static string ToValidSchemaName(this string input)
        { 
            return (input==null) ? input : input.RemoveAsChars(" \n\t\r!@#$%^&*()-=+';:<>,.?/").Trim();
        }
        public static string TrimLines(this string input )
        {
            StringBuilder sb = new StringBuilder(); 
            foreach (var line in input.Split('\n'))  {
                var result = Regex.Replace(line, "\r", "");
                if (!string.IsNullOrEmpty(result))
                {
                    sb.AppendFormat("{0}\n", result);
                }  
            }
            return sb.ToString();
        }
        public static string TrimTrailingNewline(this string input)
        {
            if (input.EndsWith("\n") || input.EndsWith("\r")) 
                input = input.Substring(0, input.Length - 1);
            
            return input;
        }
        public static string RemoveEmptyLines(this string input)
        { 
            return string.Join('\n', (from s in Regex.Split(input, $@"\r|\n")
                                      where !string.IsNullOrWhiteSpace(s)
                                      select s).ToList());
        }
        public static string RemoveWhiteAndBreaks(this string input)
        {
            do  {
                input = input.Replace(" ", "");
            } while (input.Contains(" "));
            input = input.Replace("\n", "").Replace("\r", "").Replace("\t", "");
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
