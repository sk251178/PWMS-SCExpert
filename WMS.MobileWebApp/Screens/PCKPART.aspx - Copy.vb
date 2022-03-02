Imports WMS.Logic
Imports Made4Net.Shared.Web
Imports Made4Net.Mobile

<CLSCompliant(False)> Public Class PCKPART
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.MobileWebApp.WebCtrls.Screen
    Protected WithEvents lblUnits As Made4Net.WebControls.Label
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
            If Session("MobileSourceScreen") Is Nothing Then
                If Not Request.QueryString("sourcescreen") Is Nothing Then
                    Session("MobileSourceScreen") = Request.QueryString("sourcescreen")
                Else
                    Session("MobileSourceScreen") = "PCK"
                End If
            End If

            If Not WMS.Logic.TaskManager.isAssigned(WMS.Logic.Common.GetCurrentUser, WMS.Lib.TASKTYPE.PICKING) Then
                If WMS.Logic.TaskManager.isAssigned(WMS.Logic.Common.GetCurrentUser, WMS.Lib.TASKTYPE.DELIVERY) Then
                    Response.Redirect(MapVirtualPath("Screens/DELLBLPRNT.aspx?sourcescreen=PCK"))
                Else
                    If Session("MobileSourceScreen") = "PCKPART" Then
                        Session("MobileSourceScreen") = "PCK"
                    End If
                    Response.Redirect(MapVirtualPath("screens/" & Session("MobileSourceScreen") & ".aspx"))
                End If
            End If


            'If WMS.Logic.TaskManager.isAssigned(WMS.Logic.Common.GetCurrentUser, WMS.Lib.TASKTYPE.DELIVERY) Then
            '    Response.Redirect(MapVirtualPath("Screens/DELLBLPRNT.aspx?sourcescreen=PCK"))
            'ElseIf Not WMS.Logic.TaskManager.isAssigned(WMS.Logic.Common.GetCurrentUser, WMS.Lib.TASKTYPE.PICKING) Then
            '    If Session("MobileSourceScreen") = "PCKPART" Then
            '        Session("MobileSourceScreen") = "PCK"
            '    End If
            '    Response.Redirect(MapVirtualPath("screens/" & Session("MobileSourceScreen") & ".aspx"))
            'End If


            'Dim pcks As Picklist = Session("PCKPicklist")
            Dim pck As PickJob
            Try
                If Logic.TaskManager.isAssigned(WMS.Logic.GetCurrentUser) Then
                    Dim oTask As Task
                    Try
                        oTask = Logic.TaskManager.getUserAssignedTask(WMS.Logic.GetCurrentUser)
                    Catch ex As Exception
                    End Try
                    If Not oTask Is Nothing Then
                        Session("TMTask") = oTask

                        Dim pcks As New Picklist(oTask.Picklist)

                        Dim tm As New WMS.Logic.TaskManager(pcks.PicklistID, True)
                        pck = PickTask.getNextPick(pcks)
                        Session("PCKPicklist") = pcks

                    End If
                End If
                'Dim tm As New WMS.Logic.TaskManager(pcks.PicklistID, True)
                'pck = PickTask.getNextPick(pcks)
                'Session("PCKPicklist") = pcks

            Catch ex As Exception
                MessageQue.Enqueue(ex.ToString())
            End Try


            setPick(pck)
            DO1.Value("UOMUNITS") = Session("UomUnits")
            If Not Session("WeightNeededConfirm1") Is Nothing Then
                DO1.Value("CONFIRM") = Session("WeightNeededConfirm1")
                Session("WeightNeededConfirm1") = ""

                'If Not IsNothing(Session("PCKPicklistActiveContainerIDSecond")) Then
                '    DO1.FocusField = "CONTAINER"
                'End If

                If Session("CONFTYPE") = WMS.Lib.Release.CONFIRMATIONTYPE.SKULOCATION Then
                    If Not Session("WeightNeededConfirm2") Is Nothing Then
                        DO1.Value("CONFIRMSKU") = Session("WeightNeededConfirm2")
                        Session("WeightNeededConfirm2") = ""
                    End If
                End If
            End If


        End If
    End Sub

    Private Sub setPick(ByVal pck As PickJob)
        If pck Is Nothing Then
            'Response.Redirect(MapVirtualPath("Screens/DELLBLPRNT.aspx?sourcescreen=PCK"))
            Response.Redirect(MapVirtualPath("screens/DELLBLPRNT.aspx?sourcescreen=" & Session("MobileSourceScreen")))
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
            Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
            Session("UomUnits") = pck.uomunits
            DO1.Value("UOMUNITS") = Session("UomUnits")
            If pck.SystemPickShort Then
                'MessageQue.Enqueue(trans.Translate("System Pick Short- Quantity available is less than quantity required / Wrong Location!"))
            End If
            'Check For LowLimitCount
            Dim oLoad As New WMS.Logic.Load(pck.fromload)
            Dim oSku As New WMS.Logic.SKU(oLoad.CONSIGNEE, oLoad.SKU)
            If oLoad.UNITS <= oSku.LOWLIMITCOUNT And oLoad.LASTCOUNTDATE.Date < DateTime.Now.Date Then
                'Redirect to load count and get back
                MessageQue.Enqueue(trans.Translate("Load Units is less than low limit count - Please count load"))
                Session("LoadCNTLoadId") = oLoad.LOADID
                Session("LoadCountingSourceScreen") = "PCKPART"
                Response.Redirect(MapVirtualPath("Screens/CNT2.aspx"))
            End If
            'Label Printing
            If pcks.ShouldPrintShipLabelOnPickLineCompleted Then
                If MobileUtils.LoggedInMHEID <> String.Empty And MobileUtils.GetMHEDefaultPrinter <> String.Empty Then
                    DO1.setVisibility("PRINTER", False)
                Else
                    DO1.setVisibility("PRINTER", True)
                End If
            Else
                DO1.setVisibility("PRINTER", False)
            End If
        End If
    End Sub

    Private Sub doCloseContainer()
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        If Not Session("PCKPicklistActiveContainerID") Is Nothing Then
            Try
                Dim pcklist As Picklist = Session("PCKPicklist") 'New Picklist(pck.picklist)
               
                'If (pcklist.HandelingUnitType = WarehouseParams.GetWarehouseParam("MultiPickHUType") And Session("MHType") = WarehouseParams.GetWarehouseParam("MultiPickMHType")) Then
                '    Try
                '        Response.Redirect(MapVirtualPath("screens/PCKCLOSECONTAINER.aspx"))

                '    Catch ex As Made4Net.Shared.M4NException
                '        ' MessageQue.Enqueue(ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
                '        Return
                '    Catch ex As Exception
                '        ' MessageQue.Enqueue(ex.Message)
                '        Return
                '    End Try
                'End If

                Dim relStrat As ReleaseStrategyDetail
                relStrat = pcklist.getReleaseStrategy()
                Dim pck As PickJob = Session("PCKPicklistPickJob")
                If Not relStrat Is Nothing Then
                    If relStrat.DeliverContainerOnClosing Then
                        'Should deliver the container now
                        pcklist.CloseContainer(Session("PCKPicklistActiveContainerID"), True, WMS.Logic.GetCurrentUser)
                        Response.Redirect(MapVirtualPath("screens/DELLBLPRNT.aspx?sourcescreen=PCK"))
                    Else
                        'Should close the container - go back to PCK to open a new one
                        pcklist.CloseContainer(Session("PCKPicklistActiveContainerID"), False, WMS.Logic.GetCurrentUser)
                        Session.Remove("PCKPicklistActiveContainerID")
                        Response.Redirect(MapVirtualPath("screens/PCK.aspx"))
                    End If
                End If
            Catch ex As Threading.ThreadAbortException
            Catch ex As Made4Net.Shared.M4NException
                MessageQue.Enqueue(ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
                Return
            Catch ex As Exception
                MessageQue.Enqueue(ex.Message)
                Return
            End Try
        Else
            MessageQue.Enqueue(trans.Translate("Cannot Close Cotnainer - Container is blank"))
        End If
    End Sub

    Private Sub doNext()
        Dim pck As PickJob
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Dim pcklst As Picklist = Session("PCKPicklist")
        pck = Session("PCKPicklistPickJob")
        Try
            If pck Is Nothing Then
                'Response.Redirect(MapVirtualPath("Screens/DELLBLPRNT.aspx?sourcescreen=PCK"))
                Response.Redirect(MapVirtualPath("screens/DELLBLPRNT.aspx?sourcescreen=" & Session("MobileSourceScreen")))
            End If
            Dim sku As New SKU(pck.consingee, pck.sku)


            Dim UOMUNITS As Decimal = 0
            Try
                UOMUNITS = Math.Round(Decimal.Parse(DO1.Value("UOMUNITS")), 2)
                If UOMUNITS < 0 Then Throw New Exception()
            Catch ex As Exception
                DO1.Value("UOMUNITS") = ""
                MessageQue.Enqueue(t.Translate("Invalid UOMUNITS"))
                Return
            End Try

            Try
                Session("PCKOldUomUnits") = pck.uomunits
                pck.pickedqty = sku.ConvertToUnits(pck.uom) * Convert.ToDecimal(DO1.Value("UOMUNITS"))
                pck.uomunits = DO1.Value("UOMUNITS")
                'sku = Nothing
            Catch ex As Exception
                MessageQue.Enqueue(t.Translate("Error cannot get units"))
                Return
            End Try

            Try
                If Not pcklst.Confirmed(DO1.Value("CONFIRM"), DO1.Value("WAREHOUSEAREA"), pck, DO1.Value("CONFIRMSKU")) Then
                    MessageQue.Enqueue(t.Translate("Wrong Confirmation"))
                    Return
                End If
            Catch ex As Exception
                MessageQue.Enqueue(t.Translate("Wrong Confirmation"))
                Return
            End Try


            'If Not IsNothing(Session("PCKPicklistActiveContainerIDSecond")) Then
            '    ' If Not (DO1.Value("CONTAINER") = Session("PCKPicklistActiveContainerID") Or _
            '    'MultiContManage.ContainerContainKey(DO1.Value("CONTAINER"))) And UOMUNITS > 0 Then
            '    '     MessageQue.Enqueue(t.Translate("Illegal Container"))
            '    '     Return
            '    If Not MultiContManage.ActiveContainerContain(DO1.Value("CONTAINER")) Then
            '        MessageQue.Enqueue(t.Translate("Illegal Container"))
            '        Return
            '    ElseIf MultiContManage.ActiveContainerContain(DO1.Value("CONTAINER")) Then
            '        'DO NOT MATTER WITCH cont was scann, set session of first con be first
            '        ' Session("PCKPicklistActiveContainerIDSecond") = Session("PCKPicklistActiveContainerID")
            '        Session("PCKPicklistActiveContainerID") = DO1.Value("CONTAINER")
            '    End If
            '    'ElseIf Session("PCKPicklistActiveContainerID") = DO1.Value("CONTAINER") Then
            '    '    MessageQue.Enqueue(t.Translate("Illegal Container"))
            '    '    Return
            'ElseIf IsNothing(Session("PCKPicklistActiveContainerID")) Or System.String.IsNullOrEmpty(Session("PCKPicklistActiveContainerID").ToString) Then
            '    MessageQue.Enqueue(t.Translate("All containers are closed, need to open container"))
            '    Return
            'End If


            Try
                Dim tm As New WMS.Logic.TaskManager(pcklst.PicklistID, True)
                ExtractAttributes()

                Session.Add("TaskID", tm.getAssignedTask(WMS.Logic.GetCurrentUser, WMS.Lib.TASKTYPE.PICKING).TASK)

                'If Not IsNothing(Session("PCKPicklistActiveContainerIDSecond")) Then
                pck.container = DO1.Value("CONTAINER")
                'Else
                '    pck.container = Session("PCKPicklistActiveContainerID")
                'End If
                'pcklst.Containers.

                If pck.units > pck.pickedqty Then
                    ' Session("PCKQTYConfirmed") = pck.pickedqty
                    Session("WeightNeededPickJob") = pck
                    Session("WeightNeededConfirm1") = DO1.Value("CONFIRM")
                    Session("WeightNeededConfirm2") = DO1.Value("CONFIRMSKU")

                    Session("PCKPicklist") = pcklst
                    'If Not IsNothing(Session("PCKPicklistActiveContainerIDSecond")) And pck.pickedqty > 0 Then

                    '    'no need to add first pick to dict collection, becouse it will be a last pick
                    '    'we saved all needed data into WeightNeededPickJob session

                    '    'MultiContManage.SetPickedContainerUnits(DO1.Value("CONTAINER"), pck.pickedqty)
                    '    Response.Redirect(MapVirtualPath("screens/PCKPickMulti.aspx?sourcescreen=PCKPART"))
                    'Else
                    Response.Redirect(MapVirtualPath("screens/PCKPickShort.aspx?sourcescreen=PCKPART"))
                    'End If
                End If
                'Session.Remove("PCKOldUomUnits")

                If Session("CONFTYPE") = WMS.Lib.Release.CONFIRMATIONTYPE.SKULOCATION Then
                    'If weightNeeded(sku) Then
                    '    If pck.uomunits > 0 Then

                    '        Session("WeightNeededPickJob") = pck
                    '        Session("WeightNeededConfirm1") = DO1.Value("CONFIRM")
                    '        Session("WeightNeededConfirm2") = DO1.Value("CONFIRMSKU")
                    '        Response.Redirect(MapVirtualPath("screens/PCKWEIGHTNEEDED.aspx?sourcescreen=PCKPART"))
                    '    End If
                    'End If
                    pck = PickTask.Pick(DO1.Value("CONFIRM"), DO1.Value("WAREHOUSEAREA"), pcklst, pck, WMS.Logic.Common.GetCurrentUser, True, DO1.Value("CONFIRMSKU"))
                    clearSession()
                Else
                    'If weightNeeded(sku) Then
                    '    If pck.uomunits > 0 Then
                    '        Session("WeightNeededPickJob") = pck
                    '        Session("WeightNeededConfirm1") = DO1.Value("CONFIRM")
                    '        Response.Redirect(MapVirtualPath("screens/PCKWEIGHTNEEDED.aspx?sourcescreen=PCKPART"))
                    '    End If
                    'End If

                    pck = PickTask.Pick(DO1.Value("CONFIRM"), DO1.Value("WAREHOUSEAREA"), pcklst, pck, WMS.Logic.Common.GetCurrentUser)
                    clearSession()
                End If
                Session("PCKPicklist") = pcklst
            Catch ex As Threading.ThreadAbortException
            Catch ex As Made4Net.Shared.M4NException
                'ClearAttributes()
                clearSession()
                Session("UomUnits") = pck.uomunits
                DO1.Value("UOMUNITS") = Session("UomUnits")
                MessageQue.Enqueue(ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
                Return
            Catch ex As Exception
                'ClearAttributes()
                clearSession()
                Session("UomUnits") = pck.uomunits
                DO1.Value("UOMUNITS") = Session("UomUnits")
                MessageQue.Enqueue(t.Translate(ex.Message))
                Return
            End Try

            ClearAttributes()
            DO1.Value("CONFIRM") = ""
            DO1.Value("CONFIRMSKU") = ""
            If pcklst.PickType = WMS.Lib.PICKTYPE.NEGATIVEPALLETPICK And WMS.Logic.TaskManager.isAssigned(WMS.Logic.Common.GetCurrentUser, WMS.Lib.TASKTYPE.PUTAWAY) Then
                Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("SCREENS/RPK2.aspx?sourcescreen=PCKPART"))
            End If

            'If IsNothing(pck) And WMS.Logic.TaskManager.isAssigned(WMS.Logic.Common.GetCurrentUser, WMS.Lib.TASKTYPE.DELIVERY) Then
            '    Session.Remove("PCKPicklist")
            '    Response.Redirect(MapVirtualPath("Screens/DEL.aspx?sourcescreen=PCK"))
            'End If

            setPick(pck)
            '  Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("SCREENS/PCKPART.aspx"))
        Catch ex As System.Threading.ThreadAbortException

        End Try

    End Sub

    Private Sub clearSession()
        DO1.Value("CONFIRM") = ""
        DO1.Value("CONFIRMSKU") = ""
        Session("WeightNeededPickJob") = ""
        Session("WeightNeededConfirm1") = ""
        Session("WeightNeededConfirm2") = ""

    End Sub

    Private Sub doBack()
        Session.Remove("CONFTYPE")
        Session.Remove("UomUnits")
        'Response.Redirect(MapVirtualPath("Screens/PCK.aspx"))
        Dim pcklst As Picklist = Session("PCKPicklist")
        'If (pcklst.HandelingUnitType = WarehouseParams.GetWarehouseParam("MultiPickHUType") And Session("MHType") = WarehouseParams.GetWarehouseParam("MultiPickMHType") And pcklst.PickType = WMS.Lib.PICKTYPE.PARTIALPICK) Then 'Session("MHType")  = HANDLINGEQUIPMENT
        '    Response.Redirect(MapVirtualPath("screens/PCKPARTMULTICONT.aspx"))
        'Else
        Try
            If Session("MobileSourceScreen") Is Nothing Then
                Response.Redirect(MapVirtualPath("screens/PCK.aspx"))
            Else
                Response.Redirect(MapVirtualPath("screens/" & Session("MobileSourceScreen") & ".aspx"))
            End If
        Catch ex As System.Threading.ThreadAbortException

        End Try

    End Sub

    Private Sub doMenu()
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser
        If WMS.Logic.TaskManager.isAssigned(UserId, WMS.Lib.TASKTYPE.PICKING) Then
            Dim tm As New WMS.Logic.TaskManager(UserId, WMS.Lib.TASKTYPE.PICKING)
            tm.ExitTask()
        End If
        Session.Remove("CONFTYPE")
        Session.Remove("UomUnits")
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
                Dim objSkuClass As WMS.Logic.SkuClass = New WMS.Logic.SKU(pck.consingee, pck.sku).SKUClass

                For idx As Int32 = 0 To pck.oAttributeCollection.Count - 1
                    Dim oAtt As WMS.Logic.SkuClassLoadAttribute = objSkuClass.LoadAttributes(pck.oAttributeCollection.Keys(idx))
                    If oAtt.Name.ToUpper <> "WEIGHT" And oAtt.Name.ToUpper <> "WGT" Then
                        If oAtt.CaptureAtPicking = SkuClassLoadAttribute.CaptureType.Required Then
                            DO1.AddTextboxLine(pck.oAttributeCollection.Keys(idx), True, "Next")
                        Else
                            DO1.AddTextboxLine(pck.oAttributeCollection.Keys(idx))
                        End If
                    End If
                Next
            End If
        End If
        DO1.AddTextboxLine("CONFIRM", True, "next")
        DO1.AddTextboxLine("CONFIRMSKU")
        DO1.AddTextboxLine("UOMUNITS", True, "next", "UOMUNITS", "")

        'If Not IsNothing(Session("PCKPicklistActiveContainerIDSecond")) Then
        DO1.AddTextboxLine("CONTAINER")
        'End If

        DO1.AddTextboxLine("PRINTER")
        DO1.setVisibility("UOMDesc", True)
        Dim pcklist As Picklist = Session("PCKPicklist") 'New Picklist(pck.picklist)
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
        Try
            Select Case e.CommandText.ToLower
                Case "next"
                    doNext()
                Case "back"
                    doBack()
                Case "open/close container"
                    Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

                    If Not Session("PCKPicklistActiveContainerID") Is Nothing Then
                        Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("Screens/CloseContConfirm.aspx"))
                    Else
                        MessageQue.Enqueue(trans.Translate("Cannot Close Cotnainer - Container is blank"))
                    End If
                    '  doCloseContainer()

            End Select
        Catch ex As System.Threading.ThreadAbortException

        End Try

    End Sub

    Private Function ExtractAttributes() As AttributesCollection
        Dim pck As PickJob = Session("PCKPicklistPickJob")
        Dim Val As Object
        Dim oSku As String = pck.sku
        Dim oConsignee As String = pck.consingee
        Dim objSkuClass As WMS.Logic.SkuClass = New WMS.Logic.SKU(oConsignee, oSku).SKUClass
        If objSkuClass Is Nothing Then Return Nothing
        Dim oAttCol As New WMS.Logic.AttributesCollection
        For Each oAtt As WMS.Logic.SkuClassLoadAttribute In objSkuClass.LoadAttributes
            Dim req As Boolean = False
            If oAtt.Name.ToUpper <> "WEIGHT" And oAtt.Name.ToUpper <> "WGT" Then
                If oAtt.CaptureAtPicking = Logic.SkuClassLoadAttribute.CaptureType.Required Or oAtt.CaptureAtPicking = Logic.SkuClassLoadAttribute.CaptureType.Capture Then
                    Try
                        Select Case oAtt.Type
                            Case Logic.AttributeType.Boolean
                                Val = CType(DO1.Value(oAtt.Name), Boolean)
                            Case Logic.AttributeType.DateTime
                                'val = CType(DO1.Value(oAtt.Name), DateTime)
                                Val = DateTime.ParseExact(DO1.Value(oAtt.Name), Made4Net.Shared.AppConfig.DateFormat, Nothing)
                            Case Logic.AttributeType.Decimal
                                Val = CType(DO1.Value(oAtt.Name), Decimal)
                            Case Logic.AttributeType.Integer
                                Val = CType(DO1.Value(oAtt.Name), Int32)
                            Case Else
                                Val = DO1.Value(oAtt.Name)
                        End Select
                    Catch ex As Exception
                        If oAtt.CaptureAtReceiving = Logic.SkuClassLoadAttribute.CaptureType.Required Then
                            Throw New Made4Net.Shared.M4NException(New Exception, "Attribute Validation failed for " & oAtt.Name, "Attribute Validation failed for " & oAtt.Name)
                        End If
                    End Try
                End If
            End If
        Next
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
        'Dim sql As String = String.Format("select COUNT(1) from SKU inner join SKUCLSATT on SKU.CLASSNAME = SKUCLSATT.CLASSNAME where " & _
        '"CONSIGNEE = {0} and SKU= {1} and ATTRIBUTENAME ='WEIGHT'", Made4Net.Shared.FormatField(pConsignee), Made4Net.Shared.FormatField(pSKU))
        'If CType(Made4Net.DataAccess.DataInterface.ExecuteScalar(sql), Integer) > 0 Then
        '    Return True
        'End If
        'Return False
    End Function

End Class
