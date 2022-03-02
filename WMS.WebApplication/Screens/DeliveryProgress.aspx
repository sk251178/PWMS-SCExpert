<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DeliveryProgress.aspx.vb" Inherits="WMS.WebApp.DeliveryProgress" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<!--<%@ Register TagPrefix="rad" Namespace="Telerik.Charting" Assembly="RadChart.Net2" %>-->
<%@ Register TagPrefix="rad" Namespace="Telerik.Web.UI.RadChart" Assembly="Telerik.Web.UI"%>
<!--<%@ Register TagPrefix="rad" Namespace="Telerik.WebControls" Assembly="RadChart.Net2"%>-->
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Delivery Progress</title>
    <!-- #include file="~/include/head.html" -->
</head>
<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout" >
		<!-- #include file="~/include/Header.html" -->
		<form id="Form1" method="post" runat="server">
            <telerik:RadScriptManager ID="RadScriptManager2" runat="server">     </telerik:RadScriptManager> 
		<cc3:screen id="Screen1" runat="server" ScreenID="dlpr"></cc3:screen>
			
            <table width="99%" height="100%" valign="top" border="0" cellpadding="0" cellspacing="0">
				<tr>
					<td valign="top">
						<table width="400px" valign="top">
							<tr>
								<td><img id="_chartPercent_Image" alt="" src=""/></td>
							</tr>
						</table>
					</td>
					<td valign="top" width="100%">
						<table id="_chart_tbl" width="100%" valign="top">
							<tr>
								<td align="center" width="100%">
									<img id="_chart_Image" alt="" src=""/>
								</td>
							</tr>
						</table>
					</td>
				</tr>
			</table>

    </form>
    <script type="text/javascript">
    
    		try		
			{	
			    var nw=_chart_tbl.clientWidth;
			    var nh=document.documentElement.clientHeight-140;//document.body.clientHeight;//-140;//_chart_tbl.clientHeight-100;
				//alert(nw);
				//alert(nh);
				if (nw<750 || nh<480)
				{
					nw=800;
					nh=480;
				}
				document.getElementById("_chart_Image").src="DeliveryProgress.aspx?w="+nw+"&h="+nh+"&cid=1";
				document.getElementById("_chart_Image").style.width = nw;//document.body.clientWidth-parseInt(document.getElementById("_DistSuccesProccesChart_Image").style.width)-150;
				document.getElementById("_chart_Image").style.height = nh;//parseInt(document.getElementById("_chart_Image").style.height)*d;
				document.getElementById("_chartPercent_Image").src="DeliveryProgress.aspx?w=400&h="+nh+"&cid=2";
				document.getElementById("_chartPercent_Image").style.width = 400;//document.body.clientWidth-parseInt(document.getElementById("_DistSuccesProccesChart_Image").style.width)-150;
				document.getElementById("_chartPercent_Image").style.height = nh;//parseInt(document.getElementById("_chart_Image").style.height)*d;
				
			}
			catch(e){};
		//	alert("error");
		//	alert(oDate.getDate() + "/" + (oDate.getMonth() + 1) + "/" + oDate.getFullYear());
		
		</script>
</body>
</html>