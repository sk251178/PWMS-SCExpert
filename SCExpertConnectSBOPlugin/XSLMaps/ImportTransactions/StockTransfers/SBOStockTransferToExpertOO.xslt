<?xml version="1.0" encoding="UTF-8"?>
<!-- Sales Orders -->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/">
		<DATACOLLECTION>
			<DATA>
					<CONSIGNEE>DEFAULT</CONSIGNEE>
					<ORDERID><xsl:value-of select="BOM/BO/OWTR/row/DocNum"/></ORDERID>
					<ORDERTYPE>CUST</ORDERTYPE>
					<REFERENCEORD></REFERENCEORD>
					<xsl:choose>
								 <xsl:when test="BOM/BO/OWTR/row/CardCode != ''">
									 <TARGETCOMPANY><xsl:value-of select="BOM/BO/OWTR/row/CardCode"/></TARGETCOMPANY>
								</xsl:when>
								<xsl:otherwise>
									 <TARGETCOMPANY>Default</TARGETCOMPANY>
								</xsl:otherwise>
					</xsl:choose>
					<COMPANYTYPE>CUSTOMER</COMPANYTYPE>
					<STATUS></STATUS>
					<CREATEDATE></CREATEDATE>
					<NOTES><xsl:value-of select="BOM/BO/OWTR/row/Comments"/></NOTES>
					<STAGINGLANE></STAGINGLANE>
					<REQUESTEDDATE><xsl:value-of select="concat(substring(BOM/BO/OWTR/row/DocDueDate,1,4),'-',substring(BOM/BO/OWTR/row/DocDueDate,5,2),'-',substring(BOM/BO/OWTR/row/DocDueDate,7,2))"/></REQUESTEDDATE>
					<SCHEDULEDDATE><xsl:value-of select="concat(substring(BOM/BO/OWTR/row/DocDueDate,1,4),'-',substring(BOM/BO/OWTR/row/DocDueDate,5,2),'-',substring(BOM/BO/OWTR/row/DocDueDate,7,2))"/></SCHEDULEDDATE>
					<SHIPPEDDATE></SHIPPEDDATE>
					<SHIPMENT></SHIPMENT>
					<STOPNUMBER></STOPNUMBER>
					<ORDERPRIORITY></ORDERPRIORITY>
					<ROUTE></ROUTE>
					<LOADINGSEQ></LOADINGSEQ>
					<WAVE></WAVE>
					<ROUTINGSET></ROUTINGSET>
					<STATUSDATE></STATUSDATE>
					<HOSTORDERID><xsl:value-of select="BOM/BO/OWTR/row/Comments"/></HOSTORDERID>
					<DELIVERYSTATUS></DELIVERYSTATUS>
					<TARGETCOMPANYNAME></TARGETCOMPANYNAME>
					<SHIPTO></SHIPTO>
				<LINES>
				<xsl:for-each select="BOM/BO/WTR1/row[(WhsCode='01') or (WhsCode='02')]">
					<LINE>
							<ORDERLINE><xsl:value-of select="LineNum"/></ORDERLINE>
							<REFERENCEORDLINE><xsl:value-of select="LineNum"/></REFERENCEORDLINE>
							<SKU><xsl:value-of select="ItemCode"/></SKU>
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
							<QTYORIGINAL><xsl:value-of select="Quantity"/></QTYORIGINAL>
							<QTYMODIFIED><xsl:value-of select="Quantity"/></QTYMODIFIED>
							<QTYALLOCATED></QTYALLOCATED>
							<QTYPICKED></QTYPICKED>
							<QTYSTAGED></QTYSTAGED>
							<QTYVERIFIED></QTYVERIFIED>
							<QTYLOADED></QTYLOADED>
							<QTYSHIPPED></QTYSHIPPED>
							<EXPLOADEDFLAG></EXPLOADEDFLAG>
							<INPUTQTY></INPUTQTY>
							<INPUTUOM></INPUTUOM>
							<UNITPRICE></UNITPRICE>
							<INPUTSKU></INPUTSKU>
							<SKUVOLUME></SKUVOLUME>
							<SKUWEIGHT></SKUWEIGHT>
							<NOTES></NOTES>
						</LINE>
					</xsl:for-each>
				</LINES>
			</DATA>
		</DATACOLLECTION>
	</xsl:template>
</xsl:stylesheet>
