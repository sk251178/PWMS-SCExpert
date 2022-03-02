<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ImportManagerRun.ascx.vb" Inherits="WMS.WebApp.ImportManagerRun" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<DIV>
	<cc2:tableeditor id="TE" DisableSwitches="eidtvrmns" ConnectionName="Made4NetSchema" AutoSelectGridItem="Never"
		SearchButtonPos="UnderSearchForm" GridMode="Normal" DefaultMode="Insert" ManualMode="False" GridPageSize="20"
		ObjectID="TE" AfterUpdateMode="Grid" AfterInsertMode="Grid" EnableCSVExport="Global" runat="server"></cc2:tableeditor><BR>
	<DIV style="DISPLAY: none">
		<asp:linkbutton id="LinkButton1" runat="server">Dummy Link for postback function to be registered</asp:linkbutton><BR>
	</DIV>
	<asp:placeholder id="ph" runat="server"></asp:placeholder>
</DIV>
<script language="javascript">
	//var dd = document.getElementById('DTC_IMR_TE_re_Form_field_ImportType__ctl0_DropDownList');
	var re = /\:/g;
	var id = ImportTypeId.replace(re, '_');
	
	var postbackArg = ImportTypeId.replace(re, '$');
	var dd =  document.getElementById(id);

	dd.onchange = null;
	dd.attachEvent("onchange", function(){__doPostBack(postbackArg,'');});
</script>
