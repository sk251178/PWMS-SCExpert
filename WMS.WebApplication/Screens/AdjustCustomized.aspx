<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="AdjustCustomized.aspx.vb" Inherits="WMS.WebApp.AdjustCustomized"%>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Indent Management</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<!-- #include file="~/include/head.html" -->
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout">
		<!-- #include file="~/include/Header.html" -->
		<FORM id="Form1" method="post" runat="server">
            <telerik:RadScriptManager ID="RadScriptManager2" runat="server">     </telerik:RadScriptManager> 
			<cc3:screen id="Screen1" runat="server" ScreenID="ind"></cc3:screen>
			<P><cc2:tableeditor id="TEIndent" ObjectID="TEIndent" runat="server" DefaultDT="DTIndentGrid" ManualMode="False" DefaultMode="Search"
					TableName="INDENTMANAGEMENT" GridPageSize="80" DESIGNTIMEDRAGDROP="14" SearchDT="DTIndentSearch" GridDT="DTIndentGrid"
					InsertDT="DTIndentEdit" EditDT="DTIndentEdit" DisableSwitches="mn" ForbidRequiredFieldsInSearchMode="True" AutoSelectGridItem="Never"
					AutoSelectMode="View"></cc2:tableeditor></P>
			<P>&nbsp;</P>
		</FORM>
	</body>
</HTML>
