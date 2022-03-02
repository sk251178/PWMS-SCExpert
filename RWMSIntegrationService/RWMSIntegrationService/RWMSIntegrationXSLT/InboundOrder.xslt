<?xml version="1.0"?>

<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:xs="http://www.w3.org/2001/XMLSchema"
	xmlns:msxml="urn:schemas-microsoft-com:xslt"
	xmlns:cd="urn:my-scripts"
        exclude-result-prefixes="xs msxml cd">

	<xsl:output indent="yes" encoding="windows-1255"/>

	<xsl:variable name="PathToDATA" select="DATACOLLECTION/DATA"></xsl:variable>
	<xsl:variable name="CONTACT" select="$PathToDATA/CONTACTS/CONTACT/text()"></xsl:variable>
	<xsl:variable name="CONSIGNEE" select="$PathToDATA/CONSIGNEE/text()"></xsl:variable>
	<xsl:variable name="ORDERID" select="$PathToDATA/ORDERID/text()"></xsl:variable>
	<xsl:variable name="PathToLINES" select="$PathToDATA/LINES"></xsl:variable>

	<xsl:variable name="TotalOrderdCases">
		<xsl:value-of select="$PathToDATA/ORDEREDCASES"></xsl:value-of>
	</xsl:variable>
	<xsl:variable name="TotalOrderdPallets">
		<xsl:value-of select="$PathToDATA/ORDEREDPALLETS"></xsl:value-of>
		</xsl:variable>
	<xsl:variable name="TotalReceivedCases">
		<xsl:value-of select="$PathToDATA/RECIEVEDCASES"></xsl:value-of>
	</xsl:variable>
	<xsl:variable name="TotalReceivedPallets">
		<xsl:value-of select="$PathToDATA/RECIEVEDPALLETS"></xsl:value-of>
	</xsl:variable>
	<xsl:variable name="TotalWeight">
		<xsl:value-of select="$PathToDATA/TOTALWEIGHT"></xsl:value-of>
	</xsl:variable>
	<xsl:variable name="TotalCube">
		<xsl:value-of select="$PathToDATA/TOTALCUBE"></xsl:value-of>
	</xsl:variable>
	<xsl:variable name="TotalReceivedWeight">
		<xsl:value-of select="$PathToDATA/TOTALRECIEVEDWEIGHT"></xsl:value-of>
	</xsl:variable>
	<xsl:variable name="TotalReceivedCube">
		<xsl:value-of select="$PathToDATA/TOTALRECIEVEDCUBE"></xsl:value-of>
	</xsl:variable>
	<xsl:template match="/">
		<DATACOLLECTION>
		<DATA>
			<DownloadType></DownloadType>

			<OrderNumber>
				<xsl:value-of select="$ORDERID"></xsl:value-of>
			</OrderNumber>
			<OrderDate>
				<xsl:value-of select="$PathToDATA//CREATEDATE"></xsl:value-of>
			</OrderDate>
			<FacilityCodeorOwnerCode>HYV</FacilityCodeorOwnerCode>
			<DueDate>
				<xsl:value-of select="$PathToDATA//EXPECTEDDATE"></xsl:value-of>
			</DueDate>
			<WarehouseCodeorConsigneeCode>
        <xsl:value-of select="$PathToDATA//CONSIGNEE"></xsl:value-of>
      </WarehouseCodeorConsigneeCode>
			<CompanyCodeorVendorCode>
				<xsl:value-of select="$PathToDATA//SOURCECOMPANY"></xsl:value-of>
			</CompanyCodeorVendorCode>
			<CompanyFacilityCodeorVendorOwnerCode>HYV</CompanyFacilityCodeorVendorOwnerCode>
			<OrderType>
				<xsl:value-of select="$PathToDATA//ORDERTYPE"></xsl:value-of>
			</OrderType>
			<BuyerCode>
        <xsl:value-of select="$PathToDATA//CONSIGNEE"></xsl:value-of>
      </BuyerCode>
			<DestinationCode>
        <xsl:value-of select="$PathToDATA//RECEIVEDFROM"></xsl:value-of>
      </DestinationCode>
			<DestinationFacilityCode></DestinationFacilityCode>
			<ManagedType></ManagedType>
			<OrderedPoints></OrderedPoints>
			<OrderedCost></OrderedCost>
			<BracketPrice></BracketPrice>
			<Allowance></Allowance>
			<Hot></Hot>
			<Streamline></Streamline>
			<ScheduledPickupDateTime></ScheduledPickupDateTime>
			<Comments>
				<xsl:value-of select="$PathToDATA/NOTES"></xsl:value-of>
			</Comments>
			<OrderedPallets>
				<xsl:value-of select="$TotalOrderdPallets"></xsl:value-of>
			</OrderedPallets>
			<OrderedCases>
				<xsl:value-of select="$TotalOrderdCases"></xsl:value-of>
			</OrderedCases>
			<OrderedWeight>
				<xsl:value-of select="$TotalWeight"></xsl:value-of>
			</OrderedWeight>
			<OrderedCube>					
				<xsl:value-of select="$TotalCube"></xsl:value-of>
			</OrderedCube>
			<ReceivedCasesorActualCases>
				<xsl:value-of select="$TotalReceivedCases"></xsl:value-of>
			</ReceivedCasesorActualCases>
			<ReceivedWeightorActualWeight>
				<xsl:value-of select="$TotalWeight"></xsl:value-of>
			</ReceivedWeightorActualWeight>
			<ReceivedPalletsorActualPallets>			
			<xsl:value-of select="$TotalReceivedPallets"></xsl:value-of>
		</ReceivedPalletsorActualPallets>
		<ReceivedCubeorActualCubeg>
			<!--<xsl:value-of select="$PathToDATA//TOTALRECEIVEDCUBE"></xsl:value-of>-->
		</ReceivedCubeorActualCubeg>
		<ReceivedPoints></ReceivedPoints>
		<ReceivedCost></ReceivedCost>
		<ReceivedBracketPrice></ReceivedBracketPrice>
		<ActualAllowance></ActualAllowance>
		<BuyerAllowance></BuyerAllowance>
		<ActualArriveDateTime></ActualArriveDateTime>
		<ScheduledArriveDateTime></ScheduledArriveDateTime>
		<ActualPickupDateTime></ActualPickupDateTime>
		<SuperVendor></SuperVendor>
		<PalletType></PalletType>
		<CommodityCodeorProductGroup>
			<xsl:value-of select="$PathToDATA//pruductgroup"></xsl:value-of>
		</CommodityCodeorProductGroup>
		<TrailerType></TrailerType>
		<CPUBackhaul></CPUBackhaul>
		<BuyerComments></BuyerComments>
		<TempType></TempType>
		<ReleaseNumber></ReleaseNumber>
		<LocationHostID></LocationHostID>
		<CustomFlag1></CustomFlag1>
		<CustomFlag2></CustomFlag2>
		<AppointmentID></AppointmentID>
		<VendorInPallets></VendorInPallets>
		<VendorInPalletType></VendorInPalletType>
		<VendorOutPallets></VendorOutPallets>
		<VendorOutPalletType></VendorOutPalletType>
		<ConsigneeInPallets></ConsigneeInPallets>
		<ConsigneeInPalletType></ConsigneeInPalletType>
		<ConsigneeOutPallets></ConsigneeOutPallets>
		<ConsigneeOutPalletType></ConsigneeOutPalletType>
		<OrderStartTime>
			<xsl:value-of select="$PathToDATA//STARTRECEIPTDATE"></xsl:value-of>
		</OrderStartTime>
		<OrderStopTime>
			<xsl:value-of select="$PathToDATA//LASTRECEIPTDATE"></xsl:value-of>
		</OrderStopTime>
		<DoorName>
			<xsl:value-of select="$PathToDATA//DOOR"></xsl:value-of>
		</DoorName>
		<TransportType></TransportType>
		<Dropswitch></Dropswitch>
		<CarrierName></CarrierName>
		<TrailerNumber></TrailerNumber>
		<PartCode></PartCode>
		<Sequence></Sequence>
		<Duration></Duration>
		<ReferenceNumber></ReferenceNumber>
		<LINES>
		  <xsl:for-each select="DATACOLLECTION/DATA/LINES/LINE[SKU]">
        <Owner_Code>
          <xsl:value-of select="$PathToDATA//CONSIGNEE"></xsl:value-of>
        </Owner_Code>
        <PO_Number>
          <xsl:value-of select="$PathToDATA//ORDERID"></xsl:value-of>
        </PO_Number>
        <Order_Date>
          <xsl:value-of select="$PathToDATA//CREATEDATE"></xsl:value-of>
        </Order_Date>
        <ProductKey>
          <xsl:value-of select="SKU"/>
		    </ProductKey>
		    <Quantity>
			    <xsl:value-of select="QTYADJUSTED"/>
		    </Quantity>
		    <ProductDescription></ProductDescription>
		  </xsl:for-each>
		</LINES>
		</DATA>
			</DATACOLLECTION>
	</xsl:template>
</xsl:stylesheet>
