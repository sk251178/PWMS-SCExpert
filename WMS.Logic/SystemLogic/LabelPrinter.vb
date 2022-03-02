Imports System.Data
Imports System.Collections
Imports Made4Net.DataAccess
Imports Made4Net.Shared.Conversion
#Region "LabelPrinter"
' <summary>
' This object represents the properties and methods of a LabelPrinter.
' </summary>

<CLSCompliant(False)> Public Class LabelPrinter

#Region "Variables"

#Region "Primary Keys"

    Protected _printqname As String = String.Empty

#End Region

#Region "Other Fields"

    Protected _physicalpath As String = String.Empty
#End Region

#End Region

#Region "Properties"

    Public ReadOnly Property WhereClause() As String
        Get
            Return " PRINTQNAME = '" & _printqname & "'"
        End Get
    End Property

    Public Property PHYSICALPATH() As String
        Get
            Return _physicalpath
        End Get
        Set(ByVal Value As String)
            _physicalpath = Value
        End Set
    End Property

    Public Property PrinterQName() As String
        Get
            Return _printqname
        End Get
        Set(ByVal Value As String)
            _printqname = Value
        End Set
    End Property

#End Region

#Region "Constructors"
    Public Sub New()
    End Sub

    Public Sub New(ByVal pprinterQueue As String, Optional ByVal LoadObj As Boolean = True)
        _printqname = pprinterQueue
        If LoadObj Then
            Load()
        End If
    End Sub
    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As DataSet = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        Dim dr As DataRow = ds.Tables(0).Rows(0)

        If CommandName.ToLower = "createnew" Then
            CreateNew(dr("PRINTQNAME"), dr("PHYSICALPATH"))
        ElseIf CommandName.ToLower = "update" Then
            Dim oLabelPrinter As New WMS.Logic.LabelPrinter(dr("PRINTQNAME"))
            oLabelPrinter.Update(dr("PRINTQNAME"), dr("PHYSICALPATH"))
        End If
    End Sub
#End Region

#Region "Methods"

    Public Shared Function GetLabelPrinter(ByVal PrinterName As String) As LabelPrinter
        Return New LabelPrinter(PrinterName)
    End Function

    Public Shared Function Exists(ByVal pprinterQueue As String) As Boolean
        Dim sql As String = String.Format("SELECT COUNT(1) FROM LABELPRINTERS WHERE PRINTQNAME = '{0}'", pprinterQueue)
        Return System.Convert.ToBoolean(DataInterface.ExecuteScalar(sql))
    End Function

    Public Sub Save(ByVal pUser As String)
        Dim SQL As String

        If (WMS.Logic.LabelPrinter.Exists(_printqname)) Then
            SQL = String.Format("UPDATE LABELPRINTERS " & _
                                "SET physicalpath ={0} WHERE PRINTQNAME={1} " & _
                                Made4Net.Shared.Util.FormatField(_physicalpath), Made4Net.Shared.Util.FormatField(_printqname))
            DataInterface.RunSQL(SQL)
        Else
            SQL = String.Format("INSERT INTO  LABELPRINTERS(PRINTQNAME,PHYSICALPATH) values({0},{1}) " & _
                      Made4Net.Shared.Util.FormatField(_printqname), Made4Net.Shared.Util.FormatField(_physicalpath))

            DataInterface.RunSQL(SQL)
        End If
    End Sub

    Public Sub CreateNew(ByVal pprinterQueue As String, ByVal pphysicalpath As String)
        Dim SQL As String = ""
        _printqname = pprinterQueue
        _physicalpath = pphysicalpath
        validate(False)
        SQL = String.Format("INSERT INTO LABELPRINTERS(PRINTQNAME,PHYSICALPATH) VALUES({0},{1}) ", _
        Made4Net.Shared.Util.FormatField(_printqname), Made4Net.Shared.Util.FormatField(_physicalpath))
        DataInterface.RunSQL(SQL)

    End Sub

    Public Sub Update(ByVal pprinterQueue As String, ByVal pphysicalpath As String)
        Dim sql As String
        _printqname = pprinterQueue
        _physicalpath = pphysicalpath
        validate(True)
        sql = String.Format("UPDATE LABELPRINTERS SET PHYSICALPATH={0} WHERE {1}", Made4Net.Shared.Util.FormatField(_physicalpath), WhereClause)
        DataInterface.RunSQL(sql)

    End Sub

    Private Sub validate(ByVal pEdit As Boolean)
        If pEdit Then
            If Not Exists(_printqname) Then
                Throw New Made4Net.Shared.M4NException(New Exception, "Can not edit LabelPrinter. It does not exist", "Can not edit LabelPrinter. It does not exist")
            End If

        Else
            If Exists(_printqname) Then
                Throw New Made4Net.Shared.M4NException(New Exception, "Can not add LabelPrinter. LabelPrinter already exists.", "Can not add LabelPrinter. LabelPrinter already exists.")
            End If
        End If
    End Sub

    Protected Sub Load()
        Dim SQL As String = "SELECT * FROM LABELPRINTERS WHERE " & WhereClause
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(SQL, dt)
        If dt.Rows.Count = 0 Then
            Throw New Made4Net.Shared.M4NException(New Exception, "Can not load LabelPrinter, it does not exist", "Can not load LabelPrinter, it does not exist")
        End If
        dr = dt.Rows(0)

        If Not dr.IsNull("PRINTQNAME") Then _printqname = dr.Item("PRINTQNAME")
        If Not dr.IsNull("PHYSICALPATH") Then _physicalpath = dr.Item("PHYSICALPATH")
    End Sub
#End Region


End Class

#End Region
