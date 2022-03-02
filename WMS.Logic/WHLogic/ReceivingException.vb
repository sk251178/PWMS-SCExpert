Imports System.Data
Imports System.Collections
Imports Made4Net.DataAccess
Imports Made4Net.Shared.Conversion

<CLSCompliant(False)> Public Class ReceivingException

#Region "Variables"

    Private _receipt As String = String.Empty
    Private _receiptline As String = String.Empty
    Private _qty As Decimal
    Private _reasoncode As String = String.Empty

    Private _adddate As DateTime
    Private _adduser As String = String.Empty
    Private _editdate As DateTime
    Private _edituser As String = String.Empty

#End Region

#Region "Properties"

    Public ReadOnly Property WhereClause() As String
        Get
            Return " RECEIPT = '" & _receipt & "' AND RECEIPTLINE = '" & _receiptline & "'"
        End Get
    End Property

    Public Property RECEIPT() As String
        Get
            Return _receipt
        End Get
        Set(ByVal Value As String)
            _receipt = Value
        End Set
    End Property

    Public Property RECEIPTLINE() As String
        Get
            Return _receiptline
        End Get
        Set(ByVal Value As String)
            _receiptline = Value
        End Set
    End Property

    Public Property QTY() As Decimal
        Get
            Return _qty
        End Get
        Set(ByVal Value As Decimal)
            _qty = Value
        End Set
    End Property

    Public Property REASONCODE() As String
        Get
            Return _reasoncode
        End Get
        Set(ByVal Value As String)
            _reasoncode = Value
        End Set
    End Property

    Public Property ADDDATE() As DateTime
        Get
            Return _adddate
        End Get
        Set(ByVal Value As DateTime)
            _adddate = Value
        End Set
    End Property

    Public Property ADDUSER() As String
        Get
            Return _adduser
        End Get
        Set(ByVal Value As String)
            _adduser = Value
        End Set
    End Property

    Public Property EDITDATE() As DateTime
        Get
            Return _editdate
        End Get
        Set(ByVal Value As DateTime)
            _editdate = Value
        End Set
    End Property

    Public Property EDITUSER() As String
        Get
            Return _edituser
        End Get
        Set(ByVal Value As String)
            _edituser = Value
        End Set
    End Property

#End Region

#Region "Constructors"

    Public Sub New()

    End Sub

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As New DataSet
        Dim dr As DataRow
        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        If CommandName.ToLower = "insert" Then
            Dim re As ReceivingException = New ReceivingException()
            dr = ds.Tables(0).Rows(0)
            re.RECEIPT = Convert.ReplaceDBNull(dr("RECEIPT"))
            re.RECEIPTLINE = Convert.ReplaceDBNull(dr("RECEIPTLINE"))
            re.REASONCODE = Convert.ReplaceDBNull(dr("REASONCODE"))
            re.QTY = Convert.ReplaceDBNull(dr("QTY"))
            If re.CanSave() Then
                re.Save(WMS.Logic.Common.GetCurrentUser)
            Else
                Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Quantity or status incorrect", "Quantity or status  incorrect")
                Throw m4nEx
            End If
        ElseIf CommandName.ToLower = "update" Then
            Dim re As ReceivingException = New ReceivingException()
            dr = ds.Tables(0).Rows(0)
            re.RECEIPT = Convert.ReplaceDBNull(dr("RECEIPT"))
            re.RECEIPTLINE = Convert.ReplaceDBNull(dr("RECEIPTLINE"))
            re.QTY = Convert.ReplaceDBNull(dr("QTY"))
            re.REASONCODE = Convert.ReplaceDBNull(dr("REASONCODE"))

            If re.CanSave() Then
                re.Save(WMS.Logic.Common.GetCurrentUser)
            Else
                Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Quantity or status  incorrect", "Quantity or status  incorrect")
                Throw m4nEx
            End If
        End If
    End Sub

    Public Sub New(ByVal pRECEIPT As String, ByVal pRECEIPTLINE As String, Optional ByVal LoadObj As Boolean = True)
        _receipt = pRECEIPT
        _receiptline = pRECEIPTLINE
        If LoadObj Then
            Load()
        End If
    End Sub

#End Region

#Region "Methods"

    Protected Sub Load()
        Dim SQL As String = "SELECT * FROM RECEIVINGEXCEPTION WHERE " & WhereClause
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(SQL, dt)

        dr = dt.Rows(0)

        If Not dr.IsNull("RECEIPT") Then _receipt = dr.Item("RECEIPT")
        If Not dr.IsNull("RECEIPTLINE") Then _receiptline = dr.Item("RECEIPTLINE")
        If Not dr.IsNull("QTY") Then _qty = dr.Item("QTY")
        If Not dr.IsNull("REASONCODE") Then _reasoncode = dr.Item("REASONCODE")
        If Not dr.IsNull("ADDDATE") Then _adddate = dr.Item("ADDDATE")
        If Not dr.IsNull("ADDUSER") Then _adduser = dr.Item("ADDUSER")
        If Not dr.IsNull("EDITDATE") Then _editdate = dr.Item("EDITDATE")
        If Not dr.IsNull("EDITUSER") Then _edituser = dr.Item("EDITUSER")
    End Sub

    Public Shared Function Exists(ByVal pReceiptId As String, ByVal pReceiptLine As String) As Boolean
        Dim sql As String = String.Format("SELECT COUNT(1) FROM RECEIVINGEXCEPTION WHERE RECEIPT = '{0}' AND RECEIPTLINE = '{1}'", pReceiptId, pReceiptLine)
        Return System.Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

    Public Function CanSave()
        Dim rh As ReceiptHeader = New ReceiptHeader(_receipt)
        If rh.STATUS = WMS.Lib.Statuses.Receipt.CLOSED Or rh.STATUS = WMS.Lib.Statuses.Receipt.CANCELLED Then
            Return False
        End If
        Dim rd As ReceiptDetail = New ReceiptDetail(_receipt, _receiptline)
        If rd.QTYEXPECTED - rd.QTYRECEIVED > 0 Then
            If rd.QTYEXPECTED - rd.QTYRECEIVED >= _qty Then
                Return True
            Else
                Return False
            End If
        Else
            Return False
        End If
    End Function

    Public Sub Save(ByVal pUser As String)
        Dim sql As String = ""
        If Exists(_receipt, _receiptline) Then
            ' Update
            _editdate = DateTime.Now
            _edituser = pUser
            sql = "UPDATE [RECEIVINGEXCEPTION] " & _
                   " SET [QTY] = " & Made4Net.Shared.Util.FormatField(_qty) & _
                      ",[REASONCODE] = " & Made4Net.Shared.Util.FormatField(_reasoncode) & _
                      ",[EDITDATE] = " & Made4Net.Shared.Util.FormatField(_editdate) & _
                      ",[EDITUSER] = " & Made4Net.Shared.Util.FormatField(_edituser) & _
                 " WHERE " & WhereClause
        Else
            ' Insert
            _adddate = DateTime.Now
            _editdate = DateTime.Now
            _adduser = pUser
            _edituser = pUser
            sql = "INSERT INTO [RECEIVINGEXCEPTION] " & _
                           "([RECEIPT]" & _
                           ",[RECEIPTLINE]" & _
                           ",[QTY]" & _
                           ",[REASONCODE]" & _
                           ",[ADDDATE]" & _
                           ",[ADDUSER]" & _
                           ",[EDITDATE]" & _
                           ",[EDITUSER])" & _
                           " VALUES " & _
                           "(" & Made4Net.Shared.Util.FormatField(_receipt) & _
                           "," & Made4Net.Shared.Util.FormatField(_receiptline) & _
                           "," & Made4Net.Shared.Util.FormatField(_qty) & _
                           "," & Made4Net.Shared.Util.FormatField(_reasoncode) & _
                           "," & Made4Net.Shared.Util.FormatField(_adddate) & _
                           "," & Made4Net.Shared.Util.FormatField(_adduser) & _
                           "," & Made4Net.Shared.Util.FormatField(_editdate) & _
                           "," & Made4Net.Shared.Util.FormatField(_edituser) & ")"
        End If
        DataInterface.RunSQL(sql)
    End Sub

#End Region

End Class
