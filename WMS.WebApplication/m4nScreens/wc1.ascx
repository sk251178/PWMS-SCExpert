<%@ Control Language="vb" AutoEventWireup="false" Codebehind="wc1.ascx.vb" Inherits="WMS.WebApp.wc1" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="cc3" Namespace="App.WebCtrls" Assembly="App.WebCtrls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<cc2:tableeditor id="TEReport" FilterExpression="type='DR'" ConnectionName="Made4NetSchema" InsertDT="SysDynReportInsert"
	GridType="Normal" runat="server" SearchButtonGroup="Search1" ObjectID="TEReport" GridPageSize="20"
	DisableSwitches="nm" ManualMode="False" DefaultDT="SysDynReport" DefaultMode="Grid" GridMode="Normal"
	SearchButtonPos="UnderSearchForm" AutoSelectGridItem="Never" AfterInsertMode="View" AfterUpdateMode="View"></cc2:tableeditor>
<BR>
<cc2:datatabcontrol id="DTC" runat="server" Width="216px" SyncEditMode="True" ParentID="TEReport">
	<TABLE id="Table2" cellSpacing="0" cellPadding="0" border="0" runat="server">
		<TR>
			<TD style="WIDTH: 212px">
				<cc2:tabstrip id="TS" runat="server" AutoPostBack="True" TargetTableID="Tbl">
					<iewc:Tab Text="Result Set"></iewc:Tab>
					<iewc:Tab Text="Format"></iewc:Tab>
					<iewc:Tab Text="User Input"></iewc:Tab>
				</cc2:tabstrip></TD>
		</TR>
		<TR>
			<TD style="WIDTH: 212px; HEIGHT: 141px">
				<TABLE id="Tbl" cellSpacing="0" cellPadding="5" border="0" runat="server">
					<TR>
						<TD vAlign="top">
							<asp:CheckBox id="chkShowResultSet" runat="server" Width="120px" AutoPostBack="True" Text="Show Result Set"></asp:CheckBox>
							<cc2:TableEditor id="TEReportData" AfterInsertMode="Grid" AutoSelectGridItem="SingleResult" DefaultMode="Grid"
								DisableSwitches="sndvetrm" ObjectID="TEReportData" runat="server" DESIGNTIMEDRAGDROP="36" ConnectMasterGridID="TEOrders"
								ConnectChildFields="OrderID" ConnectMasterFields="OrderID" LinkChildFields="OrderID" LinkMasterFields="OrderID"
								LinkParentID="TEOrders" AlwaysShowGrid="True"></cc2:TableEditor></TD>
						<TD>
							<cc2:dataconnector id="DCFormat" runat="server" DESIGNTIMEDRAGDROP="63" ChildFields="app_id, dt_id"
								MasterFields="app_id, dt_id" MasterGridID="TEMaster" TargetID="TEFormat" SyncMode="Reversed" MasterObjID="TEReport"></cc2:dataconnector>
							<cc2:TableEditor id="TEFormat" AfterInsertMode="Grid" AutoSelectGridItem="Never" DefaultMode="Grid"
								DefaultDT="SysDynReportFields" DisableSwitches="sveit" GridPageSize="100" ObjectID="TEFormat"
								runat="server" ConnectionName="Made4NetSchema" DESIGNTIMEDRAGDROP="64" ConnectMasterGridID="TEOrders"
								ConnectChildFields="OrderID" ConnectMasterFields="OrderID" LinkChildFields="OrderID" LinkMasterFields="OrderID"
								LinkParentID="TEOrders" AlwaysShowGrid="True" MultilineAllSelected="True"></cc2:TableEditor></TD>
						<TD>
							<cc2:dataconnector id="DCFilters" runat="server" ChildFields="app_id, dt_id" MasterFields="fltr_app_id, fltr_dt_id"
								MasterGridID="TEMaster" TargetID="TEFilters" SyncMode="Reversed" MasterObjID="TEReport"></cc2:dataconnector>
							<cc2:TableEditor id="TEFilters" AfterInsertMode="Grid" AutoSelectGridItem="Never" DefaultMode="Grid"
								DefaultDT="SysDynReportFilters" DisableSwitches="sveit" GridPageSize="100" ObjectID="TEFilters"
								runat="server" ConnectionName="Made4NetSchema" ConnectMasterGridID="TEOrders" ConnectChildFields="OrderID"
								ConnectMasterFields="OrderID" LinkChildFields="OrderID" LinkMasterFields="OrderID" LinkParentID="TEOrders"
								AlwaysShowGrid="True" MultilineAllSelected="True"></cc2:TableEditor></TD>
					</TR>
				</TABLE>
			</TD>
		</TR>
	</TABLE>
</cc2:datatabcontrol>
