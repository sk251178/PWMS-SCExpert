<?xml version="1.0" encoding="UTF-8"?>
<!-- Receipt Close -->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/">
		<BOM>
			<BO>
				<AdmInfo>
					<Object>20</Object>
				</AdmInfo>
				<OPDN>
					<row>
					<ObjType>20</ObjType>
					<DocDate><xsl:value-of select="concat(substring(DATACOLLECTION/DATA/CLOSERECEIPTDATE,6,4),substring(DATACOLLECTION/DATA/CLOSERECEIPTDATE,1,2),substring(DATACOLLECTION/DATA/CLOSERECEIPTDATE,4,2))"/></DocDate>
						<DocDueDate><xsl:value-of select="concat(substring(DATACOLLECTION/DATA/CLOSERECEIPTDATE,6,4),substring(DATACOLLECTION/DATA/CLOSERECEIPTDATE,1,2),substring(DATACOLLECTION/DATA/CLOSERECEIPTDATE,4,2))"/></DocDueDate>
					<CardCode><xsl:value-of select="DATACOLLECTION/DATA/LINES/LINE/COMPANY"/></CardCode>
					<Comments><xsl:value-of select="DATACOLLECTION/DATA/NOTES"/></Comments>
					<CreateDate><xsl:value-of select="concat(substring(DATACOLLECTION/DATA/CLOSERECEIPTDATE,6,4),substring(DATACOLLECTION/DATA/CLOSERECEIPTDATE,1,2),substring(DATACOLLECTION/DATA/CLOSERECEIPTDATE,4,2))"/></CreateDate>
					</row>
				</OPDN>
				<PDN1>
					<xsl:for-each select="DATACOLLECTION/DATA/LINES/LINE[QTYRECEIVED &gt; 0]">					
					<row>
						<LineNum><xsl:value-of select="ORDERLINE"/></LineNum>
						<BaseType>22</BaseType>
						<BaseEntry><xsl:value-of select="ORDERID"/></BaseEntry>
						<BaseLine><xsl:value-of select="ORDERLINE"/></BaseLine>
						<ItemCode><xsl:value-of select="SKU"/></ItemCode>
						<Quantity><xsl:value-of select="QTYRECEIVED"/></Quantity>
						<WhsCode>01</WhsCode>
					</row>
					</xsl:for-each>
				</PDN1>
			</BO>
		</BOM>
	</xsl:template>
</xsl:stylesheet>
