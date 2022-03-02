<?xml version="1.0" encoding="UTF-8"?>
<!-- Stock Transfers To Inbound Order -->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/">
		<DATACOLLECTION>
			<DATA>
				<CONSIGNEE>DEFAULT</CONSIGNEE>
				<ORDERID><xsl:value-of select="BOM/BO/OWTR/row/DocNum"/></ORDERID>
				<ORDERTYPE>PO</ORDERTYPE>
				<REFERENCEORD></REFERENCEORD>
				<xsl:choose>
								 <xsl:when test="BOM/BO/OWTR/row/CardCode != ''">
									 <SOURCECOMPANY><xsl:value-of select="BOM/BO/OWTR/row/CardCode"/></SOURCECOMPANY>
								</xsl:when>
								<xsl:otherwise>
									 <SOURCECOMPANY>Default</SOURCECOMPANY>
								</xsl:otherwise>
					</xsl:choose>
				<COMPANYTYPE>VENDOR</COMPANYTYPE>
				<STATUS></STATUS>
				<CREATEDATE></CREATEDATE>
				<NOTES><xsl:value-of select="BOM/BO/OWTR/row/Comments"/></NOTES>
				<HOSTORDERID><xsl:value-of select="BOM/BO/OWTR/row/DocEntry"/></HOSTORDERID>
				<DOCUMENTTYPE></DOCUMENTTYPE>
				<EXPECTEDDATE><xsl:value-of select="concat(substring(BOM/BO/OWTR/row/DocDueDate,1,4),'-',substring(BOM/BO/OWTR/row/DocDueDate,5,2),'-',substring(BOM/BO/OWTR/row/DocDueDate,7,2))"/></EXPECTEDDATE>
				<RECEIVEDFROM></RECEIVEDFROM>
				<LINES>
					<xsl:for-each select="BOM/BO/WTR1/row[(WhsCode='01') or (WhsCode='02')]">
						<LINE>
							<ORDERLINE><xsl:value-of select="LineNum"/></ORDERLINE>
							<REFERENCEORDLINE><xsl:value-of select="LineNum"/></REFERENCEORDLINE>
							<EXPECTEDDATE></EXPECTEDDATE>
							<LASTRECEIPTDATE></LASTRECEIPTDATE>
							<SKU><xsl:value-of select="ItemCode"/></SKU>
							<QTYORDERED><xsl:value-of select="Quantity"/></QTYORDERED>
							<QTYADJUSTED><xsl:value-of select="Quantity"/></QTYADJUSTED>
							<QTYRECEIVED></QTYRECEIVED>
							<INPUTUOM></INPUTUOM>
							<INPUTQTY></INPUTQTY>
							<INPUTSKU></INPUTSKU>
							<xsl:choose>
								 <xsl:when test="WhsCode = 01">
									 <INVENTORYSTATUS>AVAILABLE</INVENTORYSTATUS>
								</xsl:when>
								<xsl:when test="WhsCode = 02">
									 <INVENTORYSTATUS>DAMAGE</INVENTORYSTATUS>
								</xsl:when>
								<xsl:otherwise>
									 <INVENTORYSTATUS>AVAILABLE</INVENTORYSTATUS>
								</xsl:otherwise>
							</xsl:choose>
						</LINE>
					</xsl:for-each>
				</LINES>
			</DATA>
		</DATACOLLECTION>
	</xsl:template>
</xsl:stylesheet>
