<%@ Page Language="vb" AutoEventWireup="false" Codebehind="AssemblyWorkOrder.aspx.vb" Inherits="WMS.WebApp.AssemblyWorkOrder"%>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Register TagPrefix="cc4" Namespace="Made4Net.WebControls.Charting" Assembly="Made4Net.WebControls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>AssemblyWorkOrder</title>
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
			<cc3:screen id="Screen1" runat="server" ScreenID="aw"></cc3:screen>
			<P><cc2:tableeditor id="TEWorkOrder" runat="server" SearchDT="DTWorkOrderSearch" DisableSwitches="nrivem"
					ForbidRequiredFieldsInSearchMode="True" DESIGNTIMEDRAGDROP="14" GridPageSize="5" DefaultMode="Search"
					ManualMode="False" DefaultDT="DTWorkOrder" ObjectID="TEWorkOrder" FilterExpression="documenttype = 'ASMBL'"></cc2:tableeditor></P>
			<P>&nbsp;</P>
			<P><cc2:datatabcontrol id="DTC" runat="server" ParentID="TEWorkOrder" SyncEditMode="True">
					<TABLE id="Table1" cellSpacing="0" cellPadding="0" border="0" runat="server">
						<TR>
							<TD>
								<cc2:tabstrip id="TS" runat="server" TargetTableID="Tbl" SepDefaultStyle="border-bottom:solid 1px gray;"
									TabSelectedStyle="border:solid 1px gray;border-bottom:none;background:white;" TabHoverStyle="background-color:#DDDDDD;"
									TabDefaultStyle="background-color:#DDDDDD;border:solid 1px gray;padding-left:5px;padding-right:5px;font-size:13px;font-weight:bold;padding-top:2px;padding-bottom:2px;"
									AutoPostBack="True">
									<iewc:Tab Text="Work Order Execution"></iewc:Tab>
									<iewc:TabSeparator DefaultStyle="width:100%;"></iewc:TabSeparator>
								</cc2:tabstrip></TD>
						</TR>
						<TR>
							<TD style="BORDER-RIGHT: gray 1px solid; BORDER-LEFT: gray 1px solid; BORDER-BOTTOM: gray 1px solid; HEIGHT: 18px">
								<TABLE id="Tbl" cellSpacing="0" cellPadding="5" border="0" runat="server">
									<TR>
										<TD> 
											<cc2:TableEditor id="TEWorkOrderSelecttLoads" runat="server" DefaultDT="WorkOrderSelectLoads" DefaultMode="MultilineEdit"
												ForbidRequiredFieldsInSearchMode="True" DisableSwitches="ridvte" AfterInsertMode="MultilineEdit"
												AfterUpdateMode="MultilineEdit" GridDT="WorkOrderSelectLoads" MultiEditDT="WorkOrderSelectLoadsME"></cc2:TableEditor>
											<cc2:DataConnector id="DC1" runat="server" SyncMode="Normal" TargetID="TEWorkOrderSelecttLoads" MasterFields="consignee,orderid,location"
												ChildFields="consignee, orderid, location" MasterObjID="TEWorkOrder"></cc2:DataConnector></TD>
									</TR>
								</TABLE>
							</TD>
						</TR>
					</TABLE>
				</cc2:datatabcontrol></P>
		</form>
	</body>
</HTML>
