<%@ Control Language="vb" AutoEventWireup="false" Codebehind="AppComponentEditor.ascx.vb" Inherits="WMS.WebApp.AppComponentEditor" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI.RadTabStrip " Assembly="Telerik.Web.UI" %>

<cc2:tableeditor id="TEAC" runat="server" DefaultDT="SYS_AC" AfterUpdateMode="Grid" AfterInsertMode="Grid"
	AutoSelectGridItem="Never" SearchButtonPos="UnderSearchForm" GridMode="Normal" DefaultMode="Grid"
	ManualMode="False" GridPageSize="20" ObjectID="TEAC" SearchButtonGroup="Search1" GridType="Normal"
	EnableCSVExport="Global" EnableQuickChart="Global" EnableQuickReport="Global" EnableSavedSearch="Global"
	PersistSearchValues="Global" DESIGNTIMEDRAGDROP="3" ConnectionName="Made4NetSchema" DisableSwitches="nt"
	ForbidRequiredFieldsInSearchMode="True" AllowDeleteInViewMode="True"></cc2:tableeditor><BR>
<table runat="server" cellspacing="0" cellpadding="0" border="0" width="97%" id="T">
	<tr>
		<td valign="bottom">
			<cc2:TabStripControl id="ts" runat="server">
				<Tabs>
                     <%-- Upgrade - Telerik Radcontrols 2.0 frmwrk to Telerik Ajax UI 4.5 Tab to RadTab--%>
					<telerik:RadTab Text="Configure"></telerik:RadTab>
				</Tabs>
			</cc2:TabStripControl>
		</td>
		<td valign="bottom">
			<table cellpadding="0" border="0" cellspacing="0">
				<tr>
					<td valign="bottom">
						<cc2:TabStripControl id="TabStripControl1" runat="server" Width="100px">
							<Tabs>
                              <%--  'Upgrade - Telerik Radcontrols 2.0 frmwrk to Telerik Ajax UI 4.5 Tab to RadTab--%>
								<telerik:RadTab Text="Preview"></telerik:RadTab>
							</Tabs>
						</cc2:TabStripControl>
					</td>
					<td valign="bottom">
						<asp:Label id="lblWidth" runat="server">Width:</asp:Label>&nbsp;
						<asp:TextBox id="tbWidth" runat="server" Width="50px"></asp:TextBox>&nbsp;
						<asp:Label id="lblHeight" runat="server">Height:</asp:Label>&nbsp;
						<asp:TextBox id="tbHeight" runat="server" Width="50px"></asp:TextBox>
						<asp:Button id="btnApplySize" runat="server" Text="Apply"></asp:Button>
					</td>
				</tr>
			</table>
		</td>
	</tr>
	<tr>
		<td valign="top" width="30%" style="BORDER-RIGHT:#cccccc 1px solid; BORDER-TOP:#cccccc 1px solid; BORDER-LEFT:#cccccc 1px solid; BORDER-BOTTOM:#cccccc 1px solid">
			<div style="PADDING-RIGHT:6px; PADDING-LEFT:6px; PADDING-BOTTOM:6px; PADDING-TOP:6px">
				<cc2:tableeditor id="TEACVal" ForbidRequiredFieldsInSearchMode="True" DisableSwitches="ntvisd" PersistSearchValues="Global"
					EnableSavedSearch="Global" EnableQuickReport="Global" EnableQuickChart="Global" EnableCSVExport="Global"
					GridType="Normal" ObjectID="TEACVal" GridPageSize="20" ManualMode="False" DefaultMode="Edit"
					GridMode="Normal" SearchButtonPos="UnderSearchForm" AutoSelectGridItem="Never" AfterInsertMode="Edit"
					AfterUpdateMode="Edit" runat="server" PanelBarForm="True"></cc2:tableeditor>
			</div>
		</td>
		<td valign="top" style="BORDER-RIGHT:#cccccc 1px solid; BORDER-TOP:#cccccc 1px solid; BORDER-LEFT:#cccccc 1px solid; BORDER-BOTTOM:#cccccc 1px solid">
			<div style="PADDING-RIGHT:6px; PADDING-LEFT:6px; PADDING-BOTTOM:6px; PADDING-TOP:6px">
				<asp:PlaceHolder id="ph" runat="server"></asp:PlaceHolder>
			</div>
		</td>
	</tr>
</table>
<BR>
<BR>
<BR>
<BR>
