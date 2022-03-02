Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Text
Imports System.Xml
Imports System.IO
Imports System.Data
Imports Made4Net.Shared
Imports Made4Net.DataAccess
Imports ExpertObjectMapper
Imports SCExpertConnectPlugins.BO
Imports SAPbobsCOM
Imports B1DIEventsService

Public Class Drafts
    Inherits SBOPlugin

#Region "Members"

    Private mObjType As Int32
    Private mDocumentType As String
    Private mDocumentStatus As String
    Private mDocumentDaysRange As Int32
    Private mSBODocument As SAPbobsCOM.Documents
    Private mSBOStockTransfer As SAPbobsCOM.StockTransfer
    Private mSBODraftDocument As SAPbobsCOM.Documents
    Private mDraftLineList As List(Of Int32)

#End Region

#Region "Constructor"

    Public Sub New(ByVal pPluginId As Int32)
        MyBase.New(pPluginId)
        mObjType = GetParameterValue("DocumentObjType")
        mDocumentStatus = GetParameterValue("DocumentStatus")
        mDocumentDaysRange = Convert.ToInt32(GetParameterValue("DocumentDaysRange"))
        mSBODraftDocument = mCompanyObj.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDrafts)
        mDraftLineList = New List(Of Int32)
    End Sub

#End Region

#Region "Methods"

#Region "Accessories"

    Public Overrides Sub ProcessTransaction(ByVal SBOTransactionEventArgs As B1DIEventsService.B1DIEventsArgs)

    End Sub

    Public Overrides Function ShouldExport() As Boolean

    End Function

    Private Sub SetDocumentObjectType(ByVal oXMLDoc As XmlDocument)
        mObjType = oXMLDoc.SelectSingleNode("BOM/BO/AdmInfo/Object").InnerText
        Select Case mObjType
            Case Utils.DocumentType.Invoice
                mDocumentType = SAPbobsCOM.BoObjectTypes.oInvoices.ToString()
                mSBODocument = mCompanyObj.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices)
            Case Utils.DocumentType.CreditNotes
                mDocumentType = SAPbobsCOM.BoObjectTypes.oCreditNotes.ToString()
                mSBODocument = mCompanyObj.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oCreditNotes)
            Case Utils.DocumentType.OutboundDeliveryNote
                mDocumentType = SAPbobsCOM.BoObjectTypes.oDeliveryNotes.ToString()
                mSBODocument = mCompanyObj.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDeliveryNotes)
            Case Utils.DocumentType.OutboundReturn
                mDocumentType = SAPbobsCOM.BoObjectTypes.oReturns.ToString()
                mSBODocument = mCompanyObj.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oReturns)
            Case Utils.DocumentType.OutboundOrder
                mDocumentType = SAPbobsCOM.BoObjectTypes.oOrders.ToString()
                mSBODocument = mCompanyObj.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders)
            Case Utils.DocumentType.PurchaseInvoice
                mDocumentType = SAPbobsCOM.BoObjectTypes.oPurchaseInvoices.ToString()
                mSBODocument = mCompanyObj.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseInvoices)
            Case Utils.DocumentType.PurchaseCreditNotes
                mDocumentType = SAPbobsCOM.BoObjectTypes.oPurchaseCreditNotes.ToString()
                mSBODocument = mCompanyObj.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseCreditNotes)
            Case Utils.DocumentType.PurchaseDeliveryNote
                mDocumentType = SAPbobsCOM.BoObjectTypes.oPurchaseDeliveryNotes.ToString()
                mSBODocument = mCompanyObj.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseDeliveryNotes)
            Case Utils.DocumentType.PurchaseReturn
                mDocumentType = SAPbobsCOM.BoObjectTypes.oPurchaseReturns.ToString()
                mSBODocument = mCompanyObj.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseReturns)
            Case Utils.DocumentType.PurchaseOrder
                mDocumentType = SAPbobsCOM.BoObjectTypes.oPurchaseOrders.ToString()
                mSBODocument = mCompanyObj.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseOrders)
            Case Utils.DocumentType.Quotations
                mDocumentType = SAPbobsCOM.BoObjectTypes.oQuotations.ToString()
                mSBODocument = mCompanyObj.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)
            Case Utils.DocumentType.InventoryAdjustmentGenEtry
                mDocumentType = SAPbobsCOM.BoObjectTypes.oInventoryGenEntry.ToString()
                mSBODocument = mCompanyObj.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenEntry)
            Case Utils.DocumentType.InventoryAdjustmentGenExit
                mDocumentType = SAPbobsCOM.BoObjectTypes.oInventoryGenExit.ToString()
                mSBODocument = mCompanyObj.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenExit)
            Case Utils.DocumentType.WHTransfer
                mDocumentType = SAPbobsCOM.BoObjectTypes.oStockTransfer.ToString()
                mSBOStockTransfer = mCompanyObj.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oStockTransfer)
            Case Utils.DocumentType.WorkOrder
                mDocumentType = SAPbobsCOM.BoObjectTypes.oWorkOrders.ToString()
                mSBODocument = mCompanyObj.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oWorkOrders)
            Case Utils.DocumentType.Draft
                mDocumentType = SAPbobsCOM.BoObjectTypes.oDrafts.ToString()
                mSBODocument = mCompanyObj.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDrafts)
            Case Utils.DocumentType.ProductionOrder
                mDocumentType = SAPbobsCOM.BoObjectTypes.oProductionOrders.ToString()
                mSBODocument = mCompanyObj.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oProductionOrders)
            Case Else
                mDocumentType = String.Empty
                mSBODocument = Nothing
        End Select
    End Sub

    Public Sub CopyObjectAttributesFromXml(ByRef oDoc As SAPbobsCOM.Documents, ByVal oXMLDoc As XmlDocument)
        If Not Logger Is Nothing Then
            Logger.WriteLine(String.Format("Trying to copy attributes from XML to target document..."))
        End If
        Dim sCurrAttName, sCurrAttValue As String
        sCurrAttName = ""
        sCurrAttValue = ""
        'Copy header attributes
        Dim oHeaderNode As XmlNode = oXMLDoc.SelectSingleNode("BOM/BO").ChildNodes.Item(1).SelectSingleNode("row")
        For Each oTempNode As XmlNode In oHeaderNode.ChildNodes()
            Try
                sCurrAttName = oTempNode.Name
                sCurrAttValue = oTempNode.InnerText()
                CallByName(oDoc, sCurrAttName, CallType.Let, sCurrAttValue)
                If Not Logger Is Nothing Then
                    Logger.WriteLine(String.Format("Attribute {0} set with value {1}", sCurrAttName, sCurrAttValue))
                End If
            Catch ex As Exception
                If Not Logger Is Nothing Then
                    Logger.WriteLine(String.Format("Failed to set-attribute {0}.ErrDetails:{1}", sCurrAttName, ex.ToString()))
                End If
            End Try
        Next
        Logger.WriteSeperator()
        'Copy header attributes
        For Each oTempRowNode As XmlNode In oXMLDoc.SelectSingleNode("BOM/BO").ChildNodes.Item(2).SelectNodes("row")
            Logger.WriteLine(String.Format("Copying attributes for line number {0}", oTempRowNode.SelectSingleNode("LineNum")))
            For Each oTempAttNode As XmlNode In oTempRowNode.ChildNodes()
                Try
                    sCurrAttName = oTempAttNode.Name
                    sCurrAttValue = oTempAttNode.InnerText()
                    CallByName(oDoc.Lines, sCurrAttName, CallType.Let, sCurrAttValue)
                    If Not Logger Is Nothing Then
                        Logger.WriteLine(String.Format("Attribute {0} set with value {1}", sCurrAttName, sCurrAttValue))
                    End If
                Catch ex As Exception
                    If Not Logger Is Nothing Then
                        Logger.WriteLine(String.Format("Failed to set-attribute {0}.Errdetails: {1}", sCurrAttName, ex.ToString()))
                    End If
                End Try
            Next
        Next

    End Sub

    Public Sub CopyObjectAttributes(ByRef srcObj As Object, ByRef destObj As Object, ByVal pObjType As SAPbobsCOM.BoObjectTypes)
        If Not Logger Is Nothing Then
            Logger.WriteLine(String.Format("Trying to copy attributes from base document to target document..."))
        End If
        Dim sAtt As String = GetParameterValue("CopyTargetDocAttributeList")
        If sAtt Is Nothing OrElse sAtt = String.Empty Then
            If Not Logger Is Nothing Then
                Logger.WriteLine(String.Format("No attribute list is defined for copying...."))
            End If
            Return
        End If
        Dim sAttArr() As String = sAtt.Split(";")
        For i As Int32 = 0 To sAttArr.Length - 1
            Try
                Dim tempAtt As String = sAttArr(i)
                If tempAtt.StartsWith("UDF_") Then
                    tempAtt = tempAtt.Remove(0, 4)
                    Select Case pObjType
                        Case BoObjectTypes.oStockTransfer
                            'CallByName(destObj, tempAtt, CallType.Set, CType(srcObj, SAPbobsCOM.StockTransfer).UserFields.Fields.Item(tempAtt).Value)
                            CType(destObj, SAPbobsCOM.StockTransfer).UserFields.Fields.Item(tempAtt).Value = CType(srcObj, SAPbobsCOM.Documents).UserFields.Fields.Item(tempAtt).Value
                        Case Else
                            CType(destObj, SAPbobsCOM.Documents).UserFields.Fields.Item(tempAtt).Value = CType(srcObj, SAPbobsCOM.Documents).UserFields.Fields.Item(tempAtt).Value
                    End Select
                Else
                    CallByName(destObj, tempAtt, CallType.Let, CallByName(srcObj, tempAtt, CallType.Get, Nothing))
                End If
                If Not Logger Is Nothing Then
                    Logger.WriteLine(String.Format("Attribute {0} set.", tempAtt))
                End If
            Catch ex As Exception
                If Not Logger Is Nothing Then
                    Logger.WriteLine(String.Format("Failed to set-attribute {0}.Errdetails: {1}", sAttArr(i), ex.ToString()))
                End If
            End Try
        Next
    End Sub

    Public Sub CopyObjectLinesAttributes(ByRef srcObj As Object, ByRef destObj As Object, ByVal pObjType As SAPbobsCOM.BoObjectTypes)
        If Not Logger Is Nothing Then
            Logger.WriteLine(String.Format("Trying to copy attributes from base document lines to target document..."))
        End If
        Dim sAtt As String = GetParameterValue("CopyTargetDocLinesAttributeList")
        If sAtt Is Nothing OrElse sAtt = String.Empty Then
            If Not Logger Is Nothing Then
                Logger.WriteLine(String.Format("No attribute list is defined for copying...."))
            End If
            Return
        End If
        Dim sAttArr() As String = sAtt.Split(";")
        For i As Int32 = 0 To sAttArr.Length - 1
            Try
                Dim tempAtt As String = sAttArr(i)
                If tempAtt.StartsWith("UDF_") Then
                    tempAtt = tempAtt.Remove(0, 4)
                    Select Case pObjType
                        Case BoObjectTypes.oStockTransfer
                            'CallByName(destObj, tempAtt, CallType.Set, CType(srcObj, SAPbobsCOM.StockTransfer_Lines).UserFields.Fields.Item(tempAtt).Value)
                            CType(destObj, SAPbobsCOM.StockTransfer_Lines).UserFields.Fields.Item(tempAtt).Value = CType(srcObj, SAPbobsCOM.Document_Lines).UserFields.Fields.Item(tempAtt).Value
                        Case Else
                            'CallByName(destObj, tempAtt, CallType.Set, CType(srcObj, SAPbobsCOM.Document_Lines).UserFields.Fields.Item(tempAtt).Value)
                            CType(destObj, SAPbobsCOM.Document_Lines).UserFields.Fields.Item(tempAtt).Value = CType(srcObj, SAPbobsCOM.Document_Lines).UserFields.Fields.Item(tempAtt).Value
                    End Select
                Else
                    CallByName(destObj, tempAtt, CallType.Let, CallByName(srcObj, tempAtt, CallType.Get, Nothing))
                End If
                If Not Logger Is Nothing Then
                    Logger.WriteLine(String.Format("Attribute {0} set to {1}", tempAtt, CallByName(destObj, tempAtt, CallType.Get, Nothing)))
                End If
            Catch ex As Exception
                If Not Logger Is Nothing Then
                    Logger.WriteLine(String.Format("Failed to set-attribute {0}. Errdetails:{1}", sAttArr(i), ex.ToString()))
                End If
            End Try
        Next
    End Sub

#End Region

#Region "Export"

    Public Overrides Function Export(ByVal oXMLDoc As System.Xml.XmlDocument) As Int32
        Try
            Dim ObjectType As SAPbobsCOM.BoObjectTypes
            InitLogger(String.Format("ExportDraftDocument_{0}_{1}", DateTime.Now.ToString("ddMMyyyy_hhmmss"), New Random().Next(1000)))
            If Not Logger Is Nothing Then
                Logger.WriteLine("Trying to export document transaction...")
                Logger.WriteLine("Trying to initialize document type according to admin info object...")
            End If
            Dim sDraftDocEntry As String = oXMLDoc.SelectSingleNode("BOM/BO").ChildNodes(2).FirstChild.SelectSingleNode("BaseEntry").InnerText
            Try
                'Create an instance of the target document to work with.
                SetDocumentObjectType(oXMLDoc)
                'Get the target object type from the file
                ObjectType = GetObjectTypeFromFile(oXMLDoc)
                ' Load the target object
                Select Case ObjectType
                    Case BoObjectTypes.oStockTransfer
                        ' Before loading the stock transfer - remove the base entry and line nodes from details.
                        Try
                            RemoveBaseEntryNodes(oXMLDoc)
                            If Not Logger Is Nothing Then
                                Logger.WriteLine("Base Entry nodes removed, xml after removal: ")
                                Logger.WriteLine(oXMLDoc.InnerXml.ToString)
                            End If
                        Catch ex As Exception
                            If Not Logger Is Nothing Then
                                Logger.WriteLine("Error while removing Base Entry nodes...")
                                Logger.WriteLine(ex.ToString)
                            End If
                        End Try
                        mSBOStockTransfer = LoadObjectFromFile(oXMLDoc, ObjectType)
                        If mSBOStockTransfer Is Nothing Then
                            If Not Logger Is Nothing Then
                                Logger.WriteLine("Failed to load target document, terminating process...")
                            End If
                            Return -1
                        Else
                            If Not Logger Is Nothing Then
                                Logger.WriteLine("Target stock transfer object loaded via from connect received XML...")
                            End If
                        End If
                    Case Else
                        Try
                            RemoveDocumentBaseEntryNodes(oXMLDoc)
                            If Not Logger Is Nothing Then
                                Logger.WriteLine("Base Entry nodes removed, xml after removal: ")
                                Logger.WriteLine(oXMLDoc.InnerXml.ToString)
                            End If
                        Catch ex As Exception
                            If Not Logger Is Nothing Then
                                Logger.WriteLine("Error while removing Base Entry nodes...")
                                Logger.WriteLine(ex.ToString)
                            End If
                        End Try
                        mSBODocument = LoadObjectFromFile(oXMLDoc, ObjectType)
                        If mSBODocument Is Nothing Then
                            If Not Logger Is Nothing Then
                                Logger.WriteLine("Failed to load target document, terminating process...")
                            End If
                            Return -1
                        Else
                            If Not Logger Is Nothing Then
                                Logger.WriteLine("Target document object loaded via from connect received XML...")
                            End If
                        End If
                End Select
            Catch ex As Exception
                If Not Logger Is Nothing Then
                    Logger.WriteLine(ex.ToString)
                End If
                Return -1
            End Try

            Try
                If Not Logger Is Nothing Then
                    Logger.WriteLine("Trying to set document properties. Loading draft document from DI API. (Draft key - " & sDraftDocEntry & ")")
                End If
                ' Load the base document
                Dim bExists As Boolean = mSBODraftDocument.GetByKey(sDraftDocEntry) '(mSBODocument.Lines.BaseEntry)
                If bExists Then
                    If Not Logger Is Nothing Then
                        Logger.WriteLine("Draft document object loaded via DI API...")
                    End If
                Else
                    If Not Logger Is Nothing Then
                        Logger.WriteLine("Failed to load Draft document object!!! Terminating transaction....")
                        Return -1
                    End If
                End If
                Select Case ObjectType
                    Case BoObjectTypes.oStockTransfer
                        'Copy base attributes if needed...
                        CopyObjectAttributes(mSBODraftDocument, mSBOStockTransfer, ObjectType)
                        For i As Int32 = 0 To mDraftLineList.Count - 1      'mSBOStockTransfer.Lines.Count - 1
                            If Not Logger Is Nothing Then
                                Logger.WriteLine(String.Format("Stock transfer line: {0}", i))
                                Logger.WriteLine(String.Format("Draft document line: {0}", mDraftLineList(i)))
                            End If
                            mSBOStockTransfer.Lines.SetCurrentLine(i)
                            'Get the right line index if lines were deleted
                            mSBODraftDocument.Lines.SetCurrentLine(GetLineIndex(mDraftLineList(i)))
                            CopyObjectLinesAttributes(mSBODraftDocument.Lines, mSBOStockTransfer.Lines, ObjectType)
                        Next
                    Case Else
                        'Copy base attributes if needed...
                        CopyObjectAttributes(mSBODraftDocument, mSBODocument, ObjectType)
                        For i As Int32 = 0 To mDraftLineList.Count - 1     'mSBODocument.Lines.Count - 1
                            mSBODocument.Lines.SetCurrentLine(i)
                            mSBODraftDocument.Lines.SetCurrentLine(GetLineIndex(mDraftLineList(i)))
                            CopyObjectLinesAttributes(mSBODraftDocument.Lines, mSBODocument.Lines, ObjectType)
                        Next
                End Select
            Catch ex As Exception
                If Not Logger Is Nothing Then
                    Logger.WriteLine(ex.ToString)
                End If
            End Try

            ' Add the target document to the SBO via the DI API
            Dim NewDocEntry As String = ""
            Dim ret As Int32
            Try
                Try
                    mCompanyObj.StartTransaction()
                    Select Case ObjectType
                        Case BoObjectTypes.oStockTransfer
                            ret = mSBOStockTransfer.Add()
                        Case Else
                            ret = mSBODocument.Add()
                    End Select
                Catch ex As Exception
                    ret = -1
                Finally
                    If ret <> 0 Then
                        mErrCode = mCompanyObj.GetLastErrorCode
                        mErrDesc = mCompanyObj.GetLastErrorDescription
                        mCompanyObj.EndTransaction(BoWfTransOpt.wf_RollBack)
                    Else
                        mCompanyObj.EndTransaction(BoWfTransOpt.wf_Commit)
                    End If
                End Try

                mCompanyObj.GetNewObjectCode(NewDocEntry)
                If Not Logger Is Nothing Then
                    'Logger.WriteLine(String.Format("Working on order id: {0}", mSBODocument.DocNum))
                    If ret <> 0 Then
                        Logger.WriteLine("Error Occurred (Target Document was not created!) - The returned value = " & ret)
                        Logger.WriteLine(String.Format("Error Id and Description: {0}, {1}", mErrCode, mErrDesc))
                    Else
                        Logger.WriteLine(String.Format("New Object Key is (of the Target Document): {0}", NewDocEntry))
                    End If
                End If
            Catch ex As Exception
                If Not Logger Is Nothing Then
                    Logger.WriteLine(ex.ToString)
                End If
                Return -1
            End Try

            'Try to close the draft document uppon request
            Try
                If ret = 0 AndAlso GetParameterValue("CloseDraftDocument") = "1" Then
                    If Not Logger Is Nothing Then
                        Logger.WriteLine(String.Format("Trying to close draft document..."))
                    End If
                    mSBODraftDocument.Close()
                    If Not Logger Is Nothing Then
                        Logger.WriteLine(String.Format("Draft document was closed successfully..."))
                    End If
                End If
            Catch ex As Exception
                If Not Logger Is Nothing Then
                    Logger.WriteLine(String.Format("Error occured while trying to close draft: {0}", ex.ToString))
                End If
            End Try
            Return 0
        Catch ex As Exception
            If Not Logger Is Nothing Then
                Logger.WriteLine("Error occured while exporting document... Error detail:")
                Logger.WriteLine(ex.ToString)
            End If
            Return -1
        End Try
    End Function

    Private Sub RemoveBaseEntryNodes(ByRef oXMLDoc As System.Xml.XmlDocument)
        mDraftLineList.Clear()
        For Each rowNode As XmlNode In oXMLDoc.SelectNodes("BOM/BO/WTR1/row")
            mDraftLineList.Add(System.Convert.ToInt32(rowNode.SelectSingleNode("BaseLine").InnerText))
            rowNode.RemoveChild(rowNode.SelectSingleNode("BaseEntry"))
            rowNode.RemoveChild(rowNode.SelectSingleNode("BaseLine"))
        Next
    End Sub

    Private Sub RemoveDocumentBaseEntryNodes(ByRef oXMLDoc As System.Xml.XmlDocument)
        mDraftLineList.Clear()
        For Each rowNode As XmlNode In oXMLDoc.SelectSingleNode("BOM/BO").ChildNodes(2).SelectNodes("row")
            mDraftLineList.Add(System.Convert.ToInt32(rowNode.SelectSingleNode("BaseLine").InnerText))
            rowNode.RemoveChild(rowNode.SelectSingleNode("BaseEntry"))
            rowNode.RemoveChild(rowNode.SelectSingleNode("BaseLine"))
        Next
    End Sub

    Private Function GetLineIndex(ByVal pLineNumber As Int32) As Int32
        For i As Int32 = 0 To mSBODraftDocument.Lines.Count - 1
            mSBODraftDocument.Lines.SetCurrentLine(i)
            If mSBODraftDocument.Lines.LineNum = pLineNumber Then
                Return i
            End If
        Next
        Return -1
    End Function

#End Region

#End Region

End Class
