/*
Made4Net Version: 1.0

The client side instance of the DateBox display type
*/


/****************************************************************************************/

/* Main Class */

function m4nDateBox(TE, fieldName, elementId)
{
	//constructor function
	this.construct(TE, fieldName, elementId);
}

//parent class
m4nDateBox.prototype = new m4nDisplayType();
m4nDateBox.superclass = m4nDisplayType.prototype;

//constructor function
m4nDateBox.prototype.construct = function(TE, fieldName, elementId)
{
	m4nDateBox.superclass.construct.call(this, TE, fieldName, elementId);
}

m4nDateBox.prototype.toggleCtrlReadOnly = function(ctrl, bool)
{
	m4nDateBox.superclass.toggleCtrlReadOnly.call(this, ctrl, bool);
	
	if (ctrl.tagName.toUpperCase() == "IMG")
	{
		var visib;
		if (bool)
		{
			visib = "hidden";
		}
		else
		{
			visib = "visible";
		}
		ctrl.style.visibility = visib;
	}
}

m4nDateBox.prototype.setHidden = function(val)
{
	m4nDateBox.superclass.setHidden.call(this, val);

	//toggle visibility of image
	var ctrl = this.getControl();
	if (ctrl.all)
	{
		for (i in ctrl.all)
		{
			var child = ctrl.all[i];
			if (child.tagName != null)
			{
				if (child.tagName.toUpperCase() == "IMG")
				{
					var bool = m4nParseBool(val);
					var visib;
					if (bool)
					{
						visib = "visible";
					}
					else
					{
						visib = "hidden";
					}
					child.style.visibility = visib;
				}
			}
		}
	}
}