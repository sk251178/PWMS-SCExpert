Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports Made4Net.DataAccess
Imports Made4Net.Shared

Partial Public Class NSPICKUP
    Inherits PWMSRDTBase


#Region "ViewState"

    Protected Overrides Function LoadPageStateFromPersistenceMedium() As Object
        Return Session("_ViewState")
    End Function

    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        Session("_ViewState") = viewState
    End Sub

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not IsPostBack Then
            Dim ts As WMS.Logic.Task

            If WMS.Logic.TaskManager.isAssigned(WMS.Logic.GetCurrentUser, WMS.Lib.TASKTYPE.NSPICKUP, WMS.Logic.LogHandler.GetRDTLogger()) Then
                ts = New WMS.Logic.Task(WMS.Logic.TaskManager.getUserAssignedTask(WMS.Logic.GetCurrentUser, WMS.Lib.TASKTYPE.NSPICKUP, WMS.Logic.LogHandler.GetRDTLogger()))
                DO1.Value("WAREHOUSEAREA") = ts.FROMWAREHOUSEAREA
                DO1.Value("LOCATION") = ts.FROMLOCATION
            End If
        End If
    End Sub



    Private Sub doMenu()
        Session.Remove("NSPICKUPLOAD")
        Session.Remove("NSPICKUPCONTAINER")
        Dim taskStr As String = WMS.Logic.TaskManager.getUserAssignedTask(WMS.Logic.GetCurrentUser, WMS.Lib.TASKTYPE.NSPICKUP, WMS.Logic.LogHandler.GetRDTLogger())

        If Not String.IsNullOrEmpty(taskStr) Then
            Dim ts As WMS.Logic.Task = New WMS.Logic.Task(taskStr)
            ts.ExitTask()
        End If
        'MobileUtils.GoToMain()
        Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("screens/main.aspx"))

    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddLabelLine("LOCATION") ', True, "next")
        DO1.AddLabelLine("WAREHOUSEAREA")
        DO1.AddSpacer()
        DO1.AddTextboxLine("LOADID")
        DO1.AddTextboxLine("CONTAINER")
        DO1.AddSpacer()
    End Sub
    Sub clear()
        DO1.Value("LOADID") = ""
        DO1.Value("CONTAINER") = ""
    End Sub
    Private Sub doNext()
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

        Try
            If Not ValidateForm() Then
                Return
            End If
            Dim ts As WMS.Logic.Task
            If WMS.Logic.TaskManager.isAssigned(WMS.Logic.GetCurrentUser, WMS.Lib.TASKTYPE.NSPICKUP, WMS.Logic.LogHandler.GetRDTLogger()) Then
                ts = New WMS.Logic.Task(WMS.Logic.TaskManager.getUserAssignedTask(WMS.Logic.GetCurrentUser, WMS.Lib.TASKTYPE.NSPICKUP, WMS.Logic.LogHandler.GetRDTLogger()))
            End If
            If Not ts Is Nothing Then
                If ts.FROMLOCATION <> DO1.Value("LOCATION") Then
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("Location Confirmation incorrect"))
                    clear()
                    Return
                End If
                ts.EDITUSER = WMS.Logic.GetCurrentUser
                ts.Complete(WMS.Logic.LogHandler.GetRDTLogger)
            End If
            If DO1.Value("CONTAINER") <> "" Then
                Dim cnt As WMS.Logic.Container = New WMS.Logic.Container(DO1.Value("CONTAINER"), True)
                cnt.RequestPickUp(WMS.Logic.GetCurrentUser)
            Else
                Dim ld As WMS.Logic.Load = New WMS.Logic.Load(DO1.Value("LOADID"))
                Session("NSPICKUPLOAD") = DO1.Value("LOADID")
                ld.RequestPickUp(WMS.Logic.GetCurrentUser, "") 'RWMS-1277
            End If
        Catch m4nEx As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  m4nEx.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Return
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate(ex.Message))
            Return
        End Try
        If DO1.Value("CONTAINER") <> "" Then
            Response.Redirect(MapVirtualPath("Screens/RPKC.aspx"))
        Else
            If Not String.IsNullOrEmpty(Session("MobileSourceScreen")) Then
                Response.Redirect(MapVirtualPath("Screens/RPK.aspx?sourcescreen=" & Session("MobileSourceScreen")))
            Else
                Response.Redirect(MapVirtualPath("Screens/RPK.aspx"))
            End If
        End If
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "next"
                doNext()
            Case "menu"
                doMenu()
        End Select
    End Sub

    Private Function ValidateForm() As Boolean
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)


        If DO1.Value("LOCATION") = "" Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("Location can not be blank"))
            clear()
            Return False
        End If
        'If DO1.Value("WAREHOUSEAREA") = "" Then
        '    HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("Warehouse can not be blank"))
        '    Return False
        'End If


        If DO1.Value("LOADID") <> "" And DO1.Value("CONTAINER") <> "" Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("Must fill only one field , load or container"))
            clear()
            Return False
        End If
        If DO1.Value("LOADID") <> "" Then
            If Not WMS.Logic.Load.Exists(DO1.Value("LOADID")) Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("Load does not exist"))
                clear()
                Return False
            End If
            Dim sSql As String = String.Format("select count(1) from invload where loadid = '{0}'", DO1.Value("LOADID"))
            If Made4Net.DataAccess.DataInterface.ExecuteScalar(sSql) = 0 Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("Load does not exist"))
                clear()
                Return False
            End If
        End If
        If DO1.Value("CONTAINER") <> "" Then
            If Not WMS.Logic.Container.Exists(DO1.Value("CONTAINER")) Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("Container does not exist"))
                clear()
                Return False
            End If
        End If
        Return True
    End Function


End Class