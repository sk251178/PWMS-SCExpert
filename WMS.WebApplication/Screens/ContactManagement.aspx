<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ContactManagement.aspx.vb" Inherits="WMS.WebApp.ContactManagement" %>
<%@ Register TagPrefix="cc4" Namespace="Made4Net.WebControls.Charting" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Shipments</title>
		<meta content="False" name="vs_showGrid">
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<!-- #include file="~/include/head.html" -->
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout">
		<!-- #include file="~/include/Header.html" -->
		<form id="Form1" method="post" runat="server">
            <telerik:RadScriptManager ID="RadScriptManager2" runat="server">     </telerik:RadScriptManager> 
			<cc3:screen id="Screen1" title="Contacts Management" runat="server" ScreenID="cnm"></cc3:screen>
			<P><cc2:tableeditor id="TEMasterContact" runat="server" SearchDT="DTContactSearch" InsertDT="DTContactInsert"
					DisableSwitches="mnd" ForbidRequiredFieldsInSearchMode="True" DESIGNTIMEDRAGDROP="14" GridPageSize="5"
					DefaultMode="Search" ManualMode="False" DefaultDT="DTContactView" EditDT="DTContactView"
					AfterInsertMode="Grid" AfterUpdateMode="Grid" AutoSelectGridItem="Never" AutoSelectMode="View"></cc2:tableeditor></P>
			<P>&nbsp;</P>
			<P><cc2:datatabcontrol id="DTC" runat="server" ParentID="TEMasterContact" SyncEditMode="True">
					<TABLE id="Table1" cellSpacing="0" cellPadding="0" border="0" runat="server">
						<TR>
							<TD>
								<cc2:tabstrip id="TS" runat="server" TargetTableID="Tbl" SepDefaultStyle="border-bottom:solid 1px gray;"
									TabSelectedStyle="border:solid 1px gray;border-bottom:none;background:white;" TabHoverStyle="background-color:#DDDDDD;"
									TabDefaultStyle="background-color:#DDDDDD;border:solid 1px gray;padding-left:5px;padding-right:5px;font-size:13px;font-weight:bold;padding-top:2px;padding-bottom:2px;"
									AutoPostBack="True">
									<iewc:Tab Text="Companies"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="Assign Company"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
								<%--	<iewc:Tab Text="Contact Routes"></iewc:Tab>--%>
									<iewc:TabSeparator DefaultStyle="width:100%;"></iewc:TabSeparator>
								</cc2:tabstrip></TD>
						</TR>
						<TR>
							<TD style="BORDER-RIGHT: gray 1px solid; BORDER-LEFT: gray 1px solid; BORDER-BOTTOM: gray 1px solid; HEIGHT: 18px">
								<TABLE id="Tbl" cellSpacing="0" cellPadding="5" border="0" runat="server">
									<TR>
										<TD>
											<cc2:TableEditor id="TECompanies" runat="server" AutoSelectMode="View" AutoSelectGridItem="Never"
												DefaultDT="DTContactCompany" DefaultMode="Grid" GridPageSize="10"
												DisableSwitches="trnie" SearchDT="DTContactCompanySearch"></cc2:TableEditor>
											<cc2:DataConnector id="DC1" runat="server" DESIGNTIMEDRAGDROP="43" TargetID="TECompanies" MasterFields="contactid" ChildFields="contactid"
												MasterObjID="TEMasterContact"></cc2:DataConnector></TD>
										<TD>
											<cc2:TableEditor id="TEAssignCompanies" runat="server" AutoSelectMode="View" AutoSelectGridItem="Never" ForbidRequiredFieldsInSearchMode="True"
												DefaultDT="DTCompanyAssign" DefaultMode="Grid" SearchDT="DTCompanyAssignSearch" GridPageSize="10" DisableSwitches="eitdrn"
												></cc2:TableEditor></TD>
										<%--<TD>
											<cc2:TableEditor id="TEContactRoutes" runat="server" AutoSelectMode="View" AutoSelectGridItem="Never" ForbidRequiredFieldsInSearchMode="True"
												DefaultDT="DTContactRoutesView" DefaultMode="Grid" SearchDT="DTContactRoutes Search" GridPageSize="10" DisableSwitches="mtrnv" EditDT="DTContactRoutes Edit" InsertDT="DTContactRoutes Add"
												></cc2:TableEditor>
												<cc2:DataConnector id="DC2" runat="server" DESIGNTIMEDRAGDROP="43" TargetID="TEContactRoutes" MasterFields="contactid" ChildFields="contactid"
												MasterObjID="TEMasterContact"></cc2:DataConnector></TD>--%>
									</TR>
								</TABLE>
							</TD>
						</TR>
					</TABLE>
				</cc2:datatabcontrol></P>
		</form>
	</body>
</HTML>
