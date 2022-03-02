<%@ Register TagPrefix="cc4" Namespace="Made4Net.WebControls.Charting" Assembly="Made4Net.WebControls" %>
<%@ Register TagPrefix="cc3" Namespace="WMS.WebCtrls.WebCtrls" Assembly="WMS.WebCtrls" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<%@ Register TagPrefix="cc1" Namespace="ProgStudios.WebControls" Assembly="ProgStudios.WebControls" %>
<%@ Register TagPrefix="cc2" Namespace="Made4Net.WebControls" Assembly="Made4Net.WebControls" %>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Wave.aspx.vb" Inherits="WMS.WebApp.Wave" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" >
<html>
<head>
    <title>Wave</title>
    <meta content="False" name="vs_showGrid">
    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    <meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <!-- #include file="~/include/head.html" -->
</head>
<body bottommargin="0" leftmargin="0" topmargin="0" rightmargin="0" ms_positioning="FlowLayout">
    <!-- #include file="~/include/Header.html" -->
    <form id="Form1" method="post" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager2" runat="server"></telerik:RadScriptManager>
        <cc3:Screen ID="Screen1" runat="server" ScreenID="ws"></cc3:Screen>
        <%--<P><cc2:tableeditor id="TEMasterWave" runat="server" SortExperssion="CREATEDATE DESC, WAVE DESC" AfterUpdateMode="Grid"                 
               SQL="select DISTINCT  WAVE,WAVETYPE,STATUS,CREATEDATE,RELEASEDATE,NumOrderLines,NumExceptionLines,TotalVolume,TotalWeight,NumExceptionSku,QTYTOPICK,TOTALPICKLISTLINE  FROM vWaveHeader"
					AfterInsertMode="Grid" EditDT="DTWave Edit" DefaultDT="DTWave Search" ManualMode="False" DefaultMode="Search"
					GridPageSize="5" DESIGNTIMEDRAGDROP="14" ViewDT="DTWaveGrid"  ForbidRequiredFieldsInSearchMode="True" DisableSwitches="ner"
					InsertDT="DTWave Add" SearchDT="DTWave Search" GridDT="DTWaveGrid" AutoSelectMode="View"></cc2:tableeditor></P>--%>
        <p>
            <cc2:TableEditor ID="TEMasterWave" runat="server" SortExperssion="CREATEDATE DESC, WAVE DESC" AfterUpdateMode="Grid"
                SQL="select DISTINCT WAVE,WAVETYPE,STATUS,CREATEDATE,RELEASEDATE,NumOrderLines,NumExceptionLines,TotalVolume,TotalWeight,NumExceptionSku,QTYTOPICK,TOTALPICKLISTLINE FROM vWaveHeader_Search"
                AfterInsertMode="Grid" EditDT="DTWave Edit" DefaultDT="DTWave Search" ManualMode="False" DefaultMode="Search"
                GridPageSize="5" DESIGNTIMEDRAGDROP="14" ViewDT="DTWaveGrid" ForbidRequiredFieldsInSearchMode="True" DisableSwitches="ner"
                InsertDT="DTWave Add" SearchDT="DTWave Search" GridDT="DTWaveGrid" AutoSelectMode="View"></cc2:TableEditor>
        </p>

        <p>&nbsp;</p>
        <p>
            <cc2:DataTabControl ID="DTC" runat="server" SyncEditMode="True" ParentID="TEMasterWave">
                <table id="Table1" cellspacing="0" cellpadding="0" border="0" runat="server">
                    <tr>
                        <td>
                            <cc2:TabStrip ID="TS" runat="server" TargetTableID="Tbl" SepDefaultStyle="border-bottom:solid 1px gray;"
                                TabSelectedStyle="border:solid 1px gray;border-bottom:none;background:white;" TabHoverStyle="background-color:#DDDDDD;"
                                TabDefaultStyle="background-color:#DDDDDD;border:solid 1px gray;padding-left:5px;padding-right:5px;font-size:13px;font-weight:bold;padding-top:2px;padding-bottom:2px;"
                                AutoPostBack="True">
                                <iewc:Tab Text="Details"></iewc:Tab>
                                <iewc:TabSeparator></iewc:TabSeparator>
                                <iewc:Tab Text="Assign Orders"></iewc:Tab>
                                <iewc:TabSeparator></iewc:TabSeparator>
                                <iewc:Tab Text="Wave Exceptions"></iewc:Tab>
                                <iewc:TabSeparator></iewc:TabSeparator>
                                <iewc:Tab Text="Pick Details"></iewc:Tab>
                                <iewc:TabSeparator></iewc:TabSeparator>
                                <iewc:Tab Text="Delivery Location"></iewc:Tab>
                                <iewc:TabSeparator></iewc:TabSeparator>
                                <iewc:Tab Text="Wave Tasks"></iewc:Tab>
                                <iewc:TabSeparator></iewc:TabSeparator>
                                <iewc:Tab Text="Wave Summary"></iewc:Tab>
                                <iewc:TabSeparator></iewc:TabSeparator>
                                <iewc:Tab Text="Exception History"></iewc:Tab>
                                <iewc:TabSeparator></iewc:TabSeparator>
                            </cc2:TabStrip>
                        </td>
                    </tr>
                    <tr>
                        <td style="border-right: gray 1px solid; border-left: gray 1px solid; border-bottom: gray 1px solid; height: 18px">
                            <table id="Tbl" cellspacing="0" cellpadding="5" border="0" runat="server">
                                <tr>
                                    <td>
                                        <cc2:TableEditor ID="TEWaveOrders" runat="server" AutoSelectMode="View"
                                            DisableSwitches="tri" ForbidRequiredFieldsInSearchMode="True" GridPageSize="10" DefaultMode="Grid"
                                            DefaultDT="DTWaveDetail" EditDT="DTWaveDetailEdit" MultiEditDT="DTWaveOrdersMultiEdit" SearchDT="DTWaveDetailSearch"
                                            GridDT="DTWaveDetail" AutoSelectGridItem="Never"></cc2:TableEditor>
                                        <cc2:DataConnector ID="DC1" runat="server" DESIGNTIMEDRAGDROP="43" TargetID="TEWaveOrders" MasterFields="WAVE"
                                            ChildFields="WAVE" MasterObjID="TEMasterWave"></cc2:DataConnector>
                                    </td>
                                    <td>
                                        <cc2:TableEditor ID="TEAssignOrders" runat="server" AutoSelectMode="View" DisableSwitches="eidrn"
                                            ForbidRequiredFieldsInSearchMode="True" GridPageSize="10" DefaultMode="Grid" DefaultDT="DTWaveOrdersAssignSearch"
                                            GridDT="DTWaveOrdersAssign"></cc2:TableEditor>
                                    </td>
                                    <td>
                                        <cc2:TableEditor ID="TEWaveException" runat="server" AutoSelectMode="View" DisableSwitches="trmveidn"
                                            ForbidRequiredFieldsInSearchMode="True" GridPageSize="10" DefaultMode="Grid" DefaultDT="DTWaveException"
                                            AutoSelectGridItem="Never"></cc2:TableEditor>
                                        <cc2:DataConnector ID="DC2" runat="server" DESIGNTIMEDRAGDROP="43" TargetID="TEWaveException" MasterFields="WAVE"
                                            ChildFields="WAVE" MasterObjID="TEMasterWave"></cc2:DataConnector>
                                    </td>
                                    <td>
                                        <cc2:TableEditor ID="TEWavePicks" runat="server" AutoSelectMode="View" DisableSwitches="ridvtne"
                                            ForbidRequiredFieldsInSearchMode="True" DefaultMode="Grid" DefaultDT="DTPicklistDetail" GridDT="DTPicklistDetail"
                                            AutoSelectGridItem="Never"></cc2:TableEditor>
                                        <cc2:DataConnector ID="Dataconnector1" runat="server" TargetID="TEWavePicks" MasterFields="wave" ChildFields="wave"
                                            MasterObjID="TEMasterWave"></cc2:DataConnector>
                                    </td>
                                    <td>
                                        <cc2:TableEditor ID="TEDelLocation" runat="server" AutoSelectMode="View" DisableSwitches="ridvtne"
                                            ForbidRequiredFieldsInSearchMode="True" DefaultMode="Grid" DefaultDT="DTWaveOrderDelLocation"
                                            AutoSelectGridItem="Never"></cc2:TableEditor>
                                        <cc2:DataConnector ID="DC5" runat="server" TargetID="TEDelLocation" MasterFields="wave" ChildFields="wave"
                                            MasterObjID="TEMasterWave"></cc2:DataConnector>
                                    </td>
                                    <td>
                                        <cc2:TableEditor ID="TEWaveTasks" runat="server" AutoSelectMode="View" DisableSwitches="ridve" ForbidRequiredFieldsInSearchMode="True"
                                            DefaultMode="Grid" DefaultDT="DTWaveTasks" MultiEditDT="DTWaveTasksEdit" GridDT="DTWaveTasks"
                                            AutoSelectGridItem="Never"></cc2:TableEditor>
                                        <cc2:DataConnector ID="Dataconnector2" runat="server" TargetID="TEWaveTasks" MasterFields="wave" ChildFields="wave"
                                            MasterObjID="TEMasterWave"></cc2:DataConnector>
                                    </td>
                                    <td>
                                        <table>
                                            <tr>
                                                <td>
                                                    <cc2:TableEditor ID="TEWaveOrdersSummary" runat="server" AutoSelectMode="View" DisableSwitches="rstidte"
                                                        ForbidRequiredFieldsInSearchMode="True" DefaultMode="View" DefaultDT="DTWaveOrdersSummary" AutoSelectGridItem="Never"
                                                        HideActionBar="True"></cc2:TableEditor>
                                                    <cc2:DataConnector ID="Dataconnector3" runat="server" TargetID="TEWaveOrdersSummary" MasterFields="wave"
                                                        ChildFields="wave" MasterObjID="TEMasterWave"></cc2:DataConnector>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <cc2:TableEditor ID="TEWavePicksSummary" runat="server" AutoSelectMode="View" DisableSwitches="rstidte"
                                                        ForbidRequiredFieldsInSearchMode="True" DefaultMode="Grid" DefaultDT="DTWavePicksSummary" AutoSelectGridItem="Never"
                                                        HideActionBar="True"></cc2:TableEditor>
                                                    <cc2:DataConnector ID="Dataconnector4" runat="server" TargetID="TEWavePicksSummary" MasterFields="wave"
                                                        ChildFields="wave" MasterObjID="TEMasterWave"></cc2:DataConnector>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td>
                                        <cc2:TableEditor ID="TEExceptionHistory" runat="server" AutoSelectMode="View"
                                            DisableSwitches="triem" ForbidRequiredFieldsInSearchMode="True" GridPageSize="10" DefaultMode="Grid"
                                            DefaultDT="DTExceptionHistory" EditDT="DTExceptionHistory" MultiEditDT="DTExceptionHistory" SearchDT="DTExceptionHistory"
                                            GridDT="DTExceptionHistory" AutoSelectGridItem="Never"></cc2:TableEditor>
                                        <cc2:DataConnector ID="DataConnector5" runat="server" DESIGNTIMEDRAGDROP="43" TargetID="TEExceptionHistory" MasterFields="WAVE"
                                            ChildFields="WAVE" MasterObjID="TEMasterWave"></cc2:DataConnector>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </cc2:DataTabControl>
        </p>
    </form>
    <script language="javascript">

        function applyQty() {
            var cb = event.srcElement;
            var rowNum = getRowNum(cb);
            if (cb.checked) {
                setQty(rowNum)
            } else {
                clearQty(rowNum);
            }
        }

        function applyQtyCB(cb) {
            var rowNum = getRowNum(cb);
            if (cb.checked) {
                setQty(rowNum)
            } else {
                clearQty(rowNum);
            }
        }

        function clearQty(rowNum) {
            var qi = getQtyInput(rowNum);
            try {
                qi.value = '';
            } catch (e) { }
        }

        function setQty(rowNum) {
            var qi = getQtyInput(rowNum);
            try {
                qi.value = calcQty(rowNum);
            } catch (e) { }
        }

        function getQtyInput(rowNum) {
            return document.getElementById(Prefix + rowNum + INPSuffix);
        }

        function getQty(rowNum, Suffix) {
            var span = document.getElementById(Prefix + rowNum + Suffix);
            var html = span.innerHTML;
            var qty = parseInt(html);
            return qty;
        }

        function getAdjQty(rowNum) {
            return getQty(rowNum, AdjQtySuffix);
        }

        function getPickedQty(rowNum) {
            return getQty(rowNum, PickedQtySuffix);
        }

        function calcQty(rowNum) {
            var qty = getAdjQty(rowNum) - getPickedQty(rowNum);
            if (qty < 0) qty = 0;
            return qty;
        }

        function getRowNum(cb) {
            var strRowNum = cb.id.substring(Prefix.length, cb.id.length - CBSuffix.length);
            return parseInt(strRowNum);
        }

        function recalcAllHeader() { }

        function recalcAll() {
            return;
            for (n = 0; n <= cbs.length - 1; n++) {
                var cb = document.getElementById(cbs[n]);
                applyQtyCB(cb);
            }
        }

        var Prefix = 'DTC_TEWavePicks_TableEditorGridTEWavePicks_Grid__ctl';
        var CBSuffix = '_multi_select';
        var INPSuffix = '_DisplayTypeCtrl_UNITS_tbv_tb';
        var AdjQtySuffix = '_DisplayTypeCtrl_ADJQTY';
        var PickedQtySuffix = '_DisplayTypeCtrl_PICKEDQTY';

        try {
            var cbs = GridMultiselectCheckboxes_DTC_TEWavePicks_TableEditorGridTEWavePicks;
            test();
            for (n = 0; n <= cbs.length - 1; n++) {
                var cb = document.getElementById(cbs[n]);
                var rowNum = getRowNum(cb);
                if (calcQty(rowNum) == 0) {
                    cb.disabled = true;
                    var inp = getQtyInput(rowNum)
                    inp.disabled = true;
                    inp.style.backgroundColor = '#DDDDDD';
                } else {
                    cb.onclick = applyQty;
                }
            }
        }
        catch (e) { }

        //RWMS-823 Validate Priority can not be empty Start
        var button = document.getElementById('DTC_TEWaveTasks_ActionBar_btnSave_InnerButton');

        button.onclick = function () {
            var textBox = document.getElementById('DTC_TEWaveTasks_re_Form_field_PRIORITY_T1_tb').value;

            if (textBox.length <= 0) {
                alert("Priority can not be empty");
                return false;
            }
            return true;
        };
        //RWMS-823 Validate Priority can not be empty End

    </script>
</body>
</html>
