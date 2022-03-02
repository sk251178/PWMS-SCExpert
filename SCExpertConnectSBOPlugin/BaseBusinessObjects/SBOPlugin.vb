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

Public MustInherit Class SBOPlugin
    Inherits BasePlugin
    'Implements IDisposable

#Region "Members"

    Protected mCompany As String
    Protected mServer As String
    Protected mUserName As String
    Protected mPassword As String
    Protected mDBType As BoDataServerTypes
    Protected mTrustedConnection As Boolean
    Protected mDBUserName As String
    Protected mDBPassword As String
    Protected mActive As String
    Protected mLanguage As Int32
    Protected mErrCode As Int32
    Protected mErrDesc As String
    Protected mObjectType As String

    Protected mSBOCompanyHelper As SBOCompanyHelper
    Protected mCompanyObj As Company
    Protected mCompanyEventServer As SBOCompanyHelper.SBOEventServer

#End Region

#Region "Properties"

    Public ReadOnly Property CompanyObject()
        Get
            Return mCompanyObj
        End Get
    End Property

    Public Property Company() As String
        Get
            Return mCompany
        End Get
        Set(ByVal Value As String)
            mCompany = Value
        End Set
    End Property

    Public Property Server() As String
        Get
            Return mServer
        End Get
        Set(ByVal Value As String)
            mServer = Value
        End Set
    End Property

    Public Property UserName() As String
        Get
            Return mUserName
        End Get
        Set(ByVal Value As String)
            mUserName = Value
        End Set
    End Property

    Public Property Password() As String
        Get
            Return mPassword
        End Get
        Set(ByVal Value As String)
            mPassword = Value
        End Set
    End Property

    Public Property DBType() As BoDataServerTypes
        Get
            Return mDBType
        End Get
        Set(ByVal Value As BoDataServerTypes)
            mDBType = Value
        End Set
    End Property

    Public Property UseTrustedConnection() As Boolean
        Get
            Return mTrustedConnection
        End Get
        Set(ByVal value As Boolean)
            mTrustedConnection = value
        End Set
    End Property

    Public Property DBUserName() As String
        Get
            Return mDBUserName
        End Get
        Set(ByVal Value As String)
            mDBUserName = Value
        End Set
    End Property

    Public Property DBPassword() As String
        Get
            Return mDBPassword
        End Get
        Set(ByVal Value As String)
            mDBPassword = Value
        End Set
    End Property

    Public Property Active() As Boolean
        Get
            Return mActive
        End Get
        Set(ByVal value As Boolean)
            mActive = value
        End Set
    End Property

    Public Property Language() As Int32
        Get
            Return mLanguage
        End Get
        Set(ByVal Value As Int32)
            mLanguage = Value
        End Set
    End Property

    Public ReadOnly Property IsConnected() As Boolean
        Get
            If mCompanyObj Is Nothing Then Return False
            Return mCompanyObj.Connected
        End Get
    End Property

    Public ReadOnly Property LastErrorCode() As Int32
        Get
            mErrCode = mCompanyObj.GetLastErrorCode()
            Return mErrCode
        End Get
    End Property

    Public ReadOnly Property LastErrorDescription() As String
        Get
            mErrDesc = mCompanyObj.GetLastErrorDescription()
            Return mErrDesc
        End Get
    End Property

#End Region

#Region "Constructor"

    Public Sub New(ByVal pPluginId As Int32)
        MyBase.New(pPluginId)
        InitLogger(String.Format("Init Plugin {0}, for company {1}", pPluginId, GetParameterValue("Company")))
        mCompany = GetParameterValue("Company")
        mServer = GetParameterValue("Server")
        mUserName = GetParameterValue("UserName")
        mPassword = GetParameterValue("Password")
        mDBType = GetDBType()
        mTrustedConnection = GetBooleanValueFromParameter("TrustedConnection")
        mDBUserName = GetParameterValue("DBUserName")
        mDBPassword = GetParameterValue("DBPassword")
        mLanguage = GetParameterValue("Language")
        mActive = GetBooleanValueFromParameter("Active")
        mSBOCompanyHelper = SBOCompanyHelper.GetSBOCompanyHelper()
        mCompanyObj = mSBOCompanyHelper.GetSBOCompany(Me)
        mCompanyEventServer = mSBOCompanyHelper.GetSBOCompanyEventServer(Me, Logger)
    End Sub

#End Region

#Region "Methods"

#Region "Accessories"

    Public MustOverride Function ShouldExport() As Boolean

    Protected Function GetDBType() As BoDataServerTypes
        Select Case GetParameterValue("DBType").ToUpper
            Case "SYBASE"
                Return BoDataServerTypes.dst_SYBASE
            Case "DB_2"
                Return BoDataServerTypes.dst_DB_2
            Case "MAXDB"
                Return BoDataServerTypes.dst_MAXDB
            Case "MSSQL2005"
                Return BoDataServerTypes.dst_MSSQL2005
            Case "MSSQL2008"
                Return BoDataServerTypes.dst_MSSQL2008
            Case Else
                Return BoDataServerTypes.dst_MSSQL
        End Select
    End Function

    Protected Function GetBooleanValueFromParameter(ByVal pParameterName As String) As Boolean
        Select Case GetParameterValue(pParameterName).ToUpper
            Case "1"
                Return True
            Case "TRUE"
                Return True
            Case Else
                Return False
        End Select
    End Function

    Protected Sub WriteEventData(ByVal dbEventArgs As B1DIEventsService.B1DIEventsArgs)
        Dim sb As New StringBuilder()
        sb.AppendLine("*".PadRight(30, "*"))
        sb.AppendLine("Item event trapped, event data:")
        sb.AppendLine("Object dbName: " & Convert.ToString(dbEventArgs.dbName))
        sb.AppendLine("Object type: " & Convert.ToString(dbEventArgs.objType))
        sb.AppendLine("Object transactionType: " & Convert.ToString(dbEventArgs.transactionType))
        sb.AppendLine("Object columKeysList: " & Convert.ToString(dbEventArgs.columKeysList))
        sb.AppendLine("Object columValuesList: " & Convert.ToString(dbEventArgs.columValuesList))
        sb.AppendLine("*".PadRight(50, "*"))
        If Not Logger Is Nothing Then
            Logger.WriteLine(sb.ToString())
        End If
    End Sub

#End Region

#Region "DI API Connectivity & Events Listeners Registration"

    ''' <summary>
    ''' Disconnects from the DI API
    ''' </summary>
    Public Sub Disconnect()
        If IsConnected Then
            mCompanyObj.Disconnect()
        End If
    End Sub

    Protected Function ConnectToEventService() As Boolean
        Try
            Try
                ' This is the direct call to the di event service
                'mCompanyEventService.Connect(New B1DIEventsConnectionLostDelegate(AddressOf OnConnectionLostListener))
                ' Replaced by the following call to the SBO helper event service
                mCompanyEventServer.ConnectToEventService()
                If Not Logger Is Nothing Then
                    Logger.WriteLine("Conected to the DI Event Service.")
                End If
            Catch ex As System.Runtime.Remoting.RemotingException
                If Not Logger Is Nothing Then
                    Logger.WriteLine("Event service was already connected to the DI Event Service....(System.Runtime.Remoting.RemotingException)")
                End If
            End Try
            Return True
        Catch ex As B1DIEventsException
            If Not Logger Is Nothing Then
                Logger.WriteLine(String.Format("B1DIEventsException (Create new B1DIEventsService) : {0}", ex.ToString))
            End If
        Catch ex As Exception
            If Not Logger Is Nothing Then
                Logger.WriteLine(String.Format("B1DIEventsException (Create new B1DIEventsService) : {0}", ex.ToString))
            End If
        End Try
        Return False
    End Function

    Protected Function DisconnectFromEventService() As Boolean
        Try
            If Not Logger Is Nothing Then
                Logger.WriteLine("Trying to Disconnect from the DI Event Service.")
            End If
            If Not mCompanyEventServer Is Nothing Then
                'mCompanyEventService.Disconnect()
                ' Replaced by the following call to the SBO helper event service
                mCompanyEventServer.DisconnectFromEventService()
                If Not Logger Is Nothing Then
                    Logger.WriteLine("Disconnected from the DI Event Service.")
                End If
                Return True
            End If
        Catch ex As B1DIEventsException
            If Not Logger Is Nothing Then
                Logger.WriteLine(String.Format("B1DIEventsException (Create new B1DIEventsService) : {0}", ex.ToString))
            End If
        Catch ex As Exception
            If Not Logger Is Nothing Then
                Logger.WriteLine(String.Format("B1DIEventsException (Create new B1DIEventsService) : {0}", ex.ToString))
            End If
        End Try
        If Not Logger Is Nothing Then
            Logger.WriteLine("Could not Disconnect from the DI Event Service.")
        End If
        Return False
    End Function

#End Region

#Region "Overrides"

#Region "Import"

    Public Overrides Sub Import()
        RegisterSBOEvents()
    End Sub

    Protected Sub NotifySCExpertConnect(ByVal oResultXmlDocument As XmlDocument)
        Dim oTranslatedFiles As New List(Of XmlDocument)
        oTranslatedFiles.Add(oResultXmlDocument)
        NotifyImportProcessComplete(oTranslatedFiles)
        oTranslatedFiles.Clear()
    End Sub

#End Region

#Region "ProcessResultFromSCExpertConnect"

    Public Shadows Sub ProcessResultFromSCExpertConect(ByVal pTransactionSet As String, ByVal pObjectDataMapperResult As Dictionary(Of String, ExpertObjectMapper.ObjectProcessor.DataMapperTransactionResult))
        Try
            If Not Logger Is Nothing Then
                Logger.WriteLine(String.Format("Result for transaction set {0}: , Total results:{1}", pTransactionSet, pObjectDataMapperResult.Count), True)
                For Each pair As KeyValuePair(Of String, ExpertObjectMapper.ObjectProcessor.DataMapperTransactionResult) In pObjectDataMapperResult
                    If pair.Value = ExpertObjectMapper.ObjectProcessor.DataMapperTransactionResult.Success Then
                        Logger.WriteLine(String.Format("Object {0} processed successfully.", pair.Key), True)
                    Else
                        Logger.WriteLine(String.Format("Object {0} was not imported succesfuly.", pair.Key), True)
                    End If
                Next
            End If
        Catch ex As Exception
            If Not Logger Is Nothing Then
                Logger.WriteLine("Eror Occured: " & ex.ToString)
            End If
        End Try
    End Sub

#End Region

#Region "Expoert"

    Public Overrides Function Export(ByVal oXMLDoc As System.Xml.XmlDocument) As Int32
        Try
            Throw New ApplicationException("Plugin export method is not implemented by derived class")
        Catch ex As Exception
            If Not Logger Is Nothing Then
                Logger.WriteLine(ex.ToString())
            End If
            Return -1
        End Try
        Return 0
    End Function

    Protected Function LoadObjectFromFile(ByVal oXMLDoc As System.Xml.XmlDocument, ByRef pObjectType As SAPbobsCOM.BoObjectTypes) As Object
        Dim oRetObj As Object = Nothing
        
        ' Save the XML doc represents the transaction to disk in order to build DI object
        Dim sFilePath As String = GetParameterValue("ExportTempFilePath")
        Dim sFileName As String = String.Format("{0}_{1}_{2}.xml", PluginDescription, DateTime.Now.ToString("ddMMyyyy_hhmmss"), DateTime.Now.Ticks() & New Random().Next(100000))
        Dim sFullFileName As String = System.IO.Path.Combine(sFilePath, sFileName)
        oXMLDoc.Save(sFullFileName)
        If Not Logger Is Nothing Then
            Logger.WriteLine(String.Format("Trying to load object from file..."))
            Logger.WriteLine(String.Format("Temp file path: {0}", sFullFileName))
        End If

        ' Load the object from file
        pObjectType = mCompanyObj.GetXMLobjectType(sFullFileName, 0)
        oRetObj = mCompanyObj.GetBusinessObjectFromXML(sFullFileName, 0)

        ' Delete temp file if necessary
        Dim bSaveTempFile As Boolean = GetBooleanValueFromParameter("SaveTempFile")
        If Not bSaveTempFile Then
            Try
                If Not Logger Is Nothing Then
                    Logger.WriteLine(String.Format("Trying to delete temp SBO file..."))
                End If
                Dim oFile As New FileInfo(sFullFileName)
                oFile.Delete()
            Catch ex As Exception
                If Not Logger Is Nothing Then
                    Logger.WriteLine(String.Format("Error occured when deleting temp SBO file, error details: {0}", ex.ToString()))
                End If
            End Try
        End If

        Return oRetObj
    End Function

    Protected Function GetObjectTypeFromFile(ByVal oXMLDoc As System.Xml.XmlDocument) As SAPbobsCOM.BoObjectTypes
        Dim oRetObj As Object = Nothing
        Dim pObjectType As SAPbobsCOM.BoObjectTypes
        ' Save the XML doc represents the transaction to disk in order to build DI object
        Dim sFilePath As String = GetParameterValue("ExportTempFilePath")
        Dim sFileName As String = String.Format("{0}_{1}_{2}.xml", PluginDescription, DateTime.Now.ToString("ddMMyyyy_hhmmss"), New Random().Next(1000))
        Dim sFullFileName As String = System.IO.Path.Combine(sFilePath, sFileName)
        oXMLDoc.Save(sFullFileName)
        ' Load the object from file
        pObjectType = mCompanyObj.GetXMLobjectType(sFullFileName, 0)
        Try
            Dim oFile As New FileInfo(sFullFileName)
            oFile.Delete()
        Catch ex As Exception
        End Try
        Return pObjectType
    End Function

#End Region

#Region "Event Registration"

    Public MustOverride Sub ProcessTransaction(ByVal SBOTransactionEventArgs As B1DIEventsService.B1DIEventsArgs)

    Public Sub HandleSBOEvent(ByVal SBOTransactionEventArgs As B1DIEventsService.B1DIEventsArgs)
        If SBOTransactionEventArgs.objType.Equals(mObjectType, StringComparison.OrdinalIgnoreCase) Then
            ProcessTransaction(SBOTransactionEventArgs)
        End If
    End Sub

    Public Sub RegisterSBOEvents()
        Try
            If ConnectToEventService() Then
                If Not Logger Is Nothing Then
                    Logger.WriteLine("Connected to the DI Event Server, Trying to register events...")
                End If

                mCompanyEventServer.AddSBOEventListener(mObjectType, Logger)

                AddHandler mCompanyEventServer.SBOEventFired, New SBOCompanyHelper.SBOEventServer.SBOEventFiredHandler(AddressOf HandleSBOEvent)
 
                If Not Logger Is Nothing Then
                    Logger.WriteLine("Events registered successfully...")
                End If
            Else
                If Not Logger Is Nothing Then
                    Logger.WriteLine("Could not Register event, Connection to the DI Event Server is not set / failed...")
                End If
            End If
        Catch ex As Exception
            If Not Logger Is Nothing Then
                Logger.WriteLine("Error occured while registering events, error details: ")
                Logger.WriteLine(ex.ToString)
            End If
        End Try
    End Sub

    Public Sub UnRegisterSBOEvents()
        Try
            If Not Logger Is Nothing Then
                Logger.WriteLine("Trying to UnRegister plugin events...")
            End If

            mCompanyEventServer.RemoveSBOEventListener(mObjectType)

            If Not Logger Is Nothing Then
                Logger.WriteLine("Events UnRegistered successfully...")
            End If
        Catch ex As Exception
            If Not Logger Is Nothing Then
                Logger.WriteLine("Error occured while registering events, error details: ")
                Logger.WriteLine(ex.ToString)
            End If
        End Try
    End Sub

#End Region

#End Region

#Region "IDisposable"

    Private disposedValue As Boolean = False        ' To detect redundant calls

    Protected Shadows Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
            End If
            UnRegisterSBOEvents()
            mCompanyEventServer = Nothing
            mCompanyObj = Nothing
        End If
        Me.disposedValue = True
    End Sub

    Public Shadows Sub Dispose()
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

#End Region

#End Region

End Class
