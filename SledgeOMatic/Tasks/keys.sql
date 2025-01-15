SELECT (SELECT MAX(PK_ReportingCycle)   FROM fsma_ReportingCycles) MAX_PK_ReportingCycle
, (SELECT MAX(PK_QuestionGroup)  FROM fsma_QuestionGroups) MAX_PK_QuestionGroup
, (SELECT MAX(PK_FormPage)  FROM fsma_FormPages) MAX_PK_FormPage
, (SELECT MAX(PK_Question)   FROM fsma_Questions) MAX_PK_Question 
, (SELECT CAST(MIN(PK_Question) AS NVARCHAR(9)) FROM vwQuestions WHERE PK_FORM = '2025-Q1-CIO'  )  
+ ':' + (SELECT CAST(MAX(PK_Question) AS NVARCHAR(9)) FROM vwQuestions WHERE PK_FORM = '2025-Q1-CIO'  ) DC_PK_Question
, (SELECT CAST(MIN(FK_FormPage) AS NVARCHAR(9)) FROM vwQuestions WHERE PK_FORM = '2025-Q1-CIO'  )  
+ ':' +  (SELECT CAST(MAX(FK_FormPage) AS NVARCHAR(9)) FROM vwQuestions WHERE PK_FORM = '2025-Q1-CIO'  ) DC_FK_FormPage
, (SELECT CAST(MIN(PK_QuestionGroup) AS NVARCHAR(9)) FROM vwQuestions WHERE PK_FORM = '2025-Q1-CIO'  )  
+ ':' +  (SELECT CAST(MAX(PK_QuestionGroup) AS NVARCHAR(9)) FROM vwQuestions WHERE PK_FORM = '2025-Q1-CIO'  ) DC_PK_QuestionGroup
, (SELECT CAST(MAX(PK_ReportingCycle) AS NVARCHAR(9)) FROM vwQuestions WHERE PK_FORM = '2025-Q1-CIO'  ) DC_PK_ReportingCycle

RETURN;

SELECT MIN(PK_Question), MAX(PK_Question) FROM vwQuestions WHERE PK_FORM = '2025-Q1-CIO'  
SELECT MIN(FK_FormPage), MAX(FK_FormPage) FROM vwQuestions WHERE PK_FORM = '2025-Q1-CIO' 
SELECT MIN(PK_QuestionGroup), MAX(PK_QuestionGroup) FROM vwQuestions WHERE PK_FORM = '2025-Q1-CIO'   
 
SELECT * FROM fsma_ReportingCycles WHERE Description LIKE '%CIO%' ORDER BY PK_ReportingCycle DESC
 
SELECT * FROM vwOrgSubtoComponent WHERE PK_FORM='2025-Q1-CIO' AND Component='Department of Justice'
SELECT * FROM vwQuestions WHERE PK_FORM='2025-Q1-CIO' 

SELECT PK_Question, ROW_NUMBER() OVER (ORDER BY PK_Question ASC) + 70420  FROM vwQuestions WHERE PK_FORM='2025-Q1-CIO' 


 SELECT PK_Question, QGROUP, IdText, sortpos FROM vwQuestions WHERE PK_FORM = '2025-Q1-CIO' ORDER BY QGROUP, sortpos

