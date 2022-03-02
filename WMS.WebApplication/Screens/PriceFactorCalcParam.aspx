<%@ Page Language="vb" AutoEventWireup="false" Codebehind="PriceFactorCalcParam.aspx.vb" Inherits="WMS.WebApp.PriceFactorCalcParam"%>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
  <head>
    <title>PriceFactorCalcParam</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name=vs_defaultClientScript content="JavaScript">
    <meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5">
  </head>
  <body MS_POSITIONING="FlowLayout">
		<!-- #include file="~/include/Header.html" -->

	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout">
		<!-- #include file="~/include/Header.html" -->
		<form id="Form1" method="post" runat="server">
            <telerik:RadScriptManager ID="RadScriptManager2" runat="server">     </telerik:RadScriptManager> 
			<p><cc3:screen id="Screen1" runat="server" ScreenID="BP"></cc3:screen></p>
			<p><cc2:tableeditor id="TEBILLINGPRICEFACTORINDEX" runat="server" GridDT="DTBILLINGPRICEFACTORINDEX" DisableSwitches="nr" DESIGNTIMEDRAGDROP="14"
					GridPageSize="10" DefaultMode="Grid" DefaultVS="2" ManualMode="False" DefaultDT="DTBILLINGPRICEFACTORINDEX" SearchDT="DTBILLINGPRICEFACTORINDEX"
					AllowDeleteInViewMode="True" AutoSelectGridItem="Never" AutoSelectMode="View" ></cc2:tableeditor>
			</p>
			
			
						<P><cc2:datatabcontrol id="DTC" runat="server" SyncEditMode="True" ParentID="TEBILLINGPRICEFACTORINDEX">
					<TABLE id="Table1" cellSpacing="0" cellPadding="0" border="0" runat="server">
						<TR>
							<TD>
								<cc2:tabstrip id="TS" runat="server" TargetTableID="Tbl" SepDefaultStyle="border-bottom:solid 1px gray;"
									TabSelectedStyle="border:solid 1px gray;border-bottom:none;background:white;" TabHoverStyle="background-color:#DDDDDD;"
									TabDefaultStyle="background-color:#DDDDDD;border:solid 1px gray;padding-left:5px;padding-right:5px;font-size:13px;font-weight:bold;padding-top:2px;padding-bottom:2px;"
									AutoPostBack="True">
									<iewc:Tab Text="Fixed Params"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="Daily Params"></iewc:Tab>
									<iewc:TabSeparator DefaultStyle="width:100%;"></iewc:TabSeparator>
								</cc2:tabstrip></TD>
						</TR>
						<TR>
							<TD style="BORDER-RIGHT: gray 1px solid; BORDER-LEFT: gray 1px solid; BORDER-BOTTOM: gray 1px solid; HEIGHT: 18px">
								<TABLE id="Tbl" cellSpacing="0" cellPadding="5" border="0" runat="server">
									<TR>
										<TD>
											<cc2:TableEditor id="TEBILLINGFIXEDPRICEFACTORPARAMS" runat="server" EditDT="DTBILLINGFIXEDPRICEFACTORPARAMS" InsertDT="DTBILLINGFIXEDPRICEFACTORPARAMS" 
												  DisableSwitches="mn" ForbidRequiredFieldsInSearchMode="True" DefaultMode="Grid" DefaultDT="DTBILLINGFIXEDPRICEFACTORPARAMS"
												AllowDeleteInViewMode="False" DESIGNTIMEDRAGDROP="60" AutoSelectGridItem="Never" GridDT="DTBILLINGFIXEDPRICEFACTORPARAMS" AutoSelectMode="View"
												ObjectID="TEBILLINGFIXEDPRICEFACTORPARAMS"></cc2:TableEditor> 
												</TD>
										<TD>
												
										<TABLE id="Tbl1" cellSpacing="0" cellPadding="5" border="0" runat="server">
										<tr>
										<td>
												<cc2:TableEditor id="TEBILLINGDAILYPRICEFACTORPARAMS" runat="server" DisableSwitches="rn" ForbidRequiredFieldsInSearchMode="True"
													 GridPageSize="10" DefaultMode="Grid" DefaultDT="DTBILLINGDAILYPRICEFACTORPARAMS" GridDT="DTBILLINGDAILYPRICEFACTORPARAMS"
													AutoSelectMode="View" AutoSelectGridItem="Never"></cc2:TableEditor>
										
										</td>
										</tr>
										<tr>
										<td>
											<cc2:TableEditor id="TEBILLINGDAILYPRICEFACTORPARAMSVALUES" runat="server" EditDT="DTBILLINGDAILYPRICEFACTORPARAMSVALUES" InsertDT="DTBILLINGDAILYPRICEFACTORPARAMSVALUES" 
												  DisableSwitches="mn" ForbidRequiredFieldsInSearchMode="True" DefaultMode="Grid" DefaultDT="DTBILLINGDAILYPRICEFACTORPARAMSVALUES"
												AllowDeleteInViewMode="False" DESIGNTIMEDRAGDROP="60" AutoSelectGridItem="Never" GridDT="DTBILLINGDAILYPRICEFACTORPARAMSVALUES" AutoSelectMode="View"
												ObjectID="TEBILLINGDAILYPRICEFACTORPARAMSVALUES"></cc2:TableEditor> 
											<cc2:DataConnector id="DC2" runat="server" DESIGNTIMEDRAGDROP="38" TargetID="TEBILLINGDAILYPRICEFACTORPARAMSVALUES" MasterFields="PARAMID"
												ChildFields="PARAMID" MasterObjID="TEBILLINGDAILYPRICEFACTORPARAMS"></cc2:DataConnector>
										</td>
										</tr> 
										</table> 
										

													
										</TD>
									</TR>



								</TABLE>
							</TD>
						</TR>
					</TABLE>
				</cc2:datatabcontrol></P>

			
		</form>
	</body>

  </body>
</html>
