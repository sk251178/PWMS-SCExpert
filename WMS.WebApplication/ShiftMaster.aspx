<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ShiftMaster.aspx.vb" Inherits="WMS.WebApp.ShiftMaster" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.Logic" Assembly="WMS.Logic" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>RouteStopDetails</title>
		<meta content="False" name="vs_showGrid">
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</HEAD>
	<body bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0" MS_POSITIONING="FlowLayout">
		<FORM id="Form1" method="post" runat="server">


			<cc3:screen id="Screen1" runat="server" ScreenID="sftmst"></cc3:screen>
			<P ><cc2:tableeditor id="TEShiftMaster" runat="server" GridDT="DTShiftMaster" DefaultDT="DTShiftMaster" SearchDT="DTShiftMaster"
					DefaultMode="Grid" GridPageSize="7" ForbidRequiredFieldsInSearchMode="True" DisableSwitches="vnr"
					InsertDT="DTShiftMaster" EditDT="DTShiftMaster" SortExperssion="SHIFTCODE,WAREHOUSEAREA">
					</cc2:tableeditor><br></P>

			<P><cc2:datatabcontrol id="DTC" runat="server" SyncEditMode="True" ParentID="TEShiftMaster">
					<TABLE id="Table1" cellSpacing="0" cellPadding="0" border="0" runat="server">
						<TR>
							<TD>
                                <p>							
								<cc2:tabstrip id="TBC" runat="server" TargetTableID="Tbl" SepDefaultStyle="border-bottom:solid 1px gray;"
									TabSelectedStyle="border:solid 1px gray;border-bottom:none;background:white;" TabHoverStyle="background-color:#DDDDDD;"
									TabDefaultStyle="background-color:#DDDDDD;border:solid 1px gray;padding-left:5px;padding-right:5px;font-size:13px;font-weight:bold;padding-top:2px;padding-bottom:2px;"
									AutoPostBack="True">
									<iewc:Tab Text="Detail"></iewc:Tab>

									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="Time Blocks"></iewc:Tab>
									<iewc:TabSeparator></iewc:TabSeparator>
									<iewc:Tab Text="Users"></iewc:Tab>
									

									<iewc:TabSeparator DefaultStyle="width:100%;"></iewc:TabSeparator>
								</cc2:tabstrip>
								</p> 
								</TD>


						</TR>
						<TR>
							<TD style="BORDER-RIGHT: gray 1px solid; BORDER-LEFT: gray 1px solid; BORDER-BOTTOM: gray 1px solid; HEIGHT: 18px">
								
								<TABLE id="Tbl" cellSpacing="0" cellPadding="5" border="0" runat="server">
									<TR>
										<TD>
											<cc2:TableEditor id="TEShiftDetail" runat="server" EditDT="DTShiftDetail" InsertDT="DTShiftDetail"
												SortExpression="DAY,SHIFTSTARTTIME"  DisableSwitches="mnv" ForbidRequiredFieldsInSearchMode="True" DefaultMode="Grid" DefaultDT="DTShiftDetail"
												AllowDeleteInViewMode="False" DESIGNTIMEDRAGDROP="60" AutoSelectGridItem="Never" GridDT="DTShiftDetail" AutoSelectMode="View"
												ObjectID="TEShiftMaster"></cc2:TableEditor> 
											<cc2:DataConnector id="DC1" runat="server" DESIGNTIMEDRAGDROP="38" TargetID="TEShiftDetail" MasterFields="SHIFTCODE"
												ChildFields="SHIFTCODE" MasterObjID="TEShiftMaster"></cc2:DataConnector>
										</TD>
                                    <td>
												<cc2:TableEditor id="TEShiftTimeBlocks" runat="server" DisableSwitches="mntv" ForbidRequiredFieldsInSearchMode="True"
													 GridPageSize="10" DefaultMode="Grid" DefaultDT="DTShiftTimeBlocks" GridDT="DTShiftTimeBlocks"
													AutoSelectMode="View" AutoSelectGridItem="Never"></cc2:TableEditor>
											<cc2:DataConnector id="DataConnector1" runat="server" DESIGNTIMEDRAGDROP="38" TargetID="TEShiftTimeBlocks" MasterFields="SHIFTCODE"
												ChildFields="SHIFTCODE" MasterObjID="TEShiftDetail"></cc2:DataConnector>
                                    </td>

                                            <TD>
											<cc2:TableEditor id="TEUserDefShift" runat="server" EditDT="DTUserDefShift" 
											   SQL="select USERID,DEFAULTSHIFT,WAREHOUSE from userwarehouse " ConnectionName = "Made4NetSchema" 
											   SortExpression="userid"  DisableSwitches="mnievs" 
											  ForbidRequiredFieldsInSearchMode="True" DefaultMode="Grid" 
											 DefaultDT="DTUserDefShift" AllowDeleteInViewMode="False" DESIGNTIMEDRAGDROP="60" AutoSelectGridItem="Never" 
											 GridDT="DTUserDefShift" AutoSelectMode="View" ></cc2:TableEditor> 
										    </TD> 
									</TR>
                                    
								</TABLE>
							</TD>
				

						</TR>
					</TABLE>
				</cc2:datatabcontrol></P>
                                    
				
		</FORM>
	</body>
</HTML>
