<CLSCompliant(False)> Public Class PickByCustomerStrategy
    Inherits PickBaskets

    Protected _shipment As String
    Protected _company As String
    Protected _companytype As String
    Protected _staginglane As String

#Region "Constructor"
    Public Sub New()
        MyBase.New(WMS.Lib.PickMethods.PickMethod.PICKBYCUSTOMER)
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
            If _company = req.Company And _companytype = req.CompanyType And _
            _shipment = req.Shipment And _staginglane = req.StagingLane Then
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
            _company = req.Company
            _shipment = req.Shipment
            _companytype = req.CompanyType
            _staginglane = req.StagingLane
        End If

        'Commented for Retrofit Item PWMS-748 (RWMS-439) Start
        'MyBase.Place(req)
        'Commented for Retrofit Item PWMS-748 (RWMS-439) End
        'Made changes for Retrofit Item PWMS-748 (RWMS-439) Start
        MyBase.Place(req, oLogger)
        'Made changes for Retrofit Item PWMS-748 (RWMS-439) End
    End Sub

#End Region

End Class
