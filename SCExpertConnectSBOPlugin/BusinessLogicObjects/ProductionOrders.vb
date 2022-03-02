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

Public Class ProductionOrders
    Inherits SBOPlugin

#Region "Members"

    Private mObjType As Int32
    Private mDocumentStatus As String
    Private mDocumentDaysRange As Int32
    Private mSBODocument As SAPbobsCOM.ProductionOrders

#End Region

#Region "Constructor"

    Public Sub New(ByVal pPluginId As Int32)
        MyBase.New(pPluginId)
        mDocumentStatus = GetParameterValue("DocumentStatus")
        mDocumentDaysRange = Convert.ToInt32(GetParameterValue("DocumentDaysRange"))
        mObjectType = SAPbobsCOM.BoObjectTypes.oProductionOrders.ToString()
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
        If DateTime.Now.AddDays(mDocumentDaysRange) < mSBODocument.DueDate Then
            If Not Logger Is Nothing Then
                Logger.WriteLine("Plugin days range is less than doc due date, message will be skipped...")
            End If
            Return False
        End If
        If mDocumentStatus <> "*" Then
            Select Case mDocumentStatus.ToUpper
                Case "Released".ToUpper
                    If mSBODocument.ProductionOrderStatus = SAPbobsCOM.BoProductionOrderStatusEnum.boposReleased Then
                        Return True
                    Else
                        If Not Logger Is Nothing Then
                            Logger.WriteLine("Document status does not match plugin parameter, message will be skipped...")
                        End If
                        Return False
                    End If
                Case "Planned".ToUpper
                    If mSBODocument.ProductionOrderStatus = SAPbobsCOM.BoProductionOrderStatusEnum.boposPlanned Then
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
            mSBODocument = mCompanyObj.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oProductionOrders)
            Dim bExists As Boolean = mSBODocument.GetByKey(SBOTransactionEventArgs.columValuesList)
            If Not bExists Then
                If Not Logger Is Nothing Then
                    Logger.WriteLine("Failed to load Production Order object (document does not exists), terminating request proccessing...")
                End If
                Return
            End If
            If Not ShouldExport() Then
                Return
            End If
            If Not Logger Is Nothing Then
                Logger.WriteLine("Production Order object loaded...")
                Logger.WriteLine("Trying to build XML for connect service...")
            End If

            Dim retDoc As New XmlDocument()
            retDoc.LoadXml(mSBODocument.GetAsXML())

            '' If transaction type = outbound --> replace sku with BOM.
            'If BOType = "OUTBOUND" Then
            '    ReplaceSkuBOM(retDoc)
            'End If

            If Not Logger Is Nothing Then
                Logger.WriteLine("Sending results to Connect framework...")
            End If

            Me.TransactionSet = mSBODocument.DocumentNumber
            Me.NotifySCExpertConnect(retDoc)
        Catch ex As Exception
            If Not Logger Is Nothing Then
                Logger.WriteLine("Eror Occured: " & ex.ToString)
            End If
        End Try
    End Sub

    Private Sub ReplaceSkuBOM(ByRef WorkOrderDoc As XmlDocument)
        Dim oNodeList As New List(Of XmlNode)
        Dim iOrderLine As Int32 = 0
        Dim sapProdTree As SAPbobsCOM.ProductTrees = mCompanyObj.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oProductTrees)
        For Each oTempNode As XmlNode In WorkOrderDoc.SelectNodes("BOM/BO/WOR1/row")
            Dim CurrNodePlannedQty As Decimal = oTempNode.SelectSingleNode("PlannedQty").InnerText
            Dim Exist As Boolean = sapProdTree.GetByKey(oTempNode.SelectSingleNode("ItemCode").InnerText)
            If Exist Then
                Dim oLines As SAPbobsCOM.ProductTrees_Lines = sapProdTree.Items
                If Not Logger Is Nothing Then
                    Logger.WriteLine(String.Format("BOM loaded for item {0}, Trying to produce lines to replace the father item...", _
                        sapProdTree.GetByKey(oTempNode.SelectSingleNode("ItemCode").InnerText)))
                End If
                For i As Int32 = 0 To sapProdTree.Items.Count - 1
                    'check if we need to export this partial item 
                    oLines.SetCurrentLine(i)
                    Dim oBomNode As XmlNode = oTempNode.Clone()
                    oBomNode.SelectSingleNode("LineNum").InnerText = iOrderLine
                    oBomNode.SelectSingleNode("ItemCode").InnerText = oLines.ItemCode
                    oBomNode.SelectSingleNode("PlannedQty").InnerText = oLines.Quantity * CurrNodePlannedQty
                    If Not Logger Is Nothing Then
                        Logger.WriteLine(String.Format("Item {0} with quantity {1} added to production order lines...", oLines.ItemCode, oLines.Quantity * CurrNodePlannedQty))
                    End If
                    oNodeList.Add(oBomNode)
                    iOrderLine += 1
                Next                
            Else
                If Not Logger Is Nothing Then
                    Logger.WriteLine(String.Format("BOM not defined for item {0}, will not produce lines to replace the father item...", _
                        sapProdTree.GetByKey(oTempNode.SelectSingleNode("ItemCode").InnerText)))
                End If
                oNodeList.Add(oTempNode)
            End If
        Next
        WorkOrderDoc.SelectSingleNode("BOM/BO/WOR1").RemoveAll()
        For Each oNode As XmlNode In oNodeList
            WorkOrderDoc.SelectSingleNode("BOM/BO/WOR1").AppendChild(oNode)
        Next
        oNodeList = Nothing
    End Sub

#End Region

#End Region

#End Region

End Class
