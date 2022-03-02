<%@ Page Language="vb" AutoEventWireup="false" Codebehind="PostCharges.aspx.vb" Inherits="WMS.WebApp.PostCharges"%>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Post Charges</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<!-- #include file="~/include/head.html" -->
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout">
		<!-- #include file="~/include/Header.html" -->
		<form id="Form1" method="post" runat="server">
            <telerik:RadScriptManager ID="RadScriptManager2" runat="server">     </telerik:RadScriptManager> 
			<p><cc3:screen id="Screen1" runat="server" ScreenID="ac"></cc3:screen></p>
			<p><cc2:tableeditor id="TEBillAdditional" runat="server" InsertDT="DTBillingAdditionalAdd" DisableSwitches="mnvr"
					DESIGNTIMEDRAGDROP="14" GridPageSize="5" DefaultMode="search" DefaultVS="2" ManualMode="False"
					DefaultDT="DTBillingAdditionalSearch" AfterInsertMode="Grid" EditDT="DTBillingAdditionalEdit"
					SearchDT="DTBillingAdditionalSearch" ForbidRequiredFieldsInSearchMode="True" AutoSelectGridItem="Never"
					AutoSelectMode="View"></cc2:tableeditor></p>
		</form>
	</body>
</HTML>
