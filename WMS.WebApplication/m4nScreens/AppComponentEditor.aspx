<%@ Register TagPrefix="uc1" TagName="AppComponentEditor" Src="AppComponentEditor.ascx" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="AppComponentEditor.aspx.vb" Inherits="WMS.WebApp.AppComponentEditor1" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>AppComponentEditor</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<!-- #include file="~/include/head.html" -->
	</HEAD>
	<body MS_POSITIONING="FlowLayout" bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0">
		<!-- #include file="~/include/Header.html" -->
		<form id="Form1" method="post" runat="server">
            <telerik:RadScriptManager ID="RadScriptManager2" runat="server">     </telerik:RadScriptManager> 
			<cc3:Screen id="Screen" runat="server" DESIGNTIMEDRAGDROP="7" HideTitle="False" HideBanner="False"></cc3:Screen>
			<uc1:AppComponentEditor id="ACE" runat="server"></uc1:AppComponentEditor>
		</form>
		<script language="javascript">
		function CloneCallback(radWindow, returnValue)
		{
			if (unescape(returnValue).indexOf('<ACCloneOptions>') == 0)
			{
				var b = "ACE_TEAC_ActionBar_btnClone";
				document.getElementById(b + "_INFO").value = returnValue;
				var btn = document.getElementById(b + "_InnerButton");
				var onclk = btn.onclick;
				btn.onclick='';
				btn.click(); 
				btn.onclick = onclk;
			}
		}
		</script>
	</body>
</HTML>
