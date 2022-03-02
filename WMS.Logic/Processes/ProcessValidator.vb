Imports System.Data
Imports Made4Net.DataAccess

Public Class ProcessValidator

#Region "Variables"

    Private _processid As String
    Private _processname As String
    Private _validationexpression As String
    Private _vals As Made4Net.DataAccess.Collections.GenericCollection
                        

#End Region

#Region "Properties"
    Public Property FieldValues() As Made4Net.DataAccess.Collections.GenericCollection
        Get
            Return _vals
        End Get
        Set(ByVal value As Made4Net.DataAccess.Collections.GenericCollection)
            _vals = value
        End Set
    End Property
#End Region

#Region "Constructors"

    Public Sub New()

    End Sub

    Public Sub New(ByVal ProcessId As String)
        _processid = ProcessId
        Load()
    End Sub

#End Region

#Region "Methods"

    Private Sub Load()
        Dim sql As String = "SELECT * FROM [PROCESSVALIDATOR] WHERE PROCESSID = '" & _processid & "'"
        Dim dt As DataTable = New DataTable
        DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count = 0 Then
            Throw New ApplicationException("Process not found in [PROCESSVALIDATOR] table")
        End If
        Dim dr As DataRow = dt.Rows(0)

        If Not dr.IsNull("PROCESSNAME") Then _processname = dr.Item("PROCESSNAME")
        If Not dr.IsNull("VALIDATIONEXPRESSION") Then _validationexpression = dr.Item("VALIDATIONEXPRESSION")
    End Sub

    Public Function ValidateBoolean() As Boolean
        Dim ValidationResult As Boolean = True
        Try
            ValidationResult = Convert.ToBoolean(Convert.ToInt32(Validate))
        Catch ex As Exception
            ValidationResult = False
        End Try
        Return ValidationResult
    End Function
    Public Function Validate() As String
        Dim ValidationResult As String
        If _validationexpression <> String.Empty Then
            Dim eval As New Made4Net.Shared.Evaluation.ExpressionEvaluator()
            If Not IsNothing(Me.FieldValues) Then
                eval.FieldValues = Me.FieldValues
            End If
            Try
                ValidationResult = eval.Evaluate(_validationexpression)
            Catch ex As Exception
                ValidationResult = "-1"
            End Try
        End If
        Return ValidationResult
    End Function
#End Region

End Class
