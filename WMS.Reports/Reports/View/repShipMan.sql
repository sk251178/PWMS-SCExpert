CREATE VIEW dbo.repShipMan
AS
SELECT     dbo.vShipingManifestHeader.CONSIGNEE, dbo.vShipingManifestHeader.CONSIGNEENAME, dbo.vShipingManifestHeader.ORDERID, 
                      dbo.vShipingManifestHeader.ORDERTYPE, dbo.vShipingManifestHeader.REFERENCEORD, 
                      dbo.vShipingManifestHeader.TARGETCOMPANY, dbo.vShipingManifestHeader.COMPANYTYPE, 
                      dbo.vShipingManifestHeader.COMPANYNAME, dbo.vShipingManifestHeader.STREET1, dbo.vShipingManifestHeader.STREET2, 
                      dbo.vShipingManifestHeader.CITY, dbo.vShipingManifestHeader.STATE, dbo.vShipingManifestHeader.ZIP, 
                      dbo.vShipingManifestHeader.CONTACT1NAME, dbo.vShipingManifestHeader.CONTACT2NAME, dbo.vShipingManifestHeader.CONTACT1PHONE, 
                      dbo.vShipingManifestHeader.CONTACT2PHONE, dbo.vShipingManifestHeader.CONTACT1FAX, dbo.vShipingManifestHeader.CONTACT2FAX, 
                      dbo.vShipingManifestHeader.CONTACT1EMAIL, dbo.vShipingManifestHeader.CONTACT2EMAIL, dbo.vShipingManifestHeader.NOTES, 
                      dbo.vShipingManifestHeader.SCHEDULEDDATE, dbo.vShipingManifestHeader.SHIPPEDDATE, dbo.vShipingManifestHeader.SHIPMENT, 
                      dbo.vShipingManifestHeader.WAVE, dbo.vShipingManifestHeader.STAGINGLANE, dbo.vShipingManifestHeader.DOCTYPE, 
                      dbo.vShipingManifestDetail.ORDERLINE, dbo.vShipingManifestDetail.REFERENCEORDLINE, dbo.vShipingManifestDetail.SKU, 
                      dbo.vShipingManifestDetail.SKUDESC, dbo.vShipingManifestDetail.INVENTORYSTATUS, dbo.vShipingManifestDetail.INVSTATDESC, 
                      dbo.vShipingManifestDetail.QTYORIGINAL, dbo.vShipingManifestDetail.QTYMODIFIED, dbo.vShipingManifestDetail.QTYALLOCATED, 
                      dbo.vShipingManifestDetail.QTYPICKED, dbo.vShipingManifestDetail.QTYSTAGED, dbo.vShipingManifestDetail.QTYLOADED, 
                      dbo.vShipingManifestDetail.QTYSHIPPED
FROM         dbo.vShipingManifestDetail INNER JOIN
                      dbo.vShipingManifestHeader ON dbo.vShipingManifestDetail.CONSIGNEE = dbo.vShipingManifestHeader.CONSIGNEE AND 
                      dbo.vShipingManifestDetail.ORDERID = dbo.vShipingManifestHeader.ORDERID AND 
                      dbo.vShipingManifestDetail.DOCTYPE = dbo.vShipingManifestHeader.DOCTYPE