<%@ Page Language="vb" AutoEventWireup="false" Codebehind="CreateLoadSelectCRN.aspx.vb" Inherits="WMS.MobileWebApp.CreateLoadSelectCRN" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.Mobile.WebCtrls" Assembly="Made4Net.Mobile" %>
<%@ Register TagPrefix="cc1" Namespace="WMS.MobileWebApp.WebCtrls" Assembly="WMS.MobileWebApp" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>CreateLoad</title>
		<meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<!-- #include file="~/greenscreen/html/head-legacy.html" -->
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0">
		<form id="Form1" method="post" runat="server">
            <telerik:RadScriptManager ID="RadScriptManager2" runat="server">     </telerik:RadScriptManager> 
			<DIV align="center"><cc1:screen id="Screen1" runat="server"></cc1:screen></DIV>
			<DIV align="center">
				<DIV align="center">
					<TABLE id="Table1" cellSpacing="1" cellPadding="1" width="300" border="0">
						<TR>
							<TD>
								<asp:Label id="Label1" runat="server" Font-Bold="True">Enter Receipt Number</asp:Label></TD>
						</TR>
						<TR>
							<TD>
								<asp:TextBox id="CRN" runat="server"></asp:TextBox>
								<asp:Button id="btnGo" runat="server" Text=" Go "></asp:Button></TD>
						</TR>
						<TR>
							<TD>
								<asp:Label id="lbl" runat="server">Receipt Not Found</asp:Label></TD>
						</TR>
					</TABLE>
				</DIV>
			</DIV>
		</form>
	</body>
</HTML>
