<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="TableMaintenance.aspx.vb" Inherits="WMS.WebApp.TableMaintenance" ValidateRequest="false" %>
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
						<TD>
							<asp:Label id="Label1" runat="server">Please select a SYS table:</asp:Label></TD>
						<TD>
							<asp:Label id="Label2" runat="server">or APP table:</asp:Label></TD>
					</TR>
					<TR>
						<TD>
							<cc2:DropDownListValidated id="ddSysTable" runat="server" AutoPostBack="True" DESIGNTIMEDRAGDROP="93"></cc2:DropDownListValidated></TD>
						<TD>
							<cc2:DropDownListValidated id="ddAppTable" runat="server" AutoPostBack="True"></cc2:DropDownListValidated></TD>
					</TR>
				</TABLE>
			</P>
			<HR width="100%" SIZE="1">
			<P>
			</P>
			<P>
				<cc2:tableeditor id="TE" runat="server" ManualMode="False" DisableSwitches="t" DefaultMode="Search"
					GridPageSize="50" DESIGNTIMEDRAGDROP="14" DisableInsert="False" Started="False" AlwaysShowSearch="True"
					AlwaysShowGrid="True" ShowSearchFormAfterSearch="True" ConnectionName="Made4NetSchema" ConnectionID="1"
					ForbidRequiredFieldsInSearchMode="True"></cc2:tableeditor></P>
			<P>
			</P>
		</form>
	</body>
</HTML>
