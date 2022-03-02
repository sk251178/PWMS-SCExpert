<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="OutboundManagement.aspx.vb" Inherits="WMS.WebApp.OutboundManagement" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Outbound Management</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<!-- #include file="~/include/head.html" -->
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout">
		<!-- #include file="~/include/Header.html" -->
		<form id="Form1" method="post" runat="server">
            <telerik:RadScriptManager ID="RadScriptManager2" runat="server">     </telerik:RadScriptManager> 
			<p><cc3:screen id="Screen1" title="Outbound Management" runat="server" ScreenID="obm"></cc3:screen></p>
			<P>
                <%-- Commented for RWMS-532 --%>
                <%--<cc2:tableeditor id="TEOutboundManage" ObjectID="TEOutboundManage" runat="server" DESIGNTIMEDRAGDROP="14" GridPageSize="5" DefaultMode="Search" 
					ManualMode="False" DefaultDT="DTOutboundManage" DisableSwitches="mnvrtie" SearchDT="DTOutboundManageSearch" 
					SQL="SELECT Pickregion, sum(isnull(PlannedUnits,0)) as PlannedUnits, sum(isnull(ReleasedUnits,0)) as ReleasedUnits, sum(isnull(InProgressUnits,0)) as InProgressUnits, sum(isnull(CompletedUnits,0)) as CompletedUnits, sum(isnull(RemainingUnits,0)) as RemainingUnits, sum(isnull(TotalUnits,0)) as TotalUnits, sum(isnull(PlannedTime,0)) as PlannedTime, sum(isnull(ReleasedTime,0)) as ReleasedTime, sum(isnull(InProgressTime,0)) as InProgressTime, sum(isnull(CompletedTime,0)) as CompletedTime, sum(isnull(RemainingTime,0)) as RemainingTime, sum(isnull(TotalTime,0)) as TotalTime, sum(isnull(PlannedTasks,0)) as PlannedTasks, sum(isnull(ReleasedTasks,0)) as ReleasedTasks, sum(isnull(InProgressTasks,0)) as InProgressTasks, sum(isnull(CompletedTasks,0)) as CompletedTasks, sum(isnull(RemainingTasks,0)) as RemainingTasks, sum(isnull(TotalTasks,0)) as TotalTasks  FROM vOutboundManagementSummary group by pickregion"
					ForbidRequiredFieldsInSearchMode="True" AutoSelectGridItem="Never" AutoSelectMode="Grid"></cc2:tableeditor>--%>
                <%-- Added for RWMS-532/RWMS-379 --%>
                <cc2:tableeditor id="TEOutboundManage" ObjectID="TEOutboundManage" runat="server" DESIGNTIMEDRAGDROP="14" GridPageSize="5" DefaultMode="Search" 
					ManualMode="False" DefaultDT="DTOutboundManage" DisableSwitches="mnvrtie" SearchDT="DTOutboundManageSearch" 
					SQL="SELECT * FROM vOutboundManagementSummary"
					ForbidRequiredFieldsInSearchMode="True" AutoSelectGridItem="Never" AutoSelectMode="Grid"></cc2:tableeditor>

			</P>
		</form>
	</body>
</html>
