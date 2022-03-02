<%@ Page Language="vb" AutoEventWireup="false" Codebehind="LicCreator.aspx.vb" Inherits="WMS.WebApp.LicCreator"%>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>LicCreator</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<!-- #include file="~/include/head.html" -->
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout">
		<!-- #include file="~/include/Header.html" -->
		<form id="Form1" method="post" runat="server">
            <telerik:RadScriptManager ID="RadScriptManager2" runat="server">     </telerik:RadScriptManager> 
			<cc3:screen id="Screen1" runat="server"></cc3:screen>
			<TABLE id="Tbl" cellSpacing="0" cellPadding="5" border="0" runat="server">
				<TR>
					<TD>
						<asp:Panel id="pnlCld" runat="server" Visible="True">
							<TABLE>
								<TR>
									<TD colSpan="8">
										<cc2:TableEditorActionBar id="TEAB" runat="server" visible="True"></cc2:TableEditorActionBar></TD>
								</TR>
								<TR>
									<TD>
										<cc2:FieldLabel id="lblLoadId" runat="server" value="LoadId"></cc2:FieldLabel></TD>
									<TD>
										<cc2:TextBoxValidated id="txtLoadId" runat="server" Required="False"></cc2:TextBoxValidated></TD>
									<TD>
										<cc2:FieldLabel id="lblLoc" runat="server" value="Location"></cc2:FieldLabel></TD>
									<TD>
										<cc2:TextBoxValidated id="txtLocation" runat="server" Required="True"></cc2:TextBoxValidated></TD>
									<TD>
										<cc2:FieldLabel id="lblUnits" runat="server" value="Units"></cc2:FieldLabel></TD>
									<TD>
										<cc2:TextBoxValidated id="txtUnits" runat="server" Required="True" CheckDataType="True" DataType="Integer"
											MinValue="1"></cc2:TextBoxValidated></TD>
									<TD noWrap>
										<cc2:FieldLabel id="lblNumOfLoads" runat="server" value="Number Of Loads"></cc2:FieldLabel></TD>
									<TD>
										<cc2:TextBoxValidated id="txtNumLoads" runat="server" Required="False" CheckDataType="True" DataType="Integer"
											MinValue="1" Value="1"></cc2:TextBoxValidated></TD>
								</TR>
								<TR>
									<TD>&nbsp;</TD>
									<TD>&nbsp;</TD>
									<TD>
										<cc2:FieldLabel id="lblUOM" runat="server" value="UOM"></cc2:FieldLabel></TD>
									<TD>
										<cc2:DropDownList id="ddUOM" runat="server" width="100%"></cc2:DropDownList></TD>
									<TD>
										<cc2:FieldLabel id="lblStatus" runat="server" value="Status"></cc2:FieldLabel></TD>
									<TD>
										<cc2:DropDownList id="ddStatus" runat="server" width="100%" AutoPostBack="True"></cc2:DropDownList></TD>
									<TD>
										<cc2:FieldLabel id="lblHoldRc" runat="server" value="HoldRC"></cc2:FieldLabel></TD>
									<TD>
										<cc2:DropDownList id="ddHold" runat="server" width="100%"></cc2:DropDownList></TD>
								</TR>
							</TABLE>
							<cc2:Table id="AttribTbl" runat="server"></cc2:Table>
						</asp:Panel></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
