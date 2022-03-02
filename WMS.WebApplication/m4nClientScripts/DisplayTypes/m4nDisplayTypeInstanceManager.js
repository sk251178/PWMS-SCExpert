/*
Made4Net Version: 1.1

This object creates client-side instances of display types
*/


/****************************************************************************************/

/* Main Class */

var m4nDisplayTypeInstances = new Array();

function m4nDisplayTypeInstanceManager(id)
{
	//syntax for id is [TableEditorID:FieldName]. i.e. "TEMain.Qty"
	
	/////////////////////////////////////////
	//validate
	if (m4nIsEmpty(id))
	{
		throw new m4nArgumentNullException("id");
	}

	//syntax is "TableEditorID:FieldName"
	var errMsg = "'id' paramenter must be of the format 'TableEditorID:FieldName'. The current id value is '" + id + "'.";
	var parts = id.split(":");
	if (parts.length != 2)
	{
		throw new m4nArgumentException("id", errMsg);
	}
	//end validate
	/////////////////////////////////////////
	
	this.id = id;
	this.te = parts[0];
	this.field = parts[1];
}

//public 
//this is the main method of this class. returns an instance of a display type.
m4nDisplayTypeInstanceManager.prototype.getInstance = function()
{

	var ddInstance = null;
	//try to use existing instance
	try
	{
		//ddInstance = eval(this.getDisplayTypeVarName());
		ddInstance = m4nDisplayTypeInstances[this.getDisplayTypeVarName()];
	}
	catch(e){}
	
	if (ddInstance == null)
	{
		//instance variable not defined
		//try to create
		this.createDisplayTypeInstance();
		
		//try to get the instance again
		try
		{
			//ddInstance = eval(this.getDisplayTypeVarName());
			ddInstance = m4nDisplayTypeInstances[this.getDisplayTypeVarName()];
		} catch(e){}
		
		//if still not found...
		if (ddInstance == null)
		{
			//throw and exception
			throw new m4nDisplayTypeNotFoundException(this.te, this.field);
		}
	}

	return ddInstance;
}

//private
//returns the variable name of the display type instance
m4nDisplayTypeInstanceManager.prototype.getDisplayTypeVarName = function ()
{
	return "m4nDisplayType_" + this.te + "_" + this.field;
}

//private
//creates an instance of a display type and assigns it to a global scope variable
m4nDisplayTypeInstanceManager.prototype.createDisplayTypeInstance = function()
{
	//get the name of the client side class
	var obj = this.getDisplayTypeTopObject();
	if (m4nBrowserName == "Explorer" && m4nBrowserVersion > 8)
	{
	   PrepareObjectAttributes(obj);
	}
	var serverClassName = obj.ClassName;
	var clientClassName = this.getDisplayTypeClientSideClassName(serverClassName);
	//eval(this.createInstanceEvalString(clientClassName));
	
	var instanceKey = this.getDisplayTypeVarName();
	var elementId = obj.id;
		
	switch(clientClassName)
	{
		case "m4nDisplayType":
			m4nDisplayTypeInstances[instanceKey] = new m4nDisplayType(this.te, this.field, elementId);
			break;
		case "m4nCheckBox":
			m4nDisplayTypeInstances[instanceKey] = new m4nCheckBox(this.te, this.field, elementId);
			break;
		case "m4nDateBox":
			m4nDisplayTypeInstances[instanceKey] = new m4nDateBox(this.te, this.field, elementId);
			break;
		case "m4nNumericTextBox":
			m4nDisplayTypeInstances[instanceKey] = new m4nNumericTextBox(this.te, this.field, elementId);
			break;
		case "m4nTypeAheadBox":
			m4nDisplayTypeInstances[instanceKey] = new m4nTypeAheadBox(this.te, this.field, elementId);
			break;
		default:
			throw new m4nDisplayTypeException("No client side implementation for display type '" + clientClassName + "'.");
			break;
	}
}


//private
//Creates a string that once evaluated using the eval() methods will create an instance of the display type
/*
m4nDisplayTypeInstanceManager.prototype.createInstanceEvalString = function(clientClassName)
{
	var varName = this.getDisplayTypeVarName(this.te, this.field);
	var ctrl = this.getDisplayTypeTopObject();
	var elementId = ctrl.id;
	return varName + " = new " + clientClassName + "(\"" + this.te + "\", \"" + this.field + "\", \"" + elementId + "\");";
}
*/
//private
//returns the top HTML tag that represents the display type
//usually it is a <SPAN> tag
m4nDisplayTypeInstanceManager.prototype.getDisplayTypeTopObject = function() 
{
	var teMode = m4nGetTEMode(this.te);
	te = this.te.toUpperCase();
	fld = this.field.toUpperCase();
	var tags = document.getElementsByTagName("input");
	var h; var dtype;
	for (i=0; i<=tags.length-1; i++) {
		if (tags[i].type == "hidden") {
			if (tags[i].name.toUpperCase() == "DO:" + te + ":" + fld) {
				h = tags[i];
				break;
			}
		}
	}
	if (h == null) {
		throw new m4nDisplayTypeInstantiationException(this.te, this.field, null, "The control could not be found.");
	}
	dtype = document.getElementById(h.value);
	return dtype;
}

//returns the client side class name of a display type by using the m4nDisplayTypeInfo array
m4nDisplayTypeInstanceManager.prototype.getDisplayTypeClientSideClassName = function(serverSideClassName)
{
	var result = null;
	try
	{
		result = m4nDisplayTypeInfo.getInfo(serverSideClassName, "ClientSideClass");
	}
	catch(e){}
	if (result == null)
	{
		//use the default
		result = "m4nDisplayType";
	}
	return result;
}


/********************************************************************************************/

/* exceptions */


function m4nDisplayTypeInstantiationException(te, field, serverSideClassName, msg)
{
	this.te = te;
	this.field = field;
	this.serverSideClassName = serverSideClassName;
	this.msg = "Could not create a client side display type instance for field '" + field + "' in object '" + te + "' of type '" + serverSideClassName + "'. " + msg;
}
m4nDisplayTypeInstantiationException.prototype = new m4nDisplayTypeException();

/********************************************************************************************/
