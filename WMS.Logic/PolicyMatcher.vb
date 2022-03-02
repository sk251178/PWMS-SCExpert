Imports made4net.shared
Imports made4net.DataAccess
Imports made4net.General
Imports System.Data

Public Class PolicyMatcher

#Region "Members"

    'The name of the structures for the policy and data to match
    Protected mPoliciesStructureName As String
    'The datatables to hold the data
    Protected mDataStructureDataTable As DataTable
    Protected mPoliciesStructureDataTable As DataTable

#End Region

#Region "Properties"

    Public Property PoliciesStructureName() As String
        Get
            Return mPoliciesStructureName
        End Get
        Set(ByVal value As String)
            mPoliciesStructureName = value
        End Set
    End Property

    Public Property DataStructureTable() As DataTable
        Set(ByVal value As DataTable)
            mDataStructureDataTable = value
        End Set
        Get
            Return mDataStructureDataTable
        End Get
    End Property

    Public ReadOnly Property PoliciesStructureTable() As DataTable
        Get
            Return mPoliciesStructureDataTable
        End Get
    End Property

#End Region

#Region "Ctors"

    Public Sub New()
        mDataStructureDataTable = New DataTable()
        mPoliciesStructureDataTable = New DataTable()
    End Sub

    Public Sub New(ByVal pPoliciesStructureName As String)
        Me.New(pPoliciesStructureName, Nothing)
    End Sub

    Public Sub New(ByVal pPoliciesStructureName As String, ByVal pDataStructureDataTable As DataTable)
        If pDataStructureDataTable Is Nothing Then
            mDataStructureDataTable = New DataTable()
        Else
            mDataStructureDataTable = pDataStructureDataTable
        End If
        mPoliciesStructureName = pPoliciesStructureName
        mPoliciesStructureDataTable = New DataTable()
        'Load mPoliciesStructureDataTable to hold the correct structure
        DataInterface.FillDataset(String.Format("select * from {0}", pPoliciesStructureName), mPoliciesStructureDataTable)
    End Sub

#End Region

#Region "Methods"

    Public Function FindMatchingPolicies(ByVal pDrData As DataRow) As DataTable
        'Build a new datatable to hold the data row
        mDataStructureDataTable.Rows.Clear()
        If mDataStructureDataTable.Columns.Count = 0 Then
            For Each oCol As DataColumn In pDrData.Table.Columns
                mDataStructureDataTable.Columns.Add(oCol.ColumnName, oCol.DataType)
            Next
        End If
        Dim ClonedRow As DataRow = mDataStructureDataTable.NewRow()
        For Each oCol As DataColumn In pDrData.Table.Columns
            ClonedRow(oCol.ColumnName) = pDrData(oCol.ColumnName)
        Next
        mDataStructureDataTable.Rows.Add(ClonedRow)
        'and return the policies table
        Return FindMatchingPolicies()
    End Function

    Public Function FindMatchingPolicies() As DataTable
        Dim FinalSQL As String = String.Empty
        Dim strBuilder As New System.Text.StringBuilder()
        'Added for RWMS-1606 and RWMS-1525 - checking the Rows.Count
        Dim drData As DataRow
        If mDataStructureDataTable.Rows.Count = 0 Then
            Return Nothing
        Else
            drData = mDataStructureDataTable.Rows(0)
        End If
        'End RWMS-1525
        'Commented for RWMS-1525
        'Dim drData As DataRow = mDataStructureDataTable.Rows(0)
        'End Commented for RWMS-1606 and RWMS-1525

        If drData Is Nothing Then
            Return Nothing
        End If
        For Each oDataCol As DataColumn In mDataStructureDataTable.Columns
            For Each oPolicyCol As DataColumn In mPoliciesStructureDataTable.Columns
                Dim oPolicyColName As String = oPolicyCol.ColumnName
                If String.Compare(oPolicyColName, "policyid", True) <> 0 AndAlso String.Compare(oPolicyColName, "priority", True) <> 0 Then
                    If String.Compare(oPolicyColName, oDataCol.ColumnName, True) = 0 Then
                        strBuilder.Append(BuildConditionClause(oPolicyColName, Made4Net.Shared.Conversion.Convert.ReplaceDBNull(drData(oDataCol.ColumnName))))
                    Else
                        If String.Compare(oPolicyColName, "from" & oDataCol.ColumnName, True) = 0 Then
                            strBuilder.Append(BuildConditionClause(oPolicyColName, Made4Net.Shared.Conversion.Convert.ReplaceDBNull(drData(oDataCol.ColumnName))))
                        End If
                        If String.Compare(oPolicyColName, "to" & oDataCol.ColumnName, True) = 0 Then
                            strBuilder.Append(BuildConditionClause(oPolicyColName, Made4Net.Shared.Conversion.Convert.ReplaceDBNull(drData(oDataCol.ColumnName))))
                        End If
                    End If
                End If
            Next
        Next
        FinalSQL = String.Format("select * from {0} where {1} order by priority", mPoliciesStructureName, strBuilder.ToString().TrimEnd("and ".ToCharArray()))
        Dim FilteredPolicies As New DataTable
        DataInterface.FillDataset(FinalSQL, FilteredPolicies)
        Return FilteredPolicies
    End Function

    Private Function BuildConditionClause(ByVal pPolicyFieldName As String, ByVal pDataFieldValue As Object) As String
        If pDataFieldValue Is Nothing Then pDataFieldValue = String.Empty
        If pPolicyFieldName.StartsWith("from", StringComparison.OrdinalIgnoreCase) Then
            Select Case pDataFieldValue.GetType.FullName
                Case "System.String", "System.DateTime"
                    'Return String.Format("'{0}' >= isnull(replace({1},'','%'),'%') and ", pDataFieldValue, pPolicyFieldName)
                    Return String.Format("'{0}' >= isnull(replace({1},'',''),'') and ", pDataFieldValue, pPolicyFieldName)
                Case Else
                    Return String.Format("{0} >= isnull(({1}),0) and ", pDataFieldValue, pPolicyFieldName)
            End Select
        ElseIf pPolicyFieldName.StartsWith("to", StringComparison.OrdinalIgnoreCase) Then
            Select Case pDataFieldValue.GetType.FullName
                Case "System.String", "System.DateTime"
                    Return String.Format("'{0}' <= isnull(replace({1},'','ZZZZZZZZZZZZZZZZ'),'ZZZZZZZZZZZZZZZZ') and ", pDataFieldValue, pPolicyFieldName)
                Case Else
                    Return String.Format("{0} <= isnull(({1}),'99999999') and ", pDataFieldValue, pPolicyFieldName)
            End Select
        Else
            Select Case pDataFieldValue.GetType.FullName
                Case "System.String", "System.DateTime"
                    Return String.Format("'{0}' like isnull(replace({1},'','%'),'%') and ", pDataFieldValue, pPolicyFieldName)
                Case Else
                    Return String.Format("'{0}' like isnull(replace({1},'','%'),'%') and ", pDataFieldValue, pPolicyFieldName)
            End Select
        End If
    End Function

#End Region

End Class