<%@ Page Language="vb" AutoEventWireup="false" Codebehind="AddMatcher.aspx.vb" Inherits="WMS.WebApp.AddMatcher" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>AddMatcher</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<!-- #include file="~/include/head.html" -->
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout">
		<!-- #include file="~/include/Header.html" -->
		<form id="Form1" method="post" runat="server">
            <telerik:RadScriptManager ID="RadScriptManager2" runat="server">     </telerik:RadScriptManager> 
			<cc3:screen id="Screen1" runat="server" NoLoginRequired="True" HideBanner="True" HideMenu="True"></cc3:screen><cc2:tableeditor id="TEAddMatcher" runat="server" DefaultDT="DTAddMatch" DisableSwitches="sdtrmn"
				DefaultMode="Edit" AfterInsertMode="Edit" AfterUpdateMode="Edit" Visible="False"></cc2:tableeditor><asp:textbox id="hdVal1" style="DISPLAY: none" runat="server"></asp:textbox><asp:textbox id="hdVal2" style="DISPLAY: none" runat="server"></asp:textbox><asp:textbox id="hdVal3" style="DISPLAY: none" runat="server"></asp:textbox><asp:textbox id="hdObjectType" style="DISPLAY: none" runat="server"></asp:textbox><asp:textbox id="hdValCommand" style="DISPLAY: none" runat="server"></asp:textbox><asp:textbox id="hdPointId" style="DISPLAY: none" runat="server"></asp:textbox>
			<!--<table>
				<tr>
					<td><cc2:FieldLabel runat="server" id="lblCity" value="City"></cc2:FieldLabel></td>
					<td><asp:TextBox runat="server" ID="tbCity"></asp:TextBox></td>
				</tr>
				<tr>
					<td><cc2:FieldLabel runat="server" id="lblStreet" value="Street"></cc2:FieldLabel></td>
					<td><asp:TextBox runat="server" ID="tbStreet"></asp:TextBox></td>
				</tr>
				<tr>
					<td><cc2:FieldLabel runat="server" id="lblHouse" value="House Number"></cc2:FieldLabel></td>
					<td><asp:TextBox runat="server" ID="tbHouse"></asp:TextBox></td>
				</tr>
				<tr>
					<td colspan="2" align="center"><cc2:Button runat="server" id="btnMatch" value="Find"></cc2:Button></td>
				</tr>
			</table>-->
			<script language="javascript">
				//document.body.onload='';
			</script>
		</form>
	</body>
</HTML>
