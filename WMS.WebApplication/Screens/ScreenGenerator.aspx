<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ScreenGenerator.aspx.vb" Inherits="WMS.WebApp.ScreenGenerator" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
	<title></title>
	<!-- #include file="~/include/head.html" -->
</head>
<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" >
	<!-- #include file="~/include/Header.html" -->
	<form id="Form1" method="post" runat="server">
    	<telerik:RadScriptManager ID="RadScriptManager2" runat="server">     </telerik:RadScriptManager> 
			<cc3:Screen ScreenID="" id="oScreen" runat="server"    DESIGNTIMEDRAGDROP="7"></cc3:Screen>
           	<cc2:ScreenGenerator  id="oSG" runat="server"></cc2:ScreenGenerator>
	</form>
</body>
</html>
