Imports WMS.Logic
Imports Made4Net.Shared.Web
Imports Made4Net.Mobile

Imports System.Collections.Generic
Partial Public Class CloseContConfirm
    Inherits PWMSRDTBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        'Commented for RWMS-1643 and RWMS-745
        'DO1.Value("NOTES") = t.Translate("Are you sure you want to close container?")
        'End Commented for RWMS-1643 and RWMS-745
        'Added for RWMS-1643 and RWMS-745
        Dim contid As String = ""
        If Not Session("PCKPicklistActiveContainerID") Is Nothing Then
            contid = Session("PCKPicklistActiveContainerID")
            DO1.Value("NOTES") = t.Translate("Do you want to close conatiner " + contid + " ?")
        Else
            DO1.Value("NOTES") = t.Translate("Do you want to close conatiner?")
        End If
        'End Added for RWMS-1643 and RWMS-745

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
        doCloseContainer()
    End Sub



    Private Sub doBack()
        Try
            'Commented for RWMS-1643 and RWMS-745
            'Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("Screens/PCKPART.aspx"))
            'End Commented for RWMS-1643 and RWMS-745
            'Added for RWMS-1643 and RWMS-745
            If Not Request.QueryString("sourcescreen") Is Nothing Then
                Dim src As String = Request.QueryString("sourcescreen")
                Response.Redirect(MapVirtualPath("screens/" & src & ".aspx"))
            Else
                Response.Redirect(MapVirtualPath("screens/PCKPART.aspx"))
            End If
            'End Added for RWMS-1643 and RWMS-745


        Catch ex As System.Threading.ThreadAbortException

        End Try

    End Sub



    Private Sub doCloseContainer()
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        If Not Session("PCKPicklistActiveContainerID") Is Nothing Then
            Try
                'Commented for RWMS-1643 and RWMS-745
                'Response.Redirect(MapVirtualPath("screens/PCKCLOSECONTAINER.aspx"))
                'End Commented for RWMS-1643 and RWMS-745
                'Added for RWMS-1643 and RWMS-745
                If Not Request.QueryString("sourcescreen") Is Nothing Then
                    Dim src As String = Request.QueryString("sourcescreen")
                    Response.Redirect(MapVirtualPath("screens/PCKCLOSECONTAINER.aspx?sourcescreen=" + src))
                Else
                    Response.Redirect(MapVirtualPath("screens/PCKCLOSECONTAINER.aspx"))
                End If
                'End Added for RWMS-1643 and RWMS-745



            Catch ex As System.Threading.ThreadAbortException

            End Try

            '    Dim pcklist As WMS.Logic.Picklist = Session("PCKPicklist") 'New Picklist(pck.picklist)

            '    If (pcklist.HandelingUnitType = WMS.Logic.WarehouseParams.GetWarehouseParam("MultiPickHUType") And Session("MHType") = WarehouseParams.GetWarehouseParam("MultiPickMHType")) Then
            '        Try
            '            Response.Redirect(MapVirtualPath("screens/PCKCLOSECONTAINER.aspx"))

            '        Catch ex As Made4Net.Shared.M4NException
            '            ' HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            '            Return
            '        Catch ex As Exception
            '            ' HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.Message)
            '            Return
            '        End Try
            '    End If

            '    Dim relStrat As ReleaseStrategyDetail
            '    relStrat = pcklist.getReleaseStrategy()
            '    Dim pck As PickJob = Session("PCKPicklistPickJob")
            '    If Not relStrat Is Nothing Then
            '        If relStrat.DeliverContainerOnClosing Then
            '            'Should deliver the container now
            '            pcklist.CloseContainer(Session("PCKPicklistActiveContainerID"), True, WMS.Logic.GetCurrentUser)
            '            Response.Redirect(MapVirtualPath("screens/DELLBLPRNT.aspx?sourcescreen=PCK"))
            '        Else
            '            'Should close the container - go back to PCK to open a new one
            '            pcklist.CloseContainer(Session("PCKPicklistActiveContainerID"), False, WMS.Logic.GetCurrentUser)
            '            Session.Remove("PCKPicklistActiveContainerID")
            '            Response.Redirect(MapVirtualPath("screens/PCK.aspx"))
            '        End If
            '    End If
            'Catch ex As Threading.ThreadAbortException
            'Catch ex As Made4Net.Shared.M4NException
            '    HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            '    Return
            'Catch ex As Exception
            '    HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.Message)
            '    Return
            'End Try
        Else
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("Cannot Close Cotnainer - Container is blank"))
        End If
    End Sub


End Class