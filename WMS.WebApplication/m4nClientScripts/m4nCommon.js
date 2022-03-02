/*
Made4Net Version: 3.2

This file contains common script functions
*/

/****************************************************************************************/

/* Exceptions */

//exception super class all custom exceptions should inherit from
function m4nException(msg)
{
	this.msg = msg;
	//this.stackTrace = stacktrace();
}
/*
function stacktrace() {
	var s = "";
	for (var a = arguments.caller; a !=null; a = a.caller) {
		s += "->"+funcname(a.callee) + "\n";
		if (a.caller == a) {s+="*"; break;}
	}
	return s;
}
function funcname(f) {
 var s = f.toString().match(/function (\w*)/)[1];
 if ((s == null) || (s.length==0)) return "anonymous";
 return s;
}
*/

function m4nArgumentException(argName, msg)
{
	this.argName = argName;
	this.msg = msg;
}
m4nArgumentException.prototype = new m4nException();

function m4nArgumentNullException(argName, msg)
{
	this.argName = argName;
	this.msg = "Parameter '" + this.argName + "' cannot be null.";
	
	if (!m4nIsEmpty(msg))
	{
		this.msg = msg;
	}
}
m4nArgumentNullException.prototype = new m4nArgumentException();


/****************************************************************************************/

function m4nGetParentTEID(element) {
	var i = m4nGetParentTEIndex(element);
	return Page_TEs[i];
}

function m4nGetParentTEObjectID(element) {
	var i = m4nGetParentTEIndex(element);
	return Page_TEObjectIDs[i];
}

function m4nGetParentTEIndex(element) {
	var i;
	var s;
	var len;
	var id = element.id;
	for (i in Page_TEs) {
		s = Page_TEs[i];
		len = s.length;
		if (id == s || id.substring(0,len + 1) == s + "_") {
			return i;
		}
	}
	return null;
}

function m4nGetDisplayTypeValue(te, fld) {
	try {
		var obj = m4nGetDisplayTypeObject(te, fld);
		return obj.value;
	} catch (e) {
		alert("m4nGetDisplayTypeValue: " + e);
	}
}

function m4nGetDisplayTypeObject(te, fld) {
	if (m4nIsEmpty(te))
	{
		throw new m4nArgumentNullException("te");
	}
	if (m4nIsEmpty(fld))
	{
		throw new m4nArgumentNullException("fld");
	}
	try {
		var teMode = m4nGetTEMode(te);
		te = te.toUpperCase();
		fld = fld.toUpperCase();
		var tags = document.getElementsByTagName("input");
		var h; var dtype;
		for (i=0; i<=tags.length-1; i++) {
			if (tags[i].type == "hidden") {
				if (tags[i].name.toUpperCase() == "DO:" + te + ":" + fld) {
					h = tags[i];
					break;
				}
			}
		}
		if (h == null) {
			throw("Field '" + fld + "' not found in object '" + te + "'");
		}
		dtype = document.getElementById(h.value);
		if (m4nBrowserName=='Explorer' && m4nBrowserVersion>8)
		{
		dtype.ClassName=dtype.attributes['ClassName'].value;
		dtype.ValueID=dtype.attributes['ValueID'].value;
		}
		var vid;
		if (dtype.ClassName) {
			vid = dtype.ValueID;
		var className = dtype.ClassName;
		if (vid.length == 0) {
			throw("Class " + className + " must override property ValueID and return the ClientID of the control containing the value of the object.");
		}
		} else {
			vid = dtype.id;
		}
		var obj = document.getElementById(vid);
		return obj;
	} catch (e) {
		alert("m4nGetDisplayTypeObject: " + e);
	}
}

function m4nGetTEMode(te) {
	try {
		for (n=0; n<=Page_TEs.length-1; n++) {
			if (Page_TEs[n] == te) {
				return Page_TEModes[n]
			}
		}
		return null;
	} catch (e) {
		alert("m4nGetTEMode: " + e);
	}
}

function m4nTrim(s) {
	if (s == null) return '';
	s = s.toString();	
	while(''+s.charAt(0)==' ') {
		s=s.substring(1,s.length);
	}
	while(''+s.charAt(s.length-1)==' ') {
		s=s.substring(0,s.length-1);
	}
	return s;
}

function m4nGetURLParams() {
	var query = window.location.search.substring(1);
	var vars = query.split("&");
	var result = new Array();
	var paramName;
	for (var i=0; i<vars.length; i++) {
		pair = vars[i].split("=");
		paramName = pair[0];
		if (pair.length == 2) {
			result[paramName] = pair[1];
		} else {
			result[paramName] = null;
		}
	}
	return result;
}

function m4nStripHtml(oldString) {
	if (oldString == undefined) return oldString;

   oldString = oldString.toString();
   var newString = "";
   var inTag = false;
   for( var i = 0; i <= oldString.length-1; i++ )
   {
        if( oldString.charAt(i) == '<' ) inTag = true;
        if( !inTag ) newString += oldString.charAt(i);
        if( oldString.charAt(i) == '>' ) inTag = false;
   }
   return newString;
}

function m4nGetFloat(strHTML) {
	if (strHTML == undefined) return strHTML;
	var str = m4nStripHtml(strHTML)

	var re = /[^0-9|^\.^\-]/g;
	var pos;
	str = str.replace(re, '');
	var result = parseFloat(str);
	return result;
}

function m4nParseBool(val) {
	try {
		if (val > 0 || val < 0 || val.toLowerCase() == "true" || val == "1" || val == "-1" || val.toLowerCase() == "on" || val.toLowerCase() == "yes") {
			return true;
		}
	} catch(e) {}
	return false;
}

function m4nGetInputType(ctrl)
{
	var tag = ctrl.tagName.toUpperCase();
	if (tag == "INPUT")
	{
		return ctrl.type;
	}
	else if (tag == "SELECT" || tag == "TypeAhead" || tag == "TEXTAREA")
	{
		return tag;
	}
	else
	{
		return null;
	}
}

function m4nURLEncode(strText) {                             
	var isObj;
	var trimReg;
	if( typeof(strText) == "string" ) {
		if( strText != null ) {
			trimReg = /(^\s+)|(\s+$)/g;
			strText = strText.replace( trimReg, '');
			for(i=32;i<256;i++) {               
				strText = strText.replace(String.fromCharCode(i),escape(String.fromCharCode(i)));                                                                 
			}
		}
	}          
	return strText;
}

function m4nHTMLDecode(s) 
{ 
      var out = ""; 
      if (s==null) return; 
  
      var l = s.length; 
      for (var i=0; i<l; i++) 
      { 
            var ch = s.charAt(i); 
            
            if (ch == '&') 
            { 
                  var semicolonIndex = s.indexOf(';', i+1); 
                  
            if (semicolonIndex > 0) 
            { 
                        var entity = s.substring(i + 1, semicolonIndex); 
                        if (entity.length > 1 && entity.charAt(0) == '#') 
                        { 
                              if (entity.charAt(1) == 'x' || entity.charAt(1) == 'X') 
                                    ch = String.fromCharCode(eval('0'+entity.substring(1))); 
                              else 
                                    ch = String.fromCharCode(eval(entity.substring(1))); 
                        } 
                    else 
                      { 
                              switch (entity) 
                              { 
                                    case 'quot': ch = String.fromCharCode(0x0022); break; 
                                    case 'amp': ch = String.fromCharCode(0x0026); break; 
                                    case 'lt': ch = String.fromCharCode(0x003c); break; 
                                    case 'gt': ch = String.fromCharCode(0x003e); break; 
                                    case 'nbsp': ch = String.fromCharCode(0x00a0); break; 
                                    case 'iexcl': ch = String.fromCharCode(0x00a1); break; 
                                    case 'cent': ch = String.fromCharCode(0x00a2); break; 
                                    case 'pound': ch = String.fromCharCode(0x00a3); break; 
                                    case 'curren': ch = String.fromCharCode(0x00a4); break; 
                                    case 'yen': ch = String.fromCharCode(0x00a5); break; 
                                    case 'brvbar': ch = String.fromCharCode(0x00a6); break; 
                                    case 'sect': ch = String.fromCharCode(0x00a7); break; 
                                    case 'uml': ch = String.fromCharCode(0x00a8); break; 
                                    case 'copy': ch = String.fromCharCode(0x00a9); break; 
                                    case 'ordf': ch = String.fromCharCode(0x00aa); break; 
                                    case 'laquo': ch = String.fromCharCode(0x00ab); break; 
                                    case 'not': ch = String.fromCharCode(0x00ac); break; 
                                    case 'shy': ch = String.fromCharCode(0x00ad); break; 
                                    case 'reg': ch = String.fromCharCode(0x00ae); break; 
                                    case 'macr': ch = String.fromCharCode(0x00af); break; 
                                    case 'deg': ch = String.fromCharCode(0x00b0); break; 
                                    case 'plusmn': ch = String.fromCharCode(0x00b1); break; 
                                    case 'sup2': ch = String.fromCharCode(0x00b2); break; 
                                    case 'sup3': ch = String.fromCharCode(0x00b3); break; 
                                    case 'acute': ch = String.fromCharCode(0x00b4); break; 
                                    case 'micro': ch = String.fromCharCode(0x00b5); break; 
                                    case 'para': ch = String.fromCharCode(0x00b6); break; 
                                    case 'middot': ch = String.fromCharCode(0x00b7); break; 
                                    case 'cedil': ch = String.fromCharCode(0x00b8); break; 
                                    case 'sup1': ch = String.fromCharCode(0x00b9); break; 
                                    case 'ordm': ch = String.fromCharCode(0x00ba); break; 
                                    case 'raquo': ch = String.fromCharCode(0x00bb); break; 
                                    case 'frac14': ch = String.fromCharCode(0x00bc); break; 
                                    case 'frac12': ch = String.fromCharCode(0x00bd); break; 
                                    case 'frac34': ch = String.fromCharCode(0x00be); break; 
                                    case 'iquest': ch = String.fromCharCode(0x00bf); break; 
                                    case 'Agrave': ch = String.fromCharCode(0x00c0); break; 
                                    case 'Aacute': ch = String.fromCharCode(0x00c1); break; 
                                    case 'Acirc': ch = String.fromCharCode(0x00c2); break; 
                                    case 'Atilde': ch = String.fromCharCode(0x00c3); break; 
                                    case 'Auml': ch = String.fromCharCode(0x00c4); break; 
                                    case 'Aring': ch = String.fromCharCode(0x00c5); break; 
                                    case 'AElig': ch = String.fromCharCode(0x00c6); break; 
                                    case 'Ccedil': ch = String.fromCharCode(0x00c7); break; 
                                    case 'Egrave': ch = String.fromCharCode(0x00c8); break; 
                                    case 'Eacute': ch = String.fromCharCode(0x00c9); break; 
                                    case 'Ecirc': ch = String.fromCharCode(0x00ca); break; 
                                    case 'Euml': ch = String.fromCharCode(0x00cb); break; 
                                    case 'Igrave': ch = String.fromCharCode(0x00cc); break; 
                                    case 'Iacute': ch = String.fromCharCode(0x00cd); break; 
                                    case 'Icirc': ch = String.fromCharCode(0x00ce ); break; 
                                    case 'Iuml': ch = String.fromCharCode(0x00cf); break; 
                                    case 'ETH': ch = String.fromCharCode(0x00d0); break; 
                                    case 'Ntilde': ch = String.fromCharCode(0x00d1); break; 
                                    case 'Ograve': ch = String.fromCharCode(0x00d2); break; 
                                    case 'Oacute': ch = String.fromCharCode(0x00d3); break; 
                                    case 'Ocirc': ch = String.fromCharCode(0x00d4); break; 
                                    case 'Otilde': ch = String.fromCharCode(0x00d5); break; 
                                    case 'Ouml': ch = String.fromCharCode(0x00d6); break; 
                                    case 'times': ch = String.fromCharCode(0x00d7); break; 
                                    case 'Oslash': ch = String.fromCharCode(0x00d8); break; 
                                    case 'Ugrave': ch = String.fromCharCode(0x00d9); break; 
                                    case 'Uacute': ch = String.fromCharCode(0x00da); break; 
                                    case 'Ucirc': ch = String.fromCharCode(0x00db); break; 
                                    case 'Uuml': ch = String.fromCharCode(0x00dc); break; 
                                    case 'Yacute': ch = String.fromCharCode(0x00dd); break; 
                                    case 'THORN': ch = String.fromCharCode(0x00de); break; 
                                    case 'szlig': ch = String.fromCharCode(0x00df); break; 
                                    case 'agrave': ch = String.fromCharCode(0x00e0); break; 
                                    case 'aacute': ch = String.fromCharCode(0x00e1); break; 
                                    case 'acirc': ch = String.fromCharCode(0x00e2); break; 
                                    case 'atilde': ch = String.fromCharCode(0x00e3); break; 
                                    case 'auml': ch = String.fromCharCode(0x00e4); break; 
                                    case 'aring': ch = String.fromCharCode(0x00e5); break; 
                                    case 'aelig': ch = String.fromCharCode(0x00e6); break; 
                                    case 'ccedil': ch = String.fromCharCode(0x00e7); break; 
                                    case 'egrave': ch = String.fromCharCode(0x00e8); break; 
                                    case 'eacute': ch = String.fromCharCode(0x00e9); break; 
                                    case 'ecirc': ch = String.fromCharCode(0x00ea); break; 
                                    case 'euml': ch = String.fromCharCode(0x00eb); break; 
                                    case 'igrave': ch = String.fromCharCode(0x00ec); break; 
                                    case 'iacute': ch = String.fromCharCode(0x00ed); break; 
                                    case 'icirc': ch = String.fromCharCode(0x00ee); break; 
                                    case 'iuml': ch = String.fromCharCode(0x00ef); break; 
                                    case 'eth': ch = String.fromCharCode(0x00f0); break; 
                                    case 'ntilde': ch = String.fromCharCode(0x00f1); break; 
                                    case 'ograve': ch = String.fromCharCode(0x00f2); break; 
                                    case 'oacute': ch = String.fromCharCode(0x00f3); break; 
                                    case 'ocirc': ch = String.fromCharCode(0x00f4); break; 
                                    case 'otilde': ch = String.fromCharCode(0x00f5); break; 
                                    case 'ouml': ch = String.fromCharCode(0x00f6); break; 
                                    case 'divide': ch = String.fromCharCode(0x00f7); break; 
                                    case 'oslash': ch = String.fromCharCode(0x00f8); break; 
                                    case 'ugrave': ch = String.fromCharCode(0x00f9); break; 
                                    case 'uacute': ch = String.fromCharCode(0x00fa); break; 
                                    case 'ucirc': ch = String.fromCharCode(0x00fb); break; 
                                    case 'uuml': ch = String.fromCharCode(0x00fc); break; 
                                    case 'yacute': ch = String.fromCharCode(0x00fd); break; 
                                    case 'thorn': ch = String.fromCharCode(0x00fe); break; 
                                    case 'yuml': ch = String.fromCharCode(0x00ff); break; 
                                    case 'OElig': ch = String.fromCharCode(0x0152); break; 
                                    case 'oelig': ch = String.fromCharCode(0x0153); break; 
                                    case 'Scaron': ch = String.fromCharCode(0x0160); break; 
                                    case 'scaron': ch = String.fromCharCode(0x0161); break; 
                                    case 'Yuml': ch = String.fromCharCode(0x0178); break; 
                                    case 'fnof': ch = String.fromCharCode(0x0192); break; 
                                    case 'circ': ch = String.fromCharCode(0x02c6); break; 
                                    case 'tilde': ch = String.fromCharCode(0x02dc); break; 
                                    case 'Alpha': ch = String.fromCharCode(0x0391); break; 
                                    case 'Beta': ch = String.fromCharCode(0x0392); break; 
                                    case 'Gamma': ch = String.fromCharCode(0x0393); break; 
                                    case 'Delta': ch = String.fromCharCode(0x0394); break; 
                                    case 'Epsilon': ch = String.fromCharCode(0x0395); break; 
                                    case 'Zeta': ch = String.fromCharCode(0x0396); break; 
                                    case 'Eta': ch = String.fromCharCode(0x0397); break; 
                                    case 'Theta': ch = String.fromCharCode(0x0398); break; 
                                    case 'Iota': ch = String.fromCharCode(0x0399); break; 
                                    case 'Kappa': ch = String.fromCharCode(0x039a); break; 
                                    case 'Lambda': ch = String.fromCharCode(0x039b); break; 
                                    case 'Mu': ch = String.fromCharCode(0x039c); break; 
                                    case 'Nu': ch = String.fromCharCode(0x039d); break; 
                                    case 'Xi': ch = String.fromCharCode(0x039e); break; 
                                    case 'Omicron': ch = String.fromCharCode(0x039f); break; 
                                    case 'Pi': ch = String.fromCharCode(0x03a0); break; 
                                    case ' Rho ': ch = String.fromCharCode(0x03a1); break; 
                                    case 'Sigma': ch = String.fromCharCode(0x03a3); break; 
                                    case 'Tau': ch = String.fromCharCode(0x03a4); break; 
                                    case 'Upsilon': ch = String.fromCharCode(0x03a5); break; 
                                    case 'Phi': ch = String.fromCharCode(0x03a6); break; 
                                    case 'Chi': ch = String.fromCharCode(0x03a7); break; 
                                    case 'Psi': ch = String.fromCharCode(0x03a8); break; 
                                    case 'Omega': ch = String.fromCharCode(0x03a9); break; 
                                    case 'alpha': ch = String.fromCharCode(0x03b1); break; 
                                    case 'beta': ch = String.fromCharCode(0x03b2); break; 
                                    case 'gamma': ch = String.fromCharCode(0x03b3); break; 
                                    case 'delta': ch = String.fromCharCode(0x03b4); break; 
                                    case 'epsilon': ch = String.fromCharCode(0x03b5); break; 
                                    case 'zeta': ch = String.fromCharCode(0x03b6); break; 
                                    case 'eta': ch = String.fromCharCode(0x03b7); break; 
                                    case 'theta': ch = String.fromCharCode(0x03b8); break; 
                                    case 'iota': ch = String.fromCharCode(0x03b9); break; 
                                    case 'kappa': ch = String.fromCharCode(0x03ba); break; 
                                    case 'lambda': ch = String.fromCharCode(0x03bb); break; 
                                    case 'mu': ch = String.fromCharCode(0x03bc); break; 
                                    case 'nu': ch = String.fromCharCode(0x03bd); break; 
                                    case 'xi': ch = String.fromCharCode(0x03be); break; 
                                    case 'omicron': ch = String.fromCharCode(0x03bf); break; 
                                    case 'pi': ch = String.fromCharCode(0x03c0); break; 
                                    case 'rho': ch = String.fromCharCode(0x03c1); break; 
                                    case 'sigmaf': ch = String.fromCharCode(0x03c2); break; 
                                    case 'sigma': ch = String.fromCharCode(0x03c3); break; 
                                    case 'tau': ch = String.fromCharCode(0x03c4); break; 
                                    case 'upsilon': ch = String.fromCharCode(0x03c5); break; 
                                    case 'phi': ch = String.fromCharCode(0x03c6); break; 
                                    case 'chi': ch = String.fromCharCode(0x03c7); break; 
                                    case 'psi': ch = String.fromCharCode(0x03c8); break; 
                                    case 'omega': ch = String.fromCharCode(0x03c9); break; 
                                    case 'thetasym': ch = String.fromCharCode(0x03d1); break; 
                                    case 'upsih': ch = String.fromCharCode(0x03d2); break; 
                                    case 'piv': ch = String.fromCharCode(0x03d6); break; 
                                    case 'ensp': ch = String.fromCharCode(0x2002); break; 
                                    case 'emsp': ch = String.fromCharCode(0x2003); break; 
                                    case 'thinsp': ch = String.fromCharCode(0x2009); break; 
                                    case 'zwnj': ch = String.fromCharCode(0x200c); break; 
                                    case 'zwj': ch = String.fromCharCode(0x200d); break; 
                                    case 'lrm': ch = String.fromCharCode(0x200e); break; 
                                    case 'rlm': ch = String.fromCharCode(0x200f); break; 
                                    case 'ndash': ch = String.fromCharCode(0x2013); break; 
                                    case 'mdash': ch = String.fromCharCode(0x2014); break; 
                                    case 'lsquo': ch = String.fromCharCode(0x2018); break; 
                                    case 'rsquo': ch = String.fromCharCode(0x2019); break; 
                                    case 'sbquo': ch = String.fromCharCode(0x201a); break; 
                                    case 'ldquo': ch = String.fromCharCode(0x201c); break; 
                                    case 'rdquo': ch = String.fromCharCode(0x201d); break; 
                                    case 'bdquo': ch = String.fromCharCode(0x201e); break; 
                                    case 'dagger': ch = String.fromCharCode(0x2020); break; 
                                    case 'Dagger': ch = String.fromCharCode(0x2021); break; 
                                    case 'bull': ch = String.fromCharCode(0x2022); break; 
                                    case 'hellip': ch = String.fromCharCode(0x2026); break; 
                                    case 'permil': ch = String.fromCharCode(0x2030); break; 
                                    case 'prime': ch = String.fromCharCode(0x2032); break; 
                                    case 'Prime': ch = String.fromCharCode(0x2033); break; 
                                    case 'lsaquo': ch = String.fromCharCode(0x2039); break; 
                                    case 'rsaquo': ch = String.fromCharCode(0x203a); break; 
                                    case 'oline': ch = String.fromCharCode(0x203e); break; 
                                    case 'frasl': ch = String.fromCharCode(0x2044); break; 
                                    case 'euro': ch = String.fromCharCode(0x20ac); break; 
                                    case 'image': ch = String.fromCharCode(0x2111); break; 
                                    case 'weierp': ch = String.fromCharCode(0x2118); break; 
                                    case 'real': ch = String.fromCharCode(0x211c); break; 
                                    case 'trade': ch = String.fromCharCode(0x2122); break; 
                                    case 'alefsym': ch = String.fromCharCode(0x2135); break; 
                                    case 'larr': ch = String.fromCharCode(0x2190); break; 
                                    case 'uarr': ch = String.fromCharCode(0x2191); break; 
                                    case 'rarr': ch = String.fromCharCode(0x2192); break; 
                                    case 'darr': ch = String.fromCharCode(0x2193); break; 
                                    case 'harr': ch = String.fromCharCode(0x2194); break; 
                                    case 'crarr': ch = String.fromCharCode(0x21b5); break; 
                                    case 'lArr': ch = String.fromCharCode(0x21d0); break; 
                                    case 'uArr': ch = String.fromCharCode(0x21d1); break; 
                                    case 'rArr': ch = String.fromCharCode(0x21d2); break; 
                                    case 'dArr': ch = String.fromCharCode(0x21d3); break; 
                                    case 'hArr': ch = String.fromCharCode(0x21d4); break; 
                                    case 'forall': ch = String.fromCharCode(0x2200); break; 
                                    case 'part': ch = String.fromCharCode(0x2202); break; 
                                    case 'exist': ch = String.fromCharCode(0x2203); break; 
                                    case 'empty': ch = String.fromCharCode(0x2205); break; 
                                    case 'nabla': ch = String.fromCharCode(0x2207); break; 
                                    case 'isin': ch = String.fromCharCode(0x2208); break; 
                                    case 'notin': ch = String.fromCharCode(0x2209); break; 
                                    case 'ni': ch = String.fromCharCode(0x220b); break; 
                                    case 'prod': ch = String.fromCharCode(0x220f); break; 
                                    case 'sum': ch = String.fromCharCode(0x2211); break; 
                                    case 'minus': ch = String.fromCharCode(0x2212); break; 
                                    case 'lowast': ch = String.fromCharCode(0x2217); break; 
                                    case 'radic': ch = String.fromCharCode(0x221a); break; 
                                    case 'prop': ch = String.fromCharCode(0x221d); break; 
                                    case 'infin': ch = String.fromCharCode(0x221e); break; 
                                    case 'ang': ch = String.fromCharCode(0x2220); break; 
                                    case 'and': ch = String.fromCharCode(0x2227); break; 
                                    case 'or': ch = String.fromCharCode(0x2228); break; 
                                    case 'cap': ch = String.fromCharCode(0x2229); break; 
                                    case 'cup': ch = String.fromCharCode(0x222a); break; 
                                    case 'int': ch = String.fromCharCode(0x222b); break; 
                                    case 'there4': ch = String.fromCharCode(0x2234); break; 
                                    case 'sim': ch = String.fromCharCode(0x223c); break; 
                                    case 'cong': ch = String.fromCharCode(0x2245); break; 
                                    case 'asymp': ch = String.fromCharCode(0x2248); break; 
                                    case 'ne': ch = String.fromCharCode(0x2260); break; 
                                    case 'equiv': ch = String.fromCharCode(0x2261); break; 
                                    case 'le': ch = String.fromCharCode(0x2264); break; 
                                    case 'ge': ch = String.fromCharCode(0x2265); break; 
                                    case 'sub': ch = String.fromCharCode(0x2282); break; 
                                    case 'sup': ch = String.fromCharCode(0x2283); break; 
                                    case 'nsub': ch = String.fromCharCode(0x2284); break; 
                                    case 'sube': ch = String.fromCharCode(0x2286); break; 
                                    case 'supe': ch = String.fromCharCode(0x2287); break; 
                                    case 'oplus': ch = String.fromCharCode(0x2295); break; 
                                    case 'otimes': ch = String.fromCharCode(0x2297); break; 
                                    case 'perp': ch = String.fromCharCode(0x22a5); break; 
                                    case 'sdot': ch = String.fromCharCode(0x22c5); break; 
                                    case 'lceil': ch = String.fromCharCode(0x2308); break; 
                                    case 'rceil': ch = String.fromCharCode(0x2309); break; 
                                    case 'lfloor': ch = String.fromCharCode(0x230a); break; 
                                    case 'rfloor': ch = String.fromCharCode(0x230b); break; 
                                    case 'lang': ch = String.fromCharCode(0x2329); break; 
                                    case 'rang': ch = String.fromCharCode(0x232a); break; 
                                    case 'loz': ch = String.fromCharCode(0x25ca); break; 
                                    case 'spades': ch = String.fromCharCode(0x2660); break; 
                                    case 'clubs': ch = String.fromCharCode(0x2663); break; 
                                    case 'hearts': ch = String.fromCharCode(0x2665); break; 
                                    case 'diams': ch = String.fromCharCode(0x2666); break; 
                                    default: ch = ''; break; 
                              } 
                        } 
                        i = semicolonIndex; 
                  } 
            } 
            out += ch; 
      } 
	  return out; 
} 

function m4nIsEmpty(str)
{
	if (str == undefined) return true;
	if (str == null) return true;
	if (str.length == 0) return true;
	return false;
}

/****************************************************************************************/

/* server-side request */

function m4nServerSideRequest(url, dataToSend, method)
{
	if (m4nIsEmpty(url)) throw new m4nArgumentNullException("url");
	
	this.url = url;
	this.dataToSend = dataToSend;
	
	this.method = method; //POST or GET
	if (m4nIsEmpty(this.method)) this.method = "POST"

	this.requestSent = false;
	this.responseText = null;
	
	//public methods
	this.getResponseAsText = m4nSSR_GetResponseAsText;
	this.getResponseAsXMLDoc = m4nSSR_GetResponseAsXMLDoc;
	
	//private methods
	this.sendRequest = m4nSSR_SendRequest;
	this.ensureRequestSent = m4nSSR_EnsureRequestSent;
}

//sends the server side request
function m4nSSR_SendRequest()
{
	var xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
	xmlhttp.Open(this.method , m4nExpressionProviderURL, false);
	xmlhttp.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
	xmlhttp.send(this.dataToSend);
	this.responseText = xmlhttp.responseText;
	
	xmlhttp.abort();
}

//returns the response text
function m4nSSR_GetResponseAsText()
{
	this.ensureRequestSent();
	return this.responseText;
}

//returns the response as an xml document object
function m4nSSR_GetResponseAsXMLDoc()
{
	this.ensureRequestSent();
	var xmlDoc = new ActiveXObject("MSXML.DOMDocument");
	xmlDoc.async = false;
	xmlDoc.loadXML(this.responseText);
	return xmlDoc;
}

//ensures the request was sent
function m4nSSR_EnsureRequestSent()
{
	if (!this.requestSent)
	{
		this.sendRequest();
		this.requestSent = true;
	}
}

/****************************************************************************************/

/* the m4nCtrlValueCopy object will copy the value from one HTML input to another  */

//eventNames - comma separated i.e. "onchange,onmouseout"
function m4nCtrlValueCopy(fromId, toId, eventNames)
{
   
	this.ctrlFrom = document.getElementById(fromId);
	if (this.ctrlFrom == null) throw new m4nException("Cannot find control " + fromId + ".");

	this.ctrlTo = document.getElementById(toId);
	if (this.ctrlTo == null) throw new m4nException("Cannot find control " + toId + ".");

	this.eventNames = eventNames;
	
	this.attachCtrlEvents();
}
m4nCtrlValueCopy.prototype.attachCtrlEvents = function()
{
	var instance = this;
	var events = this.eventNames.split(",");
	var lSuccess
	var ev;
	
	for(i in events)
	{
		ev = m4nTrim(events[i]);
		lSuccess = this.ctrlFrom.attachEvent(ev ,function() { m4nCtrlValueCopyEventHandler(instance) });
		m4nAddPurgeElement(this.ctrlFrom);
		
		if (!lSuccess)
		{
			throw new m4nException("Cannot attach event " + ev + " to control " + this.ctrlFrom.id);
		}
	}
}
function m4nCtrlValueCopyEventHandler(instance)
{
	if (instance.ctrlTo.value.length > 0) return;
	try
	{
	    
	    instance.ctrlTo.value = instance.ctrlFrom.value;
	    
	}
	catch(e)
	{
	}
}
/*******************************************************************************************/
/****************************************************************************************/

/* the m4nCtrlValueCopyForHighCost object will copy the value from one HTML input to another  */

//eventNames - comma separated i.e. "onchange,onmouseout"
function m4nCtrlValueCopyForHighCost(fromId, toId, eventNames) {
    
    this.ctrlFrom = document.getElementById(fromId);
    if (this.ctrlFrom == null) throw new m4nException("Cannot find control " + fromId + ".");

    this.ctrlTo = document.getElementById(toId);
    if (this.ctrlTo == null) throw new m4nException("Cannot find control " + toId + ".");

    this.eventNames = eventNames;

    this.attachCtrlEvents();
}
m4nCtrlValueCopyForHighCost.prototype.attachCtrlEvents = function () {
    var instance = this;
    var events = this.eventNames.split(",");
    var lSuccess
    var ev;

    for (i in events) {
        ev = m4nTrim(events[i]);
        lSuccess = this.ctrlFrom.attachEvent(ev, function () { m4nCtrlValueCopyForHighCostEventHandler(instance) });
        m4nAddPurgeElement(this.ctrlFrom);

        if (!lSuccess) {
            throw new m4nException("Cannot attach event " + ev + " to control " + this.ctrlFrom.id);
        }
    }
}
function m4nCtrlValueCopyForHighCostEventHandler(instance) {
   
    if (instance.ctrlTo.value.length > 0) return;
    try {

        
        instance.ctrlTo.value = "9999999";
       
     
    }
    catch (e) {
    }
}


/*******************************************************************************************/


/* the m4nCtrlValueCopyForHighCost object will copy the value from one HTML input to another  */

//eventNames - comma separated i.e. "onchange,onmouseout"
function m4nCtrlValueCopyForLowQty(fromId, toId, eventNames) {

    this.ctrlFrom = document.getElementById(fromId);
    if (this.ctrlFrom == null) throw new m4nException("Cannot find control " + fromId + ".");

    this.ctrlTo = document.getElementById(toId);
    if (this.ctrlTo == null) throw new m4nException("Cannot find control " + toId + ".");

    this.eventNames = eventNames;

    this.attachCtrlEvents();
}
m4nCtrlValueCopyForLowQty.prototype.attachCtrlEvents = function () {
    var instance = this;
    var events = this.eventNames.split(",");
    var lSuccess
    var ev;

    for (i in events) {
        ev = m4nTrim(events[i]);
        lSuccess = this.ctrlFrom.attachEvent(ev, function () { m4nCtrlValueCopyForLowQtyEventHandler(instance) });
        m4nAddPurgeElement(this.ctrlFrom);

        if (!lSuccess) {
            throw new m4nException("Cannot attach event " + ev + " to control " + this.ctrlFrom.id);
        }
    }
}
function m4nCtrlValueCopyForLowQtyEventHandler(instance) {

    if (instance.ctrlTo.value.length > 0) return;
    try {


        instance.ctrlTo.value = "";
       

    }
    catch (e) {
    }
}


/*******************************************************************************************/
/****************************************************************************************/
/* RadWindow functions */

function m4nOpenRadWindow(url, modal, width, height, left, top, title)
{
	return m4nRadWinOpen(url, modal, width, height, left, top, title);
}

function m4nRadWinOpen(url, modal, width, height, left, top, title)
{
	var w = window.radopen(null,null);
	w.SetUrl(url);
	w.SetModal(modal);
	w.SetSize(width,height);
	w.MoveTo(left,top);
	w.SetTitle(title);
	
	return w;
}

function m4nGetRadWindow()
{
	var oWindow = null;
	if (window.radWindow) oWindow = window.radWindow;
	else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
	return oWindow;
} 

function m4nCloseRadWindow()
{
	var w = m4nGetRadWindow();
	if (w != null) w.Close();
}

/****************************************************************************************/

function m4nGetXmlNodeValue(xml, nodeName)
{
	xml = xml.toString();
	if (xml == null) return "";
	if (xml == "") return "";
	
	var xmlDoc = new ActiveXObject("MSXML.DOMDocument");
	xmlDoc.async = false;
	xmlDoc.loadXML(xml);
	
	var fullNodeName = "//" + nodeName;
	var propertyNode = xmlDoc.selectNodes(fullNodeName);

	if (propertyNode.length == 0) {
		alert ("Could not find node " + nodeName + ".");
	}
	if (propertyNode.item(0).text == "undefined")
		return "";

	var result = propertyNode.item(0).text;
	return result;
}

/****************************************************************************************/

//window.attachEvent("onunload", m4nPurgeAllElements);
window.addEventListener("onunload", m4nPurgeAllElements);
var m4nElementPurgeList = new Array();

m4nAddPurgeElement(document);
m4nAddPurgeElement(document.body);

function m4nAddPurgeElement(obj)
{
	m4nElementPurgeList[m4nElementPurgeList.length] = obj;
}

var m4nPurgeAllElementsComplete = false;
function m4nPurgeAllElements()
{
	var count = 0;
	if (!m4nPurgeAllElementsComplete)
	{
		for (i=0; i<=m4nElementPurgeList.length; i++)
		{
			m4nPurgeElement(m4nElementPurgeList[i], false);
			count++;
		}
		m4nPurgeAllElementsComplete = true;
	}
}
	
function m4nPurgeElement(obj, recursive) 
{
    try
    {
		if (obj == null) return;
		var a = obj.attributes, i, l, n;
		if (a)
		{
			l = a.length;
			for (i = 0; i < l; i += 1) {
				n = a[i].name;
				if (typeof obj[n] === 'function') {
					obj[n] = null;
				}
			}
		}
		if (recursive)
		{
			a = obj.childNodes;
			if (a) {
				l = a.length;
				for (i = 0; i < l; i += 1) {
					m4nPurgeElement(obj.childNodes[i]);
				}
			}
		}
    }
    catch(e)
    {
		alert('error purge ' + d.id + ": " + e);
    }
}
function applyRTLGridFixForOldIE()
{

  if (m4nBrowserName=='Explorer' && m4nBrowserVersion<8)
{
   //alert("Looking");
   var elem = document.getElementsByTagName('table');
       for (var i = 0; i < elem.length; i++) {
            //alert(elem[i].className);
            if (elem[i].className.indexOf("MasterTable_DefaultRTL")==0)
            {
               elem[i].className=elem[i].className + ' OLDIE';
               //alert('found');
               //return;
            }
       }
}
}
function preparePageValidatorsForIE9()
{
   if (m4nBrowserName=='Explorer' && m4nBrowserVersion>8)
   {
      if (typeof(Page_Validators) == "undefined")
        return;
        var i, val;
        for (i = 0; i < Page_Validators.length; i++) {
            val = Page_Validators[i];
            if (m4nBrowserName=='Explorer' && m4nBrowserVersion>8) PrepareObjectAttributes(val);
        }
        if (typeof(ValidatorOnLoad) == "function") {
            ValidatorOnLoad();
        }
   }
}
function PrepareObjectAttributes(val) {    
    //if (val.attributes['display']) val.display=val.attributes['display'].value;
    //if (val.attributes['evaluationfunction']) val.evaluationfunction=val.attributes['evaluationfunction'].value;
    //if (val.attributes['controltovalidate']) val.controltovalidate=val.attributes['controltovalidate'].value;
    //if (val.attributes['errormessage']) val.errormessage=val.attributes['errormessage'].value;
    //if (val.attributes['fieldLabel']) val.fieldLabel=val.attributes['fieldLabel'].value;
    //if (val.attributes['alertStartText']) val.alertStartText=val.attributes['alertStartText'].value;
    //if (val.attributes['alertEndText']) val.alertEndText=val.attributes['alertEndText'].value;
    //if (val.attributes['initialvalue']) val.initialvalue=val.attributes['initialvalue'].value;
    //if (val.attributes['alertType']) val.alertType=val.attributes['alertType'].value;
    var i;
        for (i = 0; i < val.attributes.length; i++) {
           try
           {
            if (!eval("val."+val.attributes[i].name)) {
                 eval("val."+val.attributes[i].name+"=val.attributes[i].value");
            }
           } catch(e) {}
        }
}
var m4nBrowserVersion;
var m4nBrowserName;
var BrowserDetect = {
	init: function () {
	    this.browser = this.searchString(this.dataBrowser) || "An unknown browser";
		this.version = this.searchVersion(navigator.userAgent)
			|| this.searchVersion(navigator.appVersion)
			|| "an unknown version";
		this.OS = this.searchString(this.dataOS) || "an unknown OS";
	},
	searchString: function (data) {
		for (var i=0;i<data.length;i++)	{
			var dataString = data[i].string;
			var dataProp = data[i].prop;
			this.versionSearchString = data[i].versionSearch || data[i].identity;
			if (dataString) {
				if (dataString.indexOf(data[i].subString) != -1)
					return data[i].identity;
			}
			else if (dataProp)
				return data[i].identity;
		}
	},
	searchVersion: function (dataString) {
		var index = dataString.indexOf(this.versionSearchString);
		if (index == -1) return;
		return parseFloat(dataString.substring(index+this.versionSearchString.length+1));
	},
	dataBrowser: [
		/*{
			string: navigator.userAgent,
			subString: "Chrome",
			identity: "Chrome"
		},
		{ 	string: navigator.userAgent,
			subString: "OmniWeb",
			versionSearch: "OmniWeb/",
			identity: "OmniWeb"
		},
		{
			string: navigator.vendor,
			subString: "Apple",
			identity: "Safari",
			versionSearch: "Version"
		},
		{
			prop: window.opera,
			identity: "Opera"
		},
		{
			string: navigator.vendor,
			subString: "iCab",
			identity: "iCab"
		},
		{
			string: navigator.vendor,
			subString: "KDE",
			identity: "Konqueror"
		},
		{
			string: navigator.userAgent,
			subString: "Firefox",
			identity: "Firefox"
		},
		{
			string: navigator.vendor,
			subString: "Camino",
			identity: "Camino"
		},
		{		// for newer Netscapes (6+)
			string: navigator.userAgent,
			subString: "Netscape",
			identity: "Netscape"
		},*/
		{
			string: navigator.userAgent,
			subString: "MSIE",
			identity: "Explorer",
			versionSearch: "MSIE"
		}/*,
		{
			string: navigator.userAgent,
			subString: "Gecko",
			identity: "Mozilla",
			versionSearch: "rv"
		},
		{ 		// for older Netscapes (4-)
			string: navigator.userAgent,
			subString: "Mozilla",
			identity: "Netscape",
			versionSearch: "Mozilla"
		}*/
	],
	dataOS : [
		{
			string: navigator.platform,
			subString: "Win",
			identity: "Windows"
		}
	]

}
BrowserDetect.init();
m4nBrowserName = BrowserDetect.browser; //1
m4nBrowserVersion = parseInt(BrowserDetect.version); //2
/*
if (m4nBrowserName=='Explorer' && m4nBrowserVersion<8)
{
   window.attachEvent('onload',applyRTLGridFixForOldIE);
}
if (m4nBrowserName=='Explorer' && m4nBrowserVersion>8)
{
   window.attachEvent('onload',preparePageValidatorsForIE9);
}
*/
window.addEventListener('load', preparePageValidatorsForIE9);
