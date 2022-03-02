<%@ Register TagPrefix="cc4" Namespace="Made4Net.WebControls.Charting" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="RoutingSet.aspx.vb" Inherits="WMS.WebApp.RoutingSet" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>RoutingSet</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<cc3:screen id="Screen1" runat="server" ScreenID="rts"></cc3:screen>
			<P><cc2:tableeditor id="TERoutingSet" runat="server" SortExperssion="setid DESC" AfterUpdateMode="Grid"
					AfterInsertMode="Grid" EditDT="DTRoutingSet Edit" DefaultDT="DTRoutingSet" ManualMode="False"
					DefaultMode="Search" GridPageSize="5" DESIGNTIMEDRAGDROP="14" ForbidRequiredFieldsInSearchMode="True"
					DisableSwitches="nr" InsertDT="DTRoutingSet Add" SearchDT="DTRoutingSet Search" AutoSelectMode="View"
					AutoSelectGridItem="Never"></cc2:tableeditor></P>
			<P>&nbsp;</P>
			<P><cc2:datatabcontrol id="DTC" runat="server" SyncEditMode="True" ParentID="TERoutingSet">
					<TABLE id="Table1" cellSpacing="0" cellPadding="0" border="0" runat="server">
						<TR>
							<TD>
								<cc2:tabstrip id="TS" runat="server" AutoPostBack="True" TabDefaultStyle="background-color:#DDDDDD;border:solid 1px gray;padding-left:5px;padding-right:5px;font-size:13px;font-weight:bold;padding-top:2px;padding-bottom:2px;"
									TabHoverStyle="background-color:#DDDDDD;" TabSelectedStyle="border:solid 1px gray;border-bottom:none;background:white;"
									SepDefaultStyle="border-bottom:solid 1px gray;" TargetTableID="Tbl">
									<iewc:Tab Text="Details"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="Assign Orders"></iewc:Tab>
									<iewc:TabSeparator DefaultStyle="width:100%;"></iewc:TabSeparator>
								</cc2:tabstrip></TD>
						</TR>
						<TR>
							<TD style="BORDER-RIGHT: gray 1px solid; BORDER-LEFT: gray 1px solid; BORDER-BOTTOM: gray 1px solid; HEIGHT: 18px">
								<TABLE id="Tbl" cellSpacing="0" cellPadding="5" border="0" runat="server">
									<TR>
										<TD>
											<cc2:TableEditor id="TERoutingSetOrders" runat="server" AutoSelectGridItem="Never" AutoSelectMode="View"
												InsertDT="DTWave Orders View" DisableSwitches="triedn" ForbidRequiredFieldsInSearchMode="True"
												GridPageSize="10" DefaultMode="Grid" DefaultDT="DTWave Orders Search" EditDT="DTWave Orders MultiEdit"
												GridDT="DTWave Orders Search" MultiEditDT="DTWave Orders MultiEdit"></cc2:TableEditor>
											<cc2:DataConnector id="DC1" runat="server" DESIGNTIMEDRAGDROP="43" MasterObjID="TERoutingSet" ChildFields="RoutingSet"
												MasterFields="setid" TargetID="TERoutingSetOrders"></cc2:DataConnector></TD>
										<TD>
											<cc2:TableEditor id="TEAssignOrders" runat="server" AutoSelectGridItem="Never" AutoSelectMode="View"
												DisableSwitches="eidrn" ForbidRequiredFieldsInSearchMode="True" GridPageSize="10" DefaultMode="Search"
												DefaultDT="DTShipment Orders Search" GridDT="DTRoutingSet Orders View" FilterExpression="((STATUS = 'NEW' OR STATUS = 'SASSIGNED' OR STATUS = 'WASSIGNED' OR STATUS = 'RASSIGNED' OR STATUS = 'PLANNED') AND (routingset IS NULL OR routingset = ''))"></cc2:TableEditor></TD>
									</TR>
								</TABLE>
							</TD>
						</TR>
					</TABLE>
				</cc2:datatabcontrol></P>
		</form>
	</body>
</HTML>
