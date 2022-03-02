Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports WMS.Logic

<CLSCompliant(False)> Public Class MOVCNT1
    Inherits PWMSRDTBase

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.MobileWebApp.WebCtrls.Screen
    Protected WithEvents DO1 As Made4Net.Mobile.WebCtrls.DataObject

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#Region "ViewState"

    Protected Overrides Function LoadPageStateFromPersistenceMedium() As Object
        Return Session("_ViewState")
    End Function

    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        Session("_ViewState") = viewState
    End Sub
#End Region

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not IsPostBack Then
            DO1.Value("CONTAINER") = Session("MoveContainerID")
            DO1.Value("TOWAREHOUSEAREA") = Warehouse.getUserWarehouseArea()
        End If
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddLabelLine("CONTAINER")
        DO1.AddTextboxLine("TOLOCATION", True, "move container")
        DO1.AddTextboxLine("TOWAREHOUSEAREA", True, "move container")
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "move container"
                doMove()
            Case "menu"
                doMenu()
        End Select
    End Sub

    Private Sub doMove()
        Dim t As Made4Net.Shared.Translation.Translator
        t = New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

        If Made4Net.DataAccess.DataInterface.ExecuteScalar(String.Format("select COUNT(1) from WAREHOUSEAREA where WAREHOUSEAREACODE = '{0}'", DO1.Value("TOWAREHOUSEAREA"))) = "0" Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Warehouse area does not exist"))
            Return
        End If

        If DO1.Value("TOLOCATION") <> "" And DO1.Value("TOWAREHOUSEAREA") <> "" Then
            Dim CheckCntSql As String = "SELECT * FROM LOCATION WHERE LOCATION='" & DO1.Value("TOLOCATION") & "' and WAREHOUSEAREA='" & DO1.Value("TOWAREHOUSEAREA") & "' "
            Dim dt As New DataTable
            Made4Net.DataAccess.DataInterface.FillDataset(CheckCntSql, dt)
            'Checking if container already exists, if not creating new one
            If dt.Rows.Count = 0 Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Location does not exist"))
                Return
            End If
        Else
            t = New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Location does not exist"))
            Return
        End If
        Try
            Dim oCont As WMS.Logic.Container = New WMS.Logic.Container(Session("MoveContainerID"), True)
            oCont.Move(DO1.Value("TOLOCATION"), "", DO1.Value("TOWAREHOUSEAREA"), WMS.Logic.Common.GetCurrentUser)
        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Return
        Catch ex As ApplicationException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.Message)
            Return
        End Try

        t = New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Container Moved"))
        Response.Redirect(MapVirtualPath("Screens/MOVCNT.aspx"))
    End Sub

    Private Sub doMenu()
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

End Class
