<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Driver.aspx.vb" Inherits="WMS.WebApp.Driver" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Driver</title>
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
				<cc3:screen id="Screen1" runat="server" ScreenID="dr" HideBanner="False"></cc3:screen>
			<P>
				<cc2:tableeditor id="TEDriver" runat="server" DefaultMode="Grid" DESIGNTIMEDRAGDROP="60" DisableSwitches="rmnvt"
					AfterUpdateMode="Grid" DefaultDT="DTDriver" EditDT="DTDriverEdit" SearchDT="DTDriverSearch"></cc2:tableeditor>
			</P>
		</form>
</HTML>
