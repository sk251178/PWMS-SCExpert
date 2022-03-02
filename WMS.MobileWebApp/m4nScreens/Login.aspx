<% System.Web.HttpContext.Current.Session("Made4Net_Mobile_NewWindow")=1 %>
<%@ Page Language="vb" AutoEventWireup="true" Codebehind="Login.aspx.vb" Inherits="WMS.MobileWebApp.Login" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>Login</title>
		<meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">

        <!-- #include file="~/greenscreen/html/head.html" -->
	</HEAD>
	<body>
		<div class="header">
			Login
		</div>
		<div class="content login">
			<form id="Form1" method="post" runat="server">
				<div class="table">
                    <div class="field">
						<div class="container">
							<h3>Username</h3>
							<asp:TextBox id="UserName" runat="server" />
						</div>
					</div>
					<div class="field">
						<div class="container">
							<h3>Password</h3>
							<asp:TextBox id="Password" TextMode="Password" runat="server" />
						</div>
					</div>
                </div>
                <div class="actions">
                    <div class="button highlighted">
                        <asp:Button id="Submit" Text="Submit" runat="server" />
                    </div>
                </div>
            </form>
		</div>
	</body>
</HTML>
