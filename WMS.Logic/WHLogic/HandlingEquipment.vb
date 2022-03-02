Public Class HandlingEquipment

#Region "Variables"
#Region "Primary Keys"
    Private _handlingEquipment As String
#End Region

#Region "Other Fields"
    Private _mobilityCode As String
    Private _weightCapacity As Decimal
    Private _allowShareAisle As Boolean

    Private _walkthreshold As Decimal
    Private _slowthreshold As Decimal
    Private _fastthreshold As Decimal

    Private _mheHoriontalconst As Decimal
    Private _mheHorizontalVariable As Decimal
    Private _MHECongestionFacor As Decimal
    Private _safetyCheckRequired As Boolean

    Private _addUser As String
    Private _editUser As String
    Private _addDate As DateTime
    Private _editDate As DateTime
#End Region

#End Region

#Region "Properties"

    Public ReadOnly Property WhereClause() As String
        Get
            Return String.Format("HANDLINGEQUIPMENT = '{0}'", _handlingEquipment)
        End Get
    End Property

    Public Property HANDLINGEQUIPMENT() As String
        Get
            Return _handlingEquipment
        End Get
        Set(ByVal Value As String)
            _handlingEquipment = Value
        End Set
    End Property

    Public Property MOBILITYCODE() As String
        Get
            Return _mobilityCode
        End Get
        Set(ByVal Value As String)
            _mobilityCode = Value
        End Set
    End Property

    Public Property WEIGHTCAPACITY() As Decimal
        Get
            Return _weightCapacity
        End Get
        Set(ByVal Value As Decimal)
            _weightCapacity = Value
        End Set
    End Property

    Public Property ALLOWSHAREAISLE() As Boolean
        Get
            Return _allowShareAisle
        End Get
        Set(ByVal Value As Boolean)
            _allowShareAisle = Value
        End Set
    End Property

    Public Property WALKTHRESHOLD() As Decimal
        Get
            Return _walkthreshold
        End Get
        Set(ByVal Value As Decimal)
            _walkthreshold = Value
        End Set
    End Property

    Public Property SLOWTHRESHOLD() As Decimal
        Get
            Return _slowthreshold
        End Get
        Set(ByVal Value As Decimal)
            _slowthreshold = Value
        End Set
    End Property

    Public Property FASTTHRESHOLD() As Decimal
        Get
            Return _fastthreshold
        End Get
        Set(ByVal Value As Decimal)
            _fastthreshold = Value
        End Set
    End Property

    Public Property MHEHORIZONTALCONST() As Decimal
        Get
            Return _mheHoriontalconst
        End Get
        Set(ByVal Value As Decimal)
            _mheHoriontalconst = Value
        End Set
    End Property

    Public Property MHEHORIZONTALVARIABLE() As Decimal
        Get
            Return _mheHorizontalVariable
        End Get
        Set(ByVal Value As Decimal)
            _mheHorizontalVariable = Value
        End Set
    End Property

    Public Property MHECONGESTIONFACTOR() As Decimal
        Get
            Return _MHECongestionFacor
        End Get
        Set(ByVal Value As Decimal)
            _MHECongestionFacor = Value
        End Set
    End Property

    Public Property SAFETYCHECKREQUIRED() As Boolean
        Get
            Return _safetyCheckRequired
        End Get
        Set(ByVal Value As Boolean)
            _safetyCheckRequired = Value
        End Set
    End Property

    Public Property ADDDATE() As DateTime
        Get
            Return _addDate
        End Get
        Set(ByVal Value As DateTime)
            _addDate = Value
        End Set
    End Property

    Public Property ADDUSER() As String
        Get
            Return _addUser
        End Get
        Set(ByVal Value As String)
            _addUser = Value
        End Set
    End Property

    Public Property EDITDATE() As DateTime
        Get
            Return _editDate
        End Get
        Set(ByVal Value As DateTime)
            _editDate = Value
        End Set
    End Property

    Public Property EDITUSER() As String
        Get
            Return _editUser
        End Get
        Set(ByVal Value As String)
            _editUser = Value
        End Set
    End Property


#End Region


    Public Shared Function Exists(ByVal pHandlingEquipment As String) As Boolean
        Dim sql As String = String.Format("Select count(1) from HANDLINGEQUIPMENT where HANDLINGEQUIPMENT = '{0}'", pHandlingEquipment)
        Return Convert.ToBoolean(Made4Net.DataAccess.DataInterface.ExecuteScalar(sql))
    End Function

    Public Sub Save()
        Dim sql As String
        _editDate = DateTime.Now
        If WMS.Logic.HandlingEquipment.Exists(_handlingEquipment) Then
            sql = String.Format("Update HANDLINGEQUIPMENT set MOBILITYCODE={0},WEIGHTCAPACITY={1},ALLOWSHAREAISLE={2},WALKTHRESHOLD={3},SLOWTHRESHOLD={4}, " & _
            "FASTTHRESHOLD={5},MHEHORIZONTALCONST={6},MHEHORIZONTALVARIABLE={7},MHECONGESTIONFACTOR={8},EDITDATE={9},EDITUSER={10},SAFETYCHECKREQUIRED={11} where {12}", _
            Made4Net.Shared.FormatField(_mobilityCode), Made4Net.Shared.FormatField(_weightCapacity), _
            Made4Net.Shared.FormatField(_allowShareAisle), Made4Net.Shared.FormatField(_walkthreshold), Made4Net.Shared.FormatField(_slowthreshold), _
                        Made4Net.Shared.FormatField(_fastthreshold), Made4Net.Shared.FormatField(_mheHoriontalconst), Made4Net.Shared.FormatField(_mheHorizontalVariable), _
                        Made4Net.Shared.FormatField(_MHECongestionFacor), Made4Net.Shared.FormatField(_editDate), Made4Net.Shared.FormatField(_editUser), _
                        Made4Net.Shared.FormatField(_safetyCheckRequired), WhereClause)
        Else
            _addDate = DateTime.Now
            sql = String.Format("Insert into HANDLINGEQUIPMENT (HANDLINGEQUIPMENT,MOBILITYCODE,WEIGHTCAPACITY,ALLOWSHAREAISLE,WALKTHRESHOLD," & _
            "SLOWTHRESHOLD,FASTTHRESHOLD,MHEHORIZONTALCONST,MHEHORIZONTALVARIABLE,MHECONGESTIONFACTOR,ADDDATE,ADDUSER,EDITDATE,EDITUSER,SAFETYCHECKREQUIRED)" & _
            " values ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14})", _
            Made4Net.Shared.FormatField(_handlingEquipment), Made4Net.Shared.FormatField(_mobilityCode), Made4Net.Shared.FormatField(_weightCapacity), _
            Made4Net.Shared.FormatField(_allowShareAisle), Made4Net.Shared.FormatField(_walkthreshold), Made4Net.Shared.FormatField(_slowthreshold), _
            Made4Net.Shared.FormatField(_fastthreshold), Made4Net.Shared.FormatField(_mheHoriontalconst), Made4Net.Shared.FormatField(_mheHorizontalVariable), _
            Made4Net.Shared.FormatField(_MHECongestionFacor), Made4Net.Shared.FormatField(_addDate), Made4Net.Shared.FormatField(_addUser), _
            Made4Net.Shared.FormatField(_editDate), Made4Net.Shared.FormatField(_editUser), Made4Net.Shared.FormatField(_safetyCheckRequired))
        End If

        Made4Net.DataAccess.DataInterface.RunSQL(sql)
    End Sub

    Public Sub Create(ByVal pHandlingEquipment As String, ByVal pMobilityCode As String, ByVal pWeightCapacity As Decimal, ByVal pAllowShareAisle As Boolean, _
    ByVal pWalkThreshold As Decimal, ByVal pSlowthreshold As Decimal, ByVal pFastThreshold As Decimal, ByVal pMHEHorizontalConst As Decimal, _
    ByVal pHHEHorizontalVariable As Decimal, ByVal pMHECongestionFactor As Decimal, ByVal pSafetyCheckRequired As Boolean, ByVal pUser As String)

        _handlingEquipment = pHandlingEquipment
        _mobilityCode = pMobilityCode
        _weightCapacity = pWeightCapacity
        _allowShareAisle = pAllowShareAisle
        _walkthreshold = pWalkThreshold
        _slowthreshold = pSlowthreshold
        _fastthreshold = pFastThreshold
        _mheHoriontalconst = pMHEHorizontalConst
        _mheHorizontalVariable = pHHEHorizontalVariable
        _MHECongestionFacor = pMHECongestionFactor
        _safetyCheckRequired = pSafetyCheckRequired
        _addUser = pUser
        _editUser = pUser

        If Exists(_handlingEquipment) Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Can not create handling equipment. It already exists.", "Can not create handling equipment. It already exists.")
        End If

        Save()
    End Sub


End Class
