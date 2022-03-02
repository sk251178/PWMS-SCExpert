<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="SKU.aspx.vb" Inherits="WMS.WebApp.SKU"%>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>SKU Setup</title>
		<meta content="False" name="vs_showGrid">
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<!-- #include file="~/include/head.html" -->
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout" dir="ltr">
		<!-- #include file="~/include/Header.html" -->
		<FORM id="Form1" method="post" runat="server">  
            <telerik:RadScriptManager ID="RadScriptManager2" runat="server">     </telerik:RadScriptManager> 
			<cc3:screen id="Screen1" runat="server" ScreenID="st"></cc3:screen>
			<P><cc2:tableeditor id="TEMaster" runat="server" SearchButtonPos="UnderSearchForm" ObjectID="SKUMaster"
					DisableSwitches="md" DESIGNTIMEDRAGDROP="14" GridPageSize="5" DefaultMode="Search" ManualMode="False"
					DefaultDT="DTSKU" SearchDT="DTSKUSearch" EditDT="DTSKUEdit" MultiEditDT="DTSKUMultiEdit" InsertDT="DTSKUInsert" EnableSavedSearch="On"
					AutoSelectGridItem="Never" AutoSelectMode="View" AllowMultiEditInEditMode="False"></cc2:tableeditor></P> 
			<P>&nbsp;</P>
			<P><cc2:datatabcontrol id="DTC" runat="server" ParentID="TEMaster" SyncEditMode="True">
					<TABLE id="Table1" cellSpacing="0" cellPadding="0" border="0" runat="server">
						<TR>
							<TD>
								<cc2:tabstrip id="TS" runat="server" AutoPostBack="True" TabDefaultStyle="background-color:#DDDDDD;border:solid 1px gray;padding-left:5px;padding-right:5px;font-size:13px;font-weight:bold;padding-top:2px;padding-bottom:2px;"
									TabHoverStyle="background-color:#DDDDDD;" TabSelectedStyle="border:solid 1px gray;border-bottom:none;background:white;"
									SepDefaultStyle="border-bottom:solid 1px gray;" TargetTableID="Tbl">
									<iewc:Tab Text="General"></iewc:Tab>
									<iewc:TabSeparator DefaultStyle="border-bottom:soild 1 gray;"></iewc:TabSeparator>
									<iewc:Tab Text="Inventory"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="SKU Definitions"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="UOM"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="BOM"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="SKU Substitution"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="Pick Locations"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="Labor Performance"></iewc:Tab>
									<iewc:TabSeparator DefaultStyle="width:100%;"></iewc:TabSeparator>
								</cc2:tabstrip></TD>
						</TR>
						<TR>
							<TD style="BORDER-RIGHT: gray 1px solid; BORDER-LEFT: gray 1px solid; BORDER-BOTTOM: gray 1px solid; HEIGHT: 18px">
								<TABLE id="Tbl" cellSpacing="0" cellPadding="5" border="0" runat="server">
									<TR>
										<TD>
											<cc2:TableEditor id="TEGeneral" runat="server" AutoSelectMode="View" AutoSelectGridItem="Never" DefaultDT="DTSKU General"
												DefaultMode="View" DESIGNTIMEDRAGDROP="60" DisableSwitches="tsidrmn" AfterInsertMode="View" EditDT="DTSKUGeneralEdit" AfterUpdateMode="View"></cc2:TableEditor>
											<cc2:DataConnector id="DC1" runat="server" DESIGNTIMEDRAGDROP="38" SyncMode="Reversed" MasterObjID="TEMaster"
												ChildFields="consignee,sku" MasterFields="consignee,sku" TargetID="TEGeneral"></cc2:DataConnector></TD>
										<TD>
											<cc2:TableEditor id="TEInventory" runat="server" AutoSelectMode="View" AutoSelectGridItem="Never"
												DefaultDT="DTSKU Inventory" ViewDT="DTSKUInventoryEdit" EditDT="DTSKUInventoryEdit" DefaultMode="View" DisableSwitches="tsidrmn" AfterInsertMode="View"
												AfterUpdateMode="View"></cc2:TableEditor>
											<cc2:DataConnector id="DC2" runat="server" DESIGNTIMEDRAGDROP="43" MasterObjID="TEMaster" ChildFields="consignee,sku"
												MasterFields="consignee,sku" TargetID="TEInventory"></cc2:DataConnector></TD>
										<TD>
											<cc2:TableEditor id="TESkuAtt" runat="server" AutoSelectMode="View" AutoSelectGridItem="Never" DefaultDT="DTSKUATTRIBUTESVIEW"
												DefaultMode="Grid" DisableSwitches="tsrmn" AfterInsertMode="Grid" AfterUpdateMode="Grid"></cc2:TableEditor>
											<cc2:DataConnector id="DC3" runat="server" DESIGNTIMEDRAGDROP="43" MasterObjID="TEMaster" ChildFields="consignee,sku"
												MasterFields="consignee,sku" TargetID="TESkuAtt"></cc2:DataConnector></TD>
										<TD>
											<cc2:TableEditor id="TESKUUOM" runat="server" AutoSelectMode="View" AutoSelectGridItem="Never" InsertDT="DTSKUUOMEDIT"
												EditDT="DTSKUUOMEDIT" DefaultDT="DTSKU Unit Of Measure" DefaultMode="Grid" GridPageSize="20"
												DisableSwitches="tsmn" SortExperssion="UnitsPerLowestUOM asc"></cc2:TableEditor>
											<cc2:DataConnector id="DC5" runat="server" DESIGNTIMEDRAGDROP="43" MasterObjID="TEMaster" ChildFields="consignee,sku"
												MasterFields="consignee,sku" TargetID="TESKUUOM"></cc2:DataConnector></TD>
										<TD>
											<cc2:TableEditor id="TEBOM" runat="server" AutoSelectMode="View" AutoSelectGridItem="Never" DefaultDT="DTSKU BOM"
												DefaultMode="Grid" EditDT="DTSKU BOM Edit" GridPageSize="30" DESIGNTIMEDRAGDROP="29" DisableSwitches="tsmn"></cc2:TableEditor>
											<cc2:DataConnector id="DC4" runat="server" MasterObjID="TEMaster" ChildFields="consignee,bomsku" MasterFields="consignee,sku"
												TargetID="TEBOM"></cc2:DataConnector></TD>
										<TD>
                                            <cc2:TableEditor id="TESkuSubt" runat="server" AlwaysShowGrid="True" AlwaysShowSearch="True" AutoSelectMode="View" AutoSelectGridItem="Never" InsertDT="DTSKUSUBTITUTENew"
                                                DefaultDT="DTSKUSUBTITUTE" EditDT='DTSKUSUBTITUTEEdit' SearchDT='DTSKUSUBTITUTESeArch' DefaultMode="Search" GridPageSize="40" DESIGNTIMEDRAGDROP="29" DisableSwitches="tmn"></cc2:TableEditor>
                                            <cc2:DataConnector id="DC12" runat="server" MasterObjID="TEMaster" ChildFields="consignee,sku"
                                                MasterFields="consignee,sku" TargetID="TESkuSubt"></cc2:DataConnector>
                                            </TD>
										<TD>
											<cc2:TableEditor id="TEPickLoc" runat="server" AutoSelectMode="View" AutoSelectGridItem="Never" SearchDT="DTSkuPickLocSearch"
												DefaultDT="DTSkuPickLocInsert" DefaultMode="Grid" GridPageSize="50" DESIGNTIMEDRAGDROP="29" DisableSwitches="tsmnei"
												GridDT="DTSkuPickLocResults" InsertDT="DTSkuPickLocInsert" EditDT="DTSkuPickLocEdit"></cc2:TableEditor>
											<cc2:DataConnector id="DC8" runat="server" MasterObjID="TEMaster" ChildFields="consignee,sku" MasterFields="consignee,sku"
												TargetID="TEPickLoc"></cc2:DataConnector></TD>
										<TD>
											<cc2:TableEditor id="TELaborPerformance" runat="server" AutoSelectMode="View" AutoSelectGridItem="Never" 
												InsertDT="DTLaborPerformanceUOMEdit" EditDT="DTLaborPerformanceUOMEdit" DefaultDT="DTLaborPerformanceUOM"
												DefaultMode="Grid" GridPageSize="60" DisableSwitches="tsmn"></cc2:TableEditor>
											<cc2:DataConnector id="DC9" runat="server" DESIGNTIMEDRAGDROP="43" MasterObjID="TEMaster" ChildFields="consignee,sku"
												MasterFields="consignee,sku" TargetID="TELaborPerformance"></cc2:DataConnector></TD>
									</TR>
								</TABLE>
							</TD>
						</TR>
					</TABLE>
				</cc2:datatabcontrol></P>
		</FORM>
	</body>
</HTML>
