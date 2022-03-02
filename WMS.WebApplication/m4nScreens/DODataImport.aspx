<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="DODataImport.aspx.vb" Inherits="WMS.WebApp.DODataImport" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
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
			<P><cc3:screen id="Screen1" runat="server" HideMenu="True"></cc3:screen><BR>
			</P>
			<div style="MARGIN: 10px"><cc2:tableeditor id="TE" runat="server" DisableSwitches="eidtvrmns" ConnectionName="Made4NetSchema"
					AutoSelectGridItem="Never" SearchButtonPos="UnderSearchForm" GridMode="Normal" DefaultMode="Insert" ManualMode="False"
					GridPageSize="20" ObjectID="TE" AfterUpdateMode="Grid" AfterInsertMode="Grid" EnableCSVExport="Global"></cc2:tableeditor><BR>
				<div style="DISPLAY: none"><asp:linkbutton id="LinkButton1" runat="server">Dummy Link for postback function to be registered</asp:linkbutton><BR>
				</div>
				<asp:placeholder id="ph" runat="server"></asp:placeholder></div>
		</FORM>
		<script language="javascript">

			var dd = document.getElementById('TE_re_Form_field_ImportType__ctl0_DropDownList');
			dd.onchange = null;
			dd.attachEvent("onchange", function(){__doPostBack('TE$re$Form$field_ImportType$_ctl0$DropDownList','');});
		
		</script>
	</body>
</HTML>
