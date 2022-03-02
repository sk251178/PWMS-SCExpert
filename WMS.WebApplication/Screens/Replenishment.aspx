<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Replenishment.aspx.vb" Inherits="WMS.WebApp.Replenishment"%>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Replenishment</title>
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
			<cc3:screen id="Screen1" runat="server" ScreenID="rs"></cc3:screen>
			<table cellpadding="2" cellspacing ="2" border="0">
			   <tr><td><cc2:Label runat="server" ID="lblMinFactor">Override Min Factor: </cc2:Label></td><td><cc2:TextBoxValidated ID="txtMinFactor" runat="server" Value="0"></cc2:TextBoxValidated></td></tr>
               <tr><td><cc2:Label runat="server" ID="lblTimeLimit">Task Time Limit: </cc2:Label></td><td><cc2:TextBoxValidated ID="txtTimeLimit" runat="server" Value="0"></cc2:TextBoxValidated></td></tr>
            </table>
			<p><cc2:tableeditor id="TEReplenish" runat="server" DESIGNTIMEDRAGDROP="14" DefaultMode="Grid" DefaultDT="DTNonBatchPIckLoc"
					SearchDT="DTNonBatchPickLocSearch" GridDT="DTNonBatchPickLocResults" DisableSwitches="nvreidvt" AutoSelectGridItem="Never"
					AutoSelectMode="View"></cc2:tableeditor></P>
		</form>
	</body>
</HTML>
