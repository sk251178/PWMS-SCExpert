<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ShiftPerformance.aspx.vb" Inherits="WMS.WebApp.ShiftPerformance"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<html>
  <head>
    <title>ShiftPerformance</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name=vs_defaultClientScript content="JavaScript">
    <meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5">
  </head>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
			<cc3:screen id="Screen1" runat="server" ScreenID="sps"></cc3:screen>
			<P><cc2:tableeditor id="TESHIFTPERF" runat="server" GridDT="DTShiftPerformanceSummary" AfterUpdateMode="Grid"
					AfterInsertMode="Grid" SearchDT="DTShiftPerformanceSummary" InsertDT="DTShiftPerformanceSummary"
					DisableSwitches="eidvrnm" ForbidRequiredFieldsInSearchMode="True" DESIGNTIMEDRAGDROP="14" GridPageSize="5"
					DefaultMode="Search" ManualMode="False" DefaultDT="DTShiftPerformanceSummary" EditDT="DTShiftPerformanceSummary"
					ViewDT="DTShiftPerformanceSummary" ObjectID="TESHIFTPERF" AutoSelectGridItem="Never" AutoSelectMode="View"></cc2:tableeditor></P>
			<P>&nbsp;</P>
			<P><cc2:datatabcontrol id="DTC" runat="server" SyncEditMode="True" ParentID="TESHIFTPERF">
					<TABLE id="Table1" cellSpacing="0" cellPadding="0" border="0" runat="server">
						<TR>
							<TD>
								<cc2:tabstrip id="TS" runat="server" TargetTableID="Tbl" SepDefaultStyle="border-bottom:solid 1px gray;"
									TabSelectedStyle="border:solid 1px gray;border-bottom:none;background:white;" TabHoverStyle="background-color:#DDDDDD;"
									TabDefaultStyle="background-color:#DDDDDD;border:solid 1px gray;padding-left:5px;padding-right:5px;font-size:13px;font-weight:bold;padding-top:2px;padding-bottom:2px;"
									AutoPostBack="True">
									<iewc:Tab Text="User Performance"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="Performance by Task"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="Shift Time Block"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
								</cc2:tabstrip></TD>
						</TR>
						<TR>
							<TD style="BORDER-RIGHT: gray 1px solid; BORDER-LEFT: gray 1px solid; BORDER-BOTTOM: gray 1px solid; HEIGHT: 18px">
								<TABLE id="Tbl" cellSpacing="0" cellPadding="5" border="0" runat="server">
									<TR>
										<TD>
											<cc2:TableEditor id="TEUSERPERF" runat="server" DisableSwitches="eidvrnm" SearchDT="DTUserPerformanceSummary"
												ForbidRequiredFieldsInSearchMode="True" GridPageSize="10" DefaultMode="Grid" DefaultDT="DTUserPerformanceSummary"
												GridDT="DTUserPerformanceSummary" AutoSelectGridItem="Never" AutoSelectMode="View"></cc2:TableEditor>
											<cc2:DataConnector id="DC1" runat="server" DESIGNTIMEDRAGDROP="43" TargetID="TEUSERPERF" MasterFields="SHIFTID"
												ChildFields="SHIFTID" MasterObjID="TESHIFTPERF"></cc2:DataConnector></TD>
										<TD>
											<cc2:TableEditor id="TEPERFBYTASK" runat="server" DisableSwitches="eidvrnm" SearchDT="DTTaskPerformanceSummary"
												ForbidRequiredFieldsInSearchMode="True" GridPageSize="10" DefaultMode="Grid" DefaultDT="DTTaskPerformanceSummary"
												GridDT="DTTaskPerformanceSummary" AutoSelectGridItem="Never" AutoSelectMode="View"></cc2:TableEditor>
											<cc2:DataConnector id="DC2" runat="server" DESIGNTIMEDRAGDROP="43" TargetID="TEPERFBYTASK" MasterFields="SHIFTID"
												ChildFields="SHIFTID" MasterObjID="TESHIFTPERF"></cc2:DataConnector></TD>
										<TD>
											<cc2:TableEditor id="TESHIFTBLOCK" runat="server" DisableSwitches="eidvrnm" SearchDT="DTSHIFTTIMEBLOCKS"
												ForbidRequiredFieldsInSearchMode="True" GridPageSize="10" DefaultMode="Grid" DefaultDT="DTSHIFTTIMEBLOCKS"
												GridDT="DTSHIFTTIMEBLOCKS" AutoSelectGridItem="Never" AutoSelectMode="View"></cc2:TableEditor>
											<cc2:DataConnector id="DC3" runat="server" DESIGNTIMEDRAGDROP="43" TargetID="TESHIFTBLOCK" MasterFields="SHIFTCODE"
												ChildFields="SHIFTCODE" MasterObjID="TESHIFTPERF"></cc2:DataConnector></TD>
									</TR>
								</TABLE>
							</TD>
						</TR>
					</TABLE>
				</cc2:datatabcontrol></P>
		</form>
	</body>
</html>
