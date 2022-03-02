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

Public Class Documents
    Inherits SBOPlugin

#Region "Members"

    Private mObjType As Int32
    Private mDocumentType As String
    Private mDocumentStatus As String
    Private mDocumentDaysRange As Int32
    Private mSBODocument As SAPbobsCOM.Documents
    Private mSBOTargetDocument As SAPbobsCOM.Documents

#End Region

#Region "Constructor"

    Public Sub New(ByVal pPluginId As Int32)
        MyBase.New(pPluginId)
        mObjType = GetParameterValue("DocumentObjType")
        mDocumentStatus = GetParameterValue("DocumentStatus")
        mDocumentDaysRange = Convert.ToInt32(GetParameterValue("DocumentDaysRange"))
        SetDocumentObjectType()
        mObjectType = mDocumentType
    End Sub

#End Region

#Region "Methods"

#Region "Accessories"

    Public Overrides Function ShouldExport() As Boolean
        If Not Active Then
            If Not Logger Is Nothing Then
                Logger.WriteLine("Plugin is not active, message will be skipped...")
            End If
            Return False
        End If
        If DateTime.Now.AddDays(mDocumentDaysRange) < mSBODocument.DocDueDate Then
            If Not Logger Is Nothing Then
                Logger.WriteLine("Plugin days range is less than doc due date, message will be skipped...")
            End If
            Return False
        End If
        If mDocumentStatus <> "*" Then
            Select Case mDocumentStatus.ToUpper
                Case "Open".ToUpper
                    If mSBODocument.DocumentStatus = SAPbobsCOM.BoStatus.bost_Open Then
                        Return True
                    Else
                        If Not Logger Is Nothing Then
                            Logger.WriteLine("Document status does not match plugin parameter, message will be skipped...")
                        End If
                        Return False
                    End If
                Case "Close".ToUpper
                    If mSBODocument.DocumentStatus = SAPbobsCOM.BoStatus.bost_Close Then
                        Return True
                    Else
                        If Not Logger Is Nothing Then
                            Logger.WriteLine("Document status does not match plugin parameter, message will be skipped...")
                        End If
                        Return False
                    End If
                Case Else
                    Return False
            End Select
        End If
        Return True
    End Function

    Private Sub SetDocumentObjectType()
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
                mSBODocument = mCompanyObj.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oStockTransfer)
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

    Public Sub CopyObjectAttributes(ByRef srcObj As SAPbobsCOM.Documents, ByRef destObj As SAPbobsCOM.Documents, ByVal pOjbType As String)
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
                    CallByName(destObj, tempAtt, CallType.Set, srcObj.UserFields.Fields.Item(tempAtt).Value)
                Else
                    CallByName(destObj, tempAtt, CallType.Let, CallByName(srcObj, tempAtt, CallType.Get, Nothing))
                End If
                If Not Logger Is Nothing Then
                    Logger.WriteLine(String.Format("Attribute {0} set.", sAttArr(i)))
                End If
            Catch ex As Exception
                If Not Logger Is Nothing Then
                    Logger.WriteLine(String.Format("Failed to set-attribute {0}.Errdetails: {1}", sAttArr(i), ex.ToString()))
                End If
            End Try
        Next
    End Sub

    Private Sub SetTargetDocumentObject111111()
        Dim destType As String
        Dim TargetObjectType As Int32 = GetParameterValue("TargetDocumentObjType")
        Dim oSB As New StringBuilder
        oSB.AppendLine("Trying to set target document object...")
        Select Case TargetObjectType
            Case Utils.DocumentType.Invoice
                mSBOTargetDocument = mCompanyObj.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices)
                destType = SAPbobsCOM.BoObjectTypes.oInvoices.ToString()
            Case Utils.DocumentType.CreditNotes
                mSBOTargetDocument = mCompanyObj.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oCreditNotes)
                destType = SAPbobsCOM.BoObjectTypes.oCreditNotes.ToString()
            Case Utils.DocumentType.OutboundDeliveryNote
                mSBOTargetDocument = mCompanyObj.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDeliveryNotes)
                destType = SAPbobsCOM.BoObjectTypes.oDeliveryNotes.ToString()
            Case Utils.DocumentType.OutboundReturn
                mSBOTargetDocument = mCompanyObj.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oReturns)
                destType = SAPbobsCOM.BoObjectTypes.oReturns.ToString()
            Case Utils.DocumentType.OutboundOrder
                mSBOTargetDocument = mCompanyObj.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders)
                destType = SAPbobsCOM.BoObjectTypes.oOrders.ToString()
            Case Utils.DocumentType.PurchaseInvoice
                mSBOTargetDocument = mCompanyObj.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseInvoices)
                destType = SAPbobsCOM.BoObjectTypes.oPurchaseInvoices.ToString()
            Case Utils.DocumentType.PurchaseCreditNotes
                mSBOTargetDocument = mCompanyObj.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseCreditNotes)
                destType = SAPbobsCOM.BoObjectTypes.oPurchaseCreditNotes.ToString()
            Case Utils.DocumentType.PurchaseDeliveryNote
                mSBOTargetDocument = mCompanyObj.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseDeliveryNotes)
                destType = SAPbobsCOM.BoObjectTypes.oPurchaseDeliveryNotes.ToString()
            Case Utils.DocumentType.PurchaseReturn
                mSBOTargetDocument = mCompanyObj.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseReturns)
                destType = SAPbobsCOM.BoObjectTypes.oPurchaseReturns.ToString()
            Case Utils.DocumentType.PurchaseOrder
                mSBOTargetDocument = mCompanyObj.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseOrders)
                destType = SAPbobsCOM.BoObjectTypes.oPurchaseOrders.ToString()
            Case Utils.DocumentType.Quotations
                mSBOTargetDocument = mCompanyObj.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)
                destType = SAPbobsCOM.BoObjectTypes.oQuotations.ToString()
            Case Utils.DocumentType.Draft
                mSBOTargetDocument = mCompanyObj.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDrafts)
                destType = SAPbobsCOM.BoObjectTypes.oDrafts.ToString()
            Case Else
                mSBOTargetDocument = Nothing
                destType = String.Empty
        End Select
        oSB.AppendLine(String.Format("Target type set by plugin parameter: {0} ({1})", TargetObjectType, destType))
        oSB.AppendLine(String.Format("Target Document set."))
        If Not Logger Is Nothing Then
            Logger.WriteLine(oSB.ToString())
        End If
        oSB = Nothing
    End Sub

#End Region

#Region "Import"

#Region "Process Events"

    Public Overrides Sub ProcessTransaction(ByVal SBOTransactionEventArgs As B1DIEventsService.B1DIEventsArgs)
        Try
            InitLogger(String.Format("{0}_{1}_{2}", SBOTransactionEventArgs.dbName, SBOTransactionEventArgs.objType, SBOTransactionEventArgs.columValuesList))
            WriteEventData(SBOTransactionEventArgs)
            If Not Logger Is Nothing Then
                Logger.WriteLine("Trying to load document object...")
            End If
            Dim bExists As Boolean = mSBODocument.GetByKey(SBOTransactionEventArgs.columValuesList)
            If Not bExists Then
                If Not Logger Is Nothing Then
                    Logger.WriteLine("Failed to load document object (document does not exists), terminating request proccessing...")
                End If
                Return
            End If
            If Not ShouldExport() Then
                Return
            End If
            If Not Logger Is Nothing Then
                Logger.WriteLine("Document object loaded...")
                Logger.WriteLine("Trying to build XML for connect service...")
            End If

            Dim retDoc As New XmlDocument()
            retDoc.LoadXml(mSBODocument.GetAsXML())

            If Not Logger Is Nothing Then
                'Logger.WriteLine("XML of the document: " & retDoc.InnerXml)
                Logger.WriteLine("Sending results to Connect framework...")
            End If

            Me.TransactionSet = mSBODocument.DocNum
            Me.NotifySCExpertConnect(retDoc)
        Catch ex As Exception
            If Not Logger Is Nothing Then
                Logger.WriteLine("Eror Occured: " & ex.ToString)
            End If
        End Try
    End Sub

#End Region

#End Region

#Region "Export"

    Public Overrides Function Export(ByVal oXMLDoc As System.Xml.XmlDocument) As Int32
        Try
            InitLogger(String.Format("ExportDocument_{0}_{1}", DateTime.Now.ToString("ddMMyyyy_hhmmss"), New Random().Next(1000)))
            If Not Logger Is Nothing Then
                Logger.WriteLine("Trying to export document transaction...")
            End If
            Dim sCloseBaseDoc As String = GetParameterValue("CloseBaseDoc")
            Dim bAddRemainQtyLines As Boolean = GetBooleanValueFromParameter("AddRemainQtyLines")
            Dim ObjectType As SAPbobsCOM.BoObjectTypes
            ' Load the target document
            mSBOTargetDocument = LoadObjectFromFile(oXMLDoc, ObjectType)
            If mSBOTargetDocument Is Nothing Then
                If Not Logger Is Nothing Then
                    Logger.WriteLine("Failed to load target document, terminating process...")
                End If
                Return -1
            Else
                If Not Logger Is Nothing Then
                    Logger.WriteLine("Target document object loaded via from connect received XML...")
                End If
            End If

            '' Load the base document
            'Dim bExists As Boolean = mSBODocument.GetByKey(mSBOTargetDocument.Lines.BaseEntry)
            'If bExists Then
            '    If Not Logger Is Nothing Then
            '        Logger.WriteLine("Base document object loaded via DI API...")
            '    End If
            'Else
            '    If Not Logger Is Nothing Then
            '        Logger.WriteLine("Failed to load base document object!!! Terminating transaction....")
            '        Return -1
            '    End If
            'End If
            ''Copy base attributes if needed...
            'CopyObjectAttributes(mSBODocument, mSBOTargetDocument, "")

            ' '' Set the series property if exists...
            ''If Not GetParameterValue("Series") Is Nothing AndAlso GetParameterValue("Series") <> String.Empty Then
            ''    mSBOTargetDocument.Series = GetParameterValue("Series")
            ''End If

            ''***********************************************************
            ''yet to be implemented - serials , create remain qty lines  
            ''***********************************************************

            ' Add the target document to the SBO via the DI API
            Dim NewDocEntry As String = ""
            Dim ret As Int32
            Try

                Try
                    mCompanyObj.StartTransaction()
                    ret = mSBOTargetDocument.Add()
                Catch ex As Exception
                    ret = -1
                Finally
                    If ret <> 0 Then
                        mCompanyObj.EndTransaction(BoWfTransOpt.wf_RollBack)
                    Else
                        mCompanyObj.EndTransaction(BoWfTransOpt.wf_Commit)
                    End If
                End Try

                mCompanyObj.GetNewObjectCode(NewDocEntry)
                If Not Logger Is Nothing Then
                    Logger.WriteLine(String.Format("Working on order id: {0}", mSBODocument.DocNum))
                    If ret <> 0 Then
                        Logger.WriteLine("Error Occurred (DeliveryNote was not created!) - The returned value = " & ret)
                        Logger.WriteLine(String.Format("Error Id and Description: {0}, {1}", mCompanyObj.GetLastErrorCode, mCompanyObj.GetLastErrorDescription))
                    Else
                        Logger.WriteLine(String.Format("New Object Key is (of the DeliveryNote): {0}", NewDocEntry))
                    End If
                End If
            Catch ex As Exception
                If Not Logger Is Nothing Then
                    Logger.WriteLine(ex.ToString)
                End If
                Return -1
            End Try

            Try
                Select Case sCloseBaseDoc
                    Case "1"
                        If Not Logger Is Nothing Then
                            Logger.WriteLine("Trying to close BaseDoc Only...")
                        End If
                        If ret = 0 Then
                            mSBODocument.Close()
                        End If
                    Case "2"
                        If Not Logger Is Nothing Then
                            Logger.WriteLine("Trying to close BaseDoc Only...")
                        End If
                        mSBODocument.Close()
                    Case Else
                End Select
            Catch ex As Exception
                If Not Logger Is Nothing Then
                    Logger.WriteLine(ex.ToString)
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

#End Region

#End Region

End Class
