using SOM.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using SOM.IO;
using System.Net; 
namespace SOM.Procedures 
{
    public class JiraTicketParse : IProcedure
    {
        public string Execute(string content)
        {
            StringBuilder result = new StringBuilder();
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(content);
             
            string desc = htmlDoc.DocumentNode.SelectSingleNode("//item/description").InnerHtml;
            string key = htmlDoc.DocumentNode.SelectSingleNode("//item/key").InnerHtml;
       
            desc = WebUtility.HtmlDecode(desc).CleanHTML();  
            htmlDoc.LoadHtml(desc);

            HtmlNodeCollection pnodes = htmlDoc.DocumentNode.SelectNodes("//p");
            Regex rex;
            foreach (var pnode in pnodes)
            {
                rex = new Regex(@"\d{1,3}\w{1,2}\.");
                if (rex.IsMatch(pnode.OuterHtml))
                {
                    result.AppendFormat("{0}{1}", pnode.InnerHtml, "\n");
                } 
            }
            return result.ToString();
        } 
    }
}
