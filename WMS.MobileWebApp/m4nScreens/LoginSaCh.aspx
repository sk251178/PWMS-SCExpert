<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="LoginSaCh.aspx.vb" Inherits="WMS.MobileWebApp.LoginSaCh" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
	<head runat="server">
		<title>Safety Check</title>
		<!-- #include file="~/greenscreen/html/head.html" -->
	</head>
	<body>
		<div class="header">
			Safety Check
		</div>
		<div class="content login">
			<form id="Form2" method="post" runat="server">
				<div class="table">
                    <div class="field full">
						<div class="container">
							<h3>Is your equipment safe?</h3>
							<asp:DropDownList id="SafetyCheck" runat="server"  />
						</div>
					</div>
				</div>
                <div class="actions">
					<div class="button prev">
                        <asp:Button id="SubmitPrev" Text="Prev" runat="server" />
                    </div>
					<div class="button highlighted next">
                        <asp:Button id="SubmitNext" Text="Next" runat="server" />
                    </div>
                </div>
            </form>
		</div>
	</body>
</html>
