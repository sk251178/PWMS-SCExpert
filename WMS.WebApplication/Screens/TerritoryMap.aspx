<%@ Page Language="vb" AutoEventWireup="false" Codebehind="TerritoryMap.aspx.vb" Inherits="WMS.WebApp.TerritoryMap" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Routes Map</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout">
		<input id="args" type="hidden">
		<SCRIPT language="JavaScript">
<!--
		var InternetExplorer = navigator.appName.indexOf("Microsoft") != -1;
		function mapcontrol_DoFSCommand(command, args)                
		{
			if (command=="point_selected")
			{
				window.parent.frames(0).document.getElementById("hdValCommand").value = "pin";
				window.parent.frames(0).document.getElementById("hdPointId").value = args;
			} else if (command=="map_ready")
			{
			   //alert("mapready");
			   document.getElementById("mapcontrol").SetVariable("/Map:territoryId","1");
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
			//alert(command.value);
			switch (command.value)
			{
				case "showroute":
					addroute();
					break;
				case "gotopoint":
					locatePointById();
					break;
				case "clearmap":
					ClearEntireMap();
					break;
			}
		}
		
		function locatePointById()
		{
			document.getElementById("mapcontrol").SetVariable("/Map:locatePosId","" + args.value);
			document.getElementById("mapcontrol").SetVariable("/Map:centerMap","" + Math.random());
		}
		
		function addroute()
		{
			document.getElementById("mapcontrol").SetVariable("/Map:addRoutes", args.value); //args.value);
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
		<form id="Form1" method="post" runat="server">
			<cc3:screen id="Screen1" runat="server" ScreenID="rm"></cc3:screen>
			<table cellpadding="0" cellspacing="0">
				<TBODY>
					<tr>
						<td width="70%" style="BORDER-TOP: black 1px solid;BORDER-BOTTOM: black 1px solid;BORDER-LEFT: black 1px solid;BORDER-RIGHT: black 1px solid"><iframe src="Territory.aspx" width="100%" height="100%" frameborder="no" id="frmPinGrid" name="frmPinGrid"
								style="WIDTH: 100%; HEIGHT: 287.94%"></iframe>
						</td>
						<td width="30%">
							<OBJECT id="mapcontrol" style="WIDTH: 500px; HEIGHT: 483px" codeBase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,0,0"
								classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" VIEWASTEXT>
								<PARAM NAME="_cx" VALUE="13229">
								<PARAM NAME="_cy" VALUE="12779">
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
						<!--<td style="BORDER-RIGHT: black 1px solid" width="100%" height="100%"><iframe src="PinView.aspx" width="100%" height="100%" frameborder="no" id="frmViewData"
								name="frmViewData"></iframe>
						</td> -->
						<!--/tr>
					<tr height="300" width="100%"-->
					</tr>
				</TBODY>
			</table>
		</form>
		<input id="command" type="hidden"><input id="btnAct" style="DISPLAY: none" onclick="doAction()" type="button">
	</body>
</HTML>
