Create View repShippedOrders
AS
SELECT 
OH.CONSIGNEE,
OD.ORDERID,
OD.ORDERLINE,
OD.SKU,
SKU.SKUDESC,
OD.QTYSHIPPED,
CO.COMPANYNAME,
OD.ADDDATE
FROM OUTBOUNDORHEADER OH INNER JOIN OUTBOUNDORDETAIL OD ON OD.CONSIGNEE=OH.CONSIGNEE AND OD.ORDERID = OH.ORDERID
INNER JOIN COMPANY CO ON CO.COMPANY = OH.TARGETCOMPANY  INNER JOIN SKU ON SKU.SKU = OD.SKU
WHERE OH.STATUS ='SHIPPED' AND OD.QTYSHIPPED > 0