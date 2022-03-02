<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="AppComponentDockList.aspx.vb" Inherits="WMS.WebApp.AppComponentDockList" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title></title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<META HTTP-EQUIV="Expires" CONTENT="Sat, 1 Jan 2000 12:00:00 GMT">
		<!-- #include file="~/include/head.html" -->
	</HEAD>
	<body MS_POSITIONING="FlowLayout" bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0">
		<!-- #include file="~/include/Header.html" -->
		<form id="Form1" method="post" runat="server">
            <telerik:RadScriptManager ID="RadScriptManager2" runat="server">     </telerik:RadScriptManager> 
			<cc3:Screen id="Screen" runat="server" DESIGNTIMEDRAGDROP="7"></cc3:Screen><BR>
			<cc2:tableeditor id="DockList" runat="server" DESIGNTIMEDRAGDROP="3" DefaultDT="SYS_DOCK_LIST" AfterUpdateMode="Grid"
				AfterInsertMode="Grid" AutoSelectGridItem="Never" SearchButtonPos="UnderSearchForm" GridMode="Normal"
				DefaultMode="Grid" ManualMode="False" GridPageSize="20" ObjectID="DockList" SearchButtonGroup="Search1"
				GridType="Normal" EnableCSVExport="Global" EnableQuickChart="Global" EnableQuickReport="Global"
				EnableSavedSearch="Global" PersistSearchValues="Global" ConnectionName="Made4NetSchema" DisableSwitches="nv"
				ForbidRequiredFieldsInSearchMode="True" AllowDeleteInViewMode="True"></cc2:tableeditor><BR>
			<cc2:datatabcontrol id="DTC" runat="server" ParentID="DockList" SyncEditMode="True">
				<TABLE id="Table1" cellSpacing="0" cellPadding="0" border="0" runat="server">
					<TR>
						<TD>
							<cc2:tabstrip id="TS1" runat="server" AutoPostBack="True" TargetTableID="Tbl1">
								<iewc:Tab Text="Fields"></iewc:Tab>
							</cc2:tabstrip></TD>
					</TR>
					<TR>
						<TD>
							<TABLE id="tbl1" cellSpacing="0" cellPadding="5" border="0" runat="server">
								<TR>
									<TD>
										<P>
											<cc2:tableeditor id="DockListDtl" runat="server" DefaultDT="SYS_DOCK_LIST_DTL" AfterUpdateMode="Grid"
												AfterInsertMode="Grid" AutoSelectGridItem="Never" SearchButtonPos="UnderSearchForm" GridMode="Normal"
												DefaultMode="Grid" ManualMode="False" GridPageSize="20" ObjectID="DockListDtl" ConnectionName="Made4NetSchema"
												DisableSwitches="tvn" ConnectionID="0"></cc2:tableeditor>
											<cc2:DataConnector id="DC" runat="server" MasterObjID="DockList" SyncMode="Reversed" TargetID="DockListDtl"
												MasterGridID="TEMaster" MasterFields="code" ChildFields="dock_code"></cc2:DataConnector></P>
									</TD>
								</TR>
							</TABLE>
						</TD>
					</TR>
				</TABLE>
			</cc2:datatabcontrol>
		</form>
	</body>
</HTML>
