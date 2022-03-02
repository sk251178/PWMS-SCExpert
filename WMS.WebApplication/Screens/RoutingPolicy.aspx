<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="RoutingPolicy.aspx.vb" Inherits="WMS.WebApp.RoutingPolicy" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>RoutingPolicy</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout">
		<FORM id="Form1" method="post" runat="server">
			<cc3:screen id="Screen1" runat="server" ScreenID="rtp"></cc3:screen>
			<P><cc2:tableeditor id="TERP" runat="server" DefaultDT="RoutingPolicyHeader" ManualMode="False" DefaultMode="Search"
					GridPageSize="5" DESIGNTIMEDRAGDROP="14" EditVS="0" AfterInsertMode="Grid" AfterUpdateMode="Grid"
					AllowDeleteInViewMode="True" DisableSwitches="mn" ForbidRequiredFieldsInSearchMode="True" AutoSelectGridItem="Never"
					AutoSelectMode="View"></cc2:tableeditor></P>
			<P>&nbsp;</P>
			<P><cc2:datatabcontrol id="DTC" runat="server" SyncEditMode="True" ParentID="TERP">
					<TABLE id="Table1" cellSpacing="0" cellPadding="0" border="0" runat="server">
						<TR>
							<TD>
								<cc2:tabstrip id="TS" runat="server" AutoPostBack="True" TabDefaultStyle="background-color:#DDDDDD;border:solid 1px gray;padding-left:5px;padding-right:5px;font-size:13px;font-weight:bold;padding-top:2px;padding-bottom:2px;"
									TabHoverStyle="background-color:#DDDDDD;" TabSelectedStyle="border:solid 1px gray;border-bottom:none;"
									SepDefaultStyle="border-bottom:solid 1px gray;" TargetTableID="Tbl">
									<iewc:Tab Text="Detail"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="Distance Parameters"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="Time Parameters"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="Vehicle Allocation"></iewc:Tab>
									<iewc:TabSeparator DefaultStyle="width:100%;"></iewc:TabSeparator>
								</cc2:tabstrip></TD>
						</TR>
						<TR>
							<TD style="BORDER-RIGHT: gray 1px solid; BORDER-LEFT: gray 1px solid; BORDER-BOTTOM: gray 1px solid; HEIGHT: 18px">
								<TABLE id="Tbl" cellSpacing="0" cellPadding="5" border="0" runat="server">
									<TR>
										<TD>
											<cc2:TableEditor id="TERPDETAILGeneral" runat="server" AutoSelectMode="View" AutoSelectGridItem="Never"
												ForbidRequiredFieldsInSearchMode="True" DisableSwitches="mn" AllowDeleteInViewMode="False" DESIGNTIMEDRAGDROP="60"
												DefaultMode="Grid" DefaultDT="RPPolicyDetailGeneral"></cc2:TableEditor>
											<cc2:DataConnector id="DC1" runat="server" DESIGNTIMEDRAGDROP="38" MasterObjID="TERP" ChildFields="STRATEGYID"
												MasterFields="STRATEGYID" TargetID="TERPDETAILGeneral"></cc2:DataConnector></TD>
										<TD>
											<cc2:TableEditor id="TERPDETAILDist" runat="server" AutoSelectMode="View" AutoSelectGridItem="Never"
												ForbidRequiredFieldsInSearchMode="True" DisableSwitches="mn" AllowDeleteInViewMode="False" DESIGNTIMEDRAGDROP="60"
												DefaultMode="Grid" DefaultDT="RPPolicyDetailDist"></cc2:TableEditor>
											<cc2:DataConnector id="Dataconnector1" runat="server" DESIGNTIMEDRAGDROP="38" MasterObjID="TERP" ChildFields="STRATEGYID"
												MasterFields="STRATEGYID" TargetID="TERPDETAILDist"></cc2:DataConnector></TD>
										<TD>
											<cc2:TableEditor id="TERPDETAILTime" runat="server" AutoSelectMode="View" AutoSelectGridItem="Never"
												ForbidRequiredFieldsInSearchMode="True" DisableSwitches="mn" AllowDeleteInViewMode="False" DESIGNTIMEDRAGDROP="60"
												DefaultMode="Grid" DefaultDT="RPPolicyDetailTime"></cc2:TableEditor>
											<cc2:DataConnector id="Dataconnector2" runat="server" DESIGNTIMEDRAGDROP="38" MasterObjID="TERP" ChildFields="STRATEGYID"
												MasterFields="STRATEGYID" TargetID="TERPDETAILTime"></cc2:DataConnector></TD>
										<TD>
											<cc2:TableEditor id="TEVehicleAlloc" runat="server" AutoSelectMode="View" AutoSelectGridItem="Never"
												ForbidRequiredFieldsInSearchMode="True" DisableSwitches="mn" AllowDeleteInViewMode="False" DESIGNTIMEDRAGDROP="60"
												DefaultMode="Grid" DefaultDT="RPVehicleAlloc"></cc2:TableEditor>
											<cc2:DataConnector id="Dataconnector3" runat="server" DESIGNTIMEDRAGDROP="38" MasterObjID="TERP" ChildFields="STRATEGYID"
												MasterFields="STRATEGYID" TargetID="TEVehicleAlloc"></cc2:DataConnector></TD>
									</TR>
								</TABLE>
							</TD>
						</TR>
					</TABLE>
				</cc2:datatabcontrol></P>
		</FORM>
	</body>
</HTML>
