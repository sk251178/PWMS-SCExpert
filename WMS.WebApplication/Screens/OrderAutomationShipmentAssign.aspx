<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="OrderAutomationShipmentAssign.aspx.vb" Inherits="WMS.WebApp.OrderAutomationShipmentAssign" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
    <HEAD>
		<title>Shipment Assign Policy</title>
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
			<p><cc3:screen id="Screen1" title="Shipment Assign Policy" runat="server" ScreenID="SAP"></cc3:screen></p>
			<P><cc2:tableeditor id="TESHIPASSIGNPOLICY" runat="server" GridPageSize="5" DefaultMode="Search"
					ManualMode="False" DefaultDT="DTShipAssignPolicy" DisableSwitches="mnvr"  EditDT="DTShipAssignPolicyEdit" InsertDT="DTShipAssignPolicyEdit"
					SearchDT="DTShipAssignPolicySearch" ForbidRequiredFieldsInSearchMode="True" AutoSelectGridItem="Never"
					AutoSelectMode="View"></cc2:tableeditor></P>
            <P>&nbsp;&nbsp;</P>
			<P><cc2:datatabcontrol id="DTC" runat="server" ParentID="TESHIPASSIGNPOLICY" SyncEditMode="True">
					<TABLE id="Table1" cellSpacing="0" cellPadding="0" border="0" runat="server">
						<TR>
							<TD>
								<cc2:tabstrip id="TS" runat="server" AutoPostBack="True" TabDefaultStyle="background-color:#DDDDDD;border:solid 1px gray;padding-left:5px;padding-right:5px;font-size:13px;font-weight:bold;padding-top:2px;padding-bottom:2px;"
									TabHoverStyle="background-color:#DDDDDD;" TabSelectedStyle="border:solid 1px gray;border-bottom:none;background:white;"
									SepDefaultStyle="border-bottom:solid 1px gray;" TargetTableID="Tbl">
									<iewc:Tab Text="Details"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="Scheduling"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
								</cc2:tabstrip></TD>
						</TR>
						<TR>
							<TD style="BORDER-RIGHT: gray 1px solid; BORDER-LEFT: gray 1px solid; BORDER-BOTTOM: gray 1px solid; HEIGHT: 18px">
								<TABLE id="Tbl" cellSpacing="0" cellPadding="5" border="0" runat="server">
									<TR>
										<TD>
                        <cc2:TableEditor id="TEShippmentAssignmentDetail" runat="server" DefaultDT="DTSHIPMENTASSIGNMENTDetailInsert" 
                        GridDT="DTSHIPMENTASSIGNMENTDetailGrid" DefaultMode="Grid" GridPageSize="10" DisableSwitches="mnvr"
                        AutoSelectGridItem="Never" AutoSelectMode="View"></cc2:TableEditor>

                        <cc2:DataConnector id="DC1" runat="server" TargetID="TEShippmentAssignmentDetail" MasterFields="TEMPLATENAME"
                        ChildFields="TEMPLATENAME" MasterObjID="TESHIPASSIGNPOLICY"></cc2:DataConnector>
                                        </TD>
										<TD>
											<cc2:ShedulerForm id="SheduleTrigger" ShowApplicationList="true" runat="server"></cc2:ShedulerForm>
										</TD>
									</TR>
								</TABLE>
							</TD>
						</TR>
					</TABLE>
				</cc2:datatabcontrol>
			</P>
		</form>
	</body>
</html>
