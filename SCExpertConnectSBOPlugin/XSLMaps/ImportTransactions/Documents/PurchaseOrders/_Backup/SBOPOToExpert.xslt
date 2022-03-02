<?xml version="1.0" encoding="UTF-8"?>
<!-- Purchase Orders -->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/">
		<DATACOLLECTION>
			<DATA>
				<CONSIGNEE>DEFAULT</CONSIGNEE>
				<ORDERID><xsl:value-of select="BOM/BO/OPOR/row/DocNum"/></ORDERID>
				<ORDERTYPE>PO</ORDERTYPE>
				<REFERENCEORD></REFERENCEORD>
				<SOURCECOMPANY><xsl:value-of select="BOM/BO/OPOR/row/CardCode"/></SOURCECOMPANY>
				<COMPANYTYPE>VENDOR</COMPANYTYPE>
				<STATUS></STATUS>
				<CREATEDATE></CREATEDATE>
				<NOTES><xsl:value-of select="BOM/BO/OPOR/row/Comments"/></NOTES>
				<HOSTORDERID><xsl:value-of select="BOM/BO/OPOR/row/DocEntry"/></HOSTORDERID>
				<DOCUMENTTYPE></DOCUMENTTYPE>
				<EXPECTEDDATE><xsl:value-of select="concat(substring(BOM/BO/OPOR/row/DocDueDate,1,4),'-',substring(BOM/BO/OPOR/row/DocDueDate,5,2),'-',substring(BOM/BO/OPOR/row/DocDueDate,7,2))"/></EXPECTEDDATE>
				<RECEIVEDFROM></RECEIVEDFROM>
				<LINES>
					<xsl:for-each select="BOM/BO/POR1/row">
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
							<INVENTORYSTATUS>AVAILABLE</INVENTORYSTATUS>
						</LINE>
					</xsl:for-each>
				</LINES>
			</DATA>
		</DATACOLLECTION>
	</xsl:template>
</xsl:stylesheet>
