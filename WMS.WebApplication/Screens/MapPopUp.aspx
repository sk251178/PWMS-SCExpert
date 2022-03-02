<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="MapPopUp.aspx.vb" Inherits="WMS.WebApp.MapPopUp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>MapPopUp</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<cc3:Screen id="Screen1" runat="server"></cc3:Screen>
			<SCRIPT LANGUAGE="JavaScript">
<!--
var InternetExplorer = navigator.appName.indexOf("Microsoft") != -1;

	function mapcontrol_DoFSCommand(command, args)                
	{
		//document.getElementById('mapaction').innerText = command + " " +args;
		if (command == 'networkitem_selected')
			window.open("ShowPoint.aspx?action=showroad&fpoint=" + args.split(',')[0] + "&spoint=" + args.split(',')[1],"MapPointInfo","fullscreen=no;toolbar=no;resizable=no;menubar=no;location=no",false);
		else
			window.open("ShowPoint.aspx?action=showpoing&fpoint=" + args,"MapPointInfo","fullscreen=no;toolbar=no;resizable=no;menubar=no;location=no",true);		
	}
	
	if (navigator.appName && navigator.appName.indexOf("Microsoft") != -1 && 
		navigator.userAgent.indexOf("Windows") != -1 && navigator.userAgent.indexOf("Windows 3.1") == -1) {
		document.write('<SCRIPT LANGUAGE=VBScript\> \n');
		document.write('on error resume next \n');
		document.write('Sub mapcontrol_FSCommand(ByVal command, ByVal args)\n');
		document.write(' call mapcontrol_DoFSCommand(command, args)\n');
		document.write('end sub\n');
		document.write('</SCRIPT\> \n');
	} 
//-->
			</SCRIPT>
			<table align="center">
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
				<PARAM NAME="SAlign" VALUE="">
				<PARAM NAME="Menu" VALUE="-1">
				<PARAM NAME="Base" VALUE="">
				<PARAM NAME="AllowScriptAccess" VALUE="always">
				<PARAM NAME="Scale" VALUE="ShowAll">
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
			</td></tr></table>
		</form>
	</body>
</HTML>
