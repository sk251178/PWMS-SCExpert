<%@ Page Language="vb" AutoEventWireup="false" Codebehind="SLPlans.aspx.vb" Inherits="WMS.WebApp.SLPlans"%>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="zgw" Namespace="ZedGraph.Web" Assembly="ZedGraph.Web" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>SLPlans</title>
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
			<cc3:screen id="Screen1" runat="server" ScreenID="sg"></cc3:screen>
			<p></p>
			<table>
				<tr>
					<td vAlign="top"><asp:panel id="Panel1" Runat="server" Font-Names="Arial" Font-Size="10pt" ForeColor="#000040">
							<asp:RadioButton id="rbShowAll" Runat="server" AutoPostBack="True" Text="Show All" Checked="True"
								GroupName="rbSL"></asp:RadioButton>
							<asp:RadioButton id="rbOnlyOccupied" Runat="server" AutoPostBack="True" Text="Occupied Lanes" Checked="False"
								GroupName="rbSL"></asp:RadioButton>
							<asp:RadioButton id="rbOnlyEmpty" Runat="server" AutoPostBack="True" Text="Empty Lanes" Checked="False"
								GroupName="rbSL"></asp:RadioButton>
						</asp:panel><asp:panel id="pnlSL" Runat="server"></asp:panel></td>
					<td vAlign="top"><asp:panel id="pnlAddOrders" Runat="server">
														<cc2:tableeditor id="TEOutboundOrdersStat" runat="server" AutoSelectGridItem="Never" DefaultDT="DTSLPLANGrid"
								ManualMode="False" DefaultMode="Search" GridPageSize="5" GridDT="DTSLPLANGrid" DisableSwitches="iemv"
								ForbidRequiredFieldsInSearchMode="True" ObjectID="TEOutboundOrdersStat" DESIGNTIMEDRAGDROP="14"
								SearchDT="DTSLPLANSearch" AfterInsertMode="Grid" AfterUpdateMode="Grid" MultiEditDT="DTSLPLANMulti"
								FilterExpression="main.status <> 'SHIPPED' and main.status <> 'CANCELED'"></cc2:tableeditor>
						</asp:panel></td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
