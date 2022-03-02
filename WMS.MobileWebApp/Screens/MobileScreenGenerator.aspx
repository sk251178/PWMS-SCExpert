<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="MobileScreenGenerator.aspx.vb" Inherits="WMS.MobileWebApp.MobileScreenGenerator" %>
<%@ Register TagPrefix="cc1" Namespace="WMS.MobileWebApp.WebCtrls" Assembly="WMS.MobileWebApp" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
<title></title>
	<!-- #include file="~/greenscreen/html/head-legacy.html" -->
</head>
<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" >
<form id="Form1" method="post" runat="server">
			<cc1:Screen id="Screen1" runat="server" NoLoginRequired="True"></cc1:Screen>
            <cc2:ScreenGenerator  id="oSG" runat="server"></cc2:ScreenGenerator>

		</form>
</body>
</html>

