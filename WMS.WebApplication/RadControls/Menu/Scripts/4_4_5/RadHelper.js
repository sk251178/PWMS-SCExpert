if (typeof(RadMenuHelperUtils)=="undefined"){var RadMenuHelperUtils= {On:function (In,item){In[In.length]=item; } ,oo:function (In,index,item){In[index]=item; } ,Oo:function (In,item){var Io= false; for (var i=0; i<In.length; i++){if (item==In[i]){Io= true; }if (Io){In[i]=In[i+1]; }}if (Io){In.length-=1; }} ,op:function (In,index){for (var i=index; 0<=i && i<In.length; i++){In[i]=In[i+1]; }if (0<=index && index<In.length){In.length-=1; }} ,Op:function (In){In.length=0; } ,lp:function (In,item){for (var i=0; i<In.length; i++){if (In[i]==item){return i; }}return -1; } ,ip:function (In,Ip){var oq=""; if (typeof(Ip)=="\x75\x6e\x64efine\x64" || Ip==null){Ip="\x2c"; }if (In.length<=0){return oq; }for (var i=0; i<In.length; i++){oq=oq+((oq=="")?"":Ip)+In[i].toString(); }return oq; } ,Oq:function (In){var i,j; for (i=In.length-1; i>=0; i--){for (j=0; j<=i; j++){if (In[j+1]<In[j]){var lq=In[j]; In[j]=In[j+1]; In[j+1]=lq; }}}return iq; } ,Iq:function (In,or){switch (typeof(or)){case "\x6eumb\x65\x72":return or; case "\x73\x74ring":var Or=parseInt(or); if (!isNaN(Or) && ("\x43"+Or=="C"+or)){return Or; }break; default:break; }} ,lr:function (In){var ir; for (var i in In){var Ir=RadMenuHelperUtils.Iq(In,i); if ((typeof(Ir)!="\165\x6edefined") && ((typeof(ir)=="\x75nd\x65\x66ined") || (Ir>ir))){ir=Ir; }}return ir; } ,os:function (){if (this.os.arguments.length>0){var Os=this.os.arguments[0]; for (var i=1; i<this.os.arguments.length; i++){Os=Os.replace(new RegExp("\134\x7b"+i+"\x5c}","\x69g"),this.os.arguments[i]); }}return Os; } ,ls:function (is,Is){if (typeof(Is)!="\x73tring"){return false; }return (0==is.indexOf(Is)); } ,ot:function (is,Is){if (typeof(Is)!="\x73tring"){return false; }return (is.lastIndexOf(Is)+Is.length==is.length-1); } ,Ot:function (is){return (is=="")? true : false; } ,lt:function (it){if ((typeof(it)!="unde\x66ined") && (it!=null)){return true; }return false; } ,AttachEventListener:function (eventElement,It,eventHandler){ou=RadMenuHelperUtils.Ou(It); if (eventElement.attachEvent){eventElement.attachEvent(ou,eventHandler); }else if (typeof(eventElement.addEventListener)!="un\x64\x65fined"){eventElement.addEventListener(ou,eventHandler, true); }else {var eventHandler="\x65ventElemen\x74\x2eon"+ou+"=eventHan\x64\x6cer"; eval(eventHandler); }} ,Ou:function (It){It=It.toLowerCase(); if (document.attachEvent && !RadMenuHelperUtils.ls(It,"\x6fn")){return "on"+It; }else if (document.addEventListener && RadMenuHelperUtils.ls(It,"o\x6e")){return It.substr(2); }else {return It; }} ,lu:function (processedEvent){if (null==processedEvent){return null; }if (processedEvent.srcElement){return processedEvent.srcElement; }else if (processedEvent.target){if (processedEvent.target.nodeType==3){return processedEvent.target.parentNode; }else {return processedEvent.target; }}else {return null; }} ,iu:function (processedEvent){if (null==processedEvent){return null; }if (processedEvent.toElement){return processedEvent.toElement; }else if (processedEvent.Iu){return processedEvent.Iu; }else {return null; }} ,ov:function (processedEvent){if (!processedEvent){return false; }if (processedEvent.button){return (processedEvent.button==2); }else if (processedEvent.which){return (processedEvent.which==3); }} ,Ov:function (processedEvent){if (processedEvent.button && RadBrowserUtils.IsIE){return (processedEvent.button==1); }if (processedEvent.button && RadBrowserUtils.t){return (processedEvent.button==0); }else if (processedEvent.which){return (processedEvent.which==1); }} ,lv:function (processedEvent){if (processedEvent.which && processedEvent.type.indexOf("\x6b\x65\x79")!=-1){return processedEvent.which; }else if (processedEvent.keyCode){return processedEvent.keyCode; }else {return null; }} ,iv:function (processedEvent){return String.fromCharCode(RadMenuHelperUtils.lv(processedEvent)); } ,Iv:function (processedEvent){if (processedEvent.pageX){return processedEvent.pageX; }else if (processedEvent.clientX){if (RadBrowserUtils.c){return (processedEvent.clientX+document.documentElement.scrollLeft); }return (processedEvent.clientX+document.body.scrollLeft); }} ,ow:function (processedEvent){if (processedEvent.pageY){return processedEvent.pageY; }else if (processedEvent.clientY){if (RadBrowserUtils.c){return (processedEvent.clientY+document.documentElement.scrollTop); }return (processedEvent.clientY+document.body.scrollTop); }} ,Ow:function (processedEvent){if (processedEvent && processedEvent.stopPropagation){processedEvent.stopPropagation(); }else if (window.event){window.event.cancelBubble= true; }} ,lw:function (processedEvent){if (processedEvent && processedEvent.preventDefault){processedEvent.preventDefault(); return false; }else if (window.event){window.event.returnValue= false; }} ,iw:function (processedEvent){if (RadBrowserUtils.W){return false; }else if (RadBrowserUtils.IsIE){processedEvent.returnValue= false; }else if (RadBrowserUtils.t){processedEvent.stopPropagation(); }return false; } ,Iw:function (l6){if (!RadMenuHelperUtils.lt(l6)){return null; }if (l6.style){return l6.style; }else {return l6; }} ,ox:function (l6,Ox){var lx=document.createElement("\x49\x46\x52AME"); lx.src="javas\x63\x72ipt:f\x61\x6cse;"; if (RadMenuHelperUtils.lt(Ox)){switch (Ox){case 0:lx.src="\x6aavas\x63\x72ipt:v\x6f\x69d(0\x29;"; break; case 1:lx.src="\x61bout:b\x6c\x61nk"; break; case 2:lx.src="blank\x2e\x68tm"; break; }}lx.frameBorder=0; lx.style.position="abs\x6f\x6cute"; lx.style.visibility="\x68idden"; lx.style.left="-500px"; lx.style.top="\x2d\x32000px"; lx.style.height=RadMenuHelperUtils.ix(l6)+"px"; var Ix=0; Ix=RadMenuHelperUtils.oy(l6); if (RadBrowserUtils.c && RadBrowserUtils.o1){}lx.style.width=Ix+"px"; lx.style.filter="p\x72\x6fgid:DX\x49\x6dage\x54\x72an\x73\x66orm\x2e\115\x69\x63r\x6fsoft.\x41\154p\x68\141(\x73tyle=\x30\054\x6fpaci\x74y=0)"; lx.Oy= false; return l6.parentNode.insertBefore(lx,l6); } ,ly:function (l6){if (RadMenuHelperUtils.lt(l6.offsetParent)){return l6.offsetParent; }else if (RadMenuHelperUtils.lt(l6.parentNode)){return l6.parentNode; }else if (RadMenuHelperUtils.lt(l6.parentElement)){return l6.parentElement; }} ,iy:function (Iy,oz){var Oz=Iy; while ((Oz && Oz.id && Oz.id!=oz) && Oz.nodeName!="\102\x4fDY"){Oz=Oz.parentNode; }if (Oz && Oz.id && Oz.id==oz){return Oz; }else {return null; }} ,lz:function (Iy,oz,iz){if (!Iy || !oz){return false; }var Oz=Iy; if (iz){while (((typeof(Oz)!="u\x6edefine\x64") && (typeof(Oz.id)!="u\x6e\x64efined") && (Oz.id.indexOf(oz)<0)) && Oz.nodeName!="\x42\x4fDY" && (Oz.parentNode!=null)){Oz=Oz.parentNode; }}else {while (((typeof(Oz)!="un\x64\x65fined") && (typeof(Oz.id)!="\x75\x6edefined") && (Oz.id!=oz)) && Oz.nodeName!="BODY" && (Oz.parentNode!=null)){Oz=Oz.parentNode; }}if ((typeof(Oz)!="\x75ndefine\x64") && (typeof(Oz.id)!="\x75ndefined") && iz== false && (Oz.id==oz)){return true; }if ((typeof(Oz)!="\x75\x6edefine\x64") && (typeof(Oz.id)!="undefined") && iz== true && (Oz.id.indexOf(oz)<0)){return true; }else {return false; }} ,Iz:function (Iy,o10){if (!Iy || !o10){return false; }var Oz=Iy; while (((typeof(Oz)!="\x75ndefine\x64") && (Oz!=o10)) && Oz.nodeName!="\x42ODY" && (Oz.parentNode!=null)){Oz=Oz.parentNode; }if (Oz==o10){return true; }return false; } ,Show:function (){for (var i=0; i<arguments.length; i++){RadMenuHelperUtils.O10(arguments[i], true); }} ,l10:function (){for (var i=0; i<arguments.length; i++){RadMenuHelperUtils.O10(arguments[i], false); }} ,O10:function (container,i10){var I10=RadMenuHelperUtils.Iw(container); if (I10==null){return; }if (i10== true){I10.visibility="visible"; }else if (I10!=null && i10== false){I10.visibility="\x68idden"; }return I10.visibility; } ,o11:function (){for (var i=0; i<arguments.length; i++){RadMenuHelperUtils.O11(arguments, true); }} ,l11:function (){for (var i=0; i<arguments.length; i++){RadMenuHelperUtils.O11(arguments, false); }} ,O11:function (container,i10){var I10=RadMenuHelperUtils.Iw(container); if (i10!=null && i10== true){I10.display=""; }else if (i10!=null && i10== false){I10.display="\x6eon\x65"; }return I10.display; } ,i11:function (I11,o12,O12){RadMenuHelperUtils.l12(I11,o12); RadMenuHelperUtils.i12(I11,O12); } ,l12:function (I11,o12){if (I11.style && I11.style.left){I11.style.left=o12+"\x70x"; }else if (I11.style && I11.style.pixelLeft){I11.style.pixelLeft=o12; }} ,i12:function (I11,O12){if (I11.style && I11.style.top){I11.style.top=O12+"px"; }else if (I11.style && I11.style.pixelTop){I11.style.pixelTop=O12; }} ,ScrollLeft:function (){if (self.pageYOffset){return self.pageXOffset; }else if (document.documentElement && document.documentElement.scrollTop){return document.documentElement.scrollLeft; }else if (document.body){return document.body.scrollLeft; }else if (document.layers){return window.I12; }} ,o13:function (){if (self.pageYOffset){return self.pageYOffset; }else if (document.documentElement && document.documentElement.scrollTop){return document.documentElement.scrollTop; }else if (document.body){return document.body.scrollTop; }else if (document.layers){return window.O13; }} ,l13:function (l6){if (RadBrowserUtils.I && RadBrowserUtils.IsIE && l6.offsetParent){RadMenuHelperUtils.i13(l6); }var offsetLeft=0; if (l6.offsetParent){while (l6.offsetParent){offsetLeft+=l6.offsetLeft; l6=l6.offsetParent; }}else if (RadBrowserUtils.Z && RadBrowserUtils.IsIE && typeof document.body.leftMargin!="u\x6edefine\x64"){offsetLeft+=document.body.leftMargin; }else if (l6.x){offsetLeft+=l6.x; }return offsetLeft; } ,i13:function (l6){var offsetLeft=0; var offsetTop=0; var I13= false; while (l6.offsetParent){if (l6.style.position=="\x72elati\x76\x65"){I13= true; break; }offsetLeft+=l6.offsetLeft; offsetTop+=l6.offsetTop; l6=l6.offsetParent; }if (I13){l6.style.width=l6.offsetWidth; l6.style.height=l6.offsetHeight; }} ,o14:function (l6){if (RadBrowserUtils.I && RadBrowserUtils.IsIE && l6.offsetParent){RadMenuHelperUtils.i13(l6); }var offsetTop=0; if (l6.offsetParent){while (l6.offsetParent){offsetTop+=l6.offsetTop; l6=l6.offsetParent; }}else if (RadBrowserUtils.Z && RadBrowserUtils.IsIE && typeof document.body.leftMargin!="\x75ndefi\x6e\x65d"){offsetTop+=document.body.topMargin; }else if (l6.y){offsetTop+=l6.y; }if (RadBrowserUtils.c && RadBrowserUtils.H){offsetTop+=document.body.topMargin; }return offsetTop; } ,O14:function (){if (self.innerWidth){return self.innerWidth; }else if (document.documentElement && document.documentElement.clientHeight){return document.documentElement.clientWidth; }else if (document.body){return document.body.clientWidth; }return 0; } ,l14:function (){if (self.innerHeight){return self.innerHeight; }else if (document.documentElement && document.documentElement.clientHeight){return document.documentElement.clientHeight; }else if (document.body){return document.body.clientHeight; }return 0; } ,oy:function (l6){if (!l6){return 0; }if (RadMenuHelperUtils.lt(l6.style)){if (RadBrowserUtils.c && (RadBrowserUtils.n || RadBrowserUtils.M)){if (RadMenuHelperUtils.lt(l6.offsetWidth) && l6.offsetWidth!=0){return l6.offsetWidth; }}if (RadMenuHelperUtils.lt(l6.style.pixelWidth) && l6.style.pixelWidth!=0){var i14=l6.style.pixelWidth; if (RadMenuHelperUtils.lt(l6.offsetWidth) && l6.offsetWidth!=0){i14=(i14<l6.offsetWidth)?l6.offsetWidth:i14; }return i14; }}if (RadMenuHelperUtils.lt(l6.offsetWidth)){return l6.offsetWidth; }return 0; } ,ix:function (l6){if (!l6){return 0; }if (RadMenuHelperUtils.lt(l6.style)){if (RadMenuHelperUtils.lt(l6.style.pixelHeight) && l6.style.pixelHeight!=0){return l6.style.pixelHeight; }}if (l6.offsetHeight){return l6.offsetHeight; }return 0; } ,I14:function (o15,O15,Ix,l15){var i15,I15; var o16,O16; var l16=0; var i16= false ,I16= false; i15=RadMenuHelperUtils.ScrollLeft()+RadMenuHelperUtils.O14(); I15=RadMenuHelperUtils.o13()+RadMenuHelperUtils.l14(); o16=o15+Ix; O16=O15+l15; i16=((i15-o16)>=0)?0: (i15-o16); I16=((I15-O16)>=0)?0: (I15-O16); return {o17:i16,O17:I16 } ; } ,o3:function (){}};RadMenuHelperUtils.o3(); }
