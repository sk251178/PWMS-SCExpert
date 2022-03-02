<%@ Register TagPrefix="rt" Namespace="Telerik.Web.UI.RadTreeView" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="LoadingPlan.aspx.vb" Inherits="WMS.WebApp.LoadingPlan" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>LoadingPlan</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<!-- #include file="~/include/head.html" -->
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout">
		<!-- #include file="~/include/Header.html" -->
		<form id="Form1" method="post" runat="server">
            <telerik:RadScriptManager ID="RadScriptManager2" runat="server">     </telerik:RadScriptManager> 
			<P><cc3:screen id="Screen1" runat="server" ScreenID="lp"></cc3:screen><br>
				<cc2:tableeditor id="TEShipments" runat="server" DefaultDT="DTSHIPMENTVIEW" AfterUpdateMode="Grid"
					DisableSwitches="rnietdrm" DESIGNTIMEDRAGDROP="60" DefaultMode="Search" FormViewStyle="EditForm"
					ObjectID="TEShipments" SearchDT="DTSHIPMENTSearch" ForbidRequiredFieldsInSearchMode="True" SearchButtonPos="NextToSearchForm"
					SearchButtonType="Button" AlwaysShowSearch="True" AutoSelectGridItem="Never" AutoSelectMode="View"></cc2:tableeditor><BR>
			</P>
			<div id="rt" runat="server">
				<table id="RadTrees">
					<tr>
						<td vAlign="top" align="left" width="540"><rt:radtreeview id="RadTreeView1" runat="server" BeforeClientClick="ClearLocation" OnNodeDrop="HandleDrop"
								BeforeClientDrop="DropOrder" AfterClientMove="ClientMove" Visible="False" DragAndDrop="True" NodeSpacing="0" Skin="Default" ImagesBaseDir="~/RadControls/TreeView/Skins/Default/"></rt:radtreeview></td>
						<td>
							&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</td>
						<td vAlign="top" align="left" width="540"><rt:radtreeview id="Radtreeview2" runat="server" BeforeClientClick="ClearLocation" OnNodeDrop="HandleDrop"
								BeforeClientDrop="DropOrder" AfterClientMove="ClientMove" Visible="False" DragAndDrop="True" NodeSpacing="0" Skin="Default" ImagesBaseDir="~/RadControls/TreeView/Skins/Default/"></rt:radtreeview></td>
					</tr>
				</table>
			</div>
			<table>
				<tr valign="bottom">
					<td>
						<div id="LeftVehicle" runat="server"></div>
					</td>
					<td>
						<div id="RightVehicle" runat="server"></div>
					</td>
				</tr>
			</table>
			<input id="DropLocation" type="hidden" value="NONE" name="DropLocation" runat="server">&nbsp;
		</form>
		<script language="javascript">
				
				var location = null;
				
				function IsMouseOverLocation(events)
				{
					var target = (document.all) ? events.srcElement : events.target;
					parentNode = target;
					while (parentNode != null)
					{
						if (parentNode.getAttribute)
							if (parentNode.getAttribute("DropId") != null)
								return parentNode;
						parentNode = parentNode.parentNode;
					}
					return null;
				}
				
				function DropOrder(source, dest, events)
				{   
					var target = (document.all) ? events.srcElement : events.target; 
					if (target.tagName == "TD")
					{
						target.style.cursor = "default";
						source.TreeView.HtmlElementID = target.getAttribute("DropId");  
						document.getElementById("DropLocation").value = target.getAttribute("DropId");
						//alert(target.getAttribute("DropId"));
					}   
				}

				function ClientMove(events)
				{
					var target = (document.all) ? events.srcElement : events.target;   
					if (target.tagName == "TD")
					{      
						target.style.cursor = "hand";
					}
						
					var dummy = IsMouseOverLocation(events)
					if (dummy != null)
					{
						location = dummy;   
						location.style.cursor = "hand";
					}
					else
					{
						location = null;
					}
				}
				
				function ClearLocation()
				{
					document.getElementById("DropLocation").value = "NONE";
				}

		</script>
	</body>
</HTML>
