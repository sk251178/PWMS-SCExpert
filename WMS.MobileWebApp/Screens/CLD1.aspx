<%@ Register TagPrefix="cc1" Namespace="WMS.MobileWebApp.WebCtrls" Assembly="WMS.MobileWebApp" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.Mobile.WebCtrls" Assembly="Made4Net.Mobile" %>
<%@ Register TagPrefix="cc3" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="CLD1.aspx.vb" Inherits="WMS.MobileWebApp.CLD1" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>CLD1</title>
		<meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<!-- #include file="~/greenscreen/html/head-legacy.html" -->
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0">
		<form id="Form1" method="post" runat="server">
            <telerik:RadScriptManager ID="RadScriptManager2" runat="server">     </telerik:RadScriptManager> 
			<div align="center"><cc1:screen id="Screen1" runat="server" title="CREATE LOAD" ScreenID="RDTCLD1"></cc1:screen></div>
			<DIV align="center" width="100%"><cc2:dataobject id="DO1" runat="server" LeftButtonText="Create" RightButtonText="CloseReceipt" title="Create Load"
					CenterButtonText="changeline"  DefaultButton="Create"></cc2:dataobject></DIV>
		</form>

<script src="../greenscreen/resources/jquery-1.12.4.min.js"></script>
<!-- <script src="../greenscreen/resources/respond.js"></script> --> <!-- Must come after jQuery -->
<script src="../greenscreen/javascript/wms.js?v=1.8"></script>
<script type = "text/javascript">   
   //alert('1st time');   
   var isClicked = false;   
   $(document).ready(function () {   
   //alert('document is ready');   
   if ($("#DO1_ActionBar__ctl1_btn0") != null) 
   {   
   	//alert('control found');   
      
   	$(document).on("click", "#DO1_ActionBar__ctl1_btn0", function () {   
   		if (!isClicked) 
		{   
   			//alert('button clicked');   
   			isClicked = true;   
   			return true;   
   		}   
		else 
		{   
   			return false;   
   		}   
   	});   
   }   
   else 
   {   
   	//alert('control not found is ready');   
   }   
      
   });   
   </script>   


	</body>
</HTML>
