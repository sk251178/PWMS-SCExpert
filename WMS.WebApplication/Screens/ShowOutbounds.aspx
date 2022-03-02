<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ShowOutbounds.aspx.vb" Inherits="WMS.WebApp.ShowOutbounds" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>ShowOutbounds</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<!-- #include file="~/include/head.html" -->
	</HEAD>
	<body MS_POSITIONING="FlowLayout">
		<!-- #include file="~/include/Header.html" -->
		<form id="Form1" method="post" runat="server">
            <telerik:RadScriptManager ID="RadScriptManager2" runat="server">     </telerik:RadScriptManager> 
			<cc3:Screen id="Screen1" runat="server" HideBanner="true" HideMenu="true" Hidden="True" NoLoginRequired="True"></cc3:Screen>
			<br>
			<cc2:tableeditor id="TEOrdersRouting" runat="server" DefaultMode="Search" DESIGNTIMEDRAGDROP="60"
				DisableSwitches="veidrn" DefaultDT="DTROUTINGORDERS"></cc2:tableeditor>
		</form>
		<script language="javascript">
		var tHandler;
		tHandler = setTimeout("CheckParent()",3000,"javascript");
		
		void CheckParent()
		{
			alert(window.parent.location);
			/*if (window.parent.location != "Routing/OutordRoutes.aspx")
			{
				alert("ParentChanges");
				clearTimeout(tHandler);
				window.close();
			}*/
		}
		</script>
	</body>
</HTML>
