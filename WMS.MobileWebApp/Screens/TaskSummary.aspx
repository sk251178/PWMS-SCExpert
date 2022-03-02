<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TaskSummary.aspx.vb" Inherits="WMS.MobileWebApp.TaskSummary" %>
<%@ Register TagPrefix="cc3" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.Mobile.WebCtrls" Assembly="Made4Net.Mobile" %>
<%@ Register TagPrefix="cc1" Namespace="WMS.MobileWebApp.WebCtrls" Assembly="WMS.MobileWebApp" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>CLD1</title>
		<meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<!-- #include file="~/greenscreen/html/head-legacy.html" -->
	</HEAD>
<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0">
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager2" runat="server">     </telerik:RadScriptManager> 
        <div align="center"><cc1:screen id="Screen1" title="Task Summary" runat="server" ScreenID="TSU" /></div>
		<div align="center" width="100%"><cc2:dataobject id="DO1" runat="server" LeftButtonText="Next" title="Task Summary" /></div>
	</form>	      
</body>
</html>
