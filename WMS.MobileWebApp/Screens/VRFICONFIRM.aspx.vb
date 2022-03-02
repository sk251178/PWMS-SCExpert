Imports System.Collections.Generic
Partial Public Class VRFICONFIRM
    Inherits PWMSRDTBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Session("VRFIDICT") Is Nothing Then
            Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("Screens/VRFI.aspx"))
        End If
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        DO1.Value("NOTES") = t.Translate("Are you sure you want to finish the verification process?")
    End Sub

    Protected Sub DO1_CreatedChildControls(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddLabelLine("NOTES")
    End Sub

    <CLSCompliant(False)>
    Protected Sub DO1_ButtonClick(ByVal sender As System.Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "yes"
                doDone()
            Case "no"
                doBack()
        End Select
    End Sub

    Private Sub doDone()

        MobileUtils.doDoneVerification()
        Session.Remove("VRFILD")
        Session.Remove("VRFICONT")
        Session.Remove("VRFISKU")
        Session.Remove("VRFICONSIGNEE")
        Session.Remove("objMultiUOMUnits")
        'Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

        'Dim dict As Dictionary(Of WMS.Logic.Load, MobileUtils.VerificationData) = CType(Session("VRFIDICT"), Dictionary(Of WMS.Logic.Load, MobileUtils.VerificationData))
        'For Each pair As KeyValuePair(Of WMS.Logic.Load, MobileUtils.VerificationData) In dict
        '    Dim previousStatus As String = pair.Key.ACTIVITYSTATUS
        '    pair.Key.Verify(WMS.Logic.GetCurrentUser)
        '    'sendMsgToAudit(pair.Key, previousStatus, pair.Value.VerifiedQty)
        'Next


        '' printReport(MobileUtils.GetMHEDefaultPrinter(), Made4Net.Shared.Translation.Translator.CurrentLanguageID, WMS.Logic.GetCurrentUser, Session("VRFIHUID"))
        'Session.Remove("VRFIDICT")
        ''If Not Session("VRFICONT") Is Nothing Then
        ''    sendMsgToAudit(Session("VRFICONT"))
        ''End If
        'Session.Remove("VRFICONT")
        'Session.Remove("VRFILD")
        'Session.Remove("VRFIHUID")
        ' HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Verification complete."))
        'Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("Screens/VRFI.aspx"))
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


    Private Sub doBack()
        'Begin RWMS-559 - Added the below session.remove() methods to remove sessions and get back to the verification page.
        Session.Remove("VRFILD")
        Session.Remove("VRFISKU")
        Session.Remove("VRFICONSIGNEE")
        Session.Remove("objMultiUOMUnits")
        Session.Remove("VRFICONT")
        Session.Remove("VRFIDICT")
        'End RWMS-559
        Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("Screens/VRFI3.aspx"))
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
        aq.Add("FROMLOC", pLoad.LOCATION)
        aq.Add("FROMQTY", MobileUtils.getLoadUnits(pLoad))
        aq.Add("FROMSTATUS", pPreviousStatus)
        'Dim sql As String = String.Format("select userid from audit where activitytype='PICKLOAD' and toload={0}", _
        'Made4Net.Shared.FormatField(pLoad.LOADID))
        'Dim pickedUser As String = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
        aq.Add("NOTES", "") 'pickedUser)
        aq.Add("SKU", pLoad.SKU)
        aq.Add("TOLOAD", pLoad.LOADID)
        aq.Add("TOCONTAINER", pLoad.ContainerId)
        aq.Add("TOLOC", pLoad.LOCATION)
        aq.Add("TOQTY", pVerifiedQty)
        aq.Add("TOSTATUS", pLoad.ACTIVITYSTATUS)
        If MobileUtils.getLoadUnits(pLoad) <> pVerifiedQty Then
            aq.Add("REASONCODE", "Discrepancy")
        End If
        aq.Add("USERID", WMS.Logic.Common.GetCurrentUser())
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", WMS.Logic.Common.GetCurrentUser())
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", WMS.Logic.Common.GetCurrentUser())
        aq.Send("LOADVALIDATION")
    End Sub

    Private Sub sendMsgToAudit(ByVal pCont As WMS.Logic.Container) ', ByVal pPreviousStatus As String, ByVal pVerifiedQty As Decimal)
        Dim aq As WMS.Logic.EventManagerQ = New WMS.Logic.EventManagerQ
        Dim eventType As Integer = 131313
        aq.Add("EVENT", eventType)
        aq.Add("ACTIVITYTYPE", "CONTVALD")
        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ACTIVITYTIME", "0")
        aq.Add("CONSIGNEE", "")
        aq.Add("DOCUMENT", "")
        aq.Add("DOCUMENTLINE", 0)
        aq.Add("FROMLOAD", "")
        aq.Add("FROMCONTAINER", pCont.ContainerId)
        aq.Add("FROMLOC", pCont.Location)
        aq.Add("FROMQTY", "")
        aq.Add("FROMSTATUS", pCont.Status)
        'Dim sql As String = String.Format("select userid from audit where activitytype='PICKLOAD' and toload={0}", _
        'Made4Net.Shared.FormatField(pLoad.LOADID))
        ''Dim pickedUser As String = Made4Net.DataAccess.DataInterface.ExecuteScalar(Sql)
        aq.Add("NOTES", "") 'pickedUser)
        aq.Add("SKU", "")
        aq.Add("TOLOAD", "")
        aq.Add("TOCONTAINER", pCont.ContainerId)
        aq.Add("TOLOC", pCont.Location)
        aq.Add("TOQTY", "")
        aq.Add("TOSTATUS", pCont.ActivityStatus)
        'If pLoad.UNITS <> pVerifiedQty Then
        '    aq.Add("REASONCODE", "Discrepancy")
        'End If
        aq.Add("USERID", WMS.Logic.Common.GetCurrentUser())
        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("ADDUSER", WMS.Logic.Common.GetCurrentUser())
        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        aq.Add("EDITUSER", WMS.Logic.Common.GetCurrentUser())
        aq.Send("CONTVALIDATION")
    End Sub
End Class