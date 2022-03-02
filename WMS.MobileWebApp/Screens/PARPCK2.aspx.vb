Imports WMS.Logic
Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports WLTaskManager = WMS.Logic.TaskManager

<CLSCompliant(False)> Public Class PARPCK2
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
        If WMS.Logic.GetCurrentUser = "" Then
            WMS.Logic.GotoLogin()
        End If

        If Not IsPostBack Then
            If Not WMS.Logic.TaskManager.isAssigned(WMS.Logic.Common.GetCurrentUser, WMS.Lib.TASKTYPE.PARALLELPICKING, WMS.Logic.LogHandler.GetRDTLogger()) Then
                Response.Redirect(MapVirtualPath("Screens/PARPCK1.aspx"))
            End If

            Dim pcks As ParallelPicking = Session("PARPCKPicklist")
            Dim pck As PickJob
            Try
                pck = pcks.GetNextPick()
                Session("PARPCKPicklist") = pcks
            Catch ex As Exception

            End Try
            DO1.setVisibility("ContainerType", False)
            DO1.setVisibility("ContainerTypeDesc", False)
            DO1.setVisibility("ContainerID", False)

            setPick(pck)

            If Not Session("WeightNeededConfirm1") Is Nothing Then
                DO1.Value("CONFIRM") = Session("WeightNeededConfirm1")
            End If

        End If
    End Sub

    Private Sub setPick(ByVal pck As PickJob)
        If pck Is Nothing Then

            Dim pck2 As ParallelPicking = Session("PARPCKPicklist")
            If Not Session("PCKPicklist") Is Nothing Then
                Dim oPicklst As WMS.Logic.Picklist = Session("PCKPicklist")
                Dim oPicklist As New Picklist(oPicklst.PicklistID)
                If Not oPicklist Is Nothing Then
                    Session("PCKPicklist") = oPicklist
                End If
                If oPicklist.isCompleted Then
                    If Not Session("DefaultPrinter") = "" AndAlso Not Session("DefaultPrinter") Is Nothing Then
                        Dim prntr As LabelPrinter = New LabelPrinter(Session("DefaultPrinter"))
                        MobileUtils.getDefaultPrinter(Session("DefaultPrinter"), LogHandler.GetRDTLogger())
                        If Not Session("PCKPicklist") Is Nothing Then
                            If oPicklist.ShouldPrintShipLabel Then
                                oPicklist.PrintShipLabels(prntr.PrinterQName)
                            End If
                            PickTask.UpdateCompletionTime(oPicklist)
                        ElseIf Not Session("PARPCKPicklist") Is Nothing Then
                            pck2.PrintShipLabels(prntr.PrinterQName)
                        End If
                        If oPicklist.GetPrintContentList() And Not oPicklist.GetContentListReoprtName() Is Nothing Then
                            Response.Redirect(MapVirtualPath("Screens/DELCLPRNT.aspx?sourcescreen=" & Session("MobileSourceScreen") & "&printed=1"))
                        End If
                        Response.Redirect(MapVirtualPath("Screens/DEL.aspx?sourcescreen=" & Session("MobileSourceScreen") & "&printed=1"))
                    Else
                        Response.Redirect(MapVirtualPath("screens/DELLBLPRNT.aspx?sourcescreen=" & Session("MobileSourceScreen")))
                    End If
                End If

            End If

        Else
            Dim pcklist As Picklist = New Picklist(pck.picklist)
            Session("PARPCKPicklistPickJob") = pck
            DO1.Value("ParallelPickListId") = pck.parallelpicklistid
            DO1.Value("ParallelPickListSeq") = pck.parallelpicklistseq
            DO1.Value("Picklist") = pck.picklist
            DO1.Value("LOADID") = pck.fromload
            DO1.Value("LOCATION") = pck.fromlocation
            DO1.Value("WAREHOUSEAREA") = pck.fromwarehousearea
            DO1.Value("PICKMETHOD") = pcklist.PickMethod
            DO1.Value("SKU") = pck.sku
            DO1.Value("SKUDESC") = pck.skudesc
            DO1.Value("UOM") = pck.uom
            Dim sqluom As String = " SELECT DESCRIPTION FROM CODELISTDETAIL " & _
                          " WHERE CODELISTCODE = 'UOM' AND CODE = '" & pck.uom & "'"

            DO1.Value("UOMDesc") = Made4Net.DataAccess.DataInterface.ExecuteScalar(sqluom)
            DO1.Value("UOMUNITS") = pck.uomunits
            'Label Printing
            If String.IsNullOrEmpty(MobileUtils.LoggedInMHEID) And String.IsNullOrEmpty(MobileUtils.GetMHEDefaultPrinter) Then
                DO1.setVisibility("PRINTER", False)
            Else
                DO1.setVisibility("PRINTER", True)
            End If
            SetContainerID(pcklist)
        End If
    End Sub

    Private Sub SetContainerID(ByVal pcklist As Picklist)
        If Session("PCKPicklistActiveContainerID") Is Nothing Or pcklist.ActiveContainer <> Session("PCKPicklistActiveContainerID") Then
            If Not pcklist Is Nothing Then
                If Not pcklist.PickType = WMS.Lib.PICKTYPE.FULLPICK Then
                    DO1.setVisibility("ContainerType", False)
                    DO1.setVisibility("ContainerTypeDesc", True)
                    DO1.setVisibility("ContainerID", True)
                    Dim contid As String = pcklist.ActiveContainer
                    If contid.Trim = "" Then
                        contid = Made4Net.Shared.Util.getNextCounter("CONTAINER")
                    End If
                    DO1.Value("ContainerId") = contid
                    Session("PCKPicklistActiveContainerID") = contid
                    'Dim sqltype As String = " select containerdesc from handelingunittype " & _
                    '      " WHERE container = '" & Session("PCKPicklistActiveContainerID") & "'"
                    'DO1.Value("ContainerTypeDesc") = Made4Net.DataAccess.DataInterface.ExecuteScalar(sqltype)
                    DO1.Value("ContainerTypeDesc") = setContHUDesc(pcklist)
                End If
                If Not pcklist.ShouldPrintShipLabelOnPickLineCompleted Then
                    DO1.setVisibility("PRINTER", False)
                End If
            End If
        Else
            DO1.setVisibility("ContainerType", False)
            DO1.setVisibility("ContainerTypeDesc", True)
            DO1.setVisibility("ContainerID", True)
            DO1.Value("ContainerId") = Session("PCKPicklistActiveContainerID")
            DO1.Value("ContainerTypeDesc") = setContHUDesc(pcklist)
            'Dim sqltype As String = " select containerdesc from handelingunittype " & _
            '              " WHERE container = '" & Session("PCKPicklistActiveContainerID") & "'"
            'DO1.Value("ContainerTypeDesc") = Made4Net.DataAccess.DataInterface.ExecuteScalar(sqltype)
        End If
    End Sub

    Private Function setContHUDesc(ByVal pcklist As Picklist) As String
        Dim ret As String = ""
        Dim sql As String = "SELECT  h.CONTAINERDESC FROM dbo.PICKHEADER AS p INNER JOIN dbo.HANDELINGUNITTYPE AS h ON p.HANDELINGUNITTYPE = h.CONTAINER WHERE p.PICKLIST = '{0}'"
        Try
            If Not pcklist Is Nothing Then
                sql = String.Format(sql, pcklist.PicklistID)
                ret = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
            End If
        Catch ex As Exception
        End Try
        Return ret
    End Function

    Private Sub doNext()
        Dim pck As PickJob
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Dim pcks As ParallelPicking = New ParallelPicking(DO1.Value("ParallelPickListId"))
        Session("PARPCKPicklist") = pcks
        pck = Session("PARPCKPicklistPickJob")
        If pck Is Nothing Then
            Session.Remove("PCKPicklistActiveContainerID")
            Response.Redirect(MapVirtualPath("Screens/DEL.aspx"))
        End If
        Dim sku As New SKU(pck.consingee, pck.sku)
        Dim UOMUNITS As Decimal = 0
        Try
            UOMUNITS = Math.Round(Decimal.Parse(DO1.Value("UOMUNITS")), 2)
            If UOMUNITS < 0 Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("Wrong Quantity"))
                Exit Sub
                'Throw New Exception()
            End If

        Catch ex As Exception
            DO1.Value("UOMUNITS") = ""
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("Invalid UOMUNITS"))
            Return
        End Try

        'If osku.STATUS = False Then
        '    HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("Cannot perform the pick, SKU is not active"))
        '    Exit Sub
        'End If

        Try
            Session("PCKOldUomUnits") = pck.uomunits
            pck.pickedqty = sku.ConvertToUnits(pck.uom) * Convert.ToDecimal(DO1.Value("UOMUNITS"))
            pck.uomunits = DO1.Value("UOMUNITS")
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("Error cannot get units"))
            Return
        End Try

        Try
            Dim pcklsts As ParallelPicking = Session("PARPCKPicklist")
            pck.container = Session("PCKPicklistActiveContainerID")
            pck.oncontainer = pcklsts.ToContainer
            If MobileUtils.LoggedInMHEID.Trim <> String.Empty Then
                pck.LabelPrinterName = MobileUtils.GetMHEDefaultPrinter
            End If

            Dim pcklst As New Picklist(pck.picklist)
            If Not pcklst.Confirmed(DO1.Value("CONFIRM"), DO1.Value("WAREHOUSEAREA"), pck) Then ', SecondConfirmation) Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("Wrong Confirmation"))
                Exit Sub
                'Throw New Made4Net.Shared.M4NException(New Exception, "Wrong Confirmation", "Wrong Confirmation")
                Return
            End If

            If pck.units > pck.pickedqty Then
                Session("WeightNeededPickJob") = pck
                Session("WeightNeededConfirm1") = DO1.Value("CONFIRM")
                Response.Redirect(MapVirtualPath("screens/PCKPickShort.aspx?sourcescreen=PARPCK2"))
            End If
            Session.Remove("PCKOldUomUnits")
            If weightNeeded(sku) Then
                If pck.uomunits > 0 Then
                    Session("WeightNeededPickJob") = pck
                    Session("WeightNeededConfirm1") = DO1.Value("CONFIRM")
                    Response.Redirect(MapVirtualPath("screens/PCKWEIGHTNEEDED.aspx?sourcescreen=PARPCK2"))
                End If
            End If

            pck.TaskConfirmation.ConfirmationValues = MobileUtils.ExtractConfirmationValues(pck.TaskConfirmation.ConfirmationType, DO1.Value("CONFIRM").Trim(), "", DO1.Value("WAREHOUSEAREA").Trim())
            pck = PickTask.Pick(pcklst, pck, WMS.Logic.GetCurrentUser, WMS.Logic.LogHandler.GetRDTLogger())
            'pck = pcks.Pick(DO1.Value("CONFIRM"), DO1.Value("WAREHOUSEAREA"), pck, WMS.Logic.Common.GetCurrentUser)
            Session("PARPCKPicklist") = pcklsts
        Catch ex As Threading.ThreadAbortException
        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Return
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate(ex.Message))
            Return
        End Try

        DO1.Value("CONFIRM") = ""
        setPick(pck)
    End Sub

    Private Sub doBack()
        Response.Redirect(MapVirtualPath("Screens/PARPCK1.aspx"))
    End Sub

    Private Sub doMenu()
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser
        If WMS.Logic.TaskManager.isAssigned(UserId, WMS.Lib.TASKTYPE.PARALLELPICKING, WMS.Logic.LogHandler.GetRDTLogger()) Then
            Dim tm As WLTaskManager = WLTaskManager.NewTaskManagerForUserAndTaskType(UserId, WMS.Lib.TASKTYPE.PARALLELPICKING, LogHandler.GetRDTLogger())
            tm.ExitTask()
        End If
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls


        DO1.AddLabelLine("ParallelPickListId")
        DO1.AddLabelLine("ParallelPickListSeq")
        DO1.AddLabelLine("Picklist")
        DO1.AddLabelLine("PickMethod")
        DO1.AddLabelLine("SKU")
        DO1.AddLabelLine("SKUDESC")
        DO1.AddLabelLine("LOADID")
        DO1.AddLabelLine("UOM")
        DO1.AddLabelLine("UOMDesc")
        DO1.AddLabelLine("LOCATION")
        DO1.AddLabelLine("WAREHOUSEAREA")
        DO1.AddSpacer()
        DO1.AddTextboxLine("ContainerID")
        DO1.AddLabelLine("ContainerType")
        DO1.AddLabelLine("ContainerTypeDesc")
        DO1.AddTextboxLine("CONFIRM")
        DO1.AddTextboxLine("UOMUNITS")
        DO1.AddTextboxLine("PRINTER")

    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "next"
                doNext()
            Case "back"
                doBack()
            Case "menu"
                doMenu()
        End Select
    End Sub

    Private Function weightNeeded(ByVal pSKU As WMS.Logic.SKU) As Boolean
        If Not pSKU.SKUClass Is Nothing Then
            For Each oAtt As WMS.Logic.SkuClassLoadAttribute In pSKU.SKUClass.LoadAttributes
                If (oAtt.Name.ToUpper = "WEIGHT" Or oAtt.Name.ToUpper = "WGT") AndAlso _
                (oAtt.CaptureAtPicking = SkuClassLoadAttribute.CaptureType.Capture OrElse oAtt.CaptureAtPicking = SkuClassLoadAttribute.CaptureType.Required) Then
                    Return True
                End If
            Next
        End If
        Return False
    End Function

End Class