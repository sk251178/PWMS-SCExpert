<%@ Page Language="vb" AutoEventWireup="false" Codebehind="MenuEditor.aspx.vb" Inherits="WMS.WebApp.MenuEditor" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Register TagPrefix="cc1" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<html>
	<head>
    	<title>MenuEditor</title>
    	<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    	<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    	<meta name=vs_defaultClientScript content="JavaScript">
    	<meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5">
  		<!-- #include file="~/include/head.html" -->
	</head>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0">
		<!-- #include file="~/include/Header.html" -->
		<form id="Form1" method="post" runat="server">
            <telerik:RadScriptManager ID="RadScriptManager2" runat="server">     </telerik:RadScriptManager> 
			<cc3:Screen id="Screen" runat="server" DESIGNTIMEDRAGDROP="7"></cc3:Screen>
			<cc1:MenuEditor Mode="Users" id="MenuEditor1" runat="server"></cc1:MenuEditor>
		</form>
	</body>
</html>
