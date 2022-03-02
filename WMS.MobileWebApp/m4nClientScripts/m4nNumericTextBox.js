/*
Made4Net Version: 2.5

These functions support the Numeric Text Box display type
*/


/****************************************************************************************/

function m4nNumericTextBox_OnChange()
{
return;
	var obj = event.srcElement;
	m4nNumericTextBox_ApplyFormat(obj, obj.value);
	obj.isDirty = true;
}

function m4nNumericTextBox_ApplyFormat(obj, val)
{
	obj.value = m4nNumericTextBox_Format(obj, val);
	//alert(val + "\r\n" + obj.value);
}

function m4nNumericTextBox_EnsureMethods(objId)
{
	var obj = document.getElementById(objId);
	if (obj.getValue == undefined)
	{
		m4nNumericTextBox_CreateMethods(obj);
		obj.recalc(obj.value);
		if (this.m4nBrowserName == "Explorer" && this.m4nBrowserVersion > 8)
		    obj.RawValue = obj.parentElement.attributes["RawValue"].value;
		else
		    obj.RawValue = obj.parentElement.RawValue;
		obj.title = obj.RawValue;
		/*if (obj.RawValue == undefined)
		{
			alert("Cannot find RawValue for element " + obj.parentElement);
		}*/
		obj.isDirty = false;
	}
}

function m4nNumericTextBox_CreateMethods(obj)
{
	obj.getValue = function()
	{
		if (obj.isDirty)
		{
			//alert(obj.id + " (dirty) " + m4nGetFloat(obj.value));
			return m4nGetFloat(obj.value);
		}
		else
		{
			//alert(obj.id + " (raw) " + obj.RawValue);
			return obj.RawValue;
		}
	}
	obj.recalc = function(val)
	{
		m4nNumericTextBox_ApplyFormat(obj, val);
	}
	obj.setValue = function(val)
	{
		val = val.toString();
		obj.RawValue = val;
		//if (this.m4nBrowserName == "Explorer" && this.m4nBrowserVersion > 8) obj.attributes["RawValue"].value
		obj.title = val;
		obj.recalc(val);
		obj.isDirty = false;
		//alert("set" + "\r\n" + val + "\r\n" + obj.value);
	}
}

function m4nNumericTextBox_Format(obj, pVal) 
{
	//var val = obj.value;
	var val = m4nStripNonMath(pVal);
	//var val = pVal;
	try 
	{
		val = eval(val);
	}
	catch (e)
	{
		val = pVal;
	}
	
	var rawVal = m4nGetFloat(val);
	if (isNaN(rawVal) || rawVal == undefined) {
		return '';
	}
	
	var par = obj.parentElement;

	///////////////////////////////
	//check min/max values
	if ("min_value" in par)
	{
		var minValue = m4nGetFloat(par.min_value);
		if (rawVal < minValue) rawVal = minValue;
	}
	if ("max_value" in par)
	{
		var maxValue = m4nGetFloat(par.max_value);
		if (rawVal > maxValue) rawVal = maxValue;
	}
	///////////////////////////////
	var num = new m4nNum(rawVal);
	
	var commas = m4nParseBool(par.commas);

	var decimals;
	try {decimals = parseInt(par.decimals); } catch(e) {}	
	if (isNaN(decimals)) decimals = -1;
	
	try {	num.setDecimals			(decimals				);	} catch(e) {}
	try {	num.setCommas			(commas					);	} catch(e) {}
	try {	num.setNegativeFormat	(par.negative_format	);	} catch(e) {}
	try {	num.setNegativeColor	(par.negative_color		);	} catch(e) {}
	try {	num.setSymbol			(par.symbol				);	} catch(e) {}
	try {	num.setSymbolPosition	(par.symbol_position	);	} catch(e) {}
	
	if (rawVal < 0) {
		obj.style.color = num.getNegativeColor();
	} else {
		obj.style.color = "";
	}
	return num.toFormatted();
}

//removes characters that are not digits or arithmetic signs:
// + - * / ( )
function m4nStripNonMath(str)
{
//return str;
	var re = /[^0-9|^\.^\-^\+^\*^\/^\(^\)]/g;
	var result = str.replace(re, '');
	return result;
}
