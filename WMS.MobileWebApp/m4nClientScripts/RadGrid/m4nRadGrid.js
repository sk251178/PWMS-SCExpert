/*
Made4Net Version: 1.0

Helper function for RadGrid client side functionality
*/

/****************************************************************************************/

function m4nRadGrid(gridInstance)
{
	this.grid = gridInstance;
}

//called after a grid row is selected/de-selected and checkes/unchecks
//the multiline checkbox
m4nRadGrid.prototype.selectRow = function(gridRow)
{
    var gridRowTable = gridRow.Owner;
	var cell = gridRowTable.GetCellByColumnUniqueName(gridRow, "MultiSelect");

    if (!cell)
    {
		throw new m4nRadGridException("Could not get cell object.");
    }
    var checkBox = this.getMultiSelectCheckBox(cell);
    if (!checkBox)
    {
		throw new m4nRadGridException("Could not get checkbox object.");
    }
    gridRow.Selected ? checkBox.checked = true : checkBox.checked = false;
}

//select / de-select all
m4nRadGrid.prototype.toggleSelectAll = function(bool)
{
	var row;
	for(i in this.grid.MasterTableView.Rows)
	{
		row = this.grid.MasterTableView.Rows[i]
		if (!row.Selected == bool)
		{
			this.grid.MasterTableView.SelectRow(row.Control,0);	
			this.selectRow(row);
		}
	}
}

//returns the checkbox control from the multiline select column
m4nRadGrid.prototype.getMultiSelectCheckBox = function(control)
{
	if (!control) return;
    for (var i = 0; i<control.childNodes.length; i++)
    {
        if (!control.childNodes[i].tagName) continue;
        if ((control.childNodes[i].tagName.toLowerCase() == "input") && (control.childNodes[i].type.toLowerCase() == "checkbox"))
        {
            return control.childNodes[i];
        }
    }
}

/****************************************************************************************/
/* Exceptions */

function m4nRadGridException(msg)
{
	this.msg = msg;
}
m4nRadGridException.prototype = new m4nException();