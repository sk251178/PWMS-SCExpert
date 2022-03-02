<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="AppComponentDockConfig.aspx.vb" Inherits="WMS.WebApp.AppComponentDockConfig" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title></title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		
		<!-- include file="~/include/head.html" -->
		
	</HEAD>
	<body MS_POSITIONING="FlowLayout" bottomMargin="10" topMargin="10">
		
		<!-- // Not required here include file="~/include/Header.html" -->
		
		<form id="Form1" method="post" runat="server">
            <telerik:RadScriptManager ID="RadScriptManager2" runat="server">     </telerik:RadScriptManager> 
			<cc3:Screen id="Screen" runat="server" HideMenu="True" DESIGNTIMEDRAGDROP="7" HideTitle="True"
				HideBanner="True" HideStatusLineWhenEmpty="True"></cc3:Screen><BR>
			<div style="OVERFLOW:auto; HEIGHT:290px">
				<asp:PlaceHolder id="ph" runat="server"></asp:PlaceHolder>
			</div>
			<BR>
			<div align="center">
				<INPUT runat="server" style="WIDTH: 80px; HEIGHT: 24px" onclick="CancelRadWindow()" type="button"
					value="Cancel" id="btnCancel">&nbsp;&nbsp; <INPUT id="btnOK" style="WIDTH: 80px; HEIGHT: 24px" onclick="BtnOKHandler()" type="button"
					value="OK" name="Button1" runat="server">
			</div>
		</form>
		<script language="jscript">
			function BtnOKHandler(pos)
			{
				//validate page
				if (typeof(Page_ClientValidate) == 'function') Page_ClientValidate();
				if (!ValidatorOnSubmit()) return false;

				var urlParams = m4nGetURLParams();
				var dockCode = urlParams["DockCode"];

				var xml = "<selection dockCode=\"" + dockCode + "\">";
				var t = GetTree();
				var nodes = t.AllNodes;
				//alert(nodes.length);
				var node;
				
				for(i=0; i<=nodes.length-1; i++)
				{
					node = nodes[i];
					if (node.Checked)
					{
						xml += getXmlNode("item", node.Value);
					}
				}

				xml += "</selection>";
				//alert(xml);
				var w = GetRadWindow();

				xml = escape(xml);
				w.Close(xml);
			}
			function getXmlNode(name, value)
			{
				return "<" + name + ">" + escape(value) + "</" + name + ">";
			}
			function getFieldValue(fieldName)
			{
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
			function GetTree()
			{
				return Tree;
			}
		</script>
	</body>
</HTML>
