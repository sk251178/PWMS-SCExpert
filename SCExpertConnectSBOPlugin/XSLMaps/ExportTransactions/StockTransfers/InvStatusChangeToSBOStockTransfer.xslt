<?xml version="1.0" encoding="UTF-8"?>
<!-- Stock Transfers -->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/">
		<BOM>
			<BO>
				<AdmInfo>
					<Object>67</Object>
					<Version>2</Version>
				</AdmInfo>
				<StockTransfer>
					<row>
						<DocDate><xsl:value-of select="concat(substring(DATACOLLECTION/DATA/ACTIVITYDATE,1,4),substring(DATACOLLECTION/DATA/ACTIVITYDATE,6,2),substring(DATACOLLECTION/DATA/ACTIVITYDATE,9,2))"/></DocDate>
						<xsl:choose>
								 <xsl:when test="DATACOLLECTION/DATA/REASONCODE = NONE">
									 <Comments></Comments>
								</xsl:when>
								<xsl:otherwise>
									 <Comments><xsl:value-of select="DATACOLLECTION/DATA/REASONCODE"/></Comments>
								</xsl:otherwise>
							</xsl:choose>
						<JournalMemo>Inventory Transfers -</JournalMemo>
						<xsl:choose>
								 <xsl:when test="DATACOLLECTION/DATA/TOSTATUS = AVAILABLE">
									 <FromWarehouse>01</FromWarehouse>
								</xsl:when>
								<xsl:when test="DATACOLLECTION/DATA/TOSTATUS = DAMAGE">
									 <FromWarehouse>02</FromWarehouse>
								</xsl:when>
								<xsl:otherwise>
									 <FromWarehouse>01</FromWarehouse>
								</xsl:otherwise>
						</xsl:choose>
					</row>
				</StockTransfer>
				<StockTransfer_Lines>
					<row>
						<LineNum>0</LineNum>
						<ItemCode><xsl:value-of select="DATACOLLECTION/DATA/SKU"/></ItemCode>
						<Quantity><xsl:value-of select="DATACOLLECTION/DATA/TOQTY"/></Quantity>
						<Price>0</Price>
						<xsl:choose>
								 <xsl:when test="DATACOLLECTION/DATA/TOSTATUS = AVAILABLE">
									 <WarehouseCode>01</WarehouseCode>
								</xsl:when>
								<xsl:when test="DATACOLLECTION/DATA/TOSTATUS = DAMAGE">
									 <WarehouseCode>02</WarehouseCode>
								</xsl:when>
								<xsl:otherwise>
									 <WarehouseCode>01</WarehouseCode>
								</xsl:otherwise>
						</xsl:choose>
						<ProjectCode/>
						<UseBaseUnits>tYES</UseBaseUnits>
					</row>
				</StockTransfer_Lines>
			</BO>
		</BOM>
	</xsl:template>
</xsl:stylesheet>
