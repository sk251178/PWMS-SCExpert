<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="AppComponentCloneOptions.aspx.vb" Inherits="WMS.WebApp.AppComponentCloneOptions" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>AppComponentCloneOptions</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<!-- #include file="~/include/head.html" -->
	</HEAD>
	<body MS_POSITIONING="FlowLayout" bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0">
		<!-- #include file="~/include/Header.html" -->
        <telerik:RadScriptManager ID="RadScriptManager2" runat="server">     </telerik:RadScriptManager> 
		<form id="Form1" method="post" runat="server">
			<cc3:Screen id="Screen" runat="server" HideMenu="True" DESIGNTIMEDRAGDROP="7" HideTitle="True"
				HideBanner="True" HideHeader="True"></cc3:Screen><BR>
			<cc2:tableeditor id="TE" runat="server" DESIGNTIMEDRAGDROP="3" DefaultDT="SYS_AC_CLONE" AfterUpdateMode="Insert"
				AfterInsertMode="Insert" AutoSelectGridItem="Never" SearchButtonPos="UnderSearchForm" GridMode="Normal"
				DefaultMode="Insert" ManualMode="False" GridPageSize="20" ObjectID="TE" SearchButtonGroup="Search1"
				GridType="Normal" EnableCSVExport="Global" EnableQuickChart="Global" EnableQuickReport="Global"
				EnableSavedSearch="Global" PersistSearchValues="Global" ConnectionName="Made4NetSchema" DisableSwitches="ntsve"
				ForbidRequiredFieldsInSearchMode="True" HideActionBar="True"></cc2:tableeditor><BR>
			<INPUT style="WIDTH: 80px; HEIGHT: 24px" onclick="CancelRadWindow()" type="button" value="Cancel">&nbsp;&nbsp;
			<INPUT style="WIDTH: 80px; HEIGHT: 24px" onclick="BtnOKHandler()" type="button" value="OK">
		</form>
		<script language="jscript">
			function BtnOKHandler(pos)
			{
				//validate page
				if (typeof(Page_ClientValidate) == 'function') Page_ClientValidate();
				if (!ValidatorOnSubmit()) return false;

				var code = getFieldValue("code");
				var name = getFieldValue("name");
				
				var w = GetRadWindow();
				var xml = "<ACCloneOptions>";
				xml += getXmlNode("code", code);
				xml += getXmlNode("name", name);
				xml += "</ACCloneOptions>";

				xml = escape(xml);
				w.Close(xml);
			}
			function getXmlNode(name, value)
			{
				return "<" + name + ">" + escape(value) + "</" + name + ">";
			}
			function getFieldValue(fieldName)
			{
				var im = new m4nDisplayTypeInstanceManager("TE:" + fieldName);
				var dt = im.getInstance();
				var val = dt.getValue();
				return val;
			}
			function GetRadWindow()
			{
				var oWindow = null;
				if (window.radWindow) oWindow = window.radWindow;
				else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
				return oWindow;
			} 
			function CancelRadWindow()
			{
				var w = GetRadWindow();
				w.Close();
			}

		</script>
	</body>
</HTML>
