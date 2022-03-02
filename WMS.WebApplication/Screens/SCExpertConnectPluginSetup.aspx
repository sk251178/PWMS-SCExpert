<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SCExpertConnectPluginSetup.aspx.vb" Inherits="WMS.WebApp.SCExpertConnectSetup" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>SCExpert Connect Setup</title>
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
			<cc3:screen id="Screen1" runat="server" ScreenID="st"></cc3:screen>
			<P><cc2:tableeditor id="TEPluginInstance" runat="server" ObjectID="TEPlugin"
					DisableSwitches="mnv" DESIGNTIMEDRAGDROP="14" DefaultMode="Search" ManualMode="False" ConnectionName="Made4NetSchema"
					DefaultDT="DTPluginInstance" SearchDT="DTPluginInstanceSearch" EditDT="DTPluginInstanceEdit" 
					AutoSelectGridItem="Never"></cc2:tableeditor></P>
			<P>&nbsp;</P>
			<P><cc2:datatabcontrol id="DTC" runat="server" ParentID="TEPluginInstance" SyncEditMode="True">
					<TABLE id="Table1" cellSpacing="0" cellPadding="0" border="0" runat="server">
						<TR>
							<TD>
								<cc2:tabstrip id="TS" runat="server" AutoPostBack="True" TabDefaultStyle="background-color:#DDDDDD;border:solid 1px gray;padding-left:5px;padding-right:5px;font-size:13px;font-weight:bold;padding-top:2px;padding-bottom:2px;"
									TabHoverStyle="background-color:#DDDDDD;" TabSelectedStyle="border:solid 1px gray;border-bottom:none;background:white;"
									SepDefaultStyle="border-bottom:solid 1px gray;" TargetTableID="Tbl">
									<iewc:Tab Text="Parameters"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="Transaction Keys"></iewc:Tab>
									<iewc:TabSeparator DefaultStyle="width:100%;"></iewc:TabSeparator>
								</cc2:tabstrip></TD>
						</TR>
						<TR>
							<TD style="BORDER-RIGHT: gray 1px solid; BORDER-LEFT: gray 1px solid; BORDER-BOTTOM: gray 1px solid; HEIGHT: 18px">
								<TABLE id="Tbl" cellSpacing="0" cellPadding="5" border="0" runat="server">
									<TR>
										<TD>
											<cc2:TableEditor id="TEParams" runat="server" DefaultDT="DTPluginParams" EditDT="DTPluginParamsEdit" ConnectionName="Made4NetSchema"
												DefaultMode="Grid" DESIGNTIMEDRAGDROP="60" DisableSwitches="tsmn" AfterInsertMode="Grid" AfterUpdateMode="Grid"></cc2:TableEditor>
											<cc2:DataConnector id="DC1" runat="server" DESIGNTIMEDRAGDROP="38" SyncMode="Reversed" MasterObjID="TEPluginInstance"
												ChildFields="PLUGINID" MasterFields="PLUGINID" TargetID="TEParams"></cc2:DataConnector></TD>
										<TD>
											<cc2:TableEditor id="TEKeys" runat="server" DefaultDT="DTPluginKeys" ConnectionName="Made4NetSchema"
											    DefaultMode="Grid" DisableSwitches="tsmn" AfterInsertMode="Grid" AfterUpdateMode="Grid"></cc2:TableEditor>
											<cc2:DataConnector id="DC2" runat="server" DESIGNTIMEDRAGDROP="43" MasterObjID="TEPluginInstance" ChildFields="PLUGINID"
												MasterFields="PLUGINID" TargetID="TEKeys"></cc2:DataConnector></TD>
									</TR>
								</TABLE>
							</TD>
						</TR>
					</TABLE>
				</cc2:datatabcontrol></P>
		</FORM>
	</body>
</HTML>

