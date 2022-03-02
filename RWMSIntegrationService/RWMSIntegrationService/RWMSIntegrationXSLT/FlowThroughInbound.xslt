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

        <DueDate>
          <xsl:value-of select="$PathToDATA//EXPECTEDDATE"></xsl:value-of>
        </DueDate>

        <ConsigneeCode>
          <xsl:value-of select="$CONSIGNEE"></xsl:value-of>
        </ConsigneeCode>


        <VendorCode>
          <xsl:value-of select="$PathToDATA//SOURCECOMPANY"></xsl:value-of>
        </VendorCode>

        <OrderType>
          <xsl:value-of select="$PathToDATA//ORDERTYPE"></xsl:value-of>
        </OrderType>

        <BuyerCode>
          <xsl:value-of select="$CONSIGNEE"></xsl:value-of>
        </BuyerCode>

        <DestinationCode>
          <xsl:value-of select="$PathToDATA//RECEIVEDFROM"></xsl:value-of>
        </DestinationCode>

        <ManagedType>
          <xsl:value-of select="$PathToDATA//REFERENCEORD"></xsl:value-of>
        </ManagedType>

        <COMMENTS>
          <xsl:value-of select="$PathToDATA//NOTES"></xsl:value-of>
        </COMMENTS>

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

        <ReferenceNumber>
          <xsl:value-of select="$PathToDATA//LOADINGSEQ"></xsl:value-of>
        </ReferenceNumber>

        <LINES>
          <xsl:for-each select="DATACOLLECTION/DATA/LINES/LINE">
            <LINE>
              <Owner_Code>
                <xsl:value-of select="$CONSIGNEE"></xsl:value-of>
              </Owner_Code>
              <PO_Number>
                <xsl:value-of select="$PathToDATA//FLOWTHROUGH"></xsl:value-of>
              </PO_Number>
              <Order_Date>
                <xsl:value-of select="$PathToDATA//CREATEDATE"></xsl:value-of>
              </Order_Date>
              <ProductKey>
                <xsl:value-of select="SKU"/>
              </ProductKey>
              <ProductDescription>
                <xsl:value-of select="SKUDESC"/>
              </ProductDescription>
              <Quantity>
                <xsl:value-of select="QTYMODIFIED"/>
              </Quantity>
            </LINE>
          </xsl:for-each>
        </LINES>
      </DATA>
    </DATACOLLECTION>
  </xsl:template>
</xsl:stylesheet>
