CREATE VIEW repReceivingBySKU
AS
SELECT 
IH.CONSIGNEE,
IDD.SKU ,
IH.ORDERID,
IH.ORDERTYPE,
IH.CREATEDATE,
IDD.QTYORDERED ,
IDD.QTYRECEIVED ,
IDD.EXPECTEDDATE ,
IDD.LASTRECEIPTDATE ,
IDD.ADDUSER 
FROM INBOUNDORDHEADER IH INNER JOIN INBOUNDORDDETAIL IDD ON IH.CONSIGNEE = IDD.CONSIGNEE AND IH.ORDERID = IDD.ORDERID
WHERE IDD.QTYRECEIVED > 0