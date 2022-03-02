<%@ Page Language="vb" AutoEventWireup="false" Codebehind="CrossDockDeliveryLoc.aspx.vb" Inherits="WMS.WebApp.CrossDockDeliveryLoc" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>CrossDockDeliveryLoc</title>
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
			<cc3:screen id="Screen1" runat="server" ScreenID="cddl"></cc3:screen>
			<P><cc2:tableeditor id="TECrossDockDelLoc" runat="server" DisableSwitches="n" ForbidRequiredFieldsInSearchMode="True"
					SearchDT="DTXDOCKDELIVERYLOC" DESIGNTIMEDRAGDROP="14" GridPageSize="5" DefaultMode="Search"
					ManualMode="False" DefaultDT="DTXDOCKDELIVERYLOC" EditDT="DTXDOCKDELIVERYLOC" InsertDT="DTXDOCKDELIVERYLOC"
					AfterInsertMode="Grid" GridDT="DTXDOCKDELIVERYLOC" ViewDT="DTXDOCKDELIVERYLOC" AutoSelectGridItem="Never"
					AutoSelectMode="View"></cc2:tableeditor></P>
			<P>&nbsp;</P>
		</form>
	</body>
</HTML>
