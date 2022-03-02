<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="SQLQuery.aspx.vb" Inherits="WMS.WebApp.SQLQuery" ValidateRequest="false" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Table Maintenance</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<!-- #include file="~/include/head.html" -->
	</HEAD>
	<body MS_POSITIONING="FlowLayout" bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0">
		<!-- #include file="~/include/Header.html" -->
		<form id="Form1" method="post" runat="server">
            <telerik:RadScriptManager ID="RadScriptManager2" runat="server">     </telerik:RadScriptManager> 
			<cc3:Screen id="Screen1" runat="server" title="Table Maintenance"></cc3:Screen>
			<P>&nbsp;
				<TABLE id="Table1" cellSpacing="0" cellPadding="3" width="350" border="0">
					<TR>
						<TD style="HEIGHT: 23px">Connection:
							<asp:DropDownList id="ddConn" runat="server"></asp:DropDownList></TD>
					</TR>
					<TR>
						<TD style="HEIGHT: 23px">
							<asp:Label id="Label1" runat="server"> SQL Query:</asp:Label></TD>
					</TR>
					<TR>
						<TD style="HEIGHT: 185px">
							<asp:TextBox id="tbSQL" runat="server" TextMode="MultiLine" Width="728px" Height="288px" Font-Size="Small"
								Font-Names="Courier New"></asp:TextBox></TD>
					</TR>
					<TR>
						<TD>
							<asp:Button id="btnRun" runat="server" Width="104px" Text="Run"></asp:Button></TD>
					</TR>
					<TR>
						<TD>
							<asp:Label id="lblInfo" runat="server" Font-Size="Small" Font-Names="Courier New" ForeColor="#C00000"></asp:Label></TD>
					</TR>
				</TABLE>
			</P>
			<P>
			</P>
		</form>
	</body>
</HTML>
