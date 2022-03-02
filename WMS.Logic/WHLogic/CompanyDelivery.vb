Imports System.Data
Imports System.Collections
Imports Made4Net.DataAccess

#Region "COMPANYDELIVERY"

' <summary>
' This object represents the properties and methods of a COMPANYDELIVERY.
' </summary>

<CLSCompliant(False)> Public Class CompanyServiceTime

#Region "Variables"

#Region "Primary Keys"

    Protected _contact As String
    Protected _priority As Int32

#End Region

#Region "Other Fields"

    Protected _sunopen As Int32
    Protected _sunclose As Int32
    Protected _monopen As Int32
    Protected _monclose As Int32
    Protected _tueopen As Int32
    Protected _tueclose As Int32
    Protected _wedopen As Int32
    Protected _wedclose As Int32
    Protected _thuopen As Int32
    Protected _thuclose As Int32
    Protected _friopen As Int32
    Protected _friclose As Int32
    Protected _satopen As Int32
    Protected _satclose As Int32
    Protected _adddate As DateTime
    Protected _adduser As String = String.Empty
    Protected _editdate As DateTime
    Protected _edituser As String = String.Empty

#End Region

#End Region

#Region "Properties"

    Public ReadOnly Property WhereClause() As String
        Get
            Return " contact = '" & _contact & "' And priority = " & _priority
        End Get
    End Property

    Public ReadOnly Property Contact() As String
        Get
            Return _contact
        End Get
    End Property

    Public ReadOnly Property Priority() As Boolean
        Get
            Return _priority
        End Get
    End Property

    Public Property SunOpenHour() As Int32
        Set(ByVal Value As Int32)
            _sunopen = Value
        End Set
        Get
            Return _sunopen
        End Get
    End Property

    Public Property SunCloseHour() As Int32
        Set(ByVal Value As Int32)
            _sunclose = Value
        End Set
        Get
            Return _sunclose
        End Get
    End Property

    Public Property MonOpenHour() As Int32
        Set(ByVal Value As Int32)
            _monopen = Value
        End Set
        Get
            Return _monopen
        End Get
    End Property

    Public Property MonCloseHour() As Int32
        Set(ByVal Value As Int32)
            _monclose = Value
        End Set
        Get
            Return _monclose
        End Get
    End Property

    Public Property TueOpenHour() As Int32
        Set(ByVal Value As Int32)
            _tueopen = Value
        End Set
        Get
            Return _tueopen
        End Get
    End Property

    Public Property TueCloseHour() As Int32
        Set(ByVal Value As Int32)
            _tueclose = Value
        End Set
        Get
            Return _tueclose
        End Get
    End Property

    Public Property WedOpenHour() As Int32
        Set(ByVal Value As Int32)
            _wedopen = Value
        End Set
        Get
            Return _wedopen
        End Get
    End Property

    Public Property WedCloseHour() As Int32
        Set(ByVal Value As Int32)
            _wedclose = Value
        End Set
        Get
            Return _wedclose
        End Get
    End Property

    Public Property ThuOpenHour() As Int32
        Set(ByVal Value As Int32)
            _thuopen = Value
        End Set
        Get
            Return _thuopen
        End Get
    End Property

    Public Property ThuCloseHour() As Int32
        Set(ByVal Value As Int32)
            _thuclose = Value
        End Set
        Get
            Return _thuclose
        End Get
    End Property

    Public Property FriOpenHour() As Int32
        Set(ByVal Value As Int32)
            _friopen = Value
        End Set
        Get
            Return _friopen
        End Get
    End Property

    Public Property FriCloseHour() As Int32
        Set(ByVal Value As Int32)
            _friclose = Value
        End Set
        Get
            Return _friclose
        End Get
    End Property

    Public Property SatOpenHour() As Int32
        Set(ByVal Value As Int32)
            _satopen = Value
        End Set
        Get
            Return _satopen
        End Get
    End Property

    Public Property SatCloseHour() As Int32
        Set(ByVal Value As Int32)
            _satclose = Value
        End Set
        Get
            Return _satclose
        End Get
    End Property

    Public Property ADDDATE() As DateTime
        Set(ByVal Value As DateTime)
            _adddate = Value
        End Set
        Get
            Return _adddate
        End Get
    End Property

    Public Property ADDUSER() As String
        Set(ByVal Value As String)
            _adduser = Value
        End Set
        Get
            Return _adduser
        End Get
    End Property

    Public Property EDITDATE() As DateTime
        Set(ByVal Value As DateTime)
            _editdate = Value
        End Set
        Get
            Return _editdate
        End Get
    End Property

    Public Property EDITUSER() As String
        Set(ByVal Value As String)
            _edituser = Value
        End Set
        Get
            Return _edituser
        End Get
    End Property

#End Region

#Region "Constructors"

    Public Sub New()
    End Sub

    Public Sub New(ByVal pContact As String, ByVal pPriority As Int32, Optional ByVal LoadObj As Boolean = True)
        _contact = pContact
        _priority = pPriority
        If LoadObj Then
            Load()
        End If
    End Sub

#End Region

#Region "Methods"

    Public Shared Function GetCOMPANYServiceTime(ByVal pContact As String, ByVal pPriority As Int32) As CompanyServiceTime
        Return New CompanyServiceTime(pContact, pPriority)
    End Function

    Public Shared Function Exists(ByVal pContact As String, ByVal pPriority As Int32) As Boolean
        Dim sql As String = String.Format("Select count(1) from COMPSERVICETIME where contact = '{0}' and priority = {1}", pContact, pPriority)
        Return Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

    Protected Sub Load()

        Dim SQL As String = "SELECT * FROM COMPSERVICETIME Where " & WhereClause
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(SQL, dt)
        If dt.Rows.Count = 0 Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Company Service Time Does Not Exists", "Company Delivery Does Not Exists")
            Throw m4nEx
        End If

        dr = dt.Rows(0)
        setObj(dr)
    End Sub

    Private Sub setObj(ByVal dr As DataRow)
        Try
            If Not dr.IsNull("CONTACT") Then _contact = dr.Item("CONTACT")
        Catch ex As Exception
        End Try
        Try
            If Not dr.IsNull("PRIORITY") Then _priority = dr.Item("PRIORITY")
        Catch ex As Exception
        End Try
        Try
            If Not dr.IsNull("SUNOPEN") Then _sunopen = dr.Item("SUNOPEN")
        Catch ex As Exception
        End Try
        Try
            If Not dr.IsNull("SUNCLOSE") Then _sunclose = dr.Item("SUNCLOSE")
        Catch ex As Exception
        End Try
        Try
            If Not dr.IsNull("MONOPEN") Then _monopen = dr.Item("MONOPEN")
        Catch ex As Exception
        End Try
        Try
            If Not dr.IsNull("MONCLOSE") Then _monclose = dr.Item("MONCLOSE")
        Catch ex As Exception
        End Try
        Try
            If Not dr.IsNull("TUEOPEN") Then _tueopen = dr.Item("TUEOPEN")
        Catch ex As Exception
        End Try
        Try
            If Not dr.IsNull("TUECLOSE") Then _tueclose = dr.Item("TUECLOSE")
        Catch ex As Exception
        End Try
        Try
            If Not dr.IsNull("WEDOPEN") Then _wedopen = dr.Item("WEDOPEN")
        Catch ex As Exception
        End Try
        Try
            If Not dr.IsNull("WEDCLOSE") Then _wedclose = dr.Item("WEDCLOSE")
        Catch ex As Exception
        End Try
        Try
            If Not dr.IsNull("THUOPEN") Then _thuopen = dr.Item("THUOPEN")
        Catch ex As Exception
        End Try
        Try
            If Not dr.IsNull("THUCLOSE") Then _thuclose = dr.Item("THUCLOSE")
        Catch ex As Exception
        End Try
        Try
            If Not dr.IsNull("FRIOPEN") Then _friopen = dr.Item("FRIOPEN")
        Catch ex As Exception
        End Try
        Try
            If Not dr.IsNull("FRICLOSE") Then _friclose = dr.Item("FRICLOSE")
        Catch ex As Exception
        End Try
        Try
            If Not dr.IsNull("SATOPEN") Then _satopen = dr.Item("SATOPEN")
        Catch ex As Exception
        End Try
        Try
            If Not dr.IsNull("SATCLOSE") Then _satclose = dr.Item("SATCLOSE")
        Catch ex As Exception
        End Try
        Try
            If Not dr.IsNull("ADDDATE") Then _adddate = dr.Item("ADDDATE")
        Catch ex As Exception
        End Try
        Try
            If Not dr.IsNull("ADDUSER") Then _adduser = dr.Item("ADDUSER")
        Catch ex As Exception
        End Try
        Try
            If Not dr.IsNull("EDITDATE") Then _editdate = dr.Item("EDITDATE")
        Catch ex As Exception
        End Try
        Try
            If Not dr.IsNull("EDITUSER") Then _edituser = dr.Item("EDITUSER")
        Catch ex As Exception
        End Try
    End Sub

    Public Sub setServiceTime(ByVal dr As DataRow, ByVal pUser As String)

        Dim pConatct, pPriority As Int32
        pConatct = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("contact"), "")
        pPriority = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("priority"), "")

        setObj(dr)
        _editdate = DateTime.Now
        _edituser = pUser

        If (WMS.Logic.CompanyServiceTime.Exists(pConatct, pPriority)) Then
            updateDelivery()
        Else
            _adddate = DateTime.Now
            _adduser = pUser
            addDelivery()
        End If
    End Sub

    Public Sub setServiceTime(ByVal pContact As String, ByVal pPriority As Int32, ByVal pSunOpen As Int32, ByVal pSunClose As Int32, _
                ByVal pMonOpen As Int32, ByVal pMonClose As Int32, ByVal pTueOpen As Int32, ByVal pTueClose As Int32, _
                ByVal pWedOpen As Int32, ByVal pWedClose As Int32, ByVal pThuOpen As Int32, ByVal pThuClose As Int32, _
                 ByVal pFriOpen As Int32, ByVal pFriClose As Int32, ByVal pSatOpen As Int32, ByVal pSatClose As Int32, ByVal pUser As String)

        _contact = pContact
        _priority = pPriority
        _sunopen = pSunOpen
        _sunclose = pSunClose
        _monopen = pMonOpen
        _monclose = pMonClose
        _tueopen = pTueOpen
        _tueclose = pTueClose
        _wedopen = pWedOpen
        _wedclose = pWedClose
        _thuopen = pThuOpen
        _thuclose = pThuClose
        _friopen = pFriOpen
        _friclose = pFriClose
        _satopen = pSatOpen
        _satclose = pSatClose
        _editdate = DateTime.Now
        _edituser = pUser

        If (WMS.Logic.CompanyServiceTime.Exists(_contact, _priority)) Then
            updateDelivery()
        Else
            _adddate = DateTime.Now
            _adduser = pUser
            addDelivery()
        End If
    End Sub

    Private Sub addDelivery()
        Dim sqlInsert As String = ""
        sqlInsert = "INSERT INTO COMPSERVICETIME (CONTACT, PRIORITY, SUNOPEN, SUNCLOSE, MONOPEN, MONCLOSE, TUEOPEN, TUECLOSE, WEDOPEN, WEDCLOSE, THUOPEN, THUCLOSE, FRIOPEN, FRICLOSE, SATOPEN, SATCLOSE, ADDDATE, ADDUSER, EDITDATE, EDITUSER) Values (" & _
                    Made4Net.Shared.Util.FormatField(_contact) & "," & Made4Net.Shared.Util.FormatField(_priority) & "," & Made4Net.Shared.Util.FormatField(_sunopen) & "," & _
                    Made4Net.Shared.Util.FormatField(_sunclose) & "," & Made4Net.Shared.Util.FormatField(_monopen) & _
                    "," & Made4Net.Shared.Util.FormatField(_monclose) & "," & _
                    Made4Net.Shared.Util.FormatField(_tueopen) & "," & Made4Net.Shared.Util.FormatField(_tueclose) & "," & _
                    Made4Net.Shared.Util.FormatField(_wedopen) & "," & Made4Net.Shared.Util.FormatField(_wedclose) & "," & _
                    Made4Net.Shared.Util.FormatField(_thuopen) & "," & Made4Net.Shared.Util.FormatField(_thuclose) & "," & _
                    Made4Net.Shared.Util.FormatField(_friopen) & "," & Made4Net.Shared.Util.FormatField(_friclose) & "," & _
                    Made4Net.Shared.Util.FormatField(_satopen) & "," & Made4Net.Shared.Util.FormatField(_satclose) & "," & _
                    Made4Net.Shared.Util.FormatField(_adddate) & "," & Made4Net.Shared.Util.FormatField(_adduser) & "," & _
                    Made4Net.Shared.Util.FormatField(_editdate) & "," & Made4Net.Shared.Util.FormatField(_edituser) & ")"

        DataInterface.RunSQL(sqlInsert)
    End Sub

    Private Sub updateDelivery()
        Dim sqlUpdate As String = ""

        sqlUpdate = "Update COMPSERVICETIME set "
        sqlUpdate += "SUNOPEN = " & Made4Net.Shared.Util.FormatField(_sunopen) & ","
        sqlUpdate += "SUNCLOSE = " & Made4Net.Shared.Util.FormatField(_sunclose) & ","
        sqlUpdate += "MONOPEN = " & Made4Net.Shared.Util.FormatField(_monopen) & ","
        sqlUpdate += "MONCLOSE = " & Made4Net.Shared.Util.FormatField(_monclose) & ","
        sqlUpdate += "TUEOPEN = " & Made4Net.Shared.Util.FormatField(_tueopen) & ","
        sqlUpdate += "TUECLOSE = " & Made4Net.Shared.Util.FormatField(_tueclose) & ","
        sqlUpdate += "WEDOPEN = " & Made4Net.Shared.Util.FormatField(_wedopen) & ","
        sqlUpdate += "WEDCLOSE = " & Made4Net.Shared.Util.FormatField(_wedclose) & ","
        sqlUpdate += "THUOPEN = " & Made4Net.Shared.Util.FormatField(_thuopen) & ","
        sqlUpdate += "THUCLOSE = " & Made4Net.Shared.Util.FormatField(_thuclose) & ","
        sqlUpdate += "FRIOPEN = " & Made4Net.Shared.Util.FormatField(_friopen) & ","
        sqlUpdate += "FRICLOSE = " & Made4Net.Shared.Util.FormatField(_friclose) & ","
        sqlUpdate += "SATOPEN = " & Made4Net.Shared.Util.FormatField(_satopen) & ","
        sqlUpdate += "SATCLOSE = " & Made4Net.Shared.Util.FormatField(_satclose) & ","
        sqlUpdate += "EDITDATE = " & Made4Net.Shared.Util.FormatField(_editdate) & ","
        sqlUpdate += "EDITUSER = " & Made4Net.Shared.Util.FormatField(_edituser)
        sqlUpdate += " Where " & WhereClause

        DataInterface.RunSQL(sqlUpdate)
    End Sub

#End Region

End Class

#End Region


#Region "CompanyServiceTimeCollection"

<CLSCompliant(False)> Public Class CompanyServiceTimeCollection
    Inherits ArrayList

#Region "Variables"

    Protected _contact As String

#End Region

#Region "Properties"

    Default Public Shadows ReadOnly Property item(ByVal index As String) As CompanyServiceTime
        Get
            Return CType(MyBase.Item(index), CompanyServiceTime)
        End Get
    End Property

    Public ReadOnly Property CompServiceTime(ByVal pContact As String, ByVal pPriority As Int32) As CompanyServiceTime
        Get
            Dim i As Int32
            For i = 0 To Me.Count - 1
                If item(i).Priority = pPriority And item(i).Contact = pContact Then
                    Return (CType(MyBase.Item(i), CompanyServiceTime))
                End If
            Next
            Return Nothing
        End Get
    End Property

#End Region

#Region "Constructors"

    Public Sub New()
    End Sub

    Public Sub New(ByVal pContact As String, Optional ByVal LoadAll As Boolean = True)
        _contact = pContact
        Dim SQL As String
        Dim dt As New DataTable
        Dim dr As DataRow
        SQL = String.Format("Select priority from COMPSERVICETIME where contact ='{0}'", _contact)
        DataInterface.FillDataset(SQL, dt)
        For Each dr In dt.Rows
            Me.add(New CompanyServiceTime(_contact, dr.Item("priority"), LoadAll))
        Next
    End Sub

#End Region

#Region "Methods"

    Public Shadows Function add(ByVal pObject As CompanyServiceTime) As Integer
        Return MyBase.Add(pObject)
    End Function

    Public Shadows Sub Insert(ByVal index As String, ByVal Value As CompanyServiceTime)
        MyBase.Insert(index, Value)
    End Sub

    Public Shadows Sub Remove(ByVal pObject As CompanyServiceTime)
        MyBase.Remove(pObject)
    End Sub

    Public Sub setServiceTime(ByVal dr As DataRow, ByVal pUser As String)
        Dim oCompServiceTime As CompanyServiceTime = Me.CompServiceTime(dr("contact"), dr("priority"))
        If oCompServiceTime Is Nothing Then oCompServiceTime = New CompanyServiceTime
        oCompServiceTime.setServiceTime(dr, pUser)
        If Not Contains(oCompServiceTime) Then
            add(oCompServiceTime)
        End If
    End Sub

    Public Sub setServiceTime(ByVal pContact As String, ByVal pPriority As Int32, ByVal pSunOpen As Int32, ByVal pSunClose As Int32, _
                ByVal pMonOpen As Int32, ByVal pMonClose As Int32, ByVal pTueOpen As Int32, ByVal pTueClose As Int32, _
                ByVal pWedOpen As Int32, ByVal pWedClose As Int32, ByVal pThuOpen As Int32, ByVal pThuClose As Int32, _
                 ByVal pFriOpen As Int32, ByVal pFriClose As Int32, ByVal pSatOpen As Int32, ByVal pSatClose As Int32, ByVal pUser As String)

        Dim oCompServiceTime As CompanyServiceTime = Me.CompServiceTime(pContact, pPriority)
        If oCompServiceTime Is Nothing Then oCompServiceTime = New CompanyServiceTime
        oCompServiceTime.setServiceTime(pContact, pPriority, pSunOpen, pSunClose, pMonOpen, pMonClose, pTueOpen, pTueClose, _
               pWedOpen, pWedClose, pThuOpen, pThuClose, pFriOpen, pFriClose, pSatOpen, pSatClose, pUser)
        If Not Contains(oCompServiceTime) Then
            add(oCompServiceTime)
        End If
    End Sub

#End Region

End Class

#End Region


