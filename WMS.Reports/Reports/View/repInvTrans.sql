CREATE VIEW repInvTrans
AS
SELECT 
IT.INVTRANS, 
CL.DESCRIPTION AS TRANDESC, 
IT.CONSIGNEE, 
IT.DOCUMENT, 
IT.LINE, 
IT.SKU, 
SKU.SKUDESC, 
IT.LOADID, 
IT.UOM, 
IT.QTY, 
IT.TRANDATE
FROM INVENTORYTRANS IT INNER JOIN CODELISTDETAIL CL ON IT.INVTRNTYPE = CL.CODE AND CL.CODELISTCODE = 'INVTRTYPE'
INNER JOIN SKU ON IT.SKU = SKU.SKU AND IT.CONSIGNEE = SKU.CONSIGNEE