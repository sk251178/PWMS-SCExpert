<%@ Page Language="vb" AutoEventWireup="false" Codebehind="InvLocation.aspx.vb" Inherits="WMS.WebApp.InvLocation"%>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>InvLocation</title>
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
			<cc3:screen id="Screen1" runat="server" ScreenID="il"></cc3:screen>
			<P><cc2:tableeditor id="TEInvLocation" runat="server" ManualMode="False" DefaultDT="DTInvLocation" DefaultMode="Search"
					GridPageSize="5" DisableSwitches="veidmn" DESIGNTIMEDRAGDROP="14" AutoSelectGridItem="Never"
					AutoSelectMode="View"></cc2:tableeditor></P>
			<P>&nbsp;</P>
			<P><cc2:datatabcontrol id="DTC" runat="server" SyncEditMode="True" ParentID="TEInvLocation">
					<TABLE id="Table1" cellSpacing="0" cellPadding="0" border="0" runat="server">
						<TR>
							<TD>
								<cc2:tabstrip id="TS" runat="server" TabDefaultStyle="background-color:#DDDDDD;border:solid 1px gray;padding-left:5px;padding-right:5px;font-size:13px;font-weight:bold;padding-top:2px;padding-bottom:2px;"
									TabHoverStyle="background-color:#DDDDDD;" TabSelectedStyle="border:solid 1px gray;border-bottom:none;background:white;"
									SepDefaultStyle="border-bottom:solid 1px gray;" TargetTableID="Tbl" AutoPostBack="True">
									<iewc:Tab Text="By Loads"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="By SKU"></iewc:Tab>
									<iewc:TabSeparator DefaultStyle="width:100%;"></iewc:TabSeparator>
								</cc2:tabstrip></TD>
						</TR>
						<TR>
							<TD style="BORDER-RIGHT: gray 1px solid; BORDER-LEFT: gray 1px solid; BORDER-BOTTOM: gray 1px solid; HEIGHT: 18px">
								<TABLE id="Tbl" cellSpacing="0" cellPadding="5" border="0" runat="server">
									<TR>
										<TD>
											<cc2:TableEditor id="TEInvLocationLoad" runat="server" DisableSwitches="eidmntv" GridPageSize="10"
												DefaultMode="Grid" DefaultDT="DTinvLocationLoad" SearchDT="DTLoadSearchLocBySKU" GridDT="DTinvLocationLoad"
												AutoSelectGridItem="Never" AutoSelectMode="View"></cc2:TableEditor>
											<cc2:DataConnector id="DC1" runat="server" DESIGNTIMEDRAGDROP="43" MasterObjID="TEInvLocation" ChildFields="location"
												MasterFields="location" TargetID="TEInvLocationLoad"></cc2:DataConnector></TD>
										<TD>
											<cc2:TableEditor id="TEInvLocSku" runat="server" DisableSwitches="eidmntv" GridPageSize="10" DefaultMode="Grid"
												DefaultDT="DTINVCONSBYSKUBYLOCATION" SearchDT="DTInvSkuLocBySku" AutoSelectGridItem="Never"
												AutoSelectMode="View"></cc2:TableEditor>
											<cc2:DataConnector id="DC2" runat="server" DESIGNTIMEDRAGDROP="43" MasterObjID="TEInvLocation" ChildFields="location"
												MasterFields="location" TargetID="TEInvLocSKU"></cc2:DataConnector></TD>
									</TR>
								</TABLE>
							</TD>
						</TR>
					</TABLE>
				</cc2:datatabcontrol></P>
		</form>
	</body>
</HTML>
