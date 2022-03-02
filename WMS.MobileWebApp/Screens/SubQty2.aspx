<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SubQty2.aspx.vb" Inherits="WMS.MobileWebApp.SubQty2" %>
<%@ Register TagPrefix="cc1" Namespace="WMS.MobileWebApp.WebCtrls" Assembly="WMS.MobileWebApp" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.Mobile.WebCtrls" Assembly="Made4Net.Mobile" %>
<%@ Register TagPrefix="cc3" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">

<html>
<head>
    <title>Inventory Adjustment – Sub Quantity</title>
	<!-- #include file="~/greenscreen/html/head-legacy.html" -->
</head>
<body  bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0">
		<form id="Form1" method="post" runat="server">
            <telerik:RadScriptManager ID="RadScriptManager2" runat="server">     </telerik:RadScriptManager> 
			<div align="center"><cc1:screen id="Screen1" runat="server" title="Inventory Adjustment – Sub Quantity" ScreenID="rdtSubQty2"></cc1:screen></div>
			<div align="center" width="100%">
			<cc2:dataobject id="DO1" runat="server" LeftButtonText="Next" RightButtonText="Back" CenterLeftButtonText="AddUnits" CenterButtonText="ClearUnits"  DefaultButton="Next"
			title="Inventory Adjustment – Sub Quantity"></cc2:dataobject></div>
		</form>
</body>
</html>
