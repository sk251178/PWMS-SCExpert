<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="MapScreen.aspx.vb" Inherits="WMS.WebApp.MapScreen" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >

<script language="javascript" type="text/javascript">
	function mapcontrol_DoFSCommand(command, args)                
	{
		if (command == "map_ready") {
			//addPointsToMap("mapcontrol",0);
			addNetworkToMap("mapcontrol",0);
		}
		//document.getElementById('mapaction').innerText = command + " " +args;
		// here we have to do some smart stuff depending on the action passed by flash
	}

	var InternetExplorer = navigator.appName.indexOf("Microsoft") != -1;

	if (navigator.appName && navigator.appName.indexOf("Microsoft") != -1 && 
		navigator.userAgent.indexOf("Windows") != -1 && navigator.userAgent.indexOf("Windows 3.1") == -1) {
		document.write('<SCRIPT LANGUAGE=VBScript\> \n');
		document.write('on error resume next \n');
		document.write('Sub mapcontrol_FSCommand(ByVal command, ByVal args)\n');
		document.write(' call mapcontrol_DoFSCommand(command, args)\n');
		document.write('end sub\n');
		document.write('</SCRIPT\> \n');
	} 
	
// idmap - the ID of the Flash map control in the HTML page
// pointids - comma delimited list of point ids "2315,12211,676675"
function addPointsToMap(idmap, pointids)
{
	document.getElementById(idmap).SetVariable("/Map:addPoints",""+pointids);
}
// idmap - the ID of the Flash map control in the HTML page
// netids - comma delimited list of point@nextpoint ids "2315@12211,676675@4517,3412@789"
function addNetworkToMap(idmap, netids)
{
	document.getElementById(idmap).SetVariable("/Map:addNetItems",""+netids);
}

// idmap - the ID of the Flash map control in the HTML page
// longitude - the longitude (in units as in the DB)
// latitude - the latitude (in units as in the DB)
function locatePointByPos(idmap, longitude, latitude)
{
	document.getElementById(idmap).SetVariable("/Map:locatePos","" + longitude + "," + latitude);
}

// idmap - the ID of the Flash map control in the HTML page
// pointid - the ID of the point to be located
function locatePointById(idmap, pointid)
{
	document.getElementById(idmap).SetVariable("/Map:locatePosId","" + pointid);
}
// idmap - the ID of the Flash map control in the HTML page
// centers the map on the currently selected object - ifthe object is newtwork item - centers
// it in the beginning. If there is no current selection - nothing is performed
function centerMap(idmap)
{
	document.getElementById(idmap).SetVariable("/Map:centerMap","" + Math.random());
}

// idmap - the ID of the Flash map control in the HTML page
// buttonName - the name of the button to be hidden - only "net" is supported now
function hideButton(idmap, buttonName)
{
	document.getElementById(idmap).SetVariable("/Map:hideButton",buttonName);
}

// idmap - the ID of the Flash map control in the HTML page
// buttonName - the name of the button to be shown - only "net" is supported now
function showButton(idmap, buttonName)
{
	document.getElementById(idmap).SetVariable("/Map:showButton",buttonName);
}

// sets the point type of the newly create points - in NetEdit mode when the user clicks on the map 
// a point is created - this is the default type of the point being created 
// idmap - the ID of the Flash map control in the HTML page
// pointtype  - as in the DB - "1" - for Service points, "2" - for net items (roads).
function setPointTypeOnCreate(idmap, pointtype)
{
	document.getElementById(idmap).SetVariable("/Map:pointtypecreate",pointtype);
}

// the territory with the ID passed goes in the "edit" state - means it can be editted
// if no such territory exists - nothing happens. 
// It means that if we want to create an new territory first an empty  one should be created
// (by MapTerritory.Create() function and the ID must be passed here 
function editTerritory(idmap, territoryId)
{
	document.getElementById(idmap).SetVariable("/Map:territoryId",territoryId);
}

// saves the currently editted territory (the id passed must match it).
// this means that before calling this function the editTerritory() should be called
// if not ot the id does not match - nothing happens
function saveTerritory(idmap, territoryId)
{
	document.getElementById(idmap).SetVariable("/Map:saveTerritory",territoryId);
}


</SCRIPT>

<HTML>
	<HEAD>
		<title>Map</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<cc3:screen id="Screen1" runat="server" ScreenID="cmap"></cc3:screen>
			<table style="width:99%;"><tr><td width="10">
			<OBJECT id="mapcontrol" codeBase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,0,0"
				height="400px" width="705px" classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" VIEWASTEXT>
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
				<PARAM NAME="Scale" VALUE="exactfit">
				<PARAM NAME="DeviceFont" VALUE="0">
				<PARAM NAME="EmbedMovie" VALUE="0">
				<PARAM NAME="BGColor" VALUE="000000">
				<PARAM NAME="SWRemote" VALUE="">
				<PARAM NAME="MovieData" VALUE="">
				<EMBED NAME="mapcontrol" src="Black.swf" quality="high" bgcolor="#000000" WIDTH="100%" 
					HEIGHT="100%" NAME="Black" ALIGN="" TYPE="application/x-shockwave-flash" swLiveConnect="true"
					PLUGINSPAGE="http://www.macromedia.com/go/getflashplayer"> </EMBED>
			</OBJECT>
			</td>
			<td>
				<iframe scrolling=no src="ShowPoint.aspx?fpoint=14" style="width:100%;height:400px;border:none"/></td>
			</tr></table>
		</form>						
	</body>
</HTML>


