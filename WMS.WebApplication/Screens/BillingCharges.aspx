<%@ Page Language="vb" AutoEventWireup="false" Codebehind="BillingCharges.aspx.vb" Inherits="WMS.WebApp.BillingCharges"%>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>BillingCharges</title>
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
			<cc3:screen id="Screen1" runat="server" ScreenID="ch"></cc3:screen>
			<P><cc2:tableeditor id="TEBillingChargesMaster" runat="server" DefaultDT="DTBILLINGCHARGESHEADER" ManualMode="False"
					DefaultVS="2" DefaultMode="Search" GridPageSize="5" DESIGNTIMEDRAGDROP="14" EditVS="0" ForbidRequiredFieldsInSearchMode="True"
					DisableSwitches="mndrei" ConnectionID="0" AutoSelectGridItem="Never" AutoSelectMode="View"></cc2:tableeditor>
			</P><P></P>
			<cc2:datatabcontrol id="DTC" runat="server" ParentID="TEBillingChargesMaster" SyncEditMode="True">
				<TABLE id="Table2" cellSpacing="0" cellPadding="0" border="0" runat="server">
					<TR>
						<TD>
							<cc2:tabstrip id="TS" runat="server" TargetTableID="Table1" SepDefaultStyle="border-bottom:solid 1px gray;"
								TabSelectedStyle="border:solid 1px gray;border-bottom:none;background:white;" TabHoverStyle="background-color:#DDDDDD;"
								TabDefaultStyle="background-color:#DDDDDD;border:solid 1px gray;padding-left:5px;padding-right:5px;font-size:13px;font-weight:bold;padding-top:2px;padding-bottom:2px;"
								AutoPostBack="True">
								<iewc:Tab Text="Charge Details"></iewc:Tab>
							</cc2:tabstrip></TD>
					</TR>
					<TR>
						<TD style="BORDER-RIGHT: gray 1px solid; BORDER-LEFT: gray 1px solid; BORDER-BOTTOM: gray 1px solid; HEIGHT: 18px">
							<TABLE id="Table1" cellSpacing="0" cellPadding="0" border="0" runat="server">
								<TR>
									<TD style="WIDTH: 165px">
										<cc2:TableEditor id="TEBillingChargesDetail" runat="server" AutoSelectMode="View" AutoSelectGridItem="Never"
											ConnectionID="0" DisableSwitches="mndrei" ForbidRequiredFieldsInSearchMode="True" DefaultMode="Grid"
											ManualMode="False" DefaultDT="DTBILLINGCHARGESDETAIL" SearchDT="DTBILLINGCHARGESDETAILSEARCH"></cc2:TableEditor>
										<cc2:DataConnector id="DC1" runat="server" DESIGNTIMEDRAGDROP="38" TargetID="TEBillingChargesDetail"
											MasterGridID="TEBillingChargesMaster" MasterFields="ChargeID" ChildFields="ChargeID" MasterObjID="TEBillingChargesMaster"></cc2:DataConnector></TD>
								</TR>
							</TABLE>
						</TD>
					</TR>
				</TABLE>
			</cc2:datatabcontrol>
			<P></P>
		</form>
	</body>
</HTML>
