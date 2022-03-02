<%@ Page Language="vb" AutoEventWireup="false" Codebehind="SplitLoad.aspx.vb" Inherits="WMS.WebApp.SplitLoad"%>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>SplitLoad</title>
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
			<cc3:screen id="Screen1" title="Load Split" runat="server"></cc3:screen><br>
			<cc2:tableeditor id="TESPLITLOAD" runat="server" TableName="LOADS" DefaultDT="DTINVSPLITQTY" ManualMode="False"
				DefaultMode="Search" GridPageSize="5" EditVS="0" GridDT="DTINVSPLITQTYGRID" DisableSwitches="mnvidre"
				ForbidRequiredFieldsInSearchMode="True" FilterExpression="STATUS IS NOT NULL AND STATUS <> ''"
				Visible="True" GridCheckAllJSFunction="SplitRecalcAll()" AutoSelectGridItem="Never" AutoSelectMode="View"></cc2:tableeditor>
		</form>
		<script language="javascript">

			function applySplitQty() {
				var cb1 = event.srcElement;
				var rowNum = getSplitRowNum(cb1);
				if (cb1.checked) {
					setSplitQty(rowNum)
				} else {
					clearSplitQty(rowNum);
				}
			}

			function applySplitQtyCB(cb1) {
				var rowNum = getSplitRowNum(cb1);
				if (cb1.checked) {
					setSplitQty(rowNum)
				} else {
					clearSplitQty(rowNum);
				}
			}

			function clearSplitQty(rowNum) {
				var qi = getSplitQtyInput(rowNum);
				try {
					qi.value = '';
					qi.disabled = true;
					qi.style.backgroundColor = '#DDDDDD';
				} catch (e) {}
				qi = getSplitLocInput(rowNum);
				try {
					qi.value = '';
					qi.disabled = true;
					qi.style.backgroundColor = '#DDDDDD';
				} catch (e) {}
				qi = getSplitLoadInput(rowNum);
				try {
					qi.value = '';
					qi.disabled = true;
					qi.style.backgroundColor = '#DDDDDD';
				} catch (e) {}
			}
		
			function setSplitQty(rowNum) {
				var qi = getSplitQtyInput(rowNum);
				try {
					qi.value = 0;
					qi.disabled = false;
					qi.style.backgroundColor = '';
				} catch (e) {}
				qi = getSplitLocInput(rowNum);
				try {
					qi.value = '';
					qi.disabled = false;
					qi.style.backgroundColor = '';
				} catch (e) {}
				qi = getSplitLoadInput(rowNum);
				try {
					qi.value = '';
					qi.disabled = false;
					qi.style.backgroundColor = '';
				} catch (e) {}
			}
		
			function getSplitQtyInput(rowNum) {
				return document.getElementById(SplitPrefix + rowNum + INPSuffixQty);
			}
			function getSplitLocInput(rowNum) {
				return document.getElementById(SplitPrefix + rowNum + INPSuffixLoc);
			}
			function getSplitLoadInput(rowNum) {
				return document.getElementById(SplitPrefix + rowNum + INPSuffixLoad);
			}
		
			function getSplitRowNum(cb1) {
				var strRowNum = cb1.id.substring(SplitPrefix.length,cb1.id.length - CBSuffix.length);
				return parseInt(strRowNum);
			}
		
			function SplitRecalcAll() {
				for (n=0; n<=cbsSplit.length-1; n++) {
					var cb1 = document.getElementById(cbsSplit[n]);
					applySplitQtyCB(cb1);
				}
			}
			
			var SplitPrefix = 'TESPLITLOAD_TableEditorGridTESPLITLOAD_Grid__ctl';
			var CBSuffix = '_multi_select';
			var INPSuffixQty = '_DisplayTypeCtrl_TOQTY_tb';
			var INPSuffixLoad = '_DisplayTypeCtrl_TOLOADID_tb';
			var INPSuffixLoc = '_DisplayTypeCtrl_TOLOCATION_tb';
			
			try {
				var cbsSplit = GridMultiselectCheckboxes_TESPLITLOAD_TableEditorGridTESPLITLOAD;
				for (n=0; n<=cbsSplit.length-1; n++) {
					var cb1 = document.getElementById(cbsSplit[n])
					if (!cb1.checked)
					{
						var rowNum = getSplitRowNum(cb1)
						var inp = getSplitQtyInput(rowNum)
						inp.disabled = true;
						inp.style.backgroundColor = '#DDDDDD';
						inp = getSplitLocInput(rowNum)
						inp.disabled = true;
						inp.style.backgroundColor = '#DDDDDD';
						inp = getSplitLoadInput(rowNum)
						inp.disabled = true;
						inp.style.backgroundColor = '#DDDDDD';
					}
					cb1.onclick = applySplitQty;
				}
			} catch (e) {}
		
		</script>
	</body>
</HTML>
