<?xml version="1.0" encoding="UTF-16"?>
<!-- SBO Items XSL transformation-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/">
		<DATACOLLECTION>
			<DATA>
				<CONSIGNEE>DEFAULT</CONSIGNEE>
				<SKU><xsl:value-of select="BOM/BO/OITM/row/ItemCode"/></SKU>
				<DEFAULTUOM>EACH</DEFAULTUOM>
				<SKUDESC><xsl:value-of select="BOM/BO/OITM/row/ItemName"/></SKUDESC>
				<PICKSORTORDER></PICKSORTORDER>
				<SKUSHORTDESC></SKUSHORTDESC>
				<MANUFACTURERSKU><xsl:value-of select="BOM/BO/OITM/row/SuppCatNum"/></MANUFACTURERSKU>
				<VENDORSKU></VENDORSKU>
				<OTHERSKU><xsl:value-of select="BOM/BO/OITM/row/CodeBars"/></OTHERSKU>
				<SKUGROUP><xsl:value-of select="BOM/BO/OITM/row/ItmsGrpCod"/></SKUGROUP>
				<SKUCLASS>
					<ATTRIBUTES>
						<ATTRIBUTE>
							<ATTNAME>
							</ATTNAME>
							<ATTVALUE>
							</ATTVALUE>
						</ATTRIBUTE>
					</ATTRIBUTES>
				</SKUCLASS>
				<xsl:choose>
					<xsl:when test="BOM/BO/OITM/row/ManSerNum = Y">
					  <CLASSNAME>SERIALS</CLASSNAME>
					</xsl:when>
					<xsl:otherwise>
					  <CLASSNAME></CLASSNAME>
					</xsl:otherwise>
				  </xsl:choose>
				  <xsl:choose>
					<xsl:when test="BOM/BO/OITM/row/frozenFor = Y">
					  <STATUS>1</STATUS>
					  <INVENTORY>1</INVENTORY>
					</xsl:when>
					<xsl:otherwise>
					  <STATUS>0</STATUS>
					  <INVENTORY>0</INVENTORY>
					</xsl:otherwise>
				  </xsl:choose>
				<NEWSKU>0</NEWSKU>
				<INITIALSTATUS>AVAILABLE</INITIALSTATUS>
				<VELOCITY></VELOCITY>
				<FIFOINDIFFERENCE></FIFOINDIFFERENCE>
				<ONSITEMIN>0</ONSITEMIN>
				<ONSITEMAX>0</ONSITEMAX>
				<LASTCYCLECOUNT></LASTCYCLECOUNT>
				<CYCLECOUNTINT>0</CYCLECOUNTINT>
				<LOWLIMITCOUNT>0</LOWLIMITCOUNT>
				<PREFLOCATION></PREFLOCATION>
				<PREFPUTREGION></PREFPUTREGION>
				<PICTURE></PICTURE>
				<UNITPRICE></UNITPRICE>
				<OPORTUNITYRELPFLAG>1</OPORTUNITYRELPFLAG>
				<HAZCLASS></HAZCLASS>
				<TRANSPORTATIONCLASS></TRANSPORTATIONCLASS>
				<OVERPICKPCT>1</OVERPICKPCT>
				<OVERRECEIVEPCT>1</OVERRECEIVEPCT>
				<HUTYPE></HUTYPE>
				<DEFAULTRECUOM>EACH</DEFAULTRECUOM>
				<STORAGECLASS></STORAGECLASS>
				<NOTES></NOTES>
				<UOMCOLLECTION>
					<UOMOBJ>
						 <xsl:choose>
							 <xsl:when test="BOM/BO/OITM/row/SalUnitMsr = EA">
								 <UOM>EACH</UOM>
							</xsl:when>
							<xsl:when test="BOM/BO/OITM/row/SalUnitMsr = KG">
								 <UOM>KG</UOM>
							</xsl:when>
							<xsl:otherwise>
								 <UOM>EACH</UOM>
							</xsl:otherwise>
						</xsl:choose>
						<EANUPC></EANUPC>
						<GROSSWEIGHT><xsl:value-of select="BOM/BO/OITM/row/SWght1Unit"/></GROSSWEIGHT>
						<NETWEIGHT><xsl:value-of select="BOM/BO/OITM/row/SWght1Unit"/></NETWEIGHT>
						<LENGTH><xsl:value-of select="BOM/BO/OITM/row/SLen1Unit"/></LENGTH>
						<WIDTH><xsl:value-of select="BOM/BO/OITM/row/SWdth1Unit"/></WIDTH>
						<HEIGHT><xsl:value-of select="BOM/BO/OITM/row/SHght1Unit"/></HEIGHT>
						<VOLUME><xsl:value-of select="BOM/BO/OITM/row/SVolUnit"/></VOLUME>
						<LOWERUOM></LOWERUOM>
						<UNITSPERMEASURE><xsl:value-of select="BOM/BO/OITM/row/NumInSale"/></UNITSPERMEASURE>
						<SHIPPABLE>1</SHIPPABLE>
					</UOMOBJ>
					<xsl:choose>
						 <xsl:when test="BOM/BO/OITM/row/SalPackMsr = CS">
							 <UOMOBJ>
								<UOM>CASE</UOM>
								<EANUPC></EANUPC>
								<GROSSWEIGHT><xsl:value-of select="BOM/BO/OITM/row/SWeight1"/></GROSSWEIGHT>
								<NETWEIGHT><xsl:value-of select="BOM/BO/OITM/row/SWeight1"/></NETWEIGHT>
								<LENGTH><xsl:value-of select="BOM/BO/OITM/row/SLength1"/></LENGTH>
								<WIDTH><xsl:value-of select="BOM/BO/OITM/row/SWidth1"/></WIDTH>
								<HEIGHT><xsl:value-of select="BOM/BO/OITM/row/SHeight1"/></HEIGHT>
								<VOLUME><xsl:value-of select="BOM/BO/OITM/row/SVolume"/></VOLUME>
								<LOWERUOM>EACH</LOWERUOM>
								<UNITSPERMEASURE><xsl:value-of select="BOM/BO/OITM/row/SalPackUn"/></UNITSPERMEASURE>
								<SHIPPABLE>1</SHIPPABLE>
							</UOMOBJ>
						</xsl:when>
						<xsl:otherwise>
						</xsl:otherwise>
					</xsl:choose>
				</UOMCOLLECTION>
			</DATA>
		</DATACOLLECTION>
	</xsl:template>
</xsl:stylesheet>
