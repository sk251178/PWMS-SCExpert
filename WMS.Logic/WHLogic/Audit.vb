Imports System.Data
Imports System.Collections
Imports Made4Net.Shared
Imports Made4Net.DataAccess

'Commented for RWMS-1185 Start
'Imports NCR.GEMS.Core.DataCaching
'Imports NCR.GEMS.WMS.Core.WmsQueryCaching
'Commented for RWMS-1185 End

#Region "AUDIT"

' <summary>
' This object represents the properties and methods of a AUDIT.
' </summary>

<CLSCompliant(False)> Public Class Audit

#Region "Variables"

#Region "Primary Keys"

    Protected _auditId As String = String.Empty

#End Region

#Region "Other Fields"

    Protected _userid As String
    Protected _activitydate As DateTime
    Protected _activitytype As String
    Protected _document As String
    Protected _documentline As Int32
    Protected _consignee As String
    Protected _sku As String
    Protected _fromload As String
    Protected _toload As String
    Protected _fromstatus As String
    Protected _tostatus As String
    Protected _fromloc As String
    Protected _toloc As String
    Protected _fromwarehousearea As String
    Protected _towarehousearea As String
    Protected _fromcontainer As String
    Protected _tocontainer As String
    Protected _fromqty As Decimal
    Protected _toqty As Decimal
    Protected _activitytime As Int32
    Protected _notes As String
    Protected _reasoncode As String
    Protected _adddate As DateTime
    Protected _adduser As String
    Protected _editdate As DateTime
    Protected _edituser As String

#End Region

#End Region

#Region "Properties"

    Public Property USERID() As String
        Get
            Return _userid
        End Get
        Set(ByVal Value As String)
            _userid = Value
        End Set
    End Property

    Public Property ACTIVITYDATE() As DateTime
        Get
            Return _activitydate
        End Get
        Set(ByVal Value As DateTime)
            _activitydate = Value
        End Set
    End Property

    Public Property ACTIVITYTYPE() As String
        Get
            Return _activitytype
        End Get
        Set(ByVal Value As String)
            _activitytype = Value
        End Set
    End Property

    Public Property DOCUMENT() As String
        Get
            Return _document
        End Get
        Set(ByVal Value As String)
            _document = Value
        End Set
    End Property

    Public Property DOCUMENTLINE() As Int32
        Get
            Return _documentline
        End Get
        Set(ByVal Value As Int32)
            _documentline = Value
        End Set
    End Property

    Public Property CONSIGNEE() As String
        Get
            Return _consignee
        End Get
        Set(ByVal Value As String)
            _consignee = Value
        End Set
    End Property

    Public Property SKU() As String
        Get
            Return _sku
        End Get
        Set(ByVal Value As String)
            _sku = Value
        End Set
    End Property

    Public Property FROMLOAD() As String
        Get
            Return _fromload
        End Get
        Set(ByVal Value As String)
            _fromload = Value
        End Set
    End Property

    Public Property TOLOAD() As String
        Get
            Return _toload
        End Get
        Set(ByVal Value As String)
            _toload = Value
        End Set
    End Property

    Public Property FROMSTATUS() As String
        Get
            Return _fromstatus
        End Get
        Set(ByVal Value As String)
            _fromstatus = Value
        End Set
    End Property

    Public Property TOSTATUS() As String
        Get
            Return _tostatus
        End Get
        Set(ByVal Value As String)
            _tostatus = Value
        End Set
    End Property

    Public Property FROMLOC() As String
        Get
            Return _fromloc
        End Get
        Set(ByVal Value As String)
            _fromloc = Value
        End Set
    End Property

    Public Property TOLOC() As String
        Get
            Return _toloc
        End Get
        Set(ByVal Value As String)
            _toloc = Value
        End Set
    End Property

    Public Property FROMWAREHOUSEAREA() As String
        Get
            Return _fromwarehousearea
        End Get
        Set(ByVal Value As String)
            _fromwarehousearea = Value
        End Set
    End Property

    Public Property TOWAREHOUSEAREA() As String
        Get
            Return _towarehousearea
        End Get
        Set(ByVal Value As String)
            _towarehousearea = Value
        End Set
    End Property

    Public Property FROMCONTAINER() As String
        Get
            Return _fromcontainer
        End Get
        Set(ByVal Value As String)
            _fromcontainer = Value
        End Set
    End Property

    Public Property TOCONTAINER() As String
        Get
            Return _tocontainer
        End Get
        Set(ByVal Value As String)
            _tocontainer = Value
        End Set
    End Property

    Public Property FROMQTY() As Decimal
        Get
            Return _fromqty
        End Get
        Set(ByVal Value As Decimal)
            _fromqty = Value
        End Set
    End Property

    Public Property TOQTY() As Decimal
        Get
            Return _toqty
        End Get
        Set(ByVal Value As Decimal)
            _toqty = Value
        End Set
    End Property

    Public Property ACTIVITYTIME() As Int32
        Get
            Return _activitytime
        End Get
        Set(ByVal Value As Int32)
            _activitytime = Value
        End Set
    End Property

    Public Property NOTES() As String
        Get
            Return _notes
        End Get
        Set(ByVal Value As String)
            _notes = Value
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

#End Region

#Region "Methods"

    Public Sub Post(Optional ByVal qSender As Made4Net.Shared.QMsgSender = Nothing, Optional ByVal _qhlogger As Made4Net.Shared.Logging.LogFile = Nothing)
        Dim SQL As String = ""

        _auditId = Made4Net.Shared.Util.getNextCounter("AUDIT")

        If qSender Is Nothing Then
            SQL = "INSERT INTO AUDIT (AUDITID,USERID,ACTIVITYDATE,ACTIVITYTYPE,DOCUMENT,DOCUMENTLINE,CONSIGNEE,SKU,FROMLOAD," & _
                  "TOLOAD,FROMSTATUS,TOSTATUS,FROMLOC,TOLOC,FROMWAREHOUSEAREA, TOWAREHOUSEAREA, FROMQTY,TOQTY,ACTIVITYTIME,NOTES,FROMCONTAINER,TOCONTAINER, ADDDATE,ADDUSER,EDITDATE,EDITUSER) Values ("

            SQL += Made4Net.Shared.Util.FormatField(_auditId) & "," & Made4Net.Shared.Util.FormatField(_userid) & "," & Made4Net.Shared.Util.FormatField(_activitydate) & "," & Made4Net.Shared.Util.FormatField(_activitytype) & "," & _
                    Made4Net.Shared.Util.FormatField(_document) & "," & Made4Net.Shared.Util.FormatField(_documentline) & "," & Made4Net.Shared.Util.FormatField(_consignee) & "," & _
                    Made4Net.Shared.Util.FormatField(_sku) & "," & Made4Net.Shared.Util.FormatField(_fromload) & "," & Made4Net.Shared.Util.FormatField(_toload) & "," & _
                    Made4Net.Shared.Util.FormatField(_fromstatus) & "," & Made4Net.Shared.Util.FormatField(_tostatus) & "," & Made4Net.Shared.Util.FormatField(_fromloc) & "," & _
                    Made4Net.Shared.Util.FormatField(_toloc) & "," & Made4Net.Shared.Util.FormatField(_fromwarehousearea) & "," & _
                    Made4Net.Shared.Util.FormatField(_towarehousearea) & "," & Made4Net.Shared.Util.FormatField(_fromqty) & "," & Made4Net.Shared.Util.FormatField(_toqty) & "," & _
                    Made4Net.Shared.Util.FormatField(_activitytime) & "," & Made4Net.Shared.Util.FormatField(_notes) & "," & Made4Net.Shared.Util.FormatField(_fromcontainer) & "," & Made4Net.Shared.Util.FormatField(_tocontainer) & "," & Made4Net.Shared.Util.FormatField(_adddate) & "," & Made4Net.Shared.Util.FormatField(_adduser) & "," & _
                    Made4Net.Shared.Util.FormatField(_editdate) & "," & Made4Net.Shared.Util.FormatField(_edituser) & ")"

            DataInterface.RunSQL(SQL)
        Else
            PostFromQSender(qSender, _qhlogger)
        End If


    End Sub

    Private Sub PostFromQSender(ByVal qSender As Made4Net.Shared.QMsgSender, ByVal _qhlogger As Made4Net.Shared.Logging.LogFile)
        ' Fill all fields from qSender
        Dim SQL As String
        ' Set System Fields
        Dim Fields As String = "AUDITID, ADDDATE, EDITDATE, ADDUSER, EDITUSER"
        Dim Values As String = "'" & Made4Net.Shared.Util.getNextCounter("AUDIT") & "',GETDATE(),GETDATE(),'" & Common.GetCurrentUser & "','" & Common.GetCurrentUser & "'"

        Dim TableStructureSQL As String = "SELECT COLUMN_NAME,DATA_TYPE,CHARACTER_MAXIMUM_LENGTH,NUMERIC_PRECISION,NUMERIC_SCALE,IS_NULLABLE,COLUMNPROPERTY(OBJECT_ID(TABLE_NAME), COLUMN_NAME, 'IsIdentity') AS IS_AUTOINCREMENT,COLUMN_DEFAULT " & _
                                            "FROM INFORMATION_SCHEMA.COLUMNS " & _
                                            "WHERE TABLE_NAME = 'AUDIT' AND (COLUMN_NAME<>'AUDITID' AND COLUMN_NAME<>'ADDDATE' AND COLUMN_NAME<>'ADDUSER' AND COLUMN_NAME<>'EDITDATE' AND COLUMN_NAME<>'EDITUSER') " & _
                                            "ORDER BY ORDINAL_POSITION"

        Dim dataTable As DataTable

        'Commented for RWMS-1185 Start
        'Dim objectId As String = "AUDIT"

        'If Not GemsQueryCaching.IsQueryEnabled(Of WmsQueryCachingIdTypes)(WmsQueryCachingIdTypes.AuditTableStructureSchema, objectId) Then
        '    dataTable = New DataTable()
        '    DataInterface.FillDataset(TableStructureSQL, dataTable)
        'Else
        '    dataTable = GemsQueryCaching.GetCachedObject(Of WmsQueryCachingIdTypes)(WmsQueryCachingIdTypes.AuditTableStructureSchema, objectId)
        '    If IsNothing(dataTable) Then
        '        dataTable = New DataTable()
        '        DataInterface.FillDataset(TableStructureSQL, dataTable)
        '        GemsQueryCaching.SetCachedObject(Of WmsQueryCachingIdTypes)(WmsQueryCachingIdTypes.AuditTableStructureSchema, objectId, dataTable, TableStructureSQL)
        '    End If
        'End If
        'Commented for RWMS-1185 End

        'Added for RWMS-1185 Start
        dataTable = New DataTable()
        DataInterface.FillDataset(TableStructureSQL, dataTable)
        'Added for RWMS-1185 End

        For Each ColumnRow As DataRow In dataTable.Rows
            If qSender.Values(ColumnRow("COLUMN_NAME")) Is Nothing Then Continue For


            ' In this case we have attributes in the table
            Try
                Select Case Convert.ToString(ColumnRow("DATA_TYPE"))
                    Case "nvarchar"
                        Convert.ToString(qSender.Values(ColumnRow("COLUMN_NAME")))
                        ' RWMS-2415
                        Dim length As Integer = ColumnRow("CHARACTER_MAXIMUM_LENGTH")
                        Fields = Fields & "," & ColumnRow("COLUMN_NAME")
                        Dim strValue As String = qSender.Values(ColumnRow("COLUMN_NAME")).ToString()
                        If strValue.Length > length Then
                            Dim truncatedString As String = strValue.Substring(0, length)
                            Values = Values & "," & Made4Net.Shared.Util.FormatField(truncatedString)
                        Else
                            Values = Values & "," & Made4Net.Shared.Util.FormatField(strValue)
                        End If
                        ' RWMS-2415
                    Case "int"
                        Dim valueInQSender As Integer
                        If Not Integer.TryParse(qSender.Values(ColumnRow("COLUMN_NAME")), valueInQSender) Then
                            valueInQSender = 0
                        End If
                        Fields = Fields & "," & ColumnRow("COLUMN_NAME")
                        Values = Values & "," & Made4Net.Shared.Util.FormatField(valueInQSender.ToString())
                    Case "decimal"
                        Dim valueInQSender As Decimal
                        If Not Decimal.TryParse(qSender.Values(ColumnRow("COLUMN_NAME")), valueInQSender) Then
                            valueInQSender = 0
                        End If
                        Fields = Fields & "," & ColumnRow("COLUMN_NAME")
                        Values = Values & "," & Made4Net.Shared.Util.FormatField(valueInQSender.ToString())
                    Case "datetime"
                        'Convert.ToDateTime(qSender.Values(ColumnRow("COLUMN_NAME")))
                        Dim dateTimeObj As DateTime = Made4Net.Shared.Util.WMSStringToDate(qSender.Values(ColumnRow("COLUMN_NAME")))
                        Fields = Fields & "," & ColumnRow("COLUMN_NAME")
                        If dateTimeObj.Year > 1900 Then
                            Values = Values & "," & Made4Net.Shared.Util.FormatField(dateTimeObj)
                        Else
                            Values = Values & ",NULL"
                        End If
                        'If qSender.Values(ColumnRow("COLUMN_NAME")) <> "0001-01-01 12:00:00" Then
                        '    Values = Values & "," & Made4Net.Shared.Util.FormatField(qSender.Values(ColumnRow("COLUMN_NAME")))
                        'Else
                        '    Values = Values & ",NULL"
                        'End If
                    Case Else
                        Convert.ToString(qSender.Values(ColumnRow("COLUMN_NAME")))
                        Fields = Fields & "," & ColumnRow("COLUMN_NAME")
                        Values = Values & "," & Made4Net.Shared.Util.FormatField(qSender.Values(ColumnRow("COLUMN_NAME")))
                End Select
            Catch ex As Exception
                If Not _qhlogger Is Nothing Then
                    _qhlogger.WriteLine("Ex Message " + ex.Message, True)
                    _qhlogger.WriteLine("Ex Stack Trace " + ex.StackTrace, True)
                End If
            End Try
        Next

        SQL = "INSERT INTO AUDIT(" & Fields & ") VALUES(" & Values & ")"
        If Not _qhlogger Is Nothing Then
            _qhlogger.WriteLine("SQL  " + SQL, True)
        End If

        DataInterface.RunSQL(SQL)
    End Sub

#End Region

End Class

#End Region

