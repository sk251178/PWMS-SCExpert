<%@ Page Language="vb" AutoEventWireup="false" Codebehind="TransShipment.aspx.vb" Inherits="WMS.WebApp.Forwarding"%>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<html xmlns="http://www.w3.org/1999/xhtml">
	<HEAD>
		<title>TransShipment</title>
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
			<cc3:screen id="Screen1" runat="server" ScreenID="tst"></cc3:screen>
			<P><cc2:tableeditor id="TEMasterTransShipment" runat="server" DisableSwitches="n" ForbidRequiredFieldsInSearchMode="True"
					SearchDT="DTTRANSSHIPMENTSearch" DESIGNTIMEDRAGDROP="14" GridPageSize="5" DefaultMode="Search"
					ManualMode="False" DefaultDT="DTTRANSSHIPMENTGrid" EditDT="DTTRANSSHIPMENTEdit" InsertDT="DTTRANSSHIPMENTInsert"
					AfterInsertMode="Grid" GridDT="DTTRANSSHIPMENTGrid" ViewDT="DTTRANSSHIPMENTGrid" AutoSelectGridItem="Never"
					AutoSelectMode="View"></cc2:tableeditor></P> 
			<P>&nbsp;</P>
						<cc2:datatabcontrol id="DTC" runat="server" SyncEditMode="True" ParentID="TEMasterTransShipment">
					<TABLE id="Table1" cellSpacing="0" cellPadding="0" border="0" runat="server">
						<TR>
							<TD>
								<cc2:tabstrip id="TS" runat="server" TargetTableID="Tbl" SepDefaultStyle="border-bottom:solid 1px gray;"
									TabSelectedStyle="border:solid 1px gray;border-bottom:none;background:white;" TabHoverStyle="background-color:#DDDDDD;"
									TabDefaultStyle="background-color:#DDDDDD;border:solid 1px gray;padding-left:5px;padding-right:5px;font-size:13px;font-weight:bold;padding-top:2px;padding-bottom:2px;"
									AutoPostBack="True">
									<iewc:Tab Text="Transshipment Details"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
								</cc2:tabstrip></TD>

						</TR>
						<TR>
							<TD style="BORDER-RIGHT: gray 1px solid; BORDER-LEFT: gray 1px solid; BORDER-BOTTOM: gray 1px solid; HEIGHT: 18px">
								<TABLE id="Tbl" cellSpacing="0" cellPadding="5" border="0" runat="server">
									<TR>
										<TD>
											<cc2:TableEditor id="TETransshipmentDetails" runat="server"
                                                DisableSwitches="mnrv" EditDT="DTTransshipmentDetailsEdit" InsertDT="DTTransshipmentDetailsInsert" ForbidRequiredFieldsInSearchMode="True" DefaultMode="Grid" DefaultDT="DTTransshipmentDetailsGrid"
												AllowDeleteInViewMode="False" AutoSelectGridItem="Never" AutoSelectMode="View"
												ObjectID="TETransshipmentDetails"></cc2:TableEditor> 
											<cc2:DataConnector id="DC1" runat="server" TargetID="TETransshipmentDetails" MasterFields="TRANSSHIPMENT,CONSIGNEE"
												ChildFields="TRANSSHIPMENT,CONSIGNEE" MasterObjID="TEMasterTransShipment"></cc2:DataConnector></TD>
									</TR>
								</TABLE>
							</TD>
							

							
							
						</TR>
					</TABLE>
				</cc2:datatabcontrol>
		</form>
	</body>
</html>
