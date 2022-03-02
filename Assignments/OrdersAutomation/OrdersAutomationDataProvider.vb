Imports Made4Net.DataAccess
Imports Made4Net.Shared
Imports System.Data
Imports WMS.Logic
Namespace AssignmentsGelsons


    <CLSCompliant(False)> Public Class OrdersAutomationDataProvider

#Region "Members"

        Protected _dataStructureName As String
        Protected _templatesDataTable As DataTable
        Protected _logger As LogHandler

#End Region

#Region "Properties"

        Public Property DataStructureName() As String
            Get
                Return _dataStructureName
            End Get
            Set(ByVal value As String)
                _dataStructureName = value
            End Set
        End Property

        Public Property TemplateDataTable() As DataTable
            Get
                Return _templatesDataTable
            End Get
            Set(ByVal value As DataTable)
                _templatesDataTable = value
            End Set
        End Property

        Public Property Logger() As LogHandler
            Get
                Return _logger
            End Get
            Set(ByVal value As LogHandler)
                _logger = value
            End Set
        End Property

#End Region

#Region "Ctors"

        Public Sub New(ByVal pdataStructureName As String, ByVal pTemplatesDataTable As DataTable, ByVal pLogger As LogHandler)
            _dataStructureName = pdataStructureName
            _templatesDataTable = pTemplatesDataTable
            _logger = pLogger
        End Sub

#End Region

#Region "Methods"

        Private Function buildWhereClause() As String
            Dim FinalSQL As String = String.Empty
            Dim strBuilder As New System.Text.StringBuilder()
            For Each oDataCol As DataColumn In _templatesDataTable.Columns ' _DataStructureDataTable.Columns
                If String.Compare(oDataCol.ColumnName.ToLower, "templatename", True) <> 0 Then
                    If Not String.IsNullOrEmpty(_templatesDataTable.Rows(0)(oDataCol.ColumnName).ToString()) Then
                        strBuilder.Append(BuildConditionClause(oDataCol.ColumnName, Made4Net.Shared.Conversion.Convert.ReplaceDBNull(_templatesDataTable.Rows(0)(oDataCol.ColumnName))))
                    End If
                End If
            Next
            Return strBuilder.ToString().TrimEnd("and ".ToCharArray())
        End Function

        Private Function BuildConditionClause(ByVal pPolicyFieldName As String, ByVal pDataFieldValue As Object) As String
            If pDataFieldValue Is Nothing Then pDataFieldValue = String.Empty
            If pPolicyFieldName.StartsWith("from", StringComparison.OrdinalIgnoreCase) Then
                Select Case pDataFieldValue.GetType.FullName
                    Case "System.String", "System.DateTime"
                        Return String.Format("'{0}' <= isnull(replace({1},'','%'),'%') and ", pDataFieldValue, pPolicyFieldName.Substring(4))
                    Case Else
                        Return String.Format("{0} <= isnull(({1}),0) and ", pDataFieldValue, pPolicyFieldName.Substring(4))
                End Select
            ElseIf pPolicyFieldName.StartsWith("to", StringComparison.OrdinalIgnoreCase) Then
                Select Case pDataFieldValue.GetType.FullName
                    Case "System.String", "System.DateTime"
                        Return String.Format("'{0}' >= isnull(replace({1},'','ZZZZZZZZZZZZZZZZ'),'ZZZZZZZZZZZZZZZZ') and ", pDataFieldValue, pPolicyFieldName.Substring(2))
                    Case Else
                        Return String.Format("{0} >= isnull(({1}),'99999999') and ", pDataFieldValue, pPolicyFieldName.Substring(2))
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

        Public Function ProvideData()
            Dim dt As New DataTable
            Dim sql As String = String.Format("Select * from {0} where {1}", _dataStructureName, buildWhereClause())
            If Not _logger Is Nothing Then
                _logger.Write("Sql Query for selecting matching outbound order lines :")
                _logger.Write(sql)
            End If
            DataInterface.FillDataset(sql, dt)
            Return dt
        End Function

#End Region

    End Class
End Namespace