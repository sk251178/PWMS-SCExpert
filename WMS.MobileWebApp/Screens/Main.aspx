<%@ Register TagPrefix="cc2" Namespace="Made4Net.Mobile.WebCtrls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="WMS.MobileWebApp.WebCtrls" Assembly="WMS.MobileWebApp" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Main.aspx.vb" Inherits="WMS.MobileWebApp.Nav" %>
<%@ Register TagPrefix="cc3" Namespace="Made4Net.Mobile.WebCtrls" Assembly="Made4Net.Mobile" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Nav</title>
		<meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<!-- #include file="~/greenscreen/html/head-legacy.html" -->
	</HEAD>
	<body dir="ltr" bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
            <telerik:RadScriptManager ID="RadScriptManager2" runat="server">     </telerik:RadScriptManager> 
			<div align="center">
				<cc1:screen id="Screen1" runat="server" ShowMainMenu="True" ScreenID="rdtmain"></cc1:screen>
				<cc3:NavigationInterface id="NavigationInterface1" runat="server" ShowMainMenu="True" ShowScreenMenu="False"></cc3:NavigationInterface>
			</div>
		</form>
	</body>
</HTML>
