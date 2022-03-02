<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="LoadsLocation.aspx.vb" Inherits="WMS.WebApp.LoadsLocation"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>LoadsLocation</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<!-- #include file="~/include/head.html" -->
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout">
		<!-- #include file="~/include/Header.html" -->
		<form id="Form1" method="post" runat="server">
            <telerik:RadScriptManager ID="RadScriptManager2" runat="server">     </telerik:RadScriptManager> 
			<cc3:screen id="Screen1" runat="server" ScreenID="ll"></cc3:screen> 
			<P>
				<cc2:tableeditor id="TEMasterLoad" runat="server" ForbidRequiredFieldsInSearchMode="True" DisableSwitches="ident"
					GridCheckAllJSFunction="recalcAll()" DefaultDT="DTLoadSearch" DefaultMode="Search" GridPageSize="5" ObjectID="ML" 
					GridDT="DTLoadLocationGrid" ViewDT="DTLoadView" AutoSelectGridItem="Never" AutoSelectMode="View"></cc2:tableeditor>
			</P>
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
	
	function getLoc(rowNum) {
		var span = document.getElementById(Prefix + rowNum + DestLocSuffix);
		var html = span.innerHTML;
		return html;
	}
	
	function setQty(rowNum) {
		var loc = getLoc(rowNum);
		var qi = getQtyInput(rowNum);
		try {
			qi.value = loc;
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
	
	var Prefix = 'TEMasterLoad_TableEditorGridTEMasterLoad_Grid__ctl';
	var CBSuffix = '_multi_select';
	var INPSuffix = '_DisplayTypeCtrl_NewLocation_tbv_tb';
	var DestLocSuffix = '_DisplayTypeCtrl_DESTINATIONLOCATION';
	
	try {
		var cbs = GridMultiselectCheckboxes_TEMasterLoad_TableEditorGridTEMasterLoad;
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
