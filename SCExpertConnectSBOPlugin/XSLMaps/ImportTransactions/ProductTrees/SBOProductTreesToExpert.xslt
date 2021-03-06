<?xml version="1.0" encoding="UTF-16"?>
<!-- SBO Items XSL transformation-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/">
		<DATACOLLECTION>
			<DATA>
				<CONSIGNEE>DEFAULT</CONSIGNEE>
				<SKU><xsl:value-of select="BOM/BO/OITT/row/Code"/></SKU>
				<DEFAULTUOM></DEFAULTUOM>
				<SKUDESC></SKUDESC>
				<PICKSORTORDER></PICKSORTORDER>
				<SKUSHORTDESC></SKUSHORTDESC>
				<MANUFACTURERSKU></MANUFACTURERSKU>
				<VENDORSKU></VENDORSKU>
				<OTHERSKU></OTHERSKU>
				<SKUGROUP></SKUGROUP>
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
				<CLASSNAME></CLASSNAME>
				<STATUS>1</STATUS>
				<INVENTORY>1</INVENTORY>
				<NEWSKU>0</NEWSKU>
				<INITIALSTATUS></INITIALSTATUS>
				<VELOCITY></VELOCITY>
				<FIFOINDIFFERENCE></FIFOINDIFFERENCE>
				<ONSITEMIN></ONSITEMIN>
				<ONSITEMAX></ONSITEMAX>
				<LASTCYCLECOUNT></LASTCYCLECOUNT>
				<CYCLECOUNTINT></CYCLECOUNTINT>
				<LOWLIMITCOUNT></LOWLIMITCOUNT>
				<PREFLOCATION></PREFLOCATION>
				<PREFPUTREGION></PREFPUTREGION>
				<PICTURE></PICTURE>
				<UNITPRICE></UNITPRICE>
				<OPORTUNITYRELPFLAG></OPORTUNITYRELPFLAG>
				<HAZCLASS></HAZCLASS>
				<TRANSPORTATIONCLASS></TRANSPORTATIONCLASS>
				<OVERPICKPCT></OVERPICKPCT>
				<OVERRECEIVEPCT></OVERRECEIVEPCT>
				<HUTYPE></HUTYPE>
				<DEFAULTRECUOM></DEFAULTRECUOM>
				<STORAGECLASS></STORAGECLASS>
				<NOTES></NOTES>
				<BOMCOLLECTION>
					<xsl:for-each select="BOM/BO/ITT1/row[(Warehouse='01') or (Warehouse='02')]">
							<BOMOBJ>
								<PARTSKU><xsl:value-of select="Code"/></PARTSKU>
								<PARTQTY><xsl:value-of select="Quantity"/></PARTQTY>
								<BOMORDER><xsl:value-of select="ChildNum"/></BOMORDER>
							</BOMOBJ>
					</xsl:for-each>
				</BOMCOLLECTION>
			</DATA>
		</DATACOLLECTION>
	</xsl:template>
</xsl:stylesheet>
