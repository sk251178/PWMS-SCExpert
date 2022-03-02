/*
Made4Net Version: 1.1

The client side instance of the DateBox display type
*/


/****************************************************************************************/

/* Main Class */

function m4nTypeAheadBox(TE, fieldName, elementId)
{
	//constructor function
	this.construct(TE, fieldName, elementId);
}

//parent class
m4nTypeAheadBox.prototype = new m4nDisplayType();
m4nTypeAheadBox.superclass = m4nDisplayType.prototype;

//constructor function
m4nTypeAheadBox.prototype.construct = function(TE, fieldName, elementId)
{
	m4nTypeAheadBox.superclass.construct.call(this, TE, fieldName, elementId);
}

m4nTypeAheadBox.prototype.toggleCtrlReadOnly = function(ctrl, bool)
{
	m4nTypeAheadBox.superclass.toggleCtrlReadOnly.call(this, ctrl, bool);
	
	if (!this.hidden)
	{
		switch (ctrl.tagName.toUpperCase())
		{
			case "IMG":
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
				ctrl.parentElement.style.visibility = visib;
				break;
		
			case "INPUT":
				//disabled the textbox that appears before the TypeAhead is loaded
				//so that it doesn't load the TypeAhead on click
				ctrl.disabled = bool;
				break;
				
			case "TYPEAHEAD":
				//disabled the TypeAhead
				ctrl.enabled = !bool;
				break;
		}
	}
}

m4nTypeAheadBox.prototype.setHidden = function(val)
{
	m4nTypeAheadBox.superclass.setHidden.call(this, val);

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
						visib = "hidden";
					}
					else
					{
						visib = "visible";
					}
					child.style.visibility = visib;
					child.parentElement.style.visibility = visib;
				}
			}
		}
	}
}