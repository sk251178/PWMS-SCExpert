<?xml version="1.0"?>

<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:xs="http://www.w3.org/2001/XMLSchema"
	xmlns:msxml="urn:schemas-microsoft-com:xslt"
	xmlns:cd="urn:my-scripts"
        exclude-result-prefixes="xs msxml cd">

	<xsl:output indent="yes" encoding="windows-1255"/>

	<xsl:variable name="PathToDATA" select="DATACOLLECTION/DATA"></xsl:variable>
	<xsl:variable name="CONTACT" select="$PathToDATA/CONTACTS/CONTACT/text()"></xsl:variable>
	<xsl:variable name="CONSIGNEE" select="$PathToDATA/CONSIGNEE/text()"></xsl:variable>
	<xsl:variable name="ORDERID" select="$PathToDATA/ORDERID/text()"></xsl:variable>
	<xsl:variable name="PathToLINES" select="$PathToDATA/LINES"></xsl:variable>

	<xsl:variable name="ExtendedWeight">
		<xsl:value-of select="$PathToDATA/ExtendedWeight"></xsl:value-of>
	</xsl:variable>
	<xsl:variable name="ExtendedCube">
		<xsl:value-of select="$PathToDATA/ExtendedCube"></xsl:value-of>
	</xsl:variable>

	<xsl:template match="/">
		<DATACOLLECTION>
		<DATA>
			<xsl:for-each select="DATACOLLECTION/DATA/LINES/LINE[SKU]">
				<DownloadType></DownloadType>
				<OrderNumber>
					<xsl:value-of select="$ORDERID"></xsl:value-of>
				</OrderNumber>
				<FacilityCodeorOwnerCode>ALP</FacilityCodeorOwnerCode>
				<WarehouseCode>
					<xsl:value-of select="$PathToDATA//CONSIGNEE"></xsl:value-of>
				</WarehouseCode>
				<StoreID>
					<xsl:value-of select="$PathToDATA//SHIPTO"></xsl:value-of>
				</StoreID>
				<OrderType>
					<xsl:value-of select="$PathToDATA//ORDERTYPE"></xsl:value-of>
				</OrderType>
				<ProductType>
					<xsl:value-of select="$PathToDATA//SKUGROUP"></xsl:value-of>
				</ProductType>
				<ServiceDateTime>
					<xsl:value-of select="$PathToDATA//REQUESTEDDATE"></xsl:value-of>
				</ServiceDateTime>
				<SKU>
					<xsl:value-of select="SKU"></xsl:value-of>
				</SKU>
				<Quantity>
					<xsl:value-of select="QTYMODIFIED"></xsl:value-of>
				</Quantity>
				<Description>
					<xsl:value-of select="SKUDESC"></xsl:value-of>
				</Description>
				<ExtendedWeight>
					<xsl:value-of select="ExtendedWeight"></xsl:value-of>
				</ExtendedWeight>
				<ExtendedCube>
					<xsl:value-of select="ExtendedCube"></xsl:value-of>
				</ExtendedCube>
			</xsl:for-each>
		</DATA>
		</DATACOLLECTION>
	</xsl:template>
</xsl:stylesheet>


