<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="BlindReceiving.aspx.vb" Inherits="WMS.WebApp.BlindReceiving" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>BlindReceiving</title>
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
			<p><cc3:screen id="Screen1" runat="server" ScreenID="br"></cc3:screen></p>
			<p>
				<table>
					<tr>
						<td>
							<cc2:FieldLabel id="lblReceipt" runat="server" value="Receipt"></cc2:FieldLabel>
							<cc2:TextBoxValidated id="txtReceipt" runat="server" Required="False"></cc2:TextBoxValidated>
							<cc2:Button id="btnOK" runat="server" Text="Send"></cc2:Button>
						</td>
					</tr>
				</table>
			</p>
			<P><cc2:datatabcontrol id="DTC" runat="server" visible="False">
					<TABLE id="tblDetails" cellSpacing="0" cellPadding="0" border="0" runat="server">
						<TR>
							<TD>
								<cc2:tabstrip id="TS" runat="server" TargetTableID="Tbl" SepDefaultStyle="border-bottom:solid 1px gray;"
									TabSelectedStyle="border:solid 1px gray;border-bottom:none;background:white;" TabHoverStyle="background-color:#DDDDDD;"
									TabDefaultStyle="background-color:#DDDDDD;border:solid 1px gray;padding-left:5px;padding-right:5px;font-size:13px;font-weight:bold;padding-top:2px;padding-bottom:2px;"
									AutoPostBack="True">
									<iewc:Tab Text="Create Load"></iewc:Tab>
									<iewc:TabSeparator DefaultStyle="width:100%;"></iewc:TabSeparator>
								</cc2:tabstrip></TD>
						</TR>
						<TR>
							<TD style="BORDER-RIGHT: gray 1px solid; BORDER-LEFT: gray 1px solid; BORDER-BOTTOM: gray 1px solid; HEIGHT: 18px">
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
															<cc2:FieldLabel id="lblConsignee" runat="server" value="Consignee"></cc2:FieldLabel></TD>
														<TD>
															<cc2:TextBoxValidated id="txtConsignee" runat="server" Required="True"></cc2:TextBoxValidated></TD>
														<TD>
															<cc2:FieldLabel id="lblSku" runat="server" value="Sku"></cc2:FieldLabel></TD>
														<TD>
															<cc2:TextBoxValidated id="txtSku" runat="server" Required="False"></cc2:TextBoxValidated></TD>
														<TD>
															<cc2:FieldLabel id="lblLoc" runat="server" value="Location"></cc2:FieldLabel></TD>
														<TD>
															<cc2:TextBoxValidated id="txtLocation" runat="server" Required="True"></cc2:TextBoxValidated></TD>
														<TD>
															<cc2:FieldLabel id="lblUnits" runat="server" value="Units"></cc2:FieldLabel></TD>
														<TD>
															<cc2:TextBoxValidated id="txtUnits" runat="server" Required="True" CheckDataType="True" DataType="Integer"
																MinValue="1"></cc2:TextBoxValidated></TD>
													</TR>
													<TR>
														<TD noWrap>
															<cc2:FieldLabel id="lblNumOfLoads" runat="server" value="Number Of Loads"></cc2:FieldLabel></TD>
														<TD>
															<cc2:TextBoxValidated id="txtNumLoads" runat="server" Required="False" CheckDataType="True" DataType="Integer"
																MinValue="1" Value="1"></cc2:TextBoxValidated></TD>
														<TD>
															<cc2:FieldLabel id="lblUOM" runat="server" value="UOM"></cc2:FieldLabel></TD>
														<TD>
															<cc2:DropDownList id="ddUOM" runat="server" width="100%"></cc2:DropDownList></TD>
														<TD>
															<cc2:FieldLabel id="lblStatus" runat="server" value="Status"></cc2:FieldLabel></TD>
														<TD>
															<cc2:DropDownList id="ddStatus" runat="server" AutoPostBack="True" width="100%"></cc2:DropDownList></TD>
														<TD>
															<cc2:FieldLabel id="lblHoldRc" runat="server" value="HoldRC"></cc2:FieldLabel></TD>
														<TD>
															<cc2:DropDownList id="ddHold" runat="server" width="100%" enabled="False"></cc2:DropDownList></TD>
													</TR>
												</TABLE>
												<cc2:Table id="AttribTbl" runat="server"></cc2:Table>
											</asp:Panel></TD>
									</TR>
								</TABLE>
							</TD>
						</TR>
					</TABLE>
				</cc2:datatabcontrol></P>
		</form>
	</body>
</HTML>
