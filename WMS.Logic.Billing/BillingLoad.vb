Imports System.Data
Imports System.Collections
Imports Made4Net.DataAccess
Imports Made4Net.Shared.Conversion

Public Class BillingLoad

#Region "Variables"

    Protected _loadid As String
    Protected _billingloadid As String
    Protected _consignee As String
    Protected _sku As String
    Protected _uom As String
    Protected _startdate As DateTime = Nothing
    Protected _enddate As DateTime = Nothing
    Protected _lastdate As DateTime = Nothing
    Protected _currentqty As Decimal
    Protected _adduser As String
    Protected _adddate As DateTime = Nothing
    Protected _edituser As String
    Protected _editdate As DateTime = Nothing

#End Region

#Region "Properties"

    Public Property LoadId() As String
        Get
            Return _loadid
        End Get
        Set(ByVal Value As String)
            _loadid = Value
        End Set
    End Property

    Public Property BillingLoadId() As String
        Get
            Return _billingloadid
        End Get
        Set(ByVal Value As String)
            _billingloadid = Value
        End Set
    End Property

    Public Property Consignee() As String
        Get
            Return _consignee
        End Get
        Set(ByVal Value As String)
            _consignee = Value
        End Set
    End Property

    Public Property Sku() As String
        Get
            Return _sku
        End Get
        Set(ByVal Value As String)
            _sku = Value
        End Set
    End Property

    Public Property UOM() As String
        Get
            Return _uom
        End Get
        Set(ByVal Value As String)
            _uom = Value
        End Set
    End Property

    Public Property StartDate() As DateTime
        Get
            Return _startdate
        End Get
        Set(ByVal Value As DateTime)
            _startdate = Value
        End Set
    End Property

    Public Property EndDate() As DateTime
        Get
            Return _enddate
        End Get
        Set(ByVal Value As DateTime)
            _enddate = Value
        End Set
    End Property

    Public Property LastDate() As DateTime
        Get
            Return _lastdate
        End Get
        Set(ByVal Value As DateTime)
            _lastdate = Value
        End Set
    End Property

    Public Property CurrentQty() As Decimal
        Get
            Return _currentqty
        End Get
        Set(ByVal Value As Decimal)
            _currentqty = Value
        End Set
    End Property

    Public Property AddUser() As String
        Get
            Return _adduser
        End Get
        Set(ByVal Value As String)
            _adduser = Value
        End Set
    End Property

    Public Property AddDate() As DateTime
        Get
            Return _adddate
        End Get
        Set(ByVal Value As DateTime)
            _adddate = Value
        End Set
    End Property

    Public Property EditUser() As String
        Get
            Return _edituser
        End Get
        Set(ByVal Value As String)
            _edituser = Value
        End Set
    End Property

    Public Property EditDate() As DateTime
        Get
            Return _editdate
        End Get
        Set(ByVal Value As DateTime)
            _editdate = Value
        End Set
    End Property

#End Region

#Region "Constructors"

    Public Sub New()

    End Sub

    Public Sub New(ByVal sLoadId As String, Optional ByVal bLoadObject As Boolean = True)
        _loadid = sLoadId
        Load(False)
    End Sub

    Public Sub New(ByVal sLoadId As String, ByVal sBillingLoadId As String, Optional ByVal bLoadObject As Boolean = True)
        _billingloadid = sBillingLoadId
        _loadid = sLoadId
        Load()
    End Sub

#End Region

#Region "Private Methods"

    Private Sub Load(Optional ByVal bFullLoad As Boolean = True)
        Dim SQL As String
        If bFullLoad Then
            SQL = "SELECT * FROM BILLINGLOADS WHERE LOADID = '" & _loadid & "' order by lastdate desc" ' AND BILLINGLOADID = '" & _billingloadid & "'"
        Else
            SQL = "SELECT * FROM BILLINGLOADS WHERE LOADID = '" & _loadid & "' order by lastdate desc"
        End If
        Dim dt As New DataTable
        Dim dr As DataRow
        DataInterface.FillDataset(SQL, dt)

        If dt.Rows.Count = 0 Then
            Dim m4nEx As New Made4Net.Shared.M4NException(New Exception, "Billing load transaction is not posted", "Billing load transaction is not posted")
            Throw m4nEx
        End If

        dr = dt.Rows(0)

        If Not dr.IsNull("LOADID") Then _loadid = CType(dr.Item("LOADID"), String)
        If Not dr.IsNull("BILLINGLOADID") Then _billingloadid = CType(dr.Item("BILLINGLOADID"), String)
        '_consignee = DataInterface.ExecuteScalar("SELECT DISTINCT CONSIGNEE FROM invload WHERE LOADID='" & _loadid & "'")
        '_sku = DataInterface.ExecuteScalar("SELECT DISTINCT SKU FROM invload WHERE LOADID='" & _loadid & "'")
        If Not dr.IsNull("CONSIGNEE") Then _consignee = CType(dr.Item("CONSIGNEE"), String)
        If Not dr.IsNull("SKU") Then _sku = CType(dr.Item("SKU"), String)
        If Not dr.IsNull("UOM") Then _uom = CType(dr.Item("UOM"), String)
        If Not dr.IsNull("STARTDATE") Then _startdate = CType(dr.Item("STARTDATE"), Date)
        If Not dr.IsNull("ENDDATE") Then _enddate = CType(dr.Item("ENDDATE"), Date)
        If Not dr.IsNull("LASTDATE") Then _lastdate = CType(dr.Item("LASTDATE"), Date)
        If Not dr.IsNull("CURRENTQTY") Then _currentqty = CType(dr.Item("CURRENTQTY"), Decimal)
        If Not dr.IsNull("ADDDATE") Then _adddate = CType(dr.Item("ADDDATE"), Date)
        If Not dr.IsNull("ADDUSER") Then _adduser = CType(dr.Item("ADDUSER"), String)
        If Not dr.IsNull("EDITDATE") Then _editdate = CType(dr.Item("EDITDATE"), Date)
        If Not dr.IsNull("EDITUSER") Then _edituser = CType(dr.Item("EDITUSER"), String)
    End Sub

#End Region

#Region "Public Methods"

#Region "Post"

    Public Sub Post()
        Dim SQL As String
        If _billingloadid = "" Then
            _billingloadid = GetNextBillingLoadCounter()
        End If
        If Not Exists(_billingloadid) Then
            SQL = String.Format("INSERT INTO BILLINGLOADS(LOADID, CONSIGNEE, SKU, UOM, BILLINGLOADID, STARTDATE, ENDDATE, LASTDATE, CURRENTQTY, ADDDATE, ADDUSER, EDITDATE, EDITUSER) " & _
                    "VALUES({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12})", _
                    Made4Net.Shared.Util.FormatField(_loadid), _
                    Made4Net.Shared.Util.FormatField(_consignee), _
                    Made4Net.Shared.Util.FormatField(_sku), _
                    Made4Net.Shared.Util.FormatField(_uom), _
                    Made4Net.Shared.Util.FormatField(_billingloadid), _
                    Made4Net.Shared.Util.FormatField(_startdate), _
                    Made4Net.Shared.Util.FormatField(_enddate), _
                    Made4Net.Shared.Util.FormatField(_lastdate), _
                    Made4Net.Shared.Util.FormatField(_currentqty), _
                    Made4Net.Shared.Util.FormatField(_adddate), _
                    Made4Net.Shared.Util.FormatField(_adduser), _
                    Made4Net.Shared.Util.FormatField(_editdate), _
                    Made4Net.Shared.Util.FormatField(_edituser))
        Else
            SQL = String.Format("UPDATE BILLINGLOADS SET ENDDATE ={0}, LASTDATE ={1}, CURRENTQTY ={2}, EDITDATE ={3}, EDITUSER ={4} WHERE LOADID ={5} AND BILLINGLOADID ={6}", _
                    Made4Net.Shared.Util.FormatField(_enddate), _
                    Made4Net.Shared.Util.FormatField(_lastdate), _
                    Made4Net.Shared.Util.FormatField(_currentqty), _
                    Made4Net.Shared.Util.FormatField(_editdate), _
                    Made4Net.Shared.Util.FormatField(_edituser), _
                    Made4Net.Shared.Util.FormatField(_loadid), _
                    Made4Net.Shared.Util.FormatField(_billingloadid))
        End If
        DataInterface.RunSQL(SQL)
    End Sub

#End Region

#Region "Transaction writing methods"

    ' Add quantity transaction handler
    Public Sub WriteAddLoadQty(ByVal sLoadId As String, ByVal sConsignee As String, ByVal sSku As String, ByVal sUOM As String, ByVal mQty As Decimal, ByVal dtTransactionDate As DateTime, ByVal pStartDate As DateTime)
        If IsWritingFinished(sLoadId) Then
            Return
        End If
        Dim obl As BillingLoad = New BillingLoad
        If Not Exists(sLoadId) Then     'create new load
            obl.CurrentQty = mQty
            If pStartDate = DateTime.MinValue Then
                obl.StartDate = dtTransactionDate
            Else
                obl.StartDate = pStartDate
            End If
        Else    'add qty to current load
            Dim oBillingLoad As BillingLoad = New BillingLoad(sLoadId)
            obl.CurrentQty = oBillingLoad.CurrentQty + mQty
            obl.StartDate = oBillingLoad.StartDate
        End If
        obl.LoadId = sLoadId
        obl.Consignee = sConsignee
        obl.Sku = sSku
        obl.UOM = sUOM
        obl.BillingLoadId = GetNextBillingLoadCounter()
        obl.LastDate = dtTransactionDate
        obl.AddDate = DateTime.Now
        obl.AddUser = "SYSTEM"
        obl.EditDate = DateTime.Now
        obl.EditUser = "SYSTEM"
        obl.Post()
    End Sub

    'Sub quantity transaction handler
    Public Sub WriteSubLoadQty(ByVal sLoadId As String, ByVal sConsignee As String, ByVal sSku As String, ByVal sUOM As String, ByVal mQty As Decimal, ByVal dtTransactionDate As DateTime, ByVal SetEndDate As Boolean)
        If Not Exists(sLoadId) Then Throw New Exception("Load does not exists in the  billing load table.")
        If IsWritingFinished(sLoadId) Then
            Return
        End If
        Dim oBillingLoad As BillingLoad = New BillingLoad(sLoadId)
        Dim obl As BillingLoad = New BillingLoad
        obl.LoadId = sLoadId
        obl.Consignee = sConsignee
        obl.Sku = sSku
        obl.UOM = sUOM
        obl.BillingLoadId = GetNextBillingLoadCounter()
        obl.CurrentQty = oBillingLoad.CurrentQty - mQty
        obl.StartDate = oBillingLoad.StartDate
        obl.LastDate = dtTransactionDate
        If obl.CurrentQty <= 0 Then
            obl.CurrentQty = 0
            If SetEndDate Then
                obl.EndDate = dtTransactionDate
            End If
        End If
        obl.AddDate = DateTime.Now
        obl.AddUser = "SYSTEM"
        obl.EditDate = DateTime.Now
        obl.EditUser = "SYSTEM"
        obl.Post()
    End Sub

    Private Function IsWritingFinished(ByVal pLoadID As String) As Boolean
        Dim sql As String = String.Format("select count(1) from billingloads where loadid={0} and enddate is not null", Made4Net.Shared.FormatField(pLoadID))
        Dim countEnded As Integer = Integer.Parse(CType(Made4Net.DataAccess.DataInterface.ExecuteScalar(sql), String))
        If countEnded > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

#End Region

#End Region

#Region "Shared Methods"

    Public Shared Function Exists(ByVal sLoadId As String) As Boolean
        Dim SQL As String = String.Format("SELECT COUNT(1) FROM BILLINGLOADS WHERE LOADID ={0}", Made4Net.Shared.Util.FormatField(sLoadId))
        If System.Convert.ToInt32(DataInterface.ExecuteScalar(SQL)) = 0 Then
            Return False
        Else
            Return True
        End If
    End Function

    Public Shared Function GetNextBillingLoadCounter() As String
        Return Made4Net.Shared.Util.getNextCounter("BILLINGLOADID")
    End Function

    '' Used in case of event "UnReceiveLoad"
    Public Shared Sub DeleteByLoadID(ByVal pLoadID As String)
        Dim sql As String = String.Format("DELETE FROM BILLINGLOADS WHERE LOADID={0}", Made4Net.Shared.FormatField(pLoadID))
        Made4Net.DataAccess.DataInterface.RunSQL(sql)
    End Sub

#End Region

End Class