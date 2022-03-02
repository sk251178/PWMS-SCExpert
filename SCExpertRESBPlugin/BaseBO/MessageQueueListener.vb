Imports System
Imports System.Messaging
Imports System.Threading
Imports System.Data
Imports System.Data.Odbc
Imports System.Collections.Generic
Imports System.Configuration
Imports Made4Net.Shared
Imports Made4Net.DataAccess
Imports ExpertObjectMapper
Imports SCExpertConnectPlugins.BO
Imports System.Xml
Imports system.Runtime.Serialization.Formatters.Binary

Public Class MessageQueueListener
    Inherits BasePlugin
    Implements IMessageProcessor
    Implements IDisposable

#Region "Members"

    Protected mMessageQueueName As String
    Protected mResponseMessageQueueName As String
    Protected mQHandler As QueueHandler

#End Region

#Region "Properties"

    Public ReadOnly Property MessageQueueName() As String
        Get
            Return mMessageQueueName
        End Get
    End Property

#End Region

#Region "Constructor"

    Public Sub New(ByVal pPluginId As Int32)
        MyBase.New(pPluginId)
        mMessageQueueName = GetParameterValue("MessageQueueName")
        'mResponseMessageQueueName = GetParameterValue("ResponseMessageQueueName")
        mQHandler = New QueueHandler(mMessageQueueName, Me)
    End Sub

#End Region

#Region "Methods"

#Region "Overrides"

#Region "Import"

    Public Overrides Sub Import()
        mQHandler.StartQueue()
    End Sub

    Protected Sub NotifySCExpertConnect(ByVal oResultXmlDocument As XmlDocument)
        Dim oTranslatedFiles As New List(Of XmlDocument)
        oTranslatedFiles.Add(oResultXmlDocument)
        NotifyImportProcessComplete(oTranslatedFiles)
        oTranslatedFiles.Clear()
    End Sub

#End Region

#Region "ProcessResultFromSCExpertConnect"

    Public Overrides Sub ProcessResultFromSCExpertConect(ByVal pTransactionSet As String, ByVal pObjectDataMapperResult As Dictionary(Of String, ExpertObjectMapper.DataMapperProcessResult))
        Try
            Dim status As String
            Dim errorMessage As String = String.Empty
            Dim oXmlDoc As New XmlDocument()
            oXmlDoc.LoadXml("<?xml version=""1.0"" encoding=""utf-8"" ?><ResbHeader><ResbService><ServiceName></ServiceName><FacilityId></FacilityId></ResbService><ResbAck><Status></Status><ReasonCode></ReasonCode><CorrelId></CorrelId></ResbAck></ResbHeader>")
            If Not Logger Is Nothing Then
                Logger.WriteLine(String.Format("Result for transaction set {0}: , Total results:{1}", pTransactionSet, pObjectDataMapperResult.Count), True)
            End If
            If pObjectDataMapperResult.Count > 0 Then
                For Each pair As KeyValuePair(Of String, ExpertObjectMapper.DataMapperProcessResult) In pObjectDataMapperResult
                    errorMessage = ""
                    If pair.Value.TransactionResult = ExpertObjectMapper.ObjectProcessor.DataMapperTransactionResult.Success Then
                        status = "SUCCESS"
                        If Not Logger Is Nothing Then
                            Logger.WriteLine(String.Format("Object {0} processed successfully.", pair.Key), True)
                        End If
                    Else
                        status = "FAILED"
                        errorMessage = pair.Value.TransactionErrorMessage
                        If Not Logger Is Nothing Then
                            Logger.WriteLine(String.Format("Object {0} was not imported succesfuly.", pair.Key), True)
                        End If
                    End If
                    oXmlDoc.SelectSingleNode("ResbHeader/ResbService/ServiceName").InnerText = Me.BOType
                    oXmlDoc.SelectSingleNode("ResbHeader/ResbService/FacilityId").InnerText = GetParameterValue("FacilityId")
                    oXmlDoc.SelectSingleNode("ResbHeader/ResbAck/Status").InnerText = status
                    oXmlDoc.SelectSingleNode("ResbHeader/ResbAck/ReasonCode").InnerText = errorMessage
                    oXmlDoc.SelectSingleNode("ResbHeader/ResbAck/CorrelId").InnerText = pTransactionSet
                Next
            Else
                status = "FAILED"
                If Not Logger Is Nothing Then
                    Logger.WriteLine(String.Format("Error in transaction set {0}.", pTransactionSet), True)
                End If
                oXmlDoc.SelectSingleNode("ResbHeader/ResbService/ServiceName").InnerText = Me.BOType
                oXmlDoc.SelectSingleNode("ResbHeader/ResbService/FacilityId").InnerText = GetParameterValue("FacilityId")
                oXmlDoc.SelectSingleNode("ResbHeader/ResbAck/Status").InnerText = status
                oXmlDoc.SelectSingleNode("ResbHeader/ResbAck/ReasonCode").InnerText = errorMessage
                oXmlDoc.SelectSingleNode("ResbHeader/ResbAck/CorrelId").InnerText = pTransactionSet
            End If
            'Send the xml to the response queue....
            If Not Logger Is Nothing Then
                Logger.WriteLine(String.Format("Trying to send back the transaction response..."), True)
                Logger.WriteLine(String.Format("Message Content: {0}", oXmlDoc.InnerXml.ToString()), True)
            End If
            Dim oMessageSender As New MessageSender()
            oMessageSender.Send(oXmlDoc.InnerXml.ToString(), mMessageQueueName, Logger, Me.BOType & "_" & pTransactionSet, "", MessagePriority.Normal, True)
            If Not Logger Is Nothing Then
                Logger.WriteLine(String.Format("Response XML sent to {0} response queue.", mMessageQueueName), True)
            End If
        Catch ex As Exception
            If Not Logger Is Nothing Then
                Logger.WriteLine("Eror Occured: " & ex.ToString)
            End If
        End Try
    End Sub

#End Region

#Region "Export"

    Public Overrides Function Export(ByVal oXMLDoc As System.Xml.XmlDocument) As Int32
        Try

        Catch ex As Exception
            If Not Logger Is Nothing Then
                Logger.WriteLine(ex.ToString())
            End If
            Return -1
        End Try
        Return 0
    End Function

#End Region

#End Region

#Region "IDisposable"

    Private disposedValue As Boolean = False        ' To detect redundant calls

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                mQHandler.StopQueue()
                mQHandler.Dispose()
            End If
        End If
        Me.disposedValue = True
    End Sub

    Public Overrides Sub Dispose()
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

#End Region

#Region "IMessageProcessor"

    Public Sub ProcessQueue(ByVal qMsg As System.Messaging.Message, ByVal e As System.Messaging.PeekCompletedEventArgs) Implements IMessageProcessor.ProcessQueue
        Dim MessageText As String = String.Empty
        Try
            qMsg.Formatter = New XmlMessageFormatter(New String() {"System.String,mscorlib"})
            MessageText = qMsg.Body.ToString()

            Dim oDoc As New XmlDocument()
            oDoc.LoadXml(MessageText)

            If oDoc.DocumentElement.NamespaceURI <> String.Empty Then
                oDoc.LoadXml(oDoc.OuterXml.Replace(oDoc.DocumentElement.NamespaceURI, "").Replace(" xmlns:NS1=""""", "").Replace("NS1:", ""))
                oDoc.DocumentElement.RemoveAllAttributes()
                Dim ResponseXML As String = oDoc.OuterXml
            End If
            Me.TransactionSet = oDoc.SelectSingleNode("BUSINESSOBJECT/CorrelId").InnerText
            InitLogger(String.Format("{0}_{1}_{2}", TransactionSet, DateTime.Now.ToString("ddMMyyyy_hhmmss"), New Random().Next().ToString))
            If Not Logger Is Nothing Then
                Logger.WriteLine(String.Format("Xml extracted from queue, Xml text:{0}", MessageText))
                Logger.WriteLine(String.Format("XML After Parsing: {0}", oDoc.InnerXml))
                Logger.WriteLine(String.Format("Transaction Set: {0}", Me.TransactionSet))
            End If
            Dim tempDoc As New XmlDocument()
            Dim sXml As String = ""
            If oDoc.SelectSingleNode("BUSINESSOBJECT/DATACOLLECTION/DATA") Is Nothing Then
                sXml = String.Format("<?xml version=""1.0"" encoding=""utf-8"" ?><DATACOLLECTION><DATA>{0}</DATA></DATACOLLECTION>", oDoc.SelectSingleNode("BUSINESSOBJECT/DATA").InnerXml)
            Else
                sXml = String.Format("<?xml version=""1.0"" encoding=""utf-8"" ?><DATACOLLECTION><DATA>{0}</DATA></DATACOLLECTION>", oDoc.SelectSingleNode("BUSINESSOBJECT/DATACOLLECTION/DATA").InnerXml)
            End If

            tempDoc.LoadXml(sXml)
            If Not Logger Is Nothing Then
                Logger.WriteLine(String.Format("Sending results to Connect framework, Xml content: {0}", sXml))
            End If
            Me.NotifySCExpertConnect(tempDoc)
        Catch ex As Exception
            If Not Logger Is Nothing Then
                Logger.WriteLine(String.Format("Message Text: {0}", MessageText))
                Logger.WriteLine(String.Format("Error Occurred: {0}", ex.ToString()))
            Else
                InitLogger(String.Format("{0}_{1}_{2}", TransactionSet, DateTime.Now.ToString("ddMMyyyy_hhmmss"), New Random().Next().ToString))
                If Not Logger Is Nothing Then
                    Logger.WriteLine(String.Format("Error Occurred: {0}", ex.ToString()))
                End If
            End If
        End Try
    End Sub

#End Region

#End Region

End Class