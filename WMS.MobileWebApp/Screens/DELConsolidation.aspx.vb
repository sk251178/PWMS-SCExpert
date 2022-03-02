Imports WMS.Logic
Imports Made4Net.Shared.Web
Imports Made4Net.Mobile

<CLSCompliant(False)> Public Class DELConsolidation
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

#Region "ViewState"

    Protected Overrides Function LoadPageStateFromPersistenceMedium() As Object
        Return Session("_ViewState")
    End Function

    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        Session("_ViewState") = viewState
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If WMS.Logic.GetCurrentUser Is Nothing Then
            WMS.Logic.GotoLogin()
        End If
        If Not IsPostBack Then
            Try
                'If Not Request.QueryString("sourcescreen") Is Nothing Then
                '    Session("MobileSourceScreen") = Request.QueryString("sourcescreen")
                'End If
                setDelivery()

            Catch ex As Exception
            End Try
        End If
    End Sub

    Public Sub setDelivery()
    
        Dim cont As RWMS.Logic.Container = Session("RWMSContainer")
        'CURRENTCONTAINER
        DO1.Value("CURRENTCONTAINER") = cont.fromcontainer
        DO1.Value("GOTOLOCATION") = cont.cont2fromlocation
        DO1.Value("CONSOLIDATECONTAINER") = cont.tocontainer
        DO1.Value("COMPANY") = cont.targetcompany
        Dim comp As New WMS.Logic.Company(cont.CONSIGNEE, cont.targetcompany, cont.companytype)

        DO1.Value("COMPANYNAME") = comp.COMPANYNAME

    End Sub

    Private Sub doMenu()
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Public Sub getNextDel()

        Dim taskid As String
        Dim cont As RWMS.Logic.Container = Session("RWMSContainer")

        taskid = cont.getContainerConsContainer(cont.fromcontainer, cont.fromtaskid)

        If WMS.Logic.Task.Exists(taskid) Then
            'go to consolidation delivery screen
            Dim t As New WMS.Logic.Task(taskid)
            t.AssignUser(WMS.Logic.GetCurrentUser)

            Session("RWMSContainer") = cont

            'Session("PCKDeliveryTask") = deltsk
        End If

    End Sub

    Private Sub doNext()
        Dim del As WMS.Logic.DeliveryTask
        Dim cont As RWMS.Logic.Container = Session("RWMSContainer")
        Dim sql As String = "select task from TASKS where TASKTYPE  = 'CONTDEL' and STATUS in ('AVAILABLE' , 'ASSIGNED') and TOCONTAINER ='{0}' and isnull(userid,'') in ('','{1}')"
        Dim squery As String
        Dim DT As New DataTable
        Dim NEWTASK As String
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

        If DO1.Value("CONFIRMCONTAINER") <> DO1.Value("CONSOLIDATECONTAINER") Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("CONTAINER confirmation incorrect"))
            Return
        End If

        If cont.cont1fromlocation <> cont.cont2fromlocation Then

            'run on all tasks for from container 
            squery = String.Format(sql, cont.fromcontainer, WMS.Logic.GetCurrentUser)
            Made4Net.DataAccess.DataInterface.FillDataset(squery, DT)
            For Each DR As DataRow In DT.Rows
                '1) Deliver main cont with execution location of second - consolidate container

                del = New WMS.Logic.DeliveryTask(DR("task"))

                del.Deliver(cont.cont2fromlocation, del.TOWAREHOUSEAREA, True)
                'Dim oCont As New WMS.Logic.Container(cont.fromcontainer, True)

                'Dim delJob As New WMS.Logic.DeliveryJob()

                'delJob = del.getDeliveryJob()

                'Dim tm As New WMS.Logic.TaskManager(DR("task"))
                ''Session("TaskID") = tm.Task.TASK
                '' CType(tm.Task, WMS.Logic.DeliveryTask).Deliver(cont.cont2fromlocation, DO1.Value("WAREHOUSEAREA"), delJob.IsHandOff, oCont)
                'del.Deliver(cont.cont2fromlocation, cont.warehousearea, True)

            Next
        End If

        'run on all tasks for TO container and close them
        '2) Cancel delivery of second - consolidate container
        squery = String.Format(sql, cont.tocontainer, WMS.Logic.GetCurrentUser)

        DT = New DataTable
        Made4Net.DataAccess.DataInterface.FillDataset(squery, DT)

        For Each DR As DataRow In DT.Rows
            If DR("task") <> NEWTASK Then
                del = New WMS.Logic.DeliveryTask(DR("task"))
                del.Cancel()
            End If
        Next

        If cont.cont1fromlocation <> cont.cont2fromlocation Then
            'Begin for RWMS-1294 and RWMS-1222 
            Dim logger As WMS.Logic.LogHandler
            If Not Session("RDTLogger") Is Nothing Then
                logger = CType(Session("RDTLogger"), LogHandler)
            End If
            'End for RWMS-1294 and RWMS-1222

            'Create Container Delivery Task
            del = New WMS.Logic.DeliveryTask()
            'del.CreateContainerDeliveryTask(cont.fromcontainer, cont.cont1tolocation, cont.warehousearea, WMS.Logic.GetCurrentUser)
            del.CreateContainerDeliveryTask(cont.fromcontainer, cont.cont1tolocation, cont.warehousearea, WMS.Logic.GetCurrentUser, logger)

            NEWTASK = del.TASK
            If NEWTASK = "" Then
                NEWTASK = cont.getDelTaskUser(cont.fromcontainer)
            End If
            Try
                If WMS.Logic.Task.Exists(NEWTASK) Then
                    Dim t1 As New WMS.Logic.Task(NEWTASK)
                    If t1.STATUS = WMS.Lib.Statuses.Task.AVAILABLE Then t1.AssignUser(WMS.Logic.GetCurrentUser)
                End If
            Catch ex As Exception
            End Try
            '            Session("PCKDeliveryTask") = del
        Else
            NEWTASK = cont.getDelTaskUser(cont.fromcontainer)
        End If

        '3)Consolidate 
        Dim cons As New WMS.Logic.Consolidation()
        Dim contB As New WMS.Logic.Container(cont.tocontainer, True)
        Dim contA As New WMS.Logic.Container(cont.fromcontainer, True)

        cons.Consolidate(contB, contA, WMS.Logic.GetCurrentUser)

        getNextDel(cont.fromcontainer, NEWTASK)

    End Sub

    Private Sub getNextDel(ByVal COUNTID As String, ByVal TASK As String)

        Dim taskid As String
        Dim cont As New RWMS.Logic.Container()

        taskid = cont.getContainerConsContainer(COUNTID, TASK)

        If Not IsNothing(taskid) And WMS.Logic.Task.Exists(taskid) Then
            'go to consolidation delivery screen
            Session("RWMSContainer") = cont

            Response.Redirect(MapVirtualPath("screens/DELConsolidation.aspx"))
            'Session("PCKDeliveryTask") = deltsk
        Else
            Session.Remove("RWMSContainer")
            Response.Redirect(MapVirtualPath("screens/DEL.aspx?delconsolidation=skip"))
        End If
    End Sub
    Private Sub Skip()
        Dim contOld As RWMS.Logic.Container = Session("RWMSContainer")
        contOld.setDeAssignUser(contOld.tocontainer)

        Session.Remove("RWMSContainer")
        Response.Redirect(MapVirtualPath("screens/DEL.aspx?delconsolidation=skip"))
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddLabelLine("CURRENTCONTAINER")
        DO1.AddSpacer()
        DO1.AddLabelLine("GOTOLOCATION")
        DO1.AddLabelLine("CONSOLIDATECONTAINER")
        DO1.AddLabelLine("COMPANY")

        DO1.AddLabelLine("COMPANYNAME")
        DO1.AddSpacer()
        DO1.AddTextboxLine("CONFIRMCONTAINER", True, "next")

    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "next"
                doNext()
            Case "skip"
                Skip()
        End Select
    End Sub
End Class
