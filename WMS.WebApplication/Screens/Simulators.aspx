<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Simulators.aspx.vb" Inherits="WMS.WebApp.Simulators"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<html>
  <head>
    <title>Shift</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name=vs_defaultClientScript content="JavaScript">
    <meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5">
  	<!-- #include file="~/include/head.html" -->
  </head>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" >
		<!-- #include file="~/include/Header.html" -->
		<form id="Form1" method="post" runat="server">
            <telerik:RadScriptManager ID="RadScriptManager2" runat="server">     </telerik:RadScriptManager> 
			<cc3:screen id="Screen1" runat="server" ScreenID="sml"></cc3:screen>
			<cc2:datatabcontrol id="DTC" runat="server"  SyncEditMode="True">
				
	<TABLE id="Table1" cellSpacing="0" cellPadding="0" border="0" runat="server">
						<TR>
							<TD>
								<cc2:tabstrip  OnSelectedIndexChange ="TS_OnSelectedIndexChange" id="TS" runat="server" TargetTableID="Tbl" SepDefaultStyle="border-bottom:solid 1px gray;"
									TabSelectedStyle="border:solid 1px gray;border-bottom:none;background:white;" TabHoverStyle="background-color:#DDDDDD;"
									TabDefaultStyle="background-color:#DDDDDD;border:solid 1px gray;padding-left:5px;padding-right:5px;font-size:13px;font-weight:bold;padding-top:2px;padding-bottom:2px;"
									AutoPostBack="True">
									<iewc:Tab Text="Planner"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="Task Manager"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="PutAway"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="Replenisment"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
                                    <iewc:Tab Text="Distance"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
								</cc2:tabstrip></TD>
						</TR>
						<TR>
							<TD style="BORDER-RIGHT: gray 1px solid; BORDER-LEFT: gray 1px solid; BORDER-BOTTOM: gray 1px solid; HEIGHT: 18px">
								<TABLE id="Tbl" cellSpacing="0" cellPadding="5" border="0" runat="server">
									<TR>
									<%--Planner--%>	
										<TD>
                                        <TABLE id="tblPlanner" cellSpacing="0" cellPadding="5" border="0" runat="server">
                                        <tr>
                                        <td valign=top>
                                            <asp:RadioButton ID="rdbWave" name="rdbWave" runat="server"  AutoPostBack =true  Checked OnCheckedChanged ="rdbPlanner_CheckedChanged" GroupName="Planner"/>Wave<br>
                                            <asp:RadioButton ID="rdbOrders" name="rdbOrders" runat="server"  AutoPostBack =true OnCheckedChanged ="rdbPlanner_CheckedChanged" GroupName="Planner"/>Order
                                        </TD>
                                        <td valign=top>
                                           <table cellpadding =2 cellspacing =2 border=0>

                                            <TR><TD><cc2:Label runat="server" ID="lblWave">Wave: </cc2:Label></TD><TD><cc2:TypeAheadBox ID="txtWave" name="txtWave"  TableName = "WAVE" TextField = "WAVE" ValueField = "WAVE" runat="server"></cc2:TypeAheadBox></TD></TR>
                                            <TR><TD><cc2:Label runat="server" visible=false id="lblCons">Consignee: </cc2:Label></TD><TD><cc2:TypeAheadBox ID="txtCons" TableName = "CONSIGNEE" TextField = "CONSIGNEE" ValueField = "CONSIGNEE" name="txtCons"  visible=false runat="server"></cc2:TypeAheadBox></TD></TR>
                                            <TR><TD><cc2:Label runat="server" visible=false ID="lblOrder">Order: </cc2:Label></TD><TD><cc2:TypeAheadBox ID="txtOrder" TableName = "OUTBOUNDORHEADER" TextField = "ORDERID" ValueField = "ORDERID" name="txtOrder" visible=false runat="server"></cc2:TypeAheadBox></TD></TR>
                                        </table>
                                        </td> 
                                        <td valign=top>
                                            <cc2:Button ID="btnSimulatePlanner" runat="server" Text="Simulate" AutoPostBack =true  OnClick ="rdbPlanner_Simulate"  />
                                        </td>
										</tr>
										</table>
										</td>
										
										<%--TaskManager--%>	
										<TD>
                                        <TABLE id="tblTaskManager" cellSpacing="0" cellPadding="5" border="0" runat="server">
                                        <tr>
                                        <td valign=top>
                                        <table cellpadding =2 cellspacing =2 border=0>
                                            <TR><TD><cc2:Label runat="server" ID="lblUser">User: </cc2:Label></TD><TD><cc2:TypeAheadBox  ID="taUserID" TableName = "WHACTIVITY" TextField = "USERID" ValueField = "USERID" name="taUserID"  runat="server"></cc2:TypeAheadBox></TD></TR>
                                            <TR><TD><cc2:Label runat="server" ID="lblMHType">MHEquip ID: </cc2:Label></TD><TD><cc2:TypeAheadBox  ID="taMHEquip" name="taMHType" TableName = "MHE" TextField = "MHEID" ValueField = "MHEID" runat="server"></cc2:TypeAheadBox></TD></TR>
                                            <TR><TD><cc2:Label runat="server" ID="LabeInitalLocation">Initial Location: </cc2:Label></TD><TD><cc2:TypeAheadBox  ID="taInitLoc" TableName = "LOCATION" TextField = "LOCATION" ValueField = "LOCATION" name="taInitLoc"  runat="server"></cc2:TypeAheadBox></TD></TR>
                                        </table>
                                        </td> 
                                        <td valign=top>
                                            <cc2:Button ID="btnTaskManager" runat="server" Text="Simulate" AutoPostBack =true  OnClick ="TaskManager_Simulate"  />
                                        </td>
										</tr>
										</table>											
										</TD>

                                        <%--PutAway--%>	
										<TD>
										<TABLE id="TABLE4" cellSpacing="0" cellPadding="5" border="0" runat="server">
                                        <tr>
                                        <td valign=top>
                                            <asp:RadioButton ID="rdbLoad" name="rdbLoad" runat="server"  AutoPostBack ="true" Checked="True" OnCheckedChanged ="rdbPutAway_CheckedChanged" GroupName="PutAway"/>Load<br>
                                            <asp:RadioButton   ID="rdbContainer" name="rdbContainer" runat="server"  AutoPostBack ="true"   OnCheckedChanged ="rdbPutAway_CheckedChanged" GroupName="PutAway"/>Container
                                        </TD>
                                        <td valign=top>
                                        <%--<TABLE id="TABLE2" cellSpacing="0" cellPadding="5" border="0" runat="server">--%>
                                        <table cellpadding ="2" cellspacing ="2" border="0">
                                            <tr><td><cc2:Label runat="server"  ID="LabelLoad">Load ID: </cc2:Label></TD><TD><cc2:TypeAheadBox  ID="taLoad" TableName = "LOADS" TextField = "LOADID" ValueField = "LOADID" name="taUserID"  runat="server"></cc2:TypeAheadBox></TD></TR>
                                            <TR><TD><cc2:Label runat="server" ID="LabeUsrId">User: </cc2:Label></TD><TD><cc2:TypeAheadBox  ID="TypeAheadBoxUsrId" TableName = "WHACTIVITY" TextField = "USERID" ValueField = "USERID" name="taUsrID"  runat="server"></cc2:TypeAheadBox></TD></TR>
                                            <tr><td><cc2:Label runat="server" Visible="false" ID="LabelContainer">Container ID: </cc2:Label></TD><TD><cc2:TypeAheadBox Visible="false"  ID="taContainer" name="taContainer" TableName = "CONTAINER" TextField = "CONTAINER" ValueField = "CONTAINER" runat="server"></cc2:TypeAheadBox></TD></TR>
                                        </table>
                                        </td> 
                                        <td valign=top>
                                            <cc2:Button ID="Button1" runat="server" Text="Simulate" AutoPostBack =true  OnClick ="PutAway_Simulate"  />
                                        </td>
										</tr>
										</TABLE>
											
										</TD>



										<%--Replenisment--%>											
										<TD>
                                        <TABLE id="TABLE3" cellSpacing="0" cellPadding="5" border="0" runat="server">
                                        <tr>
                                        <td valign=top>
                                            <asp:RadioButton ID="rdbReplZone" name="rdbReplZone" runat="server"  AutoPostBack =true Checked OnCheckedChanged ="rdbReplenisment_CheckedChanged" GroupName="Replenisment"/>Zone<br>
                                            <asp:RadioButton   ID="rdbReplPicking" name="rdbReplPicking" runat="server"  AutoPostBack =true   OnCheckedChanged ="rdbReplenisment_CheckedChanged" GroupName="Replenisment"/>Picking
                                        </TD>
                                        <td valign=top>
                                           <table cellpadding =2 cellspacing =2 border=0>

                                            <TR><TD><cc2:Label visible=false runat="server" ID="lblLocation">Location: </cc2:Label></TD><TD><cc2:TypeAheadBox  visible=false ID="taLocation" name="txtaLocation"  TableName = "PICKLOC" TextField = "LOCATION" ValueField = "LOCATION" runat="server"></cc2:TypeAheadBox></TD></TR>
                                            <TR><TD><cc2:Label visible=false runat="server" ID="lblWareahousearea">Wareahousearea: </cc2:Label></TD><TD><cc2:TypeAheadBox visible=false ID="taWareahousearea" name="taWareahousearea" TableName = "PICKLOC" TextField = "WAREHOUSEAREA" ValueField = "WAREHOUSEAREA" ReadOnly="True" runat="server"></cc2:TypeAheadBox></TD></TR>
                                            <TR><TD><cc2:Label runat="server" ID="lblPutRegion">Put Region: </cc2:Label></TD><TD><cc2:TypeAheadBox ID="taPutRegion" name="taPutRegion"  TableName = "LOCATION" TextField = "PUTREGION" ValueField = "PUTREGION" runat="server"></cc2:TypeAheadBox></TD></TR>
                                            <TR><TD><cc2:Label runat="server" ID="lblReplSku">SKU: </cc2:Label></TD><TD><cc2:TypeAheadBox ID="taSKU" TableName = "SKU" TextField = "SKU" ValueField = "SKU" name="taSKU"  runat="server"></cc2:TypeAheadBox></TD></TR>
                                        </table>
                                        </td> 
                                        <td valign=top>
                                            <cc2:Button ID="btnReplenisment" runat="server" Text="Simulate" AutoPostBack =true  OnClick ="rdbReplenisment_Simulate"  />
                                        </td>
										</tr>
										</table>
										</td>
										

                                        <%-- Distance --%>
										
	
										<TD>
                                        <TABLE id="TABLEDISTANCE" cellSpacing="0" cellPadding="5" border="0" runat="server">
                                        <tr>
                                        <td valign=top>
                                        <table cellpadding =2 cellspacing =2 border=0>
                                            <TR><TD><cc2:Label runat="server" ID="Label1">From Location: </cc2:Label></TD><TD><cc2:TypeAheadBox  ID="TypeAheadBoxLocationFrom" TableName = "LOCATION" TextField = "LOCATION" ValueField = "LOCATION" name="txtaLocationFrom"  runat="server"></cc2:TypeAheadBox></TD></TR>
                                            <TR><TD><cc2:Label runat="server" ID="Label2">To Location: </cc2:Label></TD><TD><cc2:TypeAheadBox  ID="TypeAheadBoxLocationTo" name="txtaLocationTo" TableName = "LOCATION" TextField = "LOCATION" ValueField = "LOCATION" runat="server"></cc2:TypeAheadBox></TD></TR>
                                            <TR><TD><cc2:Label runat="server" ID="Label3">Equipment height if any (Enter 0 if nothing): </cc2:Label></TD><TD><cc2:TextBox  ID="TextBoxHeight" name="txtaHeight" TextField = "Height" runat="server" Value="0"></cc2:TextBox></TD></TR>
                                            <TR><TD><cc2:Label runat="server" ID="Label4">Handling Equipment: </cc2:Label></TD><TD><cc2:TypeAheadBox  ID="TypeAheadBoxHandlingEquipment" TableName = "HANDLINGEQUIPMENT" TextField = "HANDLINGEQUIPMENT" ValueField = "HANDLINGEQUIPMENT" name="txtaHandlingEquip"  runat="server" AutoPostBack="true"></cc2:TypeAheadBox></TD></TR>
                                            <TR><TD><cc2:Label runat="server" ID="Label5">Calculate Oneway Distance </cc2:Label><cc2:CheckBox runat="server" ID="CheckBoxUnidirection" /></TD></TR>
                                        </table>
                                        </td> 
                                        <td valign=top>
                                            <cc2:Button ID="Button2" runat="server" Text="Calculate" AutoPostBack=true  OnClick ="Distance_Calculate"  />
                                            <cc2:Button ID="Button3" runat="server" Text="Clear" AutoPostBack=true  OnClick="Clear_Distance"  />
                                        </td>
										</tr>
										</table>											
										</TD>

									</TR>
								</TABLE>
                                <table><tr> <td valign=top><cc2:Checkbox id=chkisDeleteLog Checked ="True" Visible="False" runat="server" AutoPostBack =true /><%--&nbsp;Delete log file after simulation--%></td></tr>
                                <tr><td><cc2:TextBox readonly=true BackColor =ControlLightLight  id="logFiletxt"  BorderColor="lightGray" BorderStyle="Solid"  columns =200 Rows =30 textmode=MultiLine runat="server"></cc2:TextBox></td>
                                <%--<td valign=top >--%>
                                </tr></table>
							</TD>
						</TR>
					</TABLE>
				</cc2:datatabcontrol>
		</form>
	</body>
</html>
