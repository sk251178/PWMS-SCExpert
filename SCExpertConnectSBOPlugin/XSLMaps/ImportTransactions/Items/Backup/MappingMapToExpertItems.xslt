<?xml version="1.0" encoding="UTF-8"?>
<!--
This file was generated by Altova MapForce 2011sp1
Refer to the Altova MapForce Documentation for further details.
http://www.altova.com/mapforce
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/">
		<xsl:variable name="var1_BOM" select="BOM/BO/OITM/row"/>
		<DATACOLLECTION>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation" namespace="http://www.w3.org/2001/XMLSchema-instance">
				<xsl:value-of select="'C:/Projects/SCExpertTrunk/SCExpertConnectSBOPlugin/XSLMAP~1/Items/ExpertItems.xsd'"/>
			</xsl:attribute>
			<DATA>
				<CONSIGNEE>
					<xsl:value-of select="/.."/>
				</CONSIGNEE>
				<xsl:for-each select="$var1_BOM">
					<SKU>
						<xsl:value-of select="/.."/>
					</SKU>
				</xsl:for-each>
				<DEFAULTUOM>
					<xsl:value-of select="/.."/>
				</DEFAULTUOM>
				<xsl:for-each select="$var1_BOM">
					<SKUDESC>
						<xsl:value-of select="/.."/>
					</SKUDESC>
				</xsl:for-each>
				<PICKSORTORDER>
					<xsl:value-of select="/.."/>
				</PICKSORTORDER>
				<SKUSHORTDESC>
					<xsl:value-of select="/.."/>
				</SKUSHORTDESC>
				<xsl:for-each select="$var1_BOM">
					<MANUFACTURERSKU>
						<xsl:value-of select="/.."/>
					</MANUFACTURERSKU>
				</xsl:for-each>
				<VENDORSKU>
					<xsl:value-of select="/.."/>
				</VENDORSKU>
				<xsl:for-each select="$var1_BOM">
					<OTHERSKU>
						<xsl:value-of select="/.."/>
					</OTHERSKU>
				</xsl:for-each>
				<xsl:for-each select="$var1_BOM">
					<SKUGROUP>
						<xsl:value-of select="/.."/>
					</SKUGROUP>
				</xsl:for-each>
				<SKUCLASS>
					<ATTRIBUTES>
						<ATTRIBUTE>
							<ATTNAME>
								<xsl:value-of select="/.."/>
							</ATTNAME>
							<ATTVALUE>
								<xsl:value-of select="/.."/>
							</ATTVALUE>
						</ATTRIBUTE>
					</ATTRIBUTES>
				</SKUCLASS>
				<xsl:for-each select="$var1_BOM">
					<CLASSNAME>
						<xsl:value-of select="/.."/>
					</CLASSNAME>
				</xsl:for-each>
				<xsl:for-each select="$var1_BOM">
					<STATUS>
						<xsl:value-of select="/.."/>
					</STATUS>
				</xsl:for-each>
				<xsl:for-each select="$var1_BOM">
					<INVENTORY>
						<xsl:value-of select="/.."/>
					</INVENTORY>
				</xsl:for-each>
				<xsl:for-each select="$var1_BOM">
					<NEWSKU>
						<xsl:value-of select="/.."/>
					</NEWSKU>
				</xsl:for-each>
				<INITIALSTATUS>
					<xsl:value-of select="/.."/>
				</INITIALSTATUS>
				<VELOCITY>
					<xsl:value-of select="/.."/>
				</VELOCITY>
				<FIFOINDIFFERENCE>
					<xsl:value-of select="/.."/>
				</FIFOINDIFFERENCE>
				<ONSITEMIN>
					<xsl:value-of select="/.."/>
				</ONSITEMIN>
				<ONSITEMAX>
					<xsl:value-of select="/.."/>
				</ONSITEMAX>
				<LASTCYCLECOUNT>
					<xsl:value-of select="/.."/>
				</LASTCYCLECOUNT>
				<CYCLECOUNTINT>
					<xsl:value-of select="/.."/>
				</CYCLECOUNTINT>
				<LOWLIMITCOUNT>
					<xsl:value-of select="/.."/>
				</LOWLIMITCOUNT>
				<PREFLOCATION>
					<xsl:value-of select="/.."/>
				</PREFLOCATION>
				<PREFPUTREGION>
					<xsl:value-of select="/.."/>
				</PREFPUTREGION>
				<PICTURE>
					<xsl:value-of select="/.."/>
				</PICTURE>
				<UNITPRICE>
					<xsl:value-of select="/.."/>
				</UNITPRICE>
				<OPORTUNITYRELPFLAG>
					<xsl:value-of select="/.."/>
				</OPORTUNITYRELPFLAG>
				<HAZCLASS>
					<xsl:value-of select="/.."/>
				</HAZCLASS>
				<TRANSPORTATIONCLASS>
					<xsl:value-of select="/.."/>
				</TRANSPORTATIONCLASS>
				<OVERPICKPCT>
					<xsl:value-of select="/.."/>
				</OVERPICKPCT>
				<OVERRECEIVEPCT>
					<xsl:value-of select="/.."/>
				</OVERRECEIVEPCT>
				<HUTYPE>
					<xsl:value-of select="/.."/>
				</HUTYPE>
				<DEFAULTRECUOM>
					<xsl:value-of select="/.."/>
				</DEFAULTRECUOM>
				<STORAGECLASS>
					<xsl:value-of select="/.."/>
				</STORAGECLASS>
				<NOTES>
					<xsl:value-of select="/.."/>
				</NOTES>
				<UOMCOLLECTION>
					<UOMOBJ>
						<UOM>
							<xsl:value-of select="/.."/>
						</UOM>
						<EANUPC>
							<xsl:value-of select="/.."/>
						</EANUPC>
						<GROSSWEIGHT>
							<xsl:value-of select="/.."/>
						</GROSSWEIGHT>
						<NETWEIGHT>
							<xsl:value-of select="/.."/>
						</NETWEIGHT>
						<LENGTH>
							<xsl:value-of select="/.."/>
						</LENGTH>
						<WIDTH>
							<xsl:value-of select="/.."/>
						</WIDTH>
						<HEIGHT>
							<xsl:value-of select="/.."/>
						</HEIGHT>
						<VOLUME>
							<xsl:value-of select="/.."/>
						</VOLUME>
						<LOWERUOM>
							<xsl:value-of select="/.."/>
						</LOWERUOM>
						<xsl:for-each select="$var1_BOM">
							<UNITSPERMEASURE>
								<xsl:value-of select="/.."/>
							</UNITSPERMEASURE>
						</xsl:for-each>
						<SHIPPABLE>
							<xsl:value-of select="/.."/>
						</SHIPPABLE>
					</UOMOBJ>
				</UOMCOLLECTION>
			</DATA>
		</DATACOLLECTION>
	</xsl:template>
</xsl:stylesheet>
