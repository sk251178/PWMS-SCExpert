<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="LibImageRotator.aspx.vb" Inherits="WMS.WebApp.LibImageRotator" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>-</title>
		<meta content="False" name="vs_showGrid">
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<META HTTP-EQUIV="Expires" CONTENT="Sat, 1 Jan 2000 12:00:00 GMT">
		<!-- #include file="~/include/head.html" -->
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout"
		style="BACKGROUND-COLOR:#eeeeee">
		<!-- #include file="~/include/Header.html" -->
		<table cellpadding="0" cellspacing="0" id="outerTable" width="100%" height="100%">
			<tr>
				<td valign="middle" align="center">
					<table cellpadding="0" cellspacing="0" id="innerTable">
						<tr>
							<td>
								<FORM id="Form1" method="post" runat="server">
                                    <telerik:RadScriptManager ID="RadScriptManager2" runat="server">     </telerik:RadScriptManager> 
									<cc3:Screen id="Screen" runat="server" HideBanner="True" HideMenu="True" HideTitle="True" NoLoginRequired="True"></cc3:Screen>
									<cc2:tableeditor id="TE" runat="server" ManualMode="False" DisableSwitches="svidtrmne" DefaultMode="View"
										GridPageSize="50" DESIGNTIMEDRAGDROP="14" AfterUpdateMode="View" AfterInsertMode="View" ForbidRequiredFieldsInSearchMode="True"
										ObjectID="TE" DefaultDT="SysLibImageRotator" ConnectionID="0" HideActionBar="True"></cc2:tableeditor>
								</FORM>
							</td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
		<script language="javascript">
			var t = document.getElementById("innerTable");
			window.resizeTo(t.offsetWidth + 15, t.offsetHeight + 15);
			
			//repaint the object
			t.style.display='none';
			t.style.display='';
		</script>
	</body>
</HTML>
