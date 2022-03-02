/*
Made4Net Version: 1.7

This object adds expression calculations to fields on client side
*/

/****************************************************************************************/

/* Main Class */

function m4nExpression(ctrlId, exp, propertyName)
{
	//syntax for ctrlId is either "DataObjectName:FieldName" or just the control id
	
	if (m4nIsEmpty(ctrlId))
	{
		throw new m4nArgumentNullException("ctrlId");
	}
	if (m4nIsEmpty(exp))
	{
		throw new m4nArgumentNullException("exp");
	}
	if (m4nIsEmpty(propertyName))
	{
		throw new m4nArgumentNullException("propertyName");
	}

	this.appliedControlId = ctrlId;
	this.appliedControl;
	this.defaultTE;
	this.expression = exp;
	this.propertyName = propertyName;
	this.defaultEvent = "onchange";
	this.remoteData;
	
	this.onRemoteDataReceived = null;

	this.attachedCtrls = new Array();
	
	this.appliedControl = this.getControl(this.appliedControlId);
	this.defaultTE = this.appliedControl.TE;
}

/****************************************************************************************/

/* public methods */

//attach an event to a control that affects
//the control being calculated
m4nExpression.prototype.attachControlEvent = function(fieldEvent) {
	if (m4nIsEmpty(fieldEvent))
	{
		throw new m4nArgumentNullException("fieldEvent");
	}

	var fieldName, eventName;
	var parts = fieldEvent.split(":");
	if (parts.length == 1) {
		fieldName = m4nTrim(parts[0]);
		eventName = this.defaultEvent;
	} else if (parts.length == 2) {
		fieldName = m4nTrim(parts[0]);
		eventName = m4nTrim(parts[1]);
	}

	if (parts.length > 2 || fieldName == null || fieldName.length == 0) {
		throw new m4nExpressionException("(12) Event definition '" + fieldEvent + "' is invalid. Expecting 'fieldname:event' or 'fieldname'.");
	}
	var dispType;
	var ctrl;
    if (fieldName=='window') {
        ctrl=window;
    } else {
        dispType = this.getControl(this.defaultTE + ":" + fieldName);
	    ctrl = dispType.getValueControl();
    }
	var instance = this;
	var lSuccess;

	//if (eventName == "onpropertychange") eventName = "onchange";
	
	
	var handler = function() { expAttachedCtrlEventHandler(instance) };
	lSuccess = ctrl.attachEvent(eventName, handler);
	if (!lSuccess) {
		throw new m4nExpressionException("(5) Cannot attach to " + eventName + " event of " + ctrl.id + ".");
	}
	m4nAddPurgeElement(ctrl);

	this.attachedCtrls[this.attachedCtrls.length] = ctrl;
}

//calculates the expression by calling the internal calculation function
m4nExpression.prototype.recalc = function ()
{
	return this.calcExpression(this.expression);
}
/****************************************************************************************/

/* private methods */

//returns a string representing the type of a control
m4nExpression.prototype.getControlType = function(ctrl) {
	if (ctrl==null) {
		throw new m4nArgumentNullException("ctrl")
	}
	if (ctrl.tagName.toLowerCase() == 'input')
		return ctrl.type.toLowerCase();
	else
		return ctrl.tagName.toLowerCase();
}

//calculates the expression
m4nExpression.prototype.calcExpression = function(exp) {
	
	document.body.style.cursor = "wait";

	//try
	//{
		var expArr = exp.split(';');
		var jsexp = null; //the js expression part
		var runEval = true;
		if (expArr.length > 1) {
			jsexp = expArr.shift();
		} else {
			jsexp = "[0]";
			//result should not be parsed with eval() method
			runEval = false;
		}
		var paramArr = expArr;
		
		var n,val,paramParts,paramType,paramText;
		var hasServerSideParams,paramEvaluated;
		
		hasServerSideParams = false;
		
		for (n=0;n<paramArr.length;n++)
		{
			paramParts = paramArr[n].split(':');
			paramType = m4nTrim(paramParts[0].toLowerCase());
			paramText = m4nTrim(paramParts[1]);

			if (paramType == "field") {
				var im = new m4nDisplayTypeInstanceManager(this.defaultTE + ":" + paramText);
				var dType = im.getInstance();
				val = dType.getValue();
				if (val == null || val.length == 0) val = '';
				paramEvaluated = true;

			} else if (paramType == "url" ) {
				//get value from URL
				var urlParams = m4nGetURLParams();
				val = urlParams[paramText];
				paramEvaluated = true;
			
			} else if (paramType == "func" || paramType == "session" || paramType == "application") {
				hasServerSideParams = true;
				paramEvaluated = false;
		
			} else {
				val = paramArr[n];
				paramEvaluated = true;
			}
			
			if (paramEvaluated) {
				//replace the param with the value
				paramArr[n] = val;
				//replace placeholder in the js expression
				var ph = '[' + (n) + ']';
				while(jsexp.indexOf(ph) >= 0)
				{
					jsexp = jsexp.replace(ph, val);
				}
			}
		}
		var result;
		if (hasServerSideParams) {
			result = this.serverSideEval(jsexp + ";" + paramArr.join(";"));
		} else {
			try {
				if (runEval)
				{
					result = eval(jsexp);
					
					//isNaN returns false for white space strings
					var strResult = m4nTrim(result);
					if (strResult != '')
					{
						if (!isNaN(result))
						{
							//prevent conversion of exponential representation to string 
							var num = new Number(result);
							result = num.toFixed(20);
							result *= 1;
						}
					}
				}
				else
				{
					result = jsexp;
				}
				
			} catch (e) {
				result = jsexp;
			}
		}
	//}
	//finally
	//{
		document.body.style.cursor = "default";
	//}
	return result;
}

//returns a display type object or a control
m4nExpression.prototype.getControl = function(id) {
	id = m4nTrim(id);
	var ctrl;

	var ic = new m4nDisplayTypeInstanceManager(id);
	ctrl = ic.getInstance();
	
/*	ctrl = document.getElementById(id);
	if (ctrl == null) {
		alert("(10) Cannot find control '" + id + "'.");
		return null;
	}
	if (ctrl.ValueID) {
		ctrl = document.getElementById(ctrl.ValueID);
		if (ctrl == null) {
			alert("(11) Cannot find control '" + ctrl.ValueID + "'.");
		}
	}*/
	return ctrl;
}

//handles a change event from a field that changes the calculation
function expAttachedCtrlEventHandler(instance) {
	var expInstance = instance;
	var src = event.srcElement;

	try
	{
		if (expInstance.appliedControl.userDataEntryMode) return;
	}
	catch(e){}

	var result;
	result = expInstance.recalc();
	expInstance.appliedControl.setProperty(expInstance.propertyName, result);
}

//returns the value of a control
m4nExpression.prototype.controlValue = function(ctrl) {
	try {
		return ctrl.getValue();
	} catch(e) {}
	
	if (ctrl.value == undefined) {
		var retdata = this.stripHtml(ctrl.innerHTML);
		return retdata;
	} else {
		return ctrl.value;
	}
}

//invokes a server-side request for calculating an expression
m4nExpression.prototype.serverSideEval = function(exp) {
	exp = this.parseServerSideFunction(exp);

	//create post request
	
	//encode plus signs
	var patt = /\+/g;
	exp = exp.replace(patt, "%2B");
	
	var dataToSend = "expression=" + exp;

	var ssr = new m4nServerSideRequest(m4nExpressionProviderURL, dataToSend);
	this.remoteData = ssr.getResponseAsText();
	var XmlDoc = ssr.getResponseAsXMLDoc(); 
	
	var nodes = XmlDoc.selectNodes("Error");
	if (nodes.length>0) {
		alert("An error occured during the request:\r\n\r\n" + nodes.item(0).text);
		return;
	}

	if (this.onRemoteDataReceived != null && typeof this.onRemoteDataReceived == "function") {
		this.onRemoteDataReceived();
	}

	if (location.search.toUpperCase().indexOf("EXPRESSIONDEBUG=1") > -1) {
		alert(ssr.responseText);
	}
	
	var resultNode = XmlDoc.selectNodes("Root/result");
	if (resultNode.length == 0) {
		alert("The result element of the server side function response was not found.\n\nThe result is:\n\n" + ssr.responseText);
		return "";
	}
	var result = resultNode.item(0).text;
	var completeNode = XmlDoc.selectNodes("Root/complete");
	var complete = completeNode.item(0).text;
	
	return result;
}

m4nExpression.prototype.parseServerSideFunction = function(exp) {
	//i.e. exp = "[0]; func: MyFunc(field:fld1, field:fld2)"
	
	var funcPos = exp.indexOf("func");
	var lPos = exp.indexOf("(", funcPos);
	var rPos = exp.indexOf(")", lPos);
	//split params
	var arrParams = exp.substring(lPos+1,rPos).split(",");
	var paramVal;
	var n;
	for (n=0; n<arrParams.length; n++) {
		paramVal = this.calcExpression(m4nTrim(arrParams[n]));
		//replace param with value
		arrParams[n] = paramVal;
	}
	//recreate the expression with param values
	exp = exp.substring(0,lPos+1) + arrParams.join(",") + exp.substr(rPos);
	return exp;
}
/****************************************************************************************/

/* public properties */


/****************************************************************************************/

/* Exceptions */

function m4nExpressionException(msg)
{
	this.msg = msg;
}
m4nExpressionException.prototype = new m4nException();

/****************************************************************************************/
