<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Shipment.aspx.vb" Inherits="WMS.WebApp.Shipment"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Shipments</title>
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
			<cc3:screen id="Screen1" title="Shipments Setup" runat="server" ScreenID="se"></cc3:screen>
			<P><cc2:tableeditor id="TEMasterShipment" runat="server" SearchDT="DTShipment Search" InsertDT="DTShipment Add"
                    SQL="select distinct SHIPMENT,STATUS,CREATEDATE,SCHEDDATE,SHIPPEDDATE,DOOR,VEHICLE,DRIVER1,NOTES, ACTIVITYSTATUS,CASES from vShipment_Search "
					DisableSwitches="mn" ForbidRequiredFieldsInSearchMode="True" DESIGNTIMEDRAGDROP="14" GridPageSize="5"
					DefaultMode="Search" ManualMode="False" DefaultDT="DTShipment View" EditDT="DTShipment Edit"
					AfterInsertMode="Grid" AfterUpdateMode="Grid" AutoSelectGridItem="Never" AutoSelectMode="View" SortExperssion="SHIPMENT DESC"></cc2:tableeditor></P>
			<P>&nbsp;</P>
			<P><cc2:datatabcontrol id="DTC" runat="server" ParentID="TEMasterShipment" SyncEditMode="True">
					<TABLE id="Table1" cellSpacing="0" cellPadding="0" border="0" runat="server">
						<TR>
							<TD>
								<cc2:tabstrip id="TS" runat="server" TargetTableID="Tbl" SepDefaultStyle="border-bottom:solid 1px gray;"
									TabSelectedStyle="border:solid 1px gray;border-bottom:none;background:white;" TabHoverStyle="background-color:#DDDDDD;"
									TabDefaultStyle="background-color:#DDDDDD;border:solid 1px gray;padding-left:5px;padding-right:5px;font-size:13px;font-weight:bold;padding-top:2px;padding-bottom:2px;"
									AutoPostBack="True">
									<iewc:Tab Text="Details"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="Assign Details"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="Order Lines"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="Assign Order Lines"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="Handling Units"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="Appointment"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="Shipment Payloads"></iewc:Tab>
                                    <iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="Shipment Containers"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="Shipment Cases"></iewc:Tab>
									<%--<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="Shipment Compartments"></iewc:Tab>	--%>
									<iewc:TabSeparator DefaultStyle="width:100%;"></iewc:TabSeparator>
								</cc2:tabstrip></TD>
						</TR>
						<TR>
							<TD style="BORDER-RIGHT: gray 1px solid; BORDER-LEFT: gray 1px solid; BORDER-BOTTOM: gray 1px solid; HEIGHT: 18px">
								<TABLE id="Tbl" cellSpacing="0" cellPadding="5" border="0" runat="server">
									<TR>
									    <TD>
											<cc2:TableEditor id="TEShipmentDetails" runat="server" AutoSelectMode="View" AutoSelectGridItem="Never" ForbidRequiredFieldsInSearchMode="true"
												DefaultDT="DTShipmentLines" DefaultMode="Grid" GridPageSize="10" SearchDT="DTShipmentLinesSearch" DisableSwitches="trnmie"></cc2:TableEditor>
											<cc2:DataConnector id="DataConnector4" runat="server" DESIGNTIMEDRAGDROP="43" TargetID="TEShipmentDetails" MasterFields="SHIPMENT"
												ChildFields="SHIPMENT" MasterObjID="TEMasterShipment"></cc2:DataConnector></TD>
										<TD>
											<cc2:TableEditor id="TEShipmentAssignDetails" runat="server" AutoSelectMode="View" AutoSelectGridItem="Never" SearchDT="DTShipAssignOrderSearch" ForbidRequiredFieldsInSearchMode="true"
												DefaultDT="DTShipAssignOrder" DefaultMode="Grid" GridPageSize="10" DisableSwitches="eidrn"></cc2:TableEditor></TD>
										<TD>
											<cc2:TableEditor id="TEShipmentOrders" runat="server" AutoSelectMode="View" AutoSelectGridItem="Never" ForbidRequiredFieldsInSearchMode="true"
												EditDT="DTShipmentDetailsEdit" DefaultDT="DTShipmentDetails" DefaultMode="Grid" GridPageSize="10"
												DisableSwitches="trnmie" SearchDT="DTShipmentDetailsSearch"></cc2:TableEditor>
											<cc2:DataConnector id="DC1" runat="server" DESIGNTIMEDRAGDROP="43" TargetID="TEShipmentOrders" MasterFields="SHIPMENT"
												ChildFields="SHIPMENT" MasterObjID="TEMasterShipment"></cc2:DataConnector></TD>
										<TD>
											<cc2:TableEditor id="TEAssignOrders" runat="server" AutoSelectMode="View" AutoSelectGridItem="Never" SearchDT="DTShipmentAssignOrdersSearch" ForbidRequiredFieldsInSearchMode="true"
												DefaultDT="DTShipmentAssignOrders" DefaultMode="Grid" GridPageSize="10" DisableSwitches="eidrn" FilterExpression="qtyopen > 0"></cc2:TableEditor></TD>
										<TD>
											<cc2:TableEditor id="TEHUTrans" runat="server" AutoSelectMode="View" AutoSelectGridItem="Never" EditDT="TEHUTransEdit"
												DefaultDT="DTHUTrans" DefaultMode="Grid" GridPageSize="10" ForbidRequiredFieldsInSearchMode="True"
												DisableSwitches="dmnvs" InsertDT="TEHUTransInsert" FilterExpression="TRANSACTIONTYPE = 'SHIPMENT'"
												ObjectID="TEHUTrans"></cc2:TableEditor>
											<cc2:DataConnector id="DC4" runat="server" DESIGNTIMEDRAGDROP="43" TargetID="TEHUTrans" MasterFields="SHIPMENT"
												ChildFields="TransactionTypeId" MasterObjID="TEMasterShipment"></cc2:DataConnector></TD>
										<TD>
											<cc2:TableEditor id="TEYardEntry" runat="server" AutoSelectMode="View" AutoSelectGridItem="Never"
												EditDT="DTYardEntryEditRS" DefaultDT="DTYardEntry" GridPageSize="10" ForbidRequiredFieldsInSearchMode="True"
                                                DisableSwitches="mn" AfterInsertMode="Grid" GridDT="DTYardEntry"
												InsertDT="DTYardEntryEditRS"  DefaultMode="Grid"  ></cc2:TableEditor>
											<cc2:DataConnector id="Dataconnector2" runat="server" DESIGNTIMEDRAGDROP="43" TargetID="TEYardEntry" MasterFields="SHIPMENT"
												ChildFields="SHIPMENT" MasterObjID="TEMasterShipment" SyncMode="Reversed"></cc2:DataConnector></TD>
										<TD>
											<cc2:TableEditor id="TEShipmentLoads" runat="server" AutoSelectMode="Grid" AutoSelectGridItem="Never"
												DefaultDT="DTShipmentLoads" MultiEditDT ="DTShipmentLoadsME" DefaultMode="Grid" GridPageSize="10" DisableSwitches="rsievdm"></cc2:TableEditor>
											<cc2:DataConnector id="Dataconnector1" runat="server" TargetID="TEShipmentLoads" MasterFields="SHIPMENT"
												ChildFields="SHIPMENT" MasterObjID="TEMasterShipment"></cc2:DataConnector></TD>
										<TD>
											<cc2:TableEditor id="TEShipmentContainers" runat="server" AutoSelectMode="Grid" AutoSelectGridItem="Never"
												DefaultDT="DTShipmentContainers" MultiEditDT ="DTShipmentContainersME" DefaultMode="Grid" GridPageSize="10" DisableSwitches="rsievdm"></cc2:TableEditor>
											<cc2:DataConnector id="Dataconnector3" runat="server" TargetID="TEShipmentContainers" MasterFields="SHIPMENT"
												ChildFields="SHIPMENT" MasterObjID="TEMasterShipment"></cc2:DataConnector></TD>
										<TD>
											<cc2:TableEditor id="TEShipmentCases" runat="server" AutoSelectMode="Grid" AutoSelectGridItem="Never"
												DefaultDT="DTShipmentCases" MultiEditDT ="DTShipmentCasesME" DefaultMode="Grid" GridPageSize="10" DisableSwitches="rsievdm"></cc2:TableEditor>
											<cc2:DataConnector id="Dataconnector5" runat="server" TargetID="TEShipmentCases" MasterFields="SHIPMENT"
												ChildFields="SHIPMENT" MasterObjID="TEMasterShipment"></cc2:DataConnector></TD>

										<%--<TD>
											<cc2:TableEditor id="TEShipmentCompartments" runat="server" AutoSelectMode="Grid" AutoSelectGridItem="Never"
												DefaultDT="DTShipmentCompartment" DefaultMode="Grid" GridPageSize="10" DisableSwitches="rnivdm"></cc2:TableEditor>
											<cc2:DataConnector id="Dataconnector3" runat="server" TargetID="TEShipmentCompartments" MasterFields="SHIPMENT"
												ChildFields="SHIPMENT" MasterObjID="TEMasterShipment"></cc2:DataConnector></TD>--%>
									</TR>
								</TABLE>
							</TD>
						</TR>
					</TABLE>
				</cc2:datatabcontrol></P>
		</form>
	</body>
</HTML>