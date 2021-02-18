using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SOM.IO;
using SOM.Procedures;
using SOM.Extentions;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace UnitTests
{
     
    [TestClass]
    public class ParseTests
    {

        [TestMethod]
        public void SomExtractor_Extracts()
        {
            string content = @"

	        DECLARE @EXISTS INT  = (SELECT COUNT(*) FROM EinsteinBGP WHERE PK_EinsteinBGP<>ISNULL(@PK_EinsteinBGP,0) AND PK_OrgSubmission=@PK_OrgSubmission
			    --som:{0}={0}\n
			    --@
			    AND ASN=@ASN
			    AND BGPRoutes=@BGPRoutes
			    AND BGPPeerIP=@BGPPeerIP
			    AND NonAdvertised=@NonAdvertised
			    --:som
			    )
		    IF (@EXISTS) > 0
		    BEGIN
			    SET @OUT = -11--DUPLICATE	
			    SET @MODE='ERR' 
			    RETURN;
		    END  
			--som:{0}={0}
			--@
			    ASN,BGPRoutes,NonAdvertised,Notes,isActive
			--:som
            "; 
            SomFormatExtractor parser = new SomFormatExtractor("--@");
            StringBuilder result = new StringBuilder();
            foreach (var item in parser.Parse(content)) 
            {
                Cache.Inspect(string.Format(parser.Formatter, item)); 
                result.Append(item);
            } 
            
            string expected = "222\n333\n-target-\n444\n555";
            Assert.AreEqual(expected, result.ToString());

        }
        [TestMethod]
        public void LineExtractor_Extracts()
        {
            string content = "111\n222\n333\n-target-\n444\n555\n666\n111\n222\n333\n-target-\n444\n555\n666\n";
            LineExtractor parser = new LineExtractor("-target-", 2);
            StringBuilder result = new StringBuilder();
            foreach (var item in parser.Parse(content))
                result.Append(item);
            string expected = "222\n333\n-target-\n444\n555";
            Assert.AreEqual(expected, result.ToString());
        }

        [TestMethod]
        public void BlockExtractor_Extracts()
        {
            string content = "1\n2\n3\n-target-\n1\n2\n3\n-target-\n1\n5\n3";
            StringBuilder result = new StringBuilder();
            RangeExtractor parser = new RangeExtractor("-target-", "2", "1"); 
            foreach (var item in parser.Parse(content))
                result.Append(item);
            Assert.AreEqual("2\n3\n-target-\n12\n3\n-target-\n1", result.ToString());
        }
        [TestMethod]
        public void BlockParser_Parses()
        {
            string content = "1\n2\n3\n<-target->\n1\n2\n3\n<-target->\n1\n5\n3";
            StringBuilder result = new StringBuilder();
            RangeExtractor parser = new RangeExtractor("-target-", "<", ">"); 
            foreach (var item in parser.Parse(content))
                result.Append(item);
            Assert.AreEqual("<-target-><-target->", result.ToString());
        }
    }
 
}
