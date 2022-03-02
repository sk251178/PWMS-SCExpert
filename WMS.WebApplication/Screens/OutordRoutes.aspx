<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="OutordRoutes.aspx.vb" Inherits="WMS.WebApp.OutordRoutes" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Routing Orders</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<!-- #include file="~/include/head.html" -->
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout">
		<!-- #include file="~/include/Header.html" -->
		<FORM id="Form1" method="post" runat="server">
            <telerik:RadScriptManager ID="RadScriptManager2" runat="server">     </telerik:RadScriptManager> 
			<cc3:Screen id="Screen1" runat="server" HideBanner="False" HideMenu="False"></cc3:Screen>
			<br>
			<SCRIPT LANGUAGE="JavaScript">
<!--
var InternetExplorer = navigator.appName.indexOf("Microsoft") != -1;
var mpType = 0;

	function mapcontrol_DoFSCommand(command, args)                
	{
		//if (command == 'networkitem_selected')
		//	window.open("ShowPoint.aspx?action=showroad&fpoint=" + args.split(',')[0] + "&spoint=" + args.split(',')[1],"MapPointInfo","fullscreen=no;toolbar=no;resizable=no;menubar=no;location=no",false);
		//else
		//	window.open("ShowPoint.aspx?action=showpoing&fpoint=" + args,"MapPointInfo","fullscreen=no;toolbar=no;resizable=no;menubar=no;location=no",true);
	}
	
	function ChangeMode()
	{
		mpType++;
		if (mpType == 5)
			mpType = 0;
		alert(mpType);
		document.Form1.mapcontrol.SetVariable("/Map:mapMode",mpType);
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
			<table border="0" width="100%">
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
					</td>
				</tr>
			</table>
		</FORM>
		<input type="button" onclick="ChangeMode()">
	</body>
</HTML>
