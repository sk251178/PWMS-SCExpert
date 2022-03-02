<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="MapView.aspx.vb" Inherits="WMS.WebApp.MapView" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>

<%--    <a href="javascript:geoCodeObject('Route','showroute','R000008733','Company_6806841','null')" target="_self">R000008733</a>
    <a href="javascript:geoCodeObject('Route','showroute','R000008732','Company_6806883','null')" target="_self">R000008732</a>
--%>    
    
        <cc2:map width=500 Height =500 Visible=true id="MPRoutePos" runat="server"></cc2:map>
    
    </div>
    </form>
</body>
</html>
