<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="InvConsignee.aspx.vb" Inherits="WMS.WebApp.InvConsignee"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>InvConsignee</title>
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
			<cc3:screen id="Screen1" runat="server" ScreenID="in"></cc3:screen>
			<P><cc2:tableeditor id="TEInvByConsignee" runat="server" ManualMode="False" DefaultDT="DTConsigneeSearch"
					DefaultMode="Search" GridPageSize="5" DisableSwitches="veidmn" DESIGNTIMEDRAGDROP="14" AutoSelectGridItem="Never"
					AutoSelectMode="View"></cc2:tableeditor></P>
			<P>&nbsp;</P>
			<P><cc2:datatabcontrol id="DTC" runat="server" SyncEditMode="True" ParentID="TEInvByConsignee">
					<TABLE id="Table1" cellSpacing="0" cellPadding="0" border="0" runat="server">
						<TR>
							<TD>
								<cc2:tabstrip id="TS" runat="server" TabDefaultStyle="background-color:#DDDDDD;border:solid 1px gray;padding-left:5px;padding-right:5px;font-size:13px;font-weight:bold;padding-top:2px;padding-bottom:2px;"
									TabHoverStyle="background-color:#DDDDDD;" TabSelectedStyle="border:solid 1px gray;border-bottom:none;background:white;"
									SepDefaultStyle="border-bottom:solid 1px gray;" TargetTableID="Tbl" AutoPostBack="True">
									<iewc:Tab Text="By Loads"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="By SKU"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="By SKU By Location"></iewc:Tab>
									<iewc:TabSeparator DefaultStyle="width:100%;"></iewc:TabSeparator>
								</cc2:tabstrip></TD>
						</TR>
						<TR>
							<TD style="BORDER-RIGHT: gray 1px solid; BORDER-LEFT: gray 1px solid; BORDER-BOTTOM: gray 1px solid; HEIGHT: 18px">
								<TABLE id="Tbl" cellSpacing="0" cellPadding="5" border="0" runat="server">
									<TR>
										<TD>
											<cc2:TableEditor id="TEInvConsSkuLoad" runat="server" DisableSwitches="eidmntv" GridPageSize="10"
												DefaultMode="Grid" DefaultDT="DTinvConsSkuLoad" SearchDT="DTLoadSearchLocByConsignee" AutoSelectGridItem="Never"
												AutoSelectMode="View"></cc2:TableEditor>
											<cc2:DataConnector id="DC1" runat="server" DESIGNTIMEDRAGDROP="43" MasterObjID="TEInvByConsignee" ChildFields="consignee"
												MasterFields="consignee" TargetID="TEInvConsSkuLoad"></cc2:DataConnector></TD>
										<TD>
											<cc2:TableEditor id="TEInvConsSku" runat="server" DisableSwitches="eidmntv" GridPageSize="10" DefaultMode="Grid"
												DefaultDT="DTINVCONSBYSKU" SearchDT="DTINVCONSBYSKUSEARCH" GridDT="DTINVCONSBYSKU" AutoSelectGridItem="Never"
												AutoSelectMode="View"></cc2:TableEditor>
											<cc2:DataConnector id="DC2" runat="server" DESIGNTIMEDRAGDROP="43" MasterObjID="TEInvByConsignee" ChildFields="consignee"
												MasterFields="consignee" TargetID="TEInvConsSku"></cc2:DataConnector></TD>
										<TD>
											<cc2:TableEditor id="TEInvConsSkuByLocation" runat="server" DisableSwitches="eidmntv" GridPageSize="10"
												DefaultMode="Grid" DefaultDT="DTINVCONSBYSKUBYLOCATION" SearchDT="DTINVCONSBYSKUBYLOCATIONSEARCH"
												AutoSelectGridItem="Never" AutoSelectMode="View"></cc2:TableEditor>
											<cc2:DataConnector id="Dataconnector2" runat="server" DESIGNTIMEDRAGDROP="43" MasterObjID="TEInvByConsignee"
												ChildFields="consignee" MasterFields="consignee" TargetID="TEInvConsSkuByLocation"></cc2:DataConnector></TD>
									</TR>
								</TABLE>
							</TD>
						</TR>
					</TABLE>
				</cc2:datatabcontrol>
			</P>
		</form>
	</body>
</HTML>
