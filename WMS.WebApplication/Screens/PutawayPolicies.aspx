<%@ Page Language="vb" AutoEventWireup="false" Codebehind="PutawayPolicies.aspx.vb" Inherits="WMS.WebApp.PutawayPolicies"%>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Putaway Policies</title>
		<meta content="False" name="vs_showGrid">
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<!-- #include file="~/include/head.html" -->
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout">
		<!-- #include file="~/include/Header.html" -->
		<FORM id="Form1" method="post" runat="server">
            <telerik:RadScriptManager ID="RadScriptManager2" runat="server">     </telerik:RadScriptManager> 
			<cc3:screen id="Screen1" runat="server" ScreenID="pp"></cc3:screen>
			<P><cc2:tableeditor id="TEPW" runat="server" DefaultDT="PWPolicy" ManualMode="False" DefaultMode="Search"
					TableName="PUTAWAYPOLICY" GridPageSize="5" DESIGNTIMEDRAGDROP="14" EditVS="0" EditDT="PWPolicyEdit"
					AfterInsertMode="Grid" AfterUpdateMode="Grid" AllowDeleteInViewMode="True" DisableSwitches="mn"
					ForbidRequiredFieldsInSearchMode="True" AutoSelectGridItem="Never" AutoSelectMode="View"></cc2:tableeditor></P>
			<P>&nbsp;</P>
			<P><cc2:datatabcontrol id="DTC" runat="server" SyncEditMode="True" ParentID="TEPW">
					<TABLE id="Table1" cellSpacing="0" cellPadding="0" border="0" runat="server"> 
						<TR>
							<TD>
								<cc2:tabstrip id="TS" runat="server" TargetTableID="Tbl" SepDefaultStyle="border-bottom:solid 1px gray;"
									TabSelectedStyle="border:solid 1px gray;border-bottom:none;background:white;" TabHoverStyle="background-color:#DDDDDD;"
									TabDefaultStyle="background-color:#DDDDDD;border:solid 1px gray;padding-left:5px;padding-right:5px;font-size:13px;font-weight:bold;padding-top:2px;padding-bottom:2px;"
									AutoPostBack="True">
									<iewc:Tab Text="Lines"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="Scoring"></iewc:Tab>
									<iewc:TabSeparator DefaultStyle="width:100%;"></iewc:TabSeparator>
								</cc2:tabstrip></TD>
						</TR>
						<TR>
							<TD style="BORDER-RIGHT: gray 1px solid; BORDER-LEFT: gray 1px solid; BORDER-BOTTOM: gray 1px solid; HEIGHT: 18px">
								<TABLE id="Tbl" cellSpacing="0" cellPadding="5" border="0" runat="server">
									<TR>
										<TD>
											<cc2:TableEditor id="TEPWDETAIL" runat="server" ForbidRequiredFieldsInSearchMode="True" DisableSwitches="mn"
												AllowDeleteInViewMode="False" DESIGNTIMEDRAGDROP="60" TableName="PUTAWAYPOLICYDETAIL" DefaultMode="Grid"
												DefaultDT="PWPolicyDetail" ViewDT="PWPolicyDetailView" SQL="SELECT * FROM [PUTAWAYPOLICYDETAIL]" AutoSelectGridItem="Never"
												AutoSelectMode="View"></cc2:TableEditor>
											<cc2:DataConnector id="DC1" runat="server" DESIGNTIMEDRAGDROP="38" TargetID="TEPWDETAIL" MasterGridID=""
												MasterFields="PUTAWAYPOLICY" ChildFields="PUTAWAYPOLICY" MasterObjID="TEPW"></cc2:DataConnector></TD>
										<TD>
											<cc2:TableEditor id="TEPWScoring" runat="server" ForbidRequiredFieldsInSearchMode="True" DisableSwitches="mn"
												AllowDeleteInViewMode="False" DESIGNTIMEDRAGDROP="60" DefaultMode="Grid"
												DefaultDT="PWPolicyScoring" AutoSelectGridItem="Never"
												AutoSelectMode="View"></cc2:TableEditor>
											<cc2:DataConnector id="Dataconnector1" runat="server" DESIGNTIMEDRAGDROP="38" TargetID="TEPWScoring" MasterGridID=""
												MasterFields="PUTAWAYPOLICY" ChildFields="STRATEGYID" MasterObjID="TEPW"></cc2:DataConnector></TD>
									</TR>
								</TABLE>
							</TD>
						</TR>
					</TABLE>
				</cc2:datatabcontrol></P>
		</FORM>
	</body>
</HTML>
