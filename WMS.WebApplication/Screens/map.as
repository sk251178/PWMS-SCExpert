#initclip
function ESRI_MapClass() {
	//this.initialize();
}


// Allow ESRI_MapClass to inherit MovieClip properties
ESRI_MapClass.prototype = new MovieClip();

// initiate the clip with its internal clips;
ESRI_MapClass.prototype.initialize = function() {
		// set up the clip
	// create the MapImage clip
	this.createEmptyMovieClip("MapImage",1);
	this.MapImage.createEmptyMovieClip("MapClip",1);
	
	// create the ZoomBox clip
	this.attachMovie("ZoomBox","ZoomBox",3);
	//this.createEmptyMovieClip("ZoomBox",3);
	with (this.ZoomBox){
			lineStyle (0, 0xFF0000, 100);
			moveTo (0, 0);
			lineTo (100, 0);
			lineTo (100,100);
			lineTo (0, 100);
			lineTo (0, 0);
			endLine();
	}
	this.ZoomBox._visible = true;
	// create the Masks for the Image and ZoomBox
	this.createEmptyMovieClip("MaskClip",2);
	this.createEmptyMovieClip("MaskClip2",4);
	this.createEmptyMovieClip("MapBorder",5);
	
	with (this.MaskClip){
			beginFill (0x0000FF, 100);
			moveTo (0, 0);
			lineTo (this.imageWidth, 0);
			lineTo (this.imageWidth, this.imageHeight);
			lineTo (0, this.imageHeight);
			lineTo (0, 0);
			endFill();
	}
	with (this.MaskClip2){
			beginFill (0x0000FF, 100);
			moveTo (0, 0);
			lineTo (this.imageWidth, 0);
			lineTo (this.imageWidth, this.imageHeight);
			lineTo (0, this.imageHeight);
			lineTo (0, 0);
			endFill();
	}

	if (this.look3d) {
			// create 3D look border
		with (this.MapBorder){
				// shadow on left and top edges
				lineStyle (2, 0x000000, 100);
				moveTo (0, this.imageHeight);
				lineTo (0, 0);
				lineTo (this.imageWidth, 0);
				// highlight on right and bottom edges
				lineStyle (2, 0xD0D0D0, 100);
				lineTo (this.imageWidth, this.imageHeight);
				lineTo (0, this.imageHeight);
				endLine();
		}
	}

	this.MapImage.setMask(this.MaskClip);
	this.ZoomBox.setMask(this.MaskClip2);
	this.ZoomBox._visible=false;
	this.mapSource = "&host=" + this.host + "&serverobject=" + this.serverObject;
	if (this.dataFrame.length>0) this.mapSource += "&dataframe=" + this.dataFrame;
}

// Update draws clip at current base and height;
ESRI_MapClass.prototype.update = function() {
	// set up the clip
}


ESRI_MapClass.prototype.SetMapAction = function(m) {
	this.MapAction = m;
}

ESRI_MapClass.prototype.GetMapAction = function() {
	return this.MapAction;
}

ESRI_MapClass.prototype.SetEnvelope = function(xx1, yy1, xx2, yy2) {
	this.minx = xx1;
	this.miny = yy1;
	this.maxx = xx2;
	this.maxy = yy2;
}

// set up the drag image mode
ESRI_MapClass.prototype.MapDragImage = function(action, toolmode) {
	this.MapAction = action;
	this.ToolMode = toolmode;
	this.onPress = this.MapDragImageStart;
	this.onRelease = this.MapDragImageStop;
	this.onMouseMove = this.MapDragImageMove;
	this.onKeyUp = null;
}

ESRI_MapClass.prototype.MapTrackRectangle = function(action, toolmode) {
	this.MapAction = action;
	this.ToolMode = toolmode
	this.onPress = this.MapTrackRectangleStart;
	this.onRelease = this.MapTrackRectangleStop;
	this.onMouseMove = this.MapTrackRectangleMove;
	this.onKeyUp = null;
}

ESRI_MapClass.prototype.MapPoint = function(action) {
	this.MapAction = action;
	this.onPress = this.MapPointClick;
	this.onRelease = null;
	this.onMouseMove = null;
	this.onKeyUp = null;
}

ESRI_MapClass.prototype.MapNullAction = function() {
	this.MapAction = action;
	this.onPress = null;
	this.onRelease = null;
	this.onMouseMove = null;
	this.onKeyUp = null;
}


ESRI_MapClass.prototype.OVMapPoint = function(action) {
	this.MapAction = action;
	this.onPress = this.OVMapPointClick;
	this.onRelease = null;
	this.onMouseMove = null;
	this.onKeyUp = null;
}

// set up the drag image mode
ESRI_MapClass.prototype.OVDragZoomBox = function(action) {
	this.MapAction = action;
	this.onPress = this.OVDragZoomBoxStart;
	this.onRelease = this.OVDragZoomBoxStop;
//	this.onMouseMove = this.MapTrackRectangleMove;	
	this.onMouseMove = null;
	this.onKeyUp = null;
}


// set up the drag image mode
ESRI_MapClass.prototype.NetworkEditMode = function(action) {
	this.MapAction = action;
	this.onPress = this.NetEditPress;
	this.onRelease = this.NetEditRelease;
	this.onMouseMove = this.NetworkEditMove;	
	this.onKeyUp = this.NetKeyUp;
}


ESRI_MapClass.prototype.SetHost = function(h) {
	this.host = h;
}


// Connect the class with the linkage ID for this movie clip
Object.registerClass("Map", ESRI_MapClass);


#endinitclip

/********************************
// XML parsing functions
//********************************/
//   checks if a node of the specified nodeName exists in the child 
function hasNode(theNode, nodeName, recursiveSearch) {
  // check if there's at least one node with this name
  for(var iLoop=0; iLoop<theNode.childNodes.length; iLoop++) {
    aNode = theNode.childNodes[iLoop];
    if(aNode.nodeName.toLowerCase()==nodeName.toLowerCase()) {
      return true;
    } else if(aNode.hasChildNodes() && recursiveSearch) {
      if(hasNode(aNode, nodeName, true)) {
        // match found in children, return it
        return(true);
      }
    }

  }
  return false;
}


//   will search the XML node (and optionally all subnodes) for the
//   first node with a matching nodeName
function getNode(theNode, nodeName, recursiveSearch) {
  for( var iLoop=0; iLoop<theNode.childNodes.length; iLoop++) {
    aNode = theNode.childNodes[iLoop];
    if(aNode.nodeName.toLowerCase()==nodeName.toLowerCase()) {
      return (aNode);
    } else if(aNode.hasChildNodes() && recursiveSearch) {
      // re-use aNode here to store child search result
      // as it's current value is no longer needed
      aNode = getNode(aNode, nodeName, true);
      if(aNode!=null) {
        // match found in children, return it
        return(aNode);
      }
    }
  }
  // obviously didn't find a match... best to use hasNode first, then this one
  return(null);
}


x1 = 0;
y1 = 0;
x2 = 0;
y2 = 0;
minx = 0;
miny = 0;
maxx = 0;
maxy = 0;
startminx = 0;
starminy = 0;
starmaxx = 0;
starmaxy = 0;
lastMinX = 0;
lastMinY = 0;
lastMaxX = 0;
lastMaxY = 0;
sminx = 0;
sminy = 0;
smaxx = 0;
smaxy = 0;

// the network/stops data
netEditStartX=0;
netEditStartY=0;
netEditEndX=0;
netEditEndY=0;
selPoint=-1;
selNet=-1;
inTrack=false;

zleft = 0;
ztop = 0;
zright = 0;
ztop = 0;

mapX = 0;
mapY = 0;

devX=0;
devY=0;

xDistance = 1;
yDistance = 1;
xHalf = 0.5;
yHalf = 0.5;

panx = 0;
pany = 0;

mapURL;
ovURL;

boxOn = false;
panOn = false;

initialNotified=false;

if (baseURL==null) baseURL = "http://localhost/WMS2/Routing/map.aspx";
urlParams = "";
requestURL = "";
look3d = true;
imageWidth = 705;
imageHeight = 400;
mapType = "map";
mapIndex = 0;
mapToOvConnection = "";
ovToMapConnection = "";
mapToMapConnection = "";
dataDisplay = "";
tocConnection = "";
zoomFactorTool = "";

mapPoints = new Array(); // here we keep the points on the map
mapNetwork = new Array(); // here we keep the connections between the points
mapRoute = new Array(); // here we keep all the routes that have to displayed
mapPointTypes=new Array(); // here are kept all the types of mappoints (id, names and color);
mapTerritories = new Array(); // all the territories on the map - without the currently editted one
terBound= new Array();
terName=""; // the name ofthe territory

mapList = new Array();
mapExtents = new Array();
extentCount = 0;
currentExtent = -1;

clickTolerance = 2;
panFactor = 0.75;
host = "localhost";
serverObject = "world";
dataFrame = "";
mapSource = "";



locatePos="";
locatePosId="";
centerMap=false;
hideButton="";
showButton="";
netEditVisible=true;
addPoints="";
addNetItems="";
pointtypecreate = 4;
addRoutes="";
territoryId=0; // no ter
territoryName="";
saveTerritory=0;

thisClip=this;
Key.addListener(this);

function mapTerritorySave(prop, oldval, newval, userdata)
{
	if (territoryId != newval) // nothing to do if no ter
		return;
	var s="";
	var ind=ArrangeTerritoryBounds();
		
	var bx1=this.GetXCoordinate(this.terBound[ind[0]].longitude);
	var by1=this.GetYCoordinate(this.terBound[ind[0]].latitude);
	
	this.MapImage.roads.MoveTo(bx1,by1);	
	
	for (var i=0; i<ind.length; i++) {
		s+="$" + this.terBound[ind[i]].longitude + "," + this.terBound[ind[i]].latitude;
	}	
	
	this.sendUpdateRequest("save_territory",""+territoryId+" " +s);
	

	var pp=new Object();
	
	pp.id=territoryId;
	pp.name=territoryName;
	pp.pts=this.terBound;
	this.mapTerritories.push(pp);
	
	this.terBound=new Array();
	pointtypecreate = 1; // some default value - later can be cahnged from JS
	territoryId=0;
	territoryName="";
	
	return newval;
}

function mapTerritoryIdChange(prop, oldval, newval, userdata)
{
	//this.sendUpdateRequest("add_routes",""+newval);
	
	// set the type of the newly created points to be the TER BOUNDARY ones - 0 means new territory
	
	if (newval >= 0) {
		for (var i=0;i<this.mapTerritories.length;i++) {
			if (this.mapTerritories[i].id==newval) {
				this.pointtypecreate = 4;
				this.territoryId=newval;
				this.territoryName=this.mapTerritories[i].name;
				this.terBound=this.mapTerritories[i].pts;
				break;
			}
		}
		//this.sendUpdateRequest("load_territory",""+newval);
	} else {
		this.pointtypecreate = 1; // some default value - later can be cahnged from JS
		this.territoryId=0;
	}
		
	return newval;
}


function mapAddRoutes(prop, oldval, newval, userdata)
{
	this.sendUpdateRequest("add_routes",""+newval);
	return newval;
}

function netPointTypeCreate(prop, oldval, newval, userdata)
{
	this.pointtypecreate = newval;
	return newval;
}

function mapAddPoints(prop, oldval, newval, userdata)
{
	this.sendUpdateRequest("add_points",""+newval);
	return newval;
}
function mapAddNetItems(prop, oldval, newval, userdata)
{
	this.sendUpdateRequest("add_net_items",""+newval);
	return newval;
}

function mapLocatePos(prop, oldval, newval, userdata)
{
	this.sendUpdateRequest("locate_by_coord",""+newval);
	return newval;
}
function mapLocatePosId(prop, oldval, newval, userdata)
{
	this.sendUpdateRequest("locate_by_pointid",""+newval);
	return newval;
}
function mapCenter(prop, oldval, newval, userdata)
{
	if (this.selPoint!=-1) { // if we have point - use it
		this.sendUpdateRequest("locate_by_pointid",""+this.mapPoints[this.selPoint].id);
	} else if (this.selNet != -1) { // if we have net item - use its start
		this.sendUpdateRequest("locate_by_pointid",""+this.mapNetwork[this.selNet].pointid);
	}
	return newval;
}

function netHideButton(prop, oldval, newval, userdata)
{
	if (newval == "net") {
		_parent.NetBT._visible=false;
		if (_level0.ModeText != "Network/Stops") {
			_level0.Map.MapDragImage("ZoomToExtent","Pan");
			_level0.ModeText = "Pan Tool";
			_level0.ZoomInBT._alpha=65;
			_level0.ZoomOutBT._alpha=65;
			_level0.NetBT._alpha=65;				
		}
	} 	
	return newval;
}

function netShowButton(prop, oldval, newval, userdata)
{
	if (newval == "net") {
		_parent.NetBT._visible=true;
	}
	return newval;
		
}
/*
function mapModeChanged(prop, oldval, newval, userdata)
{
	//fscommand("networkitem_selected","mapmode has been changed");
	_parent.mapMode.selectedIndex=newval;
	
	var thisClip=this;
	
	if (thisClip.mapMode != newval) {
		thisClip.mapMode=newval;
		
		var hideNetBT=false;
		var askServer=false;
			
		if (newval == 0) { // None
			thisClip.MapImage.roads.clear();
			hideNetBT=true;
		} else if (newval == 1) { // Customers
			if (thisClip.MapPoints.length > 0) {
				thisClip.DrawNetwork();
			} else {
				askServer=true;
			}
		} else if (newval == 2) {
			if (thisClip.mapNetwork.length > 0) {
				thisClip.DrawNetwork();
			} else {
				askServer=true;
			}
		} else if (newval == 3) {
			thisClip.MapImage.roads.clear();			
			hideNetBT=true;
		} else if (newval == 4) {
			if ((thisClip.mapNetwork.length > 0) && (thisClip.MapPoints.length > 0)) {
				thisClip.DrawNetwork();
			} else {
				askServer=true;
			}
		}
		
		if (askServer) {
			thisClip.sendUpdateRequest("mapmodedata",""+newval);
		} 
		
		if (hideNetBT) {
			_parent.NetBT._visible=false;
			if (_level0.ModeText != "Network/Stops") {
				_level0.Map.MapDragImage("ZoomToExtent","Pan");
				_level0.ModeText = "Pan Tool";
				_level0.ZoomInBT._alpha=65;
				_level0.ZoomOutBT._alpha=65;
				_level0.NetBT._alpha=65;				
			}
		} else {
			_parent.NetBT._visible=true;
		}
	}
	return newval;
}
*/

// begin by reading in configuration
this.ReadConfig();
// hide the zoombox
this.ZoomBox._visible = false;

// read in configuration file and get parameters for this instance
function ReadConfig() {
	//trace(this.configFile);
	var varXML = new XML();
	varXML.onLoad = this.ProcessConfig;
	
	//varXML.load("black.xml"); 	
	//varXML.load(baseURL+"?requesttype=initialize&random=" + random(10000));
	varXML.load("map.aspx?requesttype=initialize&random=" + random(10000));
}

// process parameters
function ProcessConfig(success) {
	if (success) {
		var theXML = this;
		if (hasNode(theXML,"PARAMETERS",true)) {
			trace("Has PARAMETERS tag\n");
			oNode = getNode(theXML,"PARAMETERS",true);
			if (oNode.hasChildNodes) {
				//trace("has childNodes");
				var paramNodes = oNode.childNodes;
				var j = 0;
				for (var i = 0;i<paramNodes.length;i++) {
					if (paramNodes[i].nodeName == "REQUEST_URL") {
						// get request url
						thisClip.baseURL = paramNodes[i].attributes.path;
					}
					if (paramNodes[i].nodeName == "TITLE") {
						// set app title
						lcSend = new LocalConnection();
						lcSend.send("UpdateStatus", "SetTitle", paramNodes[i].attributes.value);
						lcSend.close();
						lcSend = null;

					}
					if (paramNodes[i].nodeName == "MAP") {
						// get params for this instance
						if (paramNodes[i].attributes.instance_name==thisClip._name) {
							trace("Found parameters for " + thisClip._name + "\n");
							iNode = paramNodes[i];
							if (iNode.attributes.host.length>0) thisClip.host = iNode.attributes.host;
							if (iNode.attributes.serverobject.length>0) thisClip.serverObject = iNode.attributes.serverobject;
							if (iNode.attributes.dataframe.length>0) thisClip.dataFrame = iNode.attributes.dataframe;
							if (iNode.attributes.map_index.length>0) thisClip.mapIndex = parseInt(iNode.attributes.map_index);
							if (iNode.attributes.map_type.length>0) thisClip.mapType = iNode.attributes.map_type;
							if (iNode.attributes.image_width.length>0) thisClip.imageWidth = parseInt(iNode.attributes.image_width);
							if (iNode.attributes.image_height.length>0) thisClip.imageHeight = parseInt(iNode.attributes.image_height);
							if (iNode.attributes.three_d_look.length>0) thisClip.look3d = (iNode.attributes.three_d_look.toLowerCase()=="true") ? true : false;
							if (iNode.attributes.unique_url.length>0) thisClip.requestURL = iNode.attributes.unique_url;
							if (iNode.attributes.map_to_overview_connection.length>0) thisClip.mapToOvConnection = iNode.attributes.map_to_overview_connection;
							if (iNode.attributes.map_to_map_connection.length>0) thisClip.mapToMapConnection = iNode.attributes.map_to_map_connection;
							if (iNode.attributes.overview_to_map_connection.length>0) thisClip.ovToMapConnection = iNode.attributes.overview_to_map_connection;
							if (iNode.attributes.toc_connection.length>0) thisClip.tocConnection = iNode.attributes.toc_connection;
							if (iNode.attributes.zoom_factor_tool.length>0) thisClip.zoomFactorTool = iNode.attributes.zoom_factor_tool;
							if (iNode.attributes.data_display.length>0) thisClip.dataDisplay = iNode.attributes.data_display;
							trace("Clip=" + thisClip._name +"\nmapIndex=" + thisClip.mapIndex + "\nmapType=" + thisClip.mapType + "\n");
						}
					}
					if (paramNodes[i].nodeName == "TOLERANCE") {
						// get tolerance value
						if (paramNodes[i].attributes.value.length>0) thisClip.clickTolerance = parseInt(paramNodes[i].attributes.value);
						trace("Tolerance = " + thisClip.clickTolerance + "\n");
					}
					if (paramNodes[i].nodeName == "pointtypes") {
						if (paramNodes[i].hasChildNodes) {
							//trace("has childNodes");
							var cnmp = paramNodes[i].childNodes;
							for (var l = 0;l<cnmp.length;l++) {
								if (cnmp[l].nodeName == "pointtype") {
									var pp = new Object();
									pp.id=parseInt(cnmp[l].attributes.id);
									pp.name=cnmp[l].attributes.name;
									pp.color=parseInt(cnmp[l].attributes.color);
									thisClip.mapPointTypes.push(pp);
								}
							}
						}
						
						
						
					}
				}
			}
		}
	} else {
		lcError = new LocalConnection();
		lcError.send("UpdateStatus","Status","Server Error.... Try again later.");
	}
	thisClip.initialize();
	//thisClip.GetMapInfo();
	// let zoomfactor clip know who this is
	if (thisClip.zoomFactorTool!="") {
		var zfName = "ZoomFactor_Command";
		lcSend = new LocalConnection();
		lcSend.send(zfName, "SetMapName", thisClip._name);
		lcSend.close();
		lcSend = null;
	}
	if (thisClip.mapType=="overview") {
		thisClip.OVDragZoomBox("ZoomIn");
		thisClip.initialNotified=false;
	} else {
		thisClip.initialNotified=true;
		thisClip.watch("locatePos",mapLocatePos, thisClip);
		thisClip.watch("locatePosId",mapLocatePosId, thisClip);
		thisClip.watch("centerMap",mapCenter, thisClip);
		thisClip.watch("addPoints",mapAddPoints, thisClip);
		thisClip.watch("addNetItems",mapAddNetItems, thisClip);
		thisClip.watch("addRoutes",mapAddRoutes, thisClip);
		
		thisClip.watch("territoryId",mapTerritoryIdChange, thisClip);
		thisClip.watch("saveTerritory",mapTerritorySave, thisClip);
		
		
		thisClip.watch("hideButton",netHideButton, thisClip);
		thisClip.watch("showButton",netShowButton, thisClip);
		thisClip.watch("pointtypecreate",netPointTypeCreate, thisClip);
		
		thisClip.imageWidth = Stage.width;
		thisClip.imageHeight = Stage.height;
		
		thisClip.FullExtent();		
		thisClip.MapTrackRectangle("ZoomToExtent","ZoomIn");
	}


}


function MapDragImageStart() {
	if (!this.panOn) {
		this.x1 = this._xmouse;
		this.y1 = this._ymouse;
		this.x2 = this.x1 + 0.01;
		this.y2 = this.y1 + 0.01;
		this.panOn = true;
		this.HideTooltip();
		startDrag ("MapImage");
	}
}

function MapDragImageStop() {
	if (this.panOn) {
		this.x2 = this._xmouse;
		this.y2 = this._ymouse;
		this.panOn = false;
		stopDrag ();
		this.panX = this.x2 - this.x1;
		this.panY = this.y2 - this.y1;
		
		if (Math.abs(this.panX) < 1 && Math.abs(this.panY) < 1) { // do nothing if we have not moved even a pixel
			this.SelectItem();
			this.NotifySelection();
			return;
		}
		
		var xincre = this.xDistance / this.imageWidth;
		var yincre = this.yDistance / this.imageHeight;
		var xmove = xincre * this.panX;
		var ymove = yincre * this.panY;
		this.minx = this.minx - xmove;
		this.maxx = this.maxx - xmove;
		this.miny = this.miny - ymove;
		this.maxy = this.maxy - ymove;
				
//		trace("MapDragImageStop: " + x1 + "," + y1 + " ---> " + x2 + "," + y2 + "\n    New Extent: " + minx + ", " + miny + ", " + maxx + ", " + maxy + "\n");
		this.realZoom=false;
		this.setMapAction();
	}
	
}

function MapDragImageMove() {
	this.x2 = this._xmouse;
	this.y2 =  this._ymouse;
	this.panX = this.x2 - this.x1;
	this.panY = this.y2 - this.y1;
	
	DisplayCoordinates(this._xmouse,this._ymouse);
	if (!this.panOn) {
		this.AdjustTooltip(this._xmouse,this._ymouse);
	}
}

function MapTrackRectangleStart() {
	if (!this.boxOn) {
		this.HideTooltip();
		this.x1 = this._xmouse;
		this.y1 = this._ymouse;
		this.x2 = this.x1 + 0.01;
		this.y2 = this.y1 + 0.01;
		this.boxOn = true;
	}	
	
}

function MapTrackRectangleStop() {
	if (this.boxOn) {
		this.x2 = this._xmouse;
		this.y2 = this._ymouse;
		this.boxOn = false;
		this.ZoomBox._visible = false;
//		trace("MapTrackRectangleStop: " + this.zleft + "," + this.ztop + " ---> " + this.zright + "," + this.zbottom); 
		var zWidth = Math.abs(this.x2-this.x1);
		var zHeight = Math.abs(this.y2-this.y1);
		
		
		if (zWidth < 2 && zHeight < 2) { // do nothing if we have not moved even a pixel
			this.SelectItem();
			this.NotifySelection();

			return;
		}
		
		
		var xincre = this.xDistance/this.imageWidth;
		var yincre = this.yDistance/this.imageHeight;
		var rx = xincre*this.x2+this.minx;
		var ty = this.miny+(yincre*this.y2);
		//var ty = this.maxy-(yincre*this.y2);
		if (this.ToolMode=="ZoomOut") {
			if ((zWidth>2) && (zHeight>2)) {
				var xRatio = this.imageWidth / zWidth;
				var yRatio = this.imageHeight / zHeight;

				var xAdd = xRatio * this.xDistance / 2;
				var yAdd = yRatio * this.yDistance / 2;
				this.minx = this.minx - xAdd;
				this.maxx = this.maxx + xAdd;
				this.maxy = this.maxy + yAdd;
				this.miny = this.miny - yAdd;
			} else {
				var rx = xincre * this.x2 + this.minx;
				var ty = this.maxy - (yincre * this.y2);
				this.minx = rx - this.xDistance;
				this.maxx = rx + this.xDistance;
				this.maxy = ty + this.yDistance;
				this.miny = ty - this.yDistance;
				
			}
			
		} else {
			if ((zWidth>2) && (zHeight>2)) {
				var left = this.minx;
				var top = this.miny;
				//var top = this.maxy;
				var xmin = (this.x1<this.x2) ? this.x1 : this.x2;
				var xmax = (this.x2>this.x1) ? this.x2 : this.x1;
				var ymin = (this.y1<this.y2) ? this.y1 : this.y2;
				var ymax = (this.y2>this.y1) ? this.y2 : this.y1;
				this.minx = (xincre*xmin)+left;
				this.maxx = (xincre*xmax)+left;
				this.miny = top+(yincre*ymin);
				this.maxy = top+(yincre*ymax);
			} else {
				var halfX = this.xDistance/4;
				var halfY = this.yDistance/4;
				this.minx = rx-halfX;
				this.miny = ty-halfY;
				this.maxx = rx+halfX;
				this.maxy = ty+halfY;
			}
		}
		this.realZoom=true;
		this.setMapAction();
	}
}

function formatCoordinate(x) {
	x=Math.floor(x/100);
	var deg=""+Math.floor(x/3600);
	var minutes=""+Math.floor(Math.floor(x%3600)/60);
	var seconds=""+Math.floor(Math.floor(x%3600)%60);
	if (deg.length == 1)
		deg="0"+deg;
	if (minutes.length == 1)
		minutes="0"+minutes;
	if (seconds.length==1)
		seconds="0"+seconds;

	tmp = "°"; 
	var ret=deg+tmp+minutes+"'" + seconds+"''";
	return ret;
}
function GetXCoordinate(longitude)
{
	var xincre = this.xDistance/this.imageWidth;
	longitude+=this.devX;
	
	if (Math.abs(xincre) > 0.0001)
		return ((longitude - this.minx)/xincre);
	else
		return -1;
		
}

function GetYCoordinate(latitude)
{
	var yincre = this.yDistance/this.imageHeight;	
	latitude+=this.devY;
	
	if (Math.abs(yincre) > 0.0001)
		return ((latitude-this.miny)/yincre);
	else
		return -1;
}


function GetCoordinateString(xx,yy)
{
	var ret="";
	if (yy < this.imageHeight && xx > 0  && yy > 0 && xx < this.imageWidth) {
	
		var bToolOff=true;
		var xincre = this.xDistance/this.imageWidth;
		var yincre = this.yDistance/this.imageHeight;	
		var x = this.minx + (xincre*xx) + this.devX;
		var y = this.miny + (yincre*yy) + this.devY;	
		
		var sy=" N";
		if (y<0)
			sy=" S";
		var sx=" E";
		if (x<0)
			sx=" W";
		ret=this.formatCoordinate(y) + sy+"  " + this.formatCoordinate(x)+sx
	}	
	return ret;	
}


function DisplayCoordinates(xx,yy)
{
	var s=this.GetCoordinateString(xx,yy);
	if (s != "")
		this._parent.CoordsText = s;
}

function PointFromCoord(x,y)
{
	var xincre = Math.abs(this.xDistance/this.imageWidth)*3;
	var yincre = Math.abs(this.yDistance/this.imageHeight)*3;	

	for (var i=0;i<this.MapPoints.length;i++) {
		if (Math.abs(this.MapPoints[i].latitude - y) < yincre && Math.abs(this.MapPoints[i].longitude- x) < xincre ) {
			return i;
		}
	}
	return -1;
}

function NetworkItemFromCoord(x,y)
{
	// we will build the equation of the line between the pointid and nextpoind and check how much 
	// thje point deviates from it :)
	for (var i=0;i<this.MapNetwork.length;i++) {
		var dy=this.MapNetwork[i].lat_next-this.MapNetwork[i].lat;
		var dx=this.MapNetwork[i].lon_next-this.MapNetwork[i].lon;
		
		var insideY= (y - this.MapNetwork[i].lat_next) * (y - this.MapNetwork[i].lat) <= 0;
		var insideX= (x - this.MapNetwork[i].lon_next) * (x - this.MapNetwork[i].lon) <= 0;

		if (Math.abs(dx) < 100) {
			if (Math.abs(x - this.MapNetwork[i].lon_next) < 100)
				insideX=true;
		} 
		if (Math.abs(dy) < 100) {
			if (Math.abs(y - this.MapNetwork[i].lat_next) < 100)
				insideY=true;
		} 

		if (insideX && insideY) {

			if (Math.abs(dx) < 50) {
				// this means we have a line almost parallel to the X axis (longitude axis).
				// so we have to compare just the latitude whether it matches
				if ((Math.abs(x-this.MapNetwork[i].lon) < 50) || (Math.abs(x-this.MapNetwork[i].lon_next) < 50))
					return i;
			} else {
				// different than zero
				var m = dy/dx;
				var b = this.MapNetwork[i].lat - m*this.MapNetwork[i].lon;
				
				var ytar=x*m+b;
				if ((Math.abs(ytar-y) < 50)) 
					return i;
			} 
		}
		
	}
	
	return -1;
}

function HideTooltip() 
{
	this.tooltip._visible=true; 
	this.tooltip.removeTextField();

}

function AdjustTooltip(xx,yy) 
{
	// lets check for points first 
	var pointFound=false;
	var networkFound = false;
	var ttext="";
	
	
	var xincre = this.xDistance/this.imageWidth;
	var yincre = this.yDistance/this.imageHeight;	
	var x = this.minx + (xincre*xx) + this.devX;
	var y = this.miny + (yincre*yy) + this.devY;	
	
	
	var pointIndex=this.PointFromCoord(x,y);
	
	if (pointIndex>=0) {
		// we are over a point - lets show the tooltip with some info 
		pointFound = true; 
		
		ttext = GetCoordinateString(this.GetXCoordinate(this.mapPoints[pointIndex].longitude),this.GetYCoordinate(this.mapPoints[pointIndex].latitude));
		ttext+="\n" +  this.mapPoints[pointIndex].name +"/"+ this.mapPoints[pointIndex].shortname;
		ttext+="\n";
	}
	// if we have not found any point - lets look for the network items
	if (!pointFound) {
		var networkIndex=NetworkItemFromCoord(x,y);
		
		if (networkIndex>=0) {
			networkFound = true;
			ttext="Driving Distance: " + this.MapNetwork[networkIndex].drivingdistance;
			ttext+="\nDriving Time: " + this.MapNetwork[networkIndex].drivingtime;
		}
	}
	
	if (pointFound || networkFound) 
	{
		if (this.tooltip == null) {
			this.createTextField("tooltip", 123232, xx+20, yy+20, 100, 100);
			this.tooltip.autoSize = "left";
			this.toolTip.border = true;
			this.toolTip.background = 0xffffff;
			this.toolTip.borderColor = 0x000000;
			
			var f = new TextFormat();
			f.font = "Arial";
			f.size = 11;
			f.color = col != undefined ? col : 0x000000;
			this.toolTip.setNewTextFormat(f);
			
		}
		this.toolTip.text = ttext;
		this.tooltip._visible=true; 
	} else {
		HideTooltip();
	}
	
}

function getPointColor(typeid)
{
	for (var i=0; i<this.mapPointTypes.length; i++) {
		if (this.mapPointTypes[i].id == typeid)
			return this.mapPointTypes[i].color;
	}
	return 0xFF00FF; // some default color - if we are asked about point which we do not have intthe array
}


function DrawPoint(i)
{
	if (this.mapPoints.length > i) {
		this.MapImage.roads.lineStyle(2,this.getPointColor(this.mapPoints[i].type), 100);
		
		var x=this.GetXCoordinate(this.mapPoints[i].longitude);
		var y=this.GetYCoordinate(this.mapPoints[i].latitude);
		
		this.MapImage.roads.MoveTo(x-3,y-3);
		this.MapImage.roads.LineTo(x+3,y-3);
		this.MapImage.roads.LineTo(x+3,y+3);
		this.MapImage.roads.LineTo(x-3,y+3);
		this.MapImage.roads.LineTo(x-3,y-3);
		
		if (this.selPoint == i) {
			this.MapImage.roads.lineStyle(2,0xFF0000, 100);
			this.MapImage.roads.MoveTo(x-5,y-5);
			this.MapImage.roads.LineTo(x+5,y-5);
			this.MapImage.roads.LineTo(x+5,y+5);
			this.MapImage.roads.LineTo(x-5,y+5);
			this.MapImage.roads.LineTo(x-5,y-5);
		}
	} else {
		// the point is not in the collection
		trace("trying to paint non existing point");
	}
}

function DrawNetworkItem(i)
{
	if (this.mapNetwork.length > i) {
		var x1=this.GetXCoordinate(this.mapNetwork[i].lon);
		var x2=this.GetXCoordinate(this.mapNetwork[i].lon_next);
		var y1=this.GetYCoordinate(this.mapNetwork[i].lat);
		var y2=this.GetYCoordinate(this.mapNetwork[i].lat_next);
		
		if (selNet == i) {
			this.MapImage.roads.lineStyle(9,0xFF0000, 100);
			this.MapImage.roads.MoveTo(x1,y1);
			this.MapImage.roads.LineTo(x2,y2);
		}
		
		
		this.MapImage.roads.lineStyle(3,0x00FF00, 100);
		this.MapImage.roads.MoveTo(x1,y1);
		this.MapImage.roads.LineTo(x2,y2);
		
	} else {
		//  the netwrok item is not in the collection
		trace("trying to paint non existing network item");
	}
}

function drawLine(clip, x1, y1, x2, y2) 
{
	clip.moveTo(x1,y1);
	clip.lineTo(x2,y2);
}

function drawArrow (clip, angle, len, x1, y1, x2, y2) { 
   	var alpha = angle*Math.PI/180.0; //allows the user to specify the angle 
	var theta = Math.atan2(y2-y1, x2-x1); 
	this.drawLine(clip,x1,y1,x2,y2); 
	this.drawLine(clip,x2-(len*Math.cos(theta-alpha)),y2-(len*Math.sin(theta-alpha)), x2, y2); 
	this.drawLine(clip,x2-(len* Math.cos(theta+alpha)),y2-(len*Math.sin(theta+alpha)), x2, y2); 
} 


function DrawRoute(n)
{
	if (this.mapRoute[n].points.length == 0)
		return;
		
	var x1=0;
	var y1=0;
	var x2=0;
	var y2=0;

	x1=this.GetXCoordinate(this.mapRoute[n].points[0].longitude);
	y1=this.GetYCoordinate(this.mapRoute[n].points[0].latitude);

	
	// this.MapImage.roads.lineStyle(3,0xFF8000, 100);
	this.MapImage.roads.lineStyle(3,mapRoute[n].color, 100);
	
	for (var i=1;i<this.mapRoute[n].points.length;i++) {
		x2=this.GetXCoordinate(this.mapRoute[n].points[i].longitude);
		y2=this.GetYCoordinate(this.mapRoute[n].points[i].latitude);
		
		if (i+1 == this.mapRoute[n].points.length) {
			this.MapImage.roads.MoveTo(x1,y1);
			this.MapImage.roads.LineTo(x2,y2);			
			// the last point !!! - some rectangle
			this.MapImage.roads.beginFill();
			this.MapImage.roads.MoveTo(x2-4,y2-4);
			this.MapImage.roads.LineTo(x2+4,y2-4);
			this.MapImage.roads.LineTo(x2+4,y2+4);
			this.MapImage.roads.LineTo(x2-4,y2+4);
			this.MapImage.roads.LineTo(x2-4,y2-4);
			this.MapImage.roads.endFill();
		} else {
			var len=Math.sqrt((x2-x1)*(x2-x1) + (y2-y1)*(y2-y1));
			if (len < 50)
				len=15.0*(len/50.0);
			else
				len=15;
			this.drawArrow(this.MapImage.roads,22,len,x1,y1,x2,y2);
		}
		x1=x2;
		y1=y2;		
	}
}

function DrawNetwork()
{
	if (this.MapImage.roads == undefined) {
		this.MapImage.createEmptyMovieClip("roads",1212);
		this.MapImage.roads._visible=true;
	}
	
	this.MapImage.roads.clear();
	
	for (var i=0;i<this.mapNetwork.length; i++) {
		this.DrawNetworkItem(i);
	}
	
	for (var i=0;i<this.mapRoute.length; i++) {
		this.DrawRoute(i);
	}	

	for (var i=0;i<this.mapPoints.length; i++) {
		this.DrawPoint(i);
	}	
	
	DrawTerritory(); // if any 
		
}

function MapTrackRectangleMove() {
	this.x2 = this._xmouse;
	this.y2 =  this._ymouse;
	if (this.boxOn)  { this.stretchBox(); }
	
	this.DisplayCoordinates(this._xmouse,this._ymouse);
		
	if (!this.boxOn) {
		this.AdjustTooltip(this._xmouse,this._ymouse);
	}
}

function MapPointClick() {

}

function OVMapPointClick() {
	this.x2 = this._xmouse;
	this.y2 = this._ymouse;
	trace("OVMapPointClick: " + this.x2 + "," + this.y2);
	this.minx = x2;
	this.miny = y2;
	this.setMapAction();
}

function OVDragZoomBoxStart() {
	startDrag ("ZoomBox",true,0,0,this._width-this.ZoomBox._width, this._height-this.ZoomBox._height);
}

function OVDragZoomBoxStop () {
	stopDrag();
	//var cx = this._xmouse;
	//var cy = this._ymouse;
	var xincre = this.xDistance/this.imageWidth;
	var yincre = this.yDistance/this.imageHeight;
	//var cx1 = xincre*cx+this.minx;
	//var cy1 = this.maxy-(yincre*cy);
	var lx = this.ZoomBox._x;
	var tx = this.ZoomBox._y;
	var mapminx = xincre * lx + this.minx;
	var mapmaxx = xincre * this.ZoomBox._width + mapminx;
	
	var mapminy = this.miny + (yincre * tx);
	var mapmaxy = this.miny + (yincre * (tx+this.ZoomBox._height));

	// this is the fucking wrong original
	//var mapmaxy = this.maxy - (yincre * tx);
	//var mapminy = mapmaxy - (yincre * this.ZoomBox._height);
	
	lcSend = new LocalConnection();
	lcSend.send(thisClip.ovToMapConnection+"_Command", "ZoomToExtent", mapminx, mapminy, mapmaxx, mapmaxy);
	lcSend.close();
	lcSend = null;
}

function NotifySelection()
{
	if (this.selPoint != -1) {
		fscommand("point_selected","" + this.mapPoints[this.selPoint].id);
	} else if (this.selNet != -1) {
		fscommand("networkitem_selected","" + this.mapNetwork[this.selNet].pointid + "," + this.mapNetwork[this.selNet].nextpointid);
	}
}

function SelectItem()
{
	this.selPoint=-1;
	this.selNet=-1;
	
	var xincre = this.xDistance/this.imageWidth;
	var yincre = this.yDistance/this.imageHeight;	
	var x = this.minx + (xincre*this._xmouse) + this.devX;
	var y = this.miny + (yincre*this._ymouse) + this.devY;	
	
	// just one item selected !!1
	var pind=PointFromCoord(x, y);
	if (pind >= 0) {
		// point selected - in thie case for sure we have a roads layer :)
		this.selPoint=pind;
		this.DrawNetwork();
	} else {
		var pnetid=this.NetworkItemFromCoord(x, y);
		if (pnetid >=0) {
			this.selNet=pnetid;				
			this.DrawNetwork();
		}
	}	
}

function MovePoint(pointId, moveX, moveY)
{
	var xincre = this.xDistance / this.imageWidth;
	var yincre = this.yDistance / this.imageHeight;
	var xmove = xincre * moveX;
	var ymove = yincre * moveY;
	this.mapPoints[pointid].longitude += xmove;
	this.mapPoints[pointid].latitude += ymove;
	
	// now after we have the new coordinates - lets update all the nework item referencencing it
	for (var i=0; i < this.mapNetwork.length; i++) {
		if (this.mapNetwork[i].pointid == this.mapPoints[pointid].id) {
			this.mapNetwork[i].lat=this.mapPoints[pointid].latitude;
			this.mapNetwork[i].lon=this.mapPoints[pointid].longitude;
		}
		
		if (this.mapNetwork[i].nextpointid == this.mapPoints[pointid].id) {
			this.mapNetwork[i].lat_next=this.mapPoints[pointid].latitude;
			this.mapNetwork[i].lon_next=this.mapPoints[pointid].longitude;
		}
	}
	for (var i=0;i<this.terBound.length;i++) {
		if (this.terBound[i].id==this.mapPoints[pointid].id) {
			this.terBound[i].latitude=this.mapPoints[pointid].latitude;
			this.terBound[i].longitude=this.mapPoints[pointid].longitude;
			break;  // no more than one point
		}
	}
	
	// now we can send the update request to the server:
	this.sendUpdateRequest("pointmoved", "" + this.mapPoints[pointid].id + "," + this.mapPoints[pointid].longitude + "," + this.mapPoints[pointid].latitude);
	
	this.selPoint=pointId;
	this.DrawNetwork();
}

function CreatePoint(xx, yy)
{
	var xincre = this.xDistance / this.imageWidth;
	var yincre = this.yDistance / this.imageHeight;
	var x = this.minx + xincre * xx;
	var y = this.miny + yincre * yy;
	
	var pp = new Object();
	pp.id="NEWPOINT" + random(100000);
	pp.name="newpoint";
	pp.shortname="newpoint";
	
	pp.type = parseInt(pointtypecreate);
	
/*	
	if (this.mapMode == 1)
		pp.type=1;
	else
		pp.type=2;
*/		
	pp.street="";
	pp.city="";
	pp.state="";
	pp.phone="";
	pp.comment="";
	pp.zip="";
	pp.countrycode="";
	pp.latitude=y;
	pp.longitude=x;
	
	if (this.mapPoints == undefined) {
		this.mapPoints = new Array();
	}
	
	this.selPoint=this.mapPoints.push(pp)-1;
	this.selNet=-1;
	
	if (this.pointtypecreate == 4) {
		if (this.terBound == undefined) {
			this.terBound = new Array();
		}
		this.terBound.push(pp); // add it to the territory boundary
	}
	
	this.sendUpdateRequest("pointcreate", pp.id + "," + x + "," + y + "," + pp.type); 
	
	this.DrawNetwork();
}

function Hull_Angle(P0, P1, P2)
{
    // "Angle" between P0->P1 and P0->P2.
    // actually returns: ||(P1-P0) ^ (P2-P0)||
  var dx21 = P2.longitude - P1.longitude;
  var dy21 = P2.latitude - P1.latitude;
  var dx10 = P1.longitude  - P0.longitude ;
  var dy10 = P1.latitude- P0.latitude;
  return (dy21*dx10 - dx21*dy10);
}

function Hull_Sort(from,to,ind, bigger)
{
	var tmp;
	for (var i=from; i<to; i++) {
		for (var j=i;j<to;j++) {
			if (bigger == true) {
				if (this.terBound[ind[i]].longitude < this.terBound[ind[j]].longitude) {
					tmp=ind[i];
					ind[i]=ind[j];
					ind[j]=tmp;
				}
			} else {
				if (this.terBound[ind[i]].longitude > this.terBound[ind[j]].longitude) {
					tmp=ind[i];
					ind[i]=ind[j];
					ind[j]=tmp;
				}
			}
		}
	}
	
	
}


// PTS - Array of points, Ind -Array of indexes used - in that order
function Hull_Sweep(from, to,  Pts, Ind)
{
  var i, n, Tmp;
  n = from+1;
  
  for(i=n+1; i<to; ++i)
  {
      // search where to insert point #i into the chain Ind[0..n]
    while (n-- > from) {
      if ( this.Hull_Angle( Pts[Ind[i]], Pts[Ind[n]], Pts[Ind[n+1]] ) > 0.0 ) {
        break;
	  }
	}
	  // Triangle (n,n+1,n+2) will be clockwise.
    n += 2;
    
    Tmp = Ind[i]; 
    Ind[i] = Ind[n]; 
    Ind[n] = Tmp; 
  }
  return n-from;
}

// PTS - Array of points, Ind -Array of indexes used - in that order
function Hull_Convex_2D(Pts, Ind)
{  
  var n;
  if (Pts.length<=2) return Pts.length;
  
  // First sweep, to find lower boundary.
  // Points are sorted right to left.
	
  this.Hull_Sort(0,Ind.length, Ind, true);	

  n = this.Hull_Sweep(0, Pts.length, Pts, Ind);
  
    // Second sweep, to find upper boundary
    // Actually, we sort the remaining [n..Nb] partition in
    // reverse order (left to right) -> The sweep loop is the same.
  Ind.push(Ind[0]);
  this.Hull_Sort(n,Ind.length-1, Ind, false);	  
  n += this.Hull_Sweep(n, Ind.length, Pts, Ind);
  Ind[n] = Ind[Ind.length-1]; // restore
  
  return n;
}

// by having the territory points - this function arranges them in order
// that thay make a convex (or almost convex if not possible real convex).
/*
function ArrangeTerritoryBounds()
{
	var Ind = new Array();
	for (var i=0;i<this.terBound.length; i++)
		Ind.push(i);
		
	var num=this.Hull_Convex_2D(this.terBound, Ind);
	return Ind.slice(0,num);
}
*/

function DrawTerritory()
{
	var ind = new Array();
	for (var i=0;i<this.terBound.length; i++)
		ind.push(i);
		
	var num=this.Hull_Convex_2D(this.terBound, ind);
	ind=ind.slice(0,num);
	
	if (ind.length <= 2) 
		return;
		
	this.MapImage.roads.beginFill(0x0000ff,10);	
	this.MapImage.roads.lineStyle(3,0xFF0000, 100);

	var bx1=this.GetXCoordinate(this.terBound[ind[0]].longitude);
	var by1=this.GetYCoordinate(this.terBound[ind[0]].latitude);
	
	this.MapImage.roads.MoveTo(bx1,by1);	
	
	for (var i=1; i<ind.length; i++) {
		var x2=this.GetXCoordinate(this.terBound[ind[i]].longitude);
		var y2=this.GetYCoordinate(this.terBound[ind[i]].latitude);
		this.MapImage.roads.LineTo(x2,y2);
	}	
	
	this.MapImage.roads.LineTo(bx1,by1);	
	this.MapImage.roads.endFill();
}

function CreateNetItem(pointId, nextPointId)
{
	for (var i=0; i < this.mapNetwork.length; i++) {
		if ((this.mapNetwork[i].nextpointid == this.mapPoints[nextPointId].id && this.mapNetwork[i].pointid == this.mapPoints[pointId].id)) {
			this.selPoint=-1;
			this.selNet=i;
			this.DrawNetwork();
			return;
		}
	}
	// for sure it is a network item
	
	var pp = new Object();

	pp.pointid=this.mapPoints[pointId].id;
	pp.nextpointid=this.mapPoints[nextPointId].id;
	pp.drivingtime=1000000; // some large number in order not to be used
	pp.drivingdistance=1000000;  // some large number in order not to be used
	pp.roadname="";
	pp.averagespeed=0;
	pp.toll=0;
	pp.lon=this.mapPoints[pointId].longitude;
	pp.lat=this.mapPoints[pointId].latitude;
	pp.lon_next=this.mapPoints[nextPointId].longitude;
	pp.lat_next=this.mapPoints[nextPointId].latitude;
		
	this.selPoint=-1;
	this.selNet=this.mapNetwork.push(pp)-1;	
	
	this.sendUpdateRequest("netconnect", "" + this.mapPoints[pointId].id + "," + this.mapPoints[nextPointId].id); 
	
	this.DrawNetwork();
}

function getPointType(id,defaultType)
{
	for (var i=0;i<this.MapPoints.length; i++) {
		if (this.MapPoints[i].id==id)
			return this.MapPoints[i].type;
	}
	return defaultType;
}


function NetEditPress()
{
	trace("NetEditPress - pressed");
	this.netEditStartX=this._xmouse;
	this.netEditStartY=this._ymouse;

	this.selPoint=-1;
	this.selNet=-1;

	
	SelectItem();
	if (this.selPoint != -1) {
		trace("pressed on a point");
		this.createEmptyMovieClip("trackline", 1122112);
		inTrack=true;
	}
}

function NetEditRelease()
{
	inTrack=false;
	this.trackline.clear();
	
	this.netEditEndX=this._xmouse;
	this.netEditEndY=this._ymouse;
	
	var firstPoint=this.selPoint;
	var firstNet=this.selNet;
	
	var allowedPointOp = (pointtypecreate != 4) || (pointtypecreate==4 && ((this.selPoint != -1 && this.MapPoints[this.selPoint].type==4) || (this.selPoint==-1)));
	var allowedNetOp = (pointtypecreate != 4) ; //|| (pointtypecreate==4 && ((this.selNet != -1 && this.getPointType(this.mapNetwork[this.selNet].pointid,4)==4 && this.getPointType(this.mapNetwork[this.selNet].nextpointid,4)==4) ||(this.selNet==-1)))
	
	this.selPoint=-1;
	this.selNet=-1;
	
	this.SelectItem();
	
	if (Math.abs(this.netEditEndX - this.netEditStartX) > 10|| Math.abs(this.netEditEndY - this.netEditStartY) > 10) {
		if (firstPoint !=-1) {
			if (this.selPoint != -1 && this.selPoint != firstPoint) {
				trace("new net item to be created");
				if (allowedNetOp) {
					this.CreateNetItem(firstPoint, this.selPoint);
					this.NotifySelection(); 
				}
			} else {
				trace("old point to be moved");
				if (allowedPointOp) {
					this.MovePoint(firstPoint,this.netEditEndX - this.netEditStartX,this.netEditEndY - this.netEditStartY);				
					this.NotifySelection(); 
				}
			}
		} 
	} else {
		if (firstPoint == -1 && firstNet == -1) {
			trace("new point to be created");
			if (allowedPointOp) {
				this.CreatePoint(this.netEditEndX, this.netEditEndY);
			}
			// do not notify now, since we do not know the id of the point created
		} else {
			this.NotifySelection(); // if any
		}
	}
}

function NetKeyUp() 
{
	trace("key ap " + Key.getCode());
	
	if (Key.getCode() == 46) { // delete key
		if (this.selPoint!=-1) {
			var idd=this.mapPoints[this.selPoint].id;
			this.mapPoints.splice(this.selPoint,1);
			
			this.sendUpdateRequest("pointdeleted", "" + idd); 			
			
			for (var i=0; i < this.mapNetwork.length;) {
				if (this.mapNetwork[i].pointid == idd) {
					this.mapNetwork.splice(i,1);					
					continue;
				}
				
				if (this.mapNetwork[i].nextpointid == idd) {
					this.mapNetwork.splice(i,1);					
					continue;
				}
				i++;
			}

			for (var i=0;i<this.terBound.length;i++) {
				if (this.terBound[i].id==idd) {
					this.terBound.splice(i,1);					
					break;  // no more than one point
				}
			}

		} else{
			if (this.selNet!=-1) {
				this.sendUpdateRequest("netitemdeleted", "" + this.mapNetwork[this.selNet].pointid + "," +this.mapNetwork[this.selNet].nextpointid); 
				this.mapNetwork.splice(this.selNet,1);
			}
		}
		this.selPoint=-1;
		this.selNet=-1;
		this.DrawNetwork();
	}
}

function NetworkEditMove()
{
	// at least lets display the coordinate
	this.DisplayCoordinates(this._xmouse,this._ymouse);
	this.AdjustTooltip(this._xmouse,this._ymouse);
	
	this.netEditEndX=this._xmouse;
	this.netEditEndY=this._ymouse;
	
	if (this.selPoint != -1 && inTrack) {
		// we have a first point from where we drag
		// draw a line till mouse pointer
		this.trackline.clear();
		this.trackline.lineStyle(2, 0x0000FF, 100);		
		this.trackline.moveTo(this.netEditStartX, this.netEditStartY);
		this.trackline.lineTo(this.netEditEndX, this.netEditEndY);
	}
	
}


// resize extent box if this is an overview map
function setZoomBoxExtent(mname, inMinX, inMinY, inMaxX, inMaxY) {
	if (mname != this.mapname) {
		this.mapname = mname;
		this.MapAction = "Overview";
		this.setMapAction();
	} 
	var xincre = this.xDistance / this.imageWidth;
	var yincre = this.yDistance / this.imageHeight;
	var bigw = inMaxX - inMinX;
	var bigh = inMaxY - inMinY;
	var zwidth = bigw / xincre;
	var zheight = bigh / yincre;
	var leftoff = inMinX - this.minx;
	//var topoff = this.maxy - inMaxY;
	var topoff = inMinY - this.miny;
	var leftx = leftoff / xincre;
	var topy = topoff / yincre;
	this.ZoomBox._x = leftx;
	this.ZoomBox._y = topy;
	this.ZoomBox._width = zwidth;
	this.ZoomBox._height = zheight;
	this.ZoomBox._visible = true;
	//trace("setZoomBoxExtent");
}

function realUpdateZoomBox()
{
	lcSend = new LocalConnection();
	lcSend.send(this.mapToOvConnection + "_Command", "OverviewExtentBox", this.mapname, this.minx, this.miny, this.maxx, this.maxy);
	lcSend.close();
	lcSend = null;	
}

// resize ZoomBox
function stretchBox() {
	w = Math.abs(this.x2 - this.x1);
	h = Math.abs(this.y2 - this.y1);
	if (this.x1>this.x2) {
		this.zright=this.x1;
		this.zleft=this.x2;
	} else {
		this.zleft=this.x1;
		this.zright=this.x2;
	}
	if (this.y1>this.y2) {
		this.zbottom=this.y1;
		this.ztop=this.y2;
	} else {
		this.ztop=this.y1;
		this.zbottom=this.y2;
	}
	this.ZoomBox._width = w;
	this.ZoomBox._height = h;
	this.ZoomBox._x = (this.x2 < this.x1) ? this.x2 : this.x1;
	this.ZoomBox._y = (this.y2 < this.y1) ? this.y2 : this.y1;
	this.ZoomBox._visible = true;
}

// Fixed Pan
function PanDirection(dir) {
	var w = this.xDistance * this.panFactor;
	var h = this.xDistance * this.panFactor;
	switch (dir) {
		case "north":
			this.miny = this.miny + h;
			this.maxy = this.maxy + h;
			break;
		
		case "south":
			this.miny = this.miny - h;
			this.maxy = this.maxy - h;
			break;
		
		case "west":
			this.minx = this.minx - w;
			this.maxx = this.maxx - w;
			break;
		
		case "east":
			this.minx = this.minx + w;
			this.maxx = this.maxx + w;
			break;
		
	}
	this.requestURL = this.baseURL + "?requesttype=ZoomToExtent&type="+ this.mapType +"&mapMode="+this.mapMode + "&width=" + this.imageWidth + "&height=" + this.imageHeight + "&map=" + this.mapIndex + "&random=" + random(10000) + "&minx=" + this.minx + "&miny=" + this.miny + "&maxx=" + this.maxx + "&maxy=" + this.maxy + this.mapSource;
	sendMapRequest();
}

function StartUp() {
	this.requestURL = this.baseURL + "?requesttype=StartUp&type="+this.mapType+"&mapMode="+this.mapMode+ "&width=" + this.imageWidth + "&height=" + this.imageHeight + "&map=" + this.mapIndex + "&random=" + random(100000) + this.mapSource;
	this.newExtent=true;
	sendMapRequest();
}


function FullExtent() {
	this.requestURL = this.baseURL + "?requesttype=FullExtent&type="+this.mapType+"&width=" + this.imageWidth + "&height=" + this.imageHeight + "&map=" + this.mapIndex + "&random=" + random(100000) + this.mapSource;
	this.newExtent=true;
	sendMapRequest();
}

function DefaultExtent() {
	this.requestURL = this.baseURL + "?requesttype=DefaultExtent&type="+this.mapType+"&width=" + this.imageWidth + "&height=" + this.imageHeight + "&map=" + this.mapIndex + "&random=" + random(100000) + this.mapSource;
	this.newExtent=true;
	sendMapRequest();
}


function sendUpdateRequest(actiontype, params) {
	this.requestURL = this.baseURL + "?requesttype=" + actiontype + "&params="+params + "&mapid="+this.mapname+"&realZoom=false"+"&mapMode="+this.mapMode+ "&type="+this.mapType+"&minx=" + this.minx + "&miny=" + this.miny + "&maxx=" + this.maxx + "&maxy=" + this.maxy + "&width=" + this.imageWidth + "&height=" + this.imageHeight + "&map=" + this.mapIndex + "&random=" + random(100000) + this.mapSource;
	this.newExtent=true;
	sendMapRequest();
}

function setMapAction() {
	this.requestURL = this.baseURL + "?requesttype=" + this.MapAction + "&mapid="+this.mapname+"&realZoom="+this.realZoom +"&mapMode="+this.mapMode+ "&type="+this.mapType+"&minx=" + this.minx + "&miny=" + this.miny + "&maxx=" + this.maxx + "&maxy=" + this.maxy + "&width=" + this.imageWidth + "&height=" + this.imageHeight + "&map=" + this.mapIndex + "&random=" + random(100000) + this.mapSource;
	this.newExtent=true;
	sendMapRequest();
}

function ZoomBack() {
	if (this.currentExtent>0) {
		this.currentExtent--;
		this.minx = this.mapExtents[currentExtent].minx;
		this.miny = this.mapExtents[currentExtent].miny;
		this.maxx = this.mapExtents[currentExtent].maxx;
		this.maxy = this.mapExtents[currentExtent].maxy;
		this.requestURL = this.baseURL + "?requesttype=ZoomToExtent&type="+this.mapType+"&width=" +"&mapMode="+this.mapMode + this.imageWidth + "&height=" + this.imageHeight + "&map=" + this.mapIndex + "&minx=" + this.minx + "&miny=" + this.miny + "&maxx=" + this.maxx + "&maxy=" + this.maxy +  "&random=" + random(100000) + this.mapSource;
		this.newExtent=false;
		sendMapRequest();
	}
}

function ZoomNext() {
	if (this.currentExtent<this.extentCount-1) {
		this.currentExtent++;
		this.minx = this.mapExtents[currentExtent].minx;
		this.miny = this.mapExtents[currentExtent].miny;
		this.maxx = this.mapExtents[currentExtent].maxx;
		this.maxy = this.mapExtents[currentExtent].maxy;
		this.requestURL = this.baseURL + "?requesttype=ZoomToExtent&width=" + this.imageWidth +"&mapMode="+this.mapMode + "&type="+this.mapType+"&height=" + this.imageHeight + "&map=" + this.mapIndex + "&minx=" + this.minx + "&miny=" + this.miny + "&maxx=" + this.maxx + "&maxy=" + this.maxy +  "&random=" + random(100000) + this.mapSource;
		this.newExtent=false;
		sendMapRequest();
	}
}

// get map image
function sendMapRequest() {
	lcSend = new LocalConnection();
	lcSend.send("UpdateStatus", "LoadMapClip",true) ;
	lcSend.close();
	lcSend = null;
	trace("Request: " + this.requestURL);
	this.myXML = new XML();

	this.myXML.onLoad = this.processMapXML;
	this.myXML.load(this.requestURL);

}

function GetPoint(idpoint)
{
	for (var i=0; i< this.mapPoints.length; i++) {
		if (this.mapPoints[i].id==idpoint)
			return i;
	}
	return -1;
}

// parse image response
function processMapXML(success) {
	if (success) {
		theXML = this;
		var imode = "";
		var iNode;
//		trace(this);
		if (hasNode(theXML,"IMAGE",true)) {
			iNode = getNode(theXML,"IMAGE",true);
			var url = iNode.attributes.url;
			loadMovie (url, thisClip.MapImage.MapClip);
			thisClip.MapImage._x = 0;
			thisClip.MapImage._y = 0;
	
		}
		if (hasNode(theXML,"MAPNAME",true)) {
			iNode = getNode(theXML,"MAPNAME",true);	
			thisClip.mapname = iNode.attributes.id;
		}
		
		if (hasNode(theXML,"DEVIATION",true)) {
			var oNode = getNode(theXML,"deviation",true);
			thisClip.devX = parseFloat(oNode.attributes.x);
			thisClip.devY = parseFloat(oNode.attributes.y);
		}

		if (hasNode(theXML,"ENVELOPE",true)) {
			//trace("Has ENVELOPE tag\n");
			iNode = getNode(theXML,"ENVELOPE",true);
			thisClip.minx = parseFloat(iNode.attributes.minx);
			thisClip.miny = parseFloat(iNode.attributes.miny);
			thisClip.maxx = parseFloat(iNode.attributes.maxx);
			thisClip.maxy = parseFloat(iNode.attributes.maxy);
			thisClip.xDistance = (thisClip.maxx-thisClip.minx);
			thisClip.yDistance = (thisClip.maxy-thisClip.miny);			
			thisClip.xHalf = thisClip.xDistance/2;
			thisClip.yHalf = thisClip.yDistance/2;
			
			if (thisClip.newExtent){
				thisClip.mapExtents[thisClip.extentCount] = {minx: thisClip.minx, miny: thisClip.miny, maxx: thisClip.maxx, maxy: thisClip.maxy };
				thisClip.extentCount++;
				thisClip.currentExtent++;
			}
			lcSend = new LocalConnection();
			lcSend.send("UpdateStatus", "MapExtent", Math.round(thisClip.minx*1000)/1000, Math.round(thisClip.miny*1000)/1000, Math.round(thisClip.maxx*1000)/1000, Math.round(thisClip.maxy*1000)/1000);
			lcSend.close();
			lcSend = null;
			
			if (thisClip.mapToOvConnection.length>0) {
				if (thisClip.initialNotified) {
					thisClip.initialNotified=false;
					fscommand("map_ready","");
				}
				lcSend = new LocalConnection();
				lcSend.send(thisClip.mapToOvConnection + "_Command", "OverviewExtentBox", thisClip.mapname, thisClip.minx, thisClip.miny, thisClip.maxx, thisClip.maxy);
				lcSend.close();
				lcSend = null;
			}
			if (thisClip.mapToMapConnection.length>0) {
				
				lcSend = new LocalConnection();
				lcSend.send(thisClip.mapToMapConnection + "_Command", "ZoomToExtent", thisClip.minx, thisClip.miny, thisClip.maxx, thisClip.maxy);
				lcSend.close();
				lcSend = null;
			}
			
			if (thisClip.ovToMapConnection.length > 0) {
				lcSend = new LocalConnection();
				lcSend.send(thisClip.ovToMapConnection + "_Command", "UpdateZoomBox");
				lcSend.close();
				lcSend = null;				
			}
		}
/*		
		if (thisClip.mapMode == 0) {
			thisClip.mapPoints = new Array();
			thisClip.mapNetwork = new Array();
			thisClip.selPoint=-1;
			thisClip.selNet=-1;
		}
*/		
		if (hasNode(theXML,"points",true) || hasNode(theXML,"network",true)) {
			var selPointDbId=-1;
			var selNetDbStart=-1;
			var selNetDbEnd=-1;
			
			if (thisClip.selPoint >=0) {
				selPointDbId=thisClip.mapPoints[thisClip.selPoint].id;
			}
			
			if (thisClip.selNet >=0) {
				selNetDbStart=thisClip.mapNetwork[thisClip.selNet].pointid;;
				selNetDbEnd=thisClip.mapNetwork[thisClip.selNet].nextpointid;
			}
			
			thisClip.mapPoints = new Array();
			thisClip.mapNetwork = new Array();
	
			thisClip.selPoint=-1;
			thisClip.selNet=-1;
			
			trace("Has points tag\n");
			var oNode = getNode(theXML,"points",true);
			if (oNode.hasChildNodes) {
				//trace("has childNodes");
				var paramNodes = oNode.childNodes;
				var j = 0;
				for (var i = 0;i<paramNodes.length;i++) {
					if (paramNodes[i].nodeName == "point") {
						var pnode = paramNodes[i];
						var pp = new Object();
						pp.id=parseInt(pnode.attributes.id);
						pp.name=pnode.attributes.name;
						pp.shortname=pnode.attributes.shortname;
						pp.type=pnode.attributes.type;
						pp.street=pnode.attributes.street;
						pp.city=pnode.attributes.city;
						pp.state=pnode.attributes.state;
						pp.phone=pnode.attributes.phone;
						pp.comment=pnode.attributes.comment;
						pp.zip=pnode.attributes.zip;
						pp.countrycode=pnode.attributes.countrycode;
						pp.latitude=parseInt(pnode.attributes.latitude);
						pp.longitude=parseInt(pnode.attributes.longitude);
						if (selPointDbId == pp.id) {
							thisClip.selPoint=thisClip.mapPoints.push(pp)-1;
						} else 
							thisClip.mapPoints.push(pp);
						
					}
				}
			}
			trace("points read from the stream:" + thisClip.mapPoints.length)
		
			if (hasNode(theXML,"network",true)) {
				trace("Has network tag\n");
				var oNode = getNode(theXML,"network",true);
				if (oNode.hasChildNodes) {
					//trace("has childNodes");
					var paramNodes = oNode.childNodes;
					var j = 0;
					for (var i = 0;i<paramNodes.length;i++) {
						if (paramNodes[i].nodeName == "networkitem") {
							var pnode = paramNodes[i];
							var pp = new Object();
	
							pp.pointid=parseInt(pnode.attributes.pointid);
							pp.nextpointid=parseInt(pnode.attributes.nextpointid);
							pp.drivingtime=parseInt(pnode.attributes.drivingtime);
							pp.drivingdistance=parseInt(pnode.attributes.drivingdistance);
							pp.roadname=pnode.attributes.roadname;
							pp.averagespeed=parseInt(pnode.attributes.averagespeed);
							pp.rushamfrom=pnode.attributes.rushamfrom;
							pp.rushamto=pnode.attributes.rushamto;
							pp.rushpmfrom=pnode.attributes.rushpmfrom;
							pp.rushpmto=pnode.attributes.rushpmto;
							pp.rushfactor=parseInt(pnode.attributes.rushfactor);
							pp.toll=parseInt(pnode.attributes.toll);
							pp.lon=parseInt(pnode.attributes.lon);
							pp.lon_next=parseInt(pnode.attributes.lon_next);
							pp.lat=parseInt(pnode.attributes.lat);
							pp.lat_next=parseInt(pnode.attributes.lat_next);
							
							if (selNetDbStart == pp.pointid && selNetDbEnd==pp.nextpointid) {
								thisClip.selNet=thisClip.mapNetwork.push(pp)-1;
							} else {
								thisClip.mapNetwork.push(pp);
							}
						}
					}
				}
				trace("network items read from the stream:" + thisClip.mapNetwork.length)
			}
			
			// !!! SHOULD BE CLEARED 
			if (thisClip.MapImage.roads != undefined)
				thisClip.MapImage.roads.clear();
			
			if (thisClip.mapPoints.length > 0 || thisClip.mapNetwork.length > 0 || thisClip.mapRoute.Length > 0) {
				thisClip.DrawNetwork();
			}
		}
		if (hasNode(theXML,"routes",true)) {
			trace("Has routes tag\n");
			thisClip.mapRoute = new Array();
			var oNode = getNode(theXML,"routes",true);
			if (oNode.hasChildNodes) {
				//trace("has childNodes");
				var paramNodes = oNode.childNodes;
				for (var i = 0;i<paramNodes.length;i++) {
					if (paramNodes[i].nodeName == "route") {
						var pnode = paramNodes[i];
						var pp = new Object();
						pp.id=parseInt(pnode.attributes.routeid);
						pp.vehicleid = pnode.attributes.vehicleid;
						pp.start = pnode.attributes.start;
						pp.end = pnode.attributes.end;
						pp.comment = pnode.attributes.comment;
						pp.color = pnode.attributes.color;
						pp.points = new Array();
						if (hasNode(pnode,"routepoints",true)) {
							var pons=getNode(pnode,"routepoints",true);
							if (pons.hasChildNodes) {
								pons=pons.childNodes;
								var j = 0;
								for (var j = 0;j<pons.length;j++) {
									var pon=pons[j];
									// there are some strange empty nodes in the XML
									// the XML itself is well formed - but seems the
									// Flash parser is stupid enough to make mistakes
									if (pon.nodeName == "point") {
										var pns=new Object();
										pns.id=parseInt(pon.attributes.id);
										pns.latitude=parseInt(pon.attributes.latitude);
										pns.longitude=parseInt(pon.attributes.longitude);
										pp.points.push(pns);
									}
								}
								
							}
						}
						thisClip.mapRoute.push(pp);								
					}
				}
			}
			thisClip.DrawNetwork();
		}
		
		// no more than one point at a time
		if (hasNode(theXML,"pointcreated",true)) {
			var pnode = getNode(theXML,"pointcreated",true);

			var pid = pnode.attributes.pid;
			var dbpid = parseInt(pnode.attributes.dbpid);
			
			for (var i=0;i<thisClip.mapPoints.length; i++) {
				if (pid == thisClip.mapPoints[i].id) {
					thisClip.mapPoints[i].id=dbpid;
					// now we are ready to fire :)
					thisClip.selPoint=i;
					thisClip.selNet=-1;
					thisClip.NotifySelection();
					break;
				}
			}
		}
		
		
		if (hasNode(theXML,"territories",true)) {
			var tnode=getNode(theXML,"territories",true);
			
			mapTerritories = new Array();
			
			if (tnode.hasChildNodes) {
				for (var i = 0;i<tnode.childNodes.length;i++) {
					if (tnode.childNodes[i].nodeName == "territory") {
						var pnode = tnode.childNodes[i];
						
						var tid = parseInt(pnode.attributes.pid);
						if (tid != thisClip.territoryId) {
							var ter=new Object();
							ter.id=tid;
							ter.name=pnode.attributes.name;
							ter.pts = new Array();
							if (pNode.hasChildNodes) {
								//trace("has childNodes");
								var paramNodes = oNode.childNodes;
								var j = 0;
								for (var i = 0;i<paramNodes.length;i++) {
									if (paramNodes[i].nodeName == "point") {
										var pnode = paramNodes[i];
										var pp = new Object();
										pp.id=parseInt(pnode.attributes.id);
										pp.name=pnode.attributes.name;
										pp.shortname=pnode.attributes.shortname;
										pp.type=pnode.attributes.type;
										pp.street=pnode.attributes.street;
										pp.city=pnode.attributes.city;
										pp.state=pnode.attributes.state;
										pp.phone=pnode.attributes.phone;
										pp.comment=pnode.attributes.comment;
										pp.zip=pnode.attributes.zip;
										pp.countrycode=pnode.attributes.countrycode;
										pp.latitude=parseInt(pnode.attributes.latitude);
										pp.longitude=parseInt(pnode.attributes.longitude);
										ter.pts.push(pp);
									}
								}
							}
							thisClip.mapTerritories.push(ter);
						}
					}
				}
				thisClip.DrawNetwork();
			}
		}
			
			
			
/*
			if (hasNode(theXML,"territory",true)) {
				var pnode = getNode(theXML,"territory",true);
				var tid = parseInt(pnode.attributes.pid);
				if (tid == thisClip.territoryId) {
					thisClip.terName=pnode.attributes.name;
					terBound=new Array();
					
					if (pNode.hasChildNodes) {
						//trace("has childNodes");
						var paramNodes = oNode.childNodes;
						var j = 0;
						for (var i = 0;i<paramNodes.length;i++) {
							if (paramNodes[i].nodeName == "point") {
								var pnode = paramNodes[i];
								var pp = new Object();
								pp.id=parseInt(pnode.attributes.id);
								pp.name=pnode.attributes.name;
								pp.shortname=pnode.attributes.shortname;
								pp.type=pnode.attributes.type;
								pp.street=pnode.attributes.street;
								pp.city=pnode.attributes.city;
								pp.state=pnode.attributes.state;
								pp.phone=pnode.attributes.phone;
								pp.comment=pnode.attributes.comment;
								pp.zip=pnode.attributes.zip;
								pp.countrycode=pnode.attributes.countrycode;
								pp.latitude=parseInt(pnode.attributes.latitude);
								pp.longitude=parseInt(pnode.attributes.longitude);
								thisClip.terBound.push(pp);
							}
						}
					}
					thisClip.DrawNetwork();
				}
			}
		}
*/		
		
	} else {
		lcError = new LocalConnection();
		lcError.send("UpdateStatus","Status","Server Error.... Try again later.");
	}
	lcSend = new LocalConnection();
	lcSend.send("UpdateStatus", "LoadMapClip",false) ;
	lcSend.close();
	lcSend = null;

}


function swapSpace(str) {
	// replace encoded space with the space
	var str2 = str;
	var pos = str.indexOf("_x0020_");
	if (pos!=-1) str2 = str.substring(0,pos) + " " + str.substring(pos + 8);
	str = str2;
	return str;
}



// set up local connections as listeners
lcTool = new LocalConnection();
lcTool.MapDragImage = function(action) {
	MapDragImage(action);
}
lcTool.FullExtent = function() {
	FullExtent();
}

lcTool.DefaultExtent = function() {
	DefaultExtent();
}
lcTool.MapTrackRectangle = function(action) {
	MapTrackRectangle(action);
}
lcTool.MapPoint = function(action) {
	MapPoint(action);
}
lcTool.PanDirection = function(dir) {
	PanDirection(dir);
}
lcTool.ZoomBack = function() {
	ZoomBack();
}
lcTool.ZoomNext = function() {
	ZoomNext();
}

lcTool.connect("toolCommand");

// set up for unique listener for this instance
//trace("Component name: " + this._name);
listenerName = this._name + "_Command";
lcMap = new LocalConnection();

lcMap.FullExtent = function() {
	FullExtent();
}

lcMap.DefaultExtent = function() {
	DefaultExtent();
}

lcMap.OverviewExtentBox = function(mname, xx1, yy1, xx2, yy2) {
	setZoomBoxExtent(mname,xx1, yy1, xx2, yy2);
}


lcMap.PanDirection = function(dir) {
	PanDirection(dir);
}

lcMap.CenterAt = function(inX, inY) {
	//trace("CenterAt() from localConnection\n");
	minx = inX;

	miny = inY;
	thisClip.requestURL = thisClip.baseURL + "?action=103&minx=" + minx + "&miny=" + miny + "&maxx=" + maxx + "&maxy=" + maxy + "&width=" + thisClip.imageWidth + "&height=" + thisClip.imageHeight + "&map=" + thisClip.mapIndex + "&random=" + random(10000);
	thisClip.sendMapRequest();
}

lcMap.ZoomToExtent = function(xx1, yy1, xx2, yy2) {
	thisClip.SetEnvelope(xx1, yy1, xx2, yy2);
	thisClip.realZoom=false;
	thisClip.MapAction = "ZoomToExtent";
	thisClip.setMapAction();
}

lcMap.UpdateZoomBox = function()
{
	realUpdateZoomBox();
}

lcMap.connect(listenerName);
