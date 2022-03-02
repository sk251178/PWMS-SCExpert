<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ReturnLoads.aspx.vb" Inherits="WMS.WebApp.ReturnLoads" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Return Loads</title>
    <!-- #include file="~/include/head.html" -->
</head>
<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout">
	<!-- #include file="~/include/Header.html" -->
    <form id="form1" runat="server"> 
        <telerik:RadScriptManager ID="RadScriptManager2" runat="server">     </telerik:RadScriptManager> 
   
    <cc3:screen id="Screen1" runat="server" ScreenID="rlo"></cc3:screen>
    			<p><cc2:tableeditor id="TEReturnLoads" ObjectID="TEReturnLoads" runat="server" DefaultDT="DTReturnLoadsGrid" 
    			GridDT="DTReturnLoadsGrid" SearchDT="DTReturnLoadsSearch" MultiEditDT="DTReturnLoadsMultiEdit"
				DisableSwitches="ivtrme" DefaultMode="Search"/>
                    &nbsp;</p>
    
    </form>
</body>
</html>
