<%@ Register TagPrefix="cc1" Namespace="WMS.MobileWebApp.WebCtrls" Assembly="WMS.MobileWebApp" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.Mobile.WebCtrls" Assembly="Made4Net.Mobile" %>
<%@ Register TagPrefix="cc3" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="MOVCNT.aspx.vb" Inherits="WMS.MobileWebApp.MOVCNT"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
  <head>
    <title>MOVCNT</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name=vs_defaultClientScript content="JavaScript">
    <meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5">
  	<!-- #include file="~/greenscreen/html/head-legacy.html" -->
</head>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0">
		<form id="Form1" method="post" runat="server">
            <telerik:RadScriptManager ID="RadScriptManager2" runat="server">     </telerik:RadScriptManager> 
			<div align="center"><cc1:screen id="Screen1" runat="server" title="MOVE CONTAINER" ScreenID="MOVCNT"></cc1:screen></div>
			<DIV align="center" width="100%"><cc2:dataobject id="DO1" runat="server" LeftButtonText="next" RightButtonText="Menu" title="Move Container"></cc2:dataobject></DIV>
		</form>
	</body>
</html>
