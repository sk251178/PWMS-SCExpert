<%@ Page Language="vb" AutoEventWireup="false" Codebehind="UseClockInAssignTask.aspx.vb" Inherits="WMS.WebApp.UseClockInAssignTask" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Shift Instances</title>
		<meta content="False" name="vs_showGrid">
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<!-- #include file="~/include/head.html" -->
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout">
		<!-- #include file="~/include/Header.html" -->
		<FORM id="Form1" method="post" runat="server">
            <telerik:RadScriptManager ID="RadScriptManager2" runat="server">     </telerik:RadScriptManager> 
			<cc3:screen id="Screen1" runat="server" ScreenID="sfti"></cc3:screen>
					<TABLE id="Table1" cellSpacing="0" cellPadding="0" border="0" runat="server" >
						<TR align="left">
						    <TD>
						        <table id="tblClockIn">
						            <tr>
						                <td>
						                    <cc2:tableeditor id="TEUseClockInAssignTask" runat="server" ViewDT="DTUseClockInAssignTask" DefaultDT="DTUseClockInAssignTask" 
			        		                    DefaultMode="View" GridPageSize="5" ForbidRequiredFieldsInSearchMode="True" DisableSwitches="vnrei"></cc2:tableeditor>
						                </td>
						            </tr>
						        </table>
            			            
							</TD>
						</TR>
						<!--<tr align="left">
						    <td style="width: 201px">
						        <p></p>
						        <p>
                                    &nbsp;</p>
                                <p>
                                    &nbsp;</p>
						    </td>
						</tr>-->
						<tr align="left">
						    <td>
						        <h2><span>Task Assignment</span></h2>
						    </td>
						</tr>
						<TR align="left">
						    <TD align="left">
						        <table id="tblTask" title="Task Assignment">
						            <tr>
						                <td>
						                    <cc2:tableeditor id="TEUserAssignTask" runat="server" ViewDT="DTUserAssignTask" DefaultDT="DTUserAssignTask" 
			        		                    DefaultMode="Grid" GridPageSize="5" ForbidRequiredFieldsInSearchMode="True" DisableSwitches="vnrei"
					                            ObjectID="TEUserAssignTask"></cc2:tableeditor>
						                </td>
						            </tr>
						        </table>
							</TD>
						</TR>
					</TABLE>
		</FORM>
	</body>
</HTML>
