<%@ Register TagPrefix="cc1" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc2" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="UserInfo.aspx.vb" Inherits="WMS.WebApp.UserInfo"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>UserInfo</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<!-- #include file="~/include/head.html" -->
	</HEAD>
	<script language="javascript">
	//setTimeout("this.window.focus()",1000);
	//window.
	//window.opener.location=window.opener.location;
	//window.opener.blur();
	//alert(window.opener.status);
	//this.window.focus();
	//window.
	//this.window.focus();
	//alert("Focused");
	function checkBlur()
	{
	try
	{
	  if (doBlur==1) {
	    
	    this.window.focus();
	    //doBlur=0;
	    //alert("OnBlur");
	  }
	} catch (err) {}
	}
	</script>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout" onBlur="checkBlur();return true;">
		<!-- #include file="~/include/Header.html" -->
		<form id="Form1" method="post" runat="server">
            <telerik:RadScriptManager ID="RadScriptManager2" runat="server">     </telerik:RadScriptManager> 
			<cc2:screen id="Screen1" title="User Detail" runat="server" Hidden="False" NoLoginRequired="True"
				HideMenu="true" HideBanner="true"></cc2:screen>
			<div id="noUserdiv" style="MARGIN-TOP: 10px" runat="server">
				<table align="center">
					<tr>
						<td><asp:label id="noUserlbl" runat="server" ForeColor="Red"></asp:label></td>
					</tr>
				</table>
			</div>
			<div id="userDetail" style="MARGIN-TOP: 10px" runat="server">
			<P><cc1:datatabcontrol id="DTC" runat="server" runat="server" ParentID="TEMasterOutboundOrders" SyncEditMode="True"><TABLE id="Table1" cellSpacing=0 cellPadding=0 border=0 runat="server"><TBODY><TR><TD><cc1:tabstrip id="TS" runat="server" TargetTableID="Tbl" SepDefaultStyle="border-bottom:solid 1px gray;" TabSelectedStyle="border:solid 1px gray;border-bottom:none;background:white;" TabHoverStyle="background-color:#DDDDDD;" TabDefaultStyle="background-color:#DDDDDD;border:solid 1px gray;padding-left:5px;padding-right:5px;font-size:13px;font-weight:bold;padding-top:2px;padding-bottom:2px;" AutoPostBack="True">
									<iewc:Tab Text="User Detail"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="Password"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="User Parameters"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
								</cc1:tabstrip></TD></TR><TR><TD style="BORDER-RIGHT: gray 1px solid; BORDER-LEFT: gray 1px solid; BORDER-BOTTOM: gray 1px solid; HEIGHT: 17px"><TABLE id="Tbl" cellSpacing=0 cellPadding=5 border=0 runat="server"><TBODY><TR><TD><cc1:TableEditor id="TEUserInfo" runat="server" ConnectionName="Made4NetSchema" GridPageSize="10" DefaultMode="View" DisableSwitches="tmnsi" DefaultDT="UserInfoProfile" AfterUpdateMode="View" AfterInsertMode="View"></cc1:TableEditor> <!--<div style="MARGIN: 10px">
						<table>
							<TBODY>
								<tr>
									<td><cc1:label id="UserID" runat="server" value="User ID:"></cc1:label></td>
									<td><cc1:textbox id="txtUserID" runat="server" Readonly="true"></cc1:textbox></td>
									<td><cc1:button id="btnSave" runat="server" Text="Save" Width="89px"></cc1:button></td>
								</tr>
								<tr>
									<td><cc1:label id="FirstName" runat="server" value="First Name:"></cc1:label></td>
									<td><cc1:textbox id="txtFirstName" runat="server"></cc1:textbox></td>
								</tr>
								<tr>
									<td><cc1:label id="LastName" runat="server" value="Last Name:"></cc1:label></td>
									<td><cc1:textbox id="txtLastName" runat="server"></cc1:textbox></td>
								</tr>
								<tr>
									<td><cc1:label id="FullName" runat="server" value="Full Name:"></cc1:label></td>
									<td><cc1:textbox id="txtFullName" runat="server"></cc1:textbox></td>
								</tr>
								<tr>
									<td style="HEIGHT: 22px"><cc1:label id="DefaultLanguage" runat="server" value="Default Language:"></cc1:label></td>
									<td style="HEIGHT: 22px"><cc1:dropdownlist id="ddLanguage" runat="server" AutoPostBack="True" width="100%"></cc1:dropdownlist></td>
								</tr>
								<tr>
									<td><cc1:label id="DefaultSkin" runat="server" value="Default Skin:"></cc1:label></td>
									<td><cc1:dropdownlist id="ddSkin" runat="server" width="100%"></cc1:dropdownlist></td>
								</tr>
								<tr>
									<td><cc1:label id="DefaultScreen" runat="server" value="Default Screen (ID):"></cc1:label></td>
									<td><cc1:textbox id="txtScreenid" runat="server"></cc1:textbox></td>
								</tr>
							</TBODY>
						</table>
					</div--></TD><TD><DIV style="MARGIN: 10px"><TABLE id="Table2" cellSpacing=0 cellPadding=3 border=0><TBODY><TR><TD noWrap colSpan=4><cc1:label id="Label2" runat="server" Font-Bold="True" Value="Your password has expired. Please select a new password:">Change Password:</cc1:label></TD></TR><TR><TD noWrap><cc1:label id="Label4" runat="server" Value="New Password:" DESIGNTIMEDRAGDROP="145">New Password:</cc1:label></TD><TD><cc1:textboxvalidated id="tbNewPass1" runat="server" CheckDataType="True" Required="False" TextMode="Password"></cc1:textboxvalidated></TD><TD>&nbsp;&nbsp;&nbsp;</TD><TD>&nbsp;</TD></TR><TR><TD><cc1:label id="Label3" runat="server" Value="Confirm:" DESIGNTIMEDRAGDROP="717">Confirm:</cc1:label></TD><TD><cc1:textboxvalidated id="tbNewPass2" runat="server" DESIGNTIMEDRAGDROP="940" CheckDataType="True" Required="False" TextMode="Password"></cc1:textboxvalidated></TD><TD>&nbsp;&nbsp;&nbsp; <cc1:button id="Button1" runat="server" Width="80px" Text="OK"></cc1:button></TD><TD></TD></TR><TR><TD></TD><TD colSpan=3><asp:label id="lblNewPassMsg" runat="server" ForeColor="Red"></asp:label></TD></TR></TBODY></TABLE></DIV></TD><TD><DIV style="MARGIN: 10px"><TABLE><TBODY><TR><TD><cc1:label id="DefaultConsignee" runat="server" value="Default Consignee:"></cc1:label></TD><TD><cc1:textbox id="txtConsignee" runat="server"></cc1:textbox></TD></TR><TR><TD><cc1:label id="lblWHArea" runat="server" value="Warehouse Area:"></cc1:label></TD><TD><cc1:dropdownlist id="ddUserWHArea" runat="server" width="100%"></cc1:dropdownlist></TD><TD><cc1:button id="btnWHAreaChange" runat="server" Width="89px" Text="Change"></cc1:button></TD></TR><TR><TD><cc1:button id="btnUserParamSave" runat="server" Width="89px" Text="Save"></cc1:button></TD></TR></TBODY></TABLE></DIV></TD></TR></TBODY></TABLE></TD></TR></TBODY></TABLE></cc1:datatabcontrol></P>
			    </div>
		</form>
		<DIV></DIV>
		<DIV></DIV>
		<DIV></DIV>
		
	</body>
</HTML>
