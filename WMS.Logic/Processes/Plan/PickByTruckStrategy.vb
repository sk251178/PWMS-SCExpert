<CLSCompliant(False)> Public Class PickByTruckStrategy
    Inherits PickBaskets

#Region "Constructor"
    Public Sub New()
        MyBase.New(WMS.Lib.PickMethods.PickMethod.PICKBYTRUCK)
    End Sub
#End Region

    Protected _shipment As String
    Protected _staginglane As String

#Region "Methods"
    'Commented for Retrofit Item PWMS-748 (RWMS-439) Start
    'Public Overrides Function CanPlace(ByVal req As Requirment) As Boolean
    'Commented for Retrofit Item PWMS-748 (RWMS-439) End
    'Made changes for Retrofit Item PWMS-748 (RWMS-439) Start
    Public Overrides Function CanPlace(ByVal req As Requirment, Optional ByVal oLogger As LogHandler = Nothing) As Boolean
        'Made changes for Retrofit Item PWMS-748 (RWMS-439) End
        If _shipment = "" Then Return False
        If Count = 0 Then
            Return True
        Else
            If _shipment = req.Shipment And _staginglane = req.StagingLane Then
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
            _shipment = req.Shipment
            _staginglane = req.StagingLane
        End If
        MyBase.Place(req)
    End Sub

#End Region

End Class
