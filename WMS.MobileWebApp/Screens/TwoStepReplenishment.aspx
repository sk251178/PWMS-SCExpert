<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TwoStepReplenishment.aspx.vb" Inherits="WMS.MobileWebApp.TwoStepReplenishment" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.Mobile.WebCtrls" Assembly="Made4Net.Mobile" %>
<%@ Register TagPrefix="cc1" Namespace="WMS.MobileWebApp.WebCtrls" Assembly="WMS.MobileWebApp" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Two Step Replenishment</title>
		<meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<!-- #include file="~/greenscreen/html/head-legacy.html" -->
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0">
		<form id="Form1" method="post" runat="server">
            <telerik:RadScriptManager ID="RadScriptManager2" runat="server">     </telerik:RadScriptManager> 
    			<div align="center"><cc1:screen id="Screen1" runat="server" title="TwoStepReplenishment" ScreenID="tsrep">
			</cc1:screen></div>
			<div align="center" width="100%"><cc2:dataobject id="DOVerify" runat="server" title="Two Step Replenishment"
			RightButtonText="Menu" LeftButtonText="Next"></cc2:dataobject></div>
		</form>
	</body>
</HTML>

