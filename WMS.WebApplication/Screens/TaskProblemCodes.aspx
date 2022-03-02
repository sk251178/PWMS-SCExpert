<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TaskProblemCodes.aspx.vb" Inherits="WMS.WebApp.TaskProblemCodes" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Task Problem Codes</title>
	<!-- #include file="~/include/head.html" -->
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout">
		<!-- #include file="~/include/Header.html" -->
		<form id="Form1" method="post" runat="server">
            <telerik:RadScriptManager ID="RadScriptManager2" runat="server">     </telerik:RadScriptManager> 
			<cc3:screen id="Screen1" runat="server" screenid="tpc"></cc3:screen>
			<P><cc2:tableeditor id="TETaskProblemCodes" runat="server" GridDT="DTTASKPROBLEMCODEGrid" AfterUpdateMode="Grid"
					AfterInsertMode="Grid" SearchDT="DTTASKPROBLEMCODESearch" InsertDT="DTTASKPROBLEMCODEInsert" DisableSwitches="nmr" ForbidRequiredFieldsInSearchMode="True"
					GridPageSize="5" DefaultMode="Search" DefaultDT="DTTASKPROBLEMCODEGrid" EditDT="DTTASKPROBLEMCODEEdit"
					ViewDT="DTTASKPROBLEMCODEGrid" ObjectID="TETaskProblemCodes"></cc2:tableeditor></P>
			<P>&nbsp;</P>
			<P><cc2:datatabcontrol id="DTC" runat="server" ParentID="TETaskProblemCodes" SyncEditMode="True">
					<TABLE id="Table1" cellSpacing="0" cellPadding="0" border="0" runat="server">
						<TR>
							<TD>
								<cc2:tabstrip id="TS" runat="server" TargetTableID="Tbl" SepDefaultStyle="border-bottom:solid 1px gray;"
									TabSelectedStyle="border:solid 1px gray;border-bottom:none;background:white;" TabHoverStyle="background-color:#DDDDDD;"
									TabDefaultStyle="background-color:#DDDDDD;border:solid 1px gray;padding-left:5px;padding-right:5px;font-size:13px;font-weight:bold;padding-top:2px;padding-bottom:2px;"
									AutoPostBack="True">
									<iewc:Tab Text="Task Types"></iewc:Tab>
									<iewc:TabSeparator DefaultStyle="width:100%;"></iewc:TabSeparator>
								</cc2:tabstrip></TD>
						</TR>
						<TR>
							<TD style="BORDER-RIGHT: gray 1px solid; BORDER-LEFT: gray 1px solid; BORDER-BOTTOM: gray 1px solid; HEIGHT: 18px">
								<TABLE id="Tbl" cellSpacing="0" cellPadding="5" border="0" runat="server">
									<TR>
										<TD> 
											<cc2:TableEditor id="TETaskTypes" runat="server" AutoSelectMode="View" AutoSelectGridItem="Never"
												ObjectID="TETaskTypes" SearchDT="DTTASKPROBLEMCODESASSIGNMENTSearch" DefaultDT="DTTASKPROBLEMCODESASSIGNMENTGrid" InsertDT="DTTASKPROBLEMCODESASSIGNMENTInsert"
												EditDT="DTTASKPROBLEMCODESASSIGNMENTEdit" DefaultMode="Grid" GridPageSize="10"
												ForbidRequiredFieldsInSearchMode="True" DisableSwitches="nmrt"></cc2:TableEditor>
											<cc2:DataConnector id="DC1" runat="server" TargetID="TETaskTypes" MasterFields="PROBLEMCODEID"
												ChildFields="PROBLEMCODEID" MasterObjID="TETaskProblemCodes"></cc2:DataConnector></TD>  
									</TR>
								</TABLE>
							</TD>
						</TR>
					</TABLE>
				</cc2:datatabcontrol></P>
		</form>
	</body>
</html>
