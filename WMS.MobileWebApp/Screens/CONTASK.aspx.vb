Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports WMS.Logic
Imports WMS.Lib
Imports WLTaskManager = WMS.Logic.TaskManager

<CLSCompliant(False)> Public Class CONTASK
    Inherits PWMSRDTBase

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.MobileWebApp.WebCtrls.Screen
    Protected WithEvents DO1 As Made4Net.Mobile.WebCtrls.DataObject

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region "ViewState"

    Protected Overrides Function LoadPageStateFromPersistenceMedium() As Object
        Return Session("_ViewState")
    End Function

    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        Session("_ViewState") = viewState
    End Sub
#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If WMS.Logic.GetCurrentUser Is Nothing Then
            WMS.Logic.GotoLogin()
        End If
        If WMS.Logic.GetCurrentUser = "" Then
            WMS.Logic.GotoLogin()
        End If
        If Not IsPostBack Then
            Session.Remove("ConsolidationTask")
            Session.Remove("ConsDeliveryTask")
            If WMS.Logic.TaskManager.isAssigned(WMS.Logic.GetCurrentUser, WMS.Logic.LogHandler.GetRDTLogger()) Then
                Response.Redirect(MapVirtualPath("screens/CONSDELPRN.aspx"))
            End If
            If Not CheckAssigned() Then
                doNext()
            End If
        End If
    End Sub

    Private Function CheckAssigned() As Boolean
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser
        Dim tm As New WMS.Logic.TaskManager
        If Logic.TaskManager.isAssigned(UserId, WMS.Lib.TASKTYPE.CONSOLIDATION, WMS.Logic.LogHandler.GetRDTLogger()) Then
            Dim ConsTask As WMS.Logic.ConsolidationTask
            Try
                ConsTask = WLTaskManager.NewTaskManagerForUserAndTaskType(UserId, TASKTYPE.CONSOLIDATION, LogHandler.GetRDTLogger()).Task
            Catch ex As Exception

            End Try
            If Not ConsTask Is Nothing Then
                Session("ConsolidationTask") = ConsTask
                setAssigned(ConsTask)
                Return True
            Else
                setNotAssigned()
                Return False
            End If
        Else
            setNotAssigned()
            Return False
        End If
    End Function

    Protected Sub setNotAssigned()
        DO1.Value("Assigned") = "Not Assigned"

        DO1.setVisibility("TASKID", False)
        DO1.setVisibility("CONSOLIDATEID", False)
        DO1.setVisibility("CONTAINERID", False)
        DO1.setVisibility("HANDLINGUNITTYPE", False)
        DO1.setVisibility("USAGETYPE", False)
        DO1.setVisibility("SERIAL", False)
        DO1.setVisibility("NUMLOADS", False)
        DO1.setVisibility("UNITS", False)
    End Sub

    Protected Sub setAssigned(ByVal constask As ConsolidationTask)
        DO1.Value("Assigned") = "Assigned"

        Dim cons As Consolidation = constask.ConsolidationObject

        DO1.setVisibility("TASKID", True)
        DO1.setVisibility("CONSOLIDATEID", True)
        DO1.setVisibility("CONTAINERID", True)
        DO1.setVisibility("HANDLINGUNITTYPE", True)
        DO1.setVisibility("USAGETYPE", True)
        DO1.setVisibility("SERIAL", True)
        DO1.setVisibility("NUMLOADS", True)
        DO1.setVisibility("UNITS", True)

        DO1.Value("TASKID") = constask.TASK
        DO1.Value("CONSOLIDATEID") = cons.ConsolidateId
        DO1.Value("CONTAINERID") = cons.ToContainer
        DO1.Value("HANDLINGUNITTYPE") = cons.HandlingUnitType
        DO1.Value("USAGETYPE") = cons.UsageType
        DO1.Value("SERIAL") = cons.Serial
        DO1.Value("NUMLOADS") = cons.Count
        DO1.Value("UNITS") = cons.Units
    End Sub

    Private Sub doNext()
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser
        If Not WMS.Logic.TaskManager.isAssigned(UserId, WMS.Lib.TASKTYPE.CONSOLIDATION, WMS.Logic.LogHandler.GetRDTLogger()) Then
            Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
            Try
                Dim tm As New WMS.Logic.TaskManager
                tm.RequestTask(UserId, WMS.Lib.TASKTYPE.CONSOLIDATION)
                CheckAssigned()
            Catch ex As Made4Net.Shared.M4NException
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Catch ex As Exception
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate(ex.Message))
            End Try
        Else
            Response.Redirect(MapVirtualPath("Screens/CONTASK1.aspx"))
        End If
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddLabelLine("ASSIGNED")
        DO1.AddLabelLine("TASKID")
        DO1.AddLabelLine("CONSOLIDATEID")
        DO1.AddLabelLine("CONTAINERID")
        DO1.AddLabelLine("HANDLINGUNITTYPE")
        DO1.AddLabelLine("USAGETYPE")
        DO1.AddLabelLine("SERIAL")
        DO1.AddLabelLine("NUMLOADS")
        DO1.AddLabelLine("UNITS")
        DO1.AddSpacer()
    End Sub

    Private Sub doMenu()
        If Not Session("ConsolidationTask") Is Nothing Then
            Dim tm As Task = Session("ConsolidationTask")
            Try
                tm.ExitTask()
            Catch ex As Exception
            End Try
        End If
        Session.Remove("ConsolidationTask")
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "next"
                doNext()
            Case "menu"
                doMenu()
        End Select
    End Sub
End Class