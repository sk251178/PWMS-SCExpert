function ExtendMenuWithKeyboard(){if ((typeof(RadMenu)=="undefine\x64") || (typeof(RadMenu.KeyDown)!="\x75ndefined")){return; }RadMenu.prototype.I30= function (){var o31=0; for (var i=0; i<this.l1j.length; i++){if ((this.l1j[i]!=null) && (o31<i)){o31=i; }}return o31; };RadMenu.prototype.O31= function (l31){if (l31!=0){return this.GetGroup(this.l1j[l31]); }else {return this.RootGroup; }};RadMenu.prototype.i31= function (I31){if (this.O1c.lt(I31)){if (this.O1c.lt(I31.I1k)){return I31.I1k; }else if (this.O1c.lt(I31.l1i) && this.O1c.lt(I31.l1i[0])){return I31.l1i[0]; }}return null; };RadMenu.prototype.o32= function (I31){if (this.O1c.lt(I31)){if (this.O1c.lt(I31.I1k)){return true; }else if (this.O1c.lt(this.RootGroup.I1k)){return true; }}return false; };RadMenu.prototype.O32= function (l32){var i32=l32.ChildGroup; if (i32 && i32.ID){ this.l1j[l32.I1c+1]=i32.ID; i32.Show(l32.Container); this.I32(i32.l1i[0]); return true; }else {return false; }};RadMenu.prototype.o33= function (){var O33="i"; for (var i=0; i<this.i29.length; i++){O33+=this.i29[i]; }return O33; };RadMenu.prototype.l33= function (){if (this.O1c.lt(this.o2a)){ this.l2a="i"; if (this.O1c.lt(this.i1f)){ this.l2a+=this.i1f; }if (this.O1c.lt(this.l1f)){ this.l2a+=this.l1f; } this.l2a+=this.o2a; }};RadMenu.prototype.i33= function (){return this.l29.O1i(this.o33()); };RadMenu.prototype.KeyUp= function (processedEvent){if (!processedEvent){var processedEvent=window.event; }var I33=this.O1c.lv(processedEvent); var lq=this.i33(); if (lq){lq.RemoveState(MODE_CLICKED); }if (I33==I2v){var o34=this.I30(); if (o34>0){o34-=1; }var O34=this.O31(o34); var l34=this.i31(O34); l34.RemoveState(MODE_CLICKED); l34.Render(MODE_HILIGHT); } this.i29.pop(); } ; RadMenu.prototype.KeyDown= function (processedEvent){if (!processedEvent){var processedEvent=window.event; }var i34=this.O1c.lu(processedEvent); if (i34.type=="text" || i34.type=="te\x78tare\x61"){return; }var I33=this.O1c.lv(processedEvent); var I34= false; var o34=this.I30(); var O34=this.O31(o34); var l34=this.i31(O34); if (this.l2a==""){ this.l33(); }for (var i=0; i<this.i29.length; i++){if (this.i29[i]==I33){I34= true; switch (I33){case I2w:case o2x:case i2w:case l2w:case i2x:case I2v:case I2x:break; default:return; }}}if (!I34){ this.i29.push(I33); }if (this.l2a==this.o33()){if (this.O1k== false){ this.O1k= true; this.o35(processedEvent); }else { this.O1k= false; this.O35(processedEvent); }return false; }var lq=this.i33(); if (lq){lq.AddState(MODE_CLICKED); lq.Action(MODE_CLICKED); return false; }if (!this.o32(O34)){return; }switch (I33){case I2w:case o2x:case i2w:case l2w:case I2x: this.O1c.iw(processedEvent); break; }if (this.O1k== true){var o34=this.I30(); var O34=this.O31(o34); var l34=this.i31(O34); if (I33==i2x){ this.CloseAll((o34-1)); if ((o34-1)==0){ this.O1k= false; }return false; }if (I33==I2x){l34.i1j(processedEvent); l34.O1j(processedEvent); }if (I33==I2v){if (l34.Enabled!= true){return; }if (!this.O32(l34)){l34.ApplyClick(processedEvent); l34.RemoveClick(processedEvent); }return false; } this.I32(this.l35(l34,O34,o34,I33)); return false; }return true; } ; RadMenu.prototype.o35= function (processedEvent){if (this.ClickToOpen== false){ this.ClickToOpen= true; this.FirstClick= false; }if (this.RootGroup && this.RootGroup.l1i && this.RootGroup.l1i.length>0){ this.I32(this.RootGroup.l1i[0]); }};RadMenu.prototype.O35= function (processedEvent){if (this.ClickToOpen== true){ this.ClickToOpen= false; this.FirstClick= true; } this.CloseAll(0); window.status=""; };RadMenu.prototype.I32= function (I2f){if (I2f){var ParentGroup=null; var o1l=0; ParentGroup=I2f.ParentGroup; o1l=I2f.I1c; if ((o1l)>0 && (ParentGroup!=null)){if (this.l1j[o1l]!=ParentGroup.ID){ this.l1j[o1l]=ParentGroup.ID; }if (ParentGroup.Visible!= true){ParentGroup.Show(ParentGroup.I1b.Container); }} this.i35(I2f); }};RadMenu.prototype.i35= function (I2f){ this.I1j(this.o1k); this.CloseAll(I2f.I1c); if (I2f==(I2f.ParentGroup.I1k)){return; }if (I2f.ParentGroup){if (I2f.ParentGroup.I1k!=null){I2f.ParentGroup.I1k.RemoveHilight(); }I2f.ParentGroup.I1k=I2f; }if (!this.O1c.lt(I2f.i1g)){I2f.ApplyHilight(); }} ; RadMenu.prototype.NextItem= function (I35){if (I35.NextItem){if (I35.NextItem.l1h){return this.NextItem(I35.NextItem); }return I35.NextItem; }else {return this.o36(I35.ParentGroup); }};RadMenu.prototype.PreviousItem= function (I35){if (I35.PreviousItem){if (I35.PreviousItem.l1h){return this.PreviousItem(I35.PreviousItem); }return I35.PreviousItem; }else {return this.O36(I35.ParentGroup); }};RadMenu.prototype.o36= function (l36){if (l36 && l36.l1i){if (l36.l1i[0].l1h){return this.NextItem(l36.l1i[0]); }return l36.l1i[0]; }return null; };RadMenu.prototype.O36= function (l36){if (l36 && l36.l1i){if (l36.l1i[(l36.l1i.length-1)].l1h){return this.PreviousItem(l36.l1i[(l36.l1i.length-1)]); }return l36.l1i[(l36.l1i.length-1)]; }return null; };RadMenu.prototype.l35= function (l2o,O2o,i36,I36){if (!this.O1c.lt(l2o) || !this.O1c.lt(O2o) || !this.O1c.lt(i36)){return null; }var o37=O2o.i1q; switch (I36){case I2w:if (o37==VERTICAL_DIRECTION){return this.PreviousItem(l2o); }else if (l2o.ChildGroup){return this.o36(l2o.ChildGroup); }break; case o2x:if (o37==VERTICAL_DIRECTION){return this.NextItem(l2o); }else if (l2o.ChildGroup){return this.o36(l2o.ChildGroup); }break; case i2w:if (o37==VERTICAL_DIRECTION){if (l2o.ChildGroup){return this.o36(l2o.ChildGroup); }else {var O37= true; if ((i36-1)<0){return null; }O2o=this.O31(i36-1); if (O2o.i1q==VERTICAL_DIRECTION){for (var i=this.l1j.length; i>=0; i--){if (this.l1j[i]){O2o=this.GetGroup(this.l1j[i]); if (O2o.i1q==HORIZONTAL_DIRECTION){O37= false; break; }}}if (O37){O2o=this.RootGroup; }}return this.NextItem(O2o.I1k); }}else {return this.NextItem(l2o); }break; case l2w:if (o37==VERTICAL_DIRECTION){if ((i36-1)<0){return null; }O2o=this.O31(i36-1); if (O2o.i1q==VERTICAL_DIRECTION){return O2o.I1k; }else {return this.PreviousItem(O2o.I1k); }}else {return this.PreviousItem(l2o); }break; default:return; }};}