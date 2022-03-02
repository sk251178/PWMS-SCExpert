<%@ Page Language="vb" AutoEventWireup="false" Codebehind="PinGrid.aspx.vb" Inherits="WMS.WebApp.PinGrid" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Pin Grid</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<cc3:screen id="Screen1" runat="server" HideMenu="True" HideBanner="True" Hidden="False" NoLoginRequired="True"></cc3:screen><br>
			<!--table>
				<tr>
					<td><cc2:label id="lblSelectObject" runat="server">lblSelectObject</cc2:label></td>
					<td>&nbsp;</td>
					<td><cc2:dropdownlist id="ddInvAct" runat="server" ForeColor="Transparent" AutoPostBack="True" Where="CODELISTCODE = 'MAPOBJTYPE'"
							ValueField="CODE" TextField="DESCRIPTION" TableName="CODELISTDETAIL"></cc2:dropdownlist></td>
				</tr>
			</table--><cc2:datatabcontrol id="DTC" runat="server" SyncEditMode="False">
				<TABLE id="Table1" cellSpacing="0" cellPadding="0" border="0" runat="server">
					<TR>
						<TD>
							<cc2:tabstrip id="TS" runat="server" TabDefaultStyle="background-color:#DDDDDD;border:solid 1px gray;padding-left:5px;padding-right:5px;font-size:13px;font-weight:bold;padding-top:2px;padding-bottom:2px;"
								TabHoverStyle="background-color:#DDDDDD;" TabSelectedStyle="border:solid 1px gray;border-bottom:none;background:white;"
								SepDefaultStyle="border-bottom:solid 1px gray;" TargetTableID="Tbl" AutoPostBack="True">
								<iewc:Tab Text="Companies"></iewc:Tab>
								<iewc:TabSeparator></iewc:TabSeparator>
								<iewc:Tab Text="Drivers"></iewc:Tab>
								<iewc:TabSeparator></iewc:TabSeparator>
								<iewc:Tab Text="Depots"></iewc:Tab>
							</cc2:tabstrip></TD>
					</TR>
					<TR>
						<TD style="BORDER-RIGHT: gray 1px solid; BORDER-LEFT: gray 1px solid; BORDER-BOTTOM: gray 1px solid; HEIGHT: 18px">
							<TABLE id="Tbl" cellSpacing="0" cellPadding="5" border="0" runat="server">
								<TR>
									<TD>
										<cc2:TableEditor id="TECompanies" runat="server" oDisableSwitches="trmie" DefaultDT="DTCompMap" 
											GridDT="DTCompMap" DisableSwitches="eidrnv" DefaultMode="Grid"></cc2:TableEditor></TD>
									<TD>
										<cc2:tableeditor id="TEDrivers" runat="server" oDisableSwitches="rmneiv" DefaultDT="DTDriverMap"
											GridPageSize="5" DisableSwitches="eidrnv" DefaultMode="Grid" DESIGNTIMEDRAGDROP="60"></cc2:tableeditor></TD>
									<TD>
										<cc2:TableEditor id="TEDepots" runat="server" DefaultDT="DTDepotMap" GridPageSize="5" DisableSwitches="eidrnv"
											DefaultMode="Grid" ObjectID="3"></cc2:TableEditor></TD>
									<TD></TD>
								</TR>
							</TABLE>
						</TD>
					</TR>
				</TABLE>
			</cc2:datatabcontrol></form>
		<SCRIPT language="JavaScript">
		
		var Prefix = 'TEPinGrid_TableEditorGridTEPinGrid_Grid__ctl';
		var CBSuffix = '_multi_select';
		var PointIdSuffix = '_DisplayTypeCtrl_POINTID';
		
		function getSelectedPoints(getFirst)
		{
			var SelectedPoints = "";
			var cbs = GridMultiselectCheckboxes_TEPinGrid_TableEditorGridTEPinGrid;
			for (n=0; n<=cbs.length-1; n++) {
				var cb = document.getElementById(cbs[n]);
				if (cb.checked)
				{
					var val = getPoint(cb);
					if (val != "")
					{
						SelectedPoints += val + ",";
						if (getFirst == true)
							return TrimEnd(SelectedPoints,',');
					}
				}
			}
			return TrimEnd(SelectedPoints,',');
		}
		
		function TrimEnd(str,ch)
		{
			if (ch == null || str == null || str == "")
				return "";
			for (var i=str.length-1; str.charAt(i)<=ch; i--);
			return str.substring(0,i+1);
		}

		function getRowNum(cb) 
		{
			var strRowNum = cb.id.substring(Prefix.length,cb.id.length - CBSuffix.length);
			return parseInt(strRowNum);
		}
		
		function getPoint(cb) 
		{
			var rowNum = getRowNum(cb);
			var span = document.getElementById(Prefix + rowNum + PointIdSuffix);
			try {
				var pt = span.innerHTML;
				return pt;
			}
			catch (e){return "";}
		}
		
		function postToMap()
		{
			
		}
		function ShowPoint()
		{
			var sel = getSelectedPoints(false);
			if (sel != "")
			{
				window.parent.args.value=sel;
				window.parent.command.value="showpoints";
				window.parent.btnAct.click()
			}
		}
		
		function Clear()
		{
			window.parent.args.value="";
			window.parent.command.value="clearmap";
			window.parent.btnAct.click()
		}
		//function bodyonload()
		//{
		//alert("start");
		// var objects=getSelectedPoints(true);
		// alert(objects);
		// if (objects!="") {
		// 
		//		window.parent.args.value=objects;
		//		window.parent.command.value="pointitem";
		//		window.parent.btnAct.click()
		// }
		// }
		</SCRIPT>
		</CC3:DROPDOWNLIST>
	</body>
</HTML>
