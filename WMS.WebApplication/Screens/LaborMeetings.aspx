<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="LaborMeetings.aspx.vb" Inherits="WMS.WebApp.LaborMeetings" %>

<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN">
<html>
<head>
    <title>Manage Meetings</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1"/>
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1"/>
    <meta name="vs_defaultClientScript" content="JavaScript"/>
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5"/>
    <!-- #include file="~/include/head.html" -->
    </head>
<body MS_POSITIONING="FlowLayout">
    <form id="Form1" method="post" runat="server">
        <telerik:RadScriptManager ID="pageRadScriptManager" Runat="server">
        </telerik:RadScriptManager>
        <cc3:Screen ID="wmsScreen" title="Manage Meetings" runat="server" ScreenID="up"/>
        <p><cc2:TableEditor ID="TE_MEETINGS" runat="server" ConnectionName="default" DefaultDT="DTManageMeetingsSearch" DisableSwitches="vnr" GridPageSize="5" SearchDT="DTManageMeetingsSearch" DefaultMode="Search" EditDT="DTManageMeetings" GridDT="DTManageMeetings" InsertDT="DTManageMeetings" MultiEditDT="DTManageMeetings" ViewDT="DTManageMeetings"/>

        <p>

        <cc2:DataTabControl ID="DTC" runat="server" ParentID="TE_MEETINGS">
            <table id="Table1" runat="server">
                <tr>
                    <td>
                        <cc2:TabStrip ID="TBC" runat="server" SepDefaultStyle="border-bottom:solid 1px gray;" TabSelectedStyle="border:solid 1px gray;border-bottom:none;background:white;" TargetTableID="Tbl1" TabDefaultStyle="background-color:#DDDDDD;border:solid 1px gray;padding-left:5px;padding-right:5px;font-size:13px;font-weight:bold;padding-top:2px;padding-bottom:2px;" AutoPostBack="True" TabHoverStyle="background-color:#DDDDDD;">
                            <iewc:Tab Text="Users in Meeting"/>
                            <iewc:TabSeparator DefaultStyle="width:100%;"/>
                             <iewc:Tab Text="Users Not in Meeting"/>
                            <iewc:TabSeparator DefaultStyle="width:100%;"/>
                       </cc2:TabStrip>
                    </td>
                </tr>
                <tr parentid="TE_MEETINGS">
                    <td>
                        <table id="Tbl1" runat="server">
                            <tr>
                                <td>
                                    <cc2:TableEditor ID="TEUsersInMeeting" runat="server" DefaultDT="DTManageMeetingsUser " AutoSelectGridItem="Never" AutoSelectMode="View" GridDT="DTManageMeetingsUser " EditDT="DTManageMeetingsUser " InsertDT="DTManageMeetingsUser " SearchDT="DTManageMeetingsUser " ViewDT="DTManageMeetingsUser " DisableSwitches="niev" ConnectionName="Made4NetSchema"/>
                                </td>
                                <td>
                                    <cc2:TableEditor ID="TEUsersNotInMeeting" runat="server" DefaultDT="DTManageMeetingsUser " AutoSelectGridItem="Never" AutoSelectMode="View" GridDT="DTManageMeetingsUser " EditDT="DTManageMeetingsUser " InsertDT="DTManageMeetingsUser " SearchDT="DTManageMeetingsUser " ViewDT="DTManageMeetingsUser " DisableSwitches="niev" ConnectionName="Made4NetSchema"/>

                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </cc2:DataTabControl>
    </form>
</body>
</html>