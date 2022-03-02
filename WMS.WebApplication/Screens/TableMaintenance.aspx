<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="TableMaintenance.aspx.vb" Inherits="WMS.WebApp.Maintenance" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Code Table</title>
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
			<cc3:screen id="Screen1" runat="server" ScreenID="ct"></cc3:screen>
			<P>&nbsp;</P>
			<P><cc2:tableeditor id="TEMaster" runat="server" DESIGNTIMEDRAGDROP="14" GridPageSize="5" DefaultMode="Grid"
					DisableSwitches="veidrmn" ManualMode="False" ConnectionID="2" DefaultDT="DTCODELIST" AutoSelectGridItem="Never"
					AutoSelectMode="View"></cc2:tableeditor></P>
			<P>&nbsp;</P>
			<cc2:TableEditor id="TEDetail1" runat="server" DefaultMode="Grid" DisableSwitches="mn" GridPageSize="10"
				ConnectionID="2" DefaultDT="DTCODELISTDETAIL" AutoSelectGridItem="Never" AutoSelectMode="View"></cc2:TableEditor>
			<cc2:DataConnector id="DC1" runat="server" DESIGNTIMEDRAGDROP="38" MasterFields="CODE" MasterGridID="TEMaster"
				ChildFields="CODELISTCODE" TargetID="TEDetail1" MasterObjID="TEMaster"></cc2:DataConnector></TD>
			<P></P>
		</FORM>
	</body>
</HTML>
