<%@ Page Language="vb" AutoEventWireup="false" Codebehind="YardViewer.aspx.vb" Inherits="WMS.WebApp.YardViewer"%>
<%@ Register TagPrefix="YardDiagram" Namespace="Made4Net.LayoutViewer.UI" Assembly="Made4Net.LayoutViewer" %>
<%@ Register TagPrefix="GoWeb" Namespace="Northwoods.GoWeb" Assembly="Northwoods.GoWeb" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Register TagPrefix="cc4" Namespace="Made4Net.WebControls.Charting" Assembly="Made4Net.WebControls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>YardViewer</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<cc3:screen id="Screen1" runat="server" ScreenID="yv"></cc3:screen>
			<YARDDIAGRAM:DIAGRAMVIEWER id="DV" runat="server" GridWidth="500" GridHeight="800" GridLayoutStyle="Dot" Width="800px"
				Height="500px" AutoRefreshInterval="3" FitToScaleVisible="True" FullExtentVisible="True" RefreshVisible="True"
				ZoomInVisible="True" ZoomOutVisible="True" ZoomInIcon="YardViewerZoomIn" FitToScaleIcon="YardViewerFit"
				FullExtentIcon="YardViewerFull" ZoomOutIcon="YardViewerZoomOut" RefreshIcon="YardViewerRefresh" AutoRefreshActive="True"
				GridXRatio="1" GridYRatio="1" ShowLegend="True"></YARDDIAGRAM:DIAGRAMVIEWER>
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
