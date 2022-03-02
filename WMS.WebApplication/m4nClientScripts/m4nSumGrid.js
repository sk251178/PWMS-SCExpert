/*
Made4Net version 1.0

This object adds summary to a grid
*/


/****************************************************************************************/

/* Main Class */

function m4nSumGrid(tableId) {

	//member vars
	this.tableId;
	this.showLabel;
	this.label;
	this.labelAdded;
	this.sums; //array of summarty objects
	this.isInitialized;
	
	//public methods
	this.setShowLabel = sgSetShowLabel;
	this.setLabel = sgSetLabel;
	this.stripHtml = sgStripHtml;
	this.getFloat = sgGetFloat;
	this.addSum = sgAddSum;
	this.recalc = sgRecalc;
	
	//private methods
	this.getGridTable = sgGetGridTable;
	this.getGridFooter = sgGetGridFooter;
	this.getGridHeader = sgGetGridHeader;
	this.getGridRow = sgGetGridRow;
	this.getGridFooterCell = sgGetGridFooterCell;
	this.getGridCellIndex = sgGetGridCellIndex;
	this.initialize = sgInitialize;
	this.ensureInitialize = sgEnsureInitialize;
	this.addLabel = sgAddLabel;
	this.recalcColumn = sgRecalcColumn;
	this.getCellValue = sgGetCellValue;
	this.getColumnResult = sgGetColumnResult;
	this.ensureTable = sgEnsureTable;
	this.tableValid = sgTableValid;

	try {
		this.ensureTable(tableId);
	} catch (e) {
		alert(e);
		return;
	}	
	
	this.tableId = tableId;
	this.label = "<b>Total</b>";
	this.showLabel = true;
	this.sums = new Array();
	this.isInitialized = false;
}

/****************************************************************************************/

/* Properties */

function sgSetShowLabel(bool) {
	this.showLabel = bool;
}

function sgSetLabel(lbl) {
	this.label = lbl;
}

function sgTableValid() {
	if (this.tableId == null) return false;
	if (this.tableId.length = 0) return false;
	return true;
}

/****************************************************************************************/

/* Public Methods */

function sgAddSum(colHeaderText, decimals, prefix, suffix, isDynamic) {
	if (!this.tableValid()) return;
	
	var sum = new m4nColumnSum();
	
	sum.headerText = colHeaderText;
	sum.colIndex = this.getGridCellIndex(colHeaderText);
	if (sum.colIndex == -1) return;

	sum.setDecimals(decimals);
	sum.setIsDynamic(isDynamic);
	sum.setPrefix(prefix);
	sum.setSuffix(suffix);
	
	this.sums[this.sums.length] = sum;
}

function sgRecalc() {
	if (!this.tableValid()) return;

	this.ensureInitialize();
	
	var i;
	var sum;
	
	for (i=0; i<=this.sums.length-1; i++) {
		sum = this.sums[i];
		//do not repeat calculation of non-dynamic columns
		if (sum.isDynamic || !sum.isCalculated) {
			this.recalcColumn(sum);
		}
	}
}

/****************************************************************************************/

/* Private Methods */

function sgRecalcColumn(colSum) {
	colSum.setResult(this.getColumnResult(colSum));
	var cell = this.getGridFooterCell(colSum.colIndex);
	cell.style.textAlign = 'right';
	cell.style.marginRight = '10px';
	cell.innerHTML = colSum.getFormattedResult();
}

function sgGetColumnResult(colSum) {
	var gt = this.getGridTable();
	
	var total = 0;
	var n;
	var cell;
	var cellVal;
	
	for (n=1; n<=gt.rows.length-2; n++) {
		cell = gt.rows(n).cells(colSum.colIndex);
		cellVal = this.getCellValue(cell);
		if (!isNaN(cellVal)) {
			total += cellVal;
		}
	}
	return total;
}

function sgGetCellValue(cell) {
	var cellContent = cell.innerHTML;
	var strVal = this.stripHtml(cellContent);
	var val = this.getFloat(strVal);
	return val;
}

function sgEnsureInitialize() {
	if (!this.isInitialized) {
		this.initialize();
		this.isInitialized = true;
	}
}

function sgInitialize() {
	this.addLabel();	
}

function sgAddLabel() {
	if (this.showLabel && this.label != null && this.label.length > 0) {
		var cell = this.getGridFooterCell(0);
		cell.innerHTML = this.label;
	}
}

function sgEnsureTable(tableId) {
	var t = document.getElementById(tableId);
	if (t == null) throw 'Cannot find table with ID "' + tableId + '".';
}

/****************************************************************************************/

/* Private HTML Object methods */

function sgGetGridTable() {
	var grid = document.getElementById(this.tableId);
	return grid;
}

function sgGetGridFooter() {
	var gt = this.getGridTable();
	return this.getGridRow(gt.rows.length - 1);
}

function sgGetGridHeader() {
	var gt = this.getGridTable();
	return this.getGridRow(0);
}

function sgGetGridRow(n) {
	var gt = this.getGridTable();
	var row = gt.rows(n);
	return row;
}

function sgGetGridFooterCell(n) {
	var footer = this.getGridFooter();
	return footer.cells(n);
}

function sgGetGridCellIndex(colHeaderText) {
	var header = this.getGridHeader();
	var i; 
	var headText;
	
	for (i=0; i<=header.cells.length-1; i++) {
		headText = header.cells(i).innerHTML;
		headText = this.stripHtml(headText);
		if (headText == colHeaderText) return i;
	}
	alert('Could not find column "' + colHeaderText + '". No summary will be shown.');
	return -1;
}

/****************************************************************************************/

/* Utility functions */

function sgGetFloat(strHTML) {
	return m4nGetFloat(strHTML);
}

function sgStripHtml(oldString) {
   var newString = "";
   var inTag = false;
   for( var i = 0; i < oldString.length; i++ )
   {
        if( oldString.charAt(i) == '<' ) inTag = true;
        if( !inTag ) newString += oldString.charAt(i);
        if( oldString.charAt(i) == '>' ) inTag = false;
   }
   return newString;
}

/****************************************************************************************/

/* Structure to keep information about each summarized column */

function m4nColumnSum() {
	//member vars
	this.headerText;
	this.colIndex;
	this.result;
	this.decimals;
	this.prefix;
	this.suffix;
	this.style;
	this.isDynamic; // column that are not dynamic will only be calculated once
	this.isCalculated;
	
	this.getFormattedResult = csGetFormattedResult;
	this.setDecimals = csSetDecimals;
	this.setSuffix = csSetSuffix;
	this.setPrefix = csSetPrefix;
	this.setCssStyle = csSetCssStyle;
	this.setResult = csSetResult;
	this.setIsDynamic = csSetIsDynamic;
	
	this.decimals = 0;
	this.prefix = '';
	this.suffix = '';
	this.cssStyle = 'font-weight: bold';
}

function csGetFormattedResult() {
	var spanStart = '<span style="margin-right:0px;';
	spanStart += this.cssStyle;
	spanStart += '">';
	
	var spanEnd = '</span>';
	var formattedValue = m4nNumFormat(this.result, this.decimals);

	return spanStart + this.prefix + formattedValue + this.suffix + spanEnd;
}

function csSetDecimals(decimals) {
	if (decimals < 0 || decimals == null) decimals = 0;
	this.decimals = decimals;
}

function csSetSuffix(suffix) {
	if (suffix == null) suffix = '';
	this.suffix = suffix;
}

function csSetPrefix(prefix) {
	if (prefix == null) prefix = '';
	this.prefix = prefix;
}

function csSetCssStyle(style) {
	if (style == null) style = '';
	this.style = style;
}

function csSetIsDynamic(isDynamic) {
	if (isDynamic) {
		this.isDynamic = true;
	} else {
		this.isDynamic = false;
	}
}

function csSetResult(result) {
	this.result = result;
	this.isCalculated = true;
}

/****************************************************************************************/