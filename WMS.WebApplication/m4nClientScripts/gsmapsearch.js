/**
 * Copyright (c) 2008 Google Inc.
 *
 * You are free to copy and use this sample.
 * License can be found here: http://code.google.com/apis/ajaxsearch/faq/#license
*/

GSmapSearchControl.ACTIVE_MAP_ZOOM = 14;
GSmapSearchControl.IDLE_MAP_ZOOM = 11;
GSmapSearchControl.MAP_TYPE_ENABLE_ALL = 1;
GSmapSearchControl.MAP_TYPE_ENABLE_ACTIVE = 2;
GSmapSearchControl.ZOOM_CONTROL_ENABLE_ALL = 1;
GSmapSearchControl.DEFAULT_RESULT_LIST = 1;
GSmapSearchControl.DRIVING_DIRECTIONS_FROM_CENTER = 1;
GSmapSearchControl.DRIVING_DIRECTIONS_FROM_USER = 2;
function GSmapSearchControl(container, mapCenter, opt_options) {
  this.letteredIconMode = true;
  this.userSelectedDirectionsMode = false;
  this.onBootCompleteHandler = null;

  this.enableResultsContainer = false;
  this.enableIdleZoomControl = false;
  this.enableActiveZoomControl = true;
  this.enableIdleMapType = false;
  this.enableActiveMapType = false;
  this.root = container;
  this.linkTarget = GSearch.LINK_TARGET_BLANK;
  this.ads = null;

  this.buildContainerGuts();

  this.activeMapZoom = GSmapSearchControl.ACTIVE_MAP_ZOOM;
  this.idleMapZoom = GSmapSearchControl.IDLE_MAP_ZOOM;

  // result scroller
  this.currentResultIndex = 0;
  this.markers = new Array();
  this.idle = true;

  // bind up the controls
  this.searchForm.input.onclick = GSmapsc_methodClosure(this, GSmapSearchControl.prototype.onPreActive, []);
  this.searchForm.input.onfocus = GSmapsc_methodClosure(this, GSmapSearchControl.prototype.onPreActive, []);
  this.searchForm.setOnSubmitCallback(this,GSmapSearchControl.prototype.formSubmit);

  this.cancel.onclick = GSmapsc_methodClosure(this, GSmapSearchControl.prototype.goIdle, []);
  this.next.onclick = GSmapsc_methodClosure(this, GSmapSearchControl.prototype.onNext, []);
  this.prev.onclick = GSmapsc_methodClosure(this, GSmapSearchControl.prototype.onPrev, []);

  // build the icons
  this.selectedIcon = new GIcon();
  this.selectedIcon.image = "http://www.google.com/mapfiles/gadget/markerSmall80.png";
  this.selectedIcon.shadow = "http://www.google.com/mapfiles/gadget/shadow50Small80.png";
  this.selectedIcon.iconSize = new GSize(16, 27);
  this.selectedIcon.shadowSize = new GSize(30, 28);
  this.selectedIcon.iconAnchor = new GPoint(8, 27);
  this.selectedIcon.infoWindowAnchor = new GPoint(5, 1);

  this.unselectedIcon = new GIcon(this.selectedIcon);
  this.unselectedIcon.image = "http://www.google.com/mapfiles/gadget/markerSmall80.png";

  this.centerIcon = new GIcon(this.selectedIcon);
  this.centerIcon.image = "http://www.google.com/mapfiles/gadget/arrowSmall80.png";
  this.centerIcon.shadow = "http://www.google.com/mapfiles/gadget/arrowshadowSmall80.png";;
  this.centerIcon.iconSize = new GSize(31, 27);
  this.centerIcon.shadowSize = new GSize(31, 27);
  this.centerIcon.iconAnchor = new GPoint(8, 27);
  this.centerIcon.infoWindowAnchor = new GPoint(5, 1);

  this.letteredIcons = new Array();
  for ( var i=0; i < 16; i++ ) {
    var icon = new GIcon(this.selectedIcon);
    var iconImageKey =
    icon.image = "http://www.google.com/mapfiles/gadget/letters/marker" +
                      String.fromCharCode(65+i) + ".png";
    this.letteredIcons[i] = icon;
  }

  this.mapCenterString = null;
  this.titleOverride = null;

  // handle title and url
  if (opt_options) {
    var title = null;
    var url = null;

    if (opt_options.linkTarget) {
      this.linkTarget = opt_options.linkTarget;
    }

    if (opt_options.title) {
      title = opt_options.title;
    }
    if (opt_options.url) {
      url = opt_options.url;
    }
    if (url && title) {
      var div = GSmapsc_createDiv(null,"gsmsc-user-title");
      var link = GSmapsc_createLink(url, title, this.linkTarget, "gsmsc-user-title");
      div.appendChild(link);
      this.titleOverride = div;
    }

    if (opt_options.hotspots) {
      // whip through the hotspots and bind away...
      for (var i=0; i < opt_options.hotspots.length; i++) {
        var hs = opt_options.hotspots[i];
        hs.element.onclick = GSmapsc_methodClosure(this, GSmapSearchControl.prototype.newSearch, [hs.query]);
      }
    }

    // allow custom center marker
    if (opt_options.centerIcon) {
      this.centerIcon = opt_options.centerIcon;
    }

    // allow custom selected marker
    if (opt_options.selectedIcon) {
      this.selectedIcon = opt_options.selectedIcon;
      this.letteredIconMode = false;
    }

    // allow custom unselected marker
    if (opt_options.unselectedIcon) {
      this.unselectedIcon = opt_options.unselectedIcon;
      this.letteredIconMode = false;
    }

    // allow zoom level specification
    if (opt_options.idleMapZoom) {
      this.idleMapZoom = opt_options.idleMapZoom;
    }

    if (opt_options.activeMapZoom) {
      this.activeMapZoom = opt_options.activeMapZoom;
    }

    if (opt_options.mapTypeControl) {
      if (opt_options.mapTypeControl == GSmapSearchControl.MAP_TYPE_ENABLE_ALL) {
        this.enableIdleMapType = true;
        this.enableActiveMapType = true;
      } else if (opt_options.mapTypeControl == GSmapSearchControl.MAP_TYPE_ENABLE_ACTIVE) {
        this.enableIdleMapType = false;
        this.enableActiveMapType = true;
      } else {
        this.enableIdleMapType = false;
        this.enableActiveMapType = false;
      }
    }

    if (opt_options.zoomControl) {
      if (opt_options.zoomControl == GSmapSearchControl.ZOOM_CONTROL_ENABLE_ALL) {
        this.enableIdleZoomControl = true;
        this.enableActiveZoomControl = true;
      }
    }

    // ads array
    if (opt_options.ads) {
      this.ads = opt_options.ads;
    }

    // results list
    if (opt_options.showResultList) {
      this.resultsList = GSmapsc_createDiv(null, "gsmsc-resultsBox");
      if (opt_options.showResultList == GSmapSearchControl.DEFAULT_RESULT_LIST ) {
        this.appContainer.appendChild(this.resultsList);
      } else {
        GSmapsc_removeChildren(opt_options.showResultList);
        opt_options.showResultList.appendChild(this.resultsList);
      }
    }

    // driving directions
    if (opt_options.drivingDirections) {
      if (opt_options.drivingDirections ==
          GSmapSearchControl.DRIVING_DIRECTIONS_FROM_USER) {
        this.userSelectedDirectionsMode = true;
      } else if (opt_options.drivingDirections ==
                 GSmapSearchControl.DRIVING_DIRECTIONS_FROM_CENTER) {
        this.userSelectedDirectionsMode = false;
      } else {
        this.userSelectedDirectionsMode = false;
      }
    }

  }

  // onBootComplete
  if (opt_options.onBootComplete) {
    this.onBootCompleteHandler = opt_options.onBootComplete;
  }
  // be kind, since typically a lot is going on at load...
  this.startupTimer = setTimeout(GSmapsc_methodClosure(this, GSmapSearchControl.prototype.deferLoad, [mapCenter]), 100);
}

GSmapSearchControl.prototype.deferLoad = function(mapCenter) {
  // allow mapCenter to be a string or a GLatLng
  if ( mapCenter && mapCenter.lat && mapCenter.lng ) {
    var lat;
    var lng;
    if (typeof mapCenter.lat == "function") {
      lat = mapCenter.lat();
    } else {
      lat = mapCenter.lat;
    }
    if (typeof mapCenter.lng == "function" ) {
      lng = mapCenter.lng();
    } else {
      lng = mapCenter.lng;
    }
    this.mapCenterString = lat + ", " + lng;
    this.mapCenter = new GLatLng(lat, lng);
    this.bootComplete(null);
  } else {
    // use ajax search based geocode
    this.mapCenter = null;
    this.mapCenterString = mapCenter;
    var gsLookup = new GlocalSearch();
    gsLookup.setSearchCompleteCallback(this, GSmapSearchControl.prototype.bootComplete, [gsLookup]);
    gsLookup.execute(mapCenter);
  }
}

GSmapSearchControl.prototype.buildContainerGuts = function() {
  // build out the map divs and search form

  GSmapsc_removeChildren(this.root);
  this.appContainer = GSmapsc_createDiv(null, "gsmsc-appContainer");

  this.resultsList = null;

  // active map div
  this.mapDiv = GSmapsc_createDiv(null, "gsmsc-mapDiv");
  this.appContainer.appendChild(this.mapDiv);

  this.attributionDiv = GSmapsc_createDiv(null, "gsmsc-attributionDiv");
  this.appContainer.appendChild(this.attributionDiv);

  // idle map div
  this.idleMapDiv = GSmapsc_createDiv(null, "gsmsc-idleMapDiv");
  this.appContainer.appendChild(this.idleMapDiv);

  var div = GSmapsc_createDiv(null, "gsmsc-controls");
  this.searchForm = new GSearchForm(false, div);

  // controls
  this.prevNext = GSmapsc_createDiv(null, "gsmsc-prevNext");
  this.prev = GSmapsc_createDiv(null, "gsmsc-prev");
  this.cancel = GSmapsc_createDiv(null, "gsmsc-cancel");
  this.next = GSmapsc_createDiv(null, "gsmsc-next");
  this.tooltip = GSmapsc_createDiv(GSearch.strings["scroll-results"],
                                   "gsmsc-tooltip");
  this.prev.innerHTML = "&nbsp;";
  this.cancel.innerHTML = "&nbsp;";
  this.next.innerHTML = "&nbsp;";
  this.prev.title = GSearch.strings["previous"];
  this.cancel.title = GSearch.strings["clear-results"];
  this.next.title = GSearch.strings["next"];
  this.prevNext.appendChild(this.prev);
  this.prevNext.appendChild(this.cancel);
  this.prevNext.appendChild(this.next);
  this.prevNext.appendChild(this.tooltip);
  this.searchForm.userDefinedCell.appendChild(this.prevNext);

  this.appContainer.appendChild(div);

  this.root.appendChild(this.appContainer);
}

// finish boot process by creating and centering our map(s)
GSmapSearchControl.prototype.bootComplete = function(gs) {
  if ( gs && gs.results && gs.results.length > 0 ) {
    // extract map center from first search result
    this.mapCenter = new GLatLng(parseFloat(gs.results[0].lat),
                                 parseFloat(gs.results[0].lng));
    var res0 = gs.results[0];
    var resHtml = res0.html.cloneNode(true);
    var div;
    if (this.titleOverride) {
      div = GSmapsc_createDiv(null, "gsmsc-user-title gsmsc-result-wrapper-user-selected-directions");
      div.appendChild(this.titleOverride);
    } else {
      div = GSmapsc_createDiv(null, "gsmsc-result-wrapper-user-selected-directions");
    }
    div.appendChild(resHtml);
    this.mapCenterHtml = div;
  } else if (this.mapCenter == null) {
    this.mapCenter = new GLatLng(37.421947, -122.084391);
    this.mapCenterString = this.mapCenter.lat() + ", " + this.mapCenter.lng();
  }

  // if we have no html, make it
  if (!this.mapCenterHtml) {
    var div;
    if (this.titleOverride) {
      div = GSmapsc_createDiv(null, "gsmsc-user-title");
      div.appendChild(this.titleOverride);
    } else {
      div = GSmapsc_createDiv(this.mapCenterString, "gsmsc-user-title");
    }
    // from user-prompt To Here
    var toUrl = "http://www.google.com/maps?source=uds&daddr=" +
      this.mapCenterString + "&iwstate1=dir:to";
    var fromUrl = "http://www.google.com/maps?source=uds&saddr=" +
      this.mapCenterString + "&iwstate1=dir:from";
    var dwrap = GSmapsc_createDiv(null, "gsmsc-directions-wrapper");
    var ditem = GSmapsc_createDiv(GSearch.strings["get-directions"]+":",
                                  "gsmsc-directions-label");
    dwrap.appendChild(ditem);

    // to here
    var ditem = GSmapsc_createLink(toUrl, GSearch.strings["to-here"], this.linkTarget,
                                   "gsmsc-directions-link");
    dwrap.appendChild(ditem);

    // spacer
    var ditem = GSmapsc_createDiv("-", "gsmsc-directions-spacer");
    dwrap.appendChild(ditem);

    // from here
    var ditem = GSmapsc_createLink(fromUrl, GSearch.strings["from-here"], this.linkTarget,
                                   "gsmsc-directions-link");
    dwrap.appendChild(ditem);

    div.appendChild(dwrap);
    this.mapCenterHtml = div;
  }

  // create the maps and add conditional controls
  this.gmap = new GMap2(this.mapDiv);
  if (this.enableActiveZoomControl) {
    this.gmap.addControl(new GSmallMapControl());
  }
  if (this.enableActiveMapType) {
    this.gmap.addControl(new GMapTypeControl());
  }
  this.gmap.setCenter(this.mapCenter, this.activeMapZoom);

  this.idleGmap = new GMap2(this.idleMapDiv);
  if (this.enableIdleZoomControl) {
    this.idleGmap.addControl(new GSmallMapControl());
  }
  if (this.enableIdleMapType) {
    this.idleGmap.addControl(new GMapTypeControl());
  }
  this.idleGmap.setCenter(this.mapCenter, this.idleMapZoom);

  // create a searcher and bind to the map
  this.gs = new GlocalSearch();
  this.gs.setResultSetSize(GSearch.LARGE_RESULTSET);
  this.gs.setLinkTarget(this.linkTarget);
  this.gs.setCenterPoint(this.mapCenter);
  this.gs.setSearchCompleteCallback(this, GSmapSearchControl.prototype.searchComplete, [null]);
  if (this.ads) {
    for (var i=0; i<this.ads.length; i++) {
      this.gs.addRelatedSearcher(this.ads[i]);
    }
  }

  this.idleMarker = new GMarker(this.mapCenter, this.centerIcon);
  this.mapCenterMarker = new GMarker(this.mapCenter, this.centerIcon);
  GEvent.bind(this.mapCenterMarker, "click", this, this.onCenterClick);
  GEvent.bind(this.idleMarker, "click", this, this.onIdleCenterClick);
  this.gmap.addOverlay(this.mapCenterMarker);
  this.idleGmap.addOverlay(this.idleMarker);

  GEvent.bind(this.gmap, "click", this, this.onMapClick);

  this.goIdle();
  if (this.onBootCompleteHandler) {
    this.onBootCompleteHandler(this);
  }
}

// show the center of the map
GSmapSearchControl.prototype.onCenterClick = function() {
  this.mapCenterMarker.openInfoWindow(this.mapCenterHtml, {maxWidth:200});
}

// show the center of the map
GSmapSearchControl.prototype.onIdleCenterClick = function() {

  // transition to semi-active state and open the center info window
  this.clearMarkers();
  GSmapsc_cssSetClass(this.prevNext, "gsmc-prevNext gsmsc-prev-next-active");
  GSmapsc_cssSetClass(this.appContainer, "gsmsc-appContainer gsmsc-active");
  GSmapsc_cssSetClass(this.prev, "gsmsc-prev gsmsc-prev-idle");
  GSmapsc_cssSetClass(this.next, "gsmsc-next gsmsc-next-idle");
  this.searchForm.input.value = "";
  this.gmap.checkResize();
  this.idle = false;

  this.mapCenterMarker.openInfoWindow(this.mapCenterHtml, {maxWidth:200});
}

// clear the old markers off of the map
GSmapSearchControl.prototype.clearMarkers = function() {
  GSmapsc_cssSetClass(this.prevNext, "gsmc-prevNext gsmsc-prev-next-idle");

  this.gmap.closeInfoWindow();
  for (var i=0; i < this.markers.length; i++) {
    this.gmap.removeOverlay(this.markers[i].marker);
    this.markers[i].resultsListItem = null;
  }

  // clear results list if present
  if (this.resultsList) {
    GSmapsc_removeChildren(this.resultsList);
  }

  // result scroller
  this.currentResultIndex = 0;
  this.markers = new Array();
}

// drop the new markers on the map
GSmapSearchControl.prototype.setMarkers = function() {

  // result scroller
  this.currentResultIndex = 0;
  this.markers = new Array();
  var bestResultUrl = null;

  if ( this.gs.results && this.gs.results.length > 0) {
    for (var i = 0; i < this.gs.results.length && i < 16; i++) {
      var result = this.gs.results[i];
      var icon = this.unselectedIcon;
      if (this.letteredIconMode) {
        icon = this.letteredIcons[i];
      }
      this.markers.push(new GSmapscLocalResult(this, result, icon, i));

      // find the first, non-address result and hold on to it for the more-link
      if (bestResultUrl == null && !result.addressLookupResult ) {
        bestResultUrl = result.url;
      }
    }
    GSmapsc_cssSetClass(this.prevNext, "gsmc-prevNext gsmsc-prev-next-active");
    GSmapsc_cssSetClass(this.appContainer, "gsmsc-appContainer gsmsc-active");
    this.gmap.checkResize();
    this.idle = false;
    this.selectMarker(0);

    // stuff in a more results link
    if (this.resultsList) {
      if (bestResultUrl) {
        // NOW, take the URL and nuke from &latlnt.*&near ->&near
        var newUrl = bestResultUrl.replace(/&latlng=.*&near/,"&near");
        var moreDiv = GSmapsc_createDiv(null, "gsmsc-result-list-more-results");
        var alink = GSmapsc_createLink(newUrl, GSearch.strings["more-results"],
                                       this.linkTarget, "gsmsc-result-list-more-results");
        moreDiv.appendChild(alink);

        var clearDiv = GSmapsc_createDiv(GSearch.strings["clear-results-uc"],
                                "gsmsc-result-list-clear-results");
        clearDiv.onclick = GSmapsc_methodClosure(this,
                                            GSmapSearchControl.prototype.goIdle,
                                            []);
        // create a table for these to sit within
        var table = GSmapsc_createTable("gsmsc-result-controls");
        var row = GSmapsc_createTableRow(table);
        var moreTd = GSmapsc_createTableCell(
                        row, "gsmsc-result-list-more-results");
        var clearTd = GSmapsc_createTableCell(
                        row, "gsmsc-result-list-clear-results");
        moreTd.appendChild(moreDiv);
        clearTd.appendChild(clearDiv);
        this.resultsList.appendChild(table);
      }
    }
  }
}

// light up the selected marker
GSmapSearchControl.prototype.selectMarker = function(index) {

  // clear info window and reset icon on current marker
  this.gmap.closeInfoWindow();

  var icon = this.unselectedIcon;
  if (this.letteredIconMode) {
    icon = this.letteredIcons[this.currentResultIndex];
  }
  this.markers[this.currentResultIndex].setMarker(icon);

  // if we have a results list, clear selected
  if (this.markers[this.currentResultIndex].resultsListItem) {
    GSmapsc_cssSetClass(this.markers[this.currentResultIndex].resultsListItem,
                        "gsmsc-result-list-item");
  }

  // snap to current
  this.currentResultIndex = index;

  // light up current
  var result = this.markers[this.currentResultIndex];

  if (result.resultsListItem) {
    GSmapsc_cssSetClass(result.resultsListItem,
                        "gsmsc-result-list-item gsmsc-selected");
  }
  var icon = this.selectedIcon;
  if (this.letteredIconMode) {
    icon = this.letteredIcons[this.currentResultIndex];
  }
  result.setMarker(icon);
  result.marker.openInfoWindow(result.getHtml(), {maxWidth:200});

  // set scroller
  if (index == 0) {
    GSmapsc_cssSetClass(this.prev, "gsmsc-prev gsmsc-prev-idle");
  } else {
    GSmapsc_cssSetClass(this.prev, "gsmsc-prev gsmsc-prev-active");
  }

  if (index == this.markers.length - 1) {
    GSmapsc_cssSetClass(this.next, "gsmsc-next gsmsc-next-idle");
  } else {
    GSmapsc_cssSetClass(this.next, "gsmsc-next gsmsc-next-active");
  }
}

// clear current markers and start a new search
GSmapSearchControl.prototype.formSubmit = function(form) {
  if (form.input.value) {
    this.newSearch(form.input.value);
  }
  return false;
}

// clear current markers and start a new search
GSmapSearchControl.prototype.execute = function(opt_query) {
  // hyperlink friendly...
  this.newSearch(opt_query);
}

GSmapSearchControl.prototype.newSearch = function(opt_query) {
  if (opt_query) {
    this.searchForm.input.value  = opt_query;
  }
  if (this.searchForm.input.value) {

    // clear markers, set prev/next
    this.clearMarkers();
    GSmapsc_removeChildren(this.attributionDiv);
    this.gs.execute(this.searchForm.input.value);
  }
  return false;
}

GSmapSearchControl.prototype.searchComplete = function() {
  var attribution = this.gs.getAttribution();
  if (attribution) {
    this.attributionDiv.appendChild(attribution);
  }
  this.setMarkers();
}

// forwards through the search results
GSmapSearchControl.prototype.onNext = function() {
  if (this.currentResultIndex < this.markers.length - 1) {
    this.selectMarker(this.currentResultIndex+1);
  }
}

// backwards through the search results
GSmapSearchControl.prototype.onPrev = function() {
  if (this.currentResultIndex > 0) {
    this.selectMarker(this.currentResultIndex-1);
  }
}

// called onboot complete, and on cancel click
GSmapSearchControl.prototype.goIdle = function() {
  this.searchForm.input.value = GSearch.strings["search-the-map"];
  this.gmap.setCenter(this.mapCenter, this.activeMapZoom);
  this.idleGmap.setCenter(this.gmap.getCenter(), this.idleMapZoom);
  GSmapsc_cssSetClass(this.appContainer, "gsmsc-appContainer gsmsc-idle");
  GSmapsc_cssSetClass(this.prevNext, "gsmc-prevNext gsmsc-prev-next-idle");
  this.idleGmap.checkResize();
  this.idle = true;

  // if we are doing resultsList, clear...
  if (this.resultsList) {
    GSmapsc_removeChildren(this.resultsList);
  }
}

// call onfocus/onclick for search input cell
GSmapSearchControl.prototype.onPreActive = function() {
  if (this.idle) {
    this.searchForm.input.value = "";
  }
}

GSmapSearchControl.prototype.onMapClick = function(marker, point) {
  if (marker && marker.__ls__) {
    var localResult = marker.__ls__;
    localResult.onClick();
  }
}

// A class representing a single Local Search result returned by the
// Google AJAX Search API.
function GSmapscLocalResult(gsmsc, result, icon, index) {
  this.gsmsc = gsmsc;
  this.result = result;
  this.latLng = new GLatLng(parseFloat(result.lat), parseFloat(result.lng));
  this.index = index;
  this.setMarker(icon);

  if (gsmsc.resultsList) {
    var div = GSmapsc_createDiv(null, "gsmsc-result-list-item");
    if (gsmsc.letteredIconMode) {
      var key = GSmapsc_createDiv("(" + String.fromCharCode(65+index) + ")",
                                  "gsmsc-result-list-item-key");
      div.appendChild(key);
    }
    var tdiv = GSmapsc_createDiv(result.title, "gs-title");
    div.appendChild(tdiv);
    if (!result.addressLookupResult) {
      if (result.streetAddress && result.streetAddress != "") {
        var str = "&nbsp;-&nbsp;" + result.streetAddress;
        var tdiv = GSmapsc_createDiv(str, "gs-street");
        div.appendChild(tdiv);
      }
    }
    div.onclick = GSmapsc_methodClosure(
                    gsmsc, GSmapSearchControl.prototype.selectMarker, [index]);
    this.resultsListItem = div;
    gsmsc.resultsList.appendChild(div)
  } else {
    this.resultsListItem = null;
  }
}

GSmapscLocalResult.prototype.getHtml = function() {
  var wrapperClass = "gsmsc-result-wrapper";
  if (this.gsmsc.userSelectedDirectionsMode) {
    wrapperClass += " gsmsc-result-wrapper-user-selected-directions";
  }
  var result = GSmapsc_createDiv(null, wrapperClass);
  var node = this.result.html.cloneNode(true);
  result.appendChild(node);

  return result;
}

GSmapscLocalResult.prototype.setMarker = function(icon) {
  if (this.marker) {
    this.gsmsc.gmap.removeOverlay(this.marker);
    this.marker.__ls__ = null;
    var marker = this.marker;
    this.marker = null;
    delete(marker);
  }
  this.marker = new GMarker(this.latLng, icon);
  this.marker.__ls__ = this;
  this.gsmsc.gmap.addOverlay(this.marker);
}

GSmapscLocalResult.prototype.onClick = function() {
  this.gsmsc.selectMarker(this.index);
}


/**
 * Various Static DOM Wrappers.
*/
function GSmapsc_methodClosure(object, method, opt_argArray) {
  return function() {
    return method.apply(object, opt_argArray);
  }
}

function GSmapsc_methodCallback(object, method) {
  return function() {
    return method.apply(object, arguments);
  }
}

function GSmapsc_createDiv(opt_text, opt_className) {
  var el = document.createElement("div");
  if (opt_text) {
    el.innerHTML = opt_text;
  }
  if (opt_className) { el.className = opt_className; }
  return el;
}

function GSmapsc_removeChildren(parent) {
  while (parent.firstChild) {
    parent.removeChild(parent.firstChild);
  }
}

function GSmapsc_cssSetClass(el, className) {
  el.className = className;
}


function GSmapsc_createForm(opt_className) {
  var el = document.createElement("form");
  if (opt_className) { el.className = opt_className; }
  return el;
}

function GSmapsc_createTable(opt_className) {
  var el = document.createElement("table");
  if (opt_className) { el.className = opt_className; }
  return el;
}

function GSmapsc_createTableRow(table) {
  var tr = table.insertRow(-1);
  return tr;
}

function GSmapsc_createTableCell(tr, opt_className) {
  var td = tr.insertCell(-1);
  if (opt_className) { td.className = opt_className; }
  return td;
}

function GSmapsc_createTextInput(opt_className) {
  var el = document.createElement("input");
  el.type = "text";
  if (opt_className) { el.className = opt_className; }
  return el;
}

function GSmapsc_createLink(href, text, opt_target, opt_className) {
  var el = document.createElement("a");
  el.href = href;
  el.appendChild(document.createTextNode(text));
  if (opt_className) {
    el.className = opt_className;
  }
  if (opt_target) {
    el.target = opt_target;
  }
  return el;
}
