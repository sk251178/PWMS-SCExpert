<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ProblemLocation.aspx.vb" Inherits="WMS.WebApp.ProblemLocation" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Location Problems</title>
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
			<cc3:screen id="Screen1" runat="server" ScreenID="loc"></cc3:screen><br>
			<asp:panel id="pnlAdj" Runat="server" Visible="False">
				<TABLE border="0">
					<TR>
						<TD>
							<cc2:fieldLabel id="lblProblemReasonCode" runat="server" text="Reason Code"></cc2:fieldLabel></TD>
						<TD>
							<cc2:dropdownlist id="ddReasonCode" runat="server" TableName="LOCATIONPROBLEMRC" TextField="PROBLEMRCDESC"
								ValueField="PROBLEMRC" AutoPostBack="False"></cc2:dropdownlist></TD>
					</TR>
				</TABLE> 
			</asp:panel>
			<cc2:tableeditor id="TELocationProblems" runat="server" DisableSwitches="eidvn" GridPageSize="5"
				    DefaultMode="Search" DefaultDT="DTLocationProblemGrid" SearchDT="DTLocationProblemSearch"
					ManualMode="False" AutoSelectGridItem="Never" AutoSelectMode="View"
				    ForbidRequiredFieldsInSearchMode="True"></cc2:tableeditor>
		</form>
	</body>
</HTML>