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

''' <summary>
'''  A Singleton class to provides SBO company objects to the transactions plugins clients
''' </summary>
''' <remarks></remarks>
Public Class SBOCompanyHelper

#Region "Members"

    Private Shared mSBOCompanyHelper As SBOCompanyHelper
    Private Shared mCompanies As Hashtable
    Private Shared mObjLock As New Object

#End Region

#Region "Constructors"

    Protected Sub New()
        mCompanies = New Hashtable
    End Sub

    ''' <summary>
    ''' Instanciate the helper object
    ''' </summary>
    ''' <returns>The Only(!!!) instance of the SBOCompanyHelper Object</returns>
    Public Shared Function GetSBOCompanyHelper() As SBOCompanyHelper
        SyncLock mObjLock
            If mSBOCompanyHelper Is Nothing Then
                mSBOCompanyHelper = New SBOCompanyHelper
            End If
        End SyncLock
        Return mSBOCompanyHelper
    End Function

#End Region

#Region "Methods"

#Region "Accessories"

    Private Function GetCompanyKey(ByVal pCompany As String, ByVal pServer As String) As String
        SyncLock mObjLock
            Return String.Format("{0}~{1}", pCompany, pServer)
        End SyncLock
    End Function

#End Region

#Region "DI API Connectivity"

    Public Function GetSBOCompany(ByVal pSBOPlugin As SBOPlugin) As SAPbobsCOM.Company
        SyncLock mObjLock
            Dim oComp As SAPbobsCOM.Company
            Dim sCompanyKey As String = GetCompanyKey(pSBOPlugin.Company, pSBOPlugin.Server)
            If mCompanies.ContainsKey(sCompanyKey) Then
                If Not pSBOPlugin.Logger Is Nothing Then
                    pSBOPlugin.Logger.WriteLine(String.Format("Company {0} found in SBOHelper cache, will return to Plugin..", pSBOPlugin.Company))
                End If
                oComp = CType(mCompanies(sCompanyKey), SBOEventServer).CompanyObject
            Else
                If Not pSBOPlugin.Logger Is Nothing Then
                    pSBOPlugin.Logger.WriteLine(String.Format("Company {0} not found in SBOHelper cache, Trying to create a new instance to add to cache...", pSBOPlugin.Company))
                End If
                'Initialize a new company object and add it to cache
                oComp = New Company
                oComp.UseTrusted = pSBOPlugin.UseTrustedConnection
                oComp.Server = pSBOPlugin.Server
                oComp.CompanyDB = pSBOPlugin.Company
                oComp.UserName = pSBOPlugin.UserName
                oComp.Password = pSBOPlugin.Password
                oComp.DbUserName = pSBOPlugin.DBUserName
                oComp.DbPassword = pSBOPlugin.DBPassword
                oComp.DbServerType = pSBOPlugin.DBType
                oComp.language = pSBOPlugin.Language
                If Not oComp.Connected Then
                    Connect(oComp, pSBOPlugin.Logger)
                End If
                Dim evtService As New B1DIEventsService.B1DIEventsService(oComp)
                Dim oSBOCompany As New SBOEventServer(oComp, evtService)
                mCompanies.Add(sCompanyKey, oSBOCompany)
            End If
            Return oComp
        End SyncLock
    End Function

    Public Function GetSBOCompanyEventServer(ByVal pSBOPlugin As SBOPlugin, ByVal Logger As Made4Net.Shared.Logging.LogFile) As SBOEventServer 'B1DIEventsService.B1DIEventsService
        SyncLock mObjLock
            Dim sCompanyKey As String = GetCompanyKey(pSBOPlugin.Company, pSBOPlugin.Server)
            If mCompanies.ContainsKey(sCompanyKey) Then
                If Not Logger Is Nothing Then
                    Logger.WriteLine(String.Format("Company {0} found in cache, will try to initiate an event service listener object.", sCompanyKey))
                End If
                Return CType(mCompanies(sCompanyKey), SBOEventServer)
            Else
                Throw New ApplicationException("Cannot get event service for company, BobsCompany not initialized...")
            End If
            Return Nothing
        End SyncLock
    End Function

    Private Function Connect(ByVal oCompany As SAPbobsCOM.Company, ByVal oLogger As Made4Net.Shared.Logging.LogFile) As Boolean
        SyncLock mObjLock
            If Not oCompany Is Nothing Then
                If Not oLogger Is Nothing Then
                    oLogger.WriteLine("Trying to connect to company database...")
                End If
                Dim iConResult As Int32 = oCompany.Connect()
                If Not oLogger Is Nothing Then
                    oLogger.WriteLine(String.Format("Company: {0} ", oCompany.CompanyDB))
                    oLogger.WriteLine(String.Format("Company Server: {0} ", oCompany.Server))
                    oLogger.WriteLine(String.Format("Company User Name: {0} ", oCompany.UserName))
                    oLogger.WriteLine(String.Format("Company Password: {0} ", oCompany.Password))
                    oLogger.WriteLine(String.Format("Company Connection state: {0} ", oCompany.Connected))
                End If
                If iConResult <> 0 Then
                    If Not oLogger Is Nothing Then
                        oLogger.WriteLine(String.Format("Could not connect to DataBase : {0}", oCompany.CompanyDB))
                        oLogger.WriteLine(String.Format("Error Code & Description : {0} ({1})", oCompany.GetLastErrorDescription(), iConResult))
                    End If
                    Dim retex As New Made4Net.Shared.M4NQHandleException("Could not connect to DataBase", False)
                    Throw retex
                Else
                End If
            End If
            Return oCompany.Connected
        End SyncLock
    End Function

    Public Sub Disconnect(ByVal oCompany As SAPbobsCOM.Company)
        SyncLock mObjLock
            If oCompany.Connected Then
                oCompany.Disconnect()
            End If
        End SyncLock
    End Sub

#End Region

#End Region

#Region "SBO Event Server"

    Public Class SBOEventServer

#Region "Members"

        Protected mCompanyObj As Company
        Protected mCompanyEventService As B1DIEventsService.B1DIEventsService
        Protected mRegisteredEventsObjectTypes As List(Of String)

#End Region

#Region "Properties"

        Public Property CompanyObject() As Company
            Get
                Return mCompanyObj
            End Get
            Set(ByVal value As Company)
                mCompanyObj = value
            End Set
        End Property

        Public Property CompanyEventService() As B1DIEventsService.B1DIEventsService
            Get
                Return mCompanyEventService
            End Get
            Set(ByVal value As B1DIEventsService.B1DIEventsService)
                mCompanyEventService = value
            End Set
        End Property

#End Region

#Region "Constructors"

        Public Sub New()
        End Sub

        Public Sub New(ByVal pCompanyObj As Company, ByVal pCompanyEventService As B1DIEventsService.B1DIEventsService)
            mCompanyObj = pCompanyObj
            mCompanyEventService = pCompanyEventService
            mRegisteredEventsObjectTypes = New List(Of String)
        End Sub

#End Region

#Region "Events Listeners Registration & Processing"

        Public Function ConnectToEventService() As Boolean
            If mCompanyObj Is Nothing Then
                Throw New ApplicationException("Company is not initialized, exiting function")
            End If
            If mCompanyEventService Is Nothing Then
                Throw New ApplicationException("Company Event Service is not initialized, exiting function")
            End If
                ' Connect to the B1DIEventServer
            mCompanyEventService.Connect(New B1DIEventsConnectionLostDelegate(AddressOf OnConnectionLostListener))
            Return True
        End Function

        Public Function DisconnectFromEventService() As Boolean
            If Not mCompanyEventService Is Nothing Then
                mCompanyEventService.Disconnect()
                Return True
            End If
            Return False
        End Function

        Public Sub OnConnectionLostListener()
            ' Recommended: Disconnect to clean all resources
            'DisconnectFromEventService()
        End Sub

        Public Sub AddSBOEventListener(ByVal pObjType As String, ByVal oLogger As Made4Net.Shared.Logging.LogFile)
            ' If this event is already registered, do nothing.... (otherwise an exception from SAP will be thrown)
            If mRegisteredEventsObjectTypes.Contains(pObjType) Then
                Return
            End If

            'Add a listener for requested object and action Add
            mCompanyEventService.AddListener(pObjType, B1DIEventsService.B1DIEventsTransactionTypes.Add.ToString(), _
                 New B1DIEventsListenerDelegate(AddressOf ProcessEvent))

            'Add a listener for requested object and action Update
            mCompanyEventService.AddListener(pObjType, B1DIEventsService.B1DIEventsTransactionTypes.Update.ToString(), _
                  New B1DIEventsListenerDelegate(AddressOf ProcessEvent))

            mRegisteredEventsObjectTypes.Add(pObjType)
        End Sub

        Public Sub RemoveSBOEventListener(ByVal pObjType As String)
            If Not mRegisteredEventsObjectTypes.Contains(pObjType) Then
                Return
            End If

            ''Add a listener for Items object and action Add
            mCompanyEventService.RemoveListener(pObjType, B1DIEventsService.B1DIEventsTransactionTypes.Add.ToString())

            ''Add a listener for Items object and action Update
            mCompanyEventService.RemoveListener(pObjType, B1DIEventsService.B1DIEventsTransactionTypes.Update.ToString())

            mRegisteredEventsObjectTypes.Remove(pObjType)
        End Sub

        Private Sub ProcessEvent(ByVal dbEventArgs As B1DIEventsService.B1DIEventsArgs)
            'Dim oLogger As New Made4Net.Shared.Logging.LogFile("C:\Program Files\Made4net\SCExpert\SCExpertConnectFiles\Logs\EventsTrapper\", New Random().Next(10000).ToString(), True)
            'oLogger.WriteLine("Event from SBO trapped... Event Data:")
            'oLogger.WriteLine("dbName: " & dbEventArgs.dbName)
            'oLogger.WriteLine("objType: " & dbEventArgs.objType)
            'oLogger.WriteLine("transactionType: " & dbEventArgs.transactionType)
            'oLogger.WriteLine("columValuesList: " & dbEventArgs.columValuesList)
            'oLogger.WriteLine("columKeysList: " & dbEventArgs.columKeysList)
            OnSBOEventFired(dbEventArgs)
        End Sub

#End Region

#Region "Delegates & Events"

        Public Delegate Sub SBOEventFiredHandler(ByVal SBOTransactionEventArgs As B1DIEventsService.B1DIEventsArgs)

        ' The event to publish
        Public Event SBOEventFired As SBOEventFiredHandler

        ' The method which fires the Event
        Public Sub OnSBOEventFired(ByVal SBOTransactionEventArgs As B1DIEventsService.B1DIEventsArgs)
            RaiseEvent SBOEventFired(SBOTransactionEventArgs)
        End Sub

#End Region

    End Class

#End Region

End Class
