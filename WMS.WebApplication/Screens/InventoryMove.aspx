<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="InventoryMove.aspx.vb" Inherits="WMS.WebApp.InventoryMove"%>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Inventory Move</title>
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
			<cc3:screen id="Screen1" runat="server" ScreenID="iv"></cc3:screen><br>
			<cc2:tableeditor id="TEINVLOADMOVE" runat="server" DefaultDT="DTINVADJMOVE" ManualMode="False" DefaultMode="Search"
				GridPageSize="5" DESIGNTIMEDRAGDROP="14" GridDT="DTINVADJMOVEGRID" DisableSwitches="nvidre" ForbidRequiredFieldsInSearchMode="True"
				GridCheckAllJSFunction="recalcAll()" FilterExpression="STATUS IS NOT NULL AND STATUS <> ''" TableName="LOADS"
				AutoSelectGridItem="Never" AutoSelectMode="View"></cc2:tableeditor>
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
			qi.disabled = true;
			qi.style.backgroundColor = '#DDDDDD';
		} catch (e) {}
	}
	
	function setQty(rowNum) {
		var qi = getQtyInput(rowNum);
		try {
			qi.value = '';
			qi.disabled = false;
			qi.style.backgroundColor = '';
		} catch (e) {}
	}
	
	function getQtyInput(rowNum) {
		return document.getElementById(Prefix + rowNum + INPSuffix);
	}
	
	function getRowNum(cb) {
		var strRowNum = cb.id.substring(Prefix.length,cb.id.length - CBSuffix.length);
		return parseInt(strRowNum);
	}
	
	function recalcAll() {
		for (n=0; n<=cbs.length-1; n++) {
			var cb = document.getElementById(cbs[n]);
			applyQtyCB(cb);
		}
	}
	
	var Prefix = 'TEINVLOADMOVE_TableEditorGridTEINVLOADMOVE_Grid__ctl';
	var CBSuffix = '_multi_select';
	var INPSuffix = '_DisplayTypeCtrl_TOLOCATION_tb';
	
	try {
		var cbs = GridMultiselectCheckboxes_TEINVLOADMOVE_TableEditorGridTEINVLOADMOVE;
		for (n=0; n<=cbs.length-1; n++) {
			var cb = document.getElementById(cbs[n]);
			if (!cb.checked)
			{
				var rowNum = getRowNum(cb);
				var inp = getQtyInput(rowNum)
				inp.disabled = true;
				inp.style.backgroundColor = '#DDDDDD';
			}
			cb.onclick = applyQty;
		}
	} catch (e) {}
	
		</script>
	</body>
</HTML>
