<CLSCompliant(False)> Public Class PickByItemStrategy
    Inherits PickBaskets

    Protected _consignee As String
    Protected _sku As String
    Protected _attributes As AttributesCollection

#Region "Constructor"
    Public Sub New()
        MyBase.New(WMS.Lib.PickMethods.PickMethod.PICKBYITEM)
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
            If req.Sku = _sku And req.Consignee = _consignee And AttributesCollection.Equal(_attributes, req.Attributes) Then
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
            _sku = req.Sku
            _attributes = req.Attributes
        End If
        MyBase.Place(req)
    End Sub

#End Region

End Class
