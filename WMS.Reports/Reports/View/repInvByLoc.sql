Create View repInvByLoc
AS
SELECT 
IL.LOADID,
IL.CONSIGNEE,
IL.SKU,
IL.LOCATION,
IL.UNITS,
IL.UNITSALLOCATED,
LOC.WAREHOUSEAREA,
INS.DESCRIPTION,
SKU.SKUDESC
FROM INVLOAD IL JOIN SKU ON IL.CONSIGNEE=SKU.CONSIGNEE AND IL.SKU=SKU.SKU JOIN INVSTATUSES INS ON IL.STATUS=INS.CODE
JOIN LOCATION LOC ON LOC.LOCATION=IL.LOCATION