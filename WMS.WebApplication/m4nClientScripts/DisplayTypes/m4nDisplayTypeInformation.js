/*
Made4Net Version: 1.0

Holds information about all display types.
The main purpose of this class is to hold the mapping between server side classes and client side classes
*/


/****************************************************************************************/

/* Main Class */

function m4nDisplayTypeInformation()
{
	this.container = new Array();
}

//public
//adds information about a display type
//usage: [instance].add("Made4Net.WebControls.DisplayTypes.NumericTextBox", "ClientSideClass", "m4nNumericTextBox");
m4nDisplayTypeInformation.prototype.add = function(serverSideClassName, key, value)
{
	this.addType(serverSideClassName);
	this.container[serverSideClassName][key] = value;
}

//public
//returns a information about a display type
//usage: [instance].getInfo("Made4Net.WebControls.DisplayTypes.NumericTextBox", "ClientSideClass");
m4nDisplayTypeInformation.prototype.getInfo = function(serverSideClassName, key)
{
	return this.container[serverSideClassName][key];
}

//private
//adds a new display type to hold information for
m4nDisplayTypeInformation.prototype.addType = function(serverSideClassName)
{
	if (this.container[serverSideClassName] == null)
	{
		this.container[serverSideClassName] = new Array();
	}
}
