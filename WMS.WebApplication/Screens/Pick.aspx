<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Pick.aspx.vb" Inherits="WMS.WebApp.Pick"%>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Picking</title>
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
			<cc3:screen id="Screen1" runat="server" ScreenID="pi"></cc3:screen>
			<P><cc2:tableeditor id="TEPick" runat="server" GridDT="DTPickGrid" SearchDT="DTPickSearch" EditDT="DTPickEdit"
					DefaultDT="DTPick" AfterInsertMode="Grid" ManualMode="False" DefaultMode="Search" GridPageSize="5"
					DESIGNTIMEDRAGDROP="14" ForbidRequiredFieldsInSearchMode="True" DisableSwitches="mnrtid" GridCheckAllJSFunction="recalcAll()"
					AutoSelectGridItem="Never" AutoSelectMode="View"></cc2:tableeditor></P>
			<P>&nbsp;</P>
			<P>&nbsp;</P>
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
		var strQty = html.substring(html.indexOf('">')+2,html.indexOf('</SPAN>'));
		strQty = strQty.replace(',','');
		var qty = parseInt(strQty);
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
	
	function recalcAll() {
		for (n=0; n<=cbs.length-1; n++) {
			var cb = document.getElementById(cbs[n]);
			applyQtyCB(cb);
		}
	}
	
	var Prefix = 'TEPick_TableEditorGridTEPick_Grid__ctl';
	var CBSuffix = '_multi_select';
	var INPSuffix = '_DisplayTypeCtrl_input_qty_tb';
	var AdjQtySuffix = '_DisplayTypeCtrl_ADJQTY';
	var PickedQtySuffix = '_DisplayTypeCtrl_PICKEDQTY';
	
	try {
		var cbs = GridMultiselectCheckboxes_TEPick_TableEditorGridTEPick;
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
	
		</script>
	</body>
</HTML>
