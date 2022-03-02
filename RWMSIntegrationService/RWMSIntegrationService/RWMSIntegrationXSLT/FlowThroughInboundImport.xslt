<?xml version="1.0" encoding="UTF-8"?>
<!-- Purchase Orders -->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:msxml="urn:schemas-microsoft-com:xslt" xmlns:cd="urn:my-scripts" exclude-result-prefixes="xs msxml cd">
  <msxml:script language="VB" implements-prefix="cd">
    <msxml:assembly name="System.Windows.Forms" />
    <msxml:using namespace="System.IO" />
    <msxml:using namespace="System.Globalization" />
    <![CDATA[
		Public function ParseDate(sDate As String) as string
      return (DateTime.Parse(sDate).ToString("yyyy-MM-dd HH:mm:ss"))
    End function
		]]>
  </msxml:script>

  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/">
    <DATACOLLECTION>
      <xsl:for-each select="NewDataSet/scheduleexport">
        <DATA>
          <CONSIGNEE>
            <xsl:value-of select="orderconsigneecode"/>
          </CONSIGNEE>
          <FLOWTHROUGH>
            <xsl:value-of select="ReferenceNumber"/>
          </FLOWTHROUGH>
          <APPOINTMENTID>
            <xsl:value-of select="Appointmentid"/>
          </APPOINTMENTID>
          <CARRIERID>
            <xsl:value-of select="carriercode"/>
          </CARRIERID>
          <SCHEDULEDARRIVALDATE>
            <xsl:value-of select="cd:ParseDate(starttime)"/>
          </SCHEDULEDARRIVALDATE>
        </DATA>
      </xsl:for-each>
    </DATACOLLECTION>
  </xsl:template>
</xsl:stylesheet>
