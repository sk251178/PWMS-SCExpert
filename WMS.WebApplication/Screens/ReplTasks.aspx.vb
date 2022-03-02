Imports RWMS.Logic

Public Class ReplTasks
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents TEMasterRepl As Made4Net.WebControls.TableEditor

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
        Dim _replid, _tolocation, _towarehousearea As String

        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        Select Case CommandName.ToLower
            Case "replenish"
                For Each dr In ds.Tables(0).Rows
                    Dim err As String

                    _replid = dr("REPLID")
                    Dim repl As New WMS.Logic.Replenishment(_replid)

                    Try
                        _tolocation = dr("TOLOCATION")
                        _towarehousearea = dr("TOWAREHOUSEAREA")
                        repl.Replenish(_tolocation, _towarehousearea, UserId, False)
                        AppUtil.isBackLocMoveFront(repl.ToLoad, repl.ToLocation, repl.ToWarehousearea, "", err)
                        If err <> "" Then Message += err

                    Catch m4nex As Made4Net.Shared.M4NException
                        Throw m4nex
                    Catch ex As Exception

                    End Try
                Next
        End Select
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
    End Sub

    Private Sub TEMasterRepl_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEMasterRepl.CreatedChildControls
        Dim CanReplenishFromWorkstation As String = WMS.Logic.GetSysParam("CanReplenishFromWorkstation")
        If CanReplenishFromWorkstation = "0" Then
            With TEMasterRepl.ActionBar
                .AddSpacer()
                .AddExecButton("CancelReplenish", "Cancel Replenish", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarCancelPicks"))
                With .Button("CancelReplenish")
                    .ObjectDLL = "WMS.Logic.dll"
                    .ObjectName = "WMS.Logic.Replenishment"
                    .CommandName = "CancelReplenish"
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                    .ConfirmRequired = True
                    .ConfirmMessage = "Are you sure you want to cancel the selected replenishments?"
                End With

                .AddSpacer()
                .AddExecButton("PrintWorksheet", "Print Worksheet", Made4Net.WebControls.SkinManager.GetImageURL("ReportPrint"))
                With .Button("PrintWorksheet")
                    .ObjectDLL = "WMS.Logic.dll"
                    .ObjectName = "WMS.Logic.Replenishment"
                    .CommandName = "PrintWorksheet"
                End With
            End With
        Else
            With TEMasterRepl.ActionBar
                .AddSpacer()
                .AddExecButton("Replenish", "Replenish", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarApprovePicks"))
                With .Button("Replenish")
                    .ObjectDLL = "WMS.Logic.dll"
                    .ObjectName = "WMS.Logic.Replenishment"
                    .CommandName = "Replenish"
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                    .ConfirmRequired = True
                    .ConfirmMessage = "Are you sure you want to approve the selected replenishments?"
                End With

                .AddSpacer()
                .AddExecButton("CancelReplenish", "Cancel Replenish", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarCancelPicks"))
                With .Button("CancelReplenish")
                    .ObjectDLL = "WMS.Logic.dll"
                    .ObjectName = "WMS.Logic.Replenishment"
                    .CommandName = "CancelReplenish"
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                    .ConfirmRequired = True
                    .ConfirmMessage = "Are you sure you want to cancel the selected replenishments?"
                End With

                .AddSpacer()
                .AddExecButton("PrintWorksheet", "Print Worksheet", Made4Net.WebControls.SkinManager.GetImageURL("ReportPrint"))
                With .Button("PrintWorksheet")
                    .ObjectDLL = "WMS.Logic.dll"
                    .ObjectName = "WMS.Logic.Replenishment"
                    .CommandName = "PrintWorksheet"
                End With
            End With
        End If

    End Sub
End Class