<?xml version="1.0" encoding="UTF-8"?>
<!-- Receipt Close -->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:variable name="HostOrderId">
	  <xsl:value-of select="DATACOLLECTION/DATA/HOSTORDERID"/>
	</xsl:variable>
	<xsl:template match="/">
		<BOM>
			<BO>
				<AdmInfo>
					<Object>15</Object>
				</AdmInfo>
				<ODLN>
					<row>
					<ObjType>15</ObjType>
					<DocDate><xsl:value-of select="concat(substring(DATACOLLECTION/DATA/SHIPPEDDATE,6,4),substring(DATACOLLECTION/DATA/SHIPPEDDATE,1,2),substring(DATACOLLECTION/DATA/SHIPPEDDATE,4,2))"/></DocDate>
						<DocDueDate><xsl:value-of select="concat(substring(DATACOLLECTION/DATA/SHIPPEDDATE,6,4),substring(DATACOLLECTION/DATA/SHIPPEDDATE,1,2),substring(DATACOLLECTION/DATA/SHIPPEDDATE,4,2))"/></DocDueDate>
					<CardCode><xsl:value-of select="DATACOLLECTION/DATA/TARGETCOMPANY"/></CardCode>
					<CardName><xsl:value-of select="DATACOLLECTION/DATA/TARGETCOMPANYNAME"/></CardName>
					<Comments><xsl:value-of select="DATACOLLECTION/DATA/NOTES"/></Comments>
					<CreateDate><xsl:value-of select="concat(substring(DATACOLLECTION/DATA/SHIPPEDDATE,6,4),substring(DATACOLLECTION/DATA/SHIPPEDDATE,1,2),substring(DATACOLLECTION/DATA/SHIPPEDDATE,4,2))"/></CreateDate>
				</row>
				</ODLN>
				<DLN1>
					<xsl:for-each select="DATACOLLECTION/DATA/LINES/LINE[QTYSHIPPED &gt; 0]">					
					<row>
						<LineNum><xsl:value-of select="ORDERLINE"/></LineNum>
						<TargetType>15</TargetType>
						<TrgetEntry>1</TrgetEntry>
						<BaseRef/>
						<BaseType>17</BaseType>
						<BaseEntry><xsl:copy-of select="$HostOrderId" /></BaseEntry>
						<BaseLine><xsl:value-of select="REFERENCEORDLINE"/></BaseLine>
						<ItemCode><xsl:value-of select="SKU"/></ItemCode>
						<Quantity><xsl:value-of select="QTYSHIPPED"/></Quantity>
						<WhsCode>01</WhsCode>
					</row>
					</xsl:for-each>
				</DLN1>
			</BO>
		</BOM>
	</xsl:template>
</xsl:stylesheet>
