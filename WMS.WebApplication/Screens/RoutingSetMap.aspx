<%@ Page Language="vb" AutoEventWireup="false" Codebehind="RoutingSetMap.aspx.vb" Inherits="WMS.WebApp.RoutingSetMap" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>RoutingSetMap</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<cc3:screen id="Screen1" runat="server" ScreenID="rsm" NoLoginRequired="True"></cc3:screen>
			<table cellpadding="0" cellspacing="0">
				<TBODY>
					<tr>
						<td width="70%" height="100%" style="BORDER-TOP: black 1px solid;BORDER-BOTTOM: black 1px solid;BORDER-LEFT: black 1px solid;BORDER-RIGHT: black 1px solid">
							<IFRAME id="frmRoutingSetDetails" name="frmRoutingSetDetails" src="RoutingSets.aspx" frameBorder="no"
								width="100%" height="100%"></IFRAME>
						</td>
						<td width="30%">
							<OBJECT id="mapcontrol" style="WIDTH: 500px; HEIGHT: 483px" codeBase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,0,0"
								aheight="524" awidth="512" classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" VIEWASTEXT>
								<PARAM NAME="_cx" VALUE="13547">
								<PARAM NAME="_cy" VALUE="13864">
								<PARAM NAME="FlashVars" VALUE="">
								<PARAM NAME="Movie" VALUE="Black.swf">
								<PARAM NAME="Src" VALUE="Black.swf">
								<PARAM NAME="WMode" VALUE="Window">
								<PARAM NAME="Play" VALUE="-1">
								<PARAM NAME="Loop" VALUE="-1">
								<PARAM NAME="Quality" VALUE="High">
								<PARAM NAME="SAlign" VALUE="LT">
								<PARAM NAME="Menu" VALUE="-1">
								<PARAM NAME="Base" VALUE="">
								<PARAM NAME="AllowScriptAccess" VALUE="always">
								<PARAM NAME="Scale" VALUE="NoScale">
								<PARAM NAME="DeviceFont" VALUE="0">
								<PARAM NAME="EmbedMovie" VALUE="0">
								<PARAM NAME="BGColor" VALUE="000000">
								<PARAM NAME="SWRemote" VALUE="">
								<PARAM NAME="MovieData" VALUE="">
								<PARAM NAME="SeamlessTabbing" VALUE="1">
								<EMBED NAME="mapcontrol" src="Black.swf" quality="high" bgcolor="#000000" WIDTH="500" HEIGHT="483"
									NAME="Black" ALIGN="" TYPE="application/x-shockwave-flash" swLiveConnect="true" PLUGINSPAGE="http://www.macromedia.com/go/getflashplayer">
								</EMBED>
							</OBJECT>
						</td>
					</tr>
				</TBODY>
			</table>
			<input id="SetId" type="hidden"><input id="btnAct" style="DISPLAY: none" onclick="doAction()" type="button">
		</form>
		<SCRIPT language="JavaScript">
<!--
		var InternetExplorer = navigator.appName.indexOf("Microsoft") != -1;
		function mapcontrol_DoFSCommand(command, args)                
		{
			//alert(command);
			if (command=="point_selected")
			{
				window.parent.frames(0).document.getElementById("hdValCommand").value = "pin";
				window.parent.frames(0).document.getElementById("hdPointId").value = args;
			}
		}
		
		function ClearEntireMap()
		{
			document.getElementById("mapcontrol").SetVariable("/Map:addPoints","-1");
			document.getElementById("mapcontrol").SetVariable("/Map:addNetItems","-1");
		}
		
		function bodyonload()
		{
			ClearEntireMap();
		}
		
		function doAction()
		{
			switch (command.value)
			{
				case "showpoints":
					addpoint();
					break;
				case "gotopoint":
					locatePointById();
					break;
				case "clearmap":
					ClearEntireMap();
					break;
				case "pointitem":
					SendSelectedItem();
					break;
			}
		}
		
		function locatePointById()
		{
			document.getElementById("mapcontrol").SetVariable("/Map:locatePosId","" + args.value);
			document.getElementById("mapcontrol").SetVariable("/Map:centerMap","" + Math.random());
		}
		function SendSelectedItem()
		{
		alert(args);
			document.getElementById("mapcontrol").SetVariable("/Map:selectedMapItem","" + args.value);
		}
		function addpoint()
		{
			document.getElementById("mapcontrol").SetVariable("/Map:addPoints","" + args.value);
		}
		
		function clearMap()
		{
			document.getElementById("mapcontrol").SetVariable("/Map:addPoints","" + -1);
		}
		
		function ClearNetworkFromMap()
		{
			document.getElementById("mapcontrol").SetVariable("/Map:addNetItems","" + -1);
		}
	
		if (navigator.appName && navigator.appName.indexOf("Microsoft") != -1 && navigator.userAgent.indexOf("Windows") != -1 && navigator.userAgent.indexOf("Windows 3.1") == -1) 
		{
			document.write('<SCRIPT LANGUAGE=VBScript\> \n');
			document.write('on error resume next \n');
			document.write('Sub mapcontrol_FSCommand(ByVal command, ByVal args)\n');
			document.write(' call mapcontrol_DoFSCommand(command, args)\n');
			document.write('end sub\n');
			document.write('</SCRIPT\> \n');
		} 
		
//-->
		</SCRIPT>
	</body>
</HTML>
