Imports Made4Net.DataAccess
Imports System.Globalization
Imports WMS.Logic
Imports Made4Net.Shared

Public Class CreateLoads
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents TECL As Made4Net.WebControls.TableEditor
    <CLSCompliant(False)> Protected WithEvents TS As Made4Net.WebControls.TabStrip
    Protected WithEvents TECreateLoad As Made4Net.WebControls.TableEditor
    Protected WithEvents DC1 As Made4Net.WebControls.DataConnector
    Protected WithEvents DTC As Made4Net.WebControls.DataTabControl
    Protected WithEvents Table1 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents Tbl As System.Web.UI.HtmlControls.HtmlTable

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region



#Region "Constructors"

    Public Sub New()

    End Sub

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As New DataSet
        Dim dr As DataRow
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)


        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)

        Select Case CommandName.ToLower
            Case "save"

                For Each dr In ds.Tables(0).Rows
                    'Dim sEXPIRYDATE As String

                    If Not RWMS.Logic.AppUtil.IsCreateLoad(dr("RECEIPT"), dr("RECEIPTLINE"), Message, True) Then
                        Throw New ApplicationException(Message)
                        Exit Sub
                    End If

                    'start validate load weight if needed
                    Dim wgtVal As New RWMS.Logic.WeightValidator
                    Dim oSku As New WMS.Logic.SKU(dr("CONSIGNEE"), dr("SKU"))

                    If wgtVal.WeightNeeded(oSku) Then

                        'If dr("WEIGHT").ToString <> "0" Then
                        Dim gotoOverride As Boolean = False
                        Dim gotoOverrideMessage As String = ""
                        Dim errMsg As String = ""
                        Dim WEIGHT As String = dr("WEIGHT").ToString
                        Dim UNITS As Decimal = Convert.ToDecimal(dr("UNITS"))
                        UNITS = oSku.ConvertToUnits(dr("LOADUOM")) * UNITS

                        If Not wgtVal.ValidateWeightSku(oSku, WEIGHT, UNITS, gotoOverride, gotoOverrideMessage, errMsg, False) Then
                            If gotoOverride Then
                                dr("WEIGHT") = WEIGHT
                                Throw New ApplicationException(errMsg & gotoOverrideMessage)
                            Else
                                Throw New ApplicationException(errMsg)
                            End If

                            Exit Sub
                        Else
                            dr("WEIGHT") = WEIGHT
                        End If
                    End If
                    'End If
                    'END validate load weight

                    Dim RECOVERRIDEVALIDATOR As String
                    RECOVERRIDEVALIDATOR = getRECOVERRIDEVALIDATOR(dr("CONSIGNEE"), dr("SKU"))


                    'If SkuMenage("EXPIRYDATE") Then
                    If EXPIRYDATENeeded(oSku, dr) And RECOVERRIDEVALIDATOR <> "" Then

                        'sEXPIRYDATE = dr("EXPIRYDATE").ToString(Made4Net.Shared.Util.GetSystemParameterValue("System_dateformat")) ' dr("EXPIRYDATE")
                        If IsDBNull(dr("EXPIRYDATE")) Then
                            Throw New ApplicationException(t.Translate("Illegal EXPIRYDATE format"))
                            Return
                        End If

                        Dim dEXPIRYDATE As Date = dr("EXPIRYDATE")

                        'If Not DateTime.TryParseExact(dr("EXPIRYDATE"), Made4Net.Shared.Util.GetSystemParameterValue("System_dateformat"), CultureInfo.InvariantCulture, DateTimeStyles.None, dEXPIRYDATE) Then
                        '    Throw New ApplicationException(t.Translate("Illegal EXPIRYDATE format"))
                        '    Return
                        'End If

                        If DateTime.Compare(dEXPIRYDATE, DateTime.Now) < 0 Then
                            Throw New ApplicationException(t.Translate("EXPIRYDATE Cannot be a past date"))
                            Return
                        End If

                        'New Validation with expression evaluation
                        Dim vals As New Made4Net.DataAccess.Collections.GenericCollection

                        vals.Add("EXPIRYDATE", dEXPIRYDATE.ToString(Made4Net.Shared.Util.GetSystemParameterValue("System_dateformat")))
                        vals.Add("DAYSTORECEIVE", getDAYSTORECEIVE(dr("CONSIGNEE"), dr("SKU")).ToString())

                        Dim exprEval As New Made4Net.Shared.Evaluation.ExpressionEvaluator(False)
                        exprEval.FieldValues = vals
                        'Dim statement As String = "[0];func:tmValidateEXPIRYDATE(FIELD:EXPIRYDATE,FIELD:DAYSTORECEIVE)"
                        Dim statement As String = "[0];func:" & RECOVERRIDEVALIDATOR & "(FIELD:EXPIRYDATE,FIELD:DAYSTORECEIVE)"
                        Dim ret As String = exprEval.Evaluate(statement)

                        Dim returnedResponse() As String = ret.Split(";")
                        If returnedResponse(0) = "0" Then
                            Throw New ApplicationException(returnedResponse(1))
                        ElseIf returnedResponse(0) = "-1" Then
                            Throw New ApplicationException(ret)
                        Else
                            Try
                                ' Dim rec1 As New WMS.Logic.Receiving(Sender, CommandName, XMLSchema, XMLData, Message) '
                                Dim ld() As WMS.Logic.Load
                                ld = CreateLoads(dr, WMS.Logic.GetCurrentUser)
                                CreateNonSpecificPickUpTasks(dr("WAREHOUSEAREA"), dr("LOCATION"), ld)
                            Catch ex As Exception
                                Throw New ApplicationException(ex.Message)
                            End Try
                        End If
                    Else
                        Try
                            'Dim rec1 As New WMS.Logic.Receiving(Sender, CommandName, XMLSchema, XMLData, Message)

                            Dim ld() As WMS.Logic.Load
                            ld = CreateLoads(dr, WMS.Logic.GetCurrentUser)
                            CreateNonSpecificPickUpTasks(dr("WAREHOUSEAREA"), dr("LOCATION"), ld)

                        Catch ex As Exception
                            Throw New ApplicationException(ex.Message)
                        End Try
                    End If
                Next

        End Select
    End Sub

    <CLSCompliant(False)>
    Function CreateLoads(ByVal dr As DataRow, ByVal pUser As String) As WMS.Logic.Load()
        Dim rcode As String, ssku As String, rline As Int32, holdrc As String, stat As String, qty As Double, loc As String, warehousearea As String, ldid As String, loaduom As String, numloads As Int32, printer As String, pDocumentType As String
        rcode = Conversion.Convert.ReplaceDBNull(dr("RECEIPT"))
        rline = Conversion.Convert.ReplaceDBNull(dr("RECEIPTLINE"))
        holdrc = Conversion.Convert.ReplaceDBNull(dr("HOLDRC"))
        loc = Conversion.Convert.ReplaceDBNull(dr("LOCATION"))
        warehousearea = Conversion.Convert.ReplaceDBNull(dr("WAREHOUSEAREA"))
        numloads = Conversion.Convert.ReplaceDBNull(dr("NUMLOADS"))
        printer = Conversion.Convert.ReplaceDBNull(dr("PRINTER"))
        If numloads = 1 Then
            ldid = Conversion.Convert.ReplaceDBNull(dr("LOADID"))
        End If
        stat = Conversion.Convert.ReplaceDBNull(dr("STATUS"))
        qty = Conversion.Convert.ReplaceDBNull(dr("UNITS"))
        loaduom = Conversion.Convert.ReplaceDBNull(dr("LOADUOM"))
        Dim oAttCol As AttributesCollection = WMS.Logic.SkuClass.ExtractReceivingAttributes(dr)
        Try
            Dim rh As ReceiptHeader = ReceiptHeader.GetReceipt(rcode)
            Dim rl As ReceiptDetail = rh.LINES.Line(rline)
            ssku = rl.SKU
            pDocumentType = rl.DOCUMENTTYPE
        Catch ex As Exception
            pDocumentType = ""
        End Try

        Dim rec As New WMS.Logic.Receiving()

        Dim ld() As WMS.Logic.Load = rec.CreateLoad(rcode, rline, ssku, ldid, loaduom, loc, warehousearea, qty, stat, holdrc, numloads, pUser, Nothing, oAttCol, printer, pDocumentType, Nothing, Nothing)
        Try
            RWMS.Logic.AppUtil.SetReceivedWeight(ld(0).CONSIGNEE, ld(0).SKU, ld(0).RECEIPT, ld(0).RECEIPTLINE)
        Catch ex As Exception
        End Try

        Return ld
    End Function

    Private Sub CreateNonSpecificPickUpTasks(ByVal WAREHOUSEAREA As String, ByVal pLocation As String, ByVal ld() As WMS.Logic.Load)
        For i As Int32 = 0 To ld.Length - 1
            Dim sSql As String = String.Format("select LOADID,PRIORITY from vNSPickupLoads where LOADID = '{0}'", ld(i).LOADID)
            Dim dt As New DataTable
            Made4Net.DataAccess.DataInterface.FillDataset(sSql, dt)

            If dt.Rows.Count > 0 Then
                Dim ts As New WMS.Logic.Task
                ts.FROMLOCATION = pLocation
                ts.PRIORITY = dt.Rows(0)("PRIORITY")
                ts.TASKTYPE = WMS.Lib.TASKTYPE.NSPICKUP
                ts.DOCUMENT = ld(i).RECEIPT ' DO1.Value("RECEIPT")
                ts.DOCUMENTLINE = ld(i).RECEIPTLINE
                ts.FROMWAREHOUSEAREA = WAREHOUSEAREA
                ts.TOWAREHOUSEAREA = WAREHOUSEAREA
                ts.ADDUSER = WMS.Logic.GetCurrentUser
                ts.EDITUSER = WMS.Logic.GetCurrentUser
                ts.EDITDATE = DateTime.Now
                ts.ADDDATE = DateTime.Now
                ts.Post()
            End If
        Next
    End Sub


    Private Function EXPIRYDATENeeded(ByVal pSKU As WMS.Logic.SKU, ByVal dr As DataRow) As Boolean
        If Not IsNothing(pSKU.SKUClass) Then
            For Each oAtt As WMS.Logic.SkuClassLoadAttribute In pSKU.SKUClass.LoadAttributes
                If oAtt.Name.ToUpper = "EXPIRYDATE" Then
                    If oAtt.CaptureAtReceiving = SkuClassLoadAttribute.CaptureType.Capture Then
                        If String.IsNullOrEmpty(dr(oAtt.Name.ToUpper).ToString) Then
                            Return False
                        Else
                            Return True
                        End If
                    ElseIf oAtt.CaptureAtReceiving = SkuClassLoadAttribute.CaptureType.Required Then
                        Return True
                    Else
                        Return False
                    End If
                End If
            Next
        End If
        Return False
    End Function


    Private Function getDAYSTORECEIVE(ByVal oConsignee As String, ByVal oSku As String) As Integer
        Dim ret As Integer = 0
        Dim sql As String = String.Format("SELECT ISNULL(DAYREC, 0) FROM SKUATTRIBUTE WHERE CONSIGNEE = '{0}' AND SKU = '{1}'", oConsignee, oSku)
        Try
            ret = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
        Catch ex As Exception
        End Try
        Return ret
    End Function


    Private Function getRECOVERRIDEVALIDATOR(ByVal oConsignee As String, ByVal oSku As String) As String
        Dim ret As String = String.Empty

        Dim objSkuClass As WMS.Logic.SkuClass = New WMS.Logic.SKU(oConsignee, oSku).SKUClass

        If Not objSkuClass Is Nothing Then
            Dim sql As String = String.Format("SELECT ISNULL(RECOVERRIDEVALIDATOR, '') FROM SKUCLSLOADATT WHERE CLASSNAME = '{0}' AND ATTRIBUTENAME = 'EXPIRYDATE'", objSkuClass.ClassName)
            Try
                ret = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
            Catch ex As Exception
            End Try
        End If
        Return ret
    End Function

#End Region



#Region "ViewState"

    Protected Overrides Function LoadPageStateFromPersistenceMedium() As Object
        Return WMS.WebCtrls.WebCtrls.Screen.LoadPageStateFromPersistenceMedium()
    End Function

    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        WMS.WebCtrls.WebCtrls.Screen.SavePageStateToPersistenceMedium(viewState, Me.Page, Screen1.ScreenID)
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
    End Sub

    Private Sub TECreateLoad_AfterItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles TECreateLoad.AfterItemCommand
        ' TECreateLoad.RefreshData()
        TECL.RefreshData()
    End Sub

    Private Sub TECreateLoad_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TECreateLoad.CreatedChildControls
        With TECreateLoad.ActionBar.Button("Save")
            '.ObjectName = "WMS.Logic.Receiving"
            '.ObjectDLL = "WMS.Logic.dll"
            .ObjectName = "WMS.WebApp.CreateLoads"
            .ObjectDLL = "WMS.WebApp.dll"
            .CommandName = "Save"
            .AccessKey = ""
        End With
    End Sub
    Private Sub TECL_RecordSelected(ByVal sender As Made4Net.WebControls.TableEditor, ByVal e As Made4Net.WebControls.TableEditorEventArgs) Handles TECL.RecordSelected
        Dim tds As DataTable = TECL.CreateDataTableForSelectedRecord()
        Dim str As String = DataInterface.ExecuteScalar("SELECT ISNULL(HUTYPE,'') FROM SKU WHERE CONSIGNEE='" & tds.Rows(0)("CONSIGNEE") & "' AND SKU='" & tds.Rows(0)("SKU") & "'")
        If str <> "" Then
            Session("HUTYPE") = str
        End If
        str = DataInterface.ExecuteScalar("SELECT ISNULL(INITIALSTATUS,'') FROM SKU WHERE CONSIGNEE='" & tds.Rows(0)("CONSIGNEE") & "' AND SKU='" & tds.Rows(0)("SKU") & "'")
        If str <> "" Then
            Session("INITIALSTATUS") = str
        End If
        str = DataInterface.ExecuteScalar("SELECT ISNULL(DEFAULTUOM,'') FROM SKU WHERE CONSIGNEE='" & tds.Rows(0)("CONSIGNEE") & "' AND SKU='" & tds.Rows(0)("SKU") & "'")
        If str <> "" Then
            Session("DEFAULTUOM") = str
        End If

    End Sub
End Class