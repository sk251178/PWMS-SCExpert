Imports WMS.Logic

Public Class Wave
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents TEMasterWave As Made4Net.WebControls.TableEditor
    Protected WithEvents TEAssignOrders As Made4Net.WebControls.TableEditor
    Protected WithEvents DC1 As Made4Net.WebControls.DataConnector
    Protected WithEvents TEWaveOrders As Made4Net.WebControls.TableEditor
    <CLSCompliant(False)> Protected WithEvents TS As Made4Net.WebControls.TabStrip
    Protected WithEvents DTC As Made4Net.WebControls.DataTabControl
    Protected WithEvents Table1 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents Tbl As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents TEWaveException As Made4Net.WebControls.TableEditor
    Protected WithEvents DC2 As Made4Net.WebControls.DataConnector
    Protected WithEvents Dataconnector1 As Made4Net.WebControls.DataConnector
    Protected WithEvents TEWaveTasks As Made4Net.WebControls.TableEditor
    Protected WithEvents TEWavePicks As Made4Net.WebControls.TableEditor
    Protected WithEvents Dataconnector2 As Made4Net.WebControls.DataConnector
    Protected WithEvents CVWavePickTasks As Made4Net.WebControls.Charting.QuickChart
    Protected WithEvents Dataconnector3 As Made4Net.WebControls.DataConnector
    Protected WithEvents TEWOC As Made4Net.WebControls.TableEditor
    Protected WithEvents DCWOC As Made4Net.WebControls.DataConnector
    Protected WithEvents TEWaveOrdersSummary As Made4Net.WebControls.TableEditor
    Protected WithEvents TEWavePicksSummary As Made4Net.WebControls.TableEditor
    Protected WithEvents Dataconnector4 As Made4Net.WebControls.DataConnector

    'Added for RWMS-2599 Start
    Protected WithEvents TEExceptionHistory As Made4Net.WebControls.TableEditor
    Protected WithEvents DataConnector5 As Made4Net.WebControls.DataConnector
    'Added for RWMS-2599 End

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
        Dim t As New Made4Net.Shared.Translation.Translator()

        Select Case CommandName.ToLower
            Case "approvepicks"
                For Each dr In ds.Tables(0).Rows
                    Try
                        Dim oPickList As WMS.Logic.Picklist = New WMS.Logic.Picklist(dr("PICKLIST"))
                        Dim oPickDetail As WMS.Logic.PicklistDetail = oPickList.Lines(dr("PICKLISTLINE"))

                        Dim oAttributeCollection As AttributesCollection
                        Dim UNITS As Decimal = 0
                        Try
                            UNITS = Math.Round(Decimal.Parse(dr("UNITS")), 2)
                            If UNITS < 0 Then Throw New Exception()
                        Catch ex As Exception
                            Throw New ApplicationException(("Invalid UNITS"))
                            Exit Sub
                        End Try

                        Dim oSku As New WMS.Logic.SKU(oPickList(dr("PICKLISTLINE")).Consignee, oPickList(dr("PICKLISTLINE")).SKU)
                        If UNITS <> 0 Then
                            OutboundOrder.MainValidation(dr, oSku)
                        End If

                        If oPickDetail.Quantity - oPickDetail.PickedQuantity > UNITS Then
                            OutboundOrder.PickShort(oPickDetail, oPickDetail.Quantity - oPickDetail.PickedQuantity, UNITS)
                        End If

                        'If (OutboundOrder.weightNeeded(oSku)) Then

                        '    If Not IsDBNull(dr("WEIGHT")) And Not IsDBNull(dr("UNITS")) Then
                        '        Dim UNITSPERCASE As Decimal = Integer.Parse(dr("UNITS")) / oSku.ConvertToUnits("CASE")

                        '        Dim weight As Decimal = Math.Round(Decimal.Parse(dr("WEIGHT")), 2)

                        '        Dim weightPerCase As Decimal = Math.Round(weight / UNITSPERCASE, 2)

                        '        OutboundOrder.VALIDATEWEIGHT(dr("CONSIGNEE"), dr("SKU"), weightPerCase)

                        '        Message = OutboundOrder.updateFromLoadWeight(oPickDetail.FromLoad, weight)
                        '    Else
                        '        Throw New ApplicationException(t.Translate("please fill Units and Weight"))
                        '        Exit Sub
                        '    End If
                        'End If

                        oAttributeCollection = WMS.Logic.SkuClass.ExtractPickingAttributes(oSku.SKUClass, dr)
                        Dim oPicking As Picking = New Picking
                        oPicking.PickLine(oPickList, dr("PICKLISTLINE"), dr("UNITS"), UserId, oAttributeCollection)
                    Catch ex As Made4Net.Shared.M4NException
                        Throw ex
                    Catch ex As Exception                        
                    End Try
                Next



            Case "pick"
                For Each dr In ds.Tables(0).Rows
                    Dim oPickList As WMS.Logic.Picklist = New WMS.Logic.Picklist(dr("PICKLIST"))
                    Try
                        oPickList.Pick(UserId)
                    Catch ex As Exception
                    End Try
                Next
            Case "cancelpicklist"
                For Each dr In ds.Tables(0).Rows
                    Dim oPickList As WMS.Logic.Picklist = New WMS.Logic.Picklist(dr("PICKLIST"))
                    Try
                        oPickList.Cancel(UserId)
                    Catch ex As Exception
                    End Try
                Next
            Case "unallocatepicklist"
                For Each dr In ds.Tables(0).Rows
                    Dim oPickList As WMS.Logic.Picklist = New WMS.Logic.Picklist(dr("PICKLIST"))
                    Try
                        oPickList.unAllocate(UserId)
                    Catch ex As Exception
                    End Try
                Next
            Case "cancelpicks"
                For Each dr In ds.Tables(0).Rows
                    Try
                        Dim oPickList As WMS.Logic.Picklist = New WMS.Logic.Picklist(dr("PICKLIST"))
                        oPickList.CancelLine(dr("PICKLISTLINE"), UserId)
                    Catch ex As Exception
                    End Try
                Next
            Case "unallocatepicks"

                For Each dr In ds.Tables(0).Rows
                    Try
                        Dim oPickList As WMS.Logic.Picklist = New WMS.Logic.Picklist(dr("PICKLIST"))
                        oPickList.unAllocateLine(dr("PICKLISTLINE"), UserId)
                    Catch ex As Exception
                    End Try
                Next
            Case "unpick"
                For Each dr In ds.Tables(0).Rows
                    Try
                        Dim oPickList As WMS.Logic.Picklist = New WMS.Logic.Picklist(dr("PICKLIST"))
                        oPickList.UnPickLine(dr("PICKLISTLINE"), UserId)
                    Catch ex As Exception
                    End Try
                Next

            Case "cancellineexception"
                Dim ORDERID, Consignee As String
                Dim outordet As OutboundOrderHeader.OutboundOrderDetail
                For Each dr In ds.Tables(0).Rows
                    outordet = New OutboundOrderHeader.OutboundOrderDetail(dr("CONSIGNEE"), dr("ORDERID"), dr("ORDERLINE"))
                    outordet.CancelExceptions(Common.GetCurrentUser)
                    WebAppUtil.UpdateShipmentDetail(dr("CONSIGNEE"), dr("ORDERID"), dr("ORDERLINE"), outordet.QTYMODIFIED)
                    ' WebAppUtil.CancelOrder(dr("CONSIGNEE"), dr("ORDERID"))
                    If ORDERID <> dr("ORDERID") Or Consignee = dr("CONSIGNEE") Then
                        WebAppUtil.CompleteOrder(dr("CONSIGNEE"), dr("ORDERID"))
                    End If

                    ORDERID = dr("ORDERID")
                    Consignee = dr("CONSIGNEE")

                Next

                WebAppUtil.CompleteOrder(dr("CONSIGNEE"), dr("ORDERID"))

            Case "cancelexception"
                'Dim wv As WMS.Logic.Wave
                For Each dr In ds.Tables(0).Rows
                    WebAppUtil.processOrderExceptionLines(dr("WAVE"))
                    WebAppUtil.processCompleteOrder(dr("WAVE"))
                    ' wv = New WMS.Logic.Wave(dr("WAVE"))
                    ' wv.CancelExceptions(Common.GetCurrentUser)
                Next

            Case "setpririty"
                Dim w As New WMS.Logic.Wave(Sender, CommandName, XMLSchema, XMLData, Message)

                For Each dr In ds.Tables(0).Rows
                    Dim task As String = dr("task")
                    'RWMS-823 Commented Start
                    'UserId = dr("userid")
                    'RWMS-823 Commented End

                    'RWMS-823 Added Start
                    UserId = Convert.ToString(dr("userid"))
                    'RWMS-823 Added End

                    Dim sql As String = String.Format("update TASKS set USERID = '{0}' where TASK = '{1}'", UserId, task)
                    Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)

                Next
            Case "plan"
                For Each dr In ds.Tables(0).Rows
                    Dim Sql As String
                    Sql = " select count(1) "
                    Sql += " FROM         dbo.OUTBOUNDORHEADER AS oh INNER JOIN "
                    Sql += "              dbo.WAVEDETAIL AS wd ON oh.CONSIGNEE = wd.CONSIGNEE AND oh.ORDERID = wd.ORDERID "
                    Sql += " WHERE  wd.WAVE='{1}'  and (oh.STATUS = 'LOADING')  "
                    Sql = String.Format(Sql, WMS.Logic.GetCurrentUser, dr("WAVE"))

                    Sql = Made4Net.DataAccess.DataInterface.ExecuteScalar(Sql)

                    If Sql <> "0" Then

                        Sql = " update OUTBOUNDORHEADER "
                        Sql += " set STATUS ='PLANNED', EDITDATE = GETDATE(), EDITUSER = '{0}' "
                        Sql += " FROM         dbo.OUTBOUNDORHEADER AS oh INNER JOIN "
                        Sql += "              dbo.WAVEDETAIL AS wd ON oh.CONSIGNEE = wd.CONSIGNEE AND oh.ORDERID = wd.ORDERID "
                        Sql += " WHERE  wd.WAVE='{1}'  and (oh.STATUS = 'LOADING')  "
                        Sql = String.Format(Sql, WMS.Logic.GetCurrentUser, dr("WAVE"))

                        Made4Net.DataAccess.DataInterface.RunSQL(Sql)
                    End If

                Next
                Dim w As New WMS.Logic.Wave(Sender, CommandName, XMLSchema, XMLData, Message)
            Case "complete"
                Dim wave As String
                For Each dr In ds.Tables(0).Rows
                    wave = dr("WAVE")
                    ' CompleteOrderLines(wave)
                    CompleteWave.CompletePicklist(wave)
                    CompleteWave.CompleteOrder(wave)
                Next
                Dim obj As WMS.Logic.Wave = New WMS.Logic.Wave(Sender, CommandName, XMLSchema, XMLData, Message)

        End Select
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub TEMasterWave_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEMasterWave.CreatedChildControls
        With TEMasterWave.ActionBar
            .AddSpacer()
            .AddExecButton("AssignDelLoc", "Assign Delivery Location", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnPlanWave"))
            'Commented for PWMS-529 Start
            '.AddSpacer()
            'Commented for PWMS-529 End
            .AddExecButton("plan", "Plan Wave", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnPlanWave"))
            .AddExecButton("softpaln", "Soft Plan", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnSoftPlanWave"))

            .AddExecButton("release", "Release Wave", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnReleaseWave"))
            'Added for PWMS-529 Start
            .AddExecButton("PrintPickList", "Print Picklist", Made4Net.WebControls.SkinManager.GetImageURL("ReportPrint"))
            .AddSpacer()
            'Added for PWMS-529 End
            .AddExecButton("complete", "Complete Wave", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnCompleteWave"))
            'Commented for PWMS-529 Start
            '.AddExecButton("PrintPickList", "Print Picklist", Made4Net.WebControls.SkinManager.GetImageURL("ReportPrint"))
            'Commented for PWMS-529 End
            With .Button("Save")
                .ObjectDLL = "WMS.Logic.dll"
                .ObjectName = "WMS.Logic.Wave"
            End With
            With .Button("AssignDelLoc")
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.Wave"
                .ConfirmRequired = True
                .ConfirmMessage = "Are you sure you want to assign delivery location for selected wave/s?"
                .EnabledInMode = Made4Net.WebControls.TableEditorMode.View Or Made4Net.WebControls.TableEditorMode.Edit Or Made4Net.WebControls.TableEditorMode.Grid
            End With
            With .Button("plan")
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.Wave"
                ''.EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
                '.ObjectDLL = "WMS.Logic.dll"
                '.ObjectName = "WMS.Logic.Wave"
                ''.CommandName = "plan"
                .EnabledInMode = Made4Net.WebControls.TableEditorMode.View Or Made4Net.WebControls.TableEditorMode.Edit Or Made4Net.WebControls.TableEditorMode.Grid
            End With

            With .Button("softpaln")
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                '.EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
                .ObjectDLL = "WMS.Logic.dll"
                .ObjectName = "WMS.Logic.Wave"
                .EnabledInMode = Made4Net.WebControls.TableEditorMode.View Or Made4Net.WebControls.TableEditorMode.Edit Or Made4Net.WebControls.TableEditorMode.Grid
            End With
            With .Button("release")
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
                .ObjectDLL = "WMS.Logic.dll"
                .ObjectName = "WMS.Logic.Wave"
                '.CommandName = "release"
            End With
            'Added for PWMS-529 Start
            With .Button("PrintPickList")
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
                .ObjectDLL = "WMS.Logic.dll"
                .ObjectName = "WMS.Logic.Wave"
                .CommandName = "PrintPickList"
            End With
            'Added for PWMS-529 End
            With .Button("complete")
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.Wave"
                '.ObjectDLL = "WMS.Logic.dll"
                '.ObjectName = "WMS.Logic.Wave"
                'Added for PWMS-529 Start
                .CommandName = "complete"
                .ConfirmRequired = True
                .ConfirmMessage = "Are you sure you want to complete the wave?"
                'End for PWMS-529 End
            End With
            'Commented for PWMS-529 Start
            'With .Button("PrintPickList")
            '    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
            '    .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
            '    .ObjectDLL = "WMS.Logic.dll"
            '    .ObjectName = "WMS.Logic.Wave"
            '    .CommandName = "PrintPickList"
            'End With
            'Commented for PWMS-529 Start
            'Added by priel
            .AddExecButton("CancelWave", "Cancel Wave", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarCancelOrders"))
            With .Button("CancelWave")
                .ObjectDLL = "WMS.Logic.dll"
                .ObjectName = "WMS.Logic.Wave"
                .CommandName = "CancelWave"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .ConfirmRequired = True
                .ConfirmMessage = "Are you sure you want to cancel the wave?"
            End With

        End With
    End Sub

    Private Sub TEAssignOrders_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEAssignOrders.CreatedChildControls
        TEAssignOrders.ActionBar.AddSpacer()
        TEAssignOrders.ActionBar.AddExecButton("AssignOrders", "Assign Order Lines To Wave", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnMultiEdit"))
        With TEAssignOrders.ActionBar.Button("AssignOrders")
            .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
            .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
            .ObjectDLL = "WMS.Logic.dll"
            .ObjectName = "WMS.Logic.Wave"
        End With
    End Sub

    Private Sub TEWaveOrders_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEWaveOrders.CreatedChildControls
        With TEWaveOrders.ActionBar.Button("Save")
            .ObjectDLL = "WMS.Logic.dll"
            .ObjectName = "WMS.Logic.OutboundOrderHeader"
            .CommandName = "SetStagingLane"
        End With
        TEWaveOrders.ActionBar.AddSpacer()
        TEWaveOrders.ActionBar.AddExecButton("DeAssignOrders", "UnAssign Details From Wave", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnMultiEdit"))
        With TEWaveOrders.ActionBar.Button("DeAssignOrders")
            .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
            .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
            .ObjectDLL = "WMS.Logic.dll"
            .ObjectName = "WMS.Logic.Wave"
        End With
    End Sub

    Private Sub TEMasterWave_RecordSelected(ByVal sender As Made4Net.WebControls.TableEditor, ByVal e As Made4Net.WebControls.TableEditorEventArgs) Handles TEMasterWave.RecordSelected
        Dim tds As DataTable = TEMasterWave.CreateDataTableForSelectedRecord()
        Dim vals As New Specialized.NameValueCollection
        vals.Add("WAVEID", tds.Rows(0)("WAVE"))
        TEAssignOrders.PreDefinedValues = vals
        'TS_SelectedIndexChange(Nothing, Nothing)
    End Sub

    Private Sub TESWaveOrders_AfterItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles TEWaveOrders.AfterItemCommand
        If e.CommandName = "DeAssignOrders" Then
            TEWaveOrders.RefreshData()
            TEMasterWave.RefreshData()
        End If
    End Sub

    Private Sub TEAssignOrders_AfterItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles TEAssignOrders.AfterItemCommand
        If e.CommandName = "AssignOrders" Then
            TEAssignOrders.RefreshData()
            TEMasterWave.RefreshData()
        End If
    End Sub

    Private Sub TEMasterWave_AfterItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles TEMasterWave.AfterItemCommand
        If e.CommandName = "plan" Or e.CommandName = "release" Or e.CommandName = "complete" Then
            TEMasterWave.RefreshData()
        End If
        'If TEMasterWave.Data Is Nothing Then
        '    If TEMasterWave.CurrentMode = Made4Net.WebControls.TableEditorMode.Grid Then
        '        TEMasterWave.Restart()
        '    End If
        'ElseIf TEMasterWave.Data.Rows.Count = 0 Then
        '    TEMasterWave.Restart()
        'End If
    End Sub

    Private Sub TEWavePicks_CreatedChildControls(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TEWavePicks.CreatedChildControls
        With TEWavePicks
            With .ActionBar
                .AddSpacer()
                .AddExecButton("ApprovePicks", "Approve Picks", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarApprovePicks"))
                With .Button("ApprovePicks")
                    '.ObjectDLL = "WMS.WebApp.dll"
                    '.ObjectName = "WMS.WebApp.Wave"
                    .ObjectName = "WMS.WebApp.Wave"
                    .ObjectDLL = "WMS.WebApp.dll"
                    .CommandName = "ApprovePicks"
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                    .ConfirmRequired = True
                    .ConfirmMessage = "Are you sure you want to pick the selected picks?"
                End With

                .AddExecButton("CancelPicks", "Cancel Picks", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarCancelPicks"))
                With .Button("CancelPicks")
                    .ObjectDLL = "WMS.WebApp.dll"
                    .ObjectName = "WMS.WebApp.Wave"
                    .CommandName = "CancelPicks"
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                    .ConfirmRequired = True
                    .ConfirmMessage = "Are you sure you want to cancel the selected picks?"
                End With

                .AddExecButton("unallocatePicks", "Unallocate Picks", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarUnAllocatePicks"))
                With .Button("unallocatePicks")
                    .ObjectDLL = "WMS.WebApp.dll"
                    .ObjectName = "WMS.WebApp.Wave"
                    .CommandName = "unallocatePicks"
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                    .ConfirmRequired = True
                    .ConfirmMessage = "Are you sure you want to unallocated the selected picks?"
                End With

                '.AddExecButton("unPick", "Undo Pick", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarUnAllocatePicks"))
                'With .Button("unPick")
                '    .ObjectDLL = "WMS.WebApp.dll"
                '    .ObjectName = "WMS.WebApp.Wave"
                '    .CommandName = "unpick"
                '    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                '    .ConfirmRequired = True
                '    .ConfirmMessage = "Are you sure you want to undo the selected picks?"
                '    '.EnabledInMode = Made4Net.WebControls.TableEditorMode.View Or Made4Net.WebControls.TableEditorMode.Edit
                'End With

            End With
        End With
    End Sub

    Private Sub TEWaveTasks_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEWaveTasks.CreatedChildControls
        With TEWaveTasks
            With .ActionBar.Button("Save")
                '.ObjectDLL = "WMS.Logic.dll"
                '.ObjectName = "WMS.Logic.Wave"

                If TEWaveTasks.CurrentMode = Made4Net.WebControls.TableEditorMode.MultilineEdit Then
                    .ObjectDLL = "WMS.WebApp.dll"
                    .ObjectName = "WMS.WebApp.Wave"
                    .CommandName = "setpririty"
                End If
            End With
        End With
    End Sub

    Private Sub TEWaveTasks_AfterItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs)
        TEWaveTasks.RefreshData()
    End Sub

    'Private Sub TS_SelectedIndexChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles TS.SelectedIndexChange
    '    If TS.SelectedIndex = 5 Then
    '        DCWOC.Shake()
    '        TEWOC.Restart()
    '        TEWOC.RefreshData()
    '        TEWOC.PrepareQuickChartParams()
    '        HttpContext.Current.Session("M4NQuickChartTEId") = "/WMSBaseLine/screens/wave.aspx__" & TEWOC.UniqueID
    '        Session.Add("QuickChartId", String.Format("{0}__{1}", Screen1.ScreenID, TEWOC.ObjectID))
    '        CVWavePickTasks.LoadSettings()
    '    End If
    'End Sub

    Private Sub TEWaveException_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEWaveException.CreatedChildControls
        With TEWaveException
            With .ActionBar
                .AddSpacer()
                .AddExecButton("CancelException", "Cancel Order Exception", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarCancelPicks"))
                With .Button("CancelException")
                    .ObjectDLL = "WMS.WebApp.dll"
                    .ObjectName = "WMS.WebApp.Wave"
                    .CommandName = "CancelLineException"
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                    .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
                    .ConfirmRequired = True
                    .ConfirmMessage = "Are you sure you want to cancel the order's exceptions?"
                End With
            End With
        End With
    End Sub

    Protected Sub TEExceptionHistory_CreatedChildControls(sender As Object, e As EventArgs) Handles TEExceptionHistory.CreatedChildControls

    End Sub
End Class
