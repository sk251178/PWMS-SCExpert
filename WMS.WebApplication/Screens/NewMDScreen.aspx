<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="NewMDScreen.aspx.vb" Inherits="WMS.WebApp.NewMDScreen" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>NewMDScreen</title>
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
			<P>
				<cc2:tableeditor id="TEMaster" runat="server" ManualMode="False" DisableSwitches="i"
					DefaultMode="s" TableName="Orders" GridPageSize="5" DESIGNTIMEDRAGDROP="14"></cc2:tableeditor></P>
			<P>&nbsp;</P>
			<P><cc2:datatabcontrol id="DTC" runat="server" ParentID="TEMaster">
					<TABLE id="Table1" cellSpacing="0" cellPadding="0" border="0" runat="server">
						<TR>
							<TD>
								<cc2:tabstrip id="TS" runat="server" AutoPostBack="True" TabDefaultStyle="background-color:#DDDDDD;border:solid 1px gray;padding-left:5px;padding-right:5px;font-size:13px;font-weight:bold;padding-top:2px;padding-bottom:2px;"
									TabHoverStyle="background-color:#DDDDDD;" TabSelectedStyle="border:solid 1px gray;border-bottom:none;background:white;"
									SepDefaultStyle="border-bottom:solid 1px gray;" TargetTableID="Tbl">
									<iewc:Tab Text="Detail1"></iewc:Tab>
									<iewc:TabSeparator DefaultStyle="border-bottom:soild 1 gray;"></iewc:TabSeparator>
									<iewc:Tab Text="Detail2"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="Detail3"></iewc:Tab>
									<iewc:TabSeparator DefaultStyle="width:100%;"></iewc:TabSeparator>
								</cc2:tabstrip></TD>
						</TR>
						<TR>
							<TD style="BORDER-RIGHT: gray 1px solid; BORDER-LEFT: gray 1px solid; BORDER-BOTTOM: gray 1px solid; HEIGHT: 18px">
								<TABLE id="Tbl" cellSpacing="0" cellPadding="5" border="0" runat="server">
									<TR>
										<TD>
											<cc2:TableEditor id="TEDetail1" runat="server" DESIGNTIMEDRAGDROP="60" TableName="Orders"
												DefaultMode="v" DisableSwitches="tsidr" LinkParentID="TEOrders" LinkMasterFields="OrderID"
												LinkChildFields="OrderID" ConnectMasterFields="OrderID" ConnectChildFields="OrderID" ConnectMasterGridID="TEOrders"
												></cc2:TableEditor>
											<cc2:DataConnector id="DC1" runat="server" DESIGNTIMEDRAGDROP="38" ChildFields="OrderID" MasterFields="OrderID"
												MasterGridID="TEMaster" TargetID="TEDetail1"></cc2:DataConnector></TD>
										<TD>
											<cc2:TableEditor id="TEDetail2" runat="server" TableName="Orders" DefaultMode="v" DisableSwitches="tsidr"
												 LinkParentID="TEOrders" LinkMasterFields="OrderID" LinkChildFields="OrderID" ConnectMasterFields="OrderID"
												ConnectChildFields="OrderID" ConnectMasterGridID="TEOrders"></cc2:TableEditor>
											<cc2:DataConnector id="DC2" runat="server" DESIGNTIMEDRAGDROP="43" ChildFields="OrderID" MasterFields="OrderID"
												MasterGridID="TEMaster" TargetID="TEDetail2"></cc2:DataConnector></TD>
										<TD>
											<cc2:TableEditor id="TEDetail3" runat="server" DESIGNTIMEDRAGDROP="29" TableName="OrderDetails" DefaultMode="g"
												DisableSwitches="ts" LinkParentID="TEOrders" LinkMasterFields="OrderID" LinkChildFields="OrderID"
												ConnectMasterFields="OrderID" ConnectChildFields="OrderID" ConnectMasterGridID="TEOrders"></cc2:TableEditor>
											<cc2:DataConnector id="DC3" runat="server" ChildFields="OrderID" MasterFields="OrderID" MasterGridID="TEMaster"
												TargetID="TEDetail3"></cc2:DataConnector></TD>
									</TR>
								</TABLE>
							</TD>
						</TR>
					</TABLE>
				</cc2:datatabcontrol></P>
		</FORM>
	</body>
</HTML>
