<%@ Page Language="vb" AutoEventWireup="false" Codebehind="LicManScr.aspx.vb" Inherits="WMS.WebApp.LinManScr"%>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>LinManScr</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<!-- #include file="~/include/head.html" -->
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout">
		<!-- #include file="~/include/Header.html" -->
		<FORM id="Form1" method="post" runat="server">
            <telerik:RadScriptManager ID="RadScriptManager2" runat="server">     </telerik:RadScriptManager> 
			<cc3:screen id="Screen1" runat="server" ScreenID="-lc"></cc3:screen>
			<br>
			<P>
				<cc2:TableEditor id="TEAdmPnl" runat="server" AutoSelectMode="View" AutoSelectGridItem="Never" DefaultDT="DTSysLic"
					DefaultMode="Grid" ForbidRequiredFieldsInSearchMode="True" DisableSwitches="iesrnmdv" ConnectionName="Made4NetSchema"></cc2:TableEditor>
			</P>
		</FORM>
	</body>
</HTML>
