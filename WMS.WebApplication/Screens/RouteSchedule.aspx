<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="RouteSchedule.aspx.vb" Inherits="WMS.WebApp.RouteSchedule" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>RouteSchedule</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<!-- #include file="~/include/head.html" -->
	</HEAD>
<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout">
		<!-- #include file="~/include/Header.html" -->
		<form id="Form1" method="post" runat="server">
            <telerik:RadScriptManager ID="RadScriptManager2" runat="server">     </telerik:RadScriptManager> 
			<cc3:screen id="Screen1" runat="server" ScreenID="rtsh"></cc3:screen>
			<P><cc2:tableeditor id="TEMasterReceipt" runat="server" DisableSwitches="n" ForbidRequiredFieldsInSearchMode="True"
					SearchDT="DTFIXROUTES" DESIGNTIMEDRAGDROP="14" GridPageSize="5" DefaultMode="Search"
					ManualMode="False" DefaultDT="DTFIXROUTES" EditDT="DTFIXROUTES" InsertDT="DTFIXROUTES"
					AfterInsertMode="Grid" GridDT="DTFIXROUTES" ViewDT="DTFIXROUTES" AutoSelectGridItem="Never" AutoSelectMode="View"></cc2:tableeditor></P>
			<P>&nbsp;</P>
			<P><cc2:datatabcontrol id="DTC" runat="server" ParentID="TEMasterReceipt" SyncEditMode="True">
					<TABLE id="Table1" cellSpacing="0" cellPadding="0" border="0" runat="server">
						<TR>
							<TD>
								<cc2:tabstrip id="TS" runat="server" AutoPostBack="True" TabDefaultStyle="background-color:#DDDDDD;border:solid 1px gray;padding-left:5px;padding-right:5px;font-size:13px;font-weight:bold;padding-top:2px;padding-bottom:2px;"
									TabHoverStyle="background-color:#DDDDDD;" TabSelectedStyle="border:solid 1px gray;border-bottom:none;background:white;"
									SepDefaultStyle="border-bottom:solid 1px gray;" TargetTableID="Tbl">
									<iewc:Tab Text="Stops"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="Schedule"></iewc:Tab>
									<iewc:TabSeparator DefaultStyle="width:100%;"></iewc:TabSeparator>
								</cc2:tabstrip></TD>
						</TR>
						<TR>
							<TD style="BORDER-RIGHT: gray 1px solid; BORDER-LEFT: gray 1px solid; BORDER-BOTTOM: gray 1px solid; HEIGHT: 18px">
								<TABLE id="Tbl" cellSpacing="0" cellPadding="5" border="0" runat="server">
									<TR>
										<TD>
											<cc2:TableEditor id="TEReceiptDetail" runat="server" AutoSelectMode="View" AutoSelectGridItem="Never"
												AfterInsertMode="Grid" InsertDT="DTFIXROUTESSTOPS" EditDT="DTFIXROUTESSTOPS" DefaultDT="DTFIXROUTESSTOPS"
												DefaultMode="Grid" GridPageSize="10" SearchDT="DTFIXROUTESSTOPS" DisableSwitches="rnm" ObjectID="DTFIXROUTESSTOPS"
												ResolveNullOnInsert="False" ></cc2:TableEditor>
											<cc2:DataConnector id="DC1" runat="server" MasterObjID="TEMasterReceipt" ChildFields="ROUTENAME" MasterFields="ROUTENAME"
												TargetID="TEReceiptDetail"></cc2:DataConnector></TD>
										<TD>
											<cc2:TableEditor id="TEReceiptASN" runat="server" AutoSelectMode="View" AutoSelectGridItem="Never"
												DefaultDT="DTFIXROUTESSCHED" DefaultMode="Grid" GridPageSize="10" DisableSwitches="rsndm"></cc2:TableEditor>
											<cc2:DataConnector id="DC3" runat="server" MasterObjID="TEMasterReceipt" ChildFields="ROUTENAME" MasterFields="ROUTENAME"
												TargetID="TEReceiptASN"></cc2:DataConnector></TD>
									</TR>
								</TABLE>
							</TD>
						</TR>
					</TABLE>
				</cc2:datatabcontrol></P>
		</form>
	</body>
</HTML>
