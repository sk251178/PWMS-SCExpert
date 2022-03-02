function m4nDateBox_FormatConversionScript(inputObject) {

	var reD = /^\d{1}$/;
	var reDD = /^\d{2}$/;
	var reMMDD = /^\d{4}$/;
	var reMMDDYY = /^\d{6}$/;
	var reMMDDYYYY = /^\d{8}$/;

	var reM_D_YY = /^\d{1}[\.\-\\\/]\d{1}[\.\-\\\/]\d{2}$/;
	var reMM_D_YY = /^\d{2}[\.\-\\\/]\d{1}[\.\-\\\/]\d{2}$/;
	var reM_DD_YY = /^\d{1}[\.\-\\\/]\d{2}[\.\-\\\/]\d{2}$/;
	var reMM_DD_YY = /^\d{2}[\.\-\\\/]\d{2}[\.\-\\\/]\d{2}$/;
	
	var reM_D_YYYY = /^\d{1}[\.\-\\\/]\d{1}[\.\-\\\/]\d{4}$/;
	var reMM_D_YYYY = /^\d{2}[\.\-\\\/]\d{1}[\.\-\\\/]\d{4}$/;
	var reM_DD_YYYY = /^\d{1}[\.\-\\\/]\d{2}[\.\-\\\/]\d{4}$/;
	var reMM_DD_YYYY = /^\d{2}[\.\-\\\/]\d{2}[\.\-\\\/]\d{4}$/;

	var reYYYY_D_M = /^\d{4}[\.\-\\\/]\d{1}[\.\-\\\/]\d{1}$/;
	var reYYYY_D_MM = /^\d{4}[\.\-\\\/]\d{1}[\.\-\\\/]\d{2}$/;
	var reYYYY_DD_M  = /^\d{4}[\.\-\\\/]\d{2}[\.\-\\\/]\d{1}$/;
	var reYYYY_DD_MM = /^\d{4}[\.\-\\\/]\d{2}[\.\-\\\/]\d{2}$/;
	
	
	var m; var d; var y;
	var resultDate = null;
	strDate = inputObject.value;
	d = new Date();
	currYear = d.getFullYear();
	currMonth = d.getMonth() + 1;
	
	if (strDate.search(reD) == 0) {
		d = strDate;
		m = currMonth;
		y = currYear;
	}
	
	if (strDate.search(reDD) == 0) {
		d = strDate;
		m = currMonth;
		y = currYear;
	}
	
	if (strDate.search(reMMDD) == 0) {
		m = m4nDateBox_ParseMonth(strDate, 0, 2, 2, 2)
		d = m4nDateBox_ParseDay(strDate, 0, 2, 2, 2)
		y = currYear;
	}
	
	if (strDate.search(reMMDDYY) == 0) {
		resultDate = m4nDateBox_ParseDate(strDate, 0, 2, 2, 2, 4, 2)
	}
	
	if (strDate.search(reMMDDYYYY) == 0) {
		resultDate = m4nDateBox_ParseDate(strDate, 0, 2, 2, 2, 4, 4)
	}

	if (strDate.search(reMM_DD_YYYY) == 0) {
		resultDate = m4nDateBox_ParseDate(strDate, 0, 3, 2, 2, 6, 4)
	}

	if (strDate.search(reM_D_YY) == 0) {
		resultDate = m4nDateBox_ParseDate(strDate, 0, 2, 1, 1, 4, 2)
	}
	
	if (strDate.search(reMM_D_YY) == 0) {
		resultDate = m4nDateBox_ParseDate(strDate, 0, 3, 2, 1, 5, 2)
	}
	
	if (strDate.search(reM_DD_YY) == 0) {
		resultDate = m4nDateBox_ParseDate(strDate, 0, 2, 1, 2, 5, 2)
	}
	
	if (strDate.search(reMM_DD_YY) == 0) {
		resultDate = m4nDateBox_ParseDate(strDate, 0, 3, 2, 2, 6, 2)
	}
	
	if (strDate.search(reM_D_YYYY) == 0) {
		resultDate = m4nDateBox_ParseDate(strDate, 0, 2, 1, 1, 4, 4)
	}
	
	if (strDate.search(reMM_D_YYYY) == 0) {
		resultDate = m4nDateBox_ParseDate(strDate, 0, 3, 2, 1, 5, 4)
	}
	
	if (strDate.search(reM_DD_YYYY) == 0) {
		resultDate = m4nDateBox_ParseDate(strDate, 0, 2, 1, 2, 5, 4)
	}

	if (strDate.search(reYYYY_D_M) == 0) {
		resultDate = m4nDateBox_ParseDate(strDate, 5, 7, 1, 1, 0, 4)
	}

	if (strDate.search(reYYYY_D_MM) == 0) {
		resultDate = m4nDateBox_ParseDate(strDate, 5, 7, 1, 2, 0, 4)
	}

	if (strDate.search(reYYYY_DD_M) == 0) {
		resultDate = m4nDateBox_ParseDate(strDate, 5, 8, 2, 1, 0, 4)
	}

	if (strDate.search(reYYYY_DD_MM) == 0) {
		resultDate = m4nDateBox_ParseDate(strDate, 5, 8, 2, 2, 0, 4)
	}
	
	if (resultDate == null) {
		resultDate = new Date(y,m-1,d);
	} else {
		y = resultDate.getFullYear();
		m = resultDate.getMonth()+1;
		d = resultDate.getDate();
	}
	
	//check if date translates into same numbers user entered
	if (resultDate.getFullYear() == y && resultDate.getMonth() == m-1 && resultDate.getDate() == d) {
		resultStr = m4nDateBox_FormatDate(resultDate);
		inputObject.value = resultStr;
	}
}

function m4nDateBox_ParseDate(strDate, pos1, pos2, pos1Len, pos2Len, posY, posYLen) {
	var result;
	m = m4nDateBox_ParseMonth(strDate, pos1, pos2, pos1Len, pos2Len);
	d = m4nDateBox_ParseDay(strDate, pos1, pos2, pos1Len, pos2Len);
	strY = strDate.substr(posY, posYLen);
	if (posYLen == 2) {
		y = m4nDateBox_Parse2DigitYear(strY);
	} else {
		y = parseInt(strY)
	}
	
	if (m >=1 && m <= 12) {
		result = new Date(y,m-1,d);
	}
	
	return result;
}

function m4nDateBox_ParseDay(strDate, pos1, pos2, pos1Len, pos2Len) {
	var startPos;
	var len;
                  var m4nDateFormatCLR;
	m4nDateFormatCLR=m4nDateFormat.replace(/\./g,"").replace(/-/g,"").replace(/\//g,"").toLowerCase();
	if (m4nDateFormatCLR == "ddmmyyyy") {
		startPos = pos1;
		len = pos1Len;
	} else {
		startPos = pos2;
		len = pos2Len;
	}
	return strDate.substr(startPos,len);
}

function m4nDateBox_ParseMonth(strDate, pos1, pos2, pos1Len, pos2Len) {
	var startPos;
	var m4nDateFormatCLR;
	m4nDateFormatCLR=m4nDateFormat.replace(/\./g,"").replace(/-/g,"").replace(/\//g,"").toLowerCase();
	if (m4nDateFormatCLR == "ddmmyyyy") {
		startPos = pos2;
		len = pos2Len;
	} else {
		startPos = pos1;
		len = pos1Len;
	}
	return strDate.substr(startPos,len);
}

function m4nDateBox_FormatDate(pDate) {
	//if (m4nDateFormat == "ddmmyyyy") {
	//	alert(pDate.getDate());
	//	return pDate.getDate() + "/" + (pDate.getMonth()+1) + "/" + pDate.getFullYear();
	//} else {
	//	alert(pDate.getMonth());
	//	return pDate.getMonth() + 1 + "/" + pDate.getDate() + "/" + pDate.getFullYear();
	//}
	var strDate=m4nDateFormat;
	var d=pDate.getDate().toString();
	var m=(pDate.getMonth() + 1).toString();
	var y=pDate.getFullYear().toString();
	if (strDate.indexOf("dd")>-1) {
		if (d.length>1) {
			strDate=strDate.replace("dd",d);
		} else {
			strDate=strDate.replace("dd","0"+d);
		 }
	} else {
		strDate=strDate.replace("d",d);
	}
	if (strDate.indexOf("mm")>-1) {
		if (m.length>1) {
			strDate=strDate.replace("mm",m);
		} else {
			strDate=strDate.replace("mm","0"+m);
		 }
	} else {
		strDate=strDate.replace("m",m);
	}
	if (strDate.indexOf("yyyy")>-1) {
		strDate=strDate.replace("yyyy",y);
	} else {
		strDate=strDate.replace("yy",y.substr(2,2));
	}
	return strDate;
}

function m4nDateBox_Parse2DigitYear(strY) {
	if (strY.substr(0,1) == "0") strY = strY.substr(1,1);
	intY = parseInt(strY);
	if (intY > 29) {
		y = 1900 + intY;
	} else {
		y = 2000 + intY;
	}
	return y;
}
