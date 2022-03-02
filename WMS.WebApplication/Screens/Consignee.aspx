<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Consignee.aspx.vb" Inherits="WMS.WebApp.Consignee" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Consignee</title>
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
			<cc3:screen id="Screen1" runat="server" ScreenID="cs"></cc3:screen>
			<P><cc2:tableeditor id="TEConsignee" runat="server" ForbidRequiredFieldsInSearchMode="True" AfterUpdateMode="View"
					AfterInsertMode="View" PagerLocation="NextToActionBar" SearchDT="DTConsigneeSearch" AlwaysShowGrid="False"
					DefaultDT="DTConsigneeView" ViewDT="DTConsigneeView" DESIGNTIMEDRAGDROP="14" GridPageSize="5" TableName="Consignee" DefaultMode="Grid"
					DisableSwitches="mnd" ManualMode="False" EditDT="DTConsigneeEdit" AutoSelectGridItem="Never" InsertDT="DTConsigneeNew"
					AutoSelectMode="View"></cc2:tableeditor></P>
			<P>&nbsp;</P>
			<P><cc2:datatabcontrol id="DTC" runat="server" ParentID="TEConsignee" SyncEditMode="True">
					<TABLE id="Table1" cellSpacing="0" cellPadding="0" border="0" runat="server">
						<TR>
							<TD>
								<cc2:tabstrip id="TS" runat="server" AutoPostBack="True" TabDefaultStyle="background-color:#DDDDDD;border:solid 1px gray;padding-left:5px;padding-right:5px;font-size:13px;font-weight:bold;padding-top:2px;padding-bottom:2px;"
									TabHoverStyle="background-color:#DDDDDD;" TabSelectedStyle="border:solid 1px gray;border-bottom:none;background:white;"
									SepDefaultStyle="border-bottom:solid 1px gray;" TargetTableID="Tbl">
									<iewc:Tab Text="Inventory Management"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="Receiving"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="Shipping"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="Contact"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="Putaway"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="Picking"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="Billing"></iewc:Tab>
									<iewc:TabSeparator DefaultStyle="width:100%;"></iewc:TabSeparator>
								</cc2:tabstrip></TD>
						</TR>
						<TR>
							<TD style="BORDER-RIGHT: gray 1px solid; BORDER-LEFT: gray 1px solid; BORDER-BOTTOM: gray 1px solid; HEIGHT: 18px">
								<TABLE id="Tbl" cellSpacing="0" cellPadding="5" border="0" runat="server">
									<TR>
										<TD>
											<cc2:TableEditor id="TEConsigneeInventory" runat="server" AutoSelectMode="View" AutoSelectGridItem="Never"
												DisableSwitches="tsidrmn" DefaultMode="Grid" TableName="Consignee" DESIGNTIMEDRAGDROP="60" DefaultDT="DTConsigneeInventory"
												ForbidRequiredFieldsInSearchMode="True"></cc2:TableEditor>
											<cc2:DataConnector id="DC1" runat="server" DESIGNTIMEDRAGDROP="38" MasterObjID="TEConsignee" ChildFields="consignee"
												MasterFields="consignee" TargetID="TEConsigneeInventory"></cc2:DataConnector></TD>
										<TD>
											<cc2:TableEditor id="TEConsigneeReceiving" runat="server" AutoSelectMode="View" AutoSelectGridItem="Never"
												DisableSwitches="tsidr" DefaultMode="Grid" TableName="Consignee" DESIGNTIMEDRAGDROP="14" DefaultDT="DTConsigneeReceiving"
												ForbidRequiredFieldsInSearchMode="True"></cc2:TableEditor>
											<cc2:DataConnector id="DC2" runat="server" DESIGNTIMEDRAGDROP="43" MasterObjID="TEConsignee" ChildFields="consignee"
												MasterFields="consignee" TargetID="TEConsigneeReceiving"></cc2:DataConnector></TD>
										<TD>
											<cc2:TableEditor id="TEConsigneeShipping" runat="server" AutoSelectMode="View" AutoSelectGridItem="Never"
												DisableSwitches="sidtrmn" DefaultMode="Grid" DefaultDT="DTConsigneeShipping" AfterInsertMode="View"
												AfterUpdateMode="View" ForbidRequiredFieldsInSearchMode="True"></cc2:TableEditor>
											<cc2:DataConnector id="Dataconnector1" runat="server" DESIGNTIMEDRAGDROP="43" MasterObjID="TEConsignee"
												ChildFields="consignee" MasterFields="consignee" TargetID="TEConsigneeShipping"></cc2:DataConnector></TD>
										<TD>
											<cc2:TableEditor id="TEConsigneeContact" runat="server" AutoSelectMode="View" AutoSelectGridItem="Never"
												DisableSwitches="srmni" DefaultMode="Grid" TableName="Contact" DefaultDT="DTConsigneeContactGrid" ForbidRequiredFieldsInSearchMode="True"></cc2:TableEditor>
											<cc2:DataConnector id="Dataconnector3" runat="server" DESIGNTIMEDRAGDROP="43" MasterObjID="TEConsignee"
												ChildFields="CONTACTID" MasterFields="CONTACTID" TargetID="TEConsigneeContact"></cc2:DataConnector></TD>
										<TD>
											<cc2:TableEditor id="TEPW" runat="server" AutoSelectMode="View" AutoSelectGridItem="Never"
												EditDT="ConsigneePWEdit" DisableSwitches="rmn" DefaultMode="Grid"  TableName="CONSIGNEEPUTAWAY"
												DefaultDT="ConsigneePWView" InsertDT="ConsigneePWInsert" ForbidRequiredFieldsInSearchMode="True"></cc2:TableEditor>
											<cc2:DataConnector id="DC4" runat="server" DESIGNTIMEDRAGDROP="43" MasterObjID="TEConsignee" ChildFields="CONSIGNEE"
												MasterFields="CONSIGNEE" TargetID="TEPW"></cc2:DataConnector></TD> 
										<TD>
											<cc2:TableEditor id="TEPicking" runat="server" AutoSelectMode="Edit" AutoSelectGridItem="Never"
												EditDT="DTConsigneePlanPolicyEdit" DisableSwitches="rmn" DefaultMode="Grid" DefaultDT="DTConsigneePlanPolicy"
												ForbidRequiredFieldsInSearchMode="True" InsertDT="DTConsigneePlanPolicyEdit" SortExperssion="Priority"
												GridDT="DTConsigneePlanPolicyGrid"></cc2:TableEditor>
											<cc2:DataConnector id="Dataconnector4" runat="server" DESIGNTIMEDRAGDROP="43" MasterObjID="TEConsignee"
												ChildFields="CONSIGNEE" MasterFields="CONSIGNEE" TargetID="TEPicking"></cc2:DataConnector></TD>
										<TD>
											<cc2:TableEditor id="TEConsigneeBilling" runat="server" DisableSwitches="tsidrmn" DefaultMode="Grid"
												DefaultDT="DTConsigneeBilling" ForbidRequiredFieldsInSearchMode="True" GridDT="DTConsigneeBilling"
												ViewDT="DTConsigneeBilling"></cc2:TableEditor>
											<cc2:DataConnector id="Dataconnector2" runat="server" DESIGNTIMEDRAGDROP="43" MasterObjID="TEConsignee"
												ChildFields="consignee" MasterFields="consignee" TargetID="TEConsigneeBilling"></cc2:DataConnector></TD>
									</TR>
								</TABLE>
							</TD>
						</TR>
					</TABLE>
				</cc2:datatabcontrol></P>
		</FORM>
	</body>
</HTML>
