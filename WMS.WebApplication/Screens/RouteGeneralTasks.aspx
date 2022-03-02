<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RouteGeneralTasks.aspx.vb" Inherits="WMS.WebApp.RouteGeneralTasks" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Route General Tasks</title>
</head>
<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout">
		<FORM id="Form1" method="post" runat="server">
			<cc3:screen id="Screen1" runat="server" ScreenID="rgt"></cc3:screen>
			<P><cc2:tableeditor id="TERoutesTasks" runat="server" GridDT="DTGenTasksGrid" DefaultDT="DTGenTasksGrid" SearchDT="DTGenTasksSearch"
					DefaultMode="Search" GridPageSize="5" ForbidRequiredFieldsInSearchMode="True" DisableSwitches="vnr"
					InsertDT="DTGenTasksAdd" EditDT="DTGenTasksEdit"></cc2:tableeditor></P>
    </form>
</body>
</html>
