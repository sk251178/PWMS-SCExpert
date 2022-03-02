Create View repPickerEfficiency
AS
SELECT 
AUDIT.USERID,
CONVERT(DATETIME,CONVERT(NVARCHAR(10),ACTIVITYDATE,101)) AS ACTIVITYDATE,
USERS.USERID  AS FULLNAME,
COUNT(DISTINCT AUDIT.SKU) AS SKUCOUNT,
COUNT(DISTINCT AUDIT.CONSIGNEE + CONVERT(NVARCHAR,AUDIT.DOCUMENT) + CONVERT(NVARCHAR,AUDIT.DOCUMENTLINE)) AS NUMROWS
FROM AUDIT JOIN SYS_USERS USERS ON AUDIT.USERID=USERS.USERID 
WHERE AUDIT.ACTIVITYTYPE='PICKLOAD'
GROUP BY AUDIT.USERID,CONVERT(DATETIME,CONVERT(NVARCHAR(10),ACTIVITYDATE,101)),USERS.USERID