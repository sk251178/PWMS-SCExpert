Imports WMS.Logic
Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports Made4Net.DataAccess
Imports WLTaskManager = WMS.Logic.TaskManager

<CLSCompliant(False)> Public Class PCKORD1
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
            If Not WMS.Logic.TaskManager.isAssigned(WMS.Logic.Common.GetCurrentUser, WMS.Lib.TASKTYPE.PICKING, WMS.Logic.LogHandler.GetRDTLogger()) Then
                Response.Redirect(MapVirtualPath("Screens/PCKORD.aspx"))
            End If

            Dim pcks As Picklist = Session("PCKPicklist")
            Dim pck As PickJob
            Try
                Dim tm As WLTaskManager = WLTaskManager.NewTaskManagerForPicklist(pcks.PicklistID)
                pck = PickTask.getNextPick(pcks)
            Catch ex As Exception

            End Try

            setPick(pck)
        End If
    End Sub

    Private Sub setPick(ByVal pck As PickJob)
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
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
            Session("PCKPicklistPickJob") = pck
            Dim pcks As Picklist = Session("PCKPicklist")
            DO1.Value("Picklist") = pck.picklist
            DO1.Value("LOADID") = pck.fromload
            DO1.Value("LOCATION") = pck.fromlocation
            DO1.Value("WAREHOUSEAREA") = pck.fromwarehousearea

            DO1.Value("PICKMETHOD") = pcks.PickMethod
            DO1.Value("PICKTYPE") = pcks.PickType
            DO1.Value("SKU") = pck.sku
            DO1.Value("SKUDESC") = pck.skudesc
            DO1.Value("UOM") = pck.uom
            Dim sqluom As String = " SELECT DESCRIPTION FROM CODELISTDETAIL " & _
                          " WHERE CODELISTCODE = 'UOM' AND CODE = '" & pck.uom & "'"
            DO1.Value("UOMDesc") = Made4Net.DataAccess.DataInterface.ExecuteScalar(sqluom)
            DO1.Value("UOMUNITS") = pck.uomunits

            If pck.SystemPickShort Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("System Pick Short- Quantity available is less than quantity required / Wrong Location!"))
            End If
        End If
    End Sub

    Private Sub doNext()
        Dim pck As PickJob
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Dim pcklst As Picklist = Session("PCKPicklist")
        pck = Session("PCKPicklistPickJob")
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

        End If

        Try
            Dim sku As New sku(pck.consingee, pck.sku)
            pck.pickedqty = sku.ConvertToUnits(pck.uom) * Convert.ToDecimal(DO1.Value("UOMUNITS"))
            sku = Nothing
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Error cannot get units"))
            Return
        End Try

        Try
            Dim tm As WLTaskManager = WLTaskManager.NewTaskManagerForPicklist(pcklst.PicklistID)
            ExtractAttributes()
            pck.container = Session("PCKPicklistActiveContainerID")
            If Session("CONFTYPE") = WMS.Lib.Release.CONFIRMATIONTYPE.SKULOCATION Then
                pck.TaskConfirmation.ConfirmationValues = MobileUtils.ExtractConfirmationValues(pck.TaskConfirmation.ConfirmationType, DO1.Value("CONFIRM").Trim(), DO1.Value("CONFIRMSKU"), DO1.Value("WAREHOUSEAREA").Trim())
                pck = PickTask.Pick(pcklst, pck, WMS.Logic.GetCurrentUser, WMS.Logic.LogHandler.GetRDTLogger())
                'pck = PickTask.Pick(DO1.Value("CONFIRM"), DO1.Value("WAREHOUSEAREA"), pcklst, pck, WMS.Logic.Common.GetCurrentUser, True, DO1.Value("CONFIRMSKU"))
            Else
                pck.TaskConfirmation.ConfirmationValues = MobileUtils.ExtractConfirmationValues(pck.TaskConfirmation.ConfirmationType, DO1.Value("CONFIRM").Trim(), "", DO1.Value("WAREHOUSEAREA").Trim())
                pck = PickTask.Pick(pcklst, pck, WMS.Logic.GetCurrentUser, WMS.Logic.LogHandler.GetRDTLogger())

                'pck = PickTask.Pick(DO1.Value("CONFIRM"), DO1.Value("WAREHOUSEAREA"), pcklst, pck, WMS.Logic.Common.GetCurrentUser)
            End If
        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Return
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate(ex.Message))
            Return
        End Try

        ClearAttributes()
        DO1.Value("CONFIRM") = ""
        DO1.Value("CONFIRMSKU") = ""
        setPick(pck)
    End Sub

    Private Sub doBack()
        Session.Remove("CONFTYPE")
        Response.Redirect(MapVirtualPath("Screens/PCKORD.aspx"))
    End Sub

    Private Sub doMenu()
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser
        If WMS.Logic.TaskManager.isAssigned(UserId, WMS.Lib.TASKTYPE.PICKING, WMS.Logic.LogHandler.GetRDTLogger()) Then
            Dim tm As WLTaskManager = WLTaskManager.NewTaskManagerForUserAndTaskType(UserId, WMS.Lib.TASKTYPE.PICKING, LogHandler.GetRDTLogger())
            tm.ExitTask()
        End If
        Session.Remove("CONFTYPE")
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddLabelLine("Picklist")
        DO1.AddLabelLine("PickMethod")
        DO1.AddLabelLine("PickType")
        DO1.AddLabelLine("SKU")
        DO1.AddLabelLine("SKUDESC")
        DO1.AddLabelLine("LOADID")
        DO1.AddLabelLine("UOM")
        DO1.AddLabelLine("UOMDesc")
        DO1.AddLabelLine("LOCATION")
        DO1.AddLabelLine("WAREHOUSEAREA")
        DO1.AddSpacer()
        Dim pck As PickJob = Session("PCKPicklistPickJob")
        If Not pck Is Nothing Then
            If Not pck.oAttributeCollection Is Nothing Then
                For idx As Int32 = 0 To pck.oAttributeCollection.Count - 1
                    DO1.AddTextboxLine(pck.oAttributeCollection.Keys(idx))
                Next
            End If
        End If
        DO1.AddTextboxLine("CONFIRM")
        DO1.AddTextboxLine("CONFIRMSKU")
        DO1.AddTextboxLine("UOMUNITS")

        DO1.setVisibility("UOMDesc", True)

        'Added by udi 01/01/2006
        Dim pcklist As Picklist = New Picklist(pck.picklist)
        Dim relStrat As ReleaseStrategyDetail

        relStrat = pcklist.getReleaseStrategy()
        If Not relStrat Is Nothing Then
            Session("CONFTYPE") = relStrat.ConfirmationType
            If relStrat.ConfirmationType = WMS.Lib.Release.CONFIRMATIONTYPE.SKULOCATION Then
                DO1.setVisibility("CONFIRMSKU", True)
            Else
                DO1.setVisibility("CONFIRMSKU", False)
            End If
        End If
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "next"
                doNext()
            Case "back"
                doBack()
            Case "closecontainer"
                doCloseContainer()
        End Select
    End Sub

    Private Function ExtractAttributes() As AttributesCollection
        Dim pck As PickJob = Session("PCKPicklistPickJob")
        Dim Val As Object
        If Not pck Is Nothing Then
            If Not pck.oAttributeCollection Is Nothing Then
                For idx As Int32 = 0 To pck.oAttributeCollection.Count - 1
                    Val = DO1.Value(pck.oAttributeCollection.Keys(idx))
                    If Val = "" Then Val = Nothing
                    pck.oAttributeCollection(idx) = Val
                Next
                Return pck.oAttributeCollection
            End If
        End If
        Return Nothing
    End Function

    Private Sub ClearAttributes()
        Dim pck As PickJob = Session("PCKPicklistPickJob")
        If Not pck Is Nothing Then
            If Not pck.oAttributeCollection Is Nothing Then
                For idx As Int32 = 0 To pck.oAttributeCollection.Count - 1
                    DO1.Value(pck.oAttributeCollection.Keys(idx)) = ""
                Next
            End If
        End If
    End Sub

    Private Sub doCloseContainer()
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        If Not Session("PCKPicklistActiveContainerID") Is Nothing Then
            Dim pcklist As Picklist = Session("PCKPicklist")
            Dim relStrat As ReleaseStrategyDetail
            relStrat = pcklist.getReleaseStrategy()
            Dim pck As PickJob = Session("PCKPicklistPickJob")
            If Not relStrat Is Nothing Then
                If relStrat.DeliverContainerOnClosing Then
                    'Should deliver the container now
                    pcklist.CloseContainer(Session("PCKPicklistActiveContainerID"), True, WMS.Logic.GetCurrentUser)

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
                    'Should close the container - go back to PCK to open a new one
                    pcklist.CloseContainer(Session("PCKPicklistActiveContainerID"), False, WMS.Logic.GetCurrentUser)
                    Session.Remove("PCKPicklistActiveContainerID")
                    Response.Redirect(MapVirtualPath("screens/PCKORD.aspx"))
                End If
            End If
        Else
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("Cannot Close Cotnainer - Container is blank"))
        End If
    End Sub

End Class