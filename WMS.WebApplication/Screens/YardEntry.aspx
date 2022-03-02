<%@ Register TagPrefix="cc4" Namespace="Made4Net.WebControls.Charting" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="YardEntry.aspx.vb" Inherits="WMS.WebApp.YardEntry"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>YardEntry</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<cc3:screen id="Screen1" runat="server" ScreenID="ye"></cc3:screen>
			<P><cc2:tableeditor id="TEYardEntry" runat="server" AfterUpdateMode="Grid" AfterInsertMode="Grid" EditDT="DTYardEntryEdit"
					DefaultDT="DTYardEntry" ManualMode="False" DefaultMode="Search" GridPageSize="5" DESIGNTIMEDRAGDROP="14"
					ForbidRequiredFieldsInSearchMode="True" DisableSwitches="nrm" InsertDT="DTYardEntryAdd" SearchDT="DTYardEntrySearch"></cc2:tableeditor></P>
			<P>&nbsp;</P>
			<P><cc2:datatabcontrol id="DTC" runat="server" SyncEditMode="True" ParentID="TEYardEntry">
					<TABLE id="Table1" cellSpacing="0" cellPadding="0" border="0" runat="server">
						<TR>
							<TD>
								<cc2:tabstrip id="TS" runat="server" TargetTableID="Tbl" SepDefaultStyle="border-bottom:solid 1px gray;"
									TabSelectedStyle="border:solid 1px gray;border-bottom:none;background:white;" TabHoverStyle="background-color:#DDDDDD;"
									TabDefaultStyle="background-color:#DDDDDD;border:solid 1px gray;padding-left:5px;padding-right:5px;font-size:13px;font-weight:bold;padding-top:2px;padding-bottom:2px;"
									AutoPostBack="True">
									<iewc:Tab Text="Receipts"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="Assign Receipts"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="Shipments"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="Assign Shipments"></iewc:Tab>
									<iewc:TabSeparator DefaultStyle="width:100%;"></iewc:TabSeparator>
								</cc2:tabstrip></TD>
						</TR>
						<TR>
							<TD style="BORDER-RIGHT: gray 1px solid; BORDER-LEFT: gray 1px solid; BORDER-BOTTOM: gray 1px solid; HEIGHT: 18px">
								<TABLE id="Tbl" cellSpacing="0" cellPadding="5" border="0" runat="server">
									<TR>
										<TD>
											<cc2:TableEditor id="TEYardEntryReceipts" runat="server" SearchDT="DTReceiptHeaderSearch" DisableSwitches="trien"
												ForbidRequiredFieldsInSearchMode="True" GridPageSize="10" DefaultMode="Grid" DefaultDT="DTReceiptHeader"></cc2:TableEditor>
											<cc2:DataConnector id="DC1" runat="server" DESIGNTIMEDRAGDROP="43" TargetID="TEYardEntryReceipts" MasterFields="yardentryid"
												ChildFields="yardentryid" MasterObjID="TEYardEntry"></cc2:DataConnector></TD>
										<TD>
											<cc2:TableEditor id="TEAssignReceipts" runat="server" AutoSelectMode="View" DisableSwitches="eidrn"
												ForbidRequiredFieldsInSearchMode="True" GridPageSize="10" DefaultMode="Grid" DefaultDT="DTReceiptHeaderSearch"
												GridDT="DTAssignReceiptYardEntry" AutoSelectGridItem="Never" FilterExpression="(STATUS <> 'CLOSE' and STATUS <> 'CANCELLED') AND (yardentryid IS NULL OR yardentryid = '')"></cc2:TableEditor></TD>
										<TD>
											<cc2:TableEditor id="TEYardEntryShipments" runat="server" SearchDT="DTShipment Search" DisableSwitches="trien"
												ForbidRequiredFieldsInSearchMode="True" GridPageSize="10" DefaultMode="Grid" DefaultDT="DTShipment View"></cc2:TableEditor>
											<cc2:DataConnector id="DC2" runat="server" DESIGNTIMEDRAGDROP="43" TargetID="TEYardEntryShipments"
												MasterFields="yardentryid" ChildFields="yardentryid" MasterObjID="TEYardEntry"></cc2:DataConnector></TD>
										<TD>
											<cc2:TableEditor id="TEAssignShipments" runat="server" AutoSelectMode="View" DisableSwitches="eidrn" ForbidRequiredFieldsInSearchMode="True"
												GridPageSize="10" DefaultMode="Grid" DefaultDT="DTShipment Search" GridDT="DTAssignShipmentYardEntry"
												FilterExpression="(STATUS <> 'SHIPPED' and STATUS <> 'CANCELLED') AND (yardentryid IS NULL OR yardentryid = '')"></cc2:TableEditor></TD>
									</TR>
								</TABLE>
							</TD>
						</TR>
					</TABLE>
				</cc2:datatabcontrol></P>
		</form>
	</body>
</HTML>
