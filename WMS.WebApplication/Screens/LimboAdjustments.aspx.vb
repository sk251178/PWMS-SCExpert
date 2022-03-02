Imports WMS.Logic
Imports Made4Net.Shared
Imports Made4Net.DataAccess

Public Class LimboAdjustments
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents lblAdjReasonCode As Made4Net.WebControls.FieldLabel
    Protected WithEvents ddReasonCode As Made4Net.WebControls.DropDownList
    Protected WithEvents pnlAdj As System.Web.UI.WebControls.Panel
    <CLSCompliant(False)> Protected WithEvents TS As Made4Net.WebControls.TabStrip
    Protected WithEvents DC1 As Made4Net.WebControls.DataConnector
    Protected WithEvents DTC As Made4Net.WebControls.DataTabControl
    Protected WithEvents Table1 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents Tbl As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents TELimboSku As Made4Net.WebControls.TableEditor
    Protected WithEvents TELimboSkuLoad As Made4Net.WebControls.TableEditor

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
        Dim UserId As String = Common.GetCurrentUser()
        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        Select Case CommandName
            Case "SubmitAdjustmentLimboBySku"
                For Each dr In ds.Tables(0).Rows
                    Dim sSku As String = dr("sku")
                    Dim adjReason As String = dr("REASONCODE")
                    Dim sql As String = String.Format("select * from invload where sku='{0}' and status = 'LIMBO'", sSku)
                    Dim dtLoads As New DataTable
                    DataInterface.FillDataset(sql, dtLoads)
                    For Each drload As DataRow In dtLoads.Rows
                        Dim tmpLoad As New WMS.Logic.Load(drload("loadid").ToString())
                        tmpLoad.AdjustLimbo(adjReason, Common.GetCurrentUser)
                    Next
                Next
            Case "SubmitMergeLimboBySku"
                For Each dr In ds.Tables(0).Rows
                    Dim sSku As String = dr("sku")
                    Dim adjReason As String = dr("REASONCODE")
                    Dim sql As String = String.Format("select * from invload where sku='{0}' and status = 'LIMBO'", sSku)
                    Dim dtLoads As New DataTable
                    DataInterface.FillDataset(sql, dtLoads)
                    If dtLoads.Rows.Count <= 1 Then
                        Throw New Made4Net.Shared.M4NException(New Exception, "Please select 2 loads (at least) to merge", "Please select 2 loads (at least) to merge")
                    End If
                    'Take the first load and merge it with the rest
                    Dim oLoad As New WMS.Logic.Load(dtLoads.Rows(0)("loadid").ToString())
                    For Each drload As DataRow In dtLoads.Rows
                        If drload("loadid") <> oLoad.LOADID Then
                            Dim tmpLoad As New WMS.Logic.Load(drload("loadid").ToString())
                            oLoad.Merge(tmpLoad)
                        End If
                    Next
                Next
            Case "SubmitAdjustmentLimbo"
                For Each dr In ds.Tables(0).Rows
                    Dim adjReason As String = dr("REASONCODE")
                    Dim tmpLoad As New WMS.Logic.Load(dr("loadid").ToString())
                    tmpLoad.AdjustLimbo(adjReason, Common.GetCurrentUser)
                Next
            Case "SubmitMergeLimbo"
                If ds.Tables(0).Rows.Count <= 1 Then
                    Throw New Made4Net.Shared.M4NException(New Exception, "Please select 2 loads (at least) to merge", "Please select 2 loads (at least) to merge")
                End If
                Dim oLoad As New WMS.Logic.Load(ds.Tables(0).Rows(0)("loadid").ToString())
                For Each dr In ds.Tables(0).Rows
                    'Take the first load and merge it with the rest
                    If dr("loadid") <> oLoad.LOADID Then
                        Dim tmpLoad As New WMS.Logic.Load(dr("loadid").ToString())
                        oLoad.Merge(tmpLoad)
                    End If
                Next
        End Select
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not IsPostBack Then
            ddReasonCode.DataBind()
            ddReasonCode_SelectedIndexChanged(Nothing, Nothing)
        End If
    End Sub

    Private Sub TELimboSku_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TELimboSku.CreatedChildControls
        With TELimboSku.ActionBar
            .AddSpacer()
            .AddExecButton("SubmitAdjustment", "Adjust", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnPlanWave"))
            With .Button("SubmitAdjustment")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.LimboAdjustments"
                .CommandName = "SubmitAdjustmentLimboBySku"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .ConfirmRequired = True
                .ConfirmMessage = WMS.Logic.Utils.TranslateMessage("Are you sure you want to adjust the loads?", Nothing)
            End With
            .AddSpacer()
            .AddExecButton("SubmitMerge", "Merge", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnReleaseWave"))
            With .Button("SubmitMerge")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.LimboAdjustments"
                .CommandName = "SubmitMergeLimboBySku"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .ConfirmRequired = True
                .ConfirmMessage = WMS.Logic.Utils.TranslateMessage("Are you sure you want to merge the loads?", Nothing)
            End With
        End With
    End Sub

    Private Sub TELimboSkuLoad_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TELimboSkuLoad.CreatedChildControls
        With TELimboSkuLoad.ActionBar
            .AddSpacer()
            .AddExecButton("SubmitAdjustmentLoads", "Adjust", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnPlanWave"))
            With .Button("SubmitAdjustmentLoads")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.LimboAdjustments"
                .CommandName = "SubmitAdjustmentLimbo"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .ConfirmRequired = True
                .ConfirmMessage = WMS.Logic.Utils.TranslateMessage("Are you sure you want to adjust the loads?", Nothing)
            End With
            .AddSpacer()
            .AddExecButton("SubmitMergeLoads", "Merge", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnReleaseWave"))
            With .Button("SubmitMergeLoads")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.LimboAdjustments"
                .CommandName = "SubmitMergeLimbo"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .ConfirmRequired = True
                .ConfirmMessage = WMS.Logic.Utils.TranslateMessage("Are you sure you want to merge the loads?", Nothing)
            End With
        End With
    End Sub

    Private Sub TELimboSku_AfterItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles TELimboSku.AfterItemCommand
        'If e.CommandName = "SubmitAdjustment" Then
        TELimboSku.RefreshData()
        'End If
    End Sub

    Private Sub TELimboSkuLoad_AfterItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles TELimboSkuLoad.AfterItemCommand
        'If e.CommandName = "SubmitAdjustment" Then
        TELimboSkuLoad.RefreshData()
        ' End If
    End Sub

    Private Sub ddReasonCode_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddReasonCode.SelectedIndexChanged
        Dim vals As New Specialized.NameValueCollection
        vals.Add("REASONCODE", ddReasonCode.SelectedValue)
        TELimboSku.PreDefinedValues = vals
        TELimboSkuLoad.PreDefinedValues = vals
    End Sub
End Class