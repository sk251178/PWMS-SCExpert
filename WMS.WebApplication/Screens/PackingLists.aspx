<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PackingLists.aspx.vb" Inherits="WMS.WebApp.PackingLists" %>
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
			<cc3:screen id="Screen1" runat="server" ScreenID="pac"></cc3:screen>
			<P><cc2:tableeditor id="TEPackList" runat="server" AutoSelectMode="View" AutoSelectGridItem="Never"
					ObjectID="TEPackList" DefaultDT="DTPacklist" ManualMode="False" DefaultMode=Search 
					GridPageSize="5" DESIGNTIMEDRAGDROP="14" ForbidRequiredFieldsInSearchMode="True" DisableSwitches="n"
					SearchDT="DTPacklistSearch" AfterInsertMode="Grid" AfterUpdateMode="Grid" GridDT="DTPacklist" InsertDT="DTPacklistInsert" EditDT="DTPacklistEdit"></cc2:tableeditor></P>
			<P>&nbsp;</P>
			<P><cc2:datatabcontrol id="DTC" runat="server" SyncEditMode="True" ParentID="TEPackList">
					<TABLE id="Table1" cellSpacing="0" cellPadding="0" border="0" runat="server">
						<TR>
							<TD>
								<cc2:tabstrip id="TS" runat="server" TargetTableID="Tbl" SepDefaultStyle="border-bottom:solid 1px gray;"
									TabSelectedStyle="border:solid 1px gray;border-bottom:none;background:white;" TabHoverStyle="background-color:#DDDDDD;"
									TabDefaultStyle="background-color:#DDDDDD;border:solid 1px gray;padding-left:5px;padding-right:5px;font-size:13px;font-weight:bold;padding-top:2px;padding-bottom:2px;"
									AutoPostBack="True">
									<iewc:Tab Text="Content"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="Pack Loads"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
								</cc2:tabstrip></TD>
						</TR>
						<TR>
							<TD style="BORDER-RIGHT: gray 1px solid; BORDER-LEFT: gray 1px solid; BORDER-BOTTOM: gray 1px solid; HEIGHT: 18px">
								<TABLE id="Tbl" cellSpacing="0" cellPadding="5" border="0" runat="server">
									<TR>
										<TD>
											<cc2:TableEditor id="TEPackListDetail" runat="server" GridDT="DTPackListDetail"
												AfterUpdateMode="Grid" AfterInsertMode="Grid" DisableSwitches="rnie" ForbidRequiredFieldsInSearchMode="True"
												GridPageSize="10" DefaultMode="Grid" DefaultDT="DTPackListDetail" ObjectID="TEPackOrder" AutoSelectGridItem="Never"
												AutoSelectMode="View"></cc2:TableEditor>
											<cc2:DataConnector id="DC1" runat="server" DESIGNTIMEDRAGDROP="43" TargetID="TEPackListDetail" MasterFields="packinglistid"
												ChildFields="packinglistid" MasterObjID="TEPackList" SyncMode="Reversed"></cc2:DataConnector></TD>
										<TD> 
											<cc2:TableEditor id="TELoadsSelect" runat="server" GridDT="DTPacklistLoads"
												DisableSwitches="ridnvte" ForbidRequiredFieldsInSearchMode="True" DefaultMode="Grid" DefaultDT="DTPacklistLoadsSearch"
												AutoSelectGridItem="Never" AutoSelectMode="View" FilterExpression="Loadid not in (select loadid from PACKINGLISTDETAIL)"></cc2:TableEditor>
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
