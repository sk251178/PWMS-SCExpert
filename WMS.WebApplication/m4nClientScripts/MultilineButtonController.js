
function MultilineButtonController(name,buttons) {
    this._counter=0;
    this._name = name;
    this._buttons = buttons.split(",");
}

MultilineButtonController.prototype._name;
MultilineButtonController.prototype._buttons;
MultilineButtonController.prototype._counter;
MultilineButtonController.prototype.getName = function() {
    return this._name;
}
MultilineButtonController.prototype.onCheckBoxAction = function(ctrl) {
    var needActivate=false;
    var needDisable=false;
    if (ctrl.checked) {
                  if (this._counter==0) {
		                        needActivate=true;
                  }
	              this._counter=this._counter+1;
    } else {
                  if (this._counter==1) {
		                        needDisable=true;
                  }
	              this._counter=this._counter-1;
    }
    if (needActivate) {
       needActivate=false;
       this.updateButtons(true);
    } else if (needDisable) {
       needDisable=false;
       this.updateButtons(false);
    }

}
MultilineButtonController.prototype.onAllCheckBoxAction = function(ctrl,numOfCheckBoxes) {
    if (ctrl.checked) {
                  this._counter=numOfCheckBoxes;
                  this.updateButtons(true);
    } else {
                 this._counter=0;
                 this.updateButtons(false);
    }
    
}
MultilineButtonController.prototype.updateButtons=function(doEnable) {
           for (var i=0;i<this._buttons.length;i++)
           {

               this.updateButton(this._buttons[i],doEnable) ;  

           }
}
MultilineButtonController.prototype.updateButton=function(btnName,doEnable) {
           var btnarray;
           var btn;
           var url;
           btnarray=document.getElementsByName(btnName);
           if (btnarray.length>0)
           {
            btn=btnarray[0];
            btn.disabled=!doEnable;
            if (doEnable)
            {
            btn.style.cursor="hand";
            } else {
            btn.style.cursor="default";
            }
            if (btn.src) {
            btn.src=this.switchUrl(btn.src,doEnable);
            } else {
            btn.parentElement.disabled=!doEnable; //Enable-Disable parent span for non image buttons
            }
            }
}
MultilineButtonController.prototype.switchUrl=function(url,doEnable) {
              var urlparts;
              var urlpart;
              var cfrom;
              var cto;
              var pos;
              urlparts=url.split(".");
              if (doEnable) {
                 cfrom="_d";
                 cto="";
              } else {
                 cfrom="_a";
                 cto="_d";
              }
                     urlpart=urlparts[urlparts.length-2];
                     
                     pos=urlpart.lastIndexOf(cfrom);
                     if (pos==-1) 
	   {
		urlpart=urlpart + cto;
                     } else if (pos<urlpart.length-2)
                     { 
                                    urlpart=urlpart + cto;
                     } else {
		urlpart=urlpart.substring(0,pos) + cto;
                     }
	   urlparts[urlparts.length-2]=urlpart;
                     url=urlparts.join(".");
                     return url; 
          
}
