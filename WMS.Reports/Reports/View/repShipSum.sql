Create View repShipSum
AS
SELECT 
SKU.SKU,
SKU.CONSIGNEE,
SKU.SKUDESC,
SUM(OD.QTYSHIPPED) AS SHIPPEDQTY 
FROM SKU JOIN OUTBOUNDORDETAIL OD ON SKU.SKU = OD.SKU AND SKU.CONSIGNEE = OD.CONSIGNEE JOIN OUTBOUNDORHEADER OH
ON OH.CONSIGNEE=OD.CONSIGNEE AND OH.ORDERID=OD.ORDERID
GROUP BY SKU.SKU,SKU.CONSIGNEE,SKU.SKUDESC  
HAVING SUM(OD.QTYSHIPPED)>0