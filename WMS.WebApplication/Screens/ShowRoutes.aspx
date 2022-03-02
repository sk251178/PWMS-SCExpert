<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ShowRoutes.aspx.vb" Inherits="WMS.WebApp.ShowRoutes" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Show Routes</title>
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
			//alert(command);
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
				case "showRoute":
					ShowRoute();
					break;
			}
		}
		var mode = 0;
		
		function ShowRoute()
		{
			//ClearEntireMap();
			document.getElementById("mapcontrol").SetVariable("/Map:addPoints","0");
			//document.getElementById("mapcontrol").SetVariable("/Map:addRoutes","1");
			//document.getElementById("mapcontrol").SetVariable("/Map:addRoutes","" + args.value);
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
			<cc3:screen id="Screen1" runat="server" ScreenID="sr" NoLoginRequired="false"></cc3:screen>
			<table style="BORDER-RIGHT: black 1px solid; BORDER-TOP: black 1px solid; BORDER-LEFT: black 1px solid; BORDER-BOTTOM: black 1px solid"
				cellpadding="0" cellspacing="0">
				<TBODY>
					<tr>
						<td>
							<OBJECT id="mapcontrol" codeBase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,0,0"
								height="340" width="980" classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" VIEWASTEXT>
								<PARAM NAME="_cx" VALUE="25929">
								<PARAM NAME="_cy" VALUE="8996">
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
								<EMBED NAME="mapcontrol" src="Black.swf" quality="high" bgcolor="#000000" WIDTH="100%"
									HEIGHT="100%" NAME="Black" ALIGN="" TYPE="application/x-shockwave-flash" swLiveConnect="true"
									PLUGINSPAGE="http://www.macromedia.com/go/getflashplayer"> </EMBED>
							</OBJECT>
						</td>
					</tr>
					<tr>
						<td height="270" style="BORDER-TOP: black 1px solid"><iframe src="ShowRouteList.aspx" width="100%" height="100%" frameborder="no" id="frmRouteList" name="frmRouteList"></iframe>
						</td>
					</tr>
				</TBODY>
			</table>
		</form>
		<input id="command" type="hidden"> <input id="btnAct" style="DISPLAY: none" onclick="doAction()" type="button">
	</body>
</HTML>
