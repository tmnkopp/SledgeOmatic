SELECT MAX(PK_ReportingCycle) PK_ReportingCycle FROM fsma_ReportingCycles
SELECT MAX(PK_QuestionGroup) PK_QuestionGroup FROM fsma_QuestionGroups
SELECT MAX(PK_FormPage) PK_FormPage FROM fsma_FormPages
SELECT MAX(PK_Question) PK_Question FROM fsma_Questions
 
SELECT MAX(PK_Question) FROM vwQuestions WHERE PK_FORM = '2024-Q1-CIO'   
 
