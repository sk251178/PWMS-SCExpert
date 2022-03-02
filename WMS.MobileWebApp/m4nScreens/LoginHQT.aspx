<%@ Page Language="vb" AutoEventWireup="false" Codebehind="LoginHQT.aspx.vb" Inherits="WMS.MobileWebApp.LoginHQT"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
  <head>
    <title>Login Parameters</title><meta content="0" name="mobileoptimized">
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name=vs_defaultClientScript content="JavaScript">
    <meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5">

	<!-- #include file="~/greenscreen/html/head.html" -->
  </head>
	<body>
		<div class="header">
			Login Parameters
		</div>
		<div class="content login">
			<form id="Form2" method="post" runat="server">
				<div class="table">
                    <div class="field full">
						<div class="container">
							<h3>Material Handling Equipment ID*</h3>
							<asp:TextBox id="MaterialHandlingEquipmentId" runat="server" />
						</div>
					</div>
					<div class="field">
						<div class="container">
							<h3>Material Handling Type</h3>
							<asp:DropDownList id="MaterialHandlingType" runat="server"  />
						</div>
					</div>
					<div class="field">
						<div class="container">
							<h3>Terminal Type</h3>
							<asp:DropDownList id="TerminalType" runat="server"  />
						</div>
					</div>
					<div class="field">
						<div class="container">
							<h3>Warehouse Area</h3>
							<asp:DropDownList id="WarehouseArea" runat="server"  />
						</div>
					</div>
					<div class="field">
						<div class="container">
							<h3>Location</h3>
							<asp:TextBox id="Location" runat="server" />
						</div>
					</div>
					<div class="field">
						<div class="container">
							<h3>Printer</h3>
							<asp:TextBox id="Printer" runat="server" />
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
