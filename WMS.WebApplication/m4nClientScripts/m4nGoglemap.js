  var directionDisplay;
  var directionsService;
  var stepDisplay;
  var LatLonArray = new Array();
  var pathCoordinates = new google.maps.MVCArray();
  var markersArray = [];
  var isTerr=0;
  var clicklistenerHandle=null;;
  var rightclicklistenerHandle=null;
  var bounds=new google.maps.LatLngBounds();
  var clickAddContactHandle;
  var Tag;
  var Tag1;
  var polygons = new Array();      
var TargetRouteID='';

var  polygon,poly;
 
// geocoding
  function geocode(address) {
    var geocoder = new google.maps.Geocoder();  
    geocoder.geocode({
      'address': address,
      'partialmatch': true}, geocodeResult);
  }
  
    function geocodeResult(results, status) {
 //   alert(results.length);
//    alert(results[0].geometry.viewport);

    if (status == 'OK' && results.length > 0) {
      map.fitBounds(results[0].geometry.viewport);
      //alert(results[0].geometry.location)

var marker=new google.maps.Marker({map: map,position: results[0].geometry.location, draggable: false ,title: results[0].formatted_address+': '+results[0].geometry.location}); 
var infowindow = new google.maps.InfoWindow({content: results[0].formatted_address+': '+results[0].geometry.location});
google.maps.event.addListener(marker, 'click', function() {infowindow.open(map, this);}); 

marker.setMap(map);

    } else {
      alert("Geocode was not successful for the following reason: " + status);
    }
  }
  
  function reverseGeocode(oLatLon) {
    var geocoder = new google.maps.Geocoder();  
    geocoder.geocode({latLng:oLatLon},reverseGeocodeResult);
  }
  
  
    function reverseGeocodeResult(results, status) {
    currentReverseGeocodeResponse = results;
    alert(status)
    if(status == 'OK') {
      if(results.length == 0) {
        alert('None');
      } else {
        
        alert(results[0].formatted_address);
      }
    } else {
      alert('Zero result');
    }
  }
  
  // end geocoding //////////

//m4n google map V2
function LoadMapSearchControl() {
      var options = {
            zoomControl : GSmapSearchControl.ZOOM_CONTROL_ENABLE_ALL,
            title : "Made4net",
            url : "http://www.Made4net.com",
            mapTypeControl : GSmapSearchControl.MAP_TYPE_ENABLE_ALL,
            idleMapZoom : GSmapSearchControl.ACTIVE_MAP_ZOOM,
            activeMapZoom : GSmapSearchControl.ACTIVE_MAP_ZOOM,
            drivingDirections : GSmapSearchControl.DRIVING_DIRECTIONS_FROM_USER,
            showResultList : GSmapSearchControl.DEFAULT_RESULT_LIST,
            centerIcon : G_DEFAULT_ICON
            }

      new GSmapSearchControl(
            document.getElementById("mapsearch"),
            "Tel Aviv",
            options
            );

    }
    
    function btnControl(controlDiv, map, tooltip,caption,callbackfunction){
    controlDiv.style.paddingTop = '6px';
    controlDiv.style.paddingLeft = '2px';
    controlDiv.style.paddingRight = '2px';
    var controlUI = document.createElement('DIV');
    controlUI.style.backgroundColor = 'white';
    controlUI.style.borderStyle = 'solid';
    controlUI.style.borderWidth = '1px';controlUI.style.cursor = 'pointer';
    controlUI.style.textAlign = 'center';
    controlUI.title = tooltip.split("_").join(" ");
    controlDiv.appendChild(controlUI);
    var controlText = document.createElement('DIV');
    controlText.style.fontFamily = 'Arial,sans-serif';
    controlText.style.fontSize = '12';
    controlText.style.paddingLeft = '2px';
    controlText.style.paddingRight = '2px';
    controlText.style.paddingTop = '0px';
    controlText.style.paddingBottom = '1px';
    controlText.innerHTML = caption.split("_").join(" ");controlUI.appendChild(controlText);
    google.maps.event.addDomListener(controlUI, 'click', callbackfunction);
    }

function setCenterPoint(){map.setCenter(CenterPoint);}

function fitMap()
{
if(!bounds.isEmpty()) 
    map.fitBounds(bounds);
else
    map.setCenter(CenterPoint);
}



function MapRefresh()
{
document.Form1.submit();
}

function MapTest()
{

//reverseGeocode(new google.maps.LatLng(41.850033, -87.6500523))
geocode("7496 Nieman Rd Shawnee Mission  KS");
//geocode("Zaritski 4, Tel Aviv, Israel");
//handler("http://localhost/RMSExpert/Screens/black.xml","?param="+polygontoString());
	//alert(polygontoString());
}



//////////// territories  ///////////

function polygontoString()
{
var res='';
	for(j=0;j<pathCoordinates.length;j++){
	var oLatLon=pathCoordinates.getAt(j);
	res+=String(oLatLon.lat())+','+String(oLatLon.lng())+'#';
	}
return res
}

function StartDrawTerritories()
{
    if(isTerr==0)
    {
    
        DrawLine();
        isTerr=1;
    }
    else
    {
        UnDrawLine()
        isTerr=0;
    }
}


function UnDrawLine()
{
    google.maps.event.clearListeners(map, 'click');
    google.maps.event.clearListeners(map, 'rightclick');

  	for(j=0;j<polygons.length;j++)
  	{
    google.maps.event.clearListeners(polygons[j], 'click');
    google.maps.event.clearListeners(polygons[j], 'rightclick');
  	}

}

function DrawLine()
{
  	for(j=0;j<polygons.length;j++)
  	{
      clicklistenerHandle=google.maps.event.addListener(polygons[j], 'click', addLatLng);
      rightclicklistenerHandle=google.maps.event.addListener(polygons[j], 'rightclick', removeLatLng);
  	}



  var polyOptions = {
    path: pathCoordinates,
    strokeColor: '#000000',
    strokeOpacity: 0.5,
    strokeWeight: 3
  }
  

  poly = new google.maps.Polyline(polyOptions);
//  poly.setMap(map);

  // Add a listener for the click event
  clicklistenerHandle=google.maps.event.addListener(map, 'click', addLatLng);
  rightclicklistenerHandle=google.maps.event.addListener(map, 'rightclick', removeLatLng);
  
  
}
 
 
function ready() {
    var map = new google.maps.Map(document.getElementById('map'), { center: new google.maps.LatLng(21.17, -86.66), zoom: 9, mapTypeId: google.maps.MapTypeId.HYBRID, scaleControl: true });
    var isClosed = false;
    var poly = new google.maps.Polyline({ map: map, path: [], strokeColor: "#FF0000", strokeOpacity: 1.0, strokeWeight: 2 });
    google.maps.event.addListener(map, 'click', function (clickEvent) {
        if (isClosed)
            return;
        var isFirstMarker = poly.getPath().length === 0;
        var marker = new google.maps.Marker({ map: map, position: clickEvent.latLng, draggable: true });
        if (isFirstMarker) {
            google.maps.event.addListener(marker, 'click', function () {
                if (isClosed)
                    return;
                var path = poly.getPath();
                poly.setMap(null);
                poly = new google.maps.Polygon({ map: map, path: path, strokeColor: "#FF0000", strokeOpacity: 0.8, strokeWeight: 2, fillColor: "#FF0000", fillOpacity: 0.35 });
                isClosed = true;
            });
        }
        google.maps.event.addListener(marker, 'drag', function (dragEvent) {
            poly.getPath().setAt(markerIndex, dragEvent.latLng);
        });
        poly.getPath().push(clickEvent.latLng);
    });
}
 
 function DrawPoligon(map)
{
 
  
  var PolygonOptions = {
    path: pathCoordinates,
    fillColor: '#FFF000',   
    fillOpacity: 0.1,
    strokeColor: '#FF0000',
    strokeOpacity: 0.5,
    strokeWeight: 1
  }

//polygon.setMap(null);
  polygon = new google.maps.Polygon(PolygonOptions);
    polygon.setPath(pathCoordinates)

  polygon.setMap(map);

}
 
 function SaveTerritories()
 {
     if (pathCoordinates.length<3){
        alert('Territory not found. Draw it first...')
        return
     }
     
    
    if (String(Tag)=='undefined'){
      alert('select territory first...')
      return
    }         

    //alert(pathCoordinates.length);
    //alert(Tag);
     
  
	var res='';
	for(j=0;j<pathCoordinates.length;j++){
	var oLatLon=pathCoordinates.getAt(j);
	res+=String(oLatLon.lat())+','+String(oLatLon.lng())+'Z';
    }
    Tag1='?t='+Tag+'&p='+res;
    var url ="SaveTerritory.aspx"+Tag1;
//window.open(url)
//return

    var xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
    xmlhttp.Open("POST",url,false);
	xmlhttp.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
	xmlhttp.send();
    xmlhttp.responseText;
    alert('Done.');

     
 }
 ////////////////
 
 function setBounds(latLng){
  LatLonArray.push(latLng);
  if (LatLonArray.length>2){
    var southWest = LatLonArray[0];
    var northEast = LatLonArray[LatLonArray.length];
    var bounds = new google.maps.LatLngBounds(southWest,northEast);
    map.fitBounds(bounds);
    }
  }

function addLatLng(event) {

  pathCoordinates.insertAt(pathCoordinates.length, event.latLng);
  
 
  poly.setPath(pathCoordinates)
  poly.setMap(map);


  
  DrawPoligon(map,pathCoordinates)


  var marker = new google.maps.Marker({
    position: event.latLng,
    draggable:true,
    map: map
  });
  marker.setTitle("#" + pathCoordinates.length);
  //google.maps.event.addListener(marker, 'rightclick', function removeMarker (){this.setMap(null);markersArray.remove(this);});
  
  markersArray.push(marker);
 
//  google.maps.event.addListener(marker, 'dragstart', function redrawPolygonMarker (){pathCoordinates=pathCoordinates.removeAt(pathCoordinates.length, event.latLng);});
//  google.maps.event.addListener(marker, 'dragend', function redrawPolygonMarker (){pathCoordinates=pathCoordinates.insertAt(pathCoordinates.length, event.latLng);poly.setMap(map);});
} 

function removeLatLng(event) {
  var path = poly.getPath();
    path.removeAt(path.getLength()-1);

polygon.setMap(map);
    poly.setMap(map);
    markersArray[markersArray.length-1].setMap(null);
    markersArray=markersArray.slice(0, -1);
}

/////////// teritories end /////////////////

/// polygon //////////
 function DrawPreparedPoligon(map,pathCoordinates,pfillColor, pfillOpacity,pstrokeColor,pstrokeOpacity,pstrokeWeight)
{
  var PolygonOptions = {
    paths: pathCoordinates,
    fillColor: pfillColor,   
    fillOpacity: pfillOpacity,
    strokeColor: pstrokeColor,
    strokeOpacity: pstrokeOpacity,
    strokeWeight: pstrokeWeight
  }
  var polygon = new google.maps.Polygon(PolygonOptions);
  polygon.setMap(map);
    polygons.push(polygon); 
} 
//////////// end polygon /////////////////


/// run serverside

function submitFormData(url, req) 
{ 
var xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
//"http://<%=Request.ServerVariables("Server_Name")%>/"
	xmlhttp.Open("POST",url,false);
	xmlhttp.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
	xmlhttp.send(req);
	//alert(xmlhttp.responseXML.xml);
    oXMLHTTP.responseText;
} 

function getXMLHttpRequest() 
{
    if (window.XMLHttpRequest) {
        return new window.XMLHttpRequest;
    }
    else {
        try {
            return new ActiveXObject("MSXML2.XMLHTTP.3.0");
        }
        catch(ex) {
            return null;
        }
    }
}
function handler(url,param)
{
var oReq = getXMLHttpRequest();
if (oReq != null) 
{
    oReq.open("GET", url+param, true);
    oReq.send();
    alert(oReq.responseText)
}
else 
    window.alert("AJAX (XMLHTTP) not supported.");
}

function strToUrl(str){return ReplSpaceSymb(escape(str),'+','%2B')}
function ReplSpaceSymb(s,sourceStr,targetStr){
	while (s.indexOf(sourceStr)!=-1)s=s.replace(sourceStr,targetStr)
	return s
}

////////run serverside
// Calculates the distance between two latlng locations in km.
function(p1, p2) {
  if (!p1 || !p2) {
    return 0;
  }

  var R = 6371; // Radius of the Earth in km 6378.137
  var dLat = (p2.lat() - p1.lat()) * Math.PI / 180;
  var dLon = (p2.lng() - p1.lng()) * Math.PI / 180;
  var a = Math.sin(dLat / 2) * Math.sin(dLat / 2) +
    Math.cos(p1.lat() * Math.PI / 180) * Math.cos(p2.lat() * Math.PI / 180) *
    Math.sin(dLon / 2) * Math.sin(dLon / 2);
  var c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));
  var d = R * c;
  return d;
};

/// addContact
function AddContact()
{
if (clickAddContactHandle==null)
  clickAddContactHandle=google.maps.event.addListener(map, 'click', addLatLngMar);
else
    google.maps.event.removeListener(clickAddContactHandle);

}

function addLatLngMar(event)
{
  var marker = new google.maps.Marker({
    position: event.latLng,
    draggable:false,
    map: map
  });
  
  Tag1='?contact='+Tag+'&lat='+String(event.latLng.lat())+'&lon='+String(event.latLng.lng());
  marker.setTitle(String(event.latLng)+' <right click to save selected contact>');
  google.maps.event.addListener(marker, 'rightclick', saveMarker);
map.setCenter(event.latLng);
map.setZoom(15);


if(markersArray.length>0)
{ 
    markersArray[markersArray.length-1].setMap(null);
    markersArray=markersArray.slice(0, -1);
}  
markersArray.push(marker);

}

function saveMarker(event)
{
    if (String(Tag)=='undefined')
      alert('select contact first...')
    else
    {
        var url ="Pincontact.aspx"+Tag1;

//window.open(url)

var xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
xmlhttp.Open("POST",url,false);

	xmlhttp.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
	xmlhttp.send();
    xmlhttp.responseText;
    alert('Done.');

    }
}

function removeStop(RouteID, StopNum)
{
//alert(RouteID);
//alert(StopNum);

var if_to= confirm("Do you really want to remove stop?");
if (if_to== true)
 {
        var url ="RemoveStop.aspx?RouteID="+RouteID+"&Stopnumber="+StopNum;
        var xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
        xmlhttp.Open("POST",url,false);

    	xmlhttp.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
	    xmlhttp.send();
        
        
    var controlText = document.createElement('DIV');
    controlText.style.fontFamily = 'Arial,sans-serif';
    controlText.style.fontSize = '12';
    controlText.style.paddingLeft = '2px';
    controlText.style.paddingRight = '2px';
    controlText.style.paddingTop = '0px';
    controlText.style.paddingBottom = '1px';
    controlText.innerHTML = xmlhttp.responseText;
    if(document.body != null){ document.body.appendChild(controlText); }
        
    
    alert("Done.");
        
        document.Form1.submit();
 }

return
}

function moveStop(RouteID, StopNum)
{
    if(TargetRouteID=='')
      alert('Choose target route first...')
    else
    {
        var if_to;
        if (RouteID==TargetRouteID)
            if_to= confirm("Do you really want resequence route: "+TargetRouteID+"?");
        else
            if_to= confirm("Do you really want to move stop from route "+RouteID+" to route "+TargetRouteID+"?");

        if (if_to== true)
         {
                var url ="MoveStop.aspx?RouteID="+RouteID+"&Stopnumber="+StopNum+"&TargetRouteID="+TargetRouteID;
                var xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
                xmlhttp.Open("POST",url,false);

    	        xmlhttp.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
	            xmlhttp.send();
//                xmlhttp.responseText;
    var controlText = document.createElement('DIV');
    controlText.style.fontFamily = 'Arial,sans-serif';
    controlText.style.fontSize = '12';
    controlText.style.paddingLeft = '2px';
    controlText.style.paddingRight = '2px';
    controlText.style.paddingTop = '0px';
    controlText.style.paddingBottom = '1px';
    controlText.innerHTML = xmlhttp.responseText;
    if(document.body != null){ document.body.appendChild(controlText); }

                alert('Done.');
                
                document.Form1.submit();
         }
    }
    return
}

function resequenceRoute(RouteID, StopNum)
{
    TargetRouteID=RouteID;
    moveStop(RouteID, StopNum);
    return
}


function placeUnrouted(ReqID)
{
    if(TargetRouteID=='')
      alert('Choose target route first...')
    else
    {
        var if_to;
            if_to= confirm("Do you really want to place unrouted requirement to route "+TargetRouteID+"?");

        if (if_to== true)
         {
                var url ="PlaceRequirementtoRoute.aspx?ReqID="+ReqID+"&TargetRouteID="+TargetRouteID;
                var xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
                xmlhttp.Open("POST",url,false);

    	        xmlhttp.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
	            xmlhttp.send();

    var controlText = document.createElement('DIV');
    controlText.style.fontFamily = 'Arial,sans-serif';
    controlText.style.fontSize = '12';
    controlText.style.paddingLeft = '2px';
    controlText.style.paddingRight = '2px';
    controlText.style.paddingTop = '0px';
    controlText.style.paddingBottom = '1px';
    controlText.innerHTML = xmlhttp.responseText;
    if(document.body != null){ document.body.appendChild(controlText); }

                alert('Done.');
                
                document.Form1.submit();
         }
    }
    return
}

var oPopup = window.createPopup();
function openPopup(str)
{
    var oPopBody = oPopup.document.body;
    oPopBody.innerHTML = "<DIV>"+str+"</DIV>"
    oPopup.show(15, 150, 50, 50, document.body);
}



if (TargetRouteID!=''){eval(TargetRouteID+'flightPath').strokeWeight=3;eval(TargetRouteID+'flightPath').strokeColor=TargetRouteColor;eval(TargetRouteID+'flightPath').setMap(map);TargetRouteColor=this.strokeColor;}