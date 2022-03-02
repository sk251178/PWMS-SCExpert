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

Public Class PickList
    Inherits SBOPlugin

#Region "Members"

    'Private mObjType As Int32
    'Private mDocumentType As String
    Private mDocumentStatus As String
    Private mDocumentDaysRange As Int32
    Private mSBOPicklist As SAPbobsCOM.PickLists
    Private mSBOTargetDocument As SAPbobsCOM.Documents

#End Region

#Region "Constructor"

    Public Sub New(ByVal pPluginId As Int32)
        MyBase.New(pPluginId)
        'mObjType = GetParameterValue("DocumentObjType")
        mDocumentStatus = GetParameterValue("DocumentStatus")
        mDocumentDaysRange = Convert.ToInt32(GetParameterValue("DocumentDaysRange"))
        'SetDocumentObjectType()
        mObjectType = SAPbobsCOM.BoObjectTypes.oPickLists.ToString()
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
        If DateTime.Now.AddDays(mDocumentDaysRange) < mSBOPicklist.PickDate Then
            If Not Logger Is Nothing Then
                Logger.WriteLine("Plugin days range is less than doc due date, message will be skipped...")
            End If
            Return False
        End If
        Dim bShouldFilterByStatus As Boolean = False
        If mDocumentStatus <> "*" Then
            Select Case mDocumentStatus.ToUpper
                Case Utils.SBOPickListStatuses.Released.ToUpper
                    If mSBOPicklist.Status = SAPbobsCOM.BoPickStatus.ps_Released Then
                        Return True
                    Else
                        bShouldFilterByStatus = True
                    End If
                Case Utils.SBOPickListStatuses.Closed.ToUpper
                    If mSBOPicklist.Status = SAPbobsCOM.BoPickStatus.ps_Closed Then
                        Return True
                    Else
                        bShouldFilterByStatus = True
                    End If
                Case Utils.SBOPickListStatuses.PartiallyDelivered.ToUpper
                    If mSBOPicklist.Status = SAPbobsCOM.BoPickStatus.ps_PartiallyDelivered Then
                        Return True
                    Else
                        bShouldFilterByStatus = True
                    End If
                Case Utils.SBOPickListStatuses.PartiallyPicked.ToUpper
                    If mSBOPicklist.Status = SAPbobsCOM.BoPickStatus.ps_Picked Then
                        Return True
                    Else
                        bShouldFilterByStatus = True
                    End If
                Case Utils.SBOPickListStatuses.Picked.ToUpper
                    If mSBOPicklist.Status = SAPbobsCOM.BoPickStatus.ps_Picked Then
                        Return True
                    Else
                        bShouldFilterByStatus = True
                    End If
                Case Else
                    bShouldFilterByStatus = True
            End Select
            If bShouldFilterByStatus Then
                If Not Logger Is Nothing Then
                    Logger.WriteLine("Document status does not match plugin parameter, message will be skipped...")
                End If
                Return False
            End If
        End If
        Return True
    End Function

    Public Sub CopyObjectAttributes(ByRef srcObj As SAPbobsCOM.PickLists, ByRef destObj As SAPbobsCOM.Documents, ByVal pOjbType As String)
        If Not Logger Is Nothing Then
            Logger.WriteLine(String.Format("Trying to copy attributes from base document to target document..."))
        End If
        Dim sAtt As String = GetParameterValue("CopyTargetDocAttributeList")
        If sAtt Is Nothing OrElse sAtt = String.Empty Then
            If Not Logger Is Nothing Then
                Logger.WriteLine(String.Format("No attribute list is defined for copying...."))
            End If
            Return
        End If
        Dim sAttArr() As String = sAtt.Split(";")
        For i As Int32 = 0 To sAttArr.Length - 1
            Try
                Dim tempAtt As String = sAttArr(i)
                If tempAtt.StartsWith("UDF_") Then
                    tempAtt = tempAtt.Remove(0, 4)
                    CallByName(destObj, tempAtt, CallType.Set, srcObj.UserFields.Fields.Item(tempAtt).Value)
                Else
                    CallByName(destObj, tempAtt, CallType.Let, CallByName(srcObj, tempAtt, CallType.Get, Nothing))
                End If
                If Not Logger Is Nothing Then
                    Logger.WriteLine(String.Format("Attribute {0} set.", sAttArr(i)))
                End If
            Catch ex As Exception
                If Not Logger Is Nothing Then
                    Logger.WriteLine(String.Format("Failed to set-attribute {0}.Errdetails: {1}", sAttArr(i), ex.ToString()))
                End If
            End Try
        Next
    End Sub

#End Region

#Region "Import"

#Region "Process Events"

    Public Overrides Sub ProcessTransaction(ByVal SBOTransactionEventArgs As B1DIEventsService.B1DIEventsArgs)
        Try
            InitLogger(String.Format("{0}_{1}_{2}", SBOTransactionEventArgs.dbName, SBOTransactionEventArgs.objType, SBOTransactionEventArgs.columValuesList))
            WriteEventData(SBOTransactionEventArgs)
            If Not Logger Is Nothing Then
                Logger.WriteLine("Trying to load pick list object...")
            End If
            mSBOPicklist = mCompanyObj.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPickLists)
            Dim bExists As Boolean = mSBOPicklist.GetByKey(SBOTransactionEventArgs.columValuesList)
            If Not bExists Then
                If Not Logger Is Nothing Then
                    Logger.WriteLine("Failed to load document object (pick list does not exists), terminating request proccessing...")
                End If
                Return
            End If
            If Not ShouldExport() Then
                Return
            End If
            If Not Logger Is Nothing Then
                Logger.WriteLine("Pick list object loaded...")
                Logger.WriteLine("Trying to build XML for connect service...")
            End If

            Dim retDoc As New XmlDocument()
            retDoc.LoadXml(mSBOPicklist.GetAsXML())

            If Not Logger Is Nothing Then
                Logger.WriteLine("XML of the document: " & retDoc.InnerXml)
                Logger.WriteLine("Sending results to Connect framework...")
            End If

            Me.TransactionSet = mSBOPicklist.AbsoluteEntry
            Me.NotifySCExpertConnect(retDoc)
        Catch ex As Exception
            If Not Logger Is Nothing Then
                Logger.WriteLine("Eror Occured: " & ex.ToString)
            End If
        End Try
    End Sub

#End Region

#End Region

#Region "Export"

    Public Overrides Function Export(ByVal oXMLDoc As System.Xml.XmlDocument) As Int32
        Try
            InitLogger(String.Format("ExportDocument_{0}_{1}", DateTime.Now.ToString("ddMMyyyy_hhmmss"), New Random().Next(1000)))
            If Not Logger Is Nothing Then
                Logger.WriteLine("Trying to export document transaction...")
            End If
            'Dim sCloseBaseDoc As String = GetParameterValue("CloseBaseDoc")
            'Dim bAddRemainQtyLines As Boolean = GetBooleanValueFromParameter("AddRemainQtyLines")
            Dim ObjectType As SAPbobsCOM.BoObjectTypes
            ' Load the target document
            mSBOTargetDocument = LoadObjectFromFile(oXMLDoc, ObjectType)
            If mSBOTargetDocument Is Nothing Then
                If Not Logger Is Nothing Then
                    Logger.WriteLine("Failed to load target document, terminating process...")
                End If
                Return -1
            Else
                If Not Logger Is Nothing Then
                    Logger.WriteLine("Target document object loaded via from connect received XML...")
                End If
            End If

            ' Load the base document
            mSBOPicklist = mCompanyObj.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPickLists)
            Dim bExists As Boolean = mSBOPicklist.GetByKey(mSBOTargetDocument.Lines.PickListIdNumber)
            If bExists Then
                If Not Logger Is Nothing Then
                    Logger.WriteLine("Base document object loaded via DI API...")
                End If
            Else
                If Not Logger Is Nothing Then
                    Logger.WriteLine("Failed to load base pick list object!!! Terminating transaction....")
                    Return -1
                End If
            End If
            'Copy base attributes if needed...
            CopyObjectAttributes(mSBOPicklist, mSBOTargetDocument, "")

            ' Add the target document to the SBO via the DI API
            Dim NewDocEntry As String = ""
            Dim ret As Int32
            Try
                Try
                    mCompanyObj.StartTransaction()
                    ret = mSBOTargetDocument.Add()
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
                    Logger.WriteLine(String.Format("Working on pick list id: {0}", mSBOPicklist.AbsoluteEntry))
                    If ret <> 0 Then
                        Logger.WriteLine("Error Occurred (Target Document was not created!) - The returned value = " & ret)
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

End Class
