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

Public Class StockTransfer
    Inherits SBOPlugin

#Region "Members"

    Private mObjType As Int32
    Private mDocumentDaysRange As Int32
    Private mSBODocument As SAPbobsCOM.StockTransfer

#End Region

#Region "Constructor"

    Public Sub New(ByVal pPluginId As Int32)
        MyBase.New(pPluginId)
        mDocumentDaysRange = Convert.ToInt32(GetParameterValue("DocumentDaysRange"))
        mObjectType = SAPbobsCOM.BoObjectTypes.oStockTransfer.ToString()
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
        If DateTime.Now.AddDays(mDocumentDaysRange) < mSBODocument.DocDate Then
            If Not Logger Is Nothing Then
                Logger.WriteLine("Plugin days range is less than doc due date, message will be skipped...")
            End If
            Return False
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
            mSBODocument = mCompanyObj.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oStockTransfer)
            Dim bExists As Boolean = mSBODocument.GetByKey(SBOTransactionEventArgs.columValuesList)
            If Not bExists Then
                If Not Logger Is Nothing Then
                    Logger.WriteLine("Failed to load Stock Transfer object (document does not exists), terminating request proccessing...")
                End If
                Return
            End If
            'If Not ShouldExport() Then
            '    Return
            'End If
            If Not Logger Is Nothing Then
                Logger.WriteLine("Stock Transfer object loaded...")
                Logger.WriteLine("Trying to build XML for connect service...")
            End If

            Dim retDoc As New XmlDocument()
            retDoc.LoadXml(mSBODocument.GetAsXML())

            If Not Logger Is Nothing Then
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

#End Region

End Class
