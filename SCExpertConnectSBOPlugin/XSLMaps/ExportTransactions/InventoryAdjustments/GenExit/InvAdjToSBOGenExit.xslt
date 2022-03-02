<?xml version="1.0" encoding="UTF-8"?>
<!-- General Exits -->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/">
		<BOM>
			<BO>
				<AdmInfo>
					<Object>60</Object>
					<Version>2</Version>
				</AdmInfo>
				<Documents>
					<row>
						<DocDate><xsl:value-of select="concat(substring(DATACOLLECTION/DATA/ACTIVITYDATE,1,4),substring(DATACOLLECTION/DATA/ACTIVITYDATE,6,2),substring(DATACOLLECTION/DATA/ACTIVITYDATE,9,2))"/></DocDate>
						<DocDueDate><xsl:value-of select="concat(substring(DATACOLLECTION/DATA/ACTIVITYDATE,1,4),substring(DATACOLLECTION/DATA/ACTIVITYDATE,6,2),substring(DATACOLLECTION/DATA/ACTIVITYDATE,9,2))"/>
						</DocDueDate>
						<xsl:choose>
								 <xsl:when test="DATACOLLECTION/DATA/REASONCODE = NONE">
									 <Comments></Comments>
								</xsl:when>
								<xsl:otherwise>
									 <Comments><xsl:value-of select="DATACOLLECTION/DATA/REASONCODE"/></Comments>
								</xsl:otherwise>
							</xsl:choose>
						<JournalMemo>Goods Issue</JournalMemo>
						<DocObjectCode>60</DocObjectCode>
					</row>
				</Documents>
				<Document_Lines>
					<row>
						<LineNum>0</LineNum>
						<ItemCode><xsl:value-of select="DATACOLLECTION/DATA/SKU"/></ItemCode>
						<Quantity><xsl:value-of select="DATACOLLECTION/DATA/FROMQTY - DATACOLLECTION/DATA/TOQTY"/></Quantity>
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
					</row>
				</Document_Lines>
			</BO>
		</BOM>
	</xsl:template>
</xsl:stylesheet>
