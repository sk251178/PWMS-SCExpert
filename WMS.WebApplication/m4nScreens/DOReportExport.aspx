<%@ Page Language="vb" AutoEventWireup="false" Codebehind="DOReportExport.aspx.vb" Inherits="WMS.WebApp.DOReportExport" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Quick Report</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<META HTTP-EQUIV="Expires" CONTENT="Sat, 1 Jan 2000 12:00:00 GMT">
		<!-- #include file="~/include/head.html" -->
	</HEAD>
	<body onload="dispRpt()" MS_POSITIONING="FlowLayout" bottomMargin="0" leftMargin="0" topMargin="0"
		rightMargin="0">
		<!-- #include file="~/include/Header.html" -->
		<form id="Form1" method="post" runat="server">
            <telerik:RadScriptManager ID="RadScriptManager2" runat="server">     </telerik:RadScriptManager> 
			<cc3:Screen id="Screen" runat="server" HideMenu="True" DESIGNTIMEDRAGDROP="7" NoLoginRequired="True"></cc3:Screen>
			<asp:PlaceHolder id="ph" runat="server"></asp:PlaceHolder>
		</form>
		<script language="javascript">try{dispRpt();}catch(e){}</script>
	</body>
</HTML>
