<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Routes.aspx.vb" Inherits="WMS.WebApp.Routes" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Routes</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<cc3:screen id="Screen1" runat="server" ScreenID="rtls"></cc3:screen>
			<TABLE cellSpacing="0" cellPadding="0">
				<TR>
					<TD vAlign="top"><cc2:map id="MPTest" runat="server"></cc2:map></TD>
					<TD valign="top">
						<cc2:tableeditor id="TERouteList" runat="server" EnableModifySavedSearch="On" EnableSavedSearch="Off"
							PersistSearchValues="Off" EnableQuickReport="Global" EnableQuickChart="Global" EnableCSVExport="Global"
							DisableSwitches="rmneiv" DESIGNTIMEDRAGDROP="60" DefaultMode="Search" DefaultDT="DTRouteList"
							ForbidRequiredFieldsInSearchMode="True" SearchDT="DTRouteListSearch" SearchButtonPos="NextToSearchForm"
							SortExperssion="routeid desc"></cc2:tableeditor></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
