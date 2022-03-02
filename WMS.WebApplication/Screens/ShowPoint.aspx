<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ShowPoint.aspx.vb" Inherits="WMS.WebApp.ShowPoint" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>ShowPoint</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style>
			body, html {
				border:none;
			}
		</style>
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout" onload="getPageUrl()">
		<form id="Form1" method="post" runat="server">
			<cc3:screen id="Screen1" runat="server" NoLoginRequired="True" HideMenu="True" HideBanner="True"></cc3:screen>
			<br>
			<cc2:tableeditor id="TEPoint" runat="server" DefaultMode="View" DESIGNTIMEDRAGDROP="60" DisableSwitches="rmns"
				AfterUpdateMode="View" DefaultDT="DTNETWORK"></cc2:tableeditor></form>
	</body>
</HTML>
