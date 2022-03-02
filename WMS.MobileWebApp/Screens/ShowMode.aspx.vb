Imports Made4Net.Mobile

<CLSCompliant(False)> Public Class ShowMode
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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Request.QueryString("modeid") Is Nothing OrElse Request.QueryString("modeid").Length = 0 Then
            GoToModeScreen()
        End If

        'Response.Write("<b>Mode: " & Common.GetCurrentModeName() & "</b><br><br><u>Available Screens:</u><br>")
        'Dim DT As DataTable = Common.GetScreenData()
        'Dim DR As DataRow
        'For Each DR In DT.Rows
        '	Response.Write(String.Format("{0} - {1}<br>", DR("KEYSEQ"), DR("SCREENNAME")))
        'Next

    End Sub
    Protected Overrides Function LoadPageStateFromPersistenceMedium() As Object
        Return Session("_ViewState")
    End Function

    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        Session("_ViewState") = viewState
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        Dim n As New Made4Net.Mobile.WebCtrls.NavigationInterface
        n.ShowScreenMenu = True
        DO1.Content.Controls.Add(n)
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        'RWMS-1970 Start
        If Not Session("m4nMobile_Mode") Is Nothing Then
            Session.Remove("m4nMobile_Mode")
        End If
        'RWMS-1970 End
        Made4Net.Mobile.GoToModeScreen()
    End Sub
End Class
