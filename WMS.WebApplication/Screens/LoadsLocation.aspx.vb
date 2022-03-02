Imports RWMS.Logic
Public Class LoadsLocation
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents TEMasterLoad As Made4Net.WebControls.TableEditor

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
            Case "approvelocation"

                For Each dr In ds.Tables(0).Rows
                    Dim err As String

                    _loadid = dr("LOADID")
                    Dim ld As New WMS.Logic.Load(_loadid)

                    'If Not ((ld.ACTIVITYSTATUS = WMS.Lib.Statuses.ActivityStatus.PUTAWAYPEND) Or (ld.DESTINATIONLOCATION <> String.Empty)) Then
                    '    'throw exception
                    '    Dim m4nEx As New Made4Net.Shared.M4NException(New Exception(), "Activity Status  of Load [#param0#] is incorrect.", "Activity Status  of Load [#param0#] is incorrect.")
                    '    m4nEx.Params.Add("load", dr("LOADID"))
                    '    Throw m4nEx
                    'End If

                    If ld.STATUS = WMS.Lib.Statuses.LoadStatus.LIMBO Then
                        Throw New Made4Net.Shared.M4NException(New Exception(), "Cannot move load in status limbo", "Cannot move load in status limbo")
                    End If

                    If ld.UNITSALLOCATED > 0 Then
                        Throw New Made4Net.Shared.M4NException(New Exception(), "Cannot move load - units allocated", "Cannot move load - units allocated")
                    End If

                    If Not AppUtil.ChangeLocationValidation(dr("NewLocation"), dr("WarehouseArea"), ld.CONSIGNEE, ld.SKU, err) Then
                        If err <> "" Then Message += err
                        Throw New ApplicationException(Message)
                    End If

                    ' Release the task
                    'Dim TaskId As String = DataInterface.ExecuteScalar("SELECT TASK FROM TASKS WHERE TASKTYPE='LOADPW' AND (STATUS='ASSIGNED' OR STATUS='AVAILABLE') AND FROMLOAD='" & _loadid & "'")
                    Dim sql As String = String.Format("select task from tasks where tasktype ={0} and (status={1} or status={2}) and fromload={3}", _
                    Made4Net.Shared.FormatField(WMS.Lib.TASKTYPE.LOADPUTAWAY), _
                    Made4Net.Shared.FormatField(WMS.Lib.Statuses.Task.AVAILABLE), Made4Net.Shared.FormatField(WMS.Lib.Statuses.Task.ASSIGNED), _
                    Made4Net.Shared.FormatField(ld.LOADID))
                    Dim taskID As String = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
                    'Put(dr("NewLocation"), "", Common.GetCurrentUser)
                    Dim oPW As New WMS.Logic.Putaway()
                    'oPW.Put(_loadid, _sublocation, dr("NewLocation"), "", dr("DestinationWarehouseArea"), Common.GetCurrentUser)
                    oPW.Put(_loadid, ld.SUBLOCATION, dr("NewLocation"), dr("WarehouseArea"), WMS.Logic.Common.GetCurrentUser)

                    If Not String.IsNullOrEmpty(taskID) Then
                        Dim taskObj As New WMS.Logic.Task(taskID)
                        taskObj.Complete(Nothing)
                    End If

                    AppUtil.isBackLocMoveFront(_loadid, dr("NewLocation"), dr("WarehouseArea"), "", err)
                    If err <> "" Then Message += err

                Next
        End Select
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
    End Sub

    Private Sub TEMasterLoad_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEMasterLoad.CreatedChildControls
        With TEMasterLoad.ActionBar
            .AddSpacer()

            .AddExecButton("findlocationsimulate", "Find Location Simulation", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnSoftPlanWave"))
            With .Button("findlocationsimulate")
                .ObjectDLL = "WMS.Logic.dll"
                .ObjectName = "WMS.Logic.Load"
                .CommandName = "findlocationsimulate"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
            End With

            .AddExecButton("findlocation", "Find Location", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnReleaseWave"))
            With .Button("findlocation")
                .ObjectDLL = "WMS.Logic.dll"
                .ObjectName = "WMS.Logic.Load"
                .CommandName = "findlocation"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
            End With

            .AddExecButton("approvelocation", "Approve Location", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarApprovePicks"))
            With .Button("approvelocation")
                '.ObjectDLL = "WMS.Logic.dll"
                '.ObjectName = "WMS.Logic.Load"
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.LoadsLocation"

                .CommandName = "approvelocation"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
            End With

            .AddExecButton("printloadslocations", "Print Loads Locations", Made4Net.WebControls.SkinManager.GetImageURL("ReportPrint"))
            With .Button("printloadslocations")
                .ObjectDLL = "WMS.Logic.dll"
                .ObjectName = "WMS.Logic.Load"
                .CommandName = "printloadslocations"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
            End With

        End With
    End Sub


    Private Sub TEMasterLoad_AfterItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles TEMasterLoad.AfterItemCommand
        TEMasterLoad.RefreshData()
    End Sub

End Class
