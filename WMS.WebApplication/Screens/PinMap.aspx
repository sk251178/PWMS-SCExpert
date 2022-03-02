<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="PinMap.aspx.vb" Inherits="WMS.WebApp.PinMap" %>
<script src="http://maps.google.com/maps?file=api&amp;v=2.x&amp;key=ABQIAAAAzr2EBOXUKnm_jVnk0OJI7xSosDVG8KKPE1-m51RBrvYughuyMxQ-i1QfUnH94QxWIa6N4U6MouMmBA" type="text/javascript"></script>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Pin Map</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout">
		<form id="Form1" method="post" runat="server">
        <input type=hidden id=matchresult name=matchresult value=''>

			<cc3:screen id="Screen1" runat="server" ScreenID="pm" noLoginRequired="true"></cc3:screen>
			<TABLE cellSpacing="0" cellPadding="0" border="1">
				<TR>
					<TD vAlign="top" borderColor="#000000"><cc2:map id="MPTest" runat="server"></cc2:map></TD>
					<TD vAlign="top" borderColor="#ffffff"><cc2:datatabcontrol id="DTC" runat="server" SyncEditMode="False">
							<TABLE id="Table1" cellSpacing="0" cellPadding="0" border="0" runat="server">
								<TR>
									<TD>
										<cc2:tabstrip id="TS" runat="server" AutoPostBack="True" TargetTableID="Tbl" SepDefaultStyle="border-bottom:solid 1px gray;"
											TabSelectedStyle="border:solid 1px gray;border-bottom:none;background:white;" TabHoverStyle="background-color:#DDDDDD;"
											TabDefaultStyle="background-color:#DDDDDD;border:solid 1px gray;padding-left:5px;padding-right:5px;font-size:13px;font-weight:bold;padding-top:2px;padding-bottom:2px;">
											<iewc:Tab Text="Companies"></iewc:Tab>
											<iewc:TabSeparator></iewc:TabSeparator>
											<iewc:Tab Text="Depots"></iewc:Tab>
										</cc2:tabstrip></TD>
								</TR>
								<TR>
									<TD style="BORDER-RIGHT: gray 1px solid; BORDER-LEFT: gray 1px solid; BORDER-BOTTOM: gray 1px solid; HEIGHT: 18px">
										<TABLE id="Tbl" cellSpacing="0" cellPadding="5" border="0" runat="server">
											<TR>
												<TD>
													<cc2:TableEditor id="TECompanies" runat="server" SortExperssion="company,contactid" DisableSwitches="trmie"
														DefaultMode="Grid"  AlwaysShowGrid=false AlwaysShowSearch=false  GridDT="DTCompanyContacts" DefaultDT="DTCompanyContacts" GridPageSize="15"
														SearchDT="DTCompanyContactsSearch"></cc2:TableEditor></TD>
												<TD>
													<cc2:TableEditor id="TEDepots" runat="server" DefaultMode="Grid" DisableSwitches="trmie" DefaultDT="DTDepotMap"
													 AutoSelectMode=Grid 	GridPageSize="5" ></cc2:TableEditor></TD>
											</TR>
										</TABLE>
									</TD>
								</TR>
							</TABLE>
						</cc2:datatabcontrol></TD>
				</TR>
			</TABLE>
		</form>
	</body>
</HTML>

<script type="text/javascript">
 
    var map = null;
    var geocoder = null;
 
    function initialize() {
      if (GBrowserIsCompatible()) {
        geocoder = new GClientGeocoder();
      }
    }
 
    function matchAddress(address) {
      if (geocoder) {
        geocoder.getLatLng(
          address,
          function(point) {
            if (!point) {
              alert(address + " not found");
            } else {
                alert(point.x);
                alert(point.y);
            }
          }
        );
      }
    }
    </script>
    
    