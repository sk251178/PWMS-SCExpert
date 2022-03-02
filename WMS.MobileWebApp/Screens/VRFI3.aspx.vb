Imports System.Collections.Generic
Partial Public Class VRFI3
    Inherits PWMSRDTBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Session("VRFIDICT") Is Nothing Then
            Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("Screens/VRFI.aspx"))
        End If

        If Not IsPostBack Then
            Dim discFound As Boolean = False
            Dim dict As Dictionary(Of String, MobileUtils.VerificationData) = CType(Session("VRFIDICT"), Dictionary(Of String, MobileUtils.VerificationData))
            For Each pair As KeyValuePair(Of String, MobileUtils.VerificationData) In dict
                If pair.Value.TotalQty > 0 AndAlso pair.Value.TotalQty <> pair.Value.VerifiedQty Then
                    discFound = True
                    'QTYToCase
                    Dim oSku As New WMS.Logic.SKU(pair.Key.Split("_")(0), pair.Key.Split("_")(1))

                    Dim skuDesc As String = Made4Net.DataAccess.DataInterface.ExecuteScalar(String.Format("select skudesc from sku where consignee='{0}' and sku='{1}'", pair.Key.Split("_")(0), pair.Key.Split("_")(1)))
                    DO1.AddLabelLine(pair.Key.Split("_")(0) & "-" & pair.Key.Split("_")(1) & " " & skuDesc & New Random().Next(), _
                                            pair.Key.Split("_")(0) & "-" & pair.Key.Split("_")(1) & " " & skuDesc, _
                                            MobileUtils.QTYToCase(oSku, pair.Value.VerifiedQty).ToString() & "/" & MobileUtils.QTYToCase(oSku, pair.Value.TotalQty).ToString())


                End If
            Next
            If Not discFound Then
                Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("Screens/VRFICONFIRM.aspx"))
            End If
        End If
    End Sub

    Protected Sub DO1_CreatedChildControls(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddLabelLine("Consignee-SKU Desc", "Consignee-SKU Desc", "Qty Verified/Qty Expected")
    End Sub

    <CLSCompliant(False)>
    Protected Sub DO1_ButtonClick(ByVal sender As System.Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "done"
                ManageMutliUOMUnits.Clear()
                doDone()
            Case "recount disc"
                ManageMutliUOMUnits.Clear()
                doRecount()
            Case "recount all"
                ManageMutliUOMUnits.Clear()
                doRecountAll()
            Case "back"
                ManageMutliUOMUnits.Clear()
                Dim sourcescreen As String = Request("sourcescreen")
                If Not IsNothing(sourcescreen) Then
                    Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("Screens/" & sourcescreen & ".aspx"))
                Else
                    Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("Screens/VRFI2.aspx"))
                End If

        End Select
    End Sub

    Private Sub doRecount()
        Dim dict As Dictionary(Of String, MobileUtils.VerificationData) = CType(Session("VRFIDICT"), Dictionary(Of String, MobileUtils.VerificationData))

        For Each vrfiData As MobileUtils.VerificationData In dict.Values
            If vrfiData.VerifiedQty <> vrfiData.TotalQty Then
                vrfiData.IsRecountStarted = False
                vrfiData.VerifiedQty = 0
            End If
        Next

        'printReport(MobileUtils.GetMHEDefaultPrinter(), Made4Net.Shared.Translation.Translator.CurrentLanguageID, WMS.Logic.GetCurrentUser, Session("VRFIHUID"))
        For Each pair As KeyValuePair(Of String, MobileUtils.VerificationData) In dict
            If pair.Value.VerifiedQty <> pair.Value.TotalQty Then
                pair.Value.IsRecountStarted = False
                MobileUtils.VerificationData.DeleteFromDB(Session("VRFIHUID"), pair.Key)
            End If
        Next
        Session("Recount") = 1
        Session("VRFIDICT") = dict

        Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("Screens/VRFI1.aspx"))
    End Sub

    Private Sub doRecountAll()
        Dim dict As Dictionary(Of String, MobileUtils.VerificationData) = CType(Session("VRFIDICT"), Dictionary(Of String, MobileUtils.VerificationData))

        For Each vrfiData As MobileUtils.VerificationData In dict.Values
            vrfiData.IsRecountStarted = False
            vrfiData.VerifiedQty = 0
        Next
        MobileUtils.VerificationData.DeleteFromDB(Session("VRFIHUID"))
        Session("Recount") = 1
        Session("VRFIDICT") = dict



        Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("Screens/VRFI1.aspx"))
    End Sub

    Private Sub doDone()



        Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("Screens/VRFICONFIRM.aspx"))

    End Sub

    Private Sub printReport(ByVal lblPrinter As String, ByVal Language As Int32, ByVal pUser As String, ByVal pHandlingUnit As String, Optional ByVal pReportName As String = "verificationstagedcontainers")
        Dim oQsender As New Made4Net.Shared.QMsgSender
        Dim repType As String
        Dim dt As New DataTable
        repType = pReportName
        Made4Net.DataAccess.DataInterface.FillDataset(String.Format("SELECT * FROM SYS_REPORTPARAMS WHERE REPORTID = '{0}' AND PARAMNAME = '{1}'", repType, "Copies"), dt, False)
        oQsender.Add("REPORTNAME", repType)
        oQsender.Add("REPORTID", repType)
        Try
            oQsender.Add("DATASETID", dt.Select("ParamName='DATASETNAME'")(0)("ParamValue")) ' "repContentList")
        Catch ex As Exception
            'oQsender.Add("DATASETID", "repContentList")
            Return
        End Try

        oQsender.Add("FORMAT", "PDF")
        oQsender.Add("WAREHOUSE", WMS.Logic.Warehouse.CurrentWarehouse)
        oQsender.Add("USERID", pUser)
        oQsender.Add("LANGUAGE", Language)
        Try
            oQsender.Add("PRINTER", lblPrinter)
            oQsender.Add("COPIES", dt.Rows(0)("ParamValue"))
            'If Made4Net.Shared.Util.GetSystemParameterValue("ReportServicePrintMode") = "SAVE" Then
            '    oQsender.Add("ACTION", WMS.Lib.REPORTACTIONTYPE.SAVE)
            'Else
            oQsender.Add("ACTION", WMS.Lib.REPORTACTIONTYPE.PRINT)
            'End If
        Catch ex As Exception
        End Try
        oQsender.Add("WHERE", String.Format("handlingunit = '{0}'", pHandlingUnit))
        oQsender.Send("Report", repType)
    End Sub

    Private Sub sendMsgToAudit(ByVal pLoad As WMS.Logic.Load, ByVal pPreviousStatus As String, ByVal pVerifiedQty As Decimal)
        Dim aq As WMS.Logic.EventManagerQ = New WMS.Logic.EventManagerQ
        Dim eventType As Integer = 131313
        aq.Add("EVENT", eventType)
        aq.Add("ACTIVITYTYPE", "LDVALD")
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", pLoad.CONSIGNEE)
        aq.Add("DOCUMENT", "")
        aq.Add("DOCUMENTLINE", 0)
        aq.Add("FROMLOAD", pLoad.LOADID)
        aq.Add("FROMCONTAINER", pLoad.ContainerId)
        aq.Add("FROMLOC", "")
        aq.Add("FROMQTY", MobileUtils.getLoadUnits(pLoad)) 'pLoad.UNITS)
        aq.Add("FROMSTATUS", pPreviousStatus)
        Dim sql As String = String.Format("select userid from audit where activitytype='PICKLOAD' and toload={0}", _
        Made4Net.Shared.FormatField(pLoad.LOADID))
        Dim pickedUser As String = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
        aq.Add("NOTES", pickedUser)
        aq.Add("SKU", pLoad.SKU)
        aq.Add("TOLOAD", "")
        aq.Add("TOCONTAINER", pLoad.ContainerId)
        aq.Add("TOLOC", "")
        aq.Add("TOQTY", pVerifiedQty)
        aq.Add("TOSTATUS", pLoad.ACTIVITYSTATUS)
        If MobileUtils.getLoadUnits(pLoad) <> pVerifiedQty Then
            aq.Add("REASONCODE", "Disc.")
        End If
        aq.Add("USERID", WMS.Logic.Common.GetCurrentUser())
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", WMS.Logic.Common.GetCurrentUser())
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", WMS.Logic.Common.GetCurrentUser())
        aq.Send("LOADVALIDATION")
    End Sub
End Class