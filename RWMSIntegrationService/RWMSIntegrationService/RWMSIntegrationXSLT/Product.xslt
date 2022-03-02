<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:variable name="PathToDATA" select="DATACOLLECTION/DATA"></xsl:variable>
	<xsl:variable name="PathToSKUCLASS" select="$PathToDATA/SKUCLASS"></xsl:variable>
	<xsl:variable name="DefaultUom" select="$PathToDATA/DEFAULTUOM"></xsl:variable>
	<xsl:variable name="PathToUOMOBJ" select="$PathToDATA/UOMCOLLECTION/UOMOBJ[UOM=$DefaultUom]"></xsl:variable>

	<xsl:template match="/">
		<DATACOLLECTION>
		<DATA>
		<ProductKeyorItemNumber>
			<xsl:value-of select="$PathToDATA/SKU"/>
		</ProductKeyorItemNumber>
		<ProductCode>
			<xsl:value-of select="$PathToDATA/SKU"/>
		</ProductCode>
		<UPC>
			<xsl:value-of select="$PathToUOMOBJ/EANUPC"/>
		</UPC>
		<SKU>
			<xsl:value-of select="$PathToDATA/SKU"/>
		</SKU>
		<VendorCode>
			<xsl:value-of select="$PathToDATA/VENDORSKU"/>
		</VendorCode>
		<VendorName></VendorName>
		<FacilityCodeorOwnerCode>HYV</FacilityCodeorOwnerCode>
		<WarehouseCode></WarehouseCode>
		<Description>
			<xsl:value-of select="$PathToDATA/SKUDESC"/>	
		</Description>
		<UnitSize></UnitSize>
		<PackSize></PackSize>
		<Location></Location>
		<BracketType></BracketType>
		<VendorPack></VendorPack>
		<BuyerNumber></BuyerNumber>
		<BuyerName></BuyerName>
		<CaseWeight>
			<xsl:value-of select="$PathToUOMOBJ/GROSSWEIGHT"/>
		</CaseWeight>
		<CaseCube>
			<xsl:value-of select="$PathToUOMOBJ/VOLUME"/>
		</CaseCube>
		<Status></Status>
		<VendorTie></VendorTie>
		<VendorTier></VendorTier>
		<HomeSlot>
      <xsl:value-of select="$PathToDATA/HomeSlot"/>
    </HomeSlot>
		<PickSlot></PickSlot>
		<HomeAisle></HomeAisle>
		<PickAisle></PickAisle>
		<WarehouseTie></WarehouseTie>
		<WarehouseTier></WarehouseTier>
		<Cost>
			<xsl:value-of select="$PathToDATA/UNITPRICE"/>
		</Cost>
		</DATA>
		</DATACOLLECTION>
	</xsl:template>
</xsl:stylesheet>
