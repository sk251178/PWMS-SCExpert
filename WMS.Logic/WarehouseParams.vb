Public Class WarehouseParams

    Private Shared _dictionaryParams As System.Collections.Generic.Dictionary(Of String, String)

    Public Shared Function GetWarehouseParam(ByVal pParamName As String) As String
        Try
            Return DictionaryParams()(pParamName)
        Catch ex As Exception
            Return ""
        End Try


    End Function

    Private Shared Function DictionaryParams() As System.Collections.Generic.Dictionary(Of String, String)
        If Not _dictionaryParams Is Nothing Then
            Return _dictionaryParams
        End If

        load()

        Return _dictionaryParams

    End Function

    Public Shared Function Reload()
        load()
    End Function

    Private Shared Sub load()
        _dictionaryParams = New System.Collections.Generic.Dictionary(Of String, String)

        Dim sql As String = String.Format("Select ParamName,ParamValue from warehouseparams")
        Dim dt As New DataTable
        Made4Net.DataAccess.DataInterface.FillDataset(sql, dt)

        For Each dr As DataRow In dt.Rows
            _dictionaryParams.Add(dr("ParamName"), dr("ParamValue"))
        Next
    End Sub

    Public Shared Sub Edit(ByVal pParamName As String, ByVal pParamDesc As String, ByVal pParamValue As String, ByVal pUser As String)
        If Not Exists(pParamName) Then
            Throw New Made4Net.Shared.M4NException(New Exception(), "Parameter does not exist", "Parameter does not exist")
        End If
        Dim sql As String
        If pParamDesc Is Nothing Then
            sql = String.Format("update warehouseparams set paramvalue={0},editdate={1},edituser={2} where paramname={3}", _
                Made4Net.Shared.FormatField(pParamValue), Made4Net.Shared.FormatField(DateTime.Now), Made4Net.Shared.FormatField(pUser), _
                Made4Net.Shared.FormatField(pParamName))
        Else
            sql = String.Format("update warehouseparams set paramdesc={0}, paramvalue={1},editdate={2},edituser={3} where paramname={4}", _
                    Made4Net.Shared.FormatField(pParamDesc), Made4Net.Shared.FormatField(pParamValue), Made4Net.Shared.FormatField(DateTime.Now), Made4Net.Shared.FormatField(pUser), _
                    Made4Net.Shared.FormatField(pParamName))
        End If
        Made4Net.DataAccess.DataInterface.RunSQL(sql)
    End Sub

    Public Shared Function Exists(ByVal pParamName As String) As Boolean
        Dim sql As String = String.Format("select count(1) from warehouseparams where paramname={0}", Made4Net.Shared.FormatField(pParamName))
        Return Convert.ToBoolean(Made4Net.DataAccess.DataInterface.ExecuteScalar(sql))
    End Function

    Public Shared Sub Insert(ByVal pParamName As String, ByVal pParamDesc As String, ByVal pParamValue As String, ByVal pUser As String)
        If Exists(pParamName) Then
            Throw New Made4Net.Shared.M4NException(New Exception(), "Parameter already exists", "Parameter already exists")
        End If
        Dim now As DateTime = DateTime.Now
        Dim sql As String = String.Format("insert into warehouseparams (paramname, paramdesc, paramvalue, adddate, adduser, editdate, edituser) values ({0},{1},{2},{3},{4},{3},{4})", _
        Made4Net.Shared.FormatField(pParamName), Made4Net.Shared.FormatField(pParamDesc), Made4Net.Shared.FormatField(pParamValue), _
        Made4Net.Shared.FormatField(now), Made4Net.Shared.FormatField(pUser))
        Made4Net.DataAccess.DataInterface.RunSQL(sql)
    End Sub



End Class
