<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="WHArea.aspx.vb" Inherits="WMS.WebApp.WHArea" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Warehouse Areas</title>
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
			<p><cc3:screen id="Screen1" title="Warehouse Area" runat="server" ScreenID="wha"></cc3:screen></p>
			<P><cc2:tableeditor id="TEWHAREA" runat="server" DESIGNTIMEDRAGDROP="14" GridPageSize="5" 
					DefaultMode="Grid" ManualMode="False" DefaultDT="DTWHArea" InsertDT="DTWHAreaInsert" 
					EditDT="DTWHAreaEdit" GridDT="DTWHArea" SearchDT="DTWHArea" DisableSwitches="mnvrd"
					AutoSelectGridItem="Never" AutoSelectMode="View"></cc2:tableeditor></P>
			<P>&nbsp;</P>
			<P><cc2:datatabcontrol id="DTC" runat="server" ParentID="TEWHAREA" SyncEditMode="True">
					<TABLE id="Table1" cellSpacing="0" cellPadding="0" border="0" runat="server">
						<TR>
							<TD>
								<cc2:tabstrip id="TS" runat="server" TargetTableID="Tbl" SepDefaultStyle="border-bottom:solid 1px gray;"
									TabSelectedStyle="border:solid 1px gray;border-bottom:none;background:white;" TabHoverStyle="background-color:#DDDDDD;"
									TabDefaultStyle="background-color:#DDDDDD;border:solid 1px gray;padding-left:5px;padding-right:5px;font-size:13px;font-weight:bold;padding-top:2px;padding-bottom:2px;"
									AutoPostBack="True">
									<iewc:Tab Text="Warehouse Area Users"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="Assign Users"></iewc:Tab>
									<iewc:TabSeparator DefaultStyle="width:100%;"></iewc:TabSeparator>
								</cc2:tabstrip></TD>
						</TR>
						<TR>
							<TD style="BORDER-RIGHT: gray 1px solid; BORDER-LEFT: gray 1px solid; BORDER-BOTTOM: gray 1px solid; HEIGHT: 18px">
								<TABLE id="Tbl" cellSpacing="0" cellPadding="5" border="0" runat="server">
									<TR>
										<TD>
											<cc2:TableEditor id="TEUSERWHAREA" runat="server" DisableSwitches="reidn" DefaultDT="DTUSERWHAREA" InsertDT="DTUSERWHAREAInsert" AllowDeleteInViewMode="true"
												DefaultMode="Grid" DESIGNTIMEDRAGDROP="60" AutoSelectGridItem="Never" AutoSelectMode="View"></cc2:TableEditor>
											<cc2:DataConnector id="DC1" runat="server" DESIGNTIMEDRAGDROP="38" TargetID="TEUSERWHAREA" MasterFields="WAREHOUSEAREACODE"
												ChildFields="WHAREA" MasterObjID="TEWHAREA"></cc2:DataConnector></TD>
										
										<TD>
											<cc2:TableEditor id="TEAssignUsers" runat="server" DisableSwitches="nreid" 
											    DefaultDT="DTUSERWHAREAUnassign" AllowDeleteInViewMode="true" ConnectionName="Made4NetSchema"
												DefaultMode="Grid" DESIGNTIMEDRAGDROP="60" AutoSelectGridItem="Never"
												AutoSelectMode="View" SQL="select distinct userid,'' as wharea from userprofile">
											</cc2:TableEditor>
										</TD>
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
