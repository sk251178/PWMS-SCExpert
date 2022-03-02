<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="VehiclePosition.aspx.vb" Inherits="WMS.WebApp.VehiclePosition" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>VehiclePosition</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<!-- #include file="~/include/head.html" -->
	</HEAD>
	<body style="OVERFLOW-X:hidden" bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0"
		MS_POSITIONING="FlowLayout">
		<!-- #include file="~/include/Header.html" -->
		<form id="Form1" method="post" runat="server">
            <telerik:RadScriptManager ID="RadScriptManager2" runat="server">     </telerik:RadScriptManager> 
			<cc3:screen id="Screen1" runat="server" ScreenID="vpm"></cc3:screen>
			<TABLE cellSpacing="0" cellPadding="0" border="1">
				<TR>
					<TD vAlign="top" bordercolor="#000000">
						<cc2:map id="MPVehiclePos" runat="server"></cc2:map>
					</TD>
					<TD vAlign="top" borderColor="#ffffff"><cc2:datatabcontrol id="DTC" runat="server" SyncEditMode="False">
							<TABLE id="Table1" cellSpacing="0" cellPadding="0" border="0" runat="server">
								<TR>
									<TD>
										<cc2:tabstrip id="TS" runat="server" AutoPostBack="True" TargetTableID="Tbl" SepDefaultStyle="border-bottom:solid 1px gray;"
											TabSelectedStyle="border:solid 1px gray;border-bottom:none;background:white;" TabHoverStyle="background-color:#DDDDDD;"
											TabDefaultStyle="background-color:#DDDDDD;border:solid 1px gray;padding-left:5px;padding-right:5px;font-size:13px;font-weight:bold;padding-top:2px;padding-bottom:2px;">
											<iewc:Tab Text="Vehicle Last Position"></iewc:Tab>
											<iewc:TabSeparator></iewc:TabSeparator>
											<iewc:Tab Text="Vehicle Routes"></iewc:Tab>
										</cc2:tabstrip></TD>
								</TR>
								<TR>
									<TD style="BORDER-RIGHT: gray 1px solid; BORDER-LEFT: gray 1px solid; BORDER-BOTTOM: gray 1px solid; HEIGHT: 18px">
										<TABLE id="Tbl" cellSpacing="0" cellPadding="5" border="0" runat="server">
											<TR>
												<TD>
													<cc2:tableeditor id="TEVehiclePosition" runat="server" SearchDT="DTVehiclePosition" GridDT="DTVehiclePosition"
														ForbidRequiredFieldsInSearchMode="True" DefaultDT="DTVehiclePosition" DefaultMode="Grid" DESIGNTIMEDRAGDROP="60"
														DisableSwitches="rmneiv"></cc2:tableeditor></TD>
												<TD>
													<cc2:TableEditor id="TEVehicleRoutes" runat="server" SearchDT="DTVehicleRoutesSearch" GridDT="DTVehicleRoutes"
														DefaultDT="DTVehicleRoutes" DefaultMode="Search" DisableSwitches="eidrnv" PersistSearchValues="Global"
														SearchButtonText="ShowRoute" ObjectID="3" GridPageSize="5"></cc2:TableEditor></TD>
											</TR>
										</TABLE>
									</TD>
								</TR>
							</TABLE>
						</cc2:datatabcontrol>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
