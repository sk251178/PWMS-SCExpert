<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="InboundOrder.aspx.vb" Inherits="WMS.WebApp.InboundOrder"%>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Inbound Order</title>
		<meta content="False" name="vs_showGrid">
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
			<cc3:screen id="Screen1" runat="server" ScreenID="io"></cc3:screen>
		<P><cc2:tableeditor id="TEMasterInboundOrders" runat="server" ObjectID="TEMasterInboundOrders" SQL="SELECT distinct CONSIGNEE,CARRIERID,SCHEDULEDATE,APPOINTMENTID, ORDERID, HOSTORDERID,BUYER, ORDERTYPE, REFERENCEORD, SOURCECOMPANY, COMPANYTYPE, STATUS, CREATEDATE, convert(nvarchar,NOTES) as NOTES, COMPANYNAME, ADDDATE, ADDUSER, EDITDATE, EDITUSER,EXPECTEDDATE, RECEIVEDFROM FROM  INBOUNDORDERVIEW"
					AfterInsertMode="Grid" SortExperssion="CREATEDATE DESC"  AfterUpdateMode="Grid" SearchDT="DTINBOUNDORDERVIEWSearch" InsertDT="DTInboundOrder Insert"
					DisableSwitches="mn" ForbidRequiredFieldsInSearchMode="True" DESIGNTIMEDRAGDROP="14" GridPageSize="5"
					DefaultMode="Search" ManualMode="False" DefaultDT="DTINBOUNDORDERVIEW" EditDT="DTInboundOrder Edit"
					></cc2:tableeditor></P>
			<P>&nbsp;</P>
			<P><cc2:datatabcontrol id="DTC" runat="server" ParentID="TEMasterInboundOrders" SyncEditMode="True">
					<TABLE id="Table1" cellSpacing="0" cellPadding="0" border="0" runat="server">
						<TR>
							<TD>
								<cc2:tabstrip id="TS" runat="server" TargetTableID="Tbl" SepDefaultStyle="border-bottom:solid 1px gray;"
									TabSelectedStyle="border:solid 1px gray;border-bottom:none;background:white;" TabHoverStyle="background-color:#DDDDDD;"
									TabDefaultStyle="background-color:#DDDDDD;border:solid 1px gray;padding-left:5px;padding-right:5px;font-size:13px;font-weight:bold;padding-top:2px;padding-bottom:2px;"
									AutoPostBack="True" >
									<iewc:Tab Text="Details"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="Receipt Lines"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="Handling Units"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="Contact Detail"></iewc:Tab>
									<iewc:TabSeparator DefaultStyle="width:100%;"></iewc:TabSeparator>
								</cc2:tabstrip></TD>
						</TR>
						<TR>
							<TD style="BORDER-RIGHT: gray 1px solid; BORDER-LEFT: gray 1px solid; BORDER-BOTTOM: gray 1px solid; HEIGHT: 18px">
								<TABLE id="Tbl" cellSpacing="0" cellPadding="5" border="0" runat="server">
									<TR>
										<TD>
											<cc2:TableEditor id="TEInboundOrderLines" runat="server" AutoSelectMode="View" AutoSelectGridItem="Never"
												EditDT="DTInboundOrder Lines Edit" DefaultDT="DTInboundOrder Lines" DefaultMode="Grid" GridPageSize="10"
												ForbidRequiredFieldsInSearchMode="True" DisableSwitches="trn" InsertDT="DTInboundOrder Lines Add"
												SearchDT="DTInboundOrder Lines Search" ObjectID="TEInboundOrderLines" TableName="INBOUNDORDDETAIL"></cc2:TableEditor>
											<cc2:DataConnector id="DC1" runat="server" DESIGNTIMEDRAGDROP="43" TargetID="TEInboundOrderLines" MasterFields="consignee,orderid"
												ChildFields="consignee,orderid" MasterObjID="TEMasterInboundOrders"></cc2:DataConnector></TD>
										<TD>
											<cc2:TableEditor id="TEReceiptLines" runat="server" AutoSelectMode="View" AutoSelectGridItem="Never"
												DefaultDT="DTInboundReceiptLines" DefaultMode="Grid" GridPageSize="10" ForbidRequiredFieldsInSearchMode="True"
												DisableSwitches="eidmnt" TableName="ReceiptDetail"></cc2:TableEditor>
											<cc2:DataConnector id="DC2" runat="server" DESIGNTIMEDRAGDROP="43" TargetID="TEReceiptLines" MasterFields="consignee,orderid"
												ChildFields="consignee,orderid" MasterObjID="TEMasterInboundOrders"></cc2:DataConnector></TD>
										<TD>
											<cc2:TableEditor id="TEHUTrans" runat="server" AutoSelectMode="View" AutoSelectGridItem="Never" EditDT="TEHUTransEdit"
												DefaultDT="DTHUTrans" DefaultMode="Grid" GridPageSize="10" ForbidRequiredFieldsInSearchMode="True"
												DisableSwitches="dmnvs" InsertDT="TEHUTransInsert" ObjectID="TEHUTrans" FilterExpression="TRANSACTIONTYPE = 'INORD'"></cc2:TableEditor>
											<cc2:DataConnector id="DC4" runat="server" DESIGNTIMEDRAGDROP="43" TargetID="TEHUTrans" MasterFields="consignee,orderid"
												ChildFields="consignee,orderid" MasterObjID="TEMasterInboundOrders"></cc2:DataConnector></TD>
										<TD>
											<cc2:TableEditor id="TEContactDetail" runat="server" AutoSelectMode="View" AutoSelectGridItem="Never"
												DefaultDT="DTCONTACT" DefaultMode="View" ForbidRequiredFieldsInSearchMode="True" DisableSwitches="rmdvtsie"
												InsertDT="DTShipTo"></cc2:TableEditor>
											<cc2:DataConnector id="DC3" runat="server" TargetID="TEContactDetail" MasterFields="receivedfrom" ChildFields="contactid"
												MasterObjID="TEMasterInboundOrders"></cc2:DataConnector></TD>
									</TR>
								</TABLE>
							</TD>
						</TR>
					</TABLE>
				</cc2:datatabcontrol></P>
		</form>
	</body>
</HTML>
