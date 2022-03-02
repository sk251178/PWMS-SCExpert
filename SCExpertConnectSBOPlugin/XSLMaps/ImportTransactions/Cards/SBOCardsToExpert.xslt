<?xml version="1.0" encoding="UTF-8"?>
<!-- SBO Cards XSL transformation -->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/">
		<DATACOLLECTION>
			<DATA>
					<CONSIGNEE>DEFAULT</CONSIGNEE>
					 <xsl:choose>
							 <xsl:when test="BOM/BO/OCRD/row/CardType = C">
								 <COMPANYTYPE>CUSTOMER</COMPANYTYPE>
							</xsl:when>
							<xsl:when test="BOM/BO/OCRD/row/CardType = V">
								 <COMPANYTYPE>VENDOR</COMPANYTYPE>
							</xsl:when>
							<xsl:when test="BOM/BO/OCRD/row/CardType = S">
								 <COMPANYTYPE>SUPPLIER</COMPANYTYPE>
							</xsl:when>
							<xsl:otherwise>
								 <COMPANYTYPE>CUSTOMER</COMPANYTYPE>
							</xsl:otherwise>
						</xsl:choose>
					<COMPANY><xsl:value-of select="BOM/BO/OCRD/row/CardCode"/></COMPANY>
					<COMPANYNAME><xsl:value-of select="BOM/BO/OCRD/row/CardName"/></COMPANYNAME>
					<OTHERCOMPANY></OTHERCOMPANY>
					<STATUS>1</STATUS>
					<MIXPICKING>1</MIXPICKING>
					<CONTAINER></CONTAINER>
					<PREFUNLOADINGSIDE></PREFUNLOADINGSIDE>
					<DELIVERYCOMMENTS></DELIVERYCOMMENTS>
					<SERVICETIME></SERVICETIME>
					<DEFAULTCONTACT><xsl:value-of select="BOM/BO/OCRD/row/CardCode"/></DEFAULTCONTACT>
					<CONTACTS>
						<CONTACT>
							<CONTACTID><xsl:value-of select="BOM/BO/OCRD/row/CardCode"/></CONTACTID>
							<STREET1><xsl:value-of select="BOM/BO/OCRD/row/Address"/></STREET1>
							<STREET2></STREET2>
							<CITY><xsl:value-of select="BOM/BO/OCRD/row/City"/></CITY>
							<STATE><xsl:value-of select="BOM/BO/OCRD/row/Country"/></STATE>
							<ZIP><xsl:value-of select="BOM/BO/OCRD/row/ZipCode"/></ZIP>
							<CONTACT1NAME><xsl:value-of select="BOM/BO/OCRD/row/CntctPrsn"/></CONTACT1NAME>
							<CONTACT2NAME></CONTACT2NAME>
							<CONTACT1PHONE><xsl:value-of select="BOM/BO/OCRD/row/Phone1"/></CONTACT1PHONE>
							<CONTACT2PHONE></CONTACT2PHONE>
							<CONTACT1FAX><xsl:value-of select="BOM/BO/OCRD/row/Fax"/></CONTACT1FAX>
							<CONTACT2FAX></CONTACT2FAX>
							<CONTACT1EMAIL><xsl:value-of select="BOM/BO/OCRD/row/E_Mail"/></CONTACT1EMAIL>
							<CONTACT2EMAIL></CONTACT2EMAIL>
							<POINTID></POINTID>
							<ROUTE></ROUTE>
							<STAGINGLANE></STAGINGLANE>
					</CONTACT>
				</CONTACTS>
			</DATA>
		</DATACOLLECTION>
	</xsl:template>
</xsl:stylesheet>
