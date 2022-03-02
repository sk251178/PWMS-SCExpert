<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="LangSkinSetup.aspx.vb" Inherits="WMS.WebApp.LangSkinSetup"%>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Language / Skin Setup</title>
		<meta content="False" name="vs_showGrid">
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
			<P>
				<cc3:Screen id="Screen1" runat="server"></cc3:Screen></P>
			<P>&nbsp;</P>
			<TABLE id="Table1" cellSpacing="1" cellPadding="1" width="300" border="0">
				<TR>
					<TD></TD>
					<TD></TD>
					<TD></TD>
				</TR>
				<TR>
					<TD style="HEIGHT: 13px">
						<cc2:Label id="Label1" runat="server" DESIGNTIMEDRAGDROP="20" ReadOnly="False" EnableSummerize="False"
							Value="Language">Language</cc2:Label></TD>
					<TD style="HEIGHT: 13px">
						<cc2:DropDownListValidated id="lang" runat="server" DESIGNTIMEDRAGDROP="22" AutoPostBack="True" ConnectionName="Made4NetSchema"
							TableName="Language" TextField="name" ValueField="language_id"></cc2:DropDownListValidated></TD>
					<TD style="HEIGHT: 13px"></TD>
				</TR>
				<TR>
					<TD>
						<cc2:Label id="Label2" runat="server" Value="Language" EnableSummerize="False" ReadOnly="False">Skin</cc2:Label></TD>
					<TD>
						<cc2:DropDownListValidated id="ddSkin" runat="server" ValueField="language_id" TextField="name" TableName="Language"
							ConnectionName="Made4NetSchema" AutoPostBack="True"></cc2:DropDownListValidated></TD>
					<TD></TD>
				</TR>
			</TABLE>
			<cc2:tableeditor id="TEOrders" runat="server" DESIGNTIMEDRAGDROP="14" TableName="SampleOrders" ManualMode="False"
				DisableSwitches="itdv" DefaultMode="View" GridPageSize="5" DefaultDT="Orders" AutoSelectGridItem="Never"
				AutoSelectMode="View"></cc2:tableeditor>
		</FORM>
	</body>
</HTML>
