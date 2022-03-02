<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RouteTasks.aspx.vb" Inherits="WMS.WebApp.RouteTasks" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Route Tasks</title>
</head>
    <body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<cc3:screen id="Screen1" runat="server" ScreenID="rtp"></cc3:screen>
			<P><cc2:tableeditor id="TERoutes" runat="server" GridDT="DTRoutesEdit" DefaultDT="DTRoutesEdit" SearchDT="DTRoutesSearch"
					SQL="SELECT distinct ROUTEID, RUNID, ROUTESTATUS, ROUTESET, DEPO, ROUTENAME, ROUTEDATE, STARTPOINT, ENDPOINT, VEHICLEID, VEHICLETYPE, DRIVER,TERRITORY, ROUTECOST, STARTTIME, ENDTIME, ACTUALSTARTTIME, ACTUALENDTIME, TOTALVOLUME, TOTALWEIGHT, STRATEGYID, numberofstops, numberofpackages FROM vRouteStops"
					DefaultMode="Search" GridPageSize="5" ForbidRequiredFieldsInSearchMode="True" DisableSwitches="venr"
					InsertDT="DTRoutesInsert" EditDT="DTRoutesEdit" SortExperssion="ROUTEID DESC"></cc2:tableeditor></P>
			<P>&nbsp;</P>
			<P><cc2:datatabcontrol id="DTC" runat="server" SyncEditMode="True" ParentID="TERoutes">
					<TABLE id="Table1" cellSpacing="0" cellPadding="0" border="0" runat="server">
						<TR>
							<TD>
								<cc2:tabstrip id="TS" runat="server" TargetTableID="Tbl" SepDefaultStyle="border-bottom:solid 1px gray;"
									TabSelectedStyle="border:solid 1px gray;border-bottom:none;background:white;" TabHoverStyle="background-color:#DDDDDD;"
									TabDefaultStyle="background-color:#DDDDDD;border:solid 1px gray;padding-left:5px;padding-right:5px;font-size:13px;font-weight:bold;padding-top:2px;padding-bottom:2px;"
									AutoPostBack="True">
									<iewc:Tab Text="Route Tasks"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="Add Task"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="Route Packages"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="Add Route Packages"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="Create Packages"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
								</cc2:tabstrip></TD>
						</TR>
						<TR>
							<TD style="BORDER-RIGHT: gray 1px solid; BORDER-LEFT: gray 1px solid; BORDER-BOTTOM: gray 1px solid; HEIGHT: 18px">
								<TABLE id="Tbl" cellSpacing="0" cellPadding="5" border="0" runat="server">
									<TR>
										<TD>
											<cc2:TableEditor id="TERouteStopTasks" runat="server" AutoSelectMode="View" DisableSwitches="trsveid"
												ForbidRequiredFieldsInSearchMode="True" GridPageSize="10" DefaultMode="Grid" MultiEditDT="DTRouteStopTasksME" DefaultDT="DTRouteStopTasksGrid" GridDT="DTRouteStopTasksGridMult"
												AutoSelectGridItem="Never"></cc2:TableEditor>
											<cc2:DataConnector id="DataConnector2" runat="server" DESIGNTIMEDRAGDROP="43" TargetID="TERouteStopTasks" MasterFields="routeid"
												ChildFields="routeid" MasterObjID="TERoutes"></cc2:DataConnector></TD>
										<TD>
											<cc2:TableEditor id="TEAssignTasks" runat="server" AutoSelectMode="View" DisableSwitches="eidrn"
												ForbidRequiredFieldsInSearchMode="True" GridPageSize="10" DefaultMode="Grid" DefaultDT="DTRouteStopTasksAssign" SearchDT="DTRouteStopTasksAssignSearch"
												AutoSelectGridItem="Never"></cc2:TableEditor></TD>
										<TD>
											<cc2:TableEditor id="TEPackages" runat="server" AutoSelectMode="View"
												DisableSwitches="trvie" ForbidRequiredFieldsInSearchMode="True" GridPageSize="10" DefaultMode="Grid"
												DefaultDT="DTRouteAssignedPackages" MultiEditDT="DTRouteAssignedPackagesME" SearchDT="DTRouteAssignedPackagesSearch" AutoSelectGridItem="Never"></cc2:TableEditor>
											<cc2:DataConnector id="DC1" runat="server" DESIGNTIMEDRAGDROP="43" TargetID="TEPackages" MasterFields="routeid"
												ChildFields="routeid" MasterObjID="TERoutes"></cc2:DataConnector></TD>
										<TD>
											<cc2:TableEditor id="TEAssignPackages" runat="server" AutoSelectMode="View" DisableSwitches="eidrn" 
												ForbidRequiredFieldsInSearchMode="True" GridPageSize="10" DefaultMode="Grid" DefaultDT="DTRouteAssignedPackages" SearchDT="DTRouteAssignedPackagesSearch" 
												AutoSelectGridItem="Never" FilterExpression="(STATUS = 'NEW' OR STATUS = 'OFFLOADED') and (routeid='' or routeid is null)"></cc2:TableEditor></TD>
										<TD>
										    <cc2:tableeditor id="TERoutesPackages" runat="server" GridDT="DTRoutePackagesGrid" DefaultDT="DTRoutePackagesGrid" SearchDT="DTRoutePackagesSearch"
					                            DefaultMode="Insert" GridPageSize="5" ForbidRequiredFieldsInSearchMode="True" DisableSwitches="vnr"
					                            InsertDT="DTRoutePackagesAdd" EditDT="DTRoutePackagesEdit"></cc2:tableeditor></TD>
									</TR>
								</TABLE>
							</TD>
						</TR>
					</TABLE>
				</cc2:datatabcontrol></P>
		</form>
</body>
</html>
