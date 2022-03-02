<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Company.aspx.vb" Inherits="WMS.WebApp.Company" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Company</title>
		
		<!-- #include file="~/include/head.html" -->
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout">
		<!-- #include file="~/include/Header.html" -->
		<FORM id="Form1" method="post" runat="server">
            <telerik:RadScriptManager ID="RadScriptManager2" runat="server">     </telerik:RadScriptManager> 
			<cc3:screen id="Screen1" runat="server" ScreenID="OS"></cc3:screen>
			<P><cc2:tableeditor id="TEMaster" runat="server" DefaultDT="DTCompanyHeader" ManualMode="False" DefaultVS="2"
					DefaultMode="Search" TableName="Company" GridPageSize="5" DESIGNTIMEDRAGDROP="14" EditVS="0"
					ForbidRequiredFieldsInSearchMode="True" SearchDT="DTCompanySearch" EditDT="DTCompanyHeaderEdit"
					DisableSwitches="mnd" AutoSelectGridItem="Never" AutoSelectMode="View"></cc2:tableeditor></P>
			<P>&nbsp;</P>
			<P><cc2:datatabcontrol id="DTC" runat="server" SyncEditMode="True" ParentID="TEMaster">
					<TABLE id="Table1" cellSpacing="0" cellPadding="0" border="0" runat="server">
						<TR>
							<TD>
								<cc2:tabstrip id="TS" runat="server" AutoPostBack="True" TabDefaultStyle="background-color:#DDDDDD;border:solid 1px gray;padding-left:5px;padding-right:5px;font-size:13px;font-weight:bold;padding-top:2px;padding-bottom:2px;"
									TabHoverStyle="background-color:#DDDDDD;" TabSelectedStyle="border:solid 1px gray;border-bottom:none;background:white;"
									SepDefaultStyle="border-bottom:solid 1px gray;" TargetTableID="Tbl">
									<iewc:Tab Text="Contact"></iewc:Tab>
									<iewc:TabSeparator DefaultStyle="border-bottom:soild 1 gray;"></iewc:TabSeparator>
									<iewc:Tab Text="Assign Contacts"></iewc:Tab> 
									<iewc:TabSeparator DefaultStyle="width:100%;"></iewc:TabSeparator>
									<iewc:Tab Text="Delivery"></iewc:Tab> 
									<iewc:TabSeparator DefaultStyle="width:100%;"></iewc:TabSeparator>
                                    <iewc:Tab Text="Customer Expiration Days"></iewc:Tab>    
                                    <iewc:TabSeparator DefaultStyle="width:100%;"></iewc:TabSeparator>  
								</cc2:tabstrip></TD>
						</TR>
						<TR>
							<TD style="BORDER-RIGHT: gray 1px solid; BORDER-LEFT: gray 1px solid; BORDER-BOTTOM: gray 1px solid; HEIGHT: 18px">
								<TABLE id="Tbl" cellSpacing="0" cellPadding="5" border="0" runat="server">
									<TR>
										<TD style="WIDTH: 165px">
											<cc2:TableEditor id="TEDetail1" runat="server" DisableSwitches="strmnd" ForbidRequiredFieldsInSearchMode="True"
												TableName="Contact" DefaultMode="Grid" EditDT="DTCompanyCONTACT Edit" DefaultDT="DTCONTACT" GridDT="DTCONTACT Grid" AutoSelectGridItem="Never"
												AutoSelectMode="View"></cc2:TableEditor>
											<cc2:DataConnector id="DC1" runat="server" DESIGNTIMEDRAGDROP="38" TargetID="TEDetail1" MasterGridID="TEMaster"
												MasterFields="consignee,company,companytype" ChildFields="consignee,company,companytype" MasterObjID="TEMaster"></cc2:DataConnector>
										</TD>
																				
										<TD style="WIDTH: 165px">
											<cc2:TableEditor id="TEAssignContacts" runat="server" DisableSwitches="eidrn" ForbidRequiredFieldsInSearchMode="True"
												TableName="Contact" DefaultMode="Grid" DefaultDT="DTAssignCompanyContacts" GridDT="DTAssignCompanyContacts" AutoSelectGridItem="Never"
												AutoSelectMode="View"></cc2:TableEditor>
												<%--
											<cc2:DataConnector id="DC2" runat="server" DESIGNTIMEDRAGDROP="38" TargetID="TEDetail1" MasterGridID="TEMaster"
												MasterFields="consignee,company,companytype" ChildFields="consignee,company,companytype" MasterObjID="TEMaster"></cc2:DataConnector>--%>
										</TD>		
										
										<TD style="WIDTH: 149px">
											<cc2:TableEditor id="TEDetail3" runat="server" DisableSwitches="tsrmn" ForbidRequiredFieldsInSearchMode="True"
												DefaultMode="Grid" DefaultDT="DTCompanyDelivery" AfterUpdateMode="Grid" AfterInsertMode="Grid"
												AutoSelectGridItem="Never" AutoSelectMode="View"></cc2:TableEditor>
											<cc2:DataConnector id="DC3" runat="server" DESIGNTIMEDRAGDROP="43" TargetID="TEDetail3" MasterGridID="TEMaster"
												MasterFields="defaultcontact" ChildFields="contact" MasterObjID="TEMaster"></cc2:DataConnector></TD>
                                        <td style="width: 149px">
                                            <cc2:TableEditor ID="TableCustExpDays" runat="server" DisableSwitches="tsrmn" ForbidRequiredFieldsInSearchMode="True"
                                                DefaultMode="Grid" DefaultDT="DTCustExpDaysGrid" InsertDT="DTCustExpDaysInsert" EditDT="DTCustExpDaysEdit" AfterUpdateMode="Grid" AfterInsertMode="Grid"
                                                AutoSelectGridItem="Never" AutoSelectMode="View"></cc2:TableEditor>
                                            <cc2:DataConnector ID="TCED" runat="server" DESIGNTIMEDRAGDROP="43" TargetID="TableCustExpDays" MasterGridID="TEMaster"
                                                MasterFields="consignee,company,companytype" ChildFields="consignee,company,companytype" MasterObjID="TEMaster"></cc2:DataConnector>
                                        </td>

                                    </TR>
								</TABLE>
							</TD>
						</TR>
					</TABLE>
				</cc2:datatabcontrol></P>
		</FORM>
	</body>
</HTML>
