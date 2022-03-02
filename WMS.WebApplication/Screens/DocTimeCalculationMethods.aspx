<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DocTimeCalculationMethods.aspx.vb" Inherits="WMS.WebApp.DocTimeCalculationMethods" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>Document Time Calculation Methods</title>
     <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1"/>
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1"/>
    <meta name=vs_defaultClientScript content="JavaScript"/>
    <meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5"/>
    <!-- #include file="~/include/head.html" -->
</head>
<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout">
		<!-- #include file="~/include/Header.html" -->
    <form id="form1" runat="server"  method="post">
        <telerik:RadScriptManager ID="RadScriptManager2" runat="server"></telerik:RadScriptManager> 
    <cc3:screen id="Screen1" runat="server" ScreenID="dtcm"></cc3:screen>
    <cc2:tableeditor id="TEDocTimeCalcMethods" runat="server" GridDT="DTDOCUMENTTIMECALCULATIONMETHODSGrid" DefaultDT="DTDOCUMENTTIMECALCULATIONMETHODSGrid"
					DefaultMode="Grid" SearchDT="DTDOCUMENTTIMECALCULATIONMETHODSSearch" DisableSwitches="mnvr" ForbidRequiredFieldsInSearchMode="True"
					DESIGNTIMEDRAGDROP="14" GridPageSize="5" ManualMode="False" AfterUpdateMode="Grid" AfterInsertMode="Grid" AutoSelectGridItem="Never"
					AutoSelectMode="View"	/>
					<br /><br />
					<cc2:datatabcontrol id="DTC" runat="server" SyncEditMode="True" ParentID="TEDocTimeCalcMethods">
					<TABLE id="Table1" cellSpacing="0" cellPadding="0" border="0" runat="server">
						<TR>
							<TD>
							
								<cc2:tabstrip id="TS" runat="server" TargetTableID="Tbl" SepDefaultStyle="border-bottom:solid 1px gray;"
									TabSelectedStyle="border:solid 1px gray;border-bottom:none;background:white;" TabHoverStyle="background-color:#DDDDDD;"
									TabDefaultStyle="background-color:#DDDDDD;border:solid 1px gray;padding-left:5px;padding-right:5px;font-size:13px;font-weight:bold;padding-top:2px;padding-bottom:2px;"
									AutoPostBack="True">
									<iewc:Tab Text="Calculation Params"></iewc:Tab>
									<iewc:TabSeparator DefaultStyle="width:100%;"></iewc:TabSeparator>
								</cc2:tabstrip>
							</TD>
						</TR>
						<tr>
							<td style="BORDER-RIGHT: gray 1px solid; BORDER-LEFT: gray 1px solid; BORDER-BOTTOM: gray 1px solid; HEIGHT: 18px">
								<table id="Tbl" cellSpacing="0" cellPadding="5" border="0" runat="server">
									<tr>
										<td>
											<cc2:TableEditor id="TEDocTimeCalcParams" runat="server" AutoSelectGridItem="Never" EditDT="DTDOCUMENTTIMECALCULATIONPARAMSEdit" InsertDT="DTDOCUMENTTIMECALCULATIONPARAMSGrid"
												DefaultMode="Grid" DefaultDT="DTDOCUMENTTIMECALCULATIONPARAMSGrid" GridPageSize="10" ForbidRequiredFieldsInSearchMode="True" DisableSwitches="mn" 
												GridDT="DTDOCUMENTTIMECALCULATIONPARAMSGrid" AfterInsertMode="Grid"
												ObjectID="TEDocTimeCalcParams"></cc2:TableEditor> 
											<cc2:DataConnector id="DC1" runat="server" TargetID="TEDocTimeCalcParams" DESIGNTIMEDRAGDROP="43" SyncMode="Reversed" MasterFields="DocType"
												ChildFields="DocType" MasterObjID="TEDocTimeCalcMethods"></cc2:DataConnector>
										</td>
									</tr>
								</table>
							</td>
						</tr>
					</TABLE>
				</cc2:datatabcontrol>
					
					

    </form>
</body>
</html>
