<CLSCompliant(False)> Public Class LogicException
    Inherits ApplicationException

    Protected _objectSender As String
    Protected _actiondone As String
    Protected _errorcode As Int32

#Region "Properties"

    Public ReadOnly Property LogicObject() As String
        Get
            Return _objectSender
        End Get
    End Property

    Public ReadOnly Property Action() As String
        Get
            Return _actiondone
        End Get
    End Property

    Public ReadOnly Property ErrorCode() As Int32
        Get
            Return _errorcode
        End Get
    End Property

#End Region

    Public Sub New(ByVal pMessage As String, ByVal pObjectname As String, ByVal pAction As String, ByVal pErrorCode As Int32)
        MyBase.new(pMessage)
        _objectSender = pObjectname
        _actiondone = pAction
        _errorcode = pErrorCode
    End Sub

    Public Sub New(ByVal pMessage As String, ByVal pObjectname As Object, ByVal pAction As String, ByVal pErrorCode As Int32)
        MyBase.new(pMessage)
        _objectSender = pObjectname.GetType().ToString()
        _actiondone = pAction
        _errorcode = pErrorCode
    End Sub

End Class
