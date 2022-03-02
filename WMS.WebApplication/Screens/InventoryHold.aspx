<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="InventoryHold.aspx.vb" Inherits="WMS.WebApp.InventoryHold" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Inventory Hold</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<!-- #include file="~/include/head.html" -->
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout">
		<!-- #include file="~/include/Header.html" -->
		<form id="Form2" method="post" runat="server">
            <telerik:RadScriptManager ID="RadScriptManager2" runat="server">     </telerik:RadScriptManager> 
			<cc3:screen id="Screen1" runat="server" ScreenID="ih"></cc3:screen>
			<br>
			<cc2:tableeditor id="TEINVLOADADJ" runat="server" DefaultDT="DTINVADJSTATUSSEARCH" ManualMode="False"
				DefaultMode="Search" GridPageSize="5" DESIGNTIMEDRAGDROP="14" EditVS="0" DisableSwitches="vidre"
				ForbidRequiredFieldsInSearchMode="True" GridDT="DTINVLOADSTATUSGRID" SearchDT="DTINVADJSTATUSSEARCH"
				FilterExpression="status is not null and status <> ''" MultiEditDT="DTINVLOADSTATUSMULTI" AutoSelectGridItem="Never"
				AutoSelectMode="View"></cc2:tableeditor>
		</form>
	</body>
</HTML>
