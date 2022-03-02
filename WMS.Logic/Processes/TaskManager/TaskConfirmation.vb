Imports Made4Net.Shared
Imports System.Collections.Specialized
Public Interface ITaskConfirmation
    ReadOnly Property ConfirmationType() As String
    Sub Confirm()
    Property ConfirmationValues() As NameValueCollection
End Interface

Public Class TaskConfirmationLoad
    Implements ITaskConfirmation
    Private _load As String
    Private _confirmationValues

    Public Property ConfirmationValues() As NameValueCollection Implements ITaskConfirmation.ConfirmationValues
        Get
            Return _confirmationValues
        End Get
        Set(ByVal value As NameValueCollection)
            _confirmationValues = value
        End Set
    End Property

    Public Sub New(ByVal pLoad As String)
        _load = pLoad
    End Sub

    Public ReadOnly Property ConfirmationType() As String Implements ITaskConfirmation.ConfirmationType
        Get
            Return WMS.Lib.CONFIRMATIONTYPE.LOAD
        End Get
    End Property

    Public Sub Confirm() Implements ITaskConfirmation.Confirm
        If String.Compare(_confirmationValues("LOAD"), _load, True) <> 0 Then
            Throw New M4NException(New Exception(), "Confirmation failed", "Confirmation failed")
        End If
    End Sub



End Class

Public Class TaskConfirmationLocation
    Implements ITaskConfirmation
    Private _location As String
    Private _warehouseArea As String
    Private _confirmationValues

    Public Property ConfirmationValues() As NameValueCollection Implements ITaskConfirmation.ConfirmationValues
        Get
            Return _confirmationValues
        End Get
        Set(ByVal value As NameValueCollection)
            _confirmationValues = value
        End Set
    End Property
    Public Sub New(ByVal pLocation As String, ByVal pWarehouseArea As String)
        _location = pLocation
        _warehouseArea = pWarehouseArea
    End Sub

    Public ReadOnly Property ConfirmationType() As String Implements ITaskConfirmation.ConfirmationType
        Get
            Return WMS.Lib.CONFIRMATIONTYPE.LOCATION
        End Get
    End Property

    Public Sub Confirm() Implements ITaskConfirmation.Confirm
        If String.Compare(_confirmationValues("LOCATION"), _location, True) <> 0 OrElse String.Compare(_confirmationValues("WAREHOUSEAREA"), _warehouseArea, True) <> 0 Then
            Throw New M4NException(New Exception(), "Confirmation failed", "Confirmation failed")
        End If
    End Sub
End Class

Public Class TaskConfirmationNone
    Implements ITaskConfirmation
    Private _confirmationValues

    Public Property ConfirmationValues() As NameValueCollection Implements ITaskConfirmation.ConfirmationValues
        Get
            Return _confirmationValues
        End Get
        Set(ByVal value As NameValueCollection)
            _confirmationValues = value
        End Set
    End Property
    Public Sub New()
    End Sub

    Public ReadOnly Property ConfirmationType() As String Implements ITaskConfirmation.ConfirmationType
        Get
            Return WMS.Lib.CONFIRMATIONTYPE.NONE
        End Get
    End Property

    Public Sub Confirm() Implements ITaskConfirmation.Confirm
        Return
    End Sub


End Class

Public Class TaskConfirmationSKU
    Implements ITaskConfirmation

    Private _sku As String
    Private _confirmationValues

    Public Property ConfirmationValues() As NameValueCollection Implements ITaskConfirmation.ConfirmationValues
        Get
            Return _confirmationValues
        End Get
        Set(ByVal value As NameValueCollection)
            _confirmationValues = value
        End Set
    End Property
    Public Sub New(ByVal pSKU As String)
        _sku = pSKU
    End Sub

    Public ReadOnly Property ConfirmationType() As String Implements ITaskConfirmation.ConfirmationType
        Get
            Return WMS.Lib.CONFIRMATIONTYPE.SKU
        End Get
    End Property

    Public Sub Confirm() Implements ITaskConfirmation.Confirm
        Dim sql As String = String.Format("SELECT * FROM vSKUCODE WHERE SKUCODE='" & _confirmationValues("SKU") & "' OR SKU='" & _confirmationValues("SKU") & "'")
        Dim dt As DataTable = New DataTable
        Made4Net.DataAccess.DataInterface.FillDataset(sql, dt)

        For Each dr As DataRow In dt.Rows
            If String.Compare(dr("SKU"), _sku, True) = 0 Then
                Return
            End If
        Next

        Throw New M4NException(New Exception(), "Confirmation failed", "Confirmation failed")
        'If String.Compare(_confirmationValues("SKU"), _sku, True) <> 0 Then
        '    Throw New M4NException(New Exception(), "Confirmation failed", "Confirmation failed")
        'End If
    End Sub

End Class

Public Class TaskConfirmationSKULocation
    Implements ITaskConfirmation

    Private _sku As String
    Private _location As String
    Private _warehoueseArea As String
    Private _confirmationValues

    Public Property ConfirmationValues() As NameValueCollection Implements ITaskConfirmation.ConfirmationValues
        Get
            Return _confirmationValues
        End Get
        Set(ByVal value As NameValueCollection)
            _confirmationValues = value
        End Set
    End Property
    Public Sub New(ByVal pSKU As String, ByVal pLocation As String, ByVal pWarehouseArea As String)
        _sku = pSKU
        _location = pLocation
        _warehoueseArea = pWarehouseArea
    End Sub

    Public ReadOnly Property ConfirmationType() As String Implements ITaskConfirmation.ConfirmationType
        Get
            Return WMS.Lib.CONFIRMATIONTYPE.SKULOCATION
        End Get
    End Property

    Public Sub Confirm() Implements ITaskConfirmation.Confirm
        'If String.Compare(_confirmationValues("SKU"), _sku, True) <> 0 Then
        '    Throw New M4NException(New Exception(), "Confirmation failed", "Confirmation failed")
        'End If
        If String.Compare(_confirmationValues("LOCATION"), _location, True) <> 0 OrElse String.Compare(_confirmationValues("WAREHOUSEAREA"), _warehoueseArea, True) <> 0 Then
            Throw New M4NException(New Exception(), "Confirmation failed", "Confirmation failed")
        End If

        Dim sql As String = String.Format("SELECT * FROM vSKUCODE WHERE SKUCODE='" & _confirmationValues("SKU") & "' OR SKU='" & _confirmationValues("SKU") & "'")
        Dim dt As DataTable = New DataTable
        Made4Net.DataAccess.DataInterface.FillDataset(sql, dt)

        For Each dr As DataRow In dt.Rows
            If String.Compare(dr("SKU"), _sku, True) = 0 Then
                Return
            End If
        Next

        Throw New M4NException(New Exception(), "Confirmation failed", "Confirmation failed")
    End Sub


End Class

Public Class TaskConfirmationSKULocationUOM
    Implements ITaskConfirmation

    Private _sku As String
    Private _location As String
    Private _warehouseArea As String
    Private _uom As String
    Private _confirmationValues

    Public Property ConfirmationValues() As NameValueCollection Implements ITaskConfirmation.ConfirmationValues
        Get
            Return _confirmationValues
        End Get
        Set(ByVal value As NameValueCollection)
            _confirmationValues = value
        End Set
    End Property
    Public Sub New(ByVal pSKU As String, ByVal pLocation As String, ByVal pWarehouseArea As String, ByVal pUOM As String)
        _sku = pSKU
        _location = pLocation
        _warehouseArea = pWarehouseArea
        _uom = pUOM
    End Sub

    Public ReadOnly Property ConfirmationType() As String Implements ITaskConfirmation.ConfirmationType
        Get
            Return WMS.Lib.CONFIRMATIONTYPE.SKULOCATIONUOM
        End Get
    End Property

    Public Sub Confirm() Implements ITaskConfirmation.Confirm
        'If String.Compare(_confirmationValues("SKU"), _sku, True) <> 0 Then
        '    Throw New M4NException(New Exception(), "Confirmation failed", "Confirmation failed")
        'End If
        If String.Compare(_confirmationValues("LOCATION"), _location, True) <> 0 OrElse String.Compare(_confirmationValues("WAREHOUSEAREA"), _warehouseArea, True) <> 0 Then
            Throw New M4NException(New Exception(), "Confirmation failed", "Confirmation failed")
        End If
        If String.Compare(_confirmationValues("UOM"), _uom, True) <> 0 Then
            Throw New M4NException(New Exception(), "Confirmation failed", "Confirmation failed")
        End If

        Dim sql As String = String.Format("SELECT * FROM vSKUCODE WHERE SKUCODE='" & _confirmationValues("SKU") & "' OR SKU='" & _confirmationValues("SKU") & "'")
        Dim dt As DataTable = New DataTable
        Made4Net.DataAccess.DataInterface.FillDataset(sql, dt)

        For Each dr As DataRow In dt.Rows
            If String.Compare(dr("SKU"), _sku, True) = 0 Then
                Return
            End If
        Next

        Throw New M4NException(New Exception(), "Confirmation failed", "Confirmation failed")

    End Sub


End Class

Public Class TaskConfirmationSKUUOM
    Implements ITaskConfirmation

    Private _sku As String
    Private _uom As String
    Private _confirmationValues

    Public Property ConfirmationValues() As NameValueCollection Implements ITaskConfirmation.ConfirmationValues
        Get
            Return _confirmationValues
        End Get
        Set(ByVal value As NameValueCollection)
            _confirmationValues = value
        End Set
    End Property

    Public Sub New(ByVal pSKU As String, ByVal pUOM As String)
        _sku = pSKU
        _uom = pUOM
    End Sub

    Public ReadOnly Property ConfirmationType() As String Implements ITaskConfirmation.ConfirmationType
        Get
            Return WMS.Lib.CONFIRMATIONTYPE.SKUUOM
        End Get
    End Property

    Public Sub Confirm() Implements ITaskConfirmation.Confirm
        'If String.Compare(_confirmationValues("SKU"), _sku, True) <> 0 Then
        '    Throw New M4NException(New Exception(), "Confirmation failed", "Confirmation failed")
        'End If
        If String.Compare(_confirmationValues("UOM"), _uom, True) <> 0 Then
            Throw New M4NException(New Exception(), "Confirmation failed", "Confirmation failed")
        End If

        Dim sql As String = String.Format("SELECT * FROM vSKUCODE WHERE SKUCODE='" & _confirmationValues("SKU") & "' OR SKU='" & _confirmationValues("SKU") & "'")
        Dim dt As DataTable = New DataTable
        Made4Net.DataAccess.DataInterface.FillDataset(sql, dt)

        For Each dr As DataRow In dt.Rows
            If String.Compare(dr("SKU"), _sku, True) = 0 Then
                Return
            End If
        Next

        Throw New M4NException(New Exception(), "Confirmation failed", "Confirmation failed")

    End Sub


End Class

Public Class TaskConfirmationUPC
    Implements ITaskConfirmation

    Private _upc As String
    Private _confirmationValues

    Public Property ConfirmationValues() As NameValueCollection Implements ITaskConfirmation.ConfirmationValues
        Get
            Return _confirmationValues
        End Get
        Set(ByVal value As NameValueCollection)
            _confirmationValues = value
        End Set
    End Property
    Public Sub New(ByVal pUPC As String)
        _upc = pUPC
    End Sub

    Public ReadOnly Property ConfirmationType() As String Implements ITaskConfirmation.ConfirmationType
        Get
            Return WMS.Lib.CONFIRMATIONTYPE.UPC
        End Get
    End Property

    Public Sub Confirm() Implements ITaskConfirmation.Confirm
        If String.Compare(_confirmationValues("UPC"), _upc, True) <> 0 Then
            Throw New M4NException(New Exception(), "Confirmation failed", "Confirmation failed")
        End If
    End Sub


End Class


