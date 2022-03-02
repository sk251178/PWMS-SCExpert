CREATE view 
repPickList as 
SELECT 
pd.PICKLIST,
(select description from codelistdetail where codelistcode = 'PICKTYPE' and code = ph.PICKTYPE)as PICKTYPE , 
(select description from codelistdetail where codelistcode = 'PICKMETHOD' and code = ph.PICKMETHOD) as PICKMETHOD , 
ph.CREATEDATE ,
ph.PLANDATE ,
pd.PICKLISTLINE, 
pd.PICKREGION, 
ph.RELEASEDATE,
ph.ASSIGNEDDATE,
ph.COMPLETEDDATE ,
ph.WAVE ,
ph.HANDELINGUNITTYPE ,
ph.PACKAREA ,
pd.CONSIGNEE, 
pd.ORDERID , 
pd.ORDERLINE,
pd.SKU,
sku.skudesc ,
pd.STATUS , 
pd.UOM , 
pd.QTY , 
pd.ADJQTY , 
pd.PICKEDQTY, 
pd.FROMLOCATION,
pd.FROMLOAD, 
pd.TOLOAD , 
pd.TOCONTAINER, 
pd.TOLOCATION ,
oh.targetcompany,
co.companyname 
FROM PICKDETAIL pd INNER JOIN PICKHEADER ph ON pd.PICKLIST = ph.PICKLIST INNER JOIN sku ON sku.SKU = pd.SKU AND sku.consignee = pd.consignee 
INNER join OUTBOUNDORHEADER oh ON oh.consignee = pd.CONSIGNEE AND oh.orderid = pd.ORDERID INNER JOIN company co 
ON oh.targetcompany = co.company AND co.consignee = oh.consignee AND oh.companytype = co.companytype