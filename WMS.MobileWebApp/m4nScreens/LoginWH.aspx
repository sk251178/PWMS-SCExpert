<% System.Web.HttpContext.Current.Session("Made4Net_Mobile_NewWindow")=1 %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="LoginWH.aspx.vb" Inherits="WMS.MobileWebApp.LoginWH"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
  <head>
    <title>Login - Choose a Warehouse</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name=vs_defaultClientScript content="JavaScript">
    <meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5">

    <!-- #include file="~/greenscreen/html/head.html" -->
  </head>
    <body>
		<div class="header">
			Warehouses
		</div>
		<div class="content login">
			<form id="Form1" method="post" runat="server">
				<div class="table">
                    <div class="field full">
						<div class="container">
							<h3>Please choose a warehouse</h3>
							<asp:DropDownList id="WarehouseSelectBox" runat="server"  />
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
</html>
