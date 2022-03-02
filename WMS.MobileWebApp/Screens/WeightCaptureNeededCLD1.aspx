<%--<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="WeightCaptureNeededCLD1.aspx.vb" Inherits="WMS.MobileWebApp.WeightCaptureNeededCLD1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
</body>
</html>--%>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="WeightCaptureNeededCLD1.aspx.vb" Inherits="WMS.MobileWebApp.WeightCaptureNeededCLD1" %>

<%@ Register TagPrefix="cc2" Namespace="Made4Net.Mobile.WebCtrls" Assembly="Made4Net.Mobile" %>
<%@ Register TagPrefix="cc3" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="WMS.MobileWebApp.WebCtrls" Assembly="WMS.MobileWebApp" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>creating a pallet on the receiving dock</title>
    <meta content="0" name="mobileoptimized">
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <!-- #include file="~/greenscreen/html/head-legacy.html" -->
</head>
<body bottommargin="0" leftmargin="0" topmargin="0" rightmargin="0">
    <form id="Form1" method="post" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager2" runat="server"></telerik:RadScriptManager>
        <div align="center">
            <cc1:Screen ID="Screen1" runat="server" ScreenID="WCLD1"></cc1:Screen>
        </div>
        <div align="center" width="100%">
            <cc2:DataObject ID="DO1" runat="server" LeftButtonText="Next" RightButtonText="Back"
                CenterLeftButtonText="Ready" DefaultButton="Next"></cc2:DataObject>
        </div>
    </form>
    <script type="text/javascript">
     //alert('1st time');
     var isClicked = false;
     $(document).ready(function () {
         //alert('document is ready');
         if ($("#DO1_ActionBar__ctl2_btn1") != null) {
             //alert('control found');

             $(document).on("click", "#DO1_ActionBar__ctl2_btn1", function () {
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
</html>

