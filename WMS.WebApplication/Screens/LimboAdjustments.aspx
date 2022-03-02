<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="LimboAdjustments.aspx.vb" Inherits="WMS.WebApp.LimboAdjustments" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>LimboAdjustments</title>
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
			<cc3:screen id="Screen1" runat="server" ScreenID="lj"></cc3:screen><br>
			<asp:panel id="pnlAdj" Runat="server">
				<TABLE border="0">
					<TR>
						<TD>
							<cc2:fieldLabel id="lblAdjReasonCode" runat="server" text="Adjustment Reason"></cc2:fieldLabel></TD>
						<TD>
							<cc2:dropdownlist id="ddReasonCode" runat="server" TableName="CODELISTDETAIL" TextField="DESCRIPTION"
								ValueField="CODE" Where="CODELISTCODE = 'INVADJRC'" AutoPostBack="True"></cc2:dropdownlist></TD>
					</TR>
				</TABLE>
			</asp:panel>
			<cc2:tableeditor id="TELimboSku" runat="server" DESIGNTIMEDRAGDROP="14" DisableSwitches="eidvn" GridPageSize="5"
				DefaultMode="Search" DefaultDT="DTLimboSku" ManualMode="False" AutoSelectGridItem="Never" AutoSelectMode="View"
				ForbidRequiredFieldsInSearchMode="True"></cc2:tableeditor>
			<P><cc2:datatabcontrol id="DTC" runat="server" ParentID="TELimboSku" SyncEditMode="True">
					<TABLE id="Table1" cellSpacing="0" cellPadding="0" border="0" runat="server">
						<TR>
							<TD>
								<cc2:tabstrip id="TS" runat="server" AutoPostBack="True" TargetTableID="Tbl" SepDefaultStyle="border-bottom:solid 1px gray;"
									TabSelectedStyle="border:solid 1px gray;border-bottom:none;background:white;" TabHoverStyle="background-color:#DDDDDD;"
									TabDefaultStyle="background-color:#DDDDDD;border:solid 1px gray;padding-left:5px;padding-right:5px;font-size:13px;font-weight:bold;padding-top:2px;padding-bottom:2px;">
									<iewc:Tab Text="Inventory Loads"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
								</cc2:tabstrip></TD>
						</TR>
						<TR>
							<TD style="BORDER-RIGHT: gray 1px solid; BORDER-LEFT: gray 1px solid; BORDER-BOTTOM: gray 1px solid; HEIGHT: 18px">
								<TABLE id="Tbl" cellSpacing="0" cellPadding="5" border="0" runat="server">
									<TR>
										<TD>
											<cc2:TableEditor id="TELimboSkuLoad" runat="server" AutoSelectMode="View" AutoSelectGridItem="Never"
												DefaultDT="DTLimboLoads" DefaultMode="Grid" GridPageSize="10" DisableSwitches="eidntsv" FilterExpression="status = 'LIMBO'"
												SearchDT="DTLoadSearchSkuByLoad"></cc2:TableEditor>
											<cc2:DataConnector id="DC1" runat="server" DESIGNTIMEDRAGDROP="43" TargetID="TELimboSkuLoad" MasterFields="consignee,sku"
												ChildFields="consignee,sku" MasterObjID="TELimboSku"></cc2:DataConnector></TD>
									</TR>
								</TABLE>
							</TD>
						</TR>
					</TABLE>
				</cc2:datatabcontrol></P>
		</form>
	</body>
</HTML>
