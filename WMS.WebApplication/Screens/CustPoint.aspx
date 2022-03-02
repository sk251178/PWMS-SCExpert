<%@ Page Language="vb" AutoEventWireup="false" Codebehind="CustPoint.aspx.vb" Inherits="WMS.WebApp.CustPoint" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Map</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<!-- #include file="~/include/head.html" -->
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout">
		<!-- #include file="~/include/Header.html" -->
		<input id="args" type="hidden">
		<SCRIPT language="JavaScript">
<!--
		var InternetExplorer = navigator.appName.indexOf("Microsoft") != -1;
		function mapcontrol_DoFSCommand(command, args)                
		{
			alert(args);
			window.parent.frames(0).document.getElementById("hdPointId").value = args;
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
			alert(args.value);
			alert(command.value);
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
			}
		}
		var mode = 0;
		
		function setMode()
		{
			mode++;
			if (mode==5)
				mode = 0;
			document.getElementById("mapcontrol").SetVariable("/Map:mapMode",mode);
		}
		
		function locatePointById()
		{
			document.getElementById("mapcontrol").SetVariable("/Map:locatePosId","" + args.value);
			document.getElementById("mapcontrol").SetVariable("/Map:centerMap","" + Math.random());
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
		<form id="Form1" method="post" runat="server">
            <telerik:RadScriptManager ID="RadScriptManager2" runat="server">     </telerik:RadScriptManager> 
			<cc3:screen id="Screen1" runat="server" ScreenID="cp"></cc3:screen>
			<table style="BORDER-RIGHT: black 1px solid; BORDER-TOP: black 1px solid; BORDER-LEFT: black 1px solid; BORDER-BOTTOM: black 1px solid"
				cellpadding="0" cellspacing="0">
				<TBODY>
					<tr>
						<td>
							<OBJECT id="mapcontrol" codeBase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,0,0"
								height="460" width="705" classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" VIEWASTEXT>
								<PARAM NAME="_cx" VALUE="18653">
								<PARAM NAME="_cy" VALUE="12171">
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
						<td style="BORDER-RIGHT: black 1px solid" width="100%" height="100%"><iframe src="CustData.aspx" width="100%" height="100%" frameborder="no" id="frmCompData"
								name="frmCompData"></iframe>
						</td>
					</tr>
					<tr>
						<td colspan="2" style="BORDER-TOP: black 1px solid"><iframe src="CompanyManagement.aspx" width="100%" height="100%" frameborder="no" id="frmCompanies"
								name="frmCompanies"></iframe>
						</td>
					</tr>
				</TBODY>
			</table>
		</form>
		<input id="command" type="hidden"> <input id="btnAct" style="DISPLAY: none" onclick="doAction()" type="button">
	</body>
</HTML>
