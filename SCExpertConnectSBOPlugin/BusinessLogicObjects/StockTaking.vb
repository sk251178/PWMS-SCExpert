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

Public Class StockTaking
    Inherits SBOPlugin

#Region "Members"

    Private mSBOStockTaking As SAPbobsCOM.StockTaking

#End Region

#Region "Constructor"

    Public Sub New(ByVal pPluginId As Int32)
        MyBase.New(pPluginId)
        mObjectType = SAPbobsCOM.BoObjectTypes.oStockTakings.ToString()
    End Sub

#End Region

#Region "Methods"

#Region "Export"

    Public Overrides Function Export(ByVal oXMLDoc As System.Xml.XmlDocument) As Int32
        Try
            InitLogger(String.Format("ExportInventorySnapShot_{0}_{1}", DateTime.Now.ToString("ddMMyyyy_hhmmss"), New Random().Next(1000)))
            If Not Logger Is Nothing Then
                Logger.WriteLine("Trying to export snap shot transaction into stock taking document...")
            End If
            
            ' Add the document to the SBO via the DI API
            Dim NewDocEntry As String = ""
            Dim ret As Int32
            Try
                For Each oNode As XmlNode In oXMLDoc.SelectNodes("BOM/BO/OITW/row")
                    Try
                        mSBOStockTaking = mCompanyObj.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oStockTakings)
                        mSBOStockTaking.ItemCode = oNode.SelectSingleNode("ItemCode").InnerText
                        mSBOStockTaking.Counted = oNode.SelectSingleNode("Counted").InnerText
                        mSBOStockTaking.WarehouseCode = oNode.SelectSingleNode("WhsCode").InnerText
                        If Not Logger Is Nothing Then
                            Logger.WriteLine(String.Format("Counting Item {0}, {1} units, warehouse code {2}...", mSBOStockTaking.ItemCode, mSBOStockTaking.Counted, mSBOStockTaking.WarehouseCode))
                        End If

                        Try
                            mCompanyObj.StartTransaction()
                            ret = mSBOStockTaking.Add()
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
                                Logger.WriteSeperator()
                                Logger.WriteLine("Error Occurred (Target Document was not created!) - The returned value = " & ret)
                                Logger.WriteLine(String.Format("Error Id and Description: {0}, {1}", mCompanyObj.GetLastErrorCode, mCompanyObj.GetLastErrorDescription))
                                Logger.WriteSeperator()
                            Else
                                'Logger.WriteLine(String.Format("New Object Key is (of the stock taking): {0}", NewDocEntry))
                            End If
                        End If
                    Catch ex As Exception
                        If Not Logger Is Nothing Then
                            Logger.WriteLine(ex.ToString)
                        End If
                    End Try
                Next
            Catch ex As Exception
                If Not Logger Is Nothing Then
                    Logger.WriteLine(ex.ToString)
                End If
                Return -1
            End Try
            Return 0
        Catch ex As Exception
            If Not Logger Is Nothing Then
                Logger.WriteLine("Error occured while exporting document... Error detail:")
                Logger.WriteLine(ex.ToString)
            End If
            Return -1
        End Try
    End Function

#End Region

#End Region

#Region "Imports"

    Public Overrides Sub ProcessTransaction(ByVal SBOTransactionEventArgs As B1DIEventsService.B1DIEventsArgs)
        Throw New Made4Net.Shared.M4NException(New Exception, "Method is not supported for Stock Taking Object", "Method is not supported for Stock Taking Object")
    End Sub

    Public Overrides Function ShouldExport() As Boolean
        Return False
    End Function

#End Region
    
End Class
