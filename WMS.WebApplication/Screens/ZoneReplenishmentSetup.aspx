<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ZoneReplenishmentSetup.aspx.vb" Inherits="WMS.WebApp.ZoneReplenishmentSetup"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<html>
  <head>
    <title>ZoneReplenishmentSetup</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name=vs_defaultClientScript content="JavaScript">
    <meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5">
	<!-- #include file="~/include/Head.html" -->
  </head>
  	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout">
		<!-- #include file="~/include/Header.html" -->
		<form id="Form1" method="post" runat="server">
            <telerik:RadScriptManager ID="RadScriptManager2" runat="server">     </telerik:RadScriptManager> 
			<cc3:screen id="Screen1" runat="server" ScreenID="zs"></cc3:screen>
			<P><cc2:tableeditor id="TEReplenish" runat="server" DESIGNTIMEDRAGDROP="14" DefaultMode="Grid" DefaultDT="DTZoneRepl"
					SearchDT="DTZoneReplSearch" DisableSwitches="nrt" AutoSelectGridItem="Never" AutoSelectMode="View"
					InsertDT="DTZoneReplInsert" EditDT="DTZoneReplEdit" ObjectID="TEReplenish" ForbidRequiredFieldsInSearchMode="True"></cc2:tableeditor></P>
			<P>&nbsp;</P>
		</form>
	</body>
</html>
