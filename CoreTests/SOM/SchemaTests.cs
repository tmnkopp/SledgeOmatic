using Microsoft.VisualStudio.TestTools.UnitTesting;
using SOM.IO;
using SOM.Models;
using SOM.Procedures;
using SOMAPI.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;

namespace CoreTests
{
    [TestClass]
    public class SchemaTests
    {
        [TestMethod]
        public void Schema_Compiles()
        {
            StringBuilder result = new StringBuilder();
            string content = @"
	        DECLARE @EXISTS INT  = (SELECT COUNT(*) FROM EinsteinBGP WHERE PK_EinsteinBGP<>ISNULL(@PK_EinsteinBGP,0) AND PK_OrgSubmission=@PK_OrgSubmission

			--som:{0},
			--@
			    ASN,BGPRoutes,NonAdvertised,Notes,isActive
			--:som
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
            "; 
            content = Reader.Read(@"D:\dev\CyberScope\CyberScopeBranch\CSwebdev\database\Sprocs\EinsteinBGP_CRUD.sql");
            content = Reader.Read(@"D:\dev\CyberScope\CyberScopeBranch\CSwebdev\code\CyberScope\CustomControls\NCEinsteinUnannounced.ascx.vb");

            string model = "EinsteinBGP"; 


            Expression<Func<AppModelItem, bool>> filter = app => !app.Name.Contains("PK_"); 
            var schema = new SchemaService(model).Select(filter);

            SomFormatExtractor parser = new SomFormatExtractor(".*':som", "'som:", "':som"); 
            foreach (var parseresult in parser.Parse(content)){
                var replacement = (from i in schema select string.Format(parser.Formatter, i.Name));
                content = content.Replace(
                    parseresult, string.Join("", replacement)
                ); 
            }
  
            //content = content.Replace("isActive,", "isActive");
            Cache.Inspect(content); 
            Assert.IsNotNull(content); 
        }
    }
}
 