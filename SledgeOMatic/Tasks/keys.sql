SELECT MAX(PK_ReportingCycle) PK_ReportingCycle FROM fsma_ReportingCycles
SELECT MAX(PK_QuestionGroup) PK_QuestionGroup FROM fsma_QuestionGroups
SELECT MAX(PK_FormPage) PK_FormPage FROM fsma_FormPages
SELECT MAX(PK_Question) PK_Question FROM fsma_Questions
 
SELECT  MAX(PK_Question), MAX(FK_FormPage), MAX(PK_QuestionGroup)  FROM vwQuestions WHERE PK_FORM = '2024-Q2-CIO'   
SELECT MIN(FK_FormPage), MAX(FK_FormPage) FROM vwQuestions WHERE PK_FORM = '2023-A-IG'  
SELECT MIN(PK_Question), MAX(PK_Question) FROM vwQuestions WHERE PK_FORM = '2023-A-IG'  
SELECT MIN(PK_QuestionGroup), MAX(PK_QuestionGroup) FROM vwQuestions WHERE PK_FORM = '2023-A-IG'  

SELECT * FROM fsma_ReportingCycles WHERE Description LIKE '%CIO%' ORDER BY PK_ReportingCycle DESC
