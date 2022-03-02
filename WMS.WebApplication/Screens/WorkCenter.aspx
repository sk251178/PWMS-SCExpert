<%@ Page Language="vb" AutoEventWireup="false" Codebehind="WorkCenter.aspx.vb" Inherits="WMS.WebApp.WorkCenter"%>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Register TagPrefix="cc4" Namespace="Made4Net.WebControls.Charting" Assembly="Made4Net.WebControls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>WorkCenter</title>
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
			<cc3:screen id="Screen1" runat="server" ScreenID="wc"></cc3:screen>
			<P><cc2:tableeditor id="TEWorkOrder" runat="server" SearchDT="DTWorkOrderSearch" InsertDT="DTWorkOrderAdd"
					DisableSwitches="nr" ForbidRequiredFieldsInSearchMode="True" DESIGNTIMEDRAGDROP="14" GridPageSize="5"
					DefaultMode="Search" ManualMode="False" DefaultDT="DTWorkOrder" EditDT="DTWorkOrderEdit" AfterInsertMode="Grid"
					AfterUpdateMode="Grid" ObjectID="TEWorkOrder"></cc2:tableeditor></P>
			<P>&nbsp;</P>
			<P><cc2:datatabcontrol id="DTC" runat="server" ParentID="TEWorkOrder" SyncEditMode="True">
					<TABLE id="Table1" cellSpacing="0" cellPadding="0" border="0" runat="server">
						<TR>
							<TD>
								<cc2:tabstrip id="TS" runat="server" AutoPostBack="True" TabDefaultStyle="background-color:#DDDDDD;border:solid 1px gray;padding-left:5px;padding-right:5px;font-size:13px;font-weight:bold;padding-top:2px;padding-bottom:2px;"
									TabHoverStyle="background-color:#DDDDDD;" TabSelectedStyle="border:solid 1px gray;border-bottom:none;background:white;"
									SepDefaultStyle="border-bottom:solid 1px gray;" TargetTableID="Tbl">
									<iewc:Tab Text="Work Order BOM"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="Work Order Produced Loads"></iewc:Tab>
									<iewc:TabSeparator DefaultStyle="width:100%;"></iewc:TabSeparator>
								</cc2:tabstrip></TD>
						</TR>
						<TR>
							<TD style="BORDER-RIGHT: gray 1px solid; BORDER-LEFT: gray 1px solid; BORDER-BOTTOM: gray 1px solid; HEIGHT: 18px">
								<TABLE id="Tbl" cellSpacing="0" cellPadding="5" border="0" runat="server">
									<TR>
										<TD>
											<cc2:TableEditor id="TEWorkOrderBOM" runat="server" DefaultDT="DTWorkOrderBOM" DefaultMode="Grid"
												GridPageSize="10" ForbidRequiredFieldsInSearchMode="True" DisableSwitches="tmvn" AutoSelectGridItem="Never"
												AutoSelectMode="View" GridDT="DTWorkOrderBOMGrid" ViewDT="DTWorkOrderBOMGrid"></cc2:TableEditor>
											<cc2:DataConnector id="DC2" runat="server" DESIGNTIMEDRAGDROP="43" MasterObjID="TEWorkOrder" ChildFields="consignee,orderid"
												MasterFields="consignee,orderid" TargetID="TEWorkOrderBOM"></cc2:DataConnector></TD>
										<TD>
											<cc2:TableEditor id="TEWorkOrderLoads" runat="server" DefaultDT="DTWOProducedLoads" DefaultMode="Grid"
												ForbidRequiredFieldsInSearchMode="True" DisableSwitches="ridvtne" AutoSelectGridItem="Never"
												AutoSelectMode="View" GridDT="DTWOProducedLoads"></cc2:TableEditor>
											<cc2:DataConnector id="Dataconnector1" runat="server" MasterObjID="TEWorkOrder" ChildFields="consignee,orderid"
												MasterFields="consignee,orderid" TargetID="TEWorkOrderLoads"></cc2:DataConnector></TD>
									</TR>
								</TABLE>
							</TD>
						</TR>
					</TABLE>
				</cc2:datatabcontrol></P>
		</form>
	</body>
</HTML>
