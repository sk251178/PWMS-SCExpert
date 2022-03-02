<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="SkuGroup.aspx.vb" Inherits="WMS.WebApp.SkuGroup"%>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>SKU Group</title>
		<meta content="False" name="vs_showGrid">
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<!-- #include file="~/include/head.html" -->
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout">
		<!-- #include file="~/include/Header.html" -->
		<FORM id="Form1" method="post" runat="server">
            <telerik:RadScriptManager ID="RadScriptManager2" runat="server">     </telerik:RadScriptManager> 
			<cc3:screen id="Screen1" runat="server" ScreenCode="SKUGroup" ScreenID="sc"></cc3:screen>
			<P><cc2:tableeditor id="TESKUGroup" runat="server" DefaultDT="DTSkuGroupGrid" ManualMode="False" DefaultMode="Grid"
					GridPageSize="5" DESIGNTIMEDRAGDROP="14" ForbidRequiredFieldsInSearchMode="True" EditDT="DTSkuGroupEdit"
					AfterInsertMode="Grid" AfterUpdateMode="Grid" DisableSwitches="mn" AutoSelectGridItem="Never"
					AutoSelectMode="View" InsertDT="DTSkuGroupGrid" TableName="SKUGROUP"></cc2:tableeditor></P>		   
		</FORM>
	</body>
</HTML>
