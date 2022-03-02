Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports WMS.Logic
Imports RWMS.Logic


Partial Public Class CNTPW1
    Inherits PWMSRDTBase

#Region "ViewState"
    Protected Overrides Function LoadPageStateFromPersistenceMedium() As Object
        Return Session("_ViewState")
    End Function

    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        Session("_ViewState") = viewState
    End Sub
#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not IsPostBack Then
            Dim oTask As WMS.Logic.PutawayTask = Session("CotnainerPWTask")
            Dim pwJob As WMS.Logic.PutawayJob = oTask.getPutawayJob()
            If Not pwJob Is Nothing Then
                Session("ContPWJob") = pwJob
                setScreen(pwJob)
            End If
        End If
    End Sub

    Private Sub doBack()
        Response.Redirect(MapVirtualPath("Screens/CNTPW.aspx"))
    End Sub

    Private Sub setScreen(ByVal pwjob As PutawayJob)
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Dim oCont As WMS.Logic.Container
        If WMS.Logic.Container.Exists(pwjob.ContainerId) Then
            oCont = New WMS.Logic.Container(pwjob.ContainerId, True)
        End If
        DO1.Value("ContainerId") = oCont.ContainerId
        DO1.Value("ContainerType") = oCont.HandlingUnitType
        DO1.Value("NumberOfLoads") = oCont.Loads.Count
        DO1.Value("Location") = oCont.Location
        DO1.Value("Warehousearea") = oCont.Warehousearea
        DO1.Value("DestinationLocation") = pwjob.toLocation
        DO1.Value("DestinationWarehousearea") = pwjob.toWarehousearea

        If pwjob.IsHandOff Then
            DO1.setVisibility("Note", True)
            DO1.Value("Note") = trans.Translate("Task Destination Location is an Hand Off Location!")
        Else
            DO1.setVisibility("Note", False)
        End If
    End Sub

    Private Sub doNext()
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser
        Dim pwjob As PutawayJob = Session("ContPWJob")
        Try
            'Check if the location is correct , if not print an error
            If DO1.Value("ConfirmLocation").ToLower = pwjob.toLocation.ToLower And _
                    DO1.Value("ConfirmWarehousearea").ToLower = pwjob.toWarehousearea.ToLower Then
                Dim oTask As WMS.Logic.PutawayTask = Session("CotnainerPWTask")
                oTask.Put(pwjob, DO1.Value("ConfirmLocation"), "", DO1.Value("ConfirmWarehousearea"))
                DO1.Value("ConfirmLocation") = ""
                DO1.Value("ConfirmWarehousearea") = ""

                Dim cl As New WMS.Logic.ContainerLoads(pwjob.ContainerId, True)
                Dim ld As New WMS.Logic.Load
                Dim i As Integer
                Dim err As String
                For i = 0 To cl.Count - 1
                    ld = cl.Item(i)
                    AppUtil.isBackLocMoveFront(pwjob.LoadId, DO1.Value("ConfirmLocation"), DO1.Value("ConfirmWarehousearea"), "", err)
                    If err <> "" Then
                        HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  err)
                    End If
                Next


            Else
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  "Location Confirmation incorrect.")
                Return
            End If
        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Return
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.ToString())
            Return
        End Try
        doBack()
    End Sub

    Private Sub doOverride()
        Session("Mode") = "Override"
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser
        Dim pwjob As PutawayJob = Session("ContPWJob")
        Try
            'Check if the location is correct , if not print an error
            If DO1.Value("ConfirmLocation").ToLower <> pwjob.toLocation.ToLower Then
                Dim oTask As WMS.Logic.PutawayTask = Session("CotnainerPWTask")

                oTask.Put(pwjob, DO1.Value("ConfirmLocation"), DO1.Value("ConfirmWarehousearea"), "")
                DO1.Value("ConfirmLocation") = ""
                DO1.Value("ConfirmWarehousearea") = ""

                Dim cl As New WMS.Logic.ContainerLoads(pwjob.ContainerId, True)
                Dim ld As New WMS.Logic.Load
                Dim i As Integer
                Dim err As String
                For i = 0 To cl.Count - 1
                    ld = cl.Item(i)
                    AppUtil.isBackLocMoveFront(pwjob.LoadId, DO1.Value("ConfirmLocation"), DO1.Value("ConfirmWarehousearea"), "", err)
                    If err <> "" Then
                        HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  err)
                    End If
                Next

            Else
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  "Location Confirmation incorrect, use next to put in original destination location")
                Return
            End If
        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Return
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.ToString())
            Return
        End Try
        doBack()
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddLabelLine("Note")
        DO1.AddLabelLine("ContainerId")
        DO1.AddLabelLine("ContainerType")
        DO1.AddLabelLine("NumberOfLoads")
        DO1.AddLabelLine("Location")
        DO1.AddLabelLine("DestinationLocation")
        DO1.AddLabelLine("Warehousearea")
        DO1.AddLabelLine("DestinationWarehousearea")

        DO1.AddSpacer()
        DO1.AddTextboxLine("ConfirmLocation", True, "next")
        DO1.AddTextboxLine("ConfirmWarehousearea", True, "next")
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "next"
                doNext()
            Case "back"
                doBack()
            Case "override"
                doOverride()
        End Select
    End Sub

End Class
