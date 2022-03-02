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
  <xsl:variable name="FLOWTHROGH" select="$PathToDATA/FLOWTHROGH/text()"></xsl:variable>
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
        <DownloadType>
          <xsl:value-of select="$PathToDATA//DOWNLOADTYPE"></xsl:value-of>
        </DownloadType>

        <OrderNumber>
          <xsl:value-of select="$PathToDATA//FLOWTHROUGH"></xsl:value-of>
        </OrderNumber>

        <OrderDate>
          <xsl:value-of select="$PathToDATA//CREATEDATE"></xsl:value-of>
        </OrderDate>

        <WarehouseCode>
          <xsl:value-of select="$CONSIGNEE"></xsl:value-of>
        </WarehouseCode>

        <StoreID>
          <xsl:value-of select="$PathToDATA//SHIPTO"></xsl:value-of>
        </StoreID>

        <OrderType>
          <xsl:value-of select="$PathToDATA//ORDERTYPE"></xsl:value-of>
        </OrderType>

        <ServiceDateTime>
          <xsl:value-of select="$PathToDATA//REQUESTEDDELIVERYDATE"></xsl:value-of>
        </ServiceDateTime>

        <ReferenceNumber>
          <xsl:value-of select="$PathToDATA//LOADINGSEQ"></xsl:value-of>
        </ReferenceNumber>



        <LINES>
          <xsl:for-each select="DATACOLLECTION/DATA/LINES/LINE">
            <LINE>
            <SKU>
              <xsl:value-of select="SKU"/>
            </SKU>

            <Quantity>
              <xsl:value-of select="QTYMODIFIED"/>
            </Quantity>
            <Description>
              <xsl:value-of select="SKUDESC"/>
            </Description>

            <ProductType>
              <xsl:value-of select="STORAGECLASS"/>
            </ProductType>

            <ExtendedWeight>
              <xsl:value-of select="EXTENDEDWEIGHT"></xsl:value-of>
            </ExtendedWeight>
            <ExtendedCube>
              <xsl:value-of select="EXTENDEDCUBE"></xsl:value-of>
            </ExtendedCube>
            </LINE>
          </xsl:for-each>
        </LINES>
      </DATA>
    </DATACOLLECTION>
  </xsl:template>
</xsl:stylesheet>
