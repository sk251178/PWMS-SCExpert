<%@ Page Language="vb" AutoEventWireup="false" Codebehind="RouteStop.aspx.vb" Inherits="WMS.WebApp.RouteStop" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>RouteStop</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" name="Form2" method="post" runat="server">
			<cc3:screen id="Screen1" runat="server" HideMenu="True" HideBanner="True" NoLoginRequired="True"></cc3:screen>
			<cc2:tableeditor id="TERouteStops" runat="server" DefaultMode="Grid" DESIGNTIMEDRAGDROP="60" DisableSwitches="seidtrmnv"
				Visible="false" DefaultDT="DTRouteStopDetail" HideActionBar="True"></cc2:tableeditor>
			<asp:textbox id="hdVal1" style="DISPLAY: none" runat="server"></asp:textbox>
			<asp:textbox id="hdValCommand" style="DISPLAY: none" runat="server"></asp:textbox>
			<asp:textbox id="hdPointId" style="DISPLAY: none" runat="server"></asp:textbox>
		</form>
		<SCRIPT language="JavaScript">
			try
			{
				TERouteStops_TableEditorGridTERouteStops_Grid.style.width="255px";
			}
			catch (ex){}
		</SCRIPT>
	</body>
</HTML>
