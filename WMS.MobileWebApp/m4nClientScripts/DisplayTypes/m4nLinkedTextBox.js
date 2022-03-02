/*
Made4Net Version: 1.1

The client side helper of the LinkedTextBox display type
*/


function LinkedTextBox(name,target,screens,pks) {
this.screensarray=screens.split(";");
this.pksarray=pks.split(";");  
this.name=name;  
this.target=target;
this.divObj=document.getElementById("linkedtextboxdiv");
this.preparedlink="";      
this.textboxxmlhttp=null;  
} 
LinkedTextBox.prototype.linkedTextBoxOpenLinksWindow=function(fieldspan)
{
 
   if (this.screensarray.length==1)
   {

       this.linkedTextBoxPrepareRedirect(0,fieldspan.pkvalues);
   }
   else
   {
   var spanpos=findObjectPosition(fieldspan);
   this.divObj.style.display="block";
   this.divObj.style.top=spanpos[0]+10;//event.clientY;
   this.divObj.style.left=spanpos[1]+10;//event.clientX;
   this.divObj.innerHTML=this.createDivContent(fieldspan);
  }
}
LinkedTextBox.prototype.linkedTextBoxPrepareRedirect=function(linkid,pkvalues)
{
    var screenInfo=this.screensarray[linkid];
    var screenInfoArr=screenInfo.split(",");
    
    currentPreparedLinkedTextBox=this;
     this.preparedlink="../" + screenInfoArr[2] + "?sc=" + screenInfoArr[0];
     this.linkedTextBoxRequestPresetValues(screenInfoArr[0],screenInfoArr[1],this.pksarray[linkid],pkvalues.split(";")[linkid]);
   
}
LinkedTextBox.prototype.linkedTextBoxRedirect=function()
{
   if (this.target=="self" || this.target=="_self" )
   {
   	window.location.href=this.preparedlink;
   }
    else
   {
	window.open(this.preparedlink,"_blank");
   }
}
LinkedTextBox.prototype.createDivContent=function(fieldObj)
{
 var content;
    content="<table border=0 width=100% class=\"LinkedTextBox_MainTable\">";
	content+="<tr class=\"LinkedTextBox_Header\">";
		content+="<td class=\"LinkedTextBox_HeaderCell\"></td><td class=\"LinkedTextBox_HeaderCell\" align=right><span class=\"LinkedTextBox_Link\" onclick=\""+this.name+".linkedTextBoxCloseLinksWindow()\">Close</span></td>";
	content+="</tr>";
                  for (var i=0;i<this.screensarray.length;i++)
                  {
                       var screenInfo=this.screensarray[i].split(",");
                       content+="<tr class=\"LinkedTextBox_Row\">";
		content+="<td class=\"LinkedTextBox_RowCell\" align=left><span class=\"LinkedTextBox_Link\" onclick=\""+this.name+".linkedTextBoxPrepareRedirect("+i+",'"+fieldObj.pkvalues+"')\">" + screenInfo[3] + "</span></td><td class=\"LinkedTextBox_RowCell\"></td>";
	    content+="</tr>";
                  }
    content+="</table>";
    return content;
}
LinkedTextBox.prototype.getRequestHTTPObject=function()
{
  return textboxxmlhttp;
}
LinkedTextBox.prototype.linkedTextBoxRequestPresetValues=function(ScreenID,TEName,PK,PKValues)
{
    //create post request
    var amp = "&";
    var sid = Math.random();
    var DataToSend="ScreenID=" + ScreenID+amp+"TEName=" + TEName + amp + "PK=" + PK + amp + "PKValues=" + PKValues + amp + "sid=" + sid;
        //alert(DataToSend);
                      
	    this.textboxxmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
	    this.textboxxmlhttp.Open("POST", "../m4nScreens/TableEditorPresetManager.aspx", true);
	    this.textboxxmlhttp.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
	    this.textboxxmlhttp.onreadystatechange=linkedTextBoxRequestStateChanged;
       	    this.textboxxmlhttp.send(DataToSend);
}
LinkedTextBox.prototype.linkedTextBoxRequestPresetValuesStateChanged=function()
{
if (this.textboxxmlhttp.readyState==4)
  {
  if (this.textboxxmlhttp.responseText=="done")
  {
      this.linkedTextBoxRedirect();
  }
  else
  {
     alert("Failed To Preset Values for screen.");
  }
  }
}
LinkedTextBox.prototype.linkedTextBoxCloseLinksWindow=function()
{
   this.divObj.style.display="none";
   this.divObj.innerHTML="";
}
document.write("<div id=\"linkedtextboxdiv\" class=\"LinkedTextBox_Container\" name=\"linkedtextboxdiv\" ></div>");
var currentPreparedLinkedTextBox;
//var textboxxmlhttp;
function linkedTextBoxRequestStateChanged()
{
  currentPreparedLinkedTextBox.linkedTextBoxRequestPresetValuesStateChanged(); 
}
function findObjectPosition(obj) {
	var curleft = curtop = 0;
	if (obj.offsetParent) {
	do {
			curleft += obj.offsetLeft;
			curtop += obj.offsetTop;
		} while (obj = obj.offsetParent);
                  } else {
                               curleft += obj.offsetLeft;
			curtop += obj.offsetTop;
                  }
	return [curtop,curleft];
}
