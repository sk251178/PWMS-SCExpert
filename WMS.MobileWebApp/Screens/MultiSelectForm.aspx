<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="MultiSelectForm.aspx.vb" Inherits="WMS.MobileWebApp.MultiSelectForm" %>
<%@ Register TagPrefix="cc1" Namespace="WMS.MobileWebApp.WebCtrls" Assembly="WMS.MobileWebApp" %>
<%@ Register TagPrefix="cc3" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.Mobile.WebCtrls" Assembly="Made4Net.Mobile" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
	<!-- #include file="~/greenscreen/html/head-legacy.html" -->
</head>
 
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0">
		<form id="Form2" method="post" runat="server">
            <telerik:RadScriptManager ID="RadScriptManager2" runat="server">     </telerik:RadScriptManager> 
			<DIV align="center" ><cc1:screen id="Screen1" runat="server" title="Select Value" ScreenId="MULSEL"></cc1:screen></DIV>
            <div style="border-width: 1px; border-style: solid; border-color: black;width:400px;margin-left:auto;margin-right:auto" align="center" >
            <br />
                <br />
                <asp:ListBox ID="ValueList" runat="server" Rows="15" TabIndex=0></asp:ListBox>
                <br />
                <br />
          <div>
              <asp:button ID="SelectValue" text = "Select Value" runat ="server" TabIndex=1/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
 <asp:button ID="Up" text = "Up" runat ="server" TabIndex=2/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
 <asp:button ID="Down" text = "Down" runat ="server" TabIndex=3/>

          </div>
          
                <br />
            </div>
		</form>
	</body>

</html>
