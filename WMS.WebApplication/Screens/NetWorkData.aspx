<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="NetWorkData.aspx.vb" Inherits="WMS.WebApp.NetWorkData" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>ShowPoint</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<cc3:screen id="Screen1" runat="server" HideBanner="True" HideMenu="True" NoLoginRequired="True"></cc3:screen><br>
			<cc2:textbox id="txtCmd" name="txtCmd" style="DISPLAY:none" runat="server" EnableViewState="False"></cc2:textbox>
			<cc2:textbox id="txtPnt" name="txtPnt" style="DISPLAY:none" runat="server" EnableViewState="False"></cc2:textbox>
			<input type="button" id="sbmt" style="DISPLAY:none" name="sbmt" onclick="showData()">
			<cc2:tableeditor id="TEPoint" runat="server" DefaultDT="DTNETWORK" AfterUpdateMode="View" DisableSwitches="rmns"
				DESIGNTIMEDRAGDROP="60" DefaultMode="View"></cc2:tableeditor></form>
		<script language="javascript">
		function showData()
		{
			Form1.submit();
		}
		</script>
	</body>
</HTML>
