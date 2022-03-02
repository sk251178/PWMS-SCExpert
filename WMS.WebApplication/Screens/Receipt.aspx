<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Receipt.aspx.vb" Inherits="WMS.WebApp.Receipt"%>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Receipt</title>
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
			<cc3:screen id="Screen1" runat="server" ScreenID="re"></cc3:screen>
			<P><cc2:tableeditor id="TEMasterReceipt" runat="server" DisableSwitches="ind" ForbidRequiredFieldsInSearchMode="True"
					SearchDT="DTRECEIPTHEADERVIEWSearch" DESIGNTIMEDRAGDROP="14" GridPageSize="5" DefaultMode="Search" SortExperssion="ADDDATE DESC"
					ManualMode="False" DefaultDT="DTReceiptHeader" EditDT="DTReceiptHeader" InsertDT="DTReceiptHeader" 
					AfterInsertMode="Grid" GridDT="DTRECEIPTHEADERVIEW" SQL="SELECT distinct RECEIPT, STATUS, SCHEDULEDDATE, BOL, convert(nvarchar,NOTES) as NOTES, CARRIERCOMPANY, STARTRECEIPTDATE, ADDDATE, ADDUSER, EDITDATE, EDITUSER, TRAILER, VEHICLE, DRIVER1, DRIVER2, SEAL1, SEAL2,CLOSERECEIPTDATE,TRANSPORTREFERENCE,TRANSPORTTYPE,DOOR,YARDENTRYID,ESTUNLOADINGTIME,LABELPRINTED,CONFIRMED,COMPANY,COMPANYNAME,ORDERID,CONSIGNEE  FROM RECEIPTHEADERVIEW"
					ViewDT="DTRECEIPTHEADERVIEW" AutoSelectGridItem="Never" AutoSelectMode="View"></cc2:tableeditor></P>
			<P>&nbsp;</P>
			<P><cc2:datatabcontrol id="DTC" runat="server" ParentID="TEMasterReceipt" SyncEditMode="True">
					<TABLE id="Table1" cellSpacing="0" cellPadding="0" border="0" runat="server">
						<TR>
							<TD>
								<cc2:tabstrip id="TS" runat="server" AutoPostBack="True" TabDefaultStyle="background-color:#DDDDDD;border:solid 1px gray;padding-left:5px;padding-right:5px;font-size:13px;font-weight:bold;padding-top:2px;padding-bottom:2px;"
									TabHoverStyle="background-color:#DDDDDD;" TabSelectedStyle="border:solid 1px gray;border-bottom:none;background:white;"
									SepDefaultStyle="border-bottom:solid 1px gray;" TargetTableID="Tbl">
									<iewc:Tab Text="Details"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="Receipt Payloads"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="ASN Payloads"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="Handling Units"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="Import Order"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="Appointment"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="Receiving Exceptions"></iewc:Tab>									
									<iewc:TabSeparator DefaultStyle="width:100%;"></iewc:TabSeparator>
								</cc2:tabstrip></TD>
						</TR>
						<TR>
							<TD style="BORDER-RIGHT: gray 1px solid; BORDER-LEFT: gray 1px solid; BORDER-BOTTOM: gray 1px solid; HEIGHT: 18px">
								<TABLE id="Tbl" cellSpacing="0" cellPadding="5" border="0" runat="server">
									<TR>
										<TD>
											<cc2:TableEditor id="TEReceiptDetail"  runat="server" AutoSelectMode="View" AutoSelectGridItem="Never"
												InsertDT="DTReceiptDetailAdd" EditDT="DTReceiptDetailEdit" DefaultDT="DTReceiptDetail"
												DefaultMode="Grid" GridPageSize="10" SearchDT="DTReceiptDetailSearch" DisableSwitches="irnm" ObjectID="TEReceiptDetail"
												ResolveNullOnInsert="False" TableName="RECEIPTDETAIL" AfterInsertMode="Grid" ></cc2:TableEditor>
											<cc2:DataConnector id="DC1" runat="server" MasterObjID="TEMasterReceipt" ChildFields="receipt" MasterFields="receipt"
												TargetID="TEReceiptDetail"></cc2:DataConnector></TD>
										<TD>
											<asp:panel id="pnlAdj" Runat="server">
												<TABLE border="0">
													<TR>
														<TD>
															<cc2:fieldLabel id="lblAdjReasonCode" runat="server" text="Adjustment Reason"></cc2:fieldLabel></TD>
														<TD>
															<cc2:dropdownlist id="ddReasonCode" runat="server" AutoPostBack="True" TableName="CODELISTDETAIL"
																Where="CODELISTCODE = 'INVADJRC'" ValueField="CODE" TextField="DESCRIPTION"></cc2:dropdownlist></TD>
													</TR>
												</TABLE>
											</asp:panel>
											<HR>
											<cc2:TableEditor id="TEReceiptLOADS" runat="server" AutoSelectMode="View" AutoSelectGridItem="Never"
												DefaultDT="DTReceiptLoad" SortExperssion="CASE WHEN ISNUMERIC(LOADID) = 1 THEN CAST(LOADID AS int) ELSE 99999999   
                                               END,STATUS" DefaultMode="Grid" GridPageSize="10" DisableSwitches="rsneid" TableName="LOADS"></cc2:TableEditor>  
											<cc2:DataConnector id="DC2" runat="server" MasterObjID="TEMasterReceipt" ChildFields="receipt" MasterFields="receipt"
												TargetID="TEReceiptLOADS"></cc2:DataConnector></TD>
										<TD>
											<cc2:TableEditor id="TEReceiptASN" runat="server" AutoSelectMode="View" AutoSelectGridItem="Never" InsertDT="DTASNLoadNew" 
												DefaultDT="DTASNLoad" EditDT="DTASNLoadEdit" DefaultMode="Grid" GridPageSize="10" DisableSwitches="rsnm"></cc2:TableEditor>
											<cc2:DataConnector id="DC3" runat="server" MasterObjID="TEMasterReceipt" ChildFields="receipt" MasterFields="receipt"
												TargetID="TEReceiptASN"></cc2:DataConnector></TD>
										<TD>
											<cc2:TableEditor id="TEHUTrans" runat="server" AutoSelectMode="View" AutoSelectGridItem="Never" InsertDT="TEHUTransInsert"
												EditDT="TEHUTransEdit" DefaultDT="DTHUTrans" DefaultMode="Grid" GridPageSize="10" ForbidRequiredFieldsInSearchMode="True"
												DisableSwitches="dmnvs" ObjectID="TEHUTrans" FilterExpression="TRANSACTIONTYPE = 'RECEIPT'"></cc2:TableEditor>
											<cc2:DataConnector id="DC4" runat="server" DESIGNTIMEDRAGDROP="43" MasterObjID="TEMasterReceipt" ChildFields="TransactionTypeId"
												MasterFields="receipt" TargetID="TEHUTrans"></cc2:DataConnector></TD>
										<TD>
											<cc2:TableEditor id="TEAvailableInboundLines" runat="server" AutoSelectMode="View" AutoSelectGridItem="Never"
												DefaultDT="DTAvailableInboundLines" SortExperssion ="OrderID" DefaultMode="Search" GridPageSize="10" DisableSwitches="rneid"></cc2:TableEditor></TD>
										<TD>
											<cc2:TableEditor id="TEYardEntry" runat="server" AutoSelectMode="View" AutoSelectGridItem="Never"
												InsertDT="DTYardEntryEditRS" EditDT="DTYardEntryEditRS" DefaultDT="DTYardEntry" DefaultMode="Grid"
												GridPageSize="10" DisableSwitches="rsnm" AfterInsertMode="View" AfterUpdateMode="View"></cc2:TableEditor>
											<cc2:DataConnector id="Dataconnector2" runat="server" MasterObjID="TEMasterReceipt" ChildFields="YARDENTRYID"
												MasterFields="YARDENTRYID" TargetID="TEYardEntry"></cc2:DataConnector></TD>
										<TD>
											<cc2:TableEditor id="TEReceivingException" runat="server" AutoSelectMode="Grid" AutoSelectGridItem="Never"
												InsertDT="DTReceivingExceptionInsert" EditDT="DTReceivingExceptionEdit" DefaultDT="DTReceivingException" DefaultMode="Grid"
												GridPageSize="10" DisableSwitches="rsndmtv"></cc2:TableEditor>
											<cc2:DataConnector id="Dataconnector1" runat="server" MasterObjID="TEMasterReceipt" ChildFields="RECEIPT"
												MasterFields="RECEIPT" TargetID="TEReceivingException"></cc2:DataConnector></TD>
												
									</TR>
								</TABLE>
							</TD>
						</TR>
					</TABLE>
				</cc2:datatabcontrol></P>
		</form>
	</body>
</HTML>
