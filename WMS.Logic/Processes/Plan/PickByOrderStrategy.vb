<CLSCompliant(False)> Public Class PickByOrderStrategy
    Inherits PickBaskets

    Protected _orderid As String
    Protected _consignee As String

#Region "Constructor"
    Public Sub New()
        MyBase.New(WMS.Lib.PickMethods.PickMethod.PICKBYORDER)
    End Sub
#End Region

#Region "Methods"
    'Commented for Retrofit Item PWMS-748 (RWMS-439) Start
    'Public Overrides Function CanPlace(ByVal req As Requirment) As Boolean
    'Commented for Retrofit Item PWMS-748 (RWMS-439) End

    'Made changes for Retrofit Item PWMS-748 (RWMS-439) Start
    Public Overrides Function CanPlace(ByVal req As Requirment, Optional ByVal oLogger As LogHandler = Nothing) As Boolean
        'Made changes for Retrofit Item PWMS-748 (RWMS-439) End
        If Count = 0 Then
            Return True
        Else
            If _consignee = req.Consignee And _orderid = req.OrderId Then
                Return True
            Else
                Return False
            End If
        End If
    End Function
    'Commented for Retrofit Item PWMS-748 (RWMS-439) Start
    'Public Overrides Sub Place(ByVal req As Requirment)
    'Commented for Retrofit Item PWMS-748 (RWMS-439) End

    'Made changes for Retrofit Item PWMS-748 (RWMS-439) Start
    Public Overrides Sub Place(ByVal req As Requirment, Optional ByVal oLogger As LogHandler = Nothing)
        'Made changes for Retrofit Item PWMS-748 (RWMS-439) End
        If Count = 0 Then
            _consignee = req.Consignee
            _orderid = req.OrderId
        End If
        MyBase.Place(req)
    End Sub

#End Region

End Class
