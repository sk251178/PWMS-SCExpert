<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Equipment.aspx.vb" Inherits="WMS.WebApp.Equipment"%>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Handling Equipment</title>
		<meta content="False" name="vs_showGrid">
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
			<cc3:screen id="Screen1" runat="server" ScreenID="hs"></cc3:screen>
			<P><cc2:tableeditor id="TEMasterEquipment" runat="server" DESIGNTIMEDRAGDROP="14" GridPageSize="20"
					TableName="HANDLINGEQUIPMENT" DefaultMode="Search" ManualMode="False" DefaultDT="DTEquipment"
					SQL="SELECT * FROM [HANDLINGEQUIPMENT]" AlwaysShowSearch="False" ViewDT="DTEquipmentView" SearchDT="DTEquipmentSearch"
					DisableSwitches="mn" ForbidRequiredFieldsInSearchMode="True" EditDT="DTEquipment Edit" AutoSelectGridItem="Never" AutoSelectMode="View"></cc2:tableeditor></P>
			<P>&nbsp;</P>
			<P><cc2:datatabcontrol id="DTC" runat="server" SyncEditMode="True" ParentID="TEMasterEquipment">
					<TABLE id="Table1" cellSpacing="0" cellPadding="0" border="0" runat="server">
						<TR>
							<TD>
								<cc2:tabstrip id="TS" runat="server" TargetTableID="Tbl" SepDefaultStyle="border-bottom:solid 1px gray;"
									TabSelectedStyle="border:solid 1px gray;border-bottom:none;background:white;" TabHoverStyle="background-color:#DDDDDD;"
									TabDefaultStyle="background-color:#DDDDDD;border:solid 1px gray;padding-left:5px;padding-right:5px;font-size:13px;font-weight:bold;padding-top:2px;padding-bottom:2px;"
									AutoPostBack="True">
									<iewc:Tab Text="MH Accessibility"></iewc:Tab>
                                    <%--RWMS-1926--%>
<%--									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="MH Edge Types"></iewc:Tab>--%>
                                    <%--RWMS-1926--%>
									<iewc:TabSeparator DefaultStyle="width:100%;"></iewc:TabSeparator>
								</cc2:tabstrip></TD>
						</TR>
						<TR>
							<TD style="BORDER-RIGHT: gray 1px solid; BORDER-LEFT: gray 1px solid; BORDER-BOTTOM: gray 1px solid; HEIGHT: 18px">
								<TABLE id="Tbl" cellSpacing="0" cellPadding="5" border="0" runat="server">
									<TR>
										<TD>
											<cc2:TableEditor id="TEEquipmentAccessibility" runat="server" AutoSelectMode="View" AutoSelectGridItem="Never"
												ForbidRequiredFieldsInSearchMode="True" DisableSwitches="tsrmn" DefaultDT="DTEquipType" DefaultMode="Grid" EditDT="DTEquipType Edit"></cc2:TableEditor>
											<cc2:DataConnector id="DC3" runat="server" DESIGNTIMEDRAGDROP="43" TargetID="TEEquipmentAccessibility"
												MasterFields="HANDLINGEQUIPMENT" ChildFields="HANDLINGEQUIPMENT" MasterObjID="TEMasterEquipment"></cc2:DataConnector></TD>
                                        <%--RWMS-1926--%>
<%--										<TD>
											<cc2:TableEditor id="TEEquipmentTypeEdges" runat="server" AutoSelectMode="View" AutoSelectGridItem="Never"
												ForbidRequiredFieldsInSearchMode="True" DisableSwitches="tsrmn" DefaultDT="DTMHEEdgeType" DefaultMode="Grid"
												UpdateDT="DTMHEEdgeTypeEdit" InsertDT="DTMHEEdgeTypeEdit"></cc2:TableEditor>
											<cc2:DataConnector id="DC1" runat="server" DESIGNTIMEDRAGDROP="43" TargetID="TEEquipmentTypeEdges"
												MasterFields="HANDLINGEQUIPMENT" ChildFields="MHETYPE" MasterObjID="TEMasterEquipment"></cc2:DataConnector></TD>--%>
                                        <%--RWMS-1926--%>
									</TR>
								</TABLE>
							</TD>
						</TR>
					</TABLE>
				</cc2:datatabcontrol></P>
		</form>
	</body>
</HTML>
