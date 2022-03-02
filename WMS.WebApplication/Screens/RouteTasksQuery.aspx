<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RouteTasksQuery.aspx.vb" Inherits="WMS.WebApp.RouteTasksQuery" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Route Tasks</title>
</head>
    <body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<cc3:screen id="Screen1" runat="server" ScreenID="rtq"></cc3:screen>
			<P><cc2:TableEditor id="TERouteStopTasks" runat="server" AutoSelectMode="View" DisableSwitches="trveidmn"
				ForbidRequiredFieldsInSearchMode="True" GridPageSize="10" DefaultMode="Grid" DefaultDT="DTRouteStopTasksGridQuery" SearchDT="DTRouteStopTasksGridQuerySearch"
				AutoSelectGridItem="Never"></cc2:TableEditor></P>
			<P>&nbsp;</P>
    </form>
</body>
</html>
