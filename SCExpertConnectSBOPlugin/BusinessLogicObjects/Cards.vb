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

Public Class Cards
    Inherits SBOPlugin

#Region "Members"

    Private mSBOCard As SAPbobsCOM.BusinessPartners

#End Region

#Region "Constructor"

    Public Sub New(ByVal pPluginId As Int32)
        MyBase.New(pPluginId)
        mObjectType = SAPbobsCOM.BoObjectTypes.oBusinessPartners.ToString()
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
        Return True
    End Function

#End Region

#Region "Import"

#Region "Process Events"

    Public Overrides Sub ProcessTransaction(ByVal SBOTransactionEventArgs As B1DIEventsService.B1DIEventsArgs)
        Try
            InitLogger(String.Format("{0}_{1}_{2}", SBOTransactionEventArgs.dbName, SBOTransactionEventArgs.objType, SBOTransactionEventArgs.columValuesList))
            WriteEventData(SBOTransactionEventArgs)
            If Not ShouldExport() Then
                Return
            End If
            If Not Logger Is Nothing Then
                Logger.WriteLine("Trying to load Card object...")
            End If

            mSBOCard = mCompanyObj.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners)
            mSBOCard.GetByKey(SBOTransactionEventArgs.columValuesList)

            If Not Logger Is Nothing Then
                Logger.WriteLine("Card object loaded...")
                Logger.WriteLine("Trying to build XML for connect service...")
            End If

            Dim retDoc As New XmlDocument()
            retDoc.LoadXml(mSBOCard.GetAsXML())

            If Not Logger Is Nothing Then
                Logger.WriteLine("Sending results to Connect framework...")
            End If

            Me.TransactionSet = mSBOCard.CardCode
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
