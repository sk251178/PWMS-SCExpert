Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports WMS.Logic
Imports WMS.Lib
Imports WLTaskManager = WMS.Logic.TaskManager
Imports RWMS.Logic

<CLSCompliant(False)> Public Class CNTTASK1
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
        If Not IsPostBack Then
            If Session("TaskLoadCNTLoadId") Is Nothing Or Not WMS.Logic.Load.Exists(Session("TaskLoadCNTLoadId").ToString()) Then
                doMenu()
            End If
            Dim ld As New WMS.Logic.Load(Session("TaskLoadCNTLoadId").ToString())
            DO1.Value("CONSIGNEE") = ld.CONSIGNEE
            DO1.Value("SKU") = ld.SKU
            DO1.Value("LOCATION") = ld.LOCATION
            DO1.Value("WAREHOUSEAREA") = ld.WAREHOUSEAREA

            DO1.Value("LOADID") = ld.LOADID

            Dim ctrl As Made4Net.WebControls.MobileDropDown = DO1.Ctrl("UOM")

            'INIT SESSION FOR MULTI COUNT
            Dim oMultiUOM As MultiUOMUnits = ManageMutliUOMUnits.SetMutliUOMObj(ld.CONSIGNEE, ld.SKU, ld.LOADUOM, ld.UNITS)

            ctrl.AllOption = False
            ctrl.TableName = "SKUUOMDESC"
            ctrl.ValueField = "UOM"
            ctrl.TextField = "DESCRIPTION"
            ctrl.Where = String.Format("CONSIGNEE = '{0}' and SKU = '{1}'", ld.CONSIGNEE, ld.SKU)
            ctrl.DataBind()
            Try

                Dim oSku As New WMS.Logic.SKU(ld.CONSIGNEE, ld.SKU)

                ctrl.SelectedValue = oSku.DEFAULTUOM
                'ctrl.SelectedValue = ld.LOADUOM
                'SELECT d.DESCRIPTION FROM dbo.SKU AS s INNER JOIN dbo.SKUUOMDESC AS d ON s.CONSIGNEE = d.CONSIGNEE AND s.SKU = d.SKU AND s.DEFAULTUOM = d.UOM WHERE (s.CONSIGNEE = 'GELSONS') AND (s.SKU = '00002')
            Catch ex As Exception
            End Try
            Try
                DO1.Value("SKUDESC") = Made4Net.DataAccess.DataInterface.ExecuteScalar(String.Format("select skudesc from sku where sku = '{0}' and consignee = '{1}'", ld.SKU, ld.CONSIGNEE))
            Catch ex As Exception
            End Try
            'Start RWMS-1262/RWMS-1332  - get the RDTBlindCounting from warehouseparams
            Try
                Dim whpSQL As String
                whpSQL = "select PARAMVALUE from WAREHOUSEPARAMS where PARAMNAME='RDTBlindCounting'"
                Dim paramval As String = Made4Net.DataAccess.DataInterface.ExecuteScalar(whpSQL)
                If paramval = "1" Then
                    'prepopulate the load units and location
                    DO1.Value("TOUNITS") = ld.UNITS
                    DO1.Value("TOLOCATION") = ld.LOCATION
                End If

            Catch ex As Exception

            End Try
            'End RWMS-1262/RWMS-1332
        End If
    End Sub

    Private Sub doMenu()
        If Not Session("LoadCountTask") Is Nothing Then
            Dim oCntTask As WMS.Logic.CountTask = Session("LoadCountTask")
            oCntTask.ExitTask()
        End If
        Session.Remove("objMultiUOMUnits")
        Session.Remove("LoadCountTask")
        Session.Remove("TaskLoadCNTLoadId")
        Session.Remove("TSKTaskId")
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub doNext()
        'If Page.IsValid Then
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Try
            'Dim ld As New WMS.Logic.Load(Session("TaskLoadCNTLoadId"))
            'ld.Count(DO1.Value("TOUNITS"), DO1.Value("UOM"), DO1.Value("TOLOCATION"), DO1.Value("TOWAREHOUSEAREA"), WMS.Logic.GetCurrentUser)
            If Not Session("LoadCountTask") Is Nothing Then
                Dim oCntTask As WMS.Logic.CountTask = Session("LoadCountTask")
                Session("TaskID") = oCntTask.TASK
                Dim oCounting As New Counting(oCntTask.COUNTID)
                'Build and fill count job object
                Dim oCountJob As CountingJob = oCntTask.getCountJob(oCounting)
                Dim ld As New WMS.Logic.Load(Session("TaskLoadCNTLoadId").ToString())

                Dim errMessage As String = String.Empty
                If Not ManageMutliUOMUnits.AddUOMUnits(DO1.Value("UOM"), DO1.Value("TOUNITS"), errMessage, True) Then
                    'HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  errMessage)
                    'DO1.Value("TOUNITS") = ""
                    'Return
                End If

                ManageMutliUOMUnits.SetValUOMUnitsAfterDrow(DO1)

                Dim SK As New WMS.Logic.SKU(ld.CONSIGNEE, ld.SKU)

                oCountJob.CountedQty = ManageMutliUOMUnits.GetTotalEachUnits() ' ManageMutliUOMUnits.GetTotal() 'DO1.Value("TOUNITS")
                oCountJob.UOM = SK.LOWESTUOM 'ld.LOADUOM ' SK.LOWESTUOM '"EACH" 'ld.LOADUOM 'DO1.Value("UOM")
                oCountJob.Location = DO1.Value("TOLOCATION")
                oCountJob.Warehousearea = DO1.Value("WAREHOUSEAREA")  'DO1.Value("TOWAREHOUSEAREA")

                oCountJob.LoadCountVerified = False
                oCountJob.LocationCountingLoadCounted = True
                Try
                    oCountJob.CountingAttributes = ExtractAttributeValues()
                Catch m4nEx As Made4Net.Shared.M4NException
                    Dim coll As New Made4Net.DataAccess.Collections.GenericCollection()
                    coll.Add("param0", m4nEx.Params("attribute"))
                    Made4Net.Mobile.MessageQue.Enqueue(t.Translate("Attribute #param0# is not Valid", coll))
                    Return
                Catch ex As Exception
                    Made4Net.Mobile.MessageQue.Enqueue(t.Translate(ex.ToString()))
                    Return
                End Try
                If Not oCntTask.Count(oCounting, oCountJob, WMS.Logic.GetCurrentUser) Then
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Tolerance exceeded - please confirm quantity"))
                    Session("CountedUnitsForVerification") = oCountJob.CountedQty
                    Session("UOMForVerification") = oCountJob.UOM
                    Session("AttributesForVerification") = oCountJob.CountingAttributes
                    Dim SrcScreen As String
                    'If Session("CountingSourceScreen") Is Nothing Then
                    SrcScreen = "CNTTASK"
                    'Else
                    '   SrcScreen = Session("CountingSourceScreen")
                    'End If
                    'Tolocation fix 05/03/14
                    Session("ToLocation") = oCountJob.Location
                    Response.Redirect(MapVirtualPath("Screens/CNTTASKVERFICATION.aspx?SRCSCREEN=" & SrcScreen))
                End If
            End If
        Catch ex As Threading.ThreadAbortException
            Return
        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Return
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.Message)
            Return
        End Try
        Session.Remove("TSKTaskId")
        Session.Remove("TaskLoadCNTLoadId")
        ManageMutliUOMUnits.Clear(True)
        If Not Session("CountingSourceScreen") Is Nothing Then
            If MobileUtils.ShouldRedirectToTaskSummary Then
                MobileUtils.RedirectToTaskSummary(Session("CountingSourceScreen"))
            Else
                Response.Redirect(MapVirtualPath("Screens/" & Session("CountingSourceScreen") & ".aspx"))
            End If
        Else
            If MobileUtils.ShouldRedirectToTaskSummary() Then
                MobileUtils.RedirectToTaskSummary("CNTTASK")
            Else
                Response.Redirect(MapVirtualPath("Screens/CNTTASK.aspx"))
            End If
        End If
        'End If
    End Sub

    Private Function ExtractAttributeValues() As WMS.Logic.AttributesCollection
        Dim ld As New WMS.Logic.Load(Convert.ToString(Session("TaskLoadCNTLoadId")))
        Dim oSku As String = ld.SKU  'DO1.Value("SKU") ' Session("CreateLoadSKU")
        Dim oConsignee As String = ld.CONSIGNEE ' DO1.Value("CONSIGNEE") 'Session("CreateLoadConsignee")
        Dim objSkuClass As WMS.Logic.SkuClass = New WMS.Logic.SKU(oConsignee, oSku).SKUClass
        If objSkuClass Is Nothing Then Return Nothing
        Dim oAttCol As New WMS.Logic.AttributesCollection
        For Each oAtt As WMS.Logic.SkuClassLoadAttribute In objSkuClass.LoadAttributes
            If oAtt.CaptureAtCounting = Logic.SkuClassLoadAttribute.CaptureType.Required OrElse
                        oAtt.CaptureAtCounting = Logic.SkuClassLoadAttribute.CaptureType.Capture Then
                Dim val As Object
                Try
                    Select Case oAtt.Type
                        Case Logic.AttributeType.Boolean
                            val = CType(DO1.Value(oAtt.Name), Boolean)
                        Case Logic.AttributeType.DateTime
                            If String.IsNullOrEmpty(DO1.Value(oAtt.Name)) Then
                                val = DateTime.MinValue
                            Else
                                val = DateTime.ParseExact(DO1.Value(oAtt.Name), Made4Net.Shared.AppConfig.DateFormat, Nothing)
                                'val = CType(DO1.Value(oAtt.Name), DateTime)
                            End If
                        Case Logic.AttributeType.Decimal
                            If String.IsNullOrEmpty(DO1.Value(oAtt.Name)) Then
                                val = Decimal.MinValue
                            Else
                                val = CType(DO1.Value(oAtt.Name), Decimal)
                            End If
                        Case Logic.AttributeType.Integer
                            If String.IsNullOrEmpty(DO1.Value(oAtt.Name)) Then
                                val = Integer.MinValue
                            Else
                                val = CType(DO1.Value(oAtt.Name), Int32)
                            End If
                        Case Logic.AttributeType.String
                            val = DO1.Value(oAtt.Name)
                    End Select
                Catch ex As Exception
                    val = Nothing
                End Try
                If Not validateAttributeValue(oAtt.Type, val, oAtt.CaptureAtCounting) Then
                    Dim m4nExc As New Made4Net.Shared.M4NException(New Exception(), "Invalid attribute value", "Invalid attribute value")
                    m4nExc.Params.Add("attribute", oAtt.Name)
                    Throw m4nExc
                End If
                oAttCol.Add(oAtt.Name, val)
            End If
        Next
        Return oAttCol
    End Function

    Private Function validateAttributeValue(ByVal pType As WMS.Logic.AttributeType, ByVal pValue As Object, ByVal pCaptureType As WMS.Logic.SkuClassLoadAttribute.CaptureType) As Boolean
        'Try
        If pValue Is Nothing Then
            Return False
        End If
        If pCaptureType = SkuClassLoadAttribute.CaptureType.Capture Then
            Return True
        End If
        If pType = AttributeType.Boolean Then
            Try
                Dim boolVal As Boolean = CType(pValue, Boolean)
            Catch ex As Exception
                Return False
            End Try
        ElseIf pType = AttributeType.DateTime Then
            Try
                Dim dateVal As DateTime = pValue
                If dateVal = DateTime.MinValue Then Return False
            Catch ex As Exception
                Return False
            End Try
        ElseIf pType = AttributeType.Decimal Then
            Try
                Dim decVal As Decimal = Decimal.Parse(pValue)
                If decVal = Decimal.MinValue Then Return False
            Catch ex As Exception
                Return False
            End Try
        ElseIf pType = AttributeType.Integer Then
            Try
                Dim intVal As Integer = Integer.Parse(pValue)
                If intVal = Integer.MinValue Then Return False
            Catch ex As Exception
                Return False
            End Try
        ElseIf pType = AttributeType.String Then
            Try
                If pValue Is Nothing Or Convert.ToString(pValue) = "" Then
                    If pCaptureType = SkuClassLoadAttribute.CaptureType.Capture Then
                        Return True
                    Else
                        Return False
                    End If
                End If
                Return True
            Catch ex As Exception
                Return False
            End Try
        End If
        Return True
    End Function


    Private Sub doBack()
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser
        If WMS.Logic.TaskManager.isAssigned(UserId, WMS.Lib.TASKTYPE.LOADCOUNTING, LogHandler.GetRDTLogger()) Then
            Dim tm As WLTaskManager = WLTaskManager.NewTaskManagerForUserAndTaskType(UserId, TASKTYPE.LOADCOUNTING, LogHandler.GetRDTLogger())
            tm.ExitTask()
        End If
        Session.Remove("objMultiUOMUnits")
        Session.Remove("TSKTaskId")
        Session.Remove("TaskLoadCNTLoadId")
        Response.Redirect(MapVirtualPath("Screens/CNTTASK.aspx"))
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddLabelLine("LOADID")
        DO1.AddLabelLine("CONSIGNEE", Nothing, "", Session("3PL"))
        DO1.AddLabelLine("SKU")
        DO1.AddLabelLine("SKUDESC")
        DO1.AddLabelLine("LOCATION")
        DO1.AddLabelLine("WAREHOUSEAREA")
        'DO1.AddSpacer()
        'DO1.AddLabelLine("UOM1")
        'DO1.AddLabelLine("UOM2")
        'DO1.AddLabelLine("UOM3")
        'DO1.AddLabelLine("UOM4")
        'DO1.AddLabelLine("UOM5")
        'DO1.AddLabelLine("UOM6")
        If Session("TaskLoadCNTLoadId") Is Nothing Then
            doMenu()
        End If
        Dim ld As New WMS.Logic.Load(Session("TaskLoadCNTLoadId").ToString())
        Dim oMultiUOM As MultiUOMUnits = ManageMutliUOMUnits.SetMutliUOMObj(ld.CONSIGNEE, ld.SKU, ld.LOADUOM, ld.UNITS)
        ManageMutliUOMUnits.DROWLABLES(DO1)
        ManageMutliUOMUnits.SetValUOMUnitsAfterDrow(DO1)

        DO1.AddDropDown("UOM")
        DO1.AddTextboxLine("TOUNITS") ', True, "next")
        DO1.AddTextboxLine("TOLOCATION") ', True, "next")
        'DO1.AddTextboxLine("TOWAREHOUSEAREA")
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "next"

                doNext()
                DO1.Value("TOUNITS") = ""
            Case "back"
                ManageMutliUOMUnits.Clear(True)
                doBack()
            Case "menu"
                ManageMutliUOMUnits.Clear(True)
                doMenu()
            Case "addunits"
                Dim errMessage As String = String.Empty
                If Not ManageMutliUOMUnits.AddUOMUnits(DO1.Value("UOM"), DO1.Value("TOUNITS"), errMessage, True) Then
                    'HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  errMessage)
                    'DO1.Value("TOUNITS") = ""
                    'Return
                End If

                ManageMutliUOMUnits.SetValUOMUnitsAfterDrow(DO1)
                DO1.Value("TOUNITS") = ""
            Case "clearunits"
                ManageMutliUOMUnits.Clear()
                ManageMutliUOMUnits.SetValUOMUnitsAfterDrow(DO1)
        End Select
    End Sub
End Class