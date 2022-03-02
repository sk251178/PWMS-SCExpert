<%@ Page Language="vb" AutoEventWireup="false"%><!-- Codebehind="HUTInbound.aspx.vb" Inherits="WMS.WebApp.HUTInbound" %>-->
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>HUTInbound</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<!-- #include file="~/include/head.html" -->
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout">
		<!-- #include file="~/include/Header.html" -->
		<form id="Form1" method="post" runat="server">
            <telerik:RadScriptManager ID="RadScriptManager2" runat="server">     </telerik:RadScriptManager> 
			<cc3:screen id="Screen1" runat="server" ScreenID="huti"></cc3:screen>
			<P><cc2:tableeditor id="TEHUTInbound" runat="server" DisableSwitches="nime" ForbidRequiredFieldsInSearchMode="True"
					SearchDT="DTHUTransaction" DESIGNTIMEDRAGDROP="14" GridPageSize="5" DefaultMode="Search" DefaultDT="DTHUTransaction"
					EditDT="DTHUTransaction" InsertDT="DTHUTransaction" GridDT="DTHUTransaction" ViewDT="DTHUTransaction"
					AutoSelectGridItem="Never" AutoSelectMode="View" FilterExpression="TRANSACTIONTYPE IN ('INBOUND','RECEIPT')"></cc2:tableeditor></P>
			<P>&nbsp;</P>
		</form>
	</body>
</HTML>
