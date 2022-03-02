/*
Made4Net Version: 2.3

This file handles key strokes and cursor focus in a data object 
*/

/****************************************************************************************/
var KEY_ENTER = 13;	   // save / search
var KEY_F7 = 118; //prev screen
var KEY_F8 = 119; //goto screen
var KEY_F9 = 120; //delete
var KEY_F12 = 123;	//cancel
var KEY_E = 69; 
var KEY_N = 78;  

var KEY_0 = 48; //(exec)
var KEY_1 = 49; //new
var KEY_2 = 50;	//edit
var KEY_3 = 51;	//m-edit
var KEY_4 = 52; //view
var KEY_5 = 53; //find
var KEY_6 = 54; //(exec)
var KEY_7 = 55; //(exec)
var KEY_8 = 56; //(exec)
var KEY_9 = 57; //(exec)

var KEY_NUM_0 = 96; 
var KEY_NUM_1 = 97; 
var KEY_NUM_2 = 98;	
var KEY_NUM_3 = 99;	
var KEY_NUM_4 = 100; 
var KEY_NUM_5 = 101; 
var KEY_NUM_6 = 102; 
var KEY_NUM_7 = 103; 
var KEY_NUM_8 = 104; 
var KEY_NUM_9 = 105; 


document.body.onkeydown = m4nHandleKeyPress;
document.body.onload = m4nSetFocus;

function m4nHandleKeyPress() {
	var kc = event.keyCode;
	//window.status=kc;

	var sender = event.srcElement;
	if (sender==null)
	    if (event.htcSrcElement)
	       sender=event.htcSrcElement;
	//////////////////////////////////////
	//Goto Screen
	if (kc == KEY_F8) {
		//focus on box
		var ctrl = m4nGetElement("GotoScreen_tb");
		if (ctrl != null) {
			ctrl.focus();
			ctrl.select();
		}
		return;
	
	} else if (sender.id == "GotoScreen_tb" && kc == KEY_ENTER) {
		event.returnValue = false;
		var ctrl = m4nGetElement("GotoScreen_tb");
		if (ctrl.value.length > 0) {
			//redirect
			var btn = m4nGetElement("btnGotoScreen");
			btn.click();
		}
		return;

	}
	//////////////////////////////////////

	// do not override enter button in textarea
	if (sender.tagName == 'TEXTAREA' && kc == KEY_ENTER && event.shiftKey) return
	
	//////////////////////////////////////

	var teId = m4nGetParentTEID(sender);
	if (teId == null) {
		teId = m4nGetFirstTEID();
	}
	if (teId == null) return;
	if (kc == KEY_ENTER) {
		//Enter button action
		event.returnValue = false;
		var btn = m4nGetElement(m4nGetDefaultButtonID(sender));
		if (btn != null) {
			var ce = document.activeElement;
			try {btn.focus();} catch(e) {}
			try {btn.click();} catch(e) {}
			if (!(event.htcSrcElement)) try {ce.focus(); } catch(e) {}  //Do not fire back HTC component event - on IE9 page will crash
		}
	
	} else {
		
		var btnName;
		var btnId;
		var num = m4nKeyCodeToNum(kc);
		
		if (num > 0 && event.ctrlKey) {
			btnId = eval('ExecBtn' + (num - 1));
		} else if (kc == KEY_F9) {
			btnName = "Delete"
		} else if (kc == KEY_F12) {
			btnName = "Cancel"
		}		
		if (btnName != null || btnId != null) {
			if (btnId == null) {
				btnId = m4nGetButtonID(teId, btnName);
			}
			var btn = m4nGetElement(btnId);
			event.returnValue = false;
			var ce = document.activeElement;
			try {btn.focus();} catch(e) {}
			try {btn.click();} catch(e) {}
			try {ce.focus(); } catch(e) {}
		}
	}
}

function m4nKeyCodeToNum(keyCode) {
	var num = -1;
	if (keyCode >= KEY_0 && keyCode <= KEY_9) {
		num = keyCode - KEY_0;
	} else if (keyCode >= KEY_NUM_0 && keyCode <= KEY_NUM_9) {
		num = keyCode - KEY_NUM_0;
	}
	return num;
}

function m4nGetFirstTEID() {
	if (Page_TEs.length > 0) {
		return Page_TEs[0];
	} else {
		return null;
	}
}

function m4nGetTEIndex(TEID) {
	var i;
	for (i in Page_TEs) {
		s = Page_TEs[i];
		if (TEID == s) {
			return i;
		}
	}
}

function m4nGetDefaultButtonID(element) {
	return Page_TEDefaultButtons[m4nGetTEIndex(m4nGetParentTEID(element))]
}

function m4nGetElement(id) {
	return document.getElementById(id);
}

function m4nGetButtonID(TEID, btnName) {
	return TEID + "_ActionBar_btn" + btnName + "_InnerButton";
}

function m4nSetFocus() {
	try {
		if (m4nDisableFocusOnLoad) return;
	} catch(e) {}
	
	var TECount = 0;
	try {
		TECount = Page_TEs.length;
	} catch (e) {}
	
	var el = m4nGetFocusElements();

	if (TECount == 0) {
		m4nSetFocus2(el,false);
	} else {
		if (!m4nSetFocus2(el,true)) {
			m4nSetFocus2(el,false);
		}
	}
}

function m4nSetFocus2(el, teOnly) {
	var e;
	for(i=0; i<el.length; i++) {
		e = el[i];
		if (teOnly) {
			try {
				parTE = m4nGetParentTEID(e);
				if (parTE != null) {
					try {e.focus();} catch(e){}
					return true;
				}
			} catch(ex) {
			}
		} else {
			try {e.focus();} catch(e){}
			return true;
		}
	}
	return false;
}

//returns an array of all objects that can receive the focus
//and sorts them by the sourceIndex
function m4nGetFocusElements() {
	var e = new Array();
	m4nAddElements(e, "INPUT");	
	m4nAddElements(e, "SELECT");	
	m4nAddElements(e, "TypeAhead");	
	m4nAddElements(e, "TEXTAREA");	
	m4nAddElements(e, "ComboBox");	
	
	e = e.sort(m4nSortBySourceIndex);
	return e;
}

//sorting function
//sorts elements by their sourceIndex property
function m4nSortBySourceIndex(x,y) {
	xsi = x.sourceIndex;
	ysi = y.sourceIndex
	if (xsi > ysi) return 1;
	else if (xsi < ysi) return -1;
	else return 0;
}

//adds all elements with the provided tagName
//that can receive focus to the [arr] array
function m4nAddElements(arr, tagName) {
	//inps = document.all.tags(tagName);
	inps = document.getElementsByTagName(tagName);
	var startIndex = arr.length;
	for (i=0; i<inps.length; i++) {
		var inp = inps[i];
		if (inp.type != "image" && inp.type != "submit" && inp.type != "hidden" && inp.id != "GotoScreen_tb" && inp.readOnly != true && inp.disabled != true && inp.isDisabled != true && inp.style.display != "none") {
			arr[i+startIndex] = inps[i];
		}
	}
}
