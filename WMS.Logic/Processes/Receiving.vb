Imports System.data
Imports Made4Net.DataAccess
Imports Made4Net.Shared

<CLSCompliant(False)> Public Class Receiving

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As New DataSet
        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        If CommandName.ToLower = "save" Then
            If ds.Tables(0).Rows.Count > 0 Then
                CreateLoads(ds.Tables(0).Rows(0), Common.GetCurrentUser)
                Message = "Load Created"
            End If
        ElseIf CommandName = "BlindReceive" Then
            CreateBlindReceivingLoads(ds.Tables(0).Rows(0), Common.GetCurrentUser)
            Message = "Load Created"
        ElseIf CommandName = "ReceiveFull" Then
            CreateBlindReceivingLoads(ds.Tables(0).Rows(0), Common.GetCurrentUser)
            Message = "Load Created"
        End If
    End Sub

    Public Sub New()
    End Sub

    Function CreateBlindReceivingLoads(ByVal dr As DataRow, ByVal pUser As String) As Load()
        Dim rcode As String, rline As Int32, holdrc As String, consignee As String, sku As String, stat As String, qty As Double, loc As String, warehousearea As String, ldid As String, loaduom As String, numloads As Int32, printer As String, pDocumentType As String
        rcode = Conversion.Convert.ReplaceDBNull(dr("RECEIPT"))
        consignee = Conversion.Convert.ReplaceDBNull(dr("consignee"))
        sku = Conversion.Convert.ReplaceDBNull(dr("sku"))
        holdrc = Conversion.Convert.ReplaceDBNull(dr("HOLDRC"))
        loc = Conversion.Convert.ReplaceDBNull(dr("LOCATION"))
        warehousearea = Conversion.Convert.ReplaceDBNull(dr("WAREHOUSEAREA"))
        If Not WMS.Logic.Location.Exists(loc, warehousearea) Then
            Throw New M4NException(New Exception(), "Location does not exist", "Location does not exist")
        End If
        numloads = Conversion.Convert.ReplaceDBNull(dr("NUMLOADS"))
        If numloads = 1 Then
            ldid = Conversion.Convert.ReplaceDBNull(dr("LOADID"))
        End If
        stat = Conversion.Convert.ReplaceDBNull(dr("STATUS"))
        qty = Conversion.Convert.ReplaceDBNull(dr("UNITS"))
        loaduom = Conversion.Convert.ReplaceDBNull(dr("LOADUOM"))
        Dim oAttCol As AttributesCollection = SkuClass.ExtractReceivingAttributes(dr)
        Try
            Dim calcQty As Decimal = New SKU(consignee, sku).ConvertToUnits(loaduom) * qty
            Dim rh As ReceiptHeader = ReceiptHeader.GetReceipt(rcode)
            Dim rl As ReceiptDetail = rh.AddLineFromBlindReceiving(consignee, sku, calcQty, pUser)
            pDocumentType = rl.DOCUMENTTYPE
            rline = rl.RECEIPTLINE
        Catch ex As Exception
            pDocumentType = ""
        End Try
        Return CreateLoad(rcode, rline, sku, ldid, loaduom, loc, warehousearea, qty, stat, holdrc, numloads, pUser, Nothing, oAttCol, printer, pDocumentType, Nothing, Nothing)
    End Function

    Function CreateBlindReceivingLoads(ByVal pReceipt As String, ByVal pConsignee As String, _
    ByVal pSKU As String, ByVal pHOLDRC As String, ByVal pLocation As String, ByVal pWarehouseArea As String, _
    ByVal pNumLoads As Integer, ByVal pLoadID As String, ByVal pStatus As String, ByVal pUnits As Decimal, _
    ByVal pUOM As String, ByVal oAttributesCollection As AttributesCollection, ByVal pUser As String) As Load()
        Dim rcode As String, rline As Int32, holdrc As String, consignee As String, sku As String, stat As String, qty As Double, _
            loc As String, warehousearea As String, ldid As String, loaduom As String, numloads As Int32, printer As String, pDocumentType As String
        rcode = pReceipt  'Conversion.Convert.ReplaceDBNull(dr("RECEIPT"))
        consignee = pConsignee 'Conversion.Convert.ReplaceDBNull(dr("consignee"))
        sku = pSKU 'Conversion.Convert.ReplaceDBNull(dr("sku"))
        holdrc = pHOLDRC ' Conversion.Convert.ReplaceDBNull(dr("HOLDRC"))

        loc = pLocation  'Conversion.Convert.ReplaceDBNull(dr("LOCATION"))
        warehousearea = pWarehouseArea 'Conversion.Convert.ReplaceDBNull(dr("WAREHOUSEAREA"))

        numloads = pNumLoads 'Conversion.Convert.ReplaceDBNull(dr("NUMLOADS"))
        If numloads = 1 Then
            ldid = pLoadID  'Conversion.Convert.ReplaceDBNull(dr("LOADID"))
        End If
        stat = pStatus  'Conversion.Convert.ReplaceDBNull(dr("STATUS"))
        qty = pUnits 'Conversion.Convert.ReplaceDBNull(dr("UNITS"))
        loaduom = pUOM 'Conversion.Convert.ReplaceDBNull(dr("LOADUOM"))
        'Dim oAttCol As AttributesCollection = SkuClass.ExtractReceivingAttributes(dr)
        Dim skuObj As New WMS.Logic.SKU(consignee, sku)

        Dim oSkuClass As SkuClass = skuObj.SKUClass
        'Attribute Validation
        If Not oSkuClass Is Nothing Then
            For Each oLoadAtt As SkuClassLoadAttribute In oSkuClass.LoadAttributes
                If oLoadAtt.CaptureAtReceiving = SkuClassLoadAttribute.CaptureType.Capture Or oLoadAtt.CaptureAtReceiving = SkuClassLoadAttribute.CaptureType.Required Then
                    ' Validate for required values
                    If oLoadAtt.CaptureAtReceiving = SkuClassLoadAttribute.CaptureType.Required Then
                        If oAttributesCollection Is Nothing Then Throw New ApplicationException(String.Format("Attribute {0} value was not supplied", oLoadAtt.Name))
                        If oAttributesCollection(oLoadAtt.Name) Is Nothing Or oAttributesCollection(oLoadAtt.Name) Is DBNull.Value Then
                            Throw New ApplicationException(String.Format("Attribute {0} value was not supplied", oLoadAtt.Name))
                        End If
                    End If
                    ' Validate that the attributes supplied are valid
                    ValidateAttributeByType(oLoadAtt, oAttributesCollection(oLoadAtt.Name))
                End If
            Next
        End If
        'End Of Attribute Validation
        Dim rh As ReceiptHeader
        Dim rl As ReceiptDetail
        Dim calcQty As Decimal = skuObj.ConvertToUnits(loaduom) * qty
        Try
            rh = ReceiptHeader.GetReceipt(rcode)
            rl = rh.AddLineFromBlindReceiving(consignee, sku, calcQty, pUser)
            pDocumentType = rl.DOCUMENTTYPE
            rline = rl.RECEIPTLINE
        Catch ex As Exception
            pDocumentType = ""
        End Try

        Dim ld As WMS.Logic.Load()
        Try
            'ld = CreateLoad(rcode, rline, sku, ldid, loaduom, loc, warehousearea, skuObj.ConvertToUnits(loaduom) * qty, stat, holdrc, numloads, pUser, oAttributesCollection, printer, pDocumentType, Nothing, Nothing)
            ld = CreateLoad(rcode, rline, sku, ldid, loaduom, loc, warehousearea, qty, stat, holdrc, numloads, pUser, Nothing, oAttributesCollection, printer, pDocumentType, Nothing, Nothing)
        Catch ex As Exception
            rl.QTYEXPECTED = rl.QTYEXPECTED - calcQty
            rh.SaveLine(rl, WMS.Lib.USERS.SYSTEMUSER)
            Throw ex
        End Try
        Return ld
    End Function
    '---10-02-2013-oded

    '---

    Protected Function CreateLoads(ByVal dr As DataRow, ByVal pUser As String) As Load()
        Dim rcode As String, ssku As String, rline As Int32, holdrc As String, stat As String, qty As Double, loc As String, warehousearea As String, ldid As String, loaduom As String, numloads As Int32, printer As String, pDocumentType As String
        rcode = Conversion.Convert.ReplaceDBNull(dr("RECEIPT"))
        rline = Conversion.Convert.ReplaceDBNull(dr("RECEIPTLINE"))
        holdrc = Conversion.Convert.ReplaceDBNull(dr("HOLDRC"))
        loc = Conversion.Convert.ReplaceDBNull(dr("LOCATION"))
        warehousearea = Conversion.Convert.ReplaceDBNull(dr("WAREHOUSEAREA"))

        If Not WMS.Logic.Location.Exists(loc, warehousearea) Then
            Throw New M4NException(New Exception(), "Location does not exist", "Location does not exist")
        End If

        numloads = Conversion.Convert.ReplaceDBNull(dr("NUMLOADS"))
        printer = Conversion.Convert.ReplaceDBNull(dr("PRINTER"))
        If numloads = 1 Then
            ldid = Conversion.Convert.ReplaceDBNull(dr("LOADID"))
        End If
        stat = Conversion.Convert.ReplaceDBNull(dr("STATUS"))
        qty = Conversion.Convert.ReplaceDBNull(dr("UNITS"))
        loaduom = Conversion.Convert.ReplaceDBNull(dr("LOADUOM"))
        Dim oAttCol As AttributesCollection = SkuClass.ExtractReceivingAttributes(dr)
        Try
            Dim rh As ReceiptHeader = ReceiptHeader.GetReceipt(rcode)
            Dim rl As ReceiptDetail = rh.LINES.Line(rline)
            ssku = rl.SKU
            pDocumentType = rl.DOCUMENTTYPE
        Catch ex As Exception
            pDocumentType = ""
        End Try
        Return CreateLoad(rcode, rline, ssku, ldid, loaduom, loc, warehousearea, qty, stat, holdrc, numloads, pUser, Nothing, oAttCol, printer, pDocumentType, Nothing, Nothing)
    End Function

    Public Function CreateLoad(ByVal pReceipt As String, ByVal pLine As Int32, ByVal pSku As String, ByVal pLoadId As String, ByVal pUOM As String, ByVal pLocation As String, ByVal pWarehousearea As String, ByVal pQty As Double, ByVal pStat As String, ByVal pHoldRc As String, ByVal pNumLoads As Int32, ByVal pUser As String, ByVal oLogger As LogHandler, ByVal oAttributesCollection As AttributesCollection, ByVal lblPrinter As String, ByVal pDocumentType As String, ByVal pHandlingUnitId As String, ByVal pHandlingUnitType As String) As Load()
        If pQty <= 0 Then
            'Throw New Made4Net.Shared.M4NException(New Exception, "Cannot create load with negative quantity", "Cannot create load with negative quantity")
            Throw New Made4Net.Shared.M4NException(New Exception, "Can not create load. Quantity must be greater than 0", "Can not create load. Quantity must be greater than 0")
        End If
        Dim pQtyInt As Decimal

        If Decimal.TryParse(pQty, pQtyInt) Then

            If Math.Round(pQty) <> pQty Then
                Throw New Made4Net.Shared.M4NException(New Exception, "only full units can be confirmed", "only full units can be confirmed")

            End If

        Else
            Throw New Made4Net.Shared.M4NException(New Exception, "Can not create load. Quantity is not a number", "Can not create load. Quantity is not a number")

        End If



        Dim rh As ReceiptHeader = ReceiptHeader.GetReceipt(pReceipt)

        Dim ld As Load
        Dim oSkuClass As SkuClass
        Dim oSku As SKU
        Dim isSubstituteSku As Boolean = False
        Dim AutoPrintLoadLabels As Boolean = False
        Dim rl As ReceiptDetail = rh.LINES.Line(pLine)

        'Validate substitute sku
        If rl.SKU.ToLower <> pSku.ToLower Then
            Dim tmpSku As New SKU(rl.CONSIGNEE, rl.SKU)
            If Not tmpSku.ContainsSubstituteSku(pSku) Then
                Throw New Made4Net.Shared.M4NException(New Exception, "SKU is not Valid sku for this receipt", "SKU is not Valid sku for this receipt")
            End If
            oSku = New SKU(rl.CONSIGNEE, pSku)
            ThrowExceptionOnNewSku(oSku)
            oSkuClass = oSku.SKUClass
            isSubstituteSku = True
            rl.UpdateOriginalSku(pSku, pUser)
        Else
            oSku = New SKU(rl.CONSIGNEE, rl.SKU)
            ThrowExceptionOnNewSku(oSku)
            oSkuClass = oSku.SKUClass
        End If

        If (pNumLoads = 1 And (pLoadId Is Nothing Or pLoadId = "")) Then
            'oSkuClass = New SKU(rl.CONSIGNEE, rl.SKU).SKUClass            
            If Not Consignee.AutoGenerateLoadID(rl.CONSIGNEE) Then
                Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Invalid Load ID", "Invalid Load ID")
                Throw m4nEx
            End If
        End If

        'Attribute Validation
        If Not oSkuClass Is Nothing Then
            For Each oLoadAtt As SkuClassLoadAttribute In oSkuClass.LoadAttributes
                Dim typeValidationResult As Int32

                If oLoadAtt.CaptureAtReceiving = SkuClassLoadAttribute.CaptureType.Capture Then
                    If oAttributesCollection Is Nothing Then Continue For
                    Select Case oLoadAtt.Type
                        Case AttributeType.String
                            If oAttributesCollection(oLoadAtt.Name) Is Nothing OrElse oAttributesCollection(oLoadAtt.Name) Is DBNull.Value OrElse oAttributesCollection(oLoadAtt.Name) = "" Then Continue For
                        Case AttributeType.Decimal, AttributeType.Integer
                            If oAttributesCollection(oLoadAtt.Name) Is Nothing OrElse oAttributesCollection(oLoadAtt.Name) Is DBNull.Value Then Continue For
                        Case AttributeType.DateTime
                            If oAttributesCollection(oLoadAtt.Name) Is Nothing OrElse oAttributesCollection(oLoadAtt.Name) Is DBNull.Value Then Continue For
                            Try
                                If oAttributesCollection(oLoadAtt.Name) = String.Empty Then
                                    Continue For
                                End If
                            Catch ex As Exception
                            End Try
                            Try
                                If oAttributesCollection(oLoadAtt.Name) = DateTime.MinValue Then
                                    Continue For
                                End If
                            Catch ex As Exception
                            End Try
                        Case AttributeType.Boolean
                            If oAttributesCollection(oLoadAtt.Name) Is Nothing OrElse oAttributesCollection(oLoadAtt.Name) Then Continue For
                    End Select
                ElseIf oLoadAtt.CaptureAtReceiving = SkuClassLoadAttribute.CaptureType.Required Then
                    ' Validate for required values
                    If oAttributesCollection Is Nothing Then Throw New ApplicationException(String.Format("Attribute {0} value was not supplied", oLoadAtt.Name))
                    'Commented for PWMS-899(RWMS-1001) Start
                    'If oAttributesCollection(oLoadAtt.Name) Is Nothing Or oAttributesCollection(oLoadAtt.Name) Is DBNull.Value Then
                    'Commented for PWMS-899(RWMS-1001) End

                    'Commented for PWMS-899(RWMS-1001) Start
                    If oAttributesCollection(oLoadAtt.Name) Is Nothing Or oAttributesCollection(oLoadAtt.Name) Is DBNull.Value Or oAttributesCollection(oLoadAtt.Name) Is "" Then
                        'Commented for PWMS-899(RWMS-1001) End


                        Throw New ApplicationException(String.Format("Attribute {0} value was not supplied", oLoadAtt.Name))
                    End If
                End If
                If oLoadAtt.CaptureAtReceiving <> SkuClassLoadAttribute.CaptureType.NoCapture Then
                    ' Validate that the attributes supplied are valid
                    typeValidationResult = ValidateAttributeByType(oLoadAtt, oAttributesCollection(oLoadAtt.Name))
                    If typeValidationResult = -1 Then
                        ' RWMS-1258 Start
                        'Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Attribute #param0# is not Valid", "Attribute #param0# is not Valid")
                        Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, String.Format("Attribute {0} is not Valid", oLoadAtt.Name), String.Format("Attribute {0} is not Valid", oLoadAtt.Name))
                        'm4nEx.Params.Add("AttName", oLoadAtt.Name)
                        ' RWMS-1258 End
                        Throw m4nEx
                    ElseIf typeValidationResult = 0 Then
                        'Continue For
                    End If
                    ' Validator
                    If Not oLoadAtt.ReceivingValidator Is Nothing Then
                        'Old Validation Code
                        'If Not oLoadAtt.ReceivingValidator.Validate(rh, pLine, pUOM, pLocation, pQty, pStat, pHoldRc, pUser, oLoadAtt.Name, oAttributesCollection(oLoadAtt.Name), oAttributesCollection) Then
                        '    Throw New ApplicationException("Invalid Attribute Value " & oLoadAtt.Name)
                        'End If

                        'New Validation with expression evaluation
                        Dim vals As New Made4Net.DataAccess.Collections.GenericCollection
                        vals.Add(oLoadAtt.Name, oAttributesCollection(oLoadAtt.Name))
                        vals.Add("RECEIPT", pReceipt)
                        vals.Add("LINE", CStr(pLine))
                        vals.Add("LOADSTATUS", pStat)
                        Dim ret As String = oLoadAtt.Evaluate(SkuClassLoadAttribute.EvaluationType.Receiving, vals)
                        Dim returnedResponse() As String = ret.Split(";")
                        'If ret = "-1" Then
                        If returnedResponse(0) = "-1" Then
                            If returnedResponse.Length > 1 Then
                                Throw New M4NException(New Exception, "Invalid Attribute Value " & oLoadAtt.Name & ". " & returnedResponse(1), "Invalid Attribute Value " & oLoadAtt.Name & "." & returnedResponse(1))
                            Else
                                Throw New ApplicationException("Invalid Attribute Value " & oLoadAtt.Name)
                            End If
                            'Throw New ApplicationException("Invalid Attribute Value " & oLoadAtt.Name)
                        Else
                            oAttributesCollection(oLoadAtt.Name) = ret
                        End If
                    End If
                End If
            Next
        End If
        'End Of Attribute Validation

        AutoPrintLoadLabels = Consignee.AutoPrintLoadIdOnReceiving(rh.LINES.Line(pLine).CONSIGNEE)
        Dim oLoadsArray As New ArrayList
        For idx As Int32 = 0 To pNumLoads - 1
            If (pLoadId Is Nothing Or pLoadId = "" Or pNumLoads > 1) Then
                pLoadId = Load.GenerateLoadId()
            End If
            ld = rh.CreateLoad(pLine, pLoadId, pUOM, pLocation, pWarehousearea, pQty, pStat, pHoldRc, oAttributesCollection, pUser, oLogger, pDocumentType)
            If pDocumentType = WMS.Lib.DOCUMENTTYPES.FLOWTHROUGH Then
                SetLoadDestinationLocation(rl, ld, pUser)
            End If
            'Create new handling unit if needed and place the load on it.
            If pHandlingUnitId <> "" Or pHandlingUnitType <> "" Then
                Dim oCont As Container = CreateContainer(pHandlingUnitId, pHandlingUnitType, pLocation, pWarehousearea)
                oCont.Place(ld, WMS.Logic.Common.GetCurrentUser)
            End If
            Try
                WMS.Logic.Merge.Put(ld, pLocation, pWarehousearea)
            Catch ex As Exception
            End Try
            If AutoPrintLoadLabels Then
                '--------------- We need to pass the document type to print the proper labels
                Select Case pDocumentType
                    Case "", WMS.Lib.DOCUMENTTYPES.INBOUNDORDER
                        ld.PrintLabel(lblPrinter)
                    Case WMS.Lib.DOCUMENTTYPES.FLOWTHROUGH
                        ld.PrintFlowthroughLabel(lblPrinter)
                End Select
            End If
            oLoadsArray.Add(ld)
            pLoadId = Nothing
        Next

        'update the warehouse activity table
        Dim oWHActivity As New WHActivity()
        oWHActivity.LOCATION = pLocation
        'Added for RWMS-1692 and RWMS-1391 
        oWHActivity.WAREHOUSEAREA = pWarehousearea
        'Ended for RWMS-1692 and RWMS-1391
        oWHActivity.USERID = pUser
        oWHActivity.ACTIVITY = WMS.Lib.Actions.Audit.CREATELOAD
        oWHActivity.ACTIVITYTIME = DateTime.Now
        oWHActivity.EDITDATE = DateTime.Now
        oWHActivity.EDITUSER = pUser
        oWHActivity.Post()

        Return oLoadsArray.ToArray(ld.GetType)
    End Function

    Public Function CreateLoadFromMultipleLines(ByVal pReceipt As String, ByVal pSku As String, ByVal pUOM As String, ByVal pLocation As String, ByVal pWarehousearea As String, ByVal pQty As Double, ByVal pStat As String, ByVal pHoldRc As String, ByVal pUser As String, ByVal oLogger As LogHandler, ByVal oAttributesCollection As AttributesCollection, ByVal lblPrinter As String, ByVal pHandlingUnitId As String, ByVal pHandlingUnitType As String) As Load()
        Dim rh As New ReceiptHeader(pReceipt)
        Dim rd As ReceiptDetail
        Dim ld As Load()
        Dim oLoadsArray As New ArrayList
        Dim oSku As New SKU(rh.LINES(0).CONSIGNEE, pSku)
        Dim totalUnitsAvailable As Decimal = oSku.ConvertToUnits(pUOM) * pQty
        For Each rd In rh.LINES
            If rd.SKU.ToLower = pSku.ToLower And totalUnitsAvailable > 0 Then
                Dim CurrLoadQty As Decimal
                If rd.QTYEXPECTED <= totalUnitsAvailable Then
                    CurrLoadQty = rd.QTYEXPECTED
                    totalUnitsAvailable -= CurrLoadQty
                Else
                    CurrLoadQty = totalUnitsAvailable
                    totalUnitsAvailable -= CurrLoadQty
                End If
                CurrLoadQty = oSku.ConvertUnitsToUom(pUOM, CurrLoadQty)
                ld = CreateLoad(pReceipt, rd.RECEIPTLINE, pSku, "", pUOM, pLocation, pWarehousearea, CurrLoadQty, pStat, pHoldRc, 1, pUser, oLogger, oAttributesCollection, lblPrinter, rd.DOCUMENTTYPE, pHandlingUnitId, pHandlingUnitType)
                oLoadsArray.Add(ld(0))
                If totalUnitsAvailable = 0 Then
                    Exit For
                End If
            End If
        Next
        Dim oload As New Load
        Return oLoadsArray.ToArray(oload.GetType)
    End Function

    Private Function ValidateAttributeByType(ByVal oAtt As SkuClassLoadAttribute, ByVal oAttVal As Object) As Int32
        Select Case oAtt.Type
            Case Logic.AttributeType.DateTime
                Dim Val As DateTime
                Try
                    If String.IsNullOrEmpty(oAttVal) Then
                        If oAtt.CaptureAtReceiving = SkuClassLoadAttribute.CaptureType.Capture Then
                            Return 0
                        Else
                            Return -1
                        End If
                    End If
                    Val = CType(oAttVal, DateTime)
                    Return 1
                Catch ex As Exception
                    Return -1
                End Try
            Case Logic.AttributeType.Decimal
                Dim Val As Decimal
                Try
                    If String.IsNullOrEmpty(oAttVal) Then
                        If oAtt.CaptureAtReceiving = SkuClassLoadAttribute.CaptureType.Capture Then
                            Return 0
                        Else
                            Return -1
                        End If
                    End If
                    Val = CType(oAttVal, Decimal)
                    Return 1
                Catch ex As Exception
                    Return -1
                End Try
            Case Logic.AttributeType.Integer
                Dim Val As Int32
                Try
                    If String.IsNullOrEmpty(oAttVal) Then
                        If oAtt.CaptureAtReceiving = SkuClassLoadAttribute.CaptureType.Capture Then
                            Return 0
                        Else
                            Return -1
                        End If
                    End If
                    Val = CType(oAttVal, Int32)
                    Return 1
                Catch ex As Exception
                    Return -1
                End Try
            Case Logic.AttributeType.String
                Try
                    If String.IsNullOrEmpty(oAttVal) Then
                        If oAtt.CaptureAtReceiving = SkuClassLoadAttribute.CaptureType.Capture Then
                            Return 0
                        Else
                            Return -1
                        End If
                    End If
                    Return 1
                Catch ex As Exception
                    Return -1
                End Try
        End Select
    End Function

    Private Sub ThrowExceptionOnNewSku(ByVal osku As SKU)
        If osku.NEWSKU Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Cannot recieve line, New sku", "Cannot recieve line, New sku")
            Throw m4nEx
        End If
        If Not osku.STATUS Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Inactive SKU", "Inactive SKU")
            Throw m4nEx
        End If
    End Sub

    Private Sub SetLoadDestinationLocation(ByVal rl As ReceiptDetail, ByVal ld As WMS.Logic.Load, ByVal pUser As String)
        Try
            Dim loc As String
            Dim warehousearea As String
            Dim dt As New DataTable
            Dim ft As New Flowthrough(rl.CONSIGNEE, rl.ORDERID)
            Dim SQL As String = String.Format("select location,warehousearea from XDOCKDELIVERYLOC where documenttype = 'FLOWTHROUGH' and consignee like '{0}%' " & _
                "and ordertype like '{1}%' and sourcecompany like '{2}%' and sourcecompanytype like '{3}%'  and targetcompany like '{4}%' " & _
                "and targetcompanytype like '{5}%' order by priority", ft.CONSIGNEE, ft.ORDERTYPE, ft.SOURCECOMPANY, ft.SOURCECOMPANYTYPE, _
                ft.TARGETCOMPANY, ft.TARGETCOMPANYTYPE)
            DataInterface.FillDataset(SQL, dt)
            If dt.Rows.Count = 0 Then
                Return
            End If
            Dim dr As DataRow = dt.Rows(0)
            loc = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("location"))
            warehousearea = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("warehousearea"))

            If loc <> "" And warehousearea <> "" Then
                ld.SetDestinationLocation(loc, warehousearea, pUser)
            ElseIf ft.STAGINGLANE <> "" And ft.STAGINGWAREHOUSEAREA <> "" Then
                ld.SetDestinationLocation(ft.STAGINGLANE, ft.STAGINGWAREHOUSEAREA, pUser)
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Function CreateContainer(ByVal pContainerId As String, ByVal pContainerType As String, ByVal pLocation As String, ByVal pWarehousearea As String) As WMS.Logic.Container
        Dim CreateNewCont As Boolean = False
        ' After Load Creation we will put the loads on his container
        If pContainerId <> "" Then
            Dim CheckCntSql As String = String.Format("SELECT * FROM CONTAINER WHERE CONTAINER='{0}'", pContainerId)
            Dim dt As New DataTable
            Made4Net.DataAccess.DataInterface.FillDataset(CheckCntSql, dt)
            'Checking if container already exists, if not creating new one
            If dt.Rows.Count = 0 Then
                CreateNewCont = True
            End If
        Else
            CreateNewCont = True
        End If

        Dim oCont As WMS.Logic.Container
        If CreateNewCont Then
            oCont = New WMS.Logic.Container
            oCont.ContainerId = pContainerId
            oCont.Location = pLocation
            oCont.Warehousearea = pWarehousearea
            oCont.HandlingUnitType = pContainerType
            oCont.Post(WMS.Logic.Common.GetCurrentUser)
        Else
            oCont = New WMS.Logic.Container(pContainerId, True)
        End If
        Return oCont
    End Function

End Class
