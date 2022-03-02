<%@ Page Language="vb" AutoEventWireup="false" Codebehind="RouteSets.aspx.vb" Inherits="WMS.WebApp.RouteSets" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>RouteSets</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<P>
				<cc3:screen id="Screen1" runat="server" ScreenID="rs" HideBanner="True" HideMenu="True" NoLoginRequired="True"></cc3:screen></P>
			<P><cc2:tableeditor id="TEMasterRoutingSet" runat="server" SortExperssion="SETID DESC" AfterUpdateMode="Grid"
					AfterInsertMode="Grid" EditDT="DTRS Add" ManualMode="False" InsertDT="DTRS Add" DefaultMode="Search"
					GridPageSize="5" DESIGNTIMEDRAGDROP="14" DisableSwitches="mner" DefaultDT="DTRS Search" SearchDT="DTRS Search"
					ForbidRequiredFieldsInSearchMode="True"></cc2:tableeditor></P>
		</form>
	</body>
</HTML>
