<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Control Language="vb" AutoEventWireup="false" Codebehind="SavedSearch.ascx.vb" Inherits="WMS.WebApp.ASCX.SavedSearch" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<asp:Panel id="pnl" runat="server" DESIGNTIMEDRAGDROP="24">
	<cc2:tableeditor id="TE" runat="server" HideActionBar="True" PersistSearchValues="Off" EnableSavedSearch="Off"
		EnableQuickReport="Off" EnableQuickChart="Off" EnableCSVExport="Off" ConnectionName="Made4NetSchema"
		GridType="Normal" SearchButtonGroup="Search1" ObjectID="TE" GridPageSize="20" DisableSwitches="nmistve"
		ManualMode="False" DefaultMode="Grid" GridMode="Normal" SearchButtonPos="UnderSearchForm" AutoSelectGridItem="Never"
		AfterInsertMode="View" AfterUpdateMode="View" DefaultDT="SysSavedSearches"></cc2:tableeditor>
</asp:Panel>
<asp:Panel id="pnlSave" runat="server">
	<DIV style="MARGIN: 10px">
		<table><tr><td>
		<asp:Label id="lblName" runat="server" Font-Names="verdana,arial" Font-Size="11px">[Label Text]</asp:Label><BR>
		</td>
		</tr>
		<tr>
		<td nowrap=true>
		<asp:TextBox id="tbSearchName" runat="server" Width="208px"></asp:TextBox>
		<asp:Button id="btnSave" runat="server" Text="Save" Width="80px"></asp:Button></DIV>
		</td></tr></table>
</asp:Panel>
<script language="javascript">

function GetRadWindow()
{
    var oWindow = null;
    if (window.radWindow) oWindow = window.radWindow;
    else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
    return oWindow;
} 
function CloseRadWindow()
{
	var w = GetRadWindow();
	var xml = '<xml><dtid>' + savedSearchDTID + '</dtid><action>' + savedSearchAction + '</action></xml>';
	xml = escape(xml);
	w.Close(xml);
}
function CloseAfterSave()
{
	var w = GetRadWindow();
	w.Close();
}
try
{
	//will fail is savedSearchDTID is not defined
	var temp = savedSearchDTID;
	CloseRadWindow();
}
catch(e)
{}
</script>
