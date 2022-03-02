<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="StatusWait.aspx.vb" Inherits="WMS.MobileWebApp.StatusWait" %>
<%@ Register TagPrefix="cc1" Namespace="WMS.MobileWebApp.WebCtrls" Assembly="WMS.MobileWebApp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >

<head id="Head1" runat="server">

    <title>Simple Waiting Page</title>

	<!-- #include file="~/greenscreen/html/head-legacy.html" -->
</head>

<body dir="ltr" bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout">
    <form id="form1" runat="server">
        <div align="center">
        <cc1:screen id="Screen1" runat="server" ShowMainMenu="True" ScreenID="rdtmain"></cc1:screen>
        <br />
        <br />

        <p align="center">

            <asp:Image ID="ImageStatus" ImageUrl="~/Images/wait.gif" runat="server" /></p>

            <h1>

                <asp:Label ID="lblStatus" Font-Size=Small   runat="server" Text="Working... Please wait..."></asp:Label>

            </h1>

        </div>

    </form>
</body>
</html>
