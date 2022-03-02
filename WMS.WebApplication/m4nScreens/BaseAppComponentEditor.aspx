<%@ Register TagPrefix="uc1" TagName="BaseAppComponentEditor" Src="BaseAppComponentEditor.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="BaseAppComponentEditor.aspx.vb" Inherits="WMS.WebApp.BaseAppComponentEditor1" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>BaseAppComponentEditor</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<!-- #include file="~/include/head.html" -->
	</HEAD>
	<body MS_POSITIONING="FlowLayout" bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0">
		<!-- #include file="~/include/Header.html" -->
		<form id="Form1" method="post" runat="server">
            <telerik:RadScriptManager ID="RadScriptManager2" runat="server">     </telerik:RadScriptManager> 
			<cc3:Screen id="Screen" runat="server" DESIGNTIMEDRAGDROP="7" HideTitle="False" HideBanner="False"
				HideHeader="False"></cc3:Screen>
			<uc1:BaseAppComponentEditor id="BaseACEditor" runat="server"></uc1:BaseAppComponentEditor>
		</form>
	</body>
</HTML>
