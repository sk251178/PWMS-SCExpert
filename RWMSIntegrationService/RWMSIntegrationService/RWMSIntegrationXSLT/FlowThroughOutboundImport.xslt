<?xml version="1.0" encoding="UTF-8"?>
<!-- Purchase Orders -->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:msxml="urn:schemas-microsoft-com:xslt" xmlns:cd="urn:my-scripts" exclude-result-prefixes="xs msxml cd">
  <msxml:script language="VB" implements-prefix="cd">
    <msxml:assembly name="System.Windows.Forms" />
    <msxml:using namespace="System.IO" />
    <msxml:using namespace="System.Globalization" />
    <msxml:assembly href="C:\Projects\SCExpertRetalixTrunk\RWMSIntegrationService\RWMSIntegrationService\bin\Debug\Made4Net.DataAccess.dll" />
    <![CDATA[
		Public function ParseDate(sDate As String) as string
      return (DateTime.Parse(sDate).ToString("yyyy-MM-dd HH:mm:ss"))
    End function
    
		    Public Function GetOrderLineBySKU(ByVal orderid As String, ByVal sku As String) As String
				Dim sql As String = String.Format("select top(1) isnull(FLOWTHROUGHLINE,0) from FLOWTHROUGHDETAIL where CONSIGNEE = '{0}' and FLOWTHROUGH = '{1}' and SKU = '{2}' ", "SFS", orderid, sku)
        dim iLine as int32 = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
				''//msgbox(iLine)
        Return iline.tostring
			End Function
		]]>
  </msxml:script>

  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/">
    <DATACOLLECTION>
      <xsl:for-each select="NewDataSet/scheduleexport">
        <DATA>
          <CONSIGNEE>SFS</CONSIGNEE>
          <FLOWTHROUGH>
            <xsl:value-of select="ReferenceNumber"/>
          </FLOWTHROUGH>

          <LINES>
            <LINE>
              <FLOWTHROUGHLINE>
                <xsl:value-of select="cd:GetOrderLineBySKU(ReferenceNumber, SKU)"/>
              </FLOWTHROUGHLINE>

              <ROUTE>
                <xsl:value-of select="RouteNumber"/>
              </ROUTE>

              <STOPNUMBER>
                <xsl:value-of select="StopNumber"/>
              </STOPNUMBER>
              
            </LINE>
            
          </LINES>
        </DATA>
      </xsl:for-each>
    </DATACOLLECTION>
  </xsl:template>
</xsl:stylesheet>
