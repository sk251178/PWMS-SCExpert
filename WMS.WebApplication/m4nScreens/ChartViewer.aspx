<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ChartViewer.aspx.vb" Inherits="WMS.WebApp.ChartViewer" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Register TagPrefix="cc4" Namespace="Made4Net.WebControls.Charting" Assembly="Made4Net.WebControls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Quick Chart</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<META HTTP-EQUIV="Expires" CONTENT="Sat, 1 Jan 2000 12:00:00 GMT">
		<!-- #include file="~/include/head.html" -->
	</HEAD>
	<body MS_POSITIONING="FlowLayout" bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0">
		<!-- #include file="~/include/Header.html" -->
		<form id="Form1" method="post" runat="server">
            <telerik:RadScriptManager ID="RadScriptManager2" runat="server">     </telerik:RadScriptManager> 
			<cc3:Screen id="Screen" runat="server" HideMenu="True" DESIGNTIMEDRAGDROP="7" NoLoginRequired="True"></cc3:Screen>
			<cc4:QuickChart id="CV" runat="server"></cc4:QuickChart>
		</form>
	</body>
</HTML>
