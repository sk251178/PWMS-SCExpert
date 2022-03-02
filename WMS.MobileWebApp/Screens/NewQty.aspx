<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="NewQty.aspx.vb" Inherits="WMS.MobileWebApp.NewQty" %>
<%@ Register TagPrefix="cc1" Namespace="WMS.MobileWebApp.WebCtrls" Assembly="WMS.MobileWebApp" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.Mobile.WebCtrls" Assembly="Made4Net.Mobile" %>
<%@ Register TagPrefix="cc3" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">

<html>
<head>
    <title>Inventory Adjustment – Add Quantity</title>
	<!-- #include file="~/greenscreen/html/head-legacy.html" -->
</head>
<body  bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0">
		<form id="Form1" method="post" runat="server">
            <telerik:RadScriptManager ID="RadScriptManager2" runat="server"></telerik:RadScriptManager> 
			<div align="center"><cc1:screen id="Screen1" runat="server" title="Inventory Adjustment – New Quantity" ScreenID="rdtNewQty"></cc1:screen></div>
			<DIV align="center" width="100%">
			<cc2:dataobject id="DO1" runat="server" LeftButtonText="Next" RightButtonText="Menu" FocusField="Loadid" DefaultButton="Next"
			title="Inventory Adjustment – New Quantity"></cc2:dataobject></DIV>
		</form>
</body>
</html>
