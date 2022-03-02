<%@ Page Language="vb" AutoEventWireup="false" Codebehind="RoutingSetDisplay.aspx.vb" Inherits="WMS.WebApp.RoutingSetDisplay" %>
<%@ Register TagPrefix="cc4" Namespace="Made4Net.WebControls.Charting" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>RoutingSetDisplay</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<cc3:screen id="Screen1" runat="server" ScreenID="rtsd"></cc3:screen>
			<TABLE border="0">
				<tr>
					<TD vAlign="top" borderColor="#000000"><cc2:map id="MPVehiclePos" runat="server"></cc2:map></TD>
					<td vAlign="top"><cc2:tableeditor id="TERoutingSet" runat="server" SearchDT="DTRouteSetDisp" FilterExpression="STATUS IS NOT NULL AND STATUS <> ''"
							ForbidRequiredFieldsInSearchMode="True" DisableSwitches="nvidre" GridDT="DTRouteSetDisp" GridPageSize="5"
							DefaultMode="Search" ManualMode="False" DefaultDT="DTRouteSetDisp" AutoSelectGridItem="Never" AutoSelectMode="View"></cc2:tableeditor></td>
				</tr>
			</TABLE>
		</form>
	</body>
</HTML>
