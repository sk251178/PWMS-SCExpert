Imports WMS.Logic
Imports System.Data
Imports System.Xml
Imports Made4Net.Shared
Imports System.Text


Public Class BatchReplenishmentTasks
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents TEBatchReplnHeader As Made4Net.WebControls.TableEditor
    Protected WithEvents DC1 As Made4Net.WebControls.DataConnector
    Protected WithEvents DTC As Made4Net.WebControls.DataTabControl
    Protected WithEvents Table1 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents Tbl As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents TEOutboundOrderLines As Made4Net.WebControls.TableEditor

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
        Return WMS.WebCtrls.WebCtrls.Screen.LoadPageStateFromPersistenceMedium()
    End Function

    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        WMS.WebCtrls.WebCtrls.Screen.SavePageStateToPersistenceMedium(viewState, Me.Page, Screen1.ScreenID)
    End Sub

#End Region

#Region "CTor"

    Public Sub New()

    End Sub

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As New DataSet
        'Dim dr As DataRow
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser()
        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)

        'Select Case CommandName.ToLower
        '    Case "replenishtask"
        '        'For Each dr In ds.Tables(0).Rows


        '        'Next
        'End Select
    End Sub

#End Region




    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Put user code to initialize the page here
    End Sub


    Private Sub TEBatchReplnHeader_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEBatchReplnHeader.CreatedChildControls
        With TEBatchReplnHeader
            With .ActionBar
                .AddExecButton("CancelBatchReplenish", "Cancel Replenish Task", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarCancelPicks"))
                With .Button("CancelBatchReplenish")
                    .ObjectDLL = "WMS.Logic.dll"
                    .ObjectName = "WMS.Logic.BatchReplenishment"
                    .CommandName = "CancelBatchReplenish"
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                    .ConfirmRequired = True
                    .ConfirmMessage = "Are you sure you want to cancel the selected Batch Replen Task?"
                End With
            End With
        End With
    End Sub

    Private Sub TEBatchReplnHeader_AfterItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles TEBatchReplnHeader.AfterItemCommand
        If e.CommandName = "CancelBatchReplenish" Then
            TEBatchReplnHeader.RefreshData()
            TEBatchReplDetailLines.RefreshData()
        End If
    End Sub

End Class