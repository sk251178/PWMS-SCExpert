Imports System.Collections
Imports Made4Net.DataAccess

<CLSCompliant(False)> Public Class InboundOrderDetailCollection
    Inherits ArrayList

#Region "Variables"

    Protected _consignee As String
    Protected _orderid As String

#End Region

#Region "Properties"

    Default Public Shadows ReadOnly Property item(ByVal index As Int32) As InboundOrderDetail
        Get
            Return CType(MyBase.Item(index), InboundOrderDetail)
        End Get
    End Property

    Public ReadOnly Property Line(ByVal pLine As Int32) As InboundOrderDetail
        Get
            Dim i As Int32
            For i = 0 To Me.Count - 1
                If item(i).ORDERLINE = pLine Then
                    Return (CType(MyBase.Item(i), InboundOrderDetail))
                End If
            Next
            Return Nothing
        End Get
    End Property

#End Region

#Region "Constructors"

    Public Sub New(ByVal pConsignee As String, ByVal pOrderId As String, Optional ByVal LoadAll As Boolean = True)
        _consignee = pConsignee
        _orderid = pOrderId
        Dim SQL As String
        Dim dt As New DataTable
        Dim dr As DataRow
        SQL = "Select * from INBOUNDORDDETAIL where CONSIGNEE = '" & _consignee & "' and ORDERID = '" & _orderid & "'"
        DataInterface.FillDataset(SQL, dt)
        For Each dr In dt.Rows
            Me.add(New InboundOrderDetail(dr))
        Next
    End Sub

#End Region

#Region "Methods"

    Public Shadows Function add(ByVal pObject As InboundOrderDetail) As Integer
        Return MyBase.Add(pObject)
    End Function

    Public Shadows Sub Insert(ByVal index As Int32, ByVal Value As InboundOrderDetail)
        MyBase.Insert(index, Value)
    End Sub

    Public Shadows Sub Remove(ByVal pObject As InboundOrderDetail)
        MyBase.Remove(pObject)
    End Sub

    Friend Sub setLine(ByVal pLineNumber As Int32, ByVal pRefOrdLine As String, ByVal pExpDate As DateTime, ByVal pSku As String, ByVal pQty As Decimal, ByVal pInventoryStatus As String, ByVal pUser As String, ByVal oAttributeCollection As AttributesCollection, Optional ByVal pInputQTY As Decimal = 0, Optional ByVal pInpupUOM As String = Nothing, Optional ByVal pInputSku As String = Nothing)
        Dim oLine As InboundOrderDetail = Line(pLineNumber)
        If IsNothing(oLine) Then
            oLine = New InboundOrderDetail
            oLine.CreateLine(_consignee, _orderid, pLineNumber, pRefOrdLine, pExpDate, pSku, pQty, pInventoryStatus, pUser, oAttributeCollection, pInputQTY, pInpupUOM, pInputSku)
            Me.add(oLine)
        Else
            oLine.UpdateLine(pRefOrdLine, pExpDate, pSku, pQty, pInventoryStatus, pUser, oAttributeCollection, pInputQTY, pInpupUOM, pInputSku)
        End If
    End Sub
#End Region

End Class
