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

Public Class WorkOrders
    Inherits SBOPlugin

#Region "Members"

    Private mObjType As Int32
    Private mDocumentStatus As String
    Private mDocumentDaysRange As Int32
    Private mSBODocument As SAPbobsCOM.WorkOrders

#End Region

#Region "Constructor"

    Public Sub New(ByVal pPluginId As Int32)
        MyBase.New(pPluginId)
        mDocumentStatus = GetParameterValue("DocumentStatus")
        mDocumentDaysRange = Convert.ToInt32(GetParameterValue("DocumentDaysRange"))
        mObjectType = SAPbobsCOM.BoObjectTypes.oWorkOrders.ToString()
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
        If DateTime.Now.AddDays(mDocumentDaysRange) < mSBODocument.WorkFinishDate Then
            If Not Logger Is Nothing Then
                Logger.WriteLine("Plugin days range is less than doc due date, message will be skipped...")
            End If
            Return False
        End If
        If mDocumentStatus <> "*" Then
            Select Case mDocumentStatus.ToUpper
                Case "New".ToUpper
                    If mSBODocument.Status = SAPbobsCOM.BoWorkOrderStat.wk_WorkOrder Then
                        Return True
                    Else
                        If Not Logger Is Nothing Then
                            Logger.WriteLine("Document status does not match plugin parameter, message will be skipped...")
                        End If
                        Return False
                    End If
                Case "Instruction".ToUpper
                    If mSBODocument.Status = SAPbobsCOM.BoWorkOrderStat.wk_WorkInstruction Then
                        Return True
                    Else
                        If Not Logger Is Nothing Then
                            Logger.WriteLine("Document status does not match plugin parameter, message will be skipped...")
                        End If
                        Return False
                    End If
                Case "Complete".ToUpper
                    If mSBODocument.Status = SAPbobsCOM.BoWorkOrderStat.wk_ProductComplete Then
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

    'Public Sub CreateWorkOrder_DBEventHandler(ByVal dbEventArgs As B1DIEventsService.B1DIEventsArgs)
    '    ProcessTransaction(dbEventArgs)
    'End Sub

    'Public Sub UpdateWorkOrder_DBEventHandler(ByVal dbEventArgs As B1DIEventsService.B1DIEventsArgs)
    '    ProcessTransaction(dbEventArgs)
    'End Sub

    Public Overrides Sub ProcessTransaction(ByVal SBOTransactionEventArgs As B1DIEventsService.B1DIEventsArgs)
        Try
            InitLogger(String.Format("{0}_{1}_{2}", SBOTransactionEventArgs.dbName, SBOTransactionEventArgs.objType, SBOTransactionEventArgs.columValuesList))
            WriteEventData(SBOTransactionEventArgs)
            If Not Logger Is Nothing Then
                Logger.WriteLine("Trying to load Work Order object...")
            End If
            Dim bExists As Boolean = mSBODocument.GetByKey(SBOTransactionEventArgs.columValuesList)
            If Not bExists Then
                If Not Logger Is Nothing Then
                    Logger.WriteLine("Failed to load Work Order object (document does not exists), terminating request proccessing...")
                End If
                Return
            End If
            If Not ShouldExport() Then
                Return
            End If
            If Not Logger Is Nothing Then
                Logger.WriteLine("Work Order object loaded...")
                Logger.WriteLine("Trying to build XML for connect service...")
            End If

            Dim retDoc As New XmlDocument()
            retDoc.LoadXml(mSBODocument.GetAsXML())

            ' If transaction type = outbound --> replace sku with BOM.
            If BOType = "OUTBOUND" Then

            End If

            If Not Logger Is Nothing Then
                Logger.WriteLine("Sending results to Connect framework...")
            End If

            Me.TransactionSet = mSBODocument.OrderNum
            Me.NotifySCExpertConnect(retDoc)
        Catch ex As Exception
            If Not Logger Is Nothing Then
                Logger.WriteLine("Eror Occured: " & ex.ToString)
            End If
        End Try
    End Sub

    Private Sub ReplaceSkuBOM(ByRef WorkOrderDoc As XmlDocument)

    End Sub

#End Region

#End Region

#Region "Event Registration"

    'Public Overrides Sub RegisterSBOEvents()
    '    Try
    '        If ConnectToEventService() Then
    '            If Not Logger Is Nothing Then
    '                Logger.WriteLine("Connected to the DI Event Server, Trying to register events...")
    '            End If

    '            'Add a listener for Items object and action Add
    '            mCompanyEventService.AddListener(SAPbobsCOM.BoObjectTypes.oWorkOrders.ToString(), _
    '                 B1DIEventsService.B1DIEventsTransactionTypes.Add.ToString(), _
    '                 New B1DIEventsListenerDelegate(AddressOf CreateWorkOrder_DBEventHandler))

    '            'Add a listener for Items object and action Update
    '            mCompanyEventService.AddListener(SAPbobsCOM.BoObjectTypes.oWorkOrders.ToString(), _
    '                  B1DIEventsService.B1DIEventsTransactionTypes.Update.ToString(), _
    '                  New B1DIEventsListenerDelegate(AddressOf UpdateWorkOrder_DBEventHandler))

    '            If Not Logger Is Nothing Then
    '                Logger.WriteLine("Events registered successfully...")
    '            End If
    '        Else
    '            If Not Logger Is Nothing Then
    '                Logger.WriteLine("Could not Register event, Connection to the DI Event Server is not set / failed...")
    '            End If
    '        End If
    '    Catch ex As Exception
    '        If Not Logger Is Nothing Then
    '            Logger.WriteLine("Error occured while registering events, error details: ")
    '            Logger.WriteLine(ex.ToString)
    '        End If
    '    End Try
    'End Sub

    'Public Overrides Sub UnRegisterSBOEvents()
    '    Try
    '        If Not Logger Is Nothing Then
    '            Logger.WriteLine("Trying to unregister plugin events...")
    '        End If

    '        'Remove listener from Items object and action Add
    '        mCompanyEventService.RemoveListener(SAPbobsCOM.BoObjectTypes.oWorkOrders.ToString(), _
    '             B1DIEventsService.B1DIEventsTransactionTypes.Add.ToString())

    '        'Remove listener from Items object and action Update
    '        mCompanyEventService.RemoveListener(SAPbobsCOM.BoObjectTypes.oWorkOrders.ToString(), _
    '              B1DIEventsService.B1DIEventsTransactionTypes.Update.ToString())

    '        DisconnectFromEventService()

    '        If Not Logger Is Nothing Then
    '            Logger.WriteLine("Events registered successfully...")
    '        End If

    '    Catch ex As Exception
    '        If Not Logger Is Nothing Then
    '            Logger.WriteLine("Error occured while registering events, error details: ")
    '            Logger.WriteLine(ex.ToString)
    '        End If
    '    End Try
    'End Sub

#End Region

#End Region

End Class
