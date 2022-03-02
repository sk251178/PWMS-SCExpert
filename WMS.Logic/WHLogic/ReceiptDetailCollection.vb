Imports System.Collections
Imports Made4Net.DataAccess

<CLSCompliant(False)> Public Class ReceiptDetailCollection
    Inherits ArrayList

#Region "Variables"

    Protected _receipt As String

#End Region

#Region "Properties"

    Default Public Shadows ReadOnly Property item(ByVal index As Int32) As ReceiptDetail
        Get
            Return CType(MyBase.Item(index), ReceiptDetail)
        End Get
    End Property

    Public ReadOnly Property Line(ByVal pLine As Int32) As ReceiptDetail
        Get
            Dim i As Int32
            For i = 0 To Me.Count - 1
                If item(i).RECEIPTLINE = pLine Then
                    Return (CType(MyBase.Item(i), ReceiptDetail))
                End If
            Next
            Return Nothing
        End Get
    End Property

#End Region

#Region "Constructors"

    Public Sub New(ByVal pReceipt As String, Optional ByVal LoadAll As Boolean = True)
        _receipt = pReceipt
        Dim SQL As String
        Dim dt As New DataTable
        Dim dr As DataRow
        SQL = "Select * from RECEIPTDETAIL where RECEIPT = '" & _receipt & "'"
        DataInterface.FillDataset(SQL, dt)
        For Each dr In dt.Rows
            Me.add(New ReceiptDetail(dr))
        Next
    End Sub

#End Region

#Region "Methods"

    Public Shadows Function add(ByVal pObject As ReceiptDetail) As Integer
        Return MyBase.Add(pObject)
    End Function

    Public Shadows Sub Insert(ByVal index As Int32, ByVal Value As ReceiptDetail)
        MyBase.Insert(index, Value)
    End Sub

    Public Shadows Sub Remove(ByVal pObject As ReceiptDetail)
        MyBase.Remove(pObject)
    End Sub

    Public Function CreateNewLine(ByVal pReceiptLine As Int32, ByVal pConsignee As String, ByVal pSku As String, ByVal pQtyExpected As Decimal, ByVal pRefOrd As String, ByVal pRefOrdLine As String, ByVal pInventoryStatus As String, ByVal pCompany As String, ByVal pCompanyType As String, ByVal pUser As String, ByVal oAttributeCollection As AttributesCollection, Optional ByVal pInputQTY As Decimal = 0, Optional ByVal pInpupUOM As String = Nothing, Optional ByVal pInputSku As String = Nothing, Optional ByVal pDocumentType As String = "", Optional ByVal pOrderID As String = "", Optional ByVal pOrderLine As Integer = 0, Optional ByVal pAvgWeight As Decimal = -1, Optional ByVal pAvgWeightUOM As String = "", Optional ByVal pReceivedWeight As Decimal = 0)
        Dim oRecDetail As New ReceiptDetail
        Me.add(oRecDetail.CreateNew(_receipt, pConsignee, pSku, pQtyExpected, pRefOrd, pRefOrdLine, pInventoryStatus, pCompany, pCompanyType, pAvgWeight, pAvgWeightUOM, pReceivedWeight, pUser, oAttributeCollection, pInputQTY, pInpupUOM, pInputSku, pReceiptLine, pDocumentType, pOrderID, pOrderLine))
        Return oRecDetail.RECEIPTLINE
    End Function

    Public Function CreateNewLine(ByVal pConsignee As String, ByVal pOrderId As String, ByVal pOrderLine As Int32, ByVal pQtyExpected As Decimal, ByVal pRefOrd As String, ByVal pRefOrdLine As String, ByVal pInventoryStatus As String, ByVal pCompany As String, ByVal pCompanyType As String, ByVal pUser As String, ByVal oAttributeCollection As AttributesCollection, Optional ByVal pInputQTY As Decimal = 0, Optional ByVal pInpupUOM As String = Nothing, Optional ByVal pInputSku As String = Nothing, Optional ByVal pAvgWeight As Decimal = -1, Optional ByVal pAvgWeightUOM As String = "", Optional ByVal pReceivedWeight As Decimal = 0)
        Me.add(New ReceiptDetail().CreateNew(_receipt, pConsignee, pOrderId, pOrderLine, pQtyExpected, pRefOrd, pRefOrdLine, pInventoryStatus, pCompany, pCompanyType, pAvgWeight, pAvgWeightUOM, pUser, oAttributeCollection, pInputQTY, pInpupUOM, pInputSku))
    End Function

    Public Function CreateNewLine(ByVal pConsignee As String, ByVal pOrderId As String, ByVal pOrderLine As Int32, ByVal pRefOrd As String, ByVal pRefOrdLine As String, ByVal pInventoryStatus As String, ByVal pCompany As String, ByVal pCompanyType As String, ByVal pUser As String, ByVal fromOrder As Boolean, ByVal oAttributeCollection As AttributesCollection, Optional ByVal pInputQTY As Decimal = 0, Optional ByVal pInpupUOM As String = Nothing, Optional ByVal pInputSku As String = Nothing, Optional ByVal pDocumentType As String = "", Optional ByVal pAvgWeight As Decimal = -1, Optional ByVal pAvgWeightUOM As String = "", Optional ByVal pReceivedWeight As Decimal = 0)
        Me.add(New ReceiptDetail().CreateNew(_receipt, pConsignee, pOrderId, pOrderLine, pRefOrd, pRefOrdLine, pInventoryStatus, pCompany, pCompanyType, pAvgWeight, pAvgWeightUOM, 0, pUser, True, oAttributeCollection, pInputQTY, pInpupUOM, pInputSku, pDocumentType))
    End Function

    Public Function UpdateLine(ByVal LineNumber As Int32, ByVal pConsignee As String, ByVal pSku As String, ByVal pQtyExpected As Decimal, ByVal pRefOrd As String, ByVal pRefOrdLine As String, ByVal pInventoryStatus As String, ByVal pCompany As String, ByVal pCompanyType As String, ByVal pAvgWeight As Decimal, ByVal pAvgWeightUOM As String, ByVal pReceivedWeight As Decimal, ByVal pUser As String, ByVal oAttributeCollection As AttributesCollection, Optional ByVal pInputQTY As Decimal = 0, Optional ByVal pInpupUOM As String = Nothing, Optional ByVal pDocumentType As String = "", Optional ByVal pOrderID As String = "", Optional ByVal pOrderLine As Integer = 0)
        Dim ln As ReceiptDetail = Line(LineNumber)
        If ln Is Nothing Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Line does not exists in receipt", "Line does not exists in receipt")
            m4nEx.Params.Add("LineNumber", LineNumber)
            m4nEx.Params.Add("receipt", _receipt)
            Throw m4nEx
        End If
        ln.Update(pConsignee, pSku, pQtyExpected, pRefOrd, pRefOrdLine, pInventoryStatus, pCompany, pCompanyType, pAvgWeight, pAvgWeightUOM, pReceivedWeight, pUser, oAttributeCollection, pInputQTY, pInpupUOM, Nothing, pDocumentType, pOrderID, pOrderLine)
    End Function


#End Region

End Class

