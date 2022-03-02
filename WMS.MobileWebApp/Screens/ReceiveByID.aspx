<%@ Register TagPrefix="cc1" Namespace="WMS.MobileWebApp.WebCtrls" Assembly="WMS.MobileWebApp" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.Mobile.WebCtrls" Assembly="Made4Net.Mobile" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ReceiveByID.aspx.vb" Inherits="WMS.MobileWebApp.ReceiveByID" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>ReceiveByID</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<!-- #include file="~/greenscreen/html/head-legacy.html" -->
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0">
		<form id="Form1" method="post" runat="server">
            <telerik:RadScriptManager ID="RadScriptManager2" runat="server">     </telerik:RadScriptManager> 
			<DIV align="center"><cc1:screen id="Screen1" runat="server" ScreenID="RECASN"></cc1:screen></DIV>
			<DIV align="center"><cc2:dataobject id="DO1" title="Receive By ID" runat="server" RightButtonText="Menu" LeftButtonText="Next"></cc2:dataobject></DIV>
		</form>
	</body>
</HTML>
