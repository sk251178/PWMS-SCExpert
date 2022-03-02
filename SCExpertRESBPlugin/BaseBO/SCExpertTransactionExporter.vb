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

Public Class SCExpertTransactionExporter
    Inherits BasePlugin

#Region "Members"

    Protected mMessageQueueName As String

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
    End Sub

#End Region

#Region "Methods"

#Region "Overrides"

#Region "Import"

    Public Overrides Sub Import()
        Throw New ApplicationException("Import transaction is not supprted by this type of plugin. Please use MessageQueueListener instead.")
    End Sub

    Protected Sub NotifySCExpertConnect(ByVal oResultXmlDocument As XmlDocument)
        Throw New ApplicationException("Import transaction is not supprted by this type of plugin. Please use MessageQueueListener instead.")
    End Sub

#End Region

#Region "ProcessResultFromSCExpertConnect"

    Public Overrides Sub ProcessResultFromSCExpertConect(ByVal pTransactionSet As String, ByVal pObjectDataMapperResult As Dictionary(Of String, ExpertObjectMapper.DataMapperProcessResult))
        Throw New ApplicationException("Import transaction is not supprted by this type of plugin. Please use MessageQueueListener instead.")
    End Sub

#End Region

#Region "Export"

    Public Overrides Function Export(ByVal oXMLDoc As System.Xml.XmlDocument) As Int32
        Try
            Try
                InitLogger(String.Format("{0}_{1}.txt", DateTime.Now.ToString("ddMMyyyy_hhmmss"), New Random().Next().ToString()))

                Dim oOutXml As New XmlDocument()
                oOutXml.LoadXml("<BUSINESSOBJECT><CorrelId></CorrelId><DATACOLLECTION></DATACOLLECTION></BUSINESSOBJECT>")

                If oXMLDoc.SelectSingleNode("DATACOLLECTION/DATA/FacilityId") Is Nothing Then
                    'Add the facility id for the current transaction
                    Dim oXmlNode As XmlNode = oXMLDoc.CreateNode(XmlNodeType.Element, "FacilityId", "")
                    oXmlNode.InnerText = GetParameterValue("FacilityId")
                    oXMLDoc.SelectSingleNode("DATACOLLECTION/DATA").InsertBefore(oXmlNode, oXMLDoc.SelectSingleNode("DATACOLLECTION/DATA").FirstChild)
                End If

                oOutXml.SelectSingleNode("BUSINESSOBJECT/DATACOLLECTION").InnerXml = oXMLDoc.SelectSingleNode("DATACOLLECTION").InnerXml

                'Send the xml to the response queue....
                If Not Logger Is Nothing Then
                    Logger.WriteLine(String.Format("Trying to send the transaction to RESB {0} queue...", mMessageQueueName), True)
                    Logger.WriteLine(String.Format("Message Content: {0}", oOutXml.InnerXml.ToString()), True)
                End If
                Dim oMessageSender As New MessageSender()
                oMessageSender.Send(oOutXml.InnerXml.ToString(), mMessageQueueName, Logger, Me.BOType, "", MessagePriority.Normal, False)
                If Not Logger Is Nothing Then
                    Logger.WriteLine(String.Format("Transaction XML sent to target queue."), True)
                End If
                oMessageSender = Nothing
            Catch ex As Exception
                If Not Logger Is Nothing Then
                    Logger.WriteLine("Eror Occured: " & ex.ToString)
                End If
            End Try
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

#End Region

End Class
