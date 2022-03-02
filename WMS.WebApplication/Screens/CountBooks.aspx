<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CountBooks.aspx.vb" Inherits="WMS.WebApp.CountBooks" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >

<HTML>
	<HEAD>
		<title>CountBooks</title>
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
			<cc3:screen id="Screen1" runat="server" ScreenID="cb"></cc3:screen>
			<P><cc2:tableeditor id="TECountBook" runat="server" DisableSwitches="rn" SearchDT="DTCountBookHeaderSearch" InsertDT="DTCountBookHeaderInsert" 
					ForbidRequiredFieldsInSearchMode="True" DESIGNTIMEDRAGDROP="14" GridPageSize="10" DefaultMode="Search"
					ManualMode="False" EditDT="DTCountBookHeaderEdit" DefaultDT="DTCountBookHeader" AfterUpdateMode="Grid"></cc2:tableeditor></P>
			<P>&nbsp;</P>
			<P><cc2:datatabcontrol id="DTC" runat="server" ParentID="TECountBook" SyncEditMode="True">
					<TABLE id="Table1" cellSpacing="0" cellPadding="0" border="0" runat="server">
						<TR>
							<TD>
								<cc2:tabstrip id="TS" runat="server" AutoPostBack="True" TabDefaultStyle="background-color:#DDDDDD;border:solid 1px gray;padding-left:5px;padding-right:5px;font-size:13px;font-weight:bold;padding-top:2px;padding-bottom:2px;"
									TabHoverStyle="background-color:#DDDDDD;" TabSelectedStyle="border:solid 1px gray;border-bottom:none;background:white;"
									SepDefaultStyle="border-bottom:solid 1px gray;" TargetTableID="Tbl">
									<iewc:Tab Text="Count Summary"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="Count Tasks"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
								</cc2:tabstrip></TD>
						</TR>
						<TR>
							<TD style="BORDER-RIGHT: gray 1px solid; BORDER-LEFT: gray 1px solid; BORDER-BOTTOM: gray 1px solid; HEIGHT: 18px">
								<TABLE id="Tbl" cellSpacing="0" cellPadding="5" border="0" runat="server">
									<TR>
										<TD>
											<TABLE border="0">
												<TR>
													<TD style="HEIGHT: 16px">
														<cc2:fieldLabel id="lblSummaryType" runat="server" text="Summary by:"></cc2:fieldLabel>													
														<cc2:dropdownlist id="ddSummaryType" runat="server" AutoPostBack="True" ValueList="Location;Location;LoadDiscrepancies;LoadDiscrepancies"></cc2:dropdownlist></TD>
												</TR>
												<TR>
													<TD>
														<asp:panel id="pnlLocationSummary" Runat="server">
															<cc2:TableEditor id="TELocSummary" runat="server" DisableSwitches="eidvrnm" SearchDT="DTCountBookLocSummarySearch"
																ForbidRequiredFieldsInSearchMode="True" GridPageSize="10" DefaultMode="Search" DefaultDT="DTCountBookLocSummary"
																AutoSelectMode="View" AutoSelectGridItem="Never"></cc2:TableEditor>
															<cc2:DataConnector id="DC1" runat="server" DESIGNTIMEDRAGDROP="43" MasterObjID="TECountBook" ChildFields="countbook,countbookrunid"
																MasterFields="countbook,countbookrunid" TargetID="TELocSummary"></cc2:DataConnector>
														</asp:panel>
														<asp:panel id="pnlLoadDiscrepancies" Runat="server">
															<cc2:TableEditor id="TECountDisc" runat="server" DisableSwitches="eidvrnm" SearchDT="DTCountBookDiscrepanciesSearch"
																ForbidRequiredFieldsInSearchMode="True" GridPageSize="10" DefaultMode="Search" DefaultDT="DTCountBookDiscrepancies"
																AutoSelectMode="View" AutoSelectGridItem="Never"></cc2:TableEditor>
															<cc2:DataConnector id="Dataconnector2" runat="server" DESIGNTIMEDRAGDROP="43" MasterObjID="TECountBook"
																ChildFields="countbook,countbookrunid" MasterFields="countbook,countbookrunid" TargetID="TECountDisc"></cc2:DataConnector>
														</asp:panel></TD>
												</TR>
											</TABLE>
										</TD>
										<TD>
											<cc2:TableEditor id="TETasks" runat="server" DisableSwitches="ridve" ForbidRequiredFieldsInSearchMode="True"
												DefaultMode="Grid" DefaultDT="DTCountBookTasks" AutoSelectMode="View" AutoSelectGridItem="Never"
												GridDT="DTCountBookTasks" MultiEditDT="DTCountBookTasksMulty"></cc2:TableEditor>
											<cc2:DataConnector id="Dataconnector1" runat="server" MasterObjID="TECountBook" ChildFields="countbook,countbookrunid"
												MasterFields="countbook,countbookrunid" TargetID="TETasks"></cc2:DataConnector></TD>
									</TR>
								</TABLE>
						</TR>
					</TABLE></TD></TR></TABLE></cc2:datatabcontrol></P>
		</form>
	</body>
</HTML>
