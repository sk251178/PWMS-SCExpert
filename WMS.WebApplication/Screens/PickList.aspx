<%@ Page Language="vb" AutoEventWireup="false" Codebehind="PickList.aspx.vb" Inherits="WMS.WebApp.PickList"%>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Picklist</title>
		<meta content="False" name="vs_showGrid">
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<!-- #include file="~/include/head.html" -->
	</HEAD>
	<body dir="ltr" bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout">
		<!-- #include file="~/include/Header.html" -->
		<form id="Form1" method="post" runat="server">
            <telerik:RadScriptManager ID="RadScriptManager2" runat="server">     </telerik:RadScriptManager> 
			<cc3:screen id="Screen1" runat="server" ScreenID="pk"></cc3:screen>
			<P><cc2:tableeditor id="TEPicklistHeader" runat="server" DisableSwitches="idrn" DESIGNTIMEDRAGDROP="14"
					GridPageSize="5" DefaultMode="Search" DefaultDT="DTPICKHEADERGrid" GridCheckAllJSFunction="recalcAllHeader()"
					EditDT="DTPicklistEdit" GridDT="DTPICKHEADERGrid" SortExperssion="CREATEDATE DESC, PICKLIST DESC" SearchDT="DTPICKHEADERSearch" ForbidRequiredFieldsInSearchMode="True"
					SQL="select distinct PICKLIST, PICKTYPE, PICKMETHOD, STRATEGYID, 
					                     CREATEDATE, PLANDATE, RELEASEDATE, ASSIGNEDDATE, COMPLETEDDATE, 
					                     STATUS, FROMWAREHOUSEAREA, WAVE, HANDELINGUNITTYPE, 
					                     adddate, adduser, editdate, edituser, USERID,COMPANY,COMPANYNAME,CUBE,CONSIGNEE from vPICKHEADER_SEARCH"
					
					ViewDT="DTPICKHEADERGrid"  AutoSelectGridItem="Never" AutoSelectMode="View"></cc2:tableeditor></P>
			<P>&nbsp;</P>
			<P><cc2:datatabcontrol id="DTC" runat="server" SyncEditMode="True" ParentID="TEPicklistHeader">
					<TABLE id="Table1" cellSpacing="0" cellPadding="0" border="0" runat="server">
						<TR>
							<TD>
								<cc2:tabstrip id="TS" runat="server" TargetTableID="Tbl" SepDefaultStyle="border-bottom:solid 1px gray;"
									TabSelectedStyle="border:solid 1px gray;border-bottom:none;background:white;" TabHoverStyle="background-color:#DDDDDD;"
									TabDefaultStyle="background-color:#DDDDDD;border:solid 1px gray;padding-left:5px;padding-right:5px;font-size:13px;font-weight:bold;padding-top:2px;padding-bottom:2px;"
									AutoPostBack="True">
									<iewc:Tab Text="Details"></iewc:Tab>
									<iewc:TabSeparator DefaultStyle="width:100%;"></iewc:TabSeparator>
                                    <iewc:Tab Text="Picking Weight Capture"></iewc:Tab>
									<iewc:TabSeparator DefaultStyle="width:100%;"></iewc:TabSeparator>

								</cc2:tabstrip></TD>
						</TR>
						<TR>
							<TD style="BORDER-RIGHT: gray 1px solid; BORDER-LEFT: gray 1px solid; BORDER-BOTTOM: gray 1px solid; HEIGHT: 18px">
								<TABLE id="Tbl" cellSpacing="0" cellPadding="5" border="0" runat="server">
									<TR>
										<TD>
											<cc2:TableEditor id="TEPicklistDetailPartail" runat="server" AutoSelectMode="Grid" SortExperssion="PICKLISTLINE" AutoSelectGridItem="Never"
												EditDT="DTPicklistDetailEdit" DefaultDT="DTPicklistDetail" DefaultMode="Grid" GridPageSize="20" 
												DisableSwitches="nvrie"></cc2:TableEditor>
											<cc2:DataConnector id="DC1" runat="server" DESIGNTIMEDRAGDROP="43" TargetID="TEPicklistDetailPartail"
												MasterFields="picklist" ChildFields="picklist" MasterObjID="TEPicklistHeader"></cc2:DataConnector></TD>

                                        <TD>
											<%--<cc2:TableEditor id="TEPickingWeightCapture" runat="server" AutoSelectMode="Grid" AutoSelectGridItem="Never"
												InsertDT="DTPickingWeightCaptureNew"  GridDT="DTPickingWeightCaptureGrid" EditDT="DTPickingWeightCaptureEdit" DefaultDT="DTPickingWeightCaptureNew" 
												DefaultMode="Insert"  SortExperssion="ID" GridPageSize="20"
												DisableSwitches="mnv" SearchDT="DTPickingWeightCaptureSearch" AfterInsertMode="Insert"></cc2:TableEditor>--%>

                                            <cc2:TableEditor id="TEPickingWeightCapture" runat="server" AutoSelectMode="View" AutoSelectGridItem="Never"
												 GridDT="DTLOADETWEIGHTGRID"  DefaultDT="DTLOADETWEIGHTGRID" MultiEditDT="DTLOADETWEIGHTGRIDEDIT"
												DisableSwitches="mntvrie" DefaultMode="Grid"  SortExperssion="ID" GridPageSize="10" 
												 SearchDT="DTPickingWeightCaptureSearch" ></cc2:TableEditor>

											<cc2:DataConnector id="DC2" runat="server" DESIGNTIMEDRAGDROP="43" TargetID="TEPickingWeightCapture"
												MasterFields="picklist" ChildFields="picklist" MasterObjID="TEPicklistHeader"></cc2:DataConnector></TD>
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
	
	var Prefix = 'DTC_TEPicklistDetailPartail_TableEditorGridTEPicklistDetailPartail_Grid__ctl';
	var CBSuffix = '_multi_select';
	var INPSuffix = '_DisplayTypeCtrl_UNITS_tbv_tb';
	var AdjQtySuffix = '_DisplayTypeCtrl_ADJQTY';
	var PickedQtySuffix = '_DisplayTypeCtrl_PICKEDQTY';
	
	try {
		var cbs = GridMultiselectCheckboxes_DTC_TEPicklistDetailPartail_TableEditorGridTEPicklistDetailPartail;
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
	} catch (e) {}
	
	//try {
	//	var cbs = document.getElementById("DTC_TEPicklistDetailPartail_TableEditorGridTEPicklistDetailPartail_Grid__ctl1_chkMultiSelect_SelectAll");
	//	alert(cbs.onclick);
	//} catch (e){}
	
		</script>
	</body>
</HTML>
