<%@ Page Language="vb" AutoEventWireup="false" Codebehind="SchedulingApplications.aspx.vb" Inherits="WMS.WebApp.SchedulingApplications" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Outbound Order</title>
		<meta content="False" name="vs_showGrid">
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<!-- #include file="~/include/head.html" -->
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout">
		<!-- #include file="~/include/Header.html" -->
		<form id="Form1" method="post" runat="server">
            <telerik:RadScriptManager ID="RadScriptManager2" runat="server">     </telerik:RadScriptManager> 
			<cc3:screen id="Screen1" runat="server" ScreenID="sch"></cc3:screen>
			
			<P><cc2:tableeditor id="TESchedulerApplicationSchedule" runat="server" ConnectionName="Made4NetSchema" AfterUpdateMode="Grid"
					AfterInsertMode="Grid" ForbidRequiredFieldsInSearchMode="True" DESIGNTIMEDRAGDROP="14" GridPageSize="5"
					DefaultMode="Grid" ManualMode="False" DefaultDT="DTSchedulerApplicationSchedule" EditDT="DTSchedulerApplicationScheduleEdit" ObjectID="TESchedulerApplicationSchedule"
					AutoSelectGridItem="Never" AutoSelectMode="View" DisableSwitches="n" InsertDT=""></cc2:tableeditor></P>
			
			<P>&nbsp;</P>
			<P><cc2:datatabcontrol id="DTC" runat="server" ParentID="TESchedulerApplicationSchedule" SyncEditMode="True" Width="760px"
					Height="162px">
					<TABLE id="Table1" cellSpacing="0" cellPadding="0" border="0" runat="server">
						<TR>
							<TD style="WIDTH: 727px">
								<cc2:tabstrip id="TS" runat="server" AutoPostBack="True" TabDefaultStyle="background-color:#DDDDDD;border:solid 1px gray;padding-left:5px;padding-right:5px;font-size:13px;font-weight:bold;padding-top:2px;padding-bottom:2px;"
									TabHoverStyle="background-color:#DDDDDD;" TabSelectedStyle="border:solid 1px gray;border-bottom:none;background:white;"
									SepDefaultStyle="border-bottom:solid 1px gray;" TargetTableID="Tbl">
									<iewc:Tab Text="Application Arguments"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="Application Triggers"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="Application Logging"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									
								</cc2:tabstrip></TD>
						</TR>
						<TR>
							<TD style="BORDER-RIGHT: gray 1px solid; BORDER-LEFT: gray 1px solid; WIDTH: 727px; BORDER-BOTTOM: gray 1px solid; HEIGHT: 18px">
								<TABLE id="Tbl" style="WIDTH: 620px; HEIGHT: 120px" cellSpacing="0" cellPadding="5" border="0"
									runat="server">
									<TR>
										<TD>
											<cc2:TableEditor id="TESchedulerApplicationArguments" runat="server" DisableSwitches="rmn" AutoSelectMode="View"
												AutoSelectGridItem="Never" ObjectID="TESchedulerApplicationArguments" DefaultDT="DTSchedulerApplicationArguments"
												DefaultMode="Grid" GridPageSize="10" ForbidRequiredFieldsInSearchMode="True" AfterInsertMode="Grid"
												ConnectionName="Made4NetSchema"></cc2:TableEditor>
											<cc2:DataConnector id="DC1" runat="server" DESIGNTIMEDRAGDROP="43" SyncMode="Reversed" MasterObjID="TESchedulerApplicationSchedule"
												ChildFields="ScheduleID" MasterFields="ScheduleID" TargetID="TESchedulerApplicationArguments"></cc2:DataConnector></TD>
										<TD>
											<cc2:ShedulerForm id="SheduleTrigger" ShowApplicationList="true" runat="server"></cc2:ShedulerForm></TD>
										<TD>
											<cc2:TableEditor id="TESysLogging" tabIndex="2" runat="server" DisableSwitches="eidrmn" AutoSelectMode="View"
												AutoSelectGridItem="Never" ObjectID="TESysLogging" DefaultDT="DTSysLogging" DefaultMode="Grid"
												ForbidRequiredFieldsInSearchMode="True" ConnectionName="Made4NetSchema"></cc2:TableEditor>
											<cc2:DataConnector id="DC3" runat="server" MasterObjID="TESchedulerApplicationSchedule" ChildFields="ApplicationName"
												MasterFields="ApplicationId" TargetID="TESysLogging"></cc2:DataConnector></TD>
										
									</TR>
								</TABLE>
							</TD>
						</TR>
					</TABLE>
				</cc2:datatabcontrol></P>
		</form>
	</body>
</HTML>
