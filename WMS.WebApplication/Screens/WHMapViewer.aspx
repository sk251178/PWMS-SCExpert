<%@ Register TagPrefix="cc4" Namespace="Made4Net.WebControls.Charting" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="GoWeb" Namespace="Northwoods.GoWeb" Assembly="Northwoods.GoWeb" %>
<%@ Register TagPrefix="YardDiagram" Namespace="Made4Net.LayoutViewer.UI" Assembly="Made4Net.LayoutViewer" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="WHMapViewer.aspx.vb" Inherits="WMS.WebApp.WHMapViewer"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>WHMapViewer</title>
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
			<cc3:screen id="Screen1" runat="server" ScreenID="wm"></cc3:screen>
			<YARDDIAGRAM:DIAGRAMVIEWER id="DV" runat="server" GridWidth="0" GridHeight="0" GridLayoutStyle="Dot" Width="815px"
				Height="360px" AutoRefreshInterval="3" FitToScaleVisible="True" FullExtentVisible="True" RefreshVisible="True"
				ZoomInVisible="True" ZoomOutVisible="True" ZoomInIcon="YardViewerZoomIn" FitToScaleIcon="YardViewerFit"
				FullExtentIcon="YardViewerFull" ZoomOutIcon="YardViewerZoomOut" RefreshIcon="YardViewerRefresh" AutoRefreshActive="True"
				GridXRatio="0.5" GridYRatio="1" ShowLegend="False"></YARDDIAGRAM:DIAGRAMVIEWER>
			<P></P> 
		</form>
		<!-- DiagramViewer Script -->
		<script>
			window.setInterval;
			function ObjectSingleClicked() {
				//alert("ObjectSingleClicked : " + goInfo.ID);
			}
			function ObjectDoubleClicked() {
				//alert("ObjectDoubleClicked : " + goInfo.ID);
			}
		</script>
	</body>
</HTML>
