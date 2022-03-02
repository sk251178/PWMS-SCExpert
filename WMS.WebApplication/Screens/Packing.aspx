<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Packing.aspx.vb" Inherits="WMS.WebApp.Packing"%>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Packing</title>
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
			<cc3:screen id="Screen1" runat="server" ScreenID="pa"></cc3:screen>
			<P><cc2:tableeditor id="TEMasterOutboundOrders" runat="server" AutoSelectMode="View" AutoSelectGridItem="Never"
					ObjectID="TEMasterOutboundOrders" DefaultDT="DTORDERSPACK" ManualMode="False" DefaultMode="Grid"
					GridPageSize="5" DESIGNTIMEDRAGDROP="14" ForbidRequiredFieldsInSearchMode="True" DisableSwitches="ien"
					SearchDT="DTORDERSPACKSearch" AfterInsertMode="Grid" AfterUpdateMode="Grid" GridDT="DTORDERSPACK"
					FilterExpression="OutboundOrderPack.STATUS in ('STAGED','PICKED')"></cc2:tableeditor></P>
			<P>&nbsp;</P>
			<P><cc2:datatabcontrol id="DTC" runat="server" SyncEditMode="True" ParentID="TEMasterOutboundOrders">
					<TABLE id="Table1" cellSpacing="0" cellPadding="0" border="0" runat="server">
						<TR>
							<TD>
								<cc2:tabstrip id="TS" runat="server" TargetTableID="Tbl" SepDefaultStyle="border-bottom:solid 1px gray;"
									TabSelectedStyle="border:solid 1px gray;border-bottom:none;background:white;" TabHoverStyle="background-color:#DDDDDD;"
									TabDefaultStyle="background-color:#DDDDDD;border:solid 1px gray;padding-left:5px;padding-right:5px;font-size:13px;font-weight:bold;padding-top:2px;padding-bottom:2px;"
									AutoPostBack="True">
									<iewc:Tab Text="Loads"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="Containers"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
								</cc2:tabstrip></TD>
						</TR>
						<TR>
							<TD style="BORDER-RIGHT: gray 1px solid; BORDER-LEFT: gray 1px solid; BORDER-BOTTOM: gray 1px solid; HEIGHT: 18px">
								<TABLE id="Tbl" cellSpacing="0" cellPadding="5" border="0" runat="server">
									<TR>
										<TD>
											<asp:panel id="pnlcnt" Runat="server">
												<TABLE border="0">
													<TR>
														<TD style="HEIGHT: 16px">
															<cc2:fieldLabel id="lblContID" runat="server" text="Container Id"></cc2:fieldLabel></TD>
														<TD style="HEIGHT: 16px">
															<cc2:TextBox id="txtShippingContainer" runat="server"></cc2:TextBox></TD>
													</TR>
												</TABLE>
											</asp:panel>
											<cc2:TableEditor id="TEPackOrder" runat="server" FilterExpression="usagetype = 'PICKCONT' or usagetype ='' or usagetype is null" GridDT="DTOutOrderPack"
												AfterUpdateMode="Grid" AfterInsertMode="Grid" DisableSwitches="rnie" ForbidRequiredFieldsInSearchMode="True"
												GridPageSize="10" DefaultMode="Grid" DefaultDT="DTOutOrderPack" ObjectID="TEPackOrder" AutoSelectGridItem="Never"
												AutoSelectMode="View"></cc2:TableEditor>
											<cc2:DataConnector id="DC1" runat="server" DESIGNTIMEDRAGDROP="43" TargetID="TEPackOrder" MasterFields="consignee,orderid"
												ChildFields="consignee,orderid" MasterObjID="TEMasterOutboundOrders" SyncMode="Reversed"></cc2:DataConnector></TD>
										<TD>
											<cc2:TableEditor id="TEUnpackOrder" runat="server" FilterExpression="usagetype = 'SHIPCONT'" GridDT="DTOutOrderUnPack"
												DisableSwitches="ridnvte" ForbidRequiredFieldsInSearchMode="True" DefaultMode="Grid" DefaultDT="DTOutOrderUnPack"
												AutoSelectGridItem="Never" AutoSelectMode="View"></cc2:TableEditor>
											<cc2:DataConnector id="DC2" runat="server" TargetID="TEUnpackOrder" MasterFields="consignee,orderid"
												ChildFields="consignee,orderid" MasterObjID="TEMasterOutboundOrders"></cc2:DataConnector></TD>
									</TR>
								</TABLE>
							</TD>
						</TR>
					</TABLE>
				</cc2:datatabcontrol></P>
		</form>
	</body>
</HTML>
