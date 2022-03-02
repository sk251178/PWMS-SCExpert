<%@ Page Language="vb" AutoEventWireup="false" Codebehind="PCKFULL.aspx.vb" Inherits="WMS.MobileWebApp.PCKFULL" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.Mobile.WebCtrls" Assembly="Made4Net.Mobile" %>
<%@ Register TagPrefix="cc3" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="WMS.MobileWebApp.WebCtrls" Assembly="WMS.MobileWebApp" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Picking</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
        <style>
div.border
{
border-color:Black;
border-width:1px;

height:28px;
width:640px;
border-collapse:collapse;


border-right-style:solid;
border-bottom-style:solid   ;
border-left-style:solid;
}

table#DO1_MainTable
{

border-bottom-style:none!important;

}

</style>
		<!-- #include file="~/greenscreen/html/head-legacy.html" -->
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0">
		<form id="Form1" method="post" runat="server">
            <telerik:RadScriptManager ID="RadScriptManager2" runat="server">     </telerik:RadScriptManager> 
			<DIV align="center"><cc1:screen id="Screen1" runat="server" ScreenID="RDTPCKFULL"></cc1:screen></DIV>
			<DIV align="center" width="100%"><cc2:dataobject id="DO1" runat="server" LeftButtonText="Next" 
			RightButtonText="Back" centerbuttontext="ReportProblem" centerleftbuttontext="pickanotherload"
			FocusField="Confirm" DefaultButton="Next"></cc2:dataobject></DIV>
           
          
		</form>
        <script type="text/javascript">
     //alert('1st time');
     var isClicked = false;
     $(document).ready(function () {
         //alert('document is ready');
         if ($("#DO1_ActionBar__ctl1_btn0") != null) {
             //alert('control found');

             $(document).on("click", "#DO1_ActionBar__ctl1_btn0", function () {
                 if (!isClicked) {
                     //alert('button clicked');
                     isClicked = true;
                     return true;
                 }
                 else {
                     return false;
                 }
             });
         }
         else {
             //alert('control not found is ready');
         }

     });
    </script>
	</body>
</HTML>
