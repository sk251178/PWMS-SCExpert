<%@ Page Language="vb" AutoEventWireup="false" Codebehind="RouteStopDetail.aspx.vb" Inherits="WMS.WebApp.RouteStopDetail" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>RouteStopDetails</title>
		<meta content="False" name="vs_showGrid">
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout">
		<FORM id="Form1" method="post" runat="server">
			<cc3:screen id="Screen1" runat="server" ScreenID="RSD"></cc3:screen>
			<P><cc2:tableeditor id="TERoutes" runat="server" GridDT="DTRoutesEdit" DefaultDT="DTRoutesEdit" SearchDT="DTRoutesSearch"
					SQL="SELECT distinct ROUTEID, RUNID, ROUTESTATUS, ROUTESET, DEPO, ROUTENAME, ROUTEDATE, STARTPOINT, ENDPOINT, VEHICLEID, VEHICLETYPE, DRIVER,TERRITORY, ROUTECOST, STARTTIME, ENDTIME, ACTUALSTARTTIME, ACTUALENDTIME, TOTALVOLUME, TOTALWEIGHT, STRATEGYID FROM vRouteStops"
					DefaultMode="Search" GridPageSize="5" ForbidRequiredFieldsInSearchMode="True" DisableSwitches="venr"
					InsertDT="DTRoutesInsert" EditDT="DTRoutesEdit" SortExperssion="ROUTEID DESC"></cc2:tableeditor></P>
			<P><cc2:datatabcontrol id="DTC" runat="server" SyncEditMode="True" ParentID="TERoutes">
					<TABLE id="Table1" cellSpacing="0" cellPadding="0" border="0" runat="server">
						<TR>
							<TD>
								<cc2:tabstrip id="TS" runat="server" TargetTableID="Tbl" SepDefaultStyle="border-bottom:solid 1px gray;"
									TabSelectedStyle="border:solid 1px gray;border-bottom:none;background:white;" TabHoverStyle="background-color:#DDDDDD;"
									TabDefaultStyle="background-color:#DDDDDD;border:solid 1px gray;padding-left:5px;padding-right:5px;font-size:13px;font-weight:bold;padding-top:2px;padding-bottom:2px;"
									AutoPostBack="True">
									<iewc:Tab Text="Route Stops"></iewc:Tab>
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
											<cc2:TableEditor id="TEStops" runat="server" EditDT="DTRouteStopInsert" InsertDT="DTRouteStopInsert" FilterExpression="stopnumber is not null"
												SortExpression="stopnumber"  DisableSwitches="mne" ForbidRequiredFieldsInSearchMode="True" DefaultMode="Grid" DefaultDT="DTRouteStopGrid"
												AllowDeleteInViewMode="False" DESIGNTIMEDRAGDROP="60" AutoSelectGridItem="Never" GridDT="DTRouteStopGridMult" AutoSelectMode="View"
												ObjectID="TEStops"></cc2:TableEditor> 
											<cc2:DataConnector id="DC1" runat="server" DESIGNTIMEDRAGDROP="38" TargetID="TEStops" MasterFields="ROUTEID"
												ChildFields="ROUTEID" MasterObjID="TERoutes"></cc2:DataConnector></TD>
										<TD>
												<cc2:TableEditor id="TEAssignOrders" runat="server" DisableSwitches="eidrn" ForbidRequiredFieldsInSearchMode="True"
													 GridPageSize="10" DefaultMode="Grid" DefaultDT="DTRouting Orders View" GridDT="DTRouting Orders View"
													FilterExpression="((STATUS = 'NEW' OR STATUS = 'SASSIGNED' OR STATUS = 'WASSIGNED' OR STATUS = 'RASSIGNED' OR STATUS = 'PLANNED')) AND (ROUTE = '' or route is null) "
													AutoSelectMode="View" AutoSelectGridItem="Never"></cc2:TableEditor></TD>
									</TR>
								</TABLE>
							</TD>
						</TR>
					</TABLE>
				</cc2:datatabcontrol></P>
		</FORM>
	</body>
</HTML>
