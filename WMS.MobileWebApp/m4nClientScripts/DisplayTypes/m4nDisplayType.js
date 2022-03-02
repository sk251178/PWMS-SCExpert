/*
Made4Net Version: 1.4

This object is the client side instance of a display type
*/


/****************************************************************************************/

/* Main Class */

//all display types must have a constructor with this signature 
function m4nDisplayType(TE, fieldName, elementId)
{
	if (arguments.length > 0) {
		this.construct(TE, fieldName, elementId);
	}
}

//override this function to run any code that initializes a display type
m4nDisplayType.prototype.init = function() {}

/*
the construct() function replaces the constructor
so it can be called from inherited classes
use as follows ("m4nNumericTextBox" is the inherited class)

function m4nNumericTextBox(TE, fieldName)
{
	//call constructor function
	this.construct(TE, fieldName);
}

//define inheritance
m4nNumericTextBox.prototype = new m4nDisplayType();

//this enables you to call the constructor of the parent class
m4nNumericTextBox.superclass = m4nDisplayType.prototype;

//constructor function
m4nNumericTextBox.prototype.construct = function(TE, fieldName)
{
	//call the constructor of the parent class
	m4nNumericTextBox.superclass.construct.call(TE, fieldName);
}
*/
m4nDisplayType.prototype.construct = function(TE, fieldName, elementId)
{
	this.TE = TE;
	this.fieldName = fieldName;

	this.controlId = elementId;
	this.control = document.getElementById(this.controlId);
	
	this.className = this.control.ClassName;
	this.visible;
	this.hidden;
	this.readOnly;
	this.labels;
	this.validators;
	this.validatorsEnabled;
	
	this.init();
}

/********************************************************************************************/

/* public methods */

//returns the outer SPAN element of the display type
m4nDisplayType.prototype.getControl = function()
{
	if (this.control == null)
	{
		throw new m4nDisplayTypeException("control member is not set.");
	}
	return this.control;
}

//returns the id of the main HTML element containing the value of the display type
m4nDisplayType.prototype.getValueId = function()
{
	var vid = this.getControl().ValueID;
	if (m4nIsEmpty(vid))
	{
		throw new m4nDisplayTypeException("Display type '" + this.controlId + "' does not have a ValueID property defined");
	}
	return vid;
}

//returns the main HTML element containing the value of the display type
m4nDisplayType.prototype.getValueControl = function()
{
	var valCtl = document.getElementById(this.getValueId());
	if (valCtl == null)
	{
		throw new m4nDisplayTypeException("Cannot find ValueControl '" + this.getValueId() + "'.");
	}
	return valCtl;
}

//returns the main HTML element containing the value of the display type
m4nDisplayType.prototype.getValueControl = function()
{
	var valCtl = document.getElementById(this.getValueId());
	if (valCtl == null)
	{
		throw new m4nDisplayTypeException("Cannot find ValueControl '" + this.getValueId() + "'.");
	}
	return valCtl;
}

//returns an array of all labels for this display type
m4nDisplayType.prototype.getLabels = function()
{
	if (this.labels != null) return this.labels;
	
	var lbls = new Array();
	var n = 0;
	
	var spans = document.all.tags("SPAN")
	for (i in spans)
	{
		var tag = spans[i];
		try
		{  
		   if (tag.attributes["ForField"]!=undefined)
			if (tag.attributes["ForField"].value == this.controlId.substring(this.controlId.length-tag.attributes["ForField"].value.length))
			{
				lbls[n] = tag;
				n++;
			}
		}
		catch(e) {}
	}
	this.labels = lbls;
	return lbls;
}

//returns an array of all validators for this display type
m4nDisplayType.prototype.getValidators = function()
{
	if (this.validators != null) return this.validators;
	
	var allValidators = Page_Validators;
	var thisValidators = new Array();
	spans = this.getControl().all.tags("SPAN")
	var n = 0;

	for (i in spans)
	{
		for (v in allValidators)
		{
			if (spans[i] == allValidators[v])
			{
				thisValidators[n] = spans[i];	
				n++;
				break;
			}
		}
	}
	this.validators = thisValidators;
	return this.validators;
}

m4nDisplayType.prototype.setProperty = function(propertyName, val)
{
	if (m4nIsEmpty(propertyName))
	{
		throw new m4nArgumentNullException("propertyName");
	}
	
	switch (propertyName.toUpperCase())
	{
		case "_VALUE":
			this.setValue(val);
			//fire onchange event
			//this.fireOnChange();
			break;
			
		case "READONLY":
			this.setReadOnly(val);
			break;
			
		case "VISIBLE":
			this.setVisible(val);
			break;

		case "HIDDEN":
			this.setHidden(val);
			break;
	}
}

// override getValue in an inherited class and return the value in raw format
m4nDisplayType.prototype.getValue = function()
{
	return this.getValueControl().value;
}

// override setValue in an inherited class and apply the new value to the display type
m4nDisplayType.prototype.setValue = function(val)
{
	this.getValueControl().value = val;
}

m4nDisplayType.prototype.getVisible = function()
{
	return this.visible;
}

m4nDisplayType.prototype.setVisible = function(val)
{
	var bool = m4nParseBool(val);
	var disp;
	if (bool)
	{
		disp = "inline";
	}
	else
	{
		disp = "none";
	}
	this.getControl().style.display = disp;
	
	var lbls = this.getLabels();
	for(i in lbls)
	{
		var lbl = lbls[i];
		lbl.style.display = disp;
	}
	
	this.visible = bool;

	//if visible, turn validators on. otherwise, turn validators off
	this.setValidatorsEnabled(bool);
}

m4nDisplayType.prototype.getHidden = function()
{
	return this.hidden;
}

m4nDisplayType.prototype.setHidden = function(val)
{
    //alert("debil");
	var bool = m4nParseBool(val);
	var visib;
	if (bool)
	{
		visib = "hidden";
		this.getValueControl().attributes["type"].value="hidden";
	}
	else
	{
		visib = "visible";
		this.getValueControl().attributes["type"].value="text";
		
	}
	//this.getControl().style.visibility = visib;
    var disp;
    if (!bool)
	{
		disp = "inline";
	}
	else
	{
		disp = "none";
	}

	var lbls = this.getLabels();
	for(i in lbls)
	{
		var lbl = lbls[i];
		//lbl.style.visibility = visib;
		lbl.style.display = disp;
	}

	this.hidden = bool;
	
	//if hidden, turn validators off. otherwise, turn validators on
	this.setValidatorsEnabled(!bool);
}

m4nDisplayType.prototype.getReadOnly = function()
{
	return this.readOnly;
}

m4nDisplayType.prototype.setReadOnly = function(val)
{
	this.toggleReadOnly(val);
}

m4nDisplayType.prototype.getValidatorsEnabled = function(val)
{
	return this.validatorsEnabled;
}

m4nDisplayType.prototype.setValidatorsEnabled = function(val)
{
	this.toggleValidators(val);
}

m4nDisplayType.prototype.fireOnChange = function(val)
{
	try
	{
		if (this.getValueControl().onchange == null) return;
	}
	catch(e)
	{
		//onchange is not defined for the value ctrl
		return;
	}
	
	var ctrl = this.getValueControl();
	if (document.activeElement != ctrl)
	{
		this.getValueControl().fireEvent('onchange');
	}
}


/********************************************************************************************/

/* private methods */

m4nDisplayType.prototype.toggleReadOnly = function(val)
{
	var bool = m4nParseBool(val);
	var topTag = this.getControl();
	this.toggleReadOnlyRecursive(topTag, bool)
	this.readOnly = bool;
}

m4nDisplayType.prototype.toggleReadOnlyRecursive = function(ctrl, bool)
{
	this.toggleCtrlReadOnly(ctrl, bool);

	if (ctrl.childNodes && ctrl.childNodes.length > 0)
	{
		var i=0;
		for (;;)
		{
			if (i > ctrl.childNodes.length-1) break;
			
			var child = ctrl.childNodes[i];
			if (child.tagName != null)
			{
				this.toggleReadOnlyRecursive(child, bool);
			}
			
			i++;
		}
	}
}

//toggles the readonly property of a control
m4nDisplayType.prototype.toggleCtrlReadOnly = function(ctrl, bool)
{
	var tag = ctrl.tagName;
	var inpType = m4nGetInputType(ctrl);
	if (inpType != null)
	{
		inpType = inpType.toUpperCase();
		
		if (inpType == "SELECT" || inpType == "CHECKBOX")
		{
			ctrl.disabled = bool;
		}
		else if (inpType == "TEXT" || inpType == "TEXTAREA" || inpType == "PASSWORD")
		{
			ctrl.readOnly = bool;
			this.toggleCtrlReadOnlyClass(ctrl, bool);
		}
	}
}

//toggles the css class name of a control to match the readonly state
m4nDisplayType.prototype.toggleCtrlReadOnlyClass = function(ctrl, bool)
{
	if (ctrl.originalClassName == null)
	{
		ctrl.originalClassName = ctrl.className;
	}

	var newClassName;
	var origClassName;
	
	if (bool)
	{
		newClassName = "readonly";
	}
	else
	{
		if (ctrl.originalClassName.toLowerCase() == "readonly")
		{
			newClassName = "";
		}
		else
		{
			newClassName = ctrl.originalClassName;
		}
	}
	ctrl.className = newClassName;
}

//toggles the validators on or off
m4nDisplayType.prototype.toggleValidators = function(val)
{
	var bool = m4nParseBool(val);
	if (this.validatorsEnabled == null)
	{
		this.validatorsEnabled = true;
	}
	else
	{
		if (this.validatorsEnabled == bool) return;
	}

	var validators = this.getValidators();
	for (i in validators)
	{
		ValidatorEnable(validators[i], bool);
	}
	this.validatorsEnabled = bool;
}

/********************************************************************************************/

/* exceptions */

function m4nDisplayTypeException(msg)
{
	this.msg = msg;
}
m4nDisplayTypeException.prototype = new m4nException();

function m4nDisplayTypeNotFoundException(te, field)
{
	this.te = te;
	this.field = field;
	this.msg = "Client side display type instance for field '" + field + "' in object '" + te + "' was not found.";
}
m4nDisplayTypeNotFoundException.prototype = new m4nDisplayTypeException();

/********************************************************************************************/
