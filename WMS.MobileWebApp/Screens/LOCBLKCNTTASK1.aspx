<%@ Page Language="vb" AutoEventWireup="false" Codebehind="LOCBLKCNTTASK1.aspx.vb" Inherits="WMS.MobileWebApp.LOCBLKCNTTASK1"%>
<%@ Register TagPrefix="cc3" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.Mobile.WebCtrls" Assembly="Made4Net.Mobile" %>
<%@ Register TagPrefix="cc1" Namespace="WMS.MobileWebApp.WebCtrls" Assembly="WMS.MobileWebApp" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>LOCBLKCNTTASK1</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<!-- #include file="~/greenscreen/html/head-legacy.html" -->
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0">
		<form id="Form1" method="post" runat="server">
            <telerik:RadScriptManager ID="RadScriptManager2" runat="server">     </telerik:RadScriptManager> 
			<div align="center"><cc1:screen id="Screen1" runat="server" title="Count Task" ScreenID="loccnblk1"></cc1:screen></div>
			<DIV align="center" width="100%"><cc2:dataobject id="DO1" runat="server" LeftButtonText="Next" RightButtonText="EndCount" title="Counting"
					CenterButtonText="Back"></cc2:dataobject></DIV>
		</form>
	</body>
</HTML>
