Imports WMS.Logic
Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports WLTaskManager = WMS.Logic.TaskManager
Imports WMS.Lib

<CLSCompliant(False)> Public Class CONSDEL
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
            If Not WMS.Logic.TaskManager.isAssigned(WMS.Logic.Common.GetCurrentUser, WMS.Lib.TASKTYPE.CONSOLIDATIONDELIVERY, WMS.Logic.LogHandler.GetRDTLogger()) Then
                Response.Redirect(MapVirtualPath("Screens/PCK.aspx"))
            End If

            Dim delobj As DeliveryJob
            Dim deltask As ConsolidationDeliveryTask = Session("ConsDeliveryTask")

            delobj = deltask.getDeliveryJob()

            If Not delobj Is Nothing Then
                setDelivery(delobj)
                Return
            Else
                Session.Remove("ConsDeliveryTask")
                Response.Redirect(MapVirtualPath("Screens/CONTASK.aspx"))
            End If
        End If
    End Sub

    Public Sub setDelivery(ByVal delobj As DeliveryJob)
        If delobj Is Nothing Then
            Session.Remove("ConsDeliveryTask")
            Response.Redirect(MapVirtualPath("Screens/CONTASK.aspx"))
        End If

        Dim consdel As ConsolidationDeliveryTask = Session("ConsDeliveryTask")
        DO1.Value("CONTAINERID") = consdel.FromContainer
        DO1.Value("DESTINATIONLOCATION") = consdel.TOLOCATION
        DO1.Value("DESTINATIONWAREHOUSEAREA") = consdel.TOWAREHOUSEAREA

    End Sub

    Private Sub doBack()
        Response.Redirect(MapVirtualPath("screens/CONSDELPRN.aspx"))
    End Sub

    Private Sub doMenu()
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub doNext()
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser
        Dim consdel As ConsolidationDeliveryTask = Session("ConsDeliveryTask")
        Try
            Dim tm As WLTaskManager = WLTaskManager.NewTaskManagerForUserAndTaskType(UserId, TASKTYPE.CONSOLIDATIONDELIVERY, LogHandler.GetRDTLogger())
            CType(tm.Task, WMS.Logic.ConsolidationDeliveryTask).Deliver(DO1.Value("CONFIRM"), DO1.Value("DESTINATIONWAREHOUSEAREA"))
        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Return
        Catch ex As Exception
            Return
        End Try
        Response.Redirect(MapVirtualPath("screens/CONTASK.aspx"))
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddLabelLine("CONTAINERID")
        DO1.AddLabelLine("DESTINATIONLOCATION")
        DO1.AddLabelLine("DESTINATIONWAREHOUSEAREA")
        DO1.AddSpacer()
        DO1.AddTextboxLine("CONFIRM")
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