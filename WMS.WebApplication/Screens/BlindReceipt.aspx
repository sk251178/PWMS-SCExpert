<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="BlindReceipt.aspx.vb" Inherits="WMS.WebApp.BlindReceipt"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>BlindReceipt</title>
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
			<cc3:screen id="Screen1" runat="server" ScreenID="br"></cc3:screen>
			<P><cc2:tableeditor id="TEMasterReceipt" runat="server" DisableSwitches="n" ForbidRequiredFieldsInSearchMode="True"
					SearchDT="DTRECEIPTHEADERVIEWSearch" DESIGNTIMEDRAGDROP="14" GridPageSize="5" DefaultMode="Search"
					ManualMode="False" DefaultDT="DTReceiptHeader" EditDT="DTReceiptHeader" InsertDT="DTReceiptHeader" 
					AfterInsertMode="Grid" GridDT="DTRECEIPTHEADERVIEW" SQL="SELECT distinct RECEIPT, STATUS, SCHEDULEDDATE, BOL, convert(nvarchar,NOTES) as NOTES, CONFIRMED,COMPANY,ADDDATE,COMPANYNAME, LABELPRINTED, STARTRECEIPTDATE, TRAILER, VEHICLE, DRIVER1, DRIVER2, SEAL1, SEAL2,CLOSERECEIPTDATE,TRANSPORTREFERENCE,TRANSPORTTYPE,DOOR,YARDENTRYID,ESTUNLOADINGTIME,CONSIGNEE FROM RECEIPTHEADERVIEW"
					ViewDT="DTRECEIPTHEADERVIEW" FilterExpression="Status <> 'Close'" AutoSelectGridItem="Never" AutoSelectMode="View"></cc2:tableeditor></P>
			<P>&nbsp;</P>
			<P><cc2:datatabcontrol id="DTC" runat="server" SyncEditMode="True" ParentID="TEMasterReceipt">
					<TABLE id="Table1" cellSpacing="0" cellPadding="0" border="0" runat="server">
						<TR>
							<TD>
								<cc2:tabstrip id="TS" runat="server" AutoPostBack="True" TabDefaultStyle="background-color:#DDDDDD;border:solid 1px gray;padding-left:5px;padding-right:5px;font-size:13px;font-weight:bold;padding-top:2px;padding-bottom:2px;"
									TabHoverStyle="background-color:#DDDDDD;" TabSelectedStyle="border:solid 1px gray;border-bottom:none;background:white;"
									SepDefaultStyle="border-bottom:solid 1px gray;" TargetTableID="Tbl">
									<iewc:Tab Text="Create Load"></iewc:Tab>
									<iewc:TabSeparator DefaultStyle="width:100%;"></iewc:TabSeparator>
								</cc2:tabstrip></TD>
						</TR>
						<TR>
							<TD style="BORDER-RIGHT: gray 1px solid; BORDER-LEFT: gray 1px solid; BORDER-BOTTOM: gray 1px solid; HEIGHT: 18px">
								<TABLE id="Tbl" cellSpacing="0" cellPadding="5" border="0" runat="server">
									<TR>
										<TD>
											<cc2:TableEditor id="TECreateLoad" runat="server" AfterInsertMode="Insert" DefaultDT="DTBRCreateLoadWithAttributes"
												DefaultMode="Insert" DisableSwitches="tvedtrmns" ObjectID="TECreateLoad" AfterUpdateMode="Insert"
												AutoSelectGridItem="Never" AutoSelectMode="View"></cc2:TableEditor>
											<cc2:DataConnector id="DC1" runat="server" SyncMode="Normal" MasterObjID="TEMasterReceipt" ChildFields="RECEIPT"
												MasterFields="RECEIPT" TargetID="TECreateLoad"></cc2:DataConnector></TD>
									</TR>
								</TABLE>
							</TD>
						</TR>
					</TABLE>
				</cc2:datatabcontrol></P>
		</form>
	</body>
</HTML>
