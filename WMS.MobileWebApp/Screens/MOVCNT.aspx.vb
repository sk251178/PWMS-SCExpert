Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports WMS.Logic

<CLSCompliant(False)> Public Class MOVCNT
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
        'Put user code to initialize the page here
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddTextboxLine("LOADID")
        DO1.AddTextboxLine("CONTAINERID")
        DO1.AddTextboxLine("LOCATION")
        'DO1.AddTextbox Line("WAREHOUSEAREA") 
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "next"
                doMove()
            Case "menu"
                doMenu()
        End Select
    End Sub

    Private Sub doMove()
        Session("MoveContainerID") = ""

        If DO1.Value("LOCATION") <> "" Then 'Or _
            'DO1.Value("WAREHOUSEAREA") <> "" Then
            Dim CheckCntSql As String = "SELECT * FROM LOCATION WHERE LOCATION='" & DO1.Value("LOCATION") & "' and WAREHOUSEAREA='" & Warehouse.getUserWarehouseArea() & "'"
            Dim dt As New DataTable
            Made4Net.DataAccess.DataInterface.FillDataset(CheckCntSql, dt)
            'Checking if container already exists, if not creating new one
            If dt.Rows.Count = 0 Then
                Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Location does not exist"))
                Return
            End If
        End If

        If DO1.Value("CONTAINERID") <> "" Then
            Dim CheckCntSql As String = "SELECT * FROM CONTAINER WHERE CONTAINER='" & DO1.Value("CONTAINERID") & "'"
            Dim dt As New DataTable
            Made4Net.DataAccess.DataInterface.FillDataset(CheckCntSql, dt)
            'Checking if container already exists, if not creating new one
            If dt.Rows.Count > 0 Then
                Session("MoveContainerID") = DO1.Value("CONTAINERID")
            Else
                Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Container does not exist"))
                Return
            End If
        End If

        If DO1.Value("LOADID") <> "" And Session("MoveContainerID") = "" Then
            Dim CheckCntSql As String = "SELECT * FROM CONTAINERLOADS WHERE LOADID='" & DO1.Value("LOADID") & "'"
            Dim dt As New DataTable
            Made4Net.DataAccess.DataInterface.FillDataset(CheckCntSql, dt)
            If dt.Rows.Count > 0 Then
                Session("MoveContainerID") = dt.Rows(0)("CONTAINERID")
            Else
                Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Container does not exist"))
                Return
            End If
        End If

        If DO1.Value("LOCATION") <> "" And _
                Session("MoveContainerID") = "" Then
            Dim ChkFromLocSql As String = "SELECT * FROM CONTAINER WHERE LOCATION='" & DO1.Value("LOCATION") & "' and WAREHOUSEAREA='" & Warehouse.getUserWarehouseArea() & "'"
            Dim chkdt As New DataTable
            Made4Net.DataAccess.DataInterface.FillDataset(ChkFromLocSql, chkdt)
            'Checking if container already exists, if not creating new one
            If chkdt.Rows.Count = 1 Then
                Session("MoveContainerID") = chkdt.Rows(0)("CONTAINER")
            Else
                If chkdt.Rows.Count > 1 Then
                    Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("More then one container in location"))
                    Return
                Else
                    Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("No containers in Location"))
                    Return
                End If
            End If
        End If

        If Session("MoveContainerID") <> "" Then
            Dim contObj As New WMS.Logic.Container(Session("MoveContainerID"), True)
            For Each loadObj As WMS.Logic.Load In contObj.Loads
                If loadObj.hasActivity Then
                    Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Can not move container. One or more loads assigned to another activity."))
                    Return
                End If
            Next
            Response.Redirect(MapVirtualPath("Screens/MOVCNT1.aspx"))
        Else
            Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("No container selected for moving"))
            Return
        End If
    End Sub

    Private Sub doMenu()
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

End Class
