<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="BillingPriceFactorIndexParams.aspx.vb" Inherits="WMS.WebApp.BillingPriceFactorIndexParams" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Price factor index params</title>
		<meta content="False" name="vs_showGrid">
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout">
		<FORM id="Form1" method="post" runat="server">
            <telerik:RadScriptManager ID="RadScriptManager2" runat="server">     </telerik:RadScriptManager> 
			<cc3:screen id="Screen1" runat="server" ScreenID="pfp"></cc3:screen>
            <P><cc2:datatabcontrol id="DTC" runat="server" SyncEditMode="True">
					<TABLE id="Table1" cellSpacing="0" cellPadding="0" border="0" runat="server">
						<TR>
							<TD>
								<cc2:tabstrip id="TS" runat="server" AutoPostBack="True" TabDefaultStyle="background-color:#DDDDDD;border:solid 1px gray;padding-left:5px;padding-right:5px;font-size:13px;font-weight:bold;padding-top:2px;padding-bottom:2px;"
									TabHoverStyle="background-color:#DDDDDD;" TabSelectedStyle="border:solid 1px gray;border-bottom:none;background:white;"
									SepDefaultStyle="border-bottom:solid 1px gray;" TargetTableID="Tbl">
									<iewc:Tab Text="Price Factor Daily Parameters"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="Price Factor Fixed Parameters"></iewc:Tab>
									<iewc:TabSeparator DefaultStyle="width:100%;"></iewc:TabSeparator>
								</cc2:tabstrip></TD>
						</TR>
						<TR>
							<TD style="BORDER-RIGHT: gray 1px solid; BORDER-LEFT: gray 1px solid; BORDER-BOTTOM: gray 1px solid; HEIGHT: 18px">
								<TABLE id="Tbl" cellSpacing="0" cellPadding="5" border="0" runat="server">
									<TR>
										<TD>
											<P><cc2:tableeditor id="TEDailyParamHeader" runat="server" DisableSwitches="mn" DESIGNTIMEDRAGDROP="14"
					                                GridPageSize="5" DefaultMode="Search" DefaultDT="DTPriceFactorDailyParam" 
					                                EditDT="DTPriceFactorDailyParamEdit" ForbidRequiredFieldsInSearchMode="True"
					                                AutoSelectGridItem="Never" AutoSelectMode="View"></cc2:tableeditor></P>
			                                <P>&nbsp;</P>
			                                <P><cc2:datatabcontrol id="Datatabcontrol1" runat="server" SyncEditMode="True" ParentID="TEDailyParamHeader">
					                                <TABLE id="Table2" cellSpacing="0" cellPadding="0" border="0" runat="server">
						                                <TR>
							                                <TD>
								                                <cc2:tabstrip id="Tabstrip1" runat="server" TargetTableID="TABLE3" SepDefaultStyle="border-bottom:solid 1px gray;"
									                                TabSelectedStyle="border:solid 1px gray;border-bottom:none;background:white;" TabHoverStyle="background-color:#DDDDDD;"
									                                TabDefaultStyle="background-color:#DDDDDD;border:solid 1px gray;padding-left:5px;padding-right:5px;font-size:13px;font-weight:bold;padding-top:2px;padding-bottom:2px;"
									                                AutoPostBack="True">
									                                <iewc:Tab Text="Parameter Values"></iewc:Tab>
									                                <iewc:TabSeparator DefaultStyle="width:100%;"></iewc:TabSeparator>
								                                </cc2:tabstrip></TD>
						                                </TR>
						                                <TR>
							                                <TD style="BORDER-RIGHT: gray 1px solid; BORDER-LEFT: gray 1px solid; BORDER-BOTTOM: gray 1px solid; HEIGHT: 18px">
								                                <TABLE id="TABLE3" cellSpacing="0" cellPadding="5" border="0" runat="server">
									                                <TR>
										                                <TD>
											                                <cc2:TableEditor id="TEDailyParamValue" runat="server" DefaultDT="DTDailyParamValue" DefaultMode="Grid" 
											                                    GridPageSize="20" DisableSwitches="nmt"></cc2:TableEditor>
											                                <cc2:DataConnector id="DC1" runat="server" DESIGNTIMEDRAGDROP="43" TargetID="TEDailyParamValue" 
												                                MasterFields="paramid" ChildFields="paramid" MasterObjID="TEDailyParamHeader"></cc2:DataConnector></TD>
									                                </TR>
								                                </TABLE>
							                                </TD>
						                                </TR>
					                                </TABLE>
				                                </cc2:datatabcontrol></P>
										</TD>
										<TD>
											<cc2:TableEditor id="TEFixedParams" runat="server" AutoSelectMode="View" AutoSelectGridItem="Never"
												DisableSwitches="tmnr" DefaultMode="Grid" DESIGNTIMEDRAGDROP="14" DefaultDT="DTPriceFactorFixedParams"
												EditDT="DTPriceFactorFixedParamsEdit"
												ForbidRequiredFieldsInSearchMode="True"></cc2:TableEditor>
										</TD>
									</TR>
								</TABLE>
							</TD>
						</TR>
					</TABLE>
				</cc2:datatabcontrol></P>
		</FORM>
	</body>
</HTML>
