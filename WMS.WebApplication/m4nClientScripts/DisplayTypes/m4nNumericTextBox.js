/*
Made4Net Version: 1.4

The client side instance of the Numeric TextBox display type
*/


/****************************************************************************************/

/* Main Class */

function m4nNumericTextBox(TE, fieldName, elementId)
{
	//constructor function
	this.construct(TE, fieldName, elementId);
	this.userDataEntryMode = false;
	this.innerCtrl;
	this.rawValCtrl;
}

//parent class
m4nNumericTextBox.prototype = new m4nDisplayType();
m4nNumericTextBox.superclass = m4nDisplayType.prototype;

//constructor function
m4nNumericTextBox.prototype.construct = function(TE, fieldName, elementId)
{
	m4nNumericTextBox.superclass.construct.call(this, TE, fieldName, elementId);
}

//initialize the display type
m4nNumericTextBox.prototype.init = function()
{
	this.innerCtrl = this.getInnerCtrl();
	this.rawValCtrl = this.getRawValCtrl();
	this.attachEvents();
	
	//maintain raw value when formatting 
	var rawVal = this.getRawValue(this.rawValCtrl.value);
	this.applyFormat();
	this.rawValCtrl.value = rawVal;
	
	if (this.rawValCtrl.value == null) 
	{
		this.rawValCtrl.value = this.getNullValue();
	}
}

//handles a change of the contol's value
m4nNumericTextBox.prototype.onChangeHandler = function()
{
	this.applyFormat();
}

m4nNumericTextBox.prototype.onPropertyChangeHandler = function()
{
	if (this.userDataEntryMode && event.propertyName == "value")
	{
		//user entered a value in the input
		this.rawValCtrl.value = this.getRawValue(m4nNumericTextBox.superclass.getValue.call(this));
	}
	if (this.rawValCtrl.value == "null")
	{
		this.rawValCtrl.value = this.getRawValue(m4nNumericTextBox.superclass.getValue.call(this));
	}
}

m4nNumericTextBox.prototype.onFocusHandler = function()
{
	this.userDataEntryMode = true;
}

m4nNumericTextBox.prototype.onBlurHandler = function()
{
	this.userDataEntryMode = false;
}

//attaches change events to the control
m4nNumericTextBox.prototype.attachEvents = function()
{
	var instance = this;
	var result;

	result = this.getValueControl().attachEvent("onchange" ,function() { instance.onChangeHandler() });
	if (!result) throw new m4nDisplayTypeException('Could not attach onchange event.');
	
	result = this.getValueControl().attachEvent("onpropertychange" ,function() { instance.onPropertyChangeHandler() });
	if (!result) throw new m4nDisplayTypeException('Could not attach onchange event.');

	result = this.getValueControl().attachEvent("onfocus" ,function() { instance.onFocusHandler() });
	if (!result) throw new m4nDisplayTypeException('Could not attach onchange event.');

	result = this.getValueControl().attachEvent("onblur" ,function() { instance.onBlurHandler() });
	if (!result) throw new m4nDisplayTypeException('Could not attach onchange event.');
}

//applies formatting to the value entered by the user
m4nNumericTextBox.prototype.applyFormat = function()
{
	this.setValue(m4nNumericTextBox.superclass.getValue.call(this));
}

m4nNumericTextBox.prototype.setValue = function(val)
{
	//store value in raw format
	this.rawValCtrl.value = this.getRawValue(val);
	
	//show formatted value
	var valToFormat;
	if (val.toString() == '')
	{
		//if user erased the value, do not format the raw value
		//it might contain zero or other numeric value
		valToFormat = '';
	}
	else
	{
		//the raw value will be formatted
		valToFormat = this.rawValCtrl.value;
	}
	m4nNumericTextBox.superclass.setValue.call(this, this.formatValue(valToFormat));
}

m4nNumericTextBox.prototype.getValue = function()
{
	if (this.userDataEntryMode)
	{
		//return value being entered by the user
		return this.getRawValue(this.getValueControl().value);
	}
	return this.rawValCtrl.value;
}

m4nNumericTextBox.prototype.getRawValue = function(val)
{
	var isEmpty = false;
	
	if (val == undefined)	val = '';
	if (val == null)		val = '';
	
	val = val.toString();
	val = this.stripFormat(val);
	
	//if empty, return the value defined by null_value property
	if (val == '') return this.getNullValue();

	try 
	{
		//run eval() to calculate 
		//i.e. the value "2*(3+1)" entered by the user becomes "8"
		val = eval(val);
		if (!isNaN(val))
		{
			var num = new Number(val);
			val = num.toFixed(20);
		}
	}
	catch (e)
	{}
	
	var rawVal = m4nGetFloat(val);
	if (isNaN(rawVal) || rawVal == undefined) {
		return '';
	}
	
	///////////////////////////////
	//check min/max values
	if ("min_value" in this.innerCtrl)
	{
		var minValue = m4nGetFloat(this.innerCtrl.min_value);
		if (rawVal < minValue) rawVal = minValue;
	}
	if ("max_value" in this.innerCtrl)
	{
		var maxValue = m4nGetFloat(this.innerCtrl.max_value);
		if (rawVal > maxValue) rawVal = maxValue;
	}
	///////////////////////////////
	
	return rawVal;
}

//converts a number from the raw value (i.e. 1234.5) to the formatted value (i.e. $1,234.50)
m4nNumericTextBox.prototype.formatValue = function(rawVal)
{
	if (rawVal == undefined)	return '';
	if (rawVal == null)			return '';
	if (rawVal.toString()=='')	return '';
	
	var num = new m4nNum(rawVal);
	
	var ctrl;
	if (this.isSecondary())
	{
		ctrl = this.control;
	}
	else
	{
		ctrl = this.innerCtrl;
	}
	var commas = m4nParseBool(ctrl.commas);

	var decimals;
	try {decimals = parseInt(ctrl.decimals); } catch(e) {}	
	if (isNaN(decimals)) decimals = -1;
	
	try {	num.setDecimals			(decimals				);	} catch(e) {}
	try {	num.setCommas			(commas					);	} catch(e) {}
	try {	num.setNegativeFormat	(ctrl.negative_format	);	} catch(e) {}
	try {	num.setNegativeColor	(ctrl.negative_color	);	} catch(e) {}
	try {	num.setSymbol			(ctrl.symbol			);	} catch(e) {}
	try {	num.setSymbolPosition	(ctrl.symbol_position	);	} catch(e) {}
	
	var obj = this.getValueControl();
	if (rawVal < 0) {
		obj.style.color = num.getNegativeColor();
	} else {
		obj.style.color = "";
	}
	return num.toFormatted();
}

//converts a number from the formatted value (i.e. $1,234.50) to the raw value (i.e. 1234.5)
m4nNumericTextBox.prototype.stripFormat = function(val)
{
	//removes characters that are not digits or arithmetic signs by using regular expression
	// 0-9 + - * / ( )
	if (val == undefined) val = '';
	val = val.toString();
	var re = /[^0-9|^\.^\-^\+^\*^\/^\(^\)]/g;
	return val.replace(re, '');
}

m4nNumericTextBox.prototype.getNullValue = function()
{
	var nullVal;
	try {nullVal = parseInt(this.innerCtrl.null_value); } catch(e) {}	
	if (isNaN(nullVal)) nullVal = null;
	return nullVal;
}

//the first child html tag contains the properties
m4nNumericTextBox.prototype.getInnerCtrl = function()
{
	var innerCtrl = this.control.firstChild;
	return innerCtrl;
}
//the second child html is the hidden input holding the raw value
m4nNumericTextBox.prototype.getRawValCtrl = function()
{
	var ctrl;
	if (this.isSecondary())
	{
		ctrl = this.control.children[1];
	}
	else
	{
		ctrl = this.control.firstChild.children[1];
	}
	return ctrl;
}
m4nNumericTextBox.prototype.isSecondary = function()
{
	ctrl = this.control.firstChild.children[1];
	if (ctrl == undefined)
	{
		return true;
	}
	else
	{
		return false;
	}
}