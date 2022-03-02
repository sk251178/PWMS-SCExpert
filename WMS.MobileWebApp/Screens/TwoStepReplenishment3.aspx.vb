Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports WMS.Logic
<CLSCompliant(False)> Public Class TwoStepReplenishment3
    Inherits PWMSRDTBase

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.MobileWebApp.WebCtrls.Screen

    Protected WithEvents ddUOM As Made4Net.WebControls.MobileDropDown
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
            SetScreen()
        End If
    End Sub

    Private Sub SetScreen()
        Dim clsRep As RWMS.Logic.TwoStepReplenishment = Session("CLSREPL")
        If clsRep.TaskIndex > clsRep.alistRepl.Count - 1 Then
            doBack()
        End If
        Dim ReplTask As New WMS.Logic.ReplenishmentTask(clsRep.alistRepl(clsRep.TaskIndex))

        Dim ReplTaskDetail As New WMS.Logic.Replenishment(ReplTask.Replenishment)


        Dim replJob As ReplenishmentJob = ReplTask.getReplenishmentJob(ReplTaskDetail)

        Session("REPLJobDetail") = replJob
        DO1.Value("TASKTYPE") = replJob.TaskType
        DO1.Value("LOCATION") = replJob.fromLocation
        DO1.Value("FROMLOADID") = replJob.fromLoad
        DO1.Value("UNITS") = replJob.Units
        DO1.Value("CONSIGNEE") = replJob.Consignee
        DO1.Value("SKU") = replJob.Sku
        DO1.Value("SKUDESC") = replJob.skuDesc
        DO1.Value("UOMUNITS") = replJob.UOMUnits
        If ReplTaskDetail.ReplType = WMS.Logic.Replenishment.ReplenishmentTypes.FullReplenishment Then
            DO1.setVisibility("UOMUNITS", False)
        Else
            DO1.setVisibility("UOMUNITS", True)
        End If
    End Sub



    Private Sub doMenu()
        Try
            'Dim ReplTask As WMS.Logic.ReplenishmentTask = Session("REPLTSKTaskId")
            If Not Session("REPLTSKTaskId") Is Nothing Then
                Dim ReplTask As WMS.Logic.ReplenishmentTask = New WMS.Logic.ReplenishmentTask(CType(Session("REPLTSKTaskId"), WMS.Logic.ReplenishmentTask).TASK)
                ReplTask.ExitTask()
                Session.Remove("REPLTSKTaskId")
            End If
            Session.Remove("REPLTSKTaskId")
            Session.Remove("REPLTSKTDetail")
        Catch ex As Exception
        End Try
        Made4Net.Mobile.Common.GoToMenu()
    End Sub



    Private Function CheckLoadId() As Boolean

        Dim inpLoadId As String = DO1.Value("LOADID")
        Dim inpConsignee As String = DO1.Value("CONS")
        Dim inpSku As String = DO1.Value("ITEM")
        Dim inpLocation As String = DO1.Value("LOC")
        Dim SQL As String
        'Dim SQL As String = String.Format("Select loadid from loads inner join sku on loads.consignee = sku.consignee and loads.sku = sku.sku  where location = '{0}' and sku.consignee like '{1}%' and (sku.sku like '{2}%' or sku.othersku like '%{2}%') and loadid='{3}'", inpLocation, inpConsignee, inpSku, CType(Session("REPLTSKTDetail"), WMS.Logic.Replenishment).FromLoad.Trim)
        Select Case LoadFindType(inpLoadId, inpLocation, inpConsignee, inpSku)
            Case LoadSearchType.ByLoad
                SQL = String.Format("SELECT LOADID FROM LOADS WHERE LOADID LIKE '{0}'", inpLoadId.Trim())
            Case LoadSearchType.ByLocationAndSku
                SQL = String.Format("SELECT LOADID FROM LOADS INNER JOIN SKU ON LOADS.CONSIGNEE = SKU.CONSIGNEE AND LOADS.SKU = SKU.SKU WHERE LOADS.LOCATION LIKE '{0}%' AND (SKU.SKU like '{1}%' or SKU.MANUFACTURERSKU like '{1}%' or SKU.VENDORSKU ='{1}' or SKU.OTHERSKU ='{1}')", inpLocation.Trim(), inpSku.Trim())
            Case LoadSearchType.ByLocationAndConsigneeAndSku
                SQL = String.Format("SELECT LOADID FROM LOADS INNER JOIN SKU ON LOADS.CONSIGNEE = SKU.CONSIGNEE AND LOADS.SKU = SKU.SKU WHERE LOADS.LOCATION LIKE '{0}%' AND SKU.CONSIGNEE LIKE '{1}%' AND (SKU.SKU like '{2}%' or SKU.MANUFACTURERSKU like '{2}%' or SKU.VENDORSKU ='{2}' or SKU.OTHERSKU ='{2}')", inpLocation.Trim(), inpConsignee.Trim(), inpSku.Trim())
            Case LoadSearchType.ByLocation
                SQL = String.Format("SELECT LOADID FROM LOADS WHERE LOCATION LIKE '{0}%'", inpLocation.Trim())
            Case LoadSearchType.BySku
                SQL = String.Format("SELECT LOADID FROM LOADS INNER JOIN SKU ON LOADS.CONSIGNEE = SKU.CONSIGNEE AND LOADS.SKU = SKU.SKU WHERE SKU.CONSIGNEE LIKE '{0}%' AND (SKU.SKU like '{1}%' or SKU.MANUFACTURERSKU like '{1}%' or SKU.VENDORSKU ='{1}' or SKU.OTHERSKU ='{1}')", inpConsignee.Trim(), inpSku.Trim())
        End Select

        If String.IsNullOrEmpty(SQL) Then
            Return False
        End If

        inpLoadId = Made4Net.DataAccess.DataInterface.ExecuteScalar(SQL)
        Dim replJob As ReplenishmentJob = Session("REPLJobDetail")
        Try
            If inpLoadId.Trim.ToLower <> replJob.fromLoad.ToLower Then
                Return False
            Else
                Return True
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddLabelLine("TASKTYPE")
        DO1.AddLabelLine("FROMLOADID", "LOADID")
        DO1.AddLabelLine("CONSIGNEE")
        DO1.AddLabelLine("SKU")
        DO1.setVisibility("SKU", False)
        DO1.AddLabelLine("SKUDESC")
        DO1.AddLabelLine("LOCATION")
        DO1.AddLabelLine("UOMUNITS")
        'DO1.AddLabelLine("UNITS")
        DO1.AddSpacer()
        DO1.AddTextboxLine("LOADID")
        DO1.AddTextboxLine("CONS", "CONSIGNEE")
        DO1.AddTextboxLine("ITEM", "SKU")
        DO1.AddTextboxLine("LOC", "LOCATION")
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "back"
                doBack()
            Case "next"
                doNext()
            Case "deliver"
                doDeliver()
        End Select
    End Sub

    Private Sub doDeliver()
        Dim clsRep As RWMS.Logic.TwoStepReplenishment = Session("CLSREPL")
        Dim msg As String
        If Not clsRep.deliver(msg) Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  msg)
            doBack()
        Else
            Session("CLSREPL") = clsRep
            Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("Screens/TwoStepReplenishment4.aspx"))

        End If

    End Sub


    Private Function LoadFindType(ByVal LoadId As String, ByVal Loc As String, ByVal cons As String, ByVal sk As String) As Int32
        If LoadId <> "" Then Return LoadSearchType.ByLoad
        If Loc <> "" And sk <> "" Then Return LoadSearchType.ByLocationAndSku
        If Loc <> "" And cons <> "" And sk <> "" Then Return LoadSearchType.ByLocationAndConsigneeAndSku
        If cons <> "" And sk <> "" Then Return LoadSearchType.BySku
        If sk <> "" Then Return LoadSearchType.BySku
        If Loc <> "" Then Return LoadSearchType.ByLocation
    End Function

    Public Enum LoadSearchType
        ByLoad = 1
        ByLocationAndSku = 2
        ByLocationAndConsigneeAndSku = 3
        BySku = 4
        ByLocation = 6
    End Enum


    Private Sub doNext()
        If Not CheckLoadId() Then
            Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("Loads Does Not Match"))
        Else
            doOverrideReplenish()
        End If
    End Sub
    'If the location is the right one then replenish the location else just move
    'the load to new location and leave the Job active
    Private Sub doOverrideReplenish()
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        'If CheckLocationOverrirde() Then
        '    HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("Locations Match, use Next button"))
        'Else
        Dim clsRep As RWMS.Logic.TwoStepReplenishment = Session("CLSREPL")
        If clsRep.TaskIndex > clsRep.alistRepl.Count - 1 Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("all replenishment task are finish"))
            doBack()
        End If
        Dim ReplTask As New WMS.Logic.ReplenishmentTask(clsRep.alistRepl(clsRep.TaskIndex))

        Dim repl As New WMS.Logic.Replenishment(ReplTask.Replenishment)
        Dim replJob As ReplenishmentJob = ReplTask.getReplenishmentJob(repl)

        'Dim repl As WMS.Logic.Replenishment = Session("REPLTSKTDetail")
        'Dim repljob As ReplenishmentJob = Session("REPLJobDetail")
        'Dim ReplTask As WMS.Logic.ReplenishmentTask = Session("REPLTSKTaskId")
        Try
            repl.OverrideReplenish(ReplTask, replJob, clsRep.trolley, clsRep.WHArea, False, WMS.Logic.Common.GetCurrentUser)
            ' Dim clsRep As RWMS.Logic.TwoStepReplenishment = Session("CLSREPL")
            clsRep.TaskIndex += 1
            Session("CLSREPL") = clsRep

            Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("Screens/TwoStepReplenishment3.aspx"))

        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Return
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.ToString)
            Return
        End Try

        'End If

    End Sub


    Private Sub doBack()
        Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("Screens/TwoStepReplenishment2.aspx"))
    End Sub

    'Private Function CheckLocationOverrirde() As Boolean
    '    Try
    '        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

    '        Dim repljob As ReplenishmentJob = Session("REPLJobDetail")

    '        Dim strConfirmationLocation As String = Location.CheckLocationConfirmation(repljob.toLocation, DO1.Value("TOLOCATION"), DO1.Value("WAREHOUSEAREA"))

    '        'Dim inpLocation As String = DO1.Value("TOLOCATION")
    '        Dim inpWarehousearea As String = DO1.Value("WAREHOUSEAREA")

    '        Dim ReplTaskDetail As WMS.Logic.Replenishment = Session("REPLTSKTDetail")

    '        If strConfirmationLocation.Trim.ToLower <> repljob.toLocation.Trim.ToLower OrElse _
    '            inpWarehousearea.Trim.ToLower <> repljob.toWarehousearea.Trim.ToLower Then
    '            Return False
    '        Else
    '            Return True
    '        End If
    '    Catch ex As Exception
    '        Return False
    '    End Try
    'End Function
End Class
