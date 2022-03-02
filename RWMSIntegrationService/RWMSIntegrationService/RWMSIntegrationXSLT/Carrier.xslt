<?xml version="1.0" encoding="utf-8"?>
<!-- General Entries -->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:variable name="PathToDATA" select="DATACOLLECTION/DATA"></xsl:variable>
	<xsl:variable name="PathToCONTACT" select="$PathToDATA/CONTACTS/CONTACT"></xsl:variable>
		<xsl:template match="/">
			<DATACOLLECTION>
				<DATA>
					<CarrierCode>
						<xsl:value-of select="$PathToDATA/CARRIER"/>
					</CarrierCode>
					<CarrierName>
						<xsl:value-of select="$PathToDATA/CARRIERNAME"/>
					</CarrierName>
					<CarrierType></CarrierType>
					<StopCharge></StopCharge>
					<UnloadCharge></UnloadCharge>
					<ServiceLevel></ServiceLevel>
					<MCNumber></MCNumber>
					<SCAC></SCAC>
					<NotifyPreference>0</NotifyPreference>
					<Active>T</Active>
					<WebSchedule>F</WebSchedule>
					<WebScheduleReview>F</WebScheduleReview>
					<PrintNotify></PrintNotify>
					<FaxNotify></FaxNotify>
					<EDINotify></EDINotify>
					<EmailNotify></EmailNotify>
					<WebNotify></WebNotify>
					<FleetExportNotify></FleetExportNotify>
					<AutoNotify></AutoNotify>
					<CargoInsuranceLimit></CargoInsuranceLimit>
					<LiabilityLimit></LiabilityLimit>
					<InsuranceExpirationDate></InsuranceExpirationDate>
					<LiabilityExpirationDate></LiabilityExpirationDate>
					<AutoApprovePayment></AutoApprovePayment>
					<CarrierComments>
						<xsl:value-of select="$PathToDATA/NOTES"/>
					</CarrierComments>
					<ContactType></ContactType>
					<ContactName>
						<xsl:value-of select="$PathToCONTACT/CONTACT1NAME"/>
					</ContactName>
					<Title></Title>
					<JobDescription></JobDescription>
					<Phone>
						<xsl:value-of select="$PathToCONTACT/CONTACT1PHONE"/>
					</Phone>
					<MobilePhone>
						<xsl:value-of select="$PathToCONTACT/CONTACT2PHONE"/>
					</MobilePhone>
					<Fax>
						<xsl:value-of select="$PathToCONTACT/CONTACT1FAX"/>
					</Fax>
					<Email>
						<xsl:value-of select="$PathToCONTACT/CONTACT1EMAIL"/>
					</Email>
					<ContactComments></ContactComments>
					<AddressType></AddressType>
					<Address1>
						<xsl:value-of select="$PathToCONTACT/STREET1"/>
					</Address1>
					<Address2>
						<xsl:value-of select="$PathToCONTACT/STREET2"/>
					</Address2>
					<City>
						<xsl:value-of select="$PathToCONTACT/CITY"/>
					</City>
					<State>
						<xsl:value-of select="$PathToCONTACT/STATE"/>
					</State>
					<Zip>
						<xsl:value-of select="$PathToCONTACT/ZIP"/>
					</Zip>
					<Country></Country>
					<WebAddress></WebAddress>
					<APNumber></APNumber>
					<RoutedOrderExport></RoutedOrderExport>
					<InvoiceOptionType></InvoiceOptionType>
					<Terms></Terms>
					<OwnerCode></OwnerCode>
					<CarrierActive></CarrierActive>
				</DATA>
			</DATACOLLECTION>
	</xsl:template>
</xsl:stylesheet>
