<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="PoliciesLookup.aspx.vb" Inherits="WMS.WebApp.PoliciesLookup" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>PoliciesLookup</title>
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
			<P><cc3:screen id="Screen1" runat="server" ScreenID="lpl"></cc3:screen></P>
			<P><cc2:tableeditor id="TELoadingPoliciesLookup" runat="server" DefaultMode="Grid" DESIGNTIMEDRAGDROP="60"
					DisableSwitches="rnmt" AfterUpdateMode="Grid" DefaultDT="DTLoadingPoliciesLookup" AutoSelectGridItem="Never"
					AutoSelectMode="View"></cc2:tableeditor><BR>
			</P>
		</form>
	</body>
</HTML>
