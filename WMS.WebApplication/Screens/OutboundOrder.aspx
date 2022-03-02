<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="OutboundOrder.aspx.vb" Inherits="WMS.WebApp.OutboundOrder"%>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN">
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
			<cc3:screen id="Screen1" runat="server" ScreenID="oo"></cc3:screen>
			<P><cc2:tableeditor id="TEMasterOutboundOrders" runat="server" SQL="SELECT distinct CONSIGNEE, ORDERID, ORDERTYPE, REFERENCEORD, TARGETCOMPANY, COMPANYTYPE, STATUS, CREATEDATE,NOTES, STAGINGLANE, REQUESTEDDATE, SCHEDULEDDATE, SHIPPEDDATE,STOPNUMBER,STATUSDATE, LOADINGSEQ, ROUTINGSET, ROUTE, DELIVERYSTATUS, POD, ORDERPRIORITY ,COMPANYNAME, SHIPTO,NUMCASES,NUMLINES,WAVE,ORDERUNITS, ORDERLINES, HOSTORDERID,SHIPMENT   FROM ORDERHEADERVIEW "
					GridDT="DTORDERHEADERVIEW" AfterUpdateMode="Grid" AfterInsertMode="Grid" SearchDT="DTORDERHEADERVIEWSearch" SortExperssion="CREATEDATE DESC"
					InsertDT="DTOutOrdHeader Insert" DisableSwitches="nd" ForbidRequiredFieldsInSearchMode="True"
					DESIGNTIMEDRAGDROP="14" GridPageSize="5" DefaultMode="Search" ManualMode="False" DefaultDT="DTOutOrdHeader View"
					EditDT="DTOutOrdHeader Edit" ViewDT="DTORDERHEADERVIEW" ObjectID="TEMasterOutboundOrders" AutoSelectGridItem="Never"
					AutoSelectMode="View"></cc2:tableeditor></P>
			<P>&nbsp;</P>
			<P><cc2:datatabcontrol id="DTC" runat="server" ParentID="TEMasterOutboundOrders" SyncEditMode="True">
					<TABLE id="Table1" cellSpacing="0" cellPadding="0" border="0" runat="server">
						<TR>
							<TD>
								<cc2:tabstrip id="TS" runat="server" AutoPostBack="True" TabDefaultStyle="background-color:#DDDDDD;border:solid 1px gray;padding-left:5px;padding-right:5px;font-size:13px;font-weight:bold;padding-top:2px;padding-bottom:2px;"
									TabHoverStyle="background-color:#DDDDDD;" TabSelectedStyle="border:solid 1px gray;border-bottom:none;background:white;"
									SepDefaultStyle="border-bottom:solid 1px gray;" TargetTableID="Tbl">
									<iewc:Tab Text="Details"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
                                    <iewc:Tab Text="Route/Stop"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="Pickdetails"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="Order Payloads"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="Order Exception"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="Handling Units"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="Contact Detail"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
								</cc2:tabstrip></TD>
						</TR>
						<TR>
							<TD style="BORDER-RIGHT: gray 1px solid; BORDER-LEFT: gray 1px solid; BORDER-BOTTOM: gray 1px solid; HEIGHT: 18px">
								<TABLE id="Tbl" cellSpacing="0" cellPadding="5" border="0" runat="server">
									<TR>
										<TD>
											<cc2:TableEditor id="TEOutboundOrderLines" runat="server" AutoSelectMode="View" AutoSelectGridItem="Never"
												ObjectID="TEOutboundOrderLines" EditDT="DTOutOrdetail Edit" DefaultDT="DTOutOrdetail Grid" DefaultMode="Grid"
												GridPageSize="10" ForbidRequiredFieldsInSearchMode="True" DisableSwitches="mn" InsertDT="DTOutOrdetail Add"
												SearchDT="DTOutOrdetail Search" AfterInsertMode="Grid" GridDT="DTOutOrdetail Grid"></cc2:TableEditor>
											<cc2:DataConnector id="DC1" runat="server" DESIGNTIMEDRAGDROP="43" SyncMode="Reversed" MasterObjID="TEMasterOutboundOrders"
												ChildFields="consignee,orderid" MasterFields="consignee,orderid" TargetID="TEOutboundOrderLines"></cc2:DataConnector></TD>
                                         <TD>
											<cc2:TableEditor id="TERouteStop" runat="server" AutoSelectMode="View" AutoSelectGridItem="Never"
												ObjectID="TERouteStop" EditDT="DTOrderLineRouteStopEdit"  DefaultDT="DTOrderLineRouteStopGrid" DefaultMode="Grid" GridPageSize="10" ForbidRequiredFieldsInSearchMode="True"
												DisableSwitches="ids" GridDT="DTOrderLineRouteStopGrid"></cc2:TableEditor>
											<cc2:DataConnector id="DC6" runat="server" MasterObjID="TEMasterOutboundOrders" ChildFields="consignee,orderid"
												MasterFields="consignee,orderid" TargetID="TERouteStop"></cc2:DataConnector></TD>

										<TD>
											<cc2:TableEditor id="TEOrderPicks" runat="server" AutoSelectMode="View" AutoSelectGridItem="Never"
												ViewDT="DTOrderPickDetail" DefaultDT="DTPicklistDetail" DefaultMode="Grid" ForbidRequiredFieldsInSearchMode="True"
												DisableSwitches="ridvte" GridDT="DTPicklistDetail"></cc2:TableEditor>
											<cc2:DataConnector id="DC2" runat="server" MasterObjID="TEMasterOutboundOrders" ChildFields="consignee,orderid"
												MasterFields="consignee,orderid" TargetID="TEOrderPicks"></cc2:DataConnector></TD>
										<TD>
											<cc2:TableEditor id="TEOrderLoads" runat="server" AutoSelectMode="View" AutoSelectGridItem="Never" MultiEditDT="DTOutOrdLoadsME"
												DefaultDT="DTOutOrdLoads" DefaultMode="Grid" ForbidRequiredFieldsInSearchMode="True" DisableSwitches="ridvte"></cc2:TableEditor>
											<cc2:DataConnector id="DC5" runat="server" MasterObjID="TEMasterOutboundOrders" ChildFields="consignee,orderid"
												MasterFields="consignee,orderid" TargetID="TEOrderLoads"></cc2:DataConnector></TD>
										<TD>
											<cc2:TableEditor id="TEOrderException" runat="server" AutoSelectMode="View" AutoSelectGridItem="Never"
												DefaultDT="DTOrderException" DefaultMode="Grid" ForbidRequiredFieldsInSearchMode="True" DisableSwitches="rmidvte"></cc2:TableEditor>
											<cc2:DataConnector id="DC3" runat="server" MasterObjID="TEMasterOutboundOrders" ChildFields="consignee,orderid"
												MasterFields="consignee,orderid" TargetID="TEOrderException"></cc2:DataConnector></TD>
										<TD>
											<cc2:TableEditor id="TEHUTrans" runat="server" AutoSelectMode="View" AutoSelectGridItem="Never" ObjectID="TEHUTrans"
												EditDT="TEHUTransEdit" DefaultDT="DTHUTrans" DefaultMode="Grid" GridPageSize="10" ForbidRequiredFieldsInSearchMode="True"
												DisableSwitches="dmnvs" InsertDT="TEHUTransInsert" FilterExpression="TRANSACTIONTYPE = 'OUTORD'"></cc2:TableEditor>
											<cc2:DataConnector id="Dataconnector1" runat="server" DESIGNTIMEDRAGDROP="43" MasterObjID="TEMasterOutboundOrders"
												ChildFields="consignee,orderid" MasterFields="consignee,orderid" TargetID="TEHUTrans"></cc2:DataConnector></TD>
										<TD>
											<cc2:TableEditor id="TEContactDetail" runat="server" AutoSelectMode="View" AutoSelectGridItem="Never"
												EditDT="DTCONTACT" DefaultDT="DTCONTACT" DefaultMode="Grid" ForbidRequiredFieldsInSearchMode="True"
												DisableSwitches="rdmnv" InsertDT="DTShipTo"></cc2:TableEditor>
											<cc2:DataConnector id="DC4" runat="server" SyncMode="Normal" MasterObjID="TEMasterOutboundOrders" ChildFields="contactid"
												MasterFields="shipto" TargetID="TEContactDetail"></cc2:DataConnector></TD>
									</TR>
								</TABLE>
							</TD>
						</TR>
					</TABLE>
				</cc2:datatabcontrol></P>
		</form>
		<script language="javascript">

			function applyQty() {
				var cb = event.srcElement;
				var rowNum = getRowNum(cb);
				if (cb.checked) {
					setQty(rowNum)
				} else {
					clearQty(rowNum);
				}
			}

			function applyQtyCB(cb) {
				var rowNum = getRowNum(cb);
				if (cb.checked) {
					setQty(rowNum)
				} else {
					clearQty(rowNum);
				}
			}

			function clearQty(rowNum) {
				var qi = getQtyInput(rowNum);
				try {
					qi.value = '';
				} catch (e) {}
			}

			function setQty(rowNum) {
				var qi = getQtyInput(rowNum);
				try {
					qi.value = calcQty(rowNum);
				} catch (e) {}
			}

			function getQtyInput(rowNum) {
				return document.getElementById(Prefix + rowNum + INPSuffix);
			}

			function getQty(rowNum,Suffix) {
				var span = document.getElementById(Prefix + rowNum + Suffix);
				var html = span.innerHTML;
				var qty = parseInt(html);
				return qty;
			}

			function getAdjQty(rowNum) {
				return getQty(rowNum,AdjQtySuffix);
			}

			function getPickedQty(rowNum) {
				return getQty(rowNum,PickedQtySuffix);
			}

			function calcQty(rowNum) {
				var qty = getAdjQty(rowNum) - getPickedQty(rowNum);
				if (qty < 0) qty = 0;
				return qty;
			}

			function getRowNum(cb) {
				var strRowNum = cb.id.substring(Prefix.length,cb.id.length - CBSuffix.length);
				return parseInt(strRowNum);
			}

			function recalcAllHeader(){}

			function recalcAll() {
				return;
				for (n=0; n<=cbs.length-1; n++) {
					var cb = document.getElementById(cbs[n]);
					applyQtyCB(cb);
				}
			}

			var Prefix = 'DTC_TEOrderPicks_TableEditorGridTEOrderPicks_Grid__ctl';
			var CBSuffix = '_multi_select';
			var INPSuffix = '_DisplayTypeCtrl_UNITS_tbv_tb';
			var AdjQtySuffix = '_DisplayTypeCtrl_ADJQTY';
			var PickedQtySuffix = '_DisplayTypeCtrl_PICKEDQTY';

			try {
				var cbs = GridMultiselectCheckboxes_DTC_TEOrderPicks_TableEditorGridTEOrderPicks;
				for (n=0; n<=cbs.length-1; n++) {
					var cb = document.getElementById(cbs[n]);
					var rowNum = getRowNum(cb);
					if (calcQty(rowNum) == 0) {
						cb.disabled = true;
						var inp = getQtyInput(rowNum)
						inp.disabled = true;
						inp.style.backgroundColor = '#DDDDDD';
					} else {
						cb.onclick = applyQty;
					}
				}
			}
			catch (e) {}
	</body>
</HTML>