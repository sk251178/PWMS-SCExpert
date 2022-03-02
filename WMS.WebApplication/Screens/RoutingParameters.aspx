<%@ Page Language="vb" AutoEventWireup="false" Codebehind="RoutingParameters.aspx.vb" Inherits="WMS.WebApp.RoutingParameters" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Routing</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<cc3:screen id="Screen1" runat="server" ScreenID="tp"></cc3:screen>
			<br>
			<cc2:tableeditor id="TERoutingParameters" runat="server" DefaultMode="Grid" DESIGNTIMEDRAGDROP="60"
				AfterUpdateMode="Grid" DefaultDT="DTROUTINGPARAMS"></cc2:tableeditor>
		</form>
	</body>
</HTML>
