'<CLSCompliant(False)> Public Class PickParalelStrategy
'    Inherits PickBaskets

'    Protected _orderid As String
'    Protected _consignee As String

'#Region "Constructor"
'    Public Sub New()
'        MyBase.New(WMS.Lib.PickMethods.PickMethod.PARALELORDERPICKING)
'    End Sub
'#End Region

'#Region "Methods"

'    Public Overrides Function CanPlace(ByVal req As Requirment) As Boolean
'        If Count = 0 Then
'            Return True
'        Else
'            If _consignee = req.Consignee And _orderid = req.OrderId Then
'                Return True
'            Else
'                Return False
'            End If
'        End If
'    End Function

'    Public Overrides Sub Place(ByVal req As Requirment)
'        If Count = 0 Then
'            _consignee = req.Consignee
'            _orderid = req.OrderId
'        End If
'        MyBase.Place(req)
'    End Sub

'#End Region

'End Class

