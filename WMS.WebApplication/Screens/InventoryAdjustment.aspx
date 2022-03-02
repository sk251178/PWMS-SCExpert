<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="InventoryAdjustment.aspx.vb" Inherits="WMS.WebApp.InventoryAdjustment" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Inventory Adjustment</title>
		<meta content="False" name="vs_snapToGrid">
		<meta content="True" name="vs_showGrid">
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
			<cc3:screen id="Screen1" runat="server" ScreenID="ij"></cc3:screen><br>
			<asp:panel id="pnlAdj" Runat="server">
				<TABLE border="0">
					<TR>
						<TD style="HEIGHT: 16px">
							<cc2:fieldLabel id="lblAdjType" runat="server" text="AdjType"></cc2:fieldLabel></TD>
						<TD style="HEIGHT: 16px">
							<cc2:dropdownlist id="ddInvAct" runat="server" TableName="CODELISTDETAIL" TextField="DESCRIPTION" AllOption="false"
								ValueField="CODE" Where="CODELISTCODE = 'INVADJTYPE'" AutoPostBack="True"></cc2:dropdownlist></TD>
					</TR>
					<TR>
						<TD>
							<cc2:fieldLabel id="lblAdjReasonCode" runat="server" text="Adjustment Reason"></cc2:fieldLabel></TD>
						<TD>
							<cc2:dropdownlist id="ddReasonCode" runat="server" TableName="CODELISTDETAIL" TextField="DESCRIPTION"
								ValueField="CODE" Where="CODELISTCODE = 'INVADJRC'" AutoPostBack="True"></cc2:dropdownlist></TD>
					</TR>
				</TABLE>
			</asp:panel>
			<hr>
			<asp:panel id="pnlAddSubQty" Runat="server">
				<cc2:tableeditor id="TEINVLOADADJ" runat="server" FilterExpression="STATUS IS NOT NULL AND STATUS <> ''"
					GridCheckAllJSFunction="recalcAll()" ForbidRequiredFieldsInSearchMode="True" DisableSwitches="nvidre"
					GridDT="DTINVADJQTYGRID" GridPageSize="5" DefaultMode="Search" ManualMode="False" DefaultDT="DTINVADJQTY"
					AutoSelectGridItem="Never" AutoSelectMode="View"></cc2:tableeditor>
			</asp:panel>
           
            <asp:panel id="pnlSplitLoad" Runat="server">
				<cc2:tableeditor id="TESPLITLOAD" runat="server" FilterExpression="STATUS IS NOT NULL AND STATUS <> ''"
					GridCheckAllJSFunction="SplitRecalcAll()" ForbidRequiredFieldsInSearchMode="True" DisableSwitches="nvidre"
					GridDT="DTINVSPLITQTYGRID" GridPageSize="5" DefaultMode="Search" ManualMode="False" DefaultDT="DTINVSPLITQTY"
					AutoSelectGridItem="Never" AutoSelectMode="View"></cc2:tableeditor>
			</asp:panel>
            <asp:panel id="pnlChangeUOM" Runat="server">
				<%--<cc2:tableeditor id="TECHANGEUOM" runat="server" FilterExpression="STATUS IS NOT NULL AND STATUS <> ''"
					DisableSwitches="vidre" GridPageSize="10" DefaultMode="Search" ManualMode="False" DefaultDT="DTCHANGEUOM"
					MultiEditDT="DTCHANGEUOMMulti" AutoSelectGridItem="Never" AutoSelectMode="View"></cc2:tableeditor>--%>
                <cc2:tableeditor id="TECHANGEUOM" runat="server" FilterExpression="STATUS IS NOT NULL AND STATUS <> ''"
					DisableSwitches="vidre" GridPageSize="10" DefaultMode="Search" ManualMode="False" DefaultDT="DTCHANGEUOM"
					MultiEditDT="DTCHANGEUOMMulti" AutoSelectGridItem="Never" AutoSelectMode="View"></cc2:tableeditor>
			</asp:panel>
            
			<asp:panel id="pnlChangeSku" Runat="server">
				<cc2:tableeditor id="TECHANHESKU" runat="server" FilterExpression="STATUS IS NOT NULL AND STATUS <> ''"
					DisableSwitches="nvidre" GridPageSize="10" DefaultMode="Search" ManualMode="False" DefaultDT="DTINVADJCHNSKU"
					GridDT="DTINVADJCHNSKU" AutoSelectGridItem="Never" AutoSelectMode="View"></cc2:tableeditor>
			</asp:panel>
            
			<asp:panel id="pnlCreateLoad" Runat="server">
				
                <cc2:tableeditor id="TECREATELOAD" runat="server" DisableSwitches="nvsdre" GridPageSize="10" DefaultMode="Insert" ManualMode="False" DefaultDT="DTInvAdjCreateLoadWithAtt"
					InsertDT="DTInvAdjCreateLoadWithAtt" AfterInsertMode="Insert" AutoSelectGridItem="Never" AutoSelectMode="View"></cc2:tableeditor>
			</asp:panel>
            <asp:panel id="pnlCreateLoadNew" Runat="server">
				<cc2:tableeditor id="TECREATELOADNEW" runat="server" DisableSwitches="nvsdre" GridPageSize="10" DefaultMode="Insert" ManualMode="False" DefaultDT="DTInvAdjCreateLoadWithAttInvNew"
					InsertDT="DTInvAdjCreateLoadWithAttInvNew" AfterInsertMode="Insert" AutoSelectGridItem="Never" AutoSelectMode="View"></cc2:tableeditor>
                </asp:panel>		

		</form>
		<%--<script language="javascript">

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
					qi.value = 0;
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
			
			function applySplitQty() {
				var cb1 = event.srcElement;
				var rowNum1 = getSplitRowNum(cb1);
				if (cb1.checked) {
					setSplitQty(rowNum1)
				} else {
					clearSplitQty(rowNum1);
				}
			}

			function applySplitQtyCB(cb1) {
				var rowNum1 = getSplitRowNum(cb1);
				if (cb1.checked) {
					setSplitQty(rowNum1)
				} else {
					clearSplitQty(rowNum1);
				}
			}

			function clearSplitQty(rowNum1) {
				var pi = getSplitQtyInput(rowNum1);
				try {
					pi.value = '';
					pi.disabled = true;
					pi.style.backgroundColor = '#DDDDDD';
				} catch (e) {}
				pi = getSplitLocInput(rowNum1);
				try {
					pi.value = '';
					pi.disabled = true;
					pi.style.backgroundColor = '#DDDDDD';
				} catch (e) {}
				pi = getSplitLoadInput(rowNum1);
				try {
					pi.value = '';
					pi.disabled = true;
					pi.style.backgroundColor = '#DDDDDD';
				} catch (e) {}
			}
		
			function setSplitQty(rowNum1) {
				var pi = getSplitQtyInput(rowNum1);
				try {
					pi.value = 0;
					pi.disabled = false;
					pi.style.backgroundColor = '';
				} catch (e) {}
				pi = getSplitLocInput(rowNum1);
				try {
					pi.value = '';
					pi.disabled = false;
					pi.style.backgroundColor = '';
				} catch (e) {}
				pi = getSplitLoadInput(rowNum1);
				try {
					pi.value = '';
					pi.disabled = false;
					pi.style.backgroundColor = '';
				} catch (e) {}
			}
		
			function getSplitQtyInput(rowNum1) {
				return document.getElementById(SplitPrefix + rowNum1 + INPSuffixQty);
			}
			function getSplitLocInput(rowNum1) {
				return document.getElementById(SplitPrefix + rowNum1 + INPSuffixLoc);
			}
			function getSplitLoadInput(rowNum1) {
				return document.getElementById(SplitPrefix + rowNum1 + INPSuffixLoad);
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
			
			var Prefix = 'TEINVLOADADJ_TableEditorGridTEINVLOADADJ_Grid__ctl';
			var CBSuffix = '_multi_select';
			var INPSuffix = '_DisplayTypeCtrl_TOQTY_tb';
			var SplitPrefix = 'TESPLITLOAD_TableEditorGridTESPLITLOAD_Grid__ctl';
			var INPSuffixQty = '_DisplayTypeCtrl_TOQTY_tb';
			var INPSuffixLoad = '_DisplayTypeCtrl_TOLOADID_tb';
			var INPSuffixLoc = '_DisplayTypeCtrl_TOLOCATION_tb';
			
			try {
				if(document.getElementsByName("pnlSplitLoad").length == 1){
					var cbsSplit = GridMultiselectCheckboxes_TESPLITLOAD_TableEditorGridTESPLITLOAD;
					for (n=0; n<=cbsSplit.length-1; n++) {
						var cb1 = document.getElementById(cbsSplit[n])
						if (!cb1.checked)
						{
							var rowNum1 = getSplitRowNum(cb1)
							var inp = getSplitQtyInput(rowNum1)
							inp.disabled = true;
							inp.style.backgroundColor = '#DDDDDD';
							inp = getSplitLocInput(rowNum1)
							inp.disabled = true;
							inp.style.backgroundColor = '#DDDDDD';
							inp = getSplitLoadInput(rowNum1)
							inp.disabled = true;
							inp.style.backgroundColor = '#DDDDDD';
						}
						cb1.onclick = applySplitQty;
					}
				}
				else if(document.getElementsByName("pnlAddSubQty").length ==1){
					var cbs = GridMultiselectCheckboxes_TEINVLOADADJ_TableEditorGridTEINVLOADADJ;
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
				}
			} catch (e) {}
	
		</script>--%>
	</body>
</HTML>
