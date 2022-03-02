<%@ Page Language="vb" AutoEventWireup="false" Codebehind="InvCount.aspx.vb" Inherits="WMS.WebApp.InvCount"%>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>InvCount</title>
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
			<cc3:screen id="Screen1" runat="server" ScreenID="ic"></cc3:screen><br>
			<cc2:tableeditor id="TEINVCOUNT" runat="server" TableName="LOADS" DefaultDT="DTINVCOUNT" ManualMode="False"
				DefaultMode="Search" GridPageSize="5" EditVS="0" GridDT="DTINVCOUNTGRID" DisableSwitches="nvidre"
				ForbidRequiredFieldsInSearchMode="True" FilterExpression="STATUS IS NOT NULL AND STATUS <> ''"
				Visible="True" GridCheckAllJSFunction="recalcAll()" AutoSelectGridItem="Never" AutoSelectMode="View" SortExperssion="CONSIGNEE, LOADID"></cc2:tableeditor>
		</form>
		<script language="javascript">

			function applyQty() {
				var cb = event.srcElement;
				var rowNum = getRowNum(cb);
				if (cb.checked) {
					setQty(rowNum)
					setLoc(rowNum)
				} else {
					clearQty(rowNum);
				}
			}

			function applyQtyCB(cb) {
				var rowNum = getRowNum(cb);
				if (cb.checked) {
					setQty(rowNum)
					setLoc(rowNum)
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
				qi = getLocInput(rowNum);
				try {
					qi.value = '';
					qi.disabled = true;
					qi.style.backgroundColor = '#DDDDDD';
				} catch (e) {}
			}
		
			function setQty(rowNum) {
				var qi = getQtyInput(rowNum);
				try {
					qi.value = 0;
					qi.disabled = false;
					qi.style.backgroundColor = '';
				} catch (e) {}
				qi = getLocInput(rowNum);
				try {
					qi.value = '';
					qi.disabled = false;
					qi.style.backgroundColor = '';
				} catch (e) {}
			}
		
			function getQtyInput(rowNum) {
				return document.getElementById(Prefix + rowNum + INPSuffixQty);
			}
			function getLocInput(rowNum) {
				return document.getElementById(Prefix + rowNum + INPSuffixLoc);
			}
			
			function getLoc(rowNum,Suffix) {
				var span = document.getElementById(Prefix + rowNum + INPSuffixOrgLoc);
				var html = span.innerHTML;
				return html;
			}
			
			function setLoc(rowNum) {
				var li = getLocInput(rowNum);
				try {
					li.value = getLoc(rowNum,INPSuffixLoc);
				} catch (e) {}
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
			
			var Prefix = 'TEINVCOUNT_TableEditorGridTEINVCOUNT_Grid__ctl';
			var CBSuffix = '_multi_select';
			var INPSuffixQty = '_DisplayTypeCtrl_TOQTY_tb';
			var INPSuffixLoc = '_DisplayTypeCtrl_TOLOCATION_tb';
			var INPSuffixOrgLoc = '_DisplayTypeCtrl_LOCATION';
			
			try {
				var cbs = GridMultiselectCheckboxes_TEINVCOUNT_TableEditorGridTEINVCOUNT;
				for (n=0; n<=cbs.length-1; n++) {
					var cb = document.getElementById(cbs[n])
					if (!cb.checked)
					{
						var rowNum = getRowNum(cb)
						var inp = getQtyInput(rowNum)
						inp.disabled = true;
						inp.style.backgroundColor = '#DDDDDD';
						inp = getLocInput(rowNum)
						inp.disabled = true;
						inp.style.backgroundColor = '#DDDDDD';
					}
					cb.onclick = applyQty;
				}
			} catch (e) {}
		
		</script>
	</body>
</HTML>
