/*
Made4Net Version: 2.8

This object adds expression calculations to a grid
*/


/****************************************************************************************/

/* Main Class */

function m4nCalcGrid(tableId,exp) {

	//member vars
	this.tableId;
	this.table;
	this.appliedControlsId;
	this.expresssion;
	//this.cssStyle;
	this.isInitialized;
	this.isTableAttached;
	this.isSingleControlAttached;
	this.attachedCtrls;
	this.shouldApplyToTable;
	this.defaultEvent = "onchange";
	this.remoteData;
	this.rowStartOffset = 3;
	this.rowStepOffset = 2;
	
	//public methods
	this.setAppliedControlsId = cgSetAppliedControlsId;
	this.attachControlEvent = cgAttachControlEvent;
	this.attachTableControlsEvents = cgAttachTableControlsEvents;
	this.attachTableMultiSelectBoxEvents = cgAttachTableMultiSelectBoxEvents;
	this.stripHtml = cgStripHtml;
	//this.setCssStyle = cgSetCssStyle;
	this.recalc = cgRecalc;
	this.getFloat = cgGetFloat;
	this.onRemoteDataReceived = null;
	
	//private methods
	this.setTableId = cgSetTableId;
	this.getGridFooter = cgGetGridFooter;
	this.getGridHeader = cgGetGridHeader;
	this.getGridRow = cgGetGridRow;
	this.getGridFooterCell = cgGetGridFooterCell;
	this.getGridCellIndex = cgGetGridCellIndex;
	this.getColValue = cgGetColValue;
	//this.getFormattedResult = cgGetFormattedResult;
	this.ensureInitialize = cgEnsureInitialize;
	this.calculate = cgCalculate;
	this.calcExpression = cgCalcExpresssion;
	this.getCellValue = cgGetCellValue;
	this.getColumnResult = cgGetColumnResult;
	this.ensureTable = cgEnsureTable;
	this.tableValid = cgTableValid;
	this.isInputField = cgIsInputField;
	this.hasAppliedControls = cgHasAppliedControls;
	this.getControl = cgGetControl;
	this.getControlType = cgGetControlType;
	this.attach = cgAttach;
	this.getTableColumnsControls = cgGetTableColumnsControls;
	this.getAppliedControlsArray = cgGetAppliedControlsArray;
	this.attachedCtrlEventRaised = cgAttachedCtrlEventRaised;
	this.getValueFromControl = cgGetValueFromControl;
	this.serverSideEval = cgServerSideEval;
	this.parseServerSideFunction = cgParseServerSideFunction;

	this.setTableId(tableId);
	try {
		this.ensureTable();
	} catch (e) {
		return;
	}	
	
	this.attachedCtrls = new Array();
	this.isTableAttached = false;
	this.isSingleControlAttached = false;
	this.shouldApplyToTable = true;
	this.appliedControlsId = new Array();
	this.expression = exp;
	//this.cssStyle='';
	this.sums = new Array();
	this.isInitialized = false;
}

/****************************************************************************************/

/* Properties */

//adds an expression field, where the result is displayed
function cgSetAppliedControlsId(ctrl_id) {
	if (!this.tableValid()) return;
	this.appliedControlsId[this.appliedControlsId.length] = ctrl_id;
}

function cgIsInputField(ctrl) {
	return (ctrl.tagName.toLowerCase() == 'input');
}
	
function cgHasAppliedControls() {
	return (!(this.appliedControlsId == null || appliedControlsId.length==0));
}

function cgTableValid() {
	if (this.table == null) return false;
	return true;
}



/****************************************************************************************/

/* Public Methods */

function cgRecalc()
{
	var ctrl;
	var n;
	for (n=0;n<this.attachedCtrls.length;n++)
	{
		ctrl=this.attachedCtrls[n];
		if (this.getControlType(ctrl)=="text")
			ctrl.fireEvent('onchange',null)
		else
			ctrl.click();
	}
}

function cgAttachControlEvent(attached_ctrl_id) {
	if (this.isTableAttached) throw '(1) Cannot attach to single control, object already attached to grid';
	var ctrl = document.getElementById(attached_ctrl_id);
	if (ctrl==null) return;
	this.attach(ctrl);
	this.isSingleControlAttached = true;
}

//attach a change event to a field that affects the calculation
function cgAttachTableControlsEvents(fieldEvent) {
	if (!this.tableValid() || fieldEvent == null || fieldEvent.length==0) return;
	if (this.isSingleControlAttached) throw '(2) Cannot attach to table, object already assigned to single control';

	var fieldName, eventName;
	var parts = fieldEvent.split(":");
	if (parts.length == 1) {
		fieldName = m4nTrim(parts[0]);
		eventName = this.defaultEvent;
	} else if (parts.length == 2) {
		fieldName = m4nTrim(parts[0]);
		eventName = m4nTrim(parts[1]);
	}

	if (parts.length > 2 || fieldName == null || fieldName.length == 0) {
		alert("(12) Event definition '" + fieldEvent + "' is invalid. Expecting 'fieldname:event' or 'fieldname'.");
		return;
	}

	var n;
	var ctrls;
	var ctrl;
	ctrls = this.getTableColumnsControls('_DisplayTypeCtrl_' + fieldName);
	if (ctrls==null) return;
	for (n=0; n<ctrls.length; n++) {
		ctrl = ctrls[n];
		this.attach(ctrl,eventName,ctrl.rowNum-1);
	}
	this.isTableAttached = true;
}

function cgAttachTableMultiSelectBoxEvents() {
	if (!this.tableValid()) return;
	if (this.isSingleControlAttached) throw '(3) Cannot attach to table, object already assigned to single control';
	
	var n;
	var ctrls;
	var ctrl;
	ctrls = this.getTableColumnsControls('_multi_select');
	if (ctrls==null) return;
	for (n=0; n<ctrls.length; n++) {
		ctrl = ctrls[n];
		if (ctrl == null)
			return;
		this.attach(ctrl,"onclick",n+1);
	}
	this.isTableAttached = true;
}

function cgGetTableColumnsControls(column_name) {
	if (!this.tableValid()) return;
	
	var gt = this.table;
	var ctrls = new Array();
	var n;
	var ctrl;
	var cid;
	for (n=this.rowStartOffset; n<=gt.rows.length-1; n+=this.rowStepOffset) {
		cid = this.tableId + '__ctl' + (n+1) + column_name;
		ctrl = this.getControl(cid);
		if (ctrl == null) {
			throw("(4) Cannot find control '" + cid + "'.");
		}
		ctrl.rowNum = n+1;
		ctrl.appliedCtrlFieldId = column_name;
		ctrls[ctrls.length] = ctrl;
	}
	return ctrls;
}

//handles a change event from a field that changes the calculation
function cgAttachedCtrlEventRaised(instance) {
	var calcGridInstance = instance;
	var src = event.srcElement;
	
	ctrls = calcGridInstance.getAppliedControlsArray(src.rowNum);
	if (ctrls==null) return;
	var n,ctrl;
	for (n=0;n<ctrls.length;n++)
	{
		ctrl=ctrls[n];

		var result;
		try {
			result = calcGridInstance.calculate(src.rowNum);
		} catch(e) {
			alert("(13) " + e.message);
			result = '';
		}

		if (calcGridInstance.isInputField(ctrl)) {
			cgSetControlValue(ctrl,result);
		} else {
			ctrl.innerHTML = result;
		}
	}
}

function cgSetControlValue(ctrl,val) {
	try
	{
		ctrl.setValue(val);
	}
	catch(e)
	{
		ctrl.value = val;
	}
}

/****************************************************************************************/

/* Private Methods */

function cgSetTableId(topId)
{
	var control = document.getElementById(topId);
	if (!control) return;
    for (var i = 0; i<control.childNodes.length; i++)
    {
        if (!control.childNodes[i].tagName) continue;
        if (control.childNodes[i].tagName.toLowerCase() == "table")
        {
            this.tableId = control.childNodes[i].id;
        }
    }
}
function cgGetControlType(ctrl) {
	if (ctrl==null) return null;
	if (ctrl.tagName.toLowerCase() == 'input')
		return ctrl.type.toLowerCase();
	else
		return ctrl.tagName.toLowerCase();
}

//attach change event of a field that affects the calculation
function cgAttach(ctrl,eventName,rownum) {
	//ctrl.CalcGridInstance=this;
	var instance = this;
	ctrl.rowNum=rownum+1;
	var lSuccess;
	lSuccess = ctrl.attachEvent(eventName,function() { cgAttachedCtrlEventRaised(instance) });
	if (!lSuccess) {
		alert("(5) Cannot attach to " + eventName + " event of " + ctrl.id + ".");
		return;
	}
	this.attachedCtrls[this.attachedCtrls.length] = ctrl;
}

function cgGetAppliedControlsArray(rowNum) {
	if (this.appliedControlsId == null || this.appliedControlsId.length == 0) return null;
	
	var ctrls = new Array();
	var ctrl;
	var n;
	
	if (this.shouldApplyToTable)
	{
		for (n=0;n<this.appliedControlsId.length;n++)
		{
			var ctrlid = this.tableId + '__ctl' + rowNum + '_DisplayTypeCtrl_' + this.appliedControlsId[n];
			var ctrlgrd;
			ctrl = this.getControl(ctrlid);
			if (ctrl == null) 
			{
				throw("could not find control '" + ctrlid + "'.");
			} 
			if (ctrl.rowNum != rowNum) ctrl.rowNum = rowNum;
			if (ctrl.appliedCtrlFieldId != this.appliedControlsId[n]) ctrl.appliedCtrlFieldId = this.appliedControlsId[n];
			ctrls[ctrls.length] = ctrl;
		}
	}
	else
	{
		for (n=0;n<this.appliedControlsId.length;n++)
		{
			var ctrlid = this.appliedControlsId[n];
			ctrl = document.getElementById(ctrlid);
			if (ctrl != null) ctrls[ctrls.length] = ctrl;
		}
	}
	return ctrls;
}

function cgGetColumnResult(colSum) {
	var gt = this.table;
	
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

function cgGetCellValue(cell) {
	var cellContent = cell.innerHTML;
	var strVal = this.stripHtml(cellContent);
	var val = this.getFloat(strVal);
	return val;
}

function cgEnsureInitialize() {
	if (!this.isInitialized) {
		this.initialize();
		this.isInitialized = true;
	}
}

function cgCalculate(rowNum) {
	document.body.style.cursor = "wait";
	var result = this.calcExpression(rowNum,this.expression);
	document.body.style.cursor = "default";
	return result;
}

function cgCalcExpresssion(rowNum,exp) {
	var expArr = exp.split(';');
	var jsexp = null; //the js expression part
	if (expArr.length > 1) {
		jsexp = expArr.shift();
	} else {
		jsexp = "[0]";
	}
	var paramArr = expArr;
	
	var n,val,paramParts,paramType,paramText;
	var hasServerSideParams,paramEvaluated;
	
	hasServerSideParams = false;
	
	for (n=0;n<paramArr.length;n++)
	{
		paramParts = paramArr[n].split(':');
		paramType = m4nTrim(paramParts[0].toLowerCase());
		paramText = m4nTrim(paramParts[1]);

		if (paramType == "field") {
			val = this.getColValue(rowNum,m4nTrim(paramText));
			if (val == null || val.length == 0) val = '';
			paramEvaluated = true;

		} else if (paramType == "url" ) {
			//get value from URL
			var urlParams = m4nGetURLParams();
			val = urlParams[paramText];
			paramEvaluated = true;
		
		} else if (paramType == "func" || paramType == "session" || paramType == "application") {
			hasServerSideParams = true;
			paramEvaluated = false;
	
		} else {
			val = paramArr[n];
			paramEvaluated = true;
		}
		
		if (paramEvaluated) {
			//replace the param with the value
			paramArr[n] = val;
			//replace placeholder in the js expression
			jsexp = jsexp.replace('[' + (n) + ']',val);
		}
	}
	var result;
	if (hasServerSideParams) {
		result = this.serverSideEval(rowNum, jsexp + ";" + paramArr.join(";"));
	} else {
		try {
			result = eval(jsexp);
		} catch (e) {
			result = jsexp;
		}
	}
	return result;
}

function cgParseServerSideFunction(rowNum,exp) {
	//i.e. exp = "[0]; func: MyFunc(field:fld1, field:fld2)"
	
	var lPos = exp.indexOf("(");
	var rPos = exp.indexOf(")");
	//split params
	var arrParams = exp.substring(lPos+1,rPos).split(",");
	var paramVal;
	for (n=0; n<arrParams.length; n++) {
		paramVal = this.calcExpression(rowNum, m4nTrim(arrParams[n]));
		//replace param with value
		arrParams[n] = paramVal;
	}
	//recreate the expression with param values
	exp = exp.substring(0,lPos+1) + arrParams.join(",") + exp.substr(rPos);
	return exp;
}

function cgServerSideEval(rowNum, exp) {
	exp = this.parseServerSideFunction(rowNum, exp);

	//create post request
	var dataToSend = "expression=" + exp;

	var ssr = new m4nServerSideRequest(m4nExpressionProviderURL, dataToSend);
	this.remoteData = ssr.getResponseAsText();
	var XmlDoc = ssr.getResponseAsXMLDoc(); 
	
	var nodes = XmlDoc.selectNodes("Error");
	if (nodes.length>0) {
		alert("An error occured during the request:\r\n\r\n" + nodes.item(0).text);
		return;
	}

	if (this.onRemoteDataReceived != null && typeof this.onRemoteDataReceived == "function") {
		this.onRemoteDataReceived();
	}

	if (location.search.toUpperCase().indexOf("EXPRESSIONDEBUG=1") > -1) {
		alert(xmlhttp.responseText);
	}
	
	var resultNode = XmlDoc.selectNodes("Root/result");
	if (resultNode.length == 0) {
		alert("The result element of the server side function response was not found.\n\nThe result is:\n\n" + xmlhttp.responseText);
		return "";
	}
	var result = resultNode.item(0).text;
	var completeNode = XmlDoc.selectNodes("Root/complete");
	var complete = completeNode.item(0).text;
	
	return result;
}

function cgEnsureTable() {
	this.table = document.getElementById(this.tableId);
	if (this.table == null) throw '(8) Cannot find table with ID "' + this.tableId + '".';
}

/****************************************************************************************/

/* Private HTML Object methods */

function cgGetGridFooter() {
	var gt = this.table;
	return this.getGridRow(gt.rows.length - 1);
}

function cgGetGridHeader() {
	return this.getGridRow(0);
}

function cgGetGridRow(n) {
	var gt = this.table;
	var row = gt.rows(n);
	return row;
}

function cgGetGridFooterCell(n) {
	var footer = this.getGridFooter();
	return footer.cells(n);
}

function cgGetValueFromControl(ctrl) {
	try {
		return ctrl.getValue();
	} catch(e) {}
	
	if (ctrl.value == undefined) {
		var retdata = this.stripHtml(ctrl.innerHTML);
		return retdata;
	} else {
		return ctrl.value;
	}
}

function cgGetColValue(rowNum,ColumnName){
	var id = this.tableId + '__ctl' + rowNum + '_DisplayTypeCtrl_' + ColumnName;
	ctrl = this.getControl(id);
	if (ctrl == null) throw ("(9) Cannot find control '" + id + "'.");
	//ctrl.rowNum = rowNum;
	//ctrl.appliedCtrlFieldId = ColumnName;
	return this.getValueFromControl(ctrl);
}

function cgGetControl(id) {
	id = m4nTrim(id);
	var ctrl;
	ctrl = document.getElementById(id);
	if (ctrl == null) {
		alert("(10) Cannot find control '" + id + "'.");
		return null;
	}
	if (ctrl.ValueID) {
		ctrl = document.getElementById(ctrl.ValueID);
		if (ctrl == null) {
			alert("(11) Cannot find control '" + ctrl.ValueID + "'.");
		}
	}
	return ctrl;
}

function cgGetGridCellIndex(colHeaderText) {
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

function cgGetFloat(strHTML) {
	var resultStr = strHTML

	var re = /\,/;
	resultStr = resultStr.replace(re,'');
	resultStr = resultStr.replace(re,'');
	resultStr = resultStr.replace(re,'');
	resultStr = resultStr.replace(re,'');

	var re = /\$/;
	resultStr = resultStr.replace(re,'');

	var result = parseFloat(resultStr);
	return result;
}

function cgStripHtml(oldString) {
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
