<%@ Page Language="vb" AutoEventWireup="false" Codebehind="VehicleType.aspx.vb" Inherits="WMS.WebApp.VehicleType" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Vehicle Type</title>
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
			<cc3:screen id="Screen1" runat="server" ScreenID="vt"></cc3:screen><br>
			<cc2:tableeditor id="TEVehicleType" runat="server" DefaultDT="DTVehicleType" AfterUpdateMode="Grid"
				DisableSwitches="rmn" DESIGNTIMEDRAGDROP="60" DefaultMode="Grid" SearchDT="DTVehicleTypeSearch"
				ForbidRequiredFieldsInSearchMode="True"></cc2:tableeditor><BR>
			&nbsp;<BR>
			<P><cc2:datatabcontrol id="DTC" runat="server" SyncEditMode="True" ParentID="TEVehicleType">
					<TABLE id="Table1" cellSpacing="0" cellPadding="0" border="0" runat="server">
						<TR>
							<TD>
								<cc2:tabstrip id="TS" runat="server" AutoPostBack="True" TabDefaultStyle="background-color:#DDDDDD;border:solid 1px gray;padding-left:5px;padding-right:5px;font-size:13px;font-weight:bold;padding-top:2px;padding-bottom:2px;"
									TabHoverStyle="background-color:#DDDDDD;" TabSelectedStyle="border:solid 1px gray;border-bottom:none;background:white;"
									SepDefaultStyle="border-bottom:solid 1px gray;" TargetTableID="Tbl">
									<iewc:Tab Text="TransportationClass"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="Vehicle Location"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="Vehicle Schema"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
								</cc2:tabstrip></TD>
						</TR>
						<TR>
							<TD style="BORDER-RIGHT: gray 1px solid; BORDER-LEFT: gray 1px solid; BORDER-BOTTOM: gray 1px solid; HEIGHT: 18px">
								<TABLE id="Tbl" cellSpacing="0" cellPadding="5" border="0" runat="server">
									<TR>
										<TD>
											<cc2:TableEditor id="TEVehicleTypeClass" runat="server" DefaultDT="DTVehicleTypeClass" DisableSwitches="rmn"
												InsertDT="DTVehicleTypeClassInsert" DefaultMode="Grid" GridPageSize="10"></cc2:TableEditor>
											<cc2:DataConnector id="DC1" runat="server" DESIGNTIMEDRAGDROP="43" MasterObjID="TEVehicleType" ChildFields="VehicleTypeName"
												MasterFields="VehicleTypeName" TargetID="TEVehicleTypeClass"></cc2:DataConnector></TD>
										<TD>
											<cc2:TableEditor id="TEVehicleLocations" runat="server" DefaultDT="DTVehicleTypeLocation" DisableSwitches="rmn"
												DefaultMode="Grid" GridPageSize="10"></cc2:TableEditor>
											<cc2:DataConnector id="DC2" runat="server" DESIGNTIMEDRAGDROP="43" MasterObjID="TEVehicleType" ChildFields="VehicleType"
												MasterFields="VehicleTypeName" TargetID="TEVehicleLocations"></cc2:DataConnector></TD>
										<TD>
											<TABLE>
												<TR vAlign="bottom">
													<TD>
														<DIV id="LeftVehicle" style="visible: inline" runat="server"></DIV>
													</TD>
													<TD>
														<DIV id="RightVehicle" style="visible: inline" runat="server"></DIV>
													</TD>
												</TR>
											</TABLE>
										</TD>
									</TR>
								</TABLE>
							</TD>
						</TR>
					</TABLE>
				</cc2:datatabcontrol></P>
		</form>
	</body>
</HTML>
