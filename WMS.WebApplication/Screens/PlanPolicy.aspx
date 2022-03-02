<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="PlanPolicy.aspx.vb" Inherits="WMS.WebApp.PlanPolicy"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Plan Policy</title>
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
			<cc3:screen id="Screen1" runat="server" ScreenID="ps"></cc3:screen>
			<P><cc2:tableeditor id="TEMasterPlan" runat="server" DisableSwitches="mnvr" DESIGNTIMEDRAGDROP="14"
					GridPageSize="5" DefaultMode="Grid" DefaultDT="DTPlanPolicy" EditDT="DTPlanPolicyEdit" InsertDT="DTPlanPolicyEdit"
					AutoSelectGridItem="Never" AutoSelectMode="View"></cc2:tableeditor></P>
			<P>&nbsp;</P>
			<P><cc2:datatabcontrol id="DTC" runat="server" ParentID="TEMasterPlan" SyncEditMode="True">
					<TABLE id="Table1" cellSpacing="0" cellPadding="0" border="0" runat="server">
						<TR>
							<TD>
								<cc2:tabstrip id="TS" runat="server" TargetTableID="Tbl" SepDefaultStyle="border-bottom:solid 1px gray;"
									TabSelectedStyle="border:solid 1px gray;border-bottom:none;background:white;" TabHoverStyle="background-color:#DDDDDD;"
									TabDefaultStyle="background-color:#DDDDDD;border:solid 1px gray;padding-left:5px;padding-right:5px;font-size:13px;font-weight:bold;padding-top:2px;padding-bottom:2px;"
									AutoPostBack="True">
									<iewc:Tab Text="Details"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="Scoring"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="Picklist Break Strategy"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="Picklist Release Strategy"></iewc:Tab>
									<%--<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="Parallel Pick Release Strategy"></iewc:Tab>--%>
									<iewc:TabSeparator DefaultStyle="width:100%;"></iewc:TabSeparator>
								</cc2:tabstrip></TD>
						</TR>
						<TR>
							<TD style="BORDER-RIGHT: gray 1px solid; BORDER-LEFT: gray 1px solid; BORDER-BOTTOM: gray 1px solid; HEIGHT: 18px">
								<TABLE id="Tbl" cellSpacing="0" cellPadding="5" border="0" runat="server">
									<TR>
										<TD>
											<cc2:TableEditor id="TEPlanPolicyLines" runat="server" InsertDT="DTPlanPolicyDetailEdit" EditDT="DTPlanPolicyDetailEdit"
												DefaultDT="DTPlanPolicyDetailSearch" DefaultMode="Grid" GridPageSize="10" DisableSwitches="mnvr"
												GridDT="DTPlanPolicyDetail" AutoSelectGridItem="Never" AutoSelectMode="View"></cc2:TableEditor>
											<cc2:DataConnector id="DC1" runat="server" DESIGNTIMEDRAGDROP="43" TargetID="TEPlanPolicyLines" MasterFields="strategyid"
												ChildFields="strategyid" MasterObjID="TEMasterPlan"></cc2:DataConnector></TD>
										<TD>
											<cc2:TableEditor id="TEPlanPolicyScoring" runat="server" DefaultDT="DTPlanPolicyScoring" DefaultMode="Grid"
												GridPageSize="10" DisableSwitches="mnvr" AutoSelectGridItem="Never" AutoSelectMode="View"></cc2:TableEditor>
											<cc2:DataConnector id="DC5" runat="server" TargetID="TEPlanPolicyScoring" MasterFields="strategyid"
												ChildFields="strategyid" MasterObjID="TEMasterPlan"></cc2:DataConnector></TD>
										<TD>
											<cc2:TableEditor id="TEPlanPolicyBreak" runat="server" DefaultDT="DTPlanPolicyBreak" DefaultMode="Grid"
												GridPageSize="10" DisableSwitches="mnvr" GridDT="DTPlanPolicyBreakGrid" SearchDT="DTPlanPolicyBreakSearch"
												AutoSelectGridItem="Never" AutoSelectMode="View"></cc2:TableEditor>
											<cc2:DataConnector id="DC2" runat="server" DESIGNTIMEDRAGDROP="43" TargetID="TEPlanPolicyBreak" MasterFields="strategyid"
												ChildFields="strategyid" MasterObjID="TEMasterPlan"></cc2:DataConnector></TD>
										<TD>
											<cc2:TableEditor id="TERelease" runat="server" InsertDT="DTReleasePolicyEdit" EditDT="DTReleasePolicyEdit"
												DefaultDT="DTReleasePolicy" DefaultMode="Grid" DisableSwitches="rmn" GridDT="DTReleasePolicyGrid"
												SortExperssion="Priority" ForbidRequiredFieldsInSearchMode="True" AutoSelectGridItem="Never"
												AutoSelectMode="View"></cc2:TableEditor>
											<cc2:DataConnector id="DC3" runat="server" DESIGNTIMEDRAGDROP="43" TargetID="TERelease" MasterFields="strategyid"
												ChildFields="strategyid" MasterObjID="TEMasterPlan"></cc2:DataConnector></TD>
										<%--<TD>
											<cc2:TableEditor id="TEParallel" runat="server" InsertDT="DTParallelEdit" EditDT="DTParallelEdit"
												DefaultDT="DTParallel" DefaultMode="Grid" DisableSwitches="rmn" GridDT="DTParallelGrid" SortExperssion="Priority"
												ForbidRequiredFieldsInSearchMode="True" AutoSelectGridItem="Never" AutoSelectMode="View"></cc2:TableEditor>
											<cc2:DataConnector id="DC4" runat="server" DESIGNTIMEDRAGDROP="43" TargetID="TEParallel" MasterFields="strategyid"
												ChildFields="strategyid" MasterObjID="TEMasterPlan"></cc2:DataConnector></TD>--%>
									</TR>
								</TABLE>
							</TD>
						</TR>
					</TABLE>
				</cc2:datatabcontrol></P>
		</form>
	</body>
</HTML>
