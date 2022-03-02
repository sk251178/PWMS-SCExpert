var InternetExplorer = navigator.appName.indexOf("Microsoft") != -1;
		function mapcontrol_DoFSCommand(command, args)                
		{
			if (command=="map_ready")
			{
			    //alert("Point Selected");
			} 
			else {
			    //alert(command + "=" + args);
			}
		}
		
		function ClearEntireMap()
		{
			document.getElementById("mapcontrol").SetVariable("/Map:addPoints","-1");
			document.getElementById("mapcontrol").SetVariable("/Map:addNetItems","-1");
		}
		
		function bodyonload()
		{
			ClearEntireMap();
		}
		
		function doAction()
		{
		    //alert("New Command");
			//alert(document.getElementById("command").value);
			document.getElementById("mapcontrol").SetVariable("/Map:commandName","11");
			switch (document.getElementById("command").value)
			{
				case "showterritory":
					showTerritory();
					break;
				case "showroute":
					addroute();
					break;
				case "showrouteset":
					addrouteset();
					break;
				case "gotopoint":
					locatePointById();
					break;
				case "clearmap":
					ClearEntireMap();
					break;
				case "pointitem":
					SendSelectedItem();
					break;
			}
		}
		
		function locatePointById()
		{
//alert(document.getElementById("args").value)

			document.getElementById("mapcontrol").SetVariable("/Map:locatePosId","" + document.getElementById("args").value);
			document.getElementById("mapcontrol").SetVariable("/Map:centerMap","" + Math.random());
		}
		function addroute()
		{
		//alert(document.getElementById("args").value)
		//alert(document.getElementById("args").value.split("@")[1])
		
		     if(document.getElementById("args").value.split("@")[1]!=null)
             {
			            document.getElementById("mapcontrol").SetVariable("/Map:locatePosId","" + document.getElementById("args").value.split("@")[1]);
			            document.getElementById("mapcontrol").SetVariable("/Map:centerMap","" + Math.random());
             }

			document.getElementById("mapcontrol").SetVariable("/Map:addRoutes", document.getElementById("args").value); 
		}
		function addrouteset()
		{
			//alert("In the addrouteset");
			document.getElementById("mapcontrol").SetVariable("/Map:addRouteSet", document.getElementById("args").value); 
		}
		function addrouteset()
		{
			//alert("In the addrouteset");
			document.getElementById("mapcontrol").SetVariable("/Map:addRouteSet", document.getElementById("args").value); 
		}
		
		function showTerritory()
        {
            var territoryid=document.getElementById("args").value.split("@")[0];
            document.getElementById("mapcontrol").SetVariable("/Map:territoryId",territoryid.replace('_',''));

        }

		
		function addDriverRoute()
		{
			document.getElementById("mapcontrol").SetVariable("/Map:addDriverRoutes", document.getElementById("args").value); 
		}
		function SendSelectedItem()
		{
			document.getElementById("mapcontrol").SetVariable("/Map:selectedMapItem","" + document.getElementById("args").value);
		}
		function clearMap()
		{
			document.getElementById("mapcontrol").SetVariable("/Map:addPoints","" + -1);
		}
		
		function ClearNetworkFromMap()
		{
			document.getElementById("mapcontrol").SetVariable("/Map:addNetItems","" + -1);
		}
		
		function geoCodeObject(objectType,command,objectID,objectID2,objectID3)
		{
		   //alert(objectID);   
		   //alert(objectID2);    
		   //alert(objectID3);
		   var objId = objectID;
		   if(objectID2 != "null")
		   {
			   objId += "@" + objectID2;
		   }
		   if(objectID3 != "null")
		   {
			   objId +=  "@" + objectID3;
		   }
		   document.getElementById("args").value=objectType + "_" + objId;
           document.getElementById("command").value=command;
           //alert(document.getElementById("args").value);
           //alert(document.getElementById("command").value);
           document.getElementById("btnAct").click();   
		}
		
		if (navigator.appName && navigator.appName.indexOf("Microsoft") != -1 && navigator.userAgent.indexOf("Windows") != -1 && navigator.userAgent.indexOf("Windows 3.1") == -1) 
		{
			document.write('<SCRIPT LANGUAGE=VBScript\> \n');
			document.write('on error resume next \n');
			document.write('Sub mapcontrol_FSCommand(ByVal command, ByVal args)\n');
			document.write(' call mapcontrol_DoFSCommand(command, args)\n');
			document.write('end sub\n');
			document.write('</SCRIPT\> \n');
		} 
		
