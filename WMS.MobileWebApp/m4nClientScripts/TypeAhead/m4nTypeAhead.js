/*
Made4Net Version: 2.4

This object contains the logic for TypeAhead Display Type
*/

/****************************************************************************************/

function m4nTypeAhead(elmnt) {
	//member vars
	this.auto_expand;
	this.connection;
	this.extra_fields;
	this.filter;
	this.limit_to_list;
	this.sql;
	this.table;
	this.text_field;
	this.value_field;
	this.record_limit;
	this.minimum_chars;
	this.data_template;
	this.value_list;

	this.element;
	this.data_provider_url;
	this.field_name;
	this._hasErrors = false;
	this._formParams = null;
	this.parent_te;
	
	this.bVer;
	this.bName;
	this.BrowserDetect;
	
	//public properties
	this.GetHasErrors = taGetHasErrors;
	this.GetFormParams = taGetFormParams;

	//public methods
	this.GetFieldNames = taGetFieldNames;
	this.GetData = taGetData;
	this.GetDataByValueField = taGetDataByValueField;
	this.GetFormParams = taGetFormParams;
	this.GetFormParamValue = taGetFormParamValue;
	this.GetFormParamObject = taGetFormParamObject;
	
	//private methods
	this.GetFieldNamesAsString = taGetFieldNamesAsString;
	this.ValidateMembers = taValidateMembers;
	this.ThrowPrEx = taThrowPropertyException;
	this.ThrowNotSupported = taThrowNotSupported;
	this.GetDataCommon = taGetDataCommon;
	this.ShowError = taShowError;
	this.CreateParam = taCreateParam;
	this.GetURLParams = taGetURLParams;
	this.SetFormParams = taSetFormParams;
	this.GetFormParamsAndValues = taGetFormParamsAndValues;
	this.GetFormParamFullName = taGetFormParamFullName;
	this.GetFormParamTE = taGetFormParamTE;
	this.GetFormParamField = taGetFormParamField;
	this.EscapeSquareBrackets = taEscapeSquareBrackets;
	this.GetElementProperty = taGetElementProperty;
	
	this.AttachEvents = taAttachEvents;
	
	this.BrowserDetection = taBrowserDetection;
 
	//constructor
	this.BrowserDetection();
	this.BrowserDetect.init();
    this.bName = this.BrowserDetect.browser;
    this.bVer = parseInt(this.BrowserDetect.version);
	this.element				= elmnt;
	this.parent_te				= m4nGetParentTEObjectID(this.element);
	
	this.auto_expand			= this.GetElementProperty("auto_expand");
	this.connection				= this.GetElementProperty("connection");
	this.extra_fields			= m4nTrim(this.GetElementProperty("extra_fields"));
	this.filter					= this.GetElementProperty("filter");
	this.limit_to_list			= this.GetElementProperty("limit_to_list");
	this.sql					= this.GetElementProperty("sql");
	this.table					= this.GetElementProperty("table");
	this.text_field				= m4nTrim(this.GetElementProperty("text_field"));
	this.value_field			= m4nTrim(this.GetElementProperty("value_field"));
	this.record_limit			= this.GetElementProperty("record_limit");
	this.minimum_chars			= this.GetElementProperty("minimum_chars");
	this.data_template			= this.GetElementProperty("data_template");
	this.value_list				= this.GetElementProperty("value_list");
	this.readonly				= this.GetElementProperty("readonly");
	
	this.data_provider_url		= this.GetElementProperty("data_provider_url");
	this.field_name				= this.GetElementProperty("field_name");
	this.display_type_name		= this.GetElementProperty("display_type_name");
	
	try {
		this.ValidateMembers();
	} catch (e) {
		this.ShowError(e);
		return;
	}
	
	this.SetFormParams();
	this.AttachEvents();
	
}

/****************************************************************************************/

/* Properties */

/****************************************************************************************/

function taGetHasErrors() {
	return this._hasErrors;
}

function taGetFormParams() {
	return this._formParams;
}

function taGetElementProperty(name) {
    if (this.bName == "Explorer" && this.bVer > 8)
        if (this.element.attributes[name] == undefined)
            return '';
        else 
            return this.element.attributes[name].value;
    else
	    if (eval('this.element.' + name) == undefined)
		    return '';
	    else
		    return eval('this.element.' + name);
	
}

/****************************************************************************************/

/* Public Methods */

/****************************************************************************************/

function taGetData(searchValue) {
	return this.GetDataCommon(searchValue,"text")
}

function taGetDataByValueField(searchValue) {
	return this.GetDataCommon(searchValue,"value")
}

function taGetFieldNames() {
	return this.GetFieldNamesAsString().split(",");
}

/****************************************************************************************/

/* Private Methods */

/****************************************************************************************/

function taEscapeSquareBrackets(s) {
	while(s.indexOf("[") != -1) {
		s = s.replace("[","^^^");
		//if (!confirm(s)) return;
	}
	while(s.indexOf("^^^") != -1) {
		s = s.replace("^^^","[[]");
		//if (!confirm(s)) return;
	}
	return s;
}

function taGetDataCommon(searchValue,field) {
	var pRand			= this.CreateParam("p_rand",			Math.random().toString().substr(3));
	var pConnection		= this.CreateParam("p_connection",		this.connection);
	var pSearchValue	= this.CreateParam("p_search_value",	this.EscapeSquareBrackets(searchValue));
	var pSearchField	= this.CreateParam("p_search_field",	(field == "text" ? this.text_field : this.value_field))
	var pFields			= this.CreateParam("p_fields",			this.GetFieldNamesAsString());
	var pTable			= this.CreateParam("p_table",			this.table);
	var pRecordLimit	= this.CreateParam("p_record_limit",	this.record_limit);
	var pOrderBy		= this.CreateParam("p_order_by",		this.text_field);
	var pFilter			= this.CreateParam("p_filter",			this.filter);
	var pParentTE		= this.CreateParam("p_parent_te",		this.parent_te);

	var amp = "&";
	var DataToSend = this.GetURLParams() + amp + this.GetFormParamsAndValues() + amp + pRand + amp + pConnection + amp + pSearchValue + amp + pSearchField + amp + pFields + amp + pTable + amp + pRecordLimit + amp + pOrderBy + amp + pFilter + amp + pParentTE;

	//create post request
	var xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
	xmlhttp.Open("POST", this.data_provider_url, false);
	xmlhttp.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
	xmlhttp.send(DataToSend);

	//create XML object
	DOM = new ActiveXObject("MSXML.DOMDocument");
	DOM.async = false;
	DOM.loadXML(xmlhttp.responseText);

	if (location.search.toUpperCase().indexOf("TYPEAHEADDEBUG") > -1) {
		this.ShowError(xmlhttp.responseText);
	}

	var nodes = DOM.selectNodes("Error");
	if (nodes.length>0) {
		this.ShowError("An error occured during the request:\r\n\r\n" + nodes.item(0).text);
	}

	return DOM;
}

function taCreateParam(paramName,value) {
	var param = paramName + "=" + escape(value)
	return param;
}

function taShowError(msg) {
	this._hasErrors = true;
	alert(msg);
}

function taGetFieldNamesAsString() {
	var fnames;
	fnames = this.value_field
	if (this.value_field != this.text_field) {
		fnames += "," + this.text_field;
	}
	if (this.extra_fields != null && this.extra_fields.length>0) {
		fnames += "," + this.extra_fields;
	}
	return fnames;
}

function taValidateMembers() {
	if (this.auto_expand == null || this.auto_expand.toUpperCase() == "TRUE" || this.auto_expand.toUpperCase() == "FALSE") {
	} else {
		this.auto_expand = 'False';
	}
	
	if (this.limit_to_list == null || this.limit_to_list.toUpperCase() == "TRUE" || this.limit_to_list.toUpperCase() == "FALSE") {
	} else {
		this.limit_to_list = 'True';
	}
	if (this.field_name == null || this.field_name.length==0) this.ThrowPrEx("'field_name' property was not set.");
	if (this.table == null || this.table.length==0) this.ThrowPrEx("'table' property was not set.");
	if (this.text_field == null || this.text_field.length==0) this.ThrowPrEx("'text_field' property was not set.");
	if (this.value_field == null || this.value_field.length==0) this.ThrowPrEx("'value_field' property was not set.");
	try {
		this.record_limit = parseInt(this.record_limit);
	} catch (e) {
		this.ThrowPrEx("'record_limit' property must be an integer.");
	}
	if (this.record_limit < 1) this.ThrowPrEx("'record_limit' must be positive.");

	try {
		this.minimum_chars = parseInt(this.minimum_chars);
	} catch (e) {
		this.ThrowPrEx("'minimum_chars' property must be an integer.");
	}
	if (this.minimum_chars < 1) this.ThrowPrEx("'minimum_chars' must be positive.");
			
	this.ThrowNotSupported("sql",this.sql);
	this.ThrowNotSupported("data_template",this.data_template);
	this.ThrowNotSupported("value_list",this.value_list);
	
	if (this.data_template != null) {
		if (this.data_template.length > 0) this.ThrowPrEx("'data_template' property is not currently supported.");
	}
	if (this.sql != null) {
		if (this.sql.length > 0) this.ThrowPrEx("'sql' property is not currently supported.");
	}
}

function taThrowPropertyException(str) {
	this.ShowError("Error in " + this.field_name + " field: " + str);
}

function taThrowNotSupported(propName, value) {
	if (value != null) {
		if (value.length > 0) this.ThrowPrEx("'" + propName + "' property is not currently supported.");
	}
}

function taGetURLParams() {
	//get query string with "urlparam_" prefix for each parameter
	var result = "";
	var query = window.location.search.substring(1);
	var vars = query.split("&");
	for (var i=0; i<vars.length; i++) {
		var pair = vars[i].split("=");
		if (pair.length == 2) {
			if (result.length>0) result += "&";
			result += "urlparam_" + pair[0] + "=" + pair[1];
		}
	}
	return result;
}

function taGetFormParamsAndValues() {
	if (this._formParams == null || this._formParams.length == 0) return "";
	
	var param; var result=""; var paramVal; var i=0;
	for (;;) {
		param = "form_" + this.GetFormParamFullName(i);
		paramVal = this.GetFormParamValue(i)
		param += "=" + escape(paramVal)
		if (result.length>0) result += "&";
		result += param;
		i++;
		if (i > this._formParams.length-1) break;
	}
	return result;
}

function taGetFormParamFullName(index) {
	return this.GetFormParamTE(index) + "." +  this.GetFormParamField(index)
}

function taGetFormParamTE(index) {
	return this._formParams[index]["TE"];
}

function taGetFormParamField(index) {
	return this._formParams[index]["Field"]
}

function taGetFormParamValue(index) {
	var TE = this.GetFormParamTE(index);
	var field = this.GetFormParamField(index);
	var val = m4nGetDisplayTypeValue(TE,field);
	return val;
}

function taGetFormParamObject(index) {
	var TE = this.GetFormParamTE(index);
	var field = this.GetFormParamField(index);
	var obj = m4nGetDisplayTypeObject(TE,field);
	return obj;
}

//create an array of all form params
function taSetFormParams() {
	if (this.filter == null) return "";
	if (this.filter.length == 0) return "";
	
	var params = this.filter.split(";");

	//remove first element
	params.reverse();
	params.pop();
	params.reverse();
	
	if (params.length == 0) return "";
	
	var formParams = new Array(); var param;
	
	for (i=0; i<=params.length-1; i++) {
		param = params[i];
	
		var pair = param.split(":");
		if (pair.length != 2) continue;
		
		var paramType = m4nTrim(pair[0]);
		var paramName = m4nTrim(pair[1]);
		
		if (paramType.toUpperCase() != "FIELD") continue;

		var arrParamName = paramName.split(".");
		if (arrParamName.length != 2 && arrParamName.length != 1) continue;
		
		var formParam = new Array();
		var paramTE;
		var paramFld;
		
		if (arrParamName.length == 2) {
			paramTE = arrParamName[0];
			paramFld = arrParamName[1];
		} else {
			paramTE = this.parent_te;
			paramFld = arrParamName[0];
		}
		formParam["TE"] = paramTE;
		formParam["Field"] = paramFld;

		//add the param to the array
		formParams[formParams.length] = formParam;
	}
	this._formParams = formParams;
}

function taAttachEvents() {
	if (this._formParams == null) return;

	var obj;		
}
function taBrowserDetection()
 {
 this.BrowserDetect = {
	init: function () {
	    this.browser = this.searchString(this.dataBrowser) || "An unknown browser";
		this.version = this.searchVersion(navigator.userAgent)
			|| this.searchVersion(navigator.appVersion)
			|| "an unknown version";
		this.OS = this.searchString(this.dataOS) || "an unknown OS";
	},
	searchString: function (data) {
		for (var i=0;i<data.length;i++)	{
			var dataString = data[i].string;
			var dataProp = data[i].prop;
			this.versionSearchString = data[i].versionSearch || data[i].identity;
			if (dataString) {
				if (dataString.indexOf(data[i].subString) != -1)
					return data[i].identity;
			}
			else if (dataProp)
				return data[i].identity;
		}
	},
	searchVersion: function (dataString) {
		var index = dataString.indexOf(this.versionSearchString);
		if (index == -1) return;
		return parseFloat(dataString.substring(index+this.versionSearchString.length+1));
	},
	dataBrowser: [
		{
			string: navigator.userAgent,
			subString: "MSIE",
			identity: "Explorer",
			versionSearch: "MSIE"
		}
	],
	dataOS : [
		{
			string: navigator.platform,
			subString: "Win",
			identity: "Windows"
		}
	]

}
}