'Imports System.Collections.Generic
'Imports System.Text
Imports SCExpertConnectPlugins.BO
Imports System.Xml
Imports System.Messaging
'Imports System.Xml.Xsl
'Imports System.IO
'Imports System.Timers
'Imports Made4Net.DataAccess
'Imports System.Data

Public Class ImporterValidator
    Public Logger As Made4Net.Shared.Logging.LogFile
    Public Sub New()

    End Sub
    Public Sub New(ByRef Logger As Made4Net.Shared.Logging.LogFile)
        Me.Logger = Logger
    End Sub

    Public Function ValidateXML(ByVal BOType As String, ByVal FacilityId As String, ByVal mMessageQueueName As String, ByRef outDoc As XmlDocument, ByVal TransactionSet As String) As Boolean
        Dim ret As Boolean = True
        Logger.WriteLine("start validate BOType " & BOType)
        Select Case BOType.ToUpper
            Case "RECEIPT"
                Dim valRec As New ValidateReceipt(Logger)
                Dim pTransactionSet As String = String.Empty
                Dim errorMessage As String = String.Empty
                ret = valRec.ValidateReceipt(outDoc, errorMessage)
                If Not ret Then
                    ret = False
                    outDoc = Nothing
                    Logger.WriteLine(errorMessage)
                    Logger.WriteLine(" validate " & BOType & " failed")
                    ProcessError(TransactionSet, BOType, FacilityId, mMessageQueueName, errorMessage)
                Else
                    Logger.WriteLine(" validate " & BOType & " success")
                End If
                Exit Select
            Case "OUTBOUND"
                Dim valOut As New ValidateOutbound(Logger)
                Dim pTransactionSet As String = String.Empty
                Dim errorMessage As String = String.Empty
                ret = valOut.ValidateOutbound(outDoc, errorMessage)
                If Not ret Then
                    ret = False
                    outDoc = Nothing
                    Logger.WriteLine(errorMessage)
                    Logger.WriteLine(" validate " & BOType & " failed")
                    ProcessError(TransactionSet, BOType, FacilityId, mMessageQueueName, errorMessage)
                Else
                    Logger.WriteLine(" validate " & BOType & " success")
                End If
                Exit Select
            Case Else
                Logger.WriteLine("validate BOType is not declare in case " & BOType)
                'do nothing
                Exit Select
        End Select
        Return ret
    End Function

    Private Sub writeToLog(ByVal pText As String)
        If Logger Is Nothing Then
            Return
        End If
        Logger.WriteLine(pText, True)
    End Sub

    Public Sub ProcessError(ByVal pTransactionSet As String, ByVal BOType As String, ByVal FacilityId As String, ByVal mMessageQueueName As String, ByVal errorMessage As String)
        Try
            Dim status As String
            Dim oXmlDoc As New XmlDocument()
            oXmlDoc.LoadXml("<?xml version=""1.0"" encoding=""utf-8"" ?><ResbHeader><ResbService><ServiceName></ServiceName><FacilityId></FacilityId></ResbService><ResbAck><Status></Status><ReasonCode></ReasonCode><CorrelId></CorrelId></ResbAck></ResbHeader>")

            status = "FAILED"
            If Not Logger Is Nothing Then
                Logger.WriteLine(String.Format("Error in transaction set {0}.", pTransactionSet), True)
            End If
            oXmlDoc.SelectSingleNode("ResbHeader/ResbService/ServiceName").InnerText = BOType
            oXmlDoc.SelectSingleNode("ResbHeader/ResbService/FacilityId").InnerText = FacilityId
            oXmlDoc.SelectSingleNode("ResbHeader/ResbAck/Status").InnerText = status
            oXmlDoc.SelectSingleNode("ResbHeader/ResbAck/ReasonCode").InnerText = errorMessage
            oXmlDoc.SelectSingleNode("ResbHeader/ResbAck/CorrelId").InnerText = pTransactionSet

            'Send the xml to the response queue....
            If Not Logger Is Nothing Then
                Logger.WriteLine(String.Format("Trying to send back the transaction response..."), True)
                Logger.WriteLine(String.Format("Message Content: {0}", oXmlDoc.InnerXml.ToString()), True)
            End If
            Dim oMessageSender As New MessageSender()
            oMessageSender.Send(oXmlDoc.InnerXml.ToString(), mMessageQueueName, Logger, BOType & "_" & pTransactionSet, "", MessagePriority.Normal, True)
            If Not Logger Is Nothing Then
                Logger.WriteLine(String.Format("Response XML sent to {0} response queue.", mMessageQueueName), True)
            End If
        Catch ex As Exception
            If Not Logger Is Nothing Then
                Logger.WriteLine("Eror Occured: " & ex.ToString)
            End If
        End Try
    End Sub

End Class


