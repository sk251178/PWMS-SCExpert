/*
Made4Net Version: 1.0

The client side instance of the CheckBox display type
*/


/****************************************************************************************/

/* Main Class */

function m4nCheckBox(TE, fieldName, elementId)
{
	//constructor function
	this.construct(TE, fieldName, elementId);
}

//parent class
m4nCheckBox.prototype = new m4nDisplayType();
m4nCheckBox.superclass = m4nDisplayType.prototype;

//constructor function
m4nCheckBox.prototype.construct = function(TE, fieldName, elementId)
{
	m4nCheckBox.superclass.construct.call(this, TE, fieldName, elementId);
}

m4nCheckBox.prototype.setValue = function(val)
{
	this.getValueControl().checked = m4nParseBool(val);
}

m4nCheckBox.prototype.getValue = function()
{
	var bool = this.getValueControl().checked;
	if (bool)
	{
		return "1";
	}
	else
	{
		return "0";
	}
}