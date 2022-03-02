<?xml version="1.0" encoding="UTF-8"?>
<!-- General Entries -->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:variable name="DefaultContactID" select="DATACOLLECTION/DATA/DEFAULTCONTACT"></xsl:variable>
	<xsl:variable name="PathToContact" select="DATACOLLECTION/DATA/CONTACTS/CONTACT[CONTACTID=$DefaultContactID]"></xsl:variable>
	<xsl:template match="/">
		<DATACOLLECTION>
			<DATA>
				<VendorCode>
					<xsl:value-of select="DATACOLLECTION/DATA/COMPANY"></xsl:value-of>
				</VendorCode>
        <FacilityCodeorOwnerCode>HYV</FacilityCodeorOwnerCode>
        <VendorName>
					<xsl:value-of select="DATACOLLECTION/DATA/COMPANYNAME"></xsl:value-of>
				</VendorName>
				<BuyerCode>
          <xsl:value-of select="DATACOLLECTION/DATA/CONSIGNEE"></xsl:value-of>
        </BuyerCode>
				<BrokerCode></BrokerCode>
				<AutoRoute></AutoRoute>
				<CalcBracketPrice></CalcBracketPrice>
				<ManagedType></ManagedType>
				<Streamline></Streamline>
				<VendorGroup>
					<xsl:value-of select="DATACOLLECTION/DATA/COMPANYGROUP"></xsl:value-of>
				</VendorGroup>
				<InternalComments></InternalComments>
				<PublicComments>
					<xsl:value-of select="DATACOLLECTION/DATA/DELIVERYCOMMENTS"></xsl:value-of>
				</PublicComments>
				<AddressType>ORI</AddressType>
				<Address1>
					<xsl:value-of select="$PathToContact/STREET1"></xsl:value-of>
				</Address1>
				<Address2>
					<xsl:value-of select="$PathToContact/STREET2"></xsl:value-of>
				</Address2>
				<City>
					<xsl:value-of select="$PathToContact/CITY"></xsl:value-of>
				</City>
				<State>
					<xsl:value-of select="$PathToContact/STATE"></xsl:value-of>
				</State>
				<Zip>
					<xsl:value-of select="$PathToContact/ZIP"></xsl:value-of>
				</Zip>
				<Country></Country>
				<ContactType>CON</ContactType>
				<ContactName>
					<xsl:value-of select="$PathToContact/CONTACT1NAME"></xsl:value-of>
				</ContactName>
				<Title></Title>
				<JobDescription></JobDescription>
				<Phone>
					<xsl:value-of select="$PathToContact/CONTACT1PHONE"></xsl:value-of>
				</Phone>
				<MobilePhone>
					<xsl:value-of select="$PathToContact/CONTACT2PHONE"></xsl:value-of>
				</MobilePhone>
				<Fax></Fax>
				<Email>
					<xsl:value-of select="$PathToContact/CONTACT1EMAIL"></xsl:value-of>
				</Email>
				<ContactComments></ContactComments>

				<CPUBackhaul></CPUBackhaul>
				<CustomFlag1></CustomFlag1>
				<CustomFlag2></CustomFlag2>
				<PalletType>CONTAINER</PalletType>
				<TrailerType></TrailerType>
				<Ext></Ext>
				<SchedulingPreference></SchedulingPreference>
				<FixedTime>SERVICETIME</FixedTime>
				<ScheduleQuantityType></ScheduleQuantityType>
				<WebSchedule></WebSchedule>
				<WebScheduleReview></WebScheduleReview>
				<TrackLoadCapacity></TrackLoadCapacity>
				<MaxAppointments></MaxAppointments>
				<DropAgreement></DropAgreement>
			</DATA>
			</DATACOLLECTION>
	</xsl:template>
</xsl:stylesheet>
