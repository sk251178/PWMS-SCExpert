<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Route.aspx.vb" Inherits="WMS.WebApp.Route" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Routes</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<cc3:screen id="Screen1" runat="server" HideMenu="True" HideBanner="True" Hidden="True" NoLoginRequired="True"></cc3:screen><br>
			<cc2:tableeditor id="TERouteList" runat="server" SearchButtonPos="NextToSearchForm" SearchDT="DTRouteListSearch"
				ForbidRequiredFieldsInSearchMode="True" DefaultDT="DTRouteList" DefaultMode="Search" DESIGNTIMEDRAGDROP="60"
				DisableSwitches="rmneiv" EnableCSVExport="Global" EnableQuickChart="Global" EnableQuickReport="Global"
				PersistSearchValues="Off" EnableSavedSearch="Off" EnableModifySavedSearch="On"></cc2:tableeditor></form>
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
		</SCRIPT>
	</body>
</HTML>
