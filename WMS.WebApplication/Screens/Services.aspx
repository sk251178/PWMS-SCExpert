<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Services.aspx.vb" Inherits="WMS.WebApp.Services" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<html>
  <head>
    <title>Services</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name=vs_defaultClientScript content="JavaScript">
    <meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5">
    <!-- #include file="~/include/head.html" -->
  </head>
  <body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout">
		<!-- #include file="~/include/Header.html" -->
		<form id="Form1" method="post" runat="server">
            <telerik:RadScriptManager ID="RadScriptManager2" runat="server">     </telerik:RadScriptManager> 
			<P>
				<cc3:screen id="Screen1" runat="server" ScreenID="se"></cc3:screen></P>
			<P>
				<cc2:tableeditor id="TEServices" runat="server" DefaultDT="DTservicesVIEW" DisableSwitches="rietnv"
					DESIGNTIMEDRAGDROP="60" DefaultMode="Search"
					ForbidRequiredFieldsInSearchMode="True"></cc2:tableeditor></P>
		</form>
		</FORM>
	</body>
</html>
