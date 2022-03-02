<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="BillingAgreement.aspx.vb" Inherits="WMS.WebApp.BillingAgreement"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>BillingAgreement</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<!-- #include file="~/include/head.html" -->
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout">
		<!-- #include file="~/include/Header.html" -->
		<form id="Form1" method="post" runat="server">
             <telerik:RadScriptManager ID="RadScriptManager2" runat="server">     </telerik:RadScriptManager> 
			<cc3:screen id="Screen1" runat="server" ScreenID="ag"></cc3:screen>
			<P>
				<cc2:tableeditor id="TEBillingAgreementMaster" runat="server" DefaultDT="DTBILLAGREEHEADER" ManualMode="False"
					DefaultVS="2" DefaultMode="Search" GridPageSize="5" EditVS="0" ForbidRequiredFieldsInSearchMode="True"
					SearchDT="DTBILLAGREEHEADER" EditDT="DTBILLAGREEHEADEREdit" DisableSwitches="mn" InsertDT="DTBILLAGREEHEADERAdd"
					GridDT="DTBILLAGREEHEADER" AutoSelectGridItem="Never" AutoSelectMode="View"></cc2:tableeditor></P>
			<p></p>
			<cc2:datatabcontrol id="DTC" runat="server" ParentID="TEBillingAgreementMaster" SyncEditMode="True">
				<TABLE id="Table2" cellSpacing="0" cellPadding="0" border="0" runat="server">
					<TR>
						<TD>
							<cc2:tabstrip id="TS" runat="server" TargetTableID="Table1" SepDefaultStyle="border-bottom:solid 1px gray;"
								TabSelectedStyle="border:solid 1px gray;border-bottom:none;background:white;" TabHoverStyle="background-color:#DDDDDD;"
								TabDefaultStyle="background-color:#DDDDDD;border:solid 1px gray;padding-left:5px;padding-right:5px;font-size:13px;font-weight:bold;padding-top:2px;padding-bottom:2px;"
								AutoPostBack="True">
								<iewc:Tab Text="Details"></iewc:Tab>
								<iewc:TabSeparator></iewc:TabSeparator>
								<iewc:Tab Text="Inbound Detail"></iewc:Tab>
								<iewc:TabSeparator></iewc:TabSeparator>
								<iewc:Tab Text="Outbound Detail"></iewc:Tab>
								<iewc:TabSeparator></iewc:TabSeparator>
								<iewc:Tab Text="Storage Detail"></iewc:Tab>
								<iewc:TabSeparator></iewc:TabSeparator>
                                <iewc:Tab Text="Storage Detail Filters"></iewc:Tab>
								<iewc:TabSeparator></iewc:TabSeparator>
								<iewc:Tab Text="Assembly Detail"></iewc:Tab>
								<iewc:TabSeparator></iewc:TabSeparator>
								<iewc:Tab Text="Disassembly Detail"></iewc:Tab>
								<iewc:TabSeparator></iewc:TabSeparator>
								<iewc:Tab Text="Value added Detail"></iewc:Tab>
								<iewc:TabSeparator></iewc:TabSeparator>
								<iewc:Tab Text="Constant Detail"></iewc:Tab>
								<iewc:TabSeparator DefaultStyle="width:100%;"></iewc:TabSeparator>
							</cc2:tabstrip></TD>
					</TR>
					<TR>
						<TD style="BORDER-RIGHT: gray 1px solid; BORDER-LEFT: gray 1px solid; BORDER-BOTTOM: gray 1px solid; HEIGHT: 18px">
							<TABLE id="Table1" cellSpacing="0" cellPadding="0" border="0" runat="server">
								<TR>
									<TD style="WIDTH: 165px">
										<cc2:TableEditor id="TEBillingAgreementDetail" runat="server" AutoSelectMode="View" AutoSelectGridItem="Never"
											InsertDT="DTBILLINGDETAILAdd" DisableSwitches="iemn" EditDT="DTBILLINGDETAILEDIT" SearchDT="DTBILLINGDETAIL"
											ForbidRequiredFieldsInSearchMode="True" EditVS="0" GridPageSize="5" DefaultMode="Grid" DefaultVS="2"
											ManualMode="False" DefaultDT="DTBILLINGDETAIL" DESIGNTIMEDRAGDROP="14" TableName="BILLINGAGREEMENTDETAIL"></cc2:TableEditor>
										<cc2:DataConnector id="DC1" runat="server" DESIGNTIMEDRAGDROP="38" TargetID="TEBillingAgreementDetail"
											MasterGridID="TEBillingAgreementMaster" MasterFields="consignee,name" ChildFields="consignee,agreementname"
											MasterObjID="TEBillingAgreementMaster"></cc2:DataConnector></TD>
									<TD style="WIDTH: 165px">
										<cc2:TableEditor id="TEBillingInboundAgreementDetail" runat="server" AutoSelectMode="View" AutoSelectGridItem="Never"
											InsertDT="DTBILLINGDETAILINBOUNDAdd" DisableSwitches="mn" EditDT="DTBILLINGDETAILINBOUNDEdit"
											SearchDT="DTBILLINGDETAIL" ForbidRequiredFieldsInSearchMode="True" EditVS="0" GridPageSize="5"
											DefaultMode="Grid" DefaultVS="2" ManualMode="False" DefaultDT="DTBILLINGDETAIL" DESIGNTIMEDRAGDROP="14"
											TableName="BILLINGAGREEMENTDETAIL" FilterExpression="TRANTYPE='INBOUND'"></cc2:TableEditor>
										<cc2:DataConnector id="Dataconnector1" runat="server" DESIGNTIMEDRAGDROP="38" TargetID="TEBillingInboundAgreementDetail"
											MasterGridID="TEBillingAgreementMaster" MasterFields="consignee,name" ChildFields="consignee,agreementname"
											MasterObjID="TEBillingAgreementMaster"></cc2:DataConnector></TD>
									<TD style="WIDTH: 165px">
										<cc2:TableEditor id="TEBillingOutboundAgreementDetail" runat="server" AutoSelectMode="View" AutoSelectGridItem="Never"
											InsertDT="DTBILLINGDETAILOUTBOUNDAdd" DisableSwitches="mn" EditDT="DTBILLINGDETAILOUTBOUNDEdit"
											SearchDT="DTBILLINGDETAIL" ForbidRequiredFieldsInSearchMode="True" EditVS="0" GridPageSize="5"
											DefaultMode="Grid" DefaultVS="2" ManualMode="False" DefaultDT="DTBILLINGDETAIL" DESIGNTIMEDRAGDROP="14"
											TableName="BILLINGAGREEMENTDETAIL" FilterExpression="TRANTYPE='OUTBOUND'"></cc2:TableEditor>
										<cc2:DataConnector id="Dataconnector2" runat="server" DESIGNTIMEDRAGDROP="38" TargetID="TEBillingOutboundAgreementDetail"
											MasterGridID="TEBillingAgreementMaster" MasterFields="consignee,name" ChildFields="consignee,agreementname"
											MasterObjID="TEBillingAgreementMaster"></cc2:DataConnector></TD>
									<TD style="WIDTH: 165px">
										<cc2:TableEditor id="TEBillingStorageAgreementDetail" runat="server" AutoSelectMode="View" AutoSelectGridItem="Never"
											InsertDT="DTBILLINGDETAILSTORAGEAdd" DisableSwitches="mn" EditDT="DTBILLINGDETAILSTORAGEAdd"
											SearchDT="DTBILLINGDETAIL" ForbidRequiredFieldsInSearchMode="True" EditVS="0" GridPageSize="5"
											DefaultMode="Grid" DefaultVS="2" ManualMode="False" DefaultDT="DTBILLINGDETAIL" DESIGNTIMEDRAGDROP="14"
											TableName="BILLINGAGREEMENTDETAIL" FilterExpression="TRANTYPE='STORAGE'"></cc2:TableEditor>
										<cc2:DataConnector id="Dataconnector3" runat="server" DESIGNTIMEDRAGDROP="38" TargetID="TEBillingStorageAgreementDetail"
											MasterGridID="TEBillingAgreementMaster" MasterFields="consignee,name" ChildFields="consignee,agreementname"
											MasterObjID="TEBillingAgreementMaster"></cc2:DataConnector></TD>
									<TD style="WIDTH: 165px">
										<cc2:TableEditor id="TEBillingAssemblyAgreementDetail" runat="server" AutoSelectMode="View" AutoSelectGridItem="Never"
											InsertDT="DTBILLINGDETAILASMAdd" DisableSwitches="mn" EditDT="DTBILLINGDETAILASMEdit" SearchDT="DTBILLINGDETAIL"
											ForbidRequiredFieldsInSearchMode="True" EditVS="0" GridPageSize="5" DefaultMode="Grid" DefaultVS="2"
											ManualMode="False" DefaultDT="DTBILLINGDETAIL" DESIGNTIMEDRAGDROP="14" TableName="BILLINGAGREEMENTDETAIL"
											FilterExpression="TRANTYPE='ASM'"></cc2:TableEditor>
										<cc2:DataConnector id="Dataconnector4" runat="server" DESIGNTIMEDRAGDROP="38" TargetID="TEBillingAssemblyAgreementDetail"
											MasterGridID="TEBillingAgreementMaster" MasterFields="consignee,name" ChildFields="consignee,agreementname"
											MasterObjID="TEBillingAgreementMaster"></cc2:DataConnector></TD>
									<TD style="WIDTH: 165px">
										<cc2:TableEditor id="TEBillingDisassemblyAgreementDetail" runat="server" AutoSelectMode="View" AutoSelectGridItem="Never"
											InsertDT="DTBILLINGDETAILDASMAdd" DisableSwitches="mn" EditDT="DTBILLINGDETAILDASMAdd" SearchDT="DTBILLINGDETAIL"
											ForbidRequiredFieldsInSearchMode="True" EditVS="0" GridPageSize="5" DefaultMode="Grid" DefaultVS="2"
											ManualMode="False" DefaultDT="DTBILLINGDETAIL" DESIGNTIMEDRAGDROP="14" TableName="BILLINGAGREEMENTDETAIL"
											FilterExpression="TRANTYPE='DASM'"></cc2:TableEditor>
										<cc2:DataConnector id="Dataconnector5" runat="server" DESIGNTIMEDRAGDROP="38" TargetID="TEBillingDisassemblyAgreementDetail"
											MasterGridID="TEBillingAgreementMaster" MasterFields="consignee,name" ChildFields="consignee,agreementname"
											MasterObjID="TEBillingAgreementMaster"></cc2:DataConnector></TD>
									<TD style="WIDTH: 165px">
										<cc2:TableEditor id="TEBillingVAAgreementDetail" runat="server" AutoSelectMode="View" AutoSelectGridItem="Never"
											InsertDT="DTBILLINGDETAILVAAdd" DisableSwitches="mn" EditDT="DTBILLINGDETAILVAAdd" SearchDT="DTBILLINGDETAIL"
											ForbidRequiredFieldsInSearchMode="True" EditVS="0" GridPageSize="5" DefaultMode="Grid" DefaultVS="2"
											ManualMode="False" DefaultDT="DTBILLINGDETAIL" DESIGNTIMEDRAGDROP="14" TableName="BILLINGAGREEMENTDETAIL"
											FilterExpression="TRANTYPE='VA'"></cc2:TableEditor>
										<cc2:DataConnector id="Dataconnector6" runat="server" DESIGNTIMEDRAGDROP="38" TargetID="TEBillingVAAgreementDetail"
											MasterGridID="TEBillingAgreementMaster" MasterFields="consignee,name" ChildFields="consignee,agreementname"
											MasterObjID="TEBillingAgreementMaster"></cc2:DataConnector></TD>
									<TD style="WIDTH: 165px">
										<cc2:TableEditor id="TEBillingConstAgreementDetail" runat="server" AutoSelectMode="View" AutoSelectGridItem="Never"
											InsertDT="DTBILLINGDETAILConstAdd" DisableSwitches="mn" EditDT="DTBILLINGDETAILConstAdd" SearchDT="DTBILLINGDETAIL"
											ForbidRequiredFieldsInSearchMode="True" EditVS="0" GridPageSize="5" DefaultMode="Grid" DefaultVS="2"
											ManualMode="False" DefaultDT="DTBILLINGDETAIL" DESIGNTIMEDRAGDROP="14" TableName="BILLINGAGREEMENTDETAIL"
											FilterExpression="TRANTYPE='CONSTANT'"></cc2:TableEditor>
										<cc2:DataConnector id="Dataconnector7" runat="server" DESIGNTIMEDRAGDROP="38" TargetID="TEBillingConstAgreementDetail"
											MasterGridID="TEBillingAgreementMaster" MasterFields="consignee,name" ChildFields="consignee,agreementname"
											MasterObjID="TEBillingAgreementMaster"></cc2:DataConnector></TD>
								</TR>
							</TABLE>
						</TD>
					</TR>
				</TABLE>
			</cc2:datatabcontrol>
			<P></P>
		</form>
	</body>
</HTML>
