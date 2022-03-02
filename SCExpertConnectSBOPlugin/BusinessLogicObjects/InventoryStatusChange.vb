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

Public Class InventoryStatusChange
    Inherits SBOPlugin

#Region "Members"

    Private mSBOStockTransfer As SAPbobsCOM.StockTransfer

#End Region

#Region "Constructor"

    Public Sub New(ByVal pPluginId As Int32)
        MyBase.New(pPluginId)
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

    Public Overrides Sub ProcessTransaction(ByVal SBOTransactionEventArgs As B1DIEventsService.B1DIEventsArgs)

    End Sub

#End Region

#Region "Export"

    Public Overrides Function Export(ByVal oXMLDoc As System.Xml.XmlDocument) As Int32
        Try
            InitLogger(String.Format("ExportInvAdjustments_{0}_{1}", DateTime.Now.ToString("ddMMyyyy_hhmmss"), New Random().Next(1000)))
            If Not Logger Is Nothing Then
                Logger.WriteLine("Trying to export Inventory adjustments transaction...")
            End If

            Dim ObjectType As SAPbobsCOM.BoObjectTypes
            ' Load the target document
            mSBOStockTransfer = LoadObjectFromFile(oXMLDoc, ObjectType)
            If mSBOStockTransfer Is Nothing Then
                If Not Logger Is Nothing Then
                    Logger.WriteLine("Failed to load Stock Transfer from file, terminating process...")
                End If
                Return -1
            Else
                If Not Logger Is Nothing Then
                    Logger.WriteLine("Target document object loaded from connect received XML...")
                End If
            End If

            '' Set the series property if exists...
            'If Not GetParameterValue("Series") Is Nothing AndAlso GetParameterValue("Series") <> String.Empty Then
            '    mSBOStockTransfer.Series = GetParameterValue("Series")
            'End If

            '******************************************
            'yet to be implemented - serials & batches
            '******************************************

            ' Add the target document to the SBO via the DI API
            Dim NewDocEntry As String = ""
            Dim ret As Int32
            Try
                If Not Logger Is Nothing Then
                    Logger.WriteLine("Trying to add the target document to SBO...")
                End If

                Try
                    mCompanyObj.StartTransaction()
                    ret = mSBOStockTransfer.Add()
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

            'Finally return 0 for success...
            Return 0
        Catch ex As Exception
            If Not Logger Is Nothing Then
                Logger.WriteLine("Error occured while exporting stock transfers... Error detail:")
                Logger.WriteLine(ex.ToString)
            End If
            Return -1
        End Try
    End Function

#End Region

#End Region

End Class
