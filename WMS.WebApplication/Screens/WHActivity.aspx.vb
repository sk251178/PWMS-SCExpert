Imports WMS.Logic

Public Class WHActivity
    Inherits System.Web.UI.Page


#Region "ViewState"

    Protected Overrides Function LoadPageStateFromPersistenceMedium() As Object
        Return WMS.WebCtrls.WebCtrls.Screen.LoadPageStateFromPersistenceMedium()
    End Function

    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        WMS.WebCtrls.WebCtrls.Screen.SavePageStateToPersistenceMedium(viewState, Me.Page, Screen1.ScreenID)
    End Sub

#End Region


#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents TEUSERACTIVITY As Made4Net.WebControls.TableEditor

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
        'Put user code to initialize the page here
    End Sub

#Region "CTor"

    Public Sub New()
    End Sub

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As New DataSet
        Dim dr As DataRow
        Dim UserId As String = Common.GetCurrentUser()
        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        Select Case CommandName.ToLower
            Case "disconnect"
                For Each dr In ds.Tables(0).Rows
                    WMS.Logic.WHActivity.Delete(dr("userid"))
                Next
        End Select
    End Sub

#End Region

    Private Sub TEUSERACTIVITY_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEUSERACTIVITY.CreatedChildControls
        With TEUSERACTIVITY.ActionBar
            .AddSpacer()
            .AddExecButton("disconnect", "Disconnect User", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnPlanWave"))
            With .Button("disconnect")
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.WHActivity"
                .EnabledInMode = Made4Net.WebControls.TableEditorMode.View Or Made4Net.WebControls.TableEditorMode.Edit Or Made4Net.WebControls.TableEditorMode.Grid
            End With
        End With
    End Sub

End Class