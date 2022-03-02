<!DOCTYPE HTML>
<%@ Register TagPrefix="cc1" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc2" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Login.aspx.vb" Inherits="WMS.WebApp.Login" %>
<HTML>
	<HEAD>
		<title>Login</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		
		<!-- #include file="~/include/head.html" -->
		<script>
			$(document).ready(function() {
				powerplus.renderLogin();
			});
		</script>
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<telerik:RadScriptManager  ID="RadScriptManager2" runat="server"></telerik:RadScriptManager> 
			<div class="app-login-screen">
				<div class="app-login-container">	
					<div class="app-logo">
						<img src="../resources/images/logo.png">
						<div class="app-logo-slogan">Power+ WMS</div>
					</div>
					<app-login-form id="app-login-form">
                         <div style="margin-left:-10px;">
                        <cc2:screen  id="Screen1" runat="server" Hidden="False" NoLoginRequired="true" HideMenu="true" 	HideBanner="true"></cc2:screen>	
                    	<div class="app-login-form-container">

							<div class="app-login-form form-fields">
								<cc1:panel id="pnlImages" runat="server" DESIGNTIMEDRAGDROP="15" width="80"></cc1:panel>
								<cc1:LoginBox id="LoginBox1" runat="server" LabelClass="LoginWhite"></cc1:LoginBox>
							</div>
						</div>
                       </div>
					</app-login-form>
				</div>
			</div>
		</form>
	</body>
</HTML>
