Imports RWMS.Logic

Public Class InventoryMove
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents TEINVLOADMOVE As Made4Net.WebControls.TableEditor

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
        Dim dr As DataRow
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser()
        Dim _loadid As String

        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)

        Select Case CommandName.ToLower
            Case "submitmove"
                For Each dr In ds.Tables(0).Rows
                    Dim err As String

                    _loadid = dr("LOADID")
                    Dim ld As New WMS.Logic.Load(_loadid)

                    If Not AppUtil.ChangeLocationValidation(dr("TOLOCATION"), WMS.Logic.Warehouse.getUserWarehouseArea(), ld.CONSIGNEE, ld.SKU, err) Then
                        If err <> "" Then Message += err
                        Throw New ApplicationException(Message)
                    End If

                    ld.Move(dr("TOLOCATION"), WMS.Logic.Warehouse.getUserWarehouseArea(), "", WMS.Logic.Common.GetCurrentUser)
                    AppUtil.isBackLocMoveFront(_loadid, dr("TOLOCATION"), WMS.Logic.Warehouse.getUserWarehouseArea(), "", err)
                    If err <> "" Then Message += err

                Next
        End Select
    End Sub

#End Region


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
    End Sub

    Private Sub TEINVLOADMOVE_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEINVLOADMOVE.CreatedChildControls
        With TEINVLOADMOVE.ActionBar
            .AddSpacer()

            .AddExecButton("SubmitMove", "Move", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnMultiEdit"))
            With .Button("SubmitMove")
                '.ObjectDLL = "WMS.Logic.dll"
                '.ObjectName = "WMS.Logic.Load"
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.InventoryMove"
                .CommandName = "SubmitMove"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .ConfirmRequired = True
                .ConfirmMessage = "Are you sure you want to move selected loads?"
            End With
        End With
    End Sub

    Private Sub TEINVLOADMOVE_AfterItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles TEINVLOADMOVE.AfterItemCommand
        If e.CommandName = "SubmitMove" Then
            TEINVLOADMOVE.RefreshData()
        End If
    End Sub
End Class