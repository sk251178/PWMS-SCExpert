Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports WMS.Logic
Imports RWMS.Logic

<CLSCompliant(False)> Public Class TwoStepReplenishment4
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
            'If Not Request.QueryString.Item("sourcescreen") Is Nothing Then
            '    Session("REPLSRCSCREEN") = Request.QueryString.Item("sourcescreen")
            'End If
            SetScreen()
        End If
    End Sub

    Private Sub SetScreen()
        Dim clsRep As RWMS.Logic.TwoStepReplenishment = Session("CLSREPL")
        If clsRep.TaskDeliveryIndex > clsRep.alistDeliver.Count - 1 Then
            'HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("finish deliver"))
            doBack()
        End If
        Dim t As New WMS.Logic.Task(clsRep.alistDeliver(clsRep.TaskDeliveryIndex))
        If t.ASSIGNED Then
            If t.USERID <> WMS.Logic.GetCurrentUser Then
                t.DeAssignUser()
            End If
        End If

        t.AssignUser(WMS.Logic.GetCurrentUser)

        Dim ReplTask As New WMS.Logic.ReplenishmentTask(clsRep.alistDeliver(clsRep.TaskDeliveryIndex))

        Dim ReplTaskDetail As New WMS.Logic.Replenishment(ReplTask.Replenishment)

        Dim replJob As ReplenishmentJob
        'If Session("REPLSRCSCREEN") = "RPKC1" Then
        '    replJob = ReplenishmentTask.getReplenishmentJob(ReplTaskDetail.FromLoad, WMS.Logic.GetCurrentUser)
        'Else
        replJob = ReplTask.getReplenishmentJob(ReplTaskDetail)
        'End If
        Session("REPLJobDetail") = replJob
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        'If replJob.IsHandOff Then
        DO1.setVisibility("Note", True)
        DO1.Value("Note") = trans.Translate("Replenishment from trolley")
        'Else
        'DO1.setVisibility("Note", False)
        'End If
        DO1.Value("TASKTYPE") = replJob.TaskType

        DO1.Value("LOCATION") = replJob.toLocation
        DO1.Value("WAREHOUSEAREA") = replJob.toWarehousearea

        DO1.Value("LOADID") = replJob.fromLoad
        DO1.Value("UNITS") = replJob.UOMUnits 'replJob.Units
        DO1.Value("CONSIGNEE") = replJob.Consignee
        DO1.Value("SKU") = replJob.Sku
        DO1.Value("SKUDESC") = replJob.skuDesc
        DO1.Value("UOMUNITS") = replJob.UOMUnits
        If ReplTaskDetail.ReplType = WMS.Logic.Replenishment.ReplenishmentTypes.FullReplenishment Then
            DO1.setVisibility("UOMUNITS", False)
        Else
            DO1.setVisibility("UOMUNITS", False) ' True)
        End If
        'Fill the problem code drop down if operation allowed
        'Dim dd1 As Made4Net.WebControls.MobileDropDown
        'dd1 = DO1.Ctrl("TaskProblemCode")
        'dd1.AllOption = False
        'dd1.TableName = "vTaskTypesProblemCodes"
        'dd1.ValueField = "PROBLEMCODEID"
        'dd1.TextField = "PROBLEMCODEDESC"
        'dd1.Where = "TASKTYPE = '" & ReplTask.TASKTYPE & "'"
        'dd1.DataBind()
        'Try
        '    If dd1.GetValues.Count > 0 Then
        '        DO1.setVisibility("TaskProblemCode", True)
        '    Else
        '        DO1.setVisibility("TaskProblemCode", False)
        '    End If
        'Catch ex As Exception
        'DO1.setVisibility("TaskProblemCode", False)
        'End Try
    End Sub
    Private Sub doNext()
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Dim flag As Boolean = True
        If Not CheckLocation(flag) Then
            If flag Then HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("Locations Does Not Match"))
        Else

            'Dim repl As WMS.Logic.Replenishment = Session("REPLTSKTDetail")
            'Dim ReplTask As WMS.Logic.ReplenishmentTask = Session("REPLTSKTaskId")
            'Dim repljob As ReplenishmentJob = Session("REPLJobDetail")
            Dim clsRep As RWMS.Logic.TwoStepReplenishment = Session("CLSREPL")
            If clsRep.TaskDeliveryIndex > clsRep.alistDeliver.Count - 1 Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("finish deliver"))
                doBack()
            End If
            Dim ReplTask As New WMS.Logic.ReplenishmentTask(clsRep.alistDeliver(clsRep.TaskDeliveryIndex))

            Dim repl As New WMS.Logic.Replenishment(ReplTask.Replenishment)

            Dim replJob As ReplenishmentJob

            replJob = ReplTask.getReplenishmentJob(repl)

            Dim ld As New WMS.Logic.Load(repljob.fromLoad)

            Try
                ReplTask.Replenish(repl, replJob, WMS.Logic.Common.GetCurrentUser, True)

                clsRep.TaskDeliveryIndex += 1

                Session("CLSREPL") = clsRep

                'repljob.toLocation
                ' Dim err As String

                'AppUtil.isBackLocMoveFront(repl.ToLoad, repl.ToLocation, repl.ToWarehousearea, "", err)
                'If err <> "" Then
                '    HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  err)
                'Else
                Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("Screens/TwoStepReplenishment4.aspx"))
                'End If
            Catch ex As Made4Net.Shared.M4NException
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
                Return
            Catch ex As Exception
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.ToString())
                Return
            End Try

        End If
    End Sub


    Private Function CheckLocation(ByRef flag As Boolean) As Boolean
        Try
            Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

            Dim repljob As ReplenishmentJob = Session("REPLJobDetail")

            Dim strConfirmationLocation As String = Location.CheckLocationConfirmation(repljob.toLocation, DO1.Value("TOLOCATION"), DO1.Value("WAREHOUSEAREA"))

            'Dim inpLocation As String = DO1.Value("TOLOCATION")
            Dim inpWarehousearea As String = DO1.Value("WAREHOUSEAREA")

            Dim ReplTaskDetail As WMS.Logic.Replenishment = Session("REPLTSKTDetail")

            If strConfirmationLocation.Trim.ToLower <> repljob.toLocation.Trim.ToLower OrElse _
                inpWarehousearea.Trim.ToLower <> repljob.toWarehousearea.Trim.ToLower Then
                Try
                    Dim locDesc As New WMS.Logic.Location(strConfirmationLocation.Trim.ToLower, inpWarehousearea.Trim.ToLower)
                    Dim locTo As New WMS.Logic.Location(repljob.toLocation.Trim.ToLower, repljob.toWarehousearea.Trim.ToLower)

                    If locTo.CHECKDIGITS <> locDesc.CHECKDIGITS Then
                        HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("wrong location confirmation"))
                        flag = False
                        Return False
                    Else
                        Return True
                    End If
                Catch ex As Exception
                    Return False
                End Try
            Else
                Return True
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function

    ''If the location is the right one then replenish the location else just move
    ''the load to new location and leave the Job active
    'Private Sub doNext()
    '    Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
    '    If CheckLocationOverrirde() Then
    '        HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("Locations Match, use Next button"))
    '    Else
    '        Dim repl As WMS.Logic.Replenishment = Session("REPLTSKTDetail")
    '        Dim repljob As ReplenishmentJob = Session("REPLJobDetail")
    '        Dim ReplTask As WMS.Logic.ReplenishmentTask = Session("REPLTSKTaskId")
    '        Try
    '            repl.OverrideReplenish(ReplTask, repljob, DO1.Value("TOLOCATION"), DO1.Value("WAREHOUSEAREA"), False, WMS.Logic.Common.GetCurrentUser)
    '            Dim clsRep As RWMS.Logic.TwoStepReplenishment = Session("CLSREPL")
    '            clsRep.TaskDeliveryIndex += 1
    '            Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("Screens/TwoStepReplenishment3.aspx"))

    '        Catch ex As Made4Net.Shared.M4NException
    '            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
    '            Return
    '        Catch ex As Exception
    '            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.ToString)
    '            Return
    '        End Try

    '    End If

    'End Sub


    Private Sub doBack()
        Dim clsRep As RWMS.Logic.TwoStepReplenishment = Session("CLSREPL")
        clsRep.UnAssignTaskAllRepl()
        clsRep.clear()
        Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("Screens/TwoStepReplenishment.aspx"))

    End Sub

    Private Function CheckLocationOverrirde() As Boolean
        Try
            Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

            Dim repljob As ReplenishmentJob = Session("REPLJobDetail")

            Dim strConfirmationLocation As String = Location.CheckLocationConfirmation(repljob.toLocation, DO1.Value("TOLOCATION"), DO1.Value("WAREHOUSEAREA"))

            'Dim inpLocation As String = DO1.Value("TOLOCATION")
            Dim inpWarehousearea As String = DO1.Value("WAREHOUSEAREA")

            Dim ReplTaskDetail As WMS.Logic.Replenishment = Session("REPLTSKTDetail")

            If strConfirmationLocation.Trim.ToLower <> repljob.toLocation.Trim.ToLower OrElse _
                inpWarehousearea.Trim.ToLower <> repljob.toWarehousearea.Trim.ToLower Then
                Return False
            Else
                Return True
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddLabelLine("Note")
        DO1.AddLabelLine("TASKTYPE")
        DO1.AddLabelLine("LOADID")
        DO1.AddLabelLine("CONSIGNEE")
        DO1.AddLabelLine("SKU")
        DO1.setVisibility("SKU", False)
        DO1.AddLabelLine("SKUDESC")
        DO1.AddLabelLine("LOCATION")
        DO1.AddLabelLine("WAREHOUSEAREA")
        DO1.AddLabelLine("UOMUNITS")
        DO1.AddLabelLine("UNITS")
        DO1.AddSpacer()
        DO1.AddTextboxLine("TOLOCATION", True, "next")
        'DO1.AddTextboxLine("TOWAREHOUSEAREA")
        'DO1.AddDropDown("TaskProblemCode")
        DO1.AddSpacer()
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "next"
                doNext()
            Case "finish"
                doBack()


        End Select
    End Sub


End Class

