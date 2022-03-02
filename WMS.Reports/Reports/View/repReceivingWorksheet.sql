CREATE VIEW dbo.repReceivingWorksheet
AS
SELECT 
RECEIPTHEADER.RECEIPT, 
RECEIPTHEADER.STATUS, 
RECEIPTHEADER.SCHEDULEDDATE, 
RECEIPTHEADER.BOL, 
RECEIPTHEADER.NOTES, 
RECEIPTHEADER.CARRIERCOMPANY, 
RECEIPTHEADER.STARTRECEIPTDATE,
CARRIER.CARRIERNAME,
RECEIPTDETAIL.RECEIPTLINE, 
RECEIPTDETAIL.CONSIGNEE, 
RECEIPTDETAIL.SKU, 
RECEIPTDETAIL.ORDERID, 
RECEIPTDETAIL.ORDERLINE, 
RECEIPTDETAIL.QTYEXPECTED, 
RECEIPTDETAIL.QTYRECEIVED, 
RECEIPTDETAIL.DOCUMENTTYPE,
SKU.SKUDESC,
CONSIGNEE.CONSIGNEENAME 
FROM RECEIPTHEADER LEFT OUTER JOIN CARRIER ON RECEIPTHEADER.CARRIERCOMPANY = CARRIER.CARRIER
JOIN RECEIPTDETAIL ON RECEIPTHEADER.RECEIPT=RECEIPTDETAIL.RECEIPT
JOIN SKU ON RECEIPTDETAIL.SKU = SKU.SKU AND RECEIPTDETAIL.CONSIGNEE = SKU.CONSIGNEE JOIN
CONSIGNEE ON RECEIPTDETAIL.CONSIGNEE = CONSIGNEE.CONSIGNEE