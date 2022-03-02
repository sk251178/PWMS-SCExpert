Public Class ReplPolicy
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents TEMasterRepl As Made4Net.WebControls.TableEditor
    <CLSCompliant(False)> Protected WithEvents TS As Made4Net.WebControls.TabStrip
    Protected WithEvents TEReplPolicyLines As Made4Net.WebControls.TableEditor
    Protected WithEvents DC1 As Made4Net.WebControls.DataConnector
    Protected WithEvents DTC As Made4Net.WebControls.DataTabControl
    Protected WithEvents Table1 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents Tbl As System.Web.UI.HtmlControls.HtmlTable

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region "Ctor"

    Public Sub New()

    End Sub

    Public Sub New(ByVal pSender As Object, ByVal pCommandName As String, ByVal pXMLSchema As String, ByVal pXMLData As String, ByRef pMessage As String)
        Dim ds As New DataSet
        ds = Made4Net.Shared.Util.XmlToDS(pXMLSchema, pXMLData)
        Select Case pCommandName.ToLower
            Case "deleteheader"
                Dim replPolicyObj As New WMS.Logic.ReplenishmentPolicy(ds.Tables(0).Rows(0)("policyID"))
                replPolicyObj.Delete()
        End Select
    End Sub

#End Region

#Region "ViewState"

    Protected Overrides Function LoadPageStateFromPersistenceMedium() As Object
        Return WMS.WebCtrls.WebCtrls.Screen.LoadPageStateFromPersistenceMedium()
    End Function

    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        WMS.WebCtrls.WebCtrls.Screen.SavePageStateToPersistenceMedium(viewState, Me.Page, Screen1.ScreenID)
    End Sub

#End Region


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
    End Sub

    Protected Sub TEMasterRepl_CreatedChildControls(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TEMasterRepl.CreatedChildControls
        If TEMasterRepl.CurrentMode = Made4Net.WebControls.TableEditorMode.Edit Then
            With TEMasterRepl.ActionBar.Button("Delete")
                .ObjectDLL = "WMS.WebAPP.dll"
                .ObjectName = "WMS.WebApp.ReplPolicy"
                .CommandName = "deleteheader"
                .ConfirmRequired = True
                .ConfirmMessage = "Are you sure you want to delete the selected policy?"
            End With
        End If
    End Sub
End Class