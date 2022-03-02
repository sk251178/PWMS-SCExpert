<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="LangManagement.aspx.vb" Inherits="WMS.WebApp.LangManagement" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Language Management</title>
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
			<cc3:Screen id="Screen1" runat="server"></cc3:Screen>
			<P><cc2:tableeditor id="TEMaster" runat="server" DefaultMode="Grid" ConnectionID="2" DefaultDT="lang"
					ManualMode="False" GridPageSize="20" DESIGNTIMEDRAGDROP="14" ObjectID="lang" ConnectionName="Made4NetSchema"></cc2:tableeditor><BR>
			</P>
			<P><cc2:datatabcontrol id="DTC" runat="server" SyncEditMode="True" ParentID="TEMaster">
					<TABLE id="Table1" cellSpacing="0" cellPadding="0" border="0" runat="server">
						<TR>
							<TD>
								<cc2:tabstrip id="TS" runat="server" TargetTableID="Tbl" AutoPostBack="True">
									<iewc:Tab Text="Vocabulary"></iewc:Tab>
								</cc2:tabstrip></TD>
						</TR>
						<TR>
							<TD>
								<TABLE id="Tbl" cellSpacing="0" cellPadding="5" border="0" runat="server">
									<TR>
										<TD>
											<cc2:TableEditor id="TEDetail1" runat="server" ConnectionName="Made4NetSchema" ObjectID="vocab" DESIGNTIMEDRAGDROP="60"
												DefaultDT="Vocabulary" ConnectionID="2" DefaultMode="Search" DisableSwitches="t" ConnectMasterGridID="TEOrders"
												ConnectChildFields="OrderID" ConnectMasterFields="OrderID" LinkChildFields="OrderID" LinkMasterFields="OrderID"
												LinkParentID="TEOrders" ForbidRequiredFieldsInSearchMode="True" OnBeforeModeChange="TEDetail1_BeforeModeChange" ></cc2:TableEditor>
											<cc2:DataConnector id="DC1" runat="server" DESIGNTIMEDRAGDROP="38" MasterObjID="TEMaster" TargetID="TEDetail1"
												MasterGridID="TEMaster" MasterFields="language_id" ChildFields="language_id"></cc2:DataConnector></TD>
									</TR>
								</TABLE>
							</TD>
						</TR>
					</TABLE>
				</cc2:datatabcontrol></P>
		</FORM>
	</body>
</HTML>
