<%@ Register TagPrefix="cc1" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc2" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="LoginWarehouse.aspx.vb" Inherits="WMS.WebApp.LoginWarehouse"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Login Warehouse</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<style>BODY { BACKGROUND-COLOR: #CCCCCC! important }</style>
		<!-- #include file="~/include/head.html" -->
		<script>
			$(document).ready(function() {
				powerplus.renderLogin();
			});
		</script>
	</HEAD> 
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0">
		<form id="Form1" method="post" runat="server">
			<telerik:RadScriptManager  ID="RadScriptManager2" runat="server"></telerik:RadScriptManager> 
			<cc2:screen id="Screen1" runat="server" Hidden="true" NoLoginRequired="true" HideMenu="true" HideBanner="true"></cc2:screen>	
			<div class="app-login-screen">
				<div class="app-login-container">	
					<div class="app-logo">
						<img src="../resources/images/logo.png">
						<div class="app-logo-slogan">Power+ WMS</div>
					</div>
					<app-login-form id="app-login-form">
						<div class="app-login-form-container">
							<div class="app-login-form form-fields">
								<cc1:panel id="pnlImages" runat="server" DESIGNTIMEDRAGDROP="15" width="80"></cc1:panel>
								<cc1:label id="lblWareHouse" runat="server" value="Warehouse" class="LoginWhite"></cc1:label>
								<cc1:listbox id="lstWareHouse" runat="server"></cc1:listbox>
								<asp:imagebutton id="btnGo" runat="server"></asp:imagebutton>
							</div>
						</div>
					</app-login-form>
				</div>
			</div>
			<!-- <cc1:panel id="pnlAppInfo" runat="server" ></cc1:panel>-->
		</form>
	</body>
</HTML>
