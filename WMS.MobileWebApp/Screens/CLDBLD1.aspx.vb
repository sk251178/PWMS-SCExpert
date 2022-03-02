Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports Made4Net.Shared
Imports Wms.Logic

<CLSCompliant(False)> Public Class CLDBLD1
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
            If Session("CreateLoadReciptId") Is Nothing Then
                Response.Redirect(MapVirtualPath("Screens/CLDBLD.aspx"))
            End If

            DO1.Value("CONTAINERID") = Session("CreateLoadContainerID")
            If (Session("CreateLoadLocation") = "" Or Session("CreateLoadWarehousearea") = "") Then
                If WMS.Logic.Consignee.Exists(Session("CreateLoadConsignee")) Then
                    Dim oCon As New WMS.Logic.Consignee(Session("CreateLoadConsignee"))
                    DO1.Value("LOCATION") = oCon.DEFAULTRECEIVINGLOCATION
                    DO1.Value("WAREHOUSEAREA") = oCon.DEFAULTRECEIVINGWAREHOUSEAREA
                End If
            Else
                DO1.Value("LOCATION") = Session("CreateLoadLocation")
                DO1.Value("WAREHOUSEAREA") = Session("CreateLoadWarehousearea")
            End If
        End If
    End Sub

    Private Sub doMenu()
        MobileUtils.ClearBlindReceivingSession()
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub doNext()
        'If Page.IsValid Then
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Try
            Dim oCon As New WMS.Logic.Consignee(Session("CreateLoadConsignee"))
            Session("CreateLoadLocation") = DO1.Value("LOCATION")
            Session("CreateLoadWarehousearea") = DO1.Value("WAREHOUSEAREA")
            ' Handling Container 
            If DO1.Value("CONTAINERID") <> "" Then
                Dim CheckCntSql As String = "SELECT * FROM CONTAINER WHERE CONTAINER='" & DO1.Value("CONTAINERID") & "'"
                Dim dt As New DataTable
                Made4Net.DataAccess.DataInterface.FillDataset(CheckCntSql, dt)
                'Checking if container already exists, if not creating new one
                If dt.Rows.Count > 0 Then
                    Session("CreateLoadContainerID") = DO1.Value("CONTAINERID")
                Else
                    Dim oCont As WMS.Logic.Container = New WMS.Logic.Container
                    oCont.ContainerId = DO1.Value("CONTAINERID")
                    oCont.Location = DO1.Value("LOCATION")
                    oCont.Post(WMS.Logic.Common.GetCurrentUser)
                    Session("CreateLoadContainerID") = DO1.Value("CONTAINERID")
                End If
            Else
                Session("CreateLoadContainerID") = ""
            End If
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate(ex.Message))
            Return
        End Try

        Response.Redirect(MapVirtualPath("Screens/CLDBLD2.aspx"))
        'End If
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddTextboxLine("LOCATION", True, "next")
        DO1.AddTextboxLine("CONTAINERID")
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
