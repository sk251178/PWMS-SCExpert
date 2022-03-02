<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Networks.aspx.vb" Inherits="WMS.WebApp.Networks" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Networks</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			&nbsp;
		</form>
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
