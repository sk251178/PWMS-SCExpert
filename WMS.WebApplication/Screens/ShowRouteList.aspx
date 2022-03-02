<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ShowRouteList.aspx.vb" Inherits="WMS.WebApp.ShowRouteList" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Routes</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<cc3:screen id="Screen1" runat="server" NoLoginRequired="True" ScreenID="rl" Hidden="True" HideBanner="True"
				HideMenu="True"></cc3:screen><cc2:tableeditor id="TEROUTE" runat="server" DefaultDT="DTROUTE" DisableSwitches="rmnveid" DESIGNTIMEDRAGDROP="60"
				DefaultMode="Grid"></cc2:tableeditor></form>
	</body>
</HTML>
