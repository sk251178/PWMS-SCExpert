<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ReportSetup.aspx.vb" Inherits="WMS.WebApp.ReportSetup"%>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>ReportSetup</title>
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
			<cc3:screen id="Screen1" runat="server" NoLoginRequired="True"></cc3:screen>
			<P>&nbsp;</P>
			<TABLE id="Table1" cellSpacing="0" cellPadding="0" border="0" runat="server">
				<TR>
					<TD>
						<cc2:tabstrip id="TS" runat="server" TargetTableID="Tbl" SepDefaultStyle="border-bottom:solid 1px gray;"
							TabSelectedStyle="border:solid 1px gray;border-bottom:none;background:white;" TabHoverStyle="background-color:#DDDDDD;"
							TabDefaultStyle="background-color:#DDDDDD;border:solid 1px gray;padding-left:5px;padding-right:5px;font-size:13px;font-weight:bold;padding-top:2px;padding-bottom:2px;"
							AutoPostBack="True">
							<iewc:Tab Text="Report Manager"></iewc:Tab>
							<iewc:TabSeparator DefaultStyle="border-bottom:soild 1 gray;"></iewc:TabSeparator>
							<iewc:Tab Text="Report Generator Manager"></iewc:Tab>
							<iewc:TabSeparator DefaultStyle="border-bottom:soild 1 gray;"></iewc:TabSeparator>
							<iewc:Tab Text="Report Data Provider"></iewc:Tab>
							<iewc:TabSeparator DefaultStyle="border-bottom:soild 1 gray;"></iewc:TabSeparator>
							<iewc:Tab Text="SQL Result"></iewc:Tab>
							<iewc:TabSeparator DefaultStyle="width:100%;"></iewc:TabSeparator>
						</cc2:tabstrip></TD>
				</TR>
				<TR>
					<TD style="BORDER-RIGHT: gray 1px solid; BORDER-LEFT: gray 1px solid; BORDER-BOTTOM: gray 1px solid; HEIGHT: 18px">
						<TABLE id="Tbl" cellSpacing="0" cellPadding="5" border="0" runat="server">
							<TR>
								<TD>
									<cc2:tableeditor id="TEReport" runat="server" AfterUpdateMode="Grid" AfterInsertMode="Grid" DisableSwitches="nv"
										ForbidRequiredFieldsInSearchMode="True" DESIGNTIMEDRAGDROP="14" GridPageSize="5" DefaultMode="Grid"
										ManualMode="False" DefaultDT="SysReports" ></cc2:tableeditor>
									<P><cc2:datatabcontrol id="DTC" runat="server" ParentID="TEReport" SyncEditMode="True">
											<TABLE id="tblTabStrip2" cellSpacing="0" cellPadding="0" border="0" runat="server">
												<TR>
													<TD>
														<cc2:tabstrip id="TS2" runat="server" AutoPostBack="True" TabDefaultStyle="background-color:#DDDDDD;border:solid 1px gray;padding-left:5px;padding-right:5px;font-size:13px;font-weight:bold;padding-top:2px;padding-bottom:2px;"
															TabHoverStyle="background-color:#DDDDDD;" TabSelectedStyle="border:solid 1px gray;border-bottom:none;background:white;"
															SepDefaultStyle="border-bottom:solid 1px gray;" TargetTableID="tblReportParam">
															<iewc:Tab Text="Report Params"></iewc:Tab>
															<iewc:TabSeparator></iewc:TabSeparator>
															<iewc:Tab Text="Report Data Provider Interface"></iewc:Tab>
															<iewc:TabSeparator DefaultStyle="width:100%;"></iewc:TabSeparator>
														</cc2:tabstrip></TD>
												</TR>
												<TR>
													<TD style="BORDER-RIGHT: gray 1px solid; BORDER-LEFT: gray 1px solid; BORDER-BOTTOM: gray 1px solid; HEIGHT: 18px">
														<TABLE id="tblReportParam" cellSpacing="0" cellPadding="5" border="0" runat="server">
															<TR>
																<TD>
																	<cc2:TableEditor id="TEReportParam" runat="server"  DefaultDT="SysReportParams"
																		DefaultMode="Grid" ForbidRequiredFieldsInSearchMode="True" DisableSwitches="rmnidvt"></cc2:TableEditor>
																	<cc2:DataConnector id="DC1" runat="server" MasterObjID="TEReport" ChildFields="ReportID" MasterFields="ReportID"
																		TargetID="TEReportParam"></cc2:DataConnector></TD>
																<TD>
																	<cc2:TableEditor id="TEDataProviderInterface" runat="server"  DefaultDT="SysDataProviderInterface"
																		DefaultMode="Grid" ForbidRequiredFieldsInSearchMode="True" DisableSwitches="rmnidvt"></cc2:TableEditor>
																	<cc2:DataConnector id="DC3" runat="server" MasterObjID="TEReport" ChildFields="ReportID" MasterFields="ReportID"
																		TargetID="TEDataProviderInterface"></cc2:DataConnector></TD>
															</TR>
														</TABLE>
													</TD>
												</TR>
											</TABLE>
										</cc2:datatabcontrol></P>
								</TD>
								<TD>
									<cc2:tableeditor id="TEReportGenerator" runat="server" AfterUpdateMode="Grid" AfterInsertMode="Grid"
										DisableSwitches="nvt" ForbidRequiredFieldsInSearchMode="True" DESIGNTIMEDRAGDROP="14" GridPageSize="5"
										DefaultMode="Grid" ManualMode="False" DefaultDT="SysReportGenerators" ></cc2:tableeditor>
								</TD>
								<TD>
									<cc2:TableEditor id="TEDataProvider" runat="server"  DefaultDT="SysDataProvider"
										DefaultMode="Grid" ForbidRequiredFieldsInSearchMode="True" DisableSwitches="rmndvt"></cc2:TableEditor>
								</TD>
								<TD>
									<TABLE width="100%">
										<TR>
											<TD>
												<asp:label id="SQLBuilder" runat="server" Font-Bold="True">SQL Builder</asp:label>
											</TD>
										</TR>
										<TR>
											<TD>
												<asp:TextBox id="SQLText" runat="server" Width="544px" TextMode="MultiLine" Height="120px"></asp:TextBox>
											</TD>
										</TR>
										<TR>
											<TD>
												<TABLE>
													<TR>
														<TD><asp:button id="RunSQL" runat="server" Text="Run SQL!"></asp:button></TD>
													</TR>
												</TABLE>
											</TD>
										</TR>
										<TR>
											<TD><asp:label id="SQLResult" runat="server" Font-Bold="True">SQL Result Grid</asp:label></TD>
										</TR>
										<TR>
											<TD><asp:datagrid id="ResDataGrid" runat="server" Width="100%" BorderStyle="Solid" BorderWidth="1px"
													BorderColor="Black">
													<HeaderStyle BorderWidth="1px"></HeaderStyle>
												</asp:datagrid></TD>
										</TR>
									</TABLE>
								</TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>
