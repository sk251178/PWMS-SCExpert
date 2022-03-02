<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Territory.aspx.vb" Inherits="WMS.WebApp.Territory" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Routes</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</HEAD> 
	<body style="OVERFLOW-X:hidden" bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0"
		MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<cc3:screen id="Screen1" runat="server" ScreenID="tr"></cc3:screen>
			<TABLE border="0">
				<tr>
					<TD vAlign="top" borderColor="#000000"><cc2:map id="MapTerritory" runat="server"></cc2:map>
					</TD>
					<td vAlign="top">
						<TABLE border="0">
							<tr>
								<td>
									<cc2:tableeditor id="TETerritoryList" runat="server" DisableSwitches="rmneiv" DESIGNTIMEDRAGDROP="60"
										DefaultMode="Grid" DefaultDT="DTTerritory" ForbidRequiredFieldsInSearchMode="True" MultilineAllSelected="True"></cc2:tableeditor>
								</td>
							</tr>
							<tr>
								<td>
									<cc2:datatabcontrol id="DTC" runat="server" SyncEditMode="True" ParentID="TETerritoryList">
										<TABLE id="Table1" cellSpacing="0" cellPadding="0" border="0" runat="server">
											<TR>
												<TD>
													<cc2:tabstrip id="TS" runat="server" TargetTableID="Tbl" SepDefaultStyle="border-bottom:solid 1px gray;"
														TabSelectedStyle="border:solid 1px gray;border-bottom:none;" TabHoverStyle="background-color:#DDDDDD;"
														TabDefaultStyle="background-color:#DDDDDD;border:solid 1px gray;padding-left:5px;padding-right:5px;font-size:13px;font-weight:bold;padding-top:2px;padding-bottom:2px;"
														AutoPostBack="True">
														<iewc:Tab Text="Detail"></iewc:Tab>
														<iewc:TabSeparator DefaultStyle="width:100%;"></iewc:TabSeparator>
													</cc2:tabstrip></TD>
											</TR>
											<TR>
												<TD style="BORDER-RIGHT: gray 1px solid; BORDER-LEFT: gray 1px solid; BORDER-BOTTOM: gray 1px solid; HEIGHT: 18px">
													<TABLE id="Tbl" cellSpacing="0" cellPadding="5" border="0" runat="server">
														<TR>
															<TD>
																<cc2:TableEditor id="TETerritoryDetail" runat="server" ForbidRequiredFieldsInSearchMode="True" DefaultDT="TerritoryDetail"
																	DefaultMode="Grid" DESIGNTIMEDRAGDROP="60" DisableSwitches="mn" AllowDeleteInViewMode="False"
																	AutoSelectGridItem="Never" AutoSelectMode="View"></cc2:TableEditor>
																<cc2:DataConnector id="DC1" runat="server" DESIGNTIMEDRAGDROP="38" TargetID="TETerritoryDetail" MasterFields="TERRITORYID"
																	ChildFields="TERRITORYID" MasterObjID="TETerritoryList"></cc2:DataConnector></TD>
														</TR>
													</TABLE>
												</TD>
											</TR>
										</TABLE>
									</cc2:datatabcontrol>
								</td>
							</tr>
						</TABLE>
					</td>
				</tr>
			</TABLE>
		</form>
	</body>
</HTML>
