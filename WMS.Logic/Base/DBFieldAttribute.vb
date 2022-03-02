Imports System
Imports System.Collections.Generic
Imports System.Text

<AttributeUsage(AttributeTargets.Property)> _
Public Class DBFieldAttribute
    Inherits Attribute

#Region "Private data"

    Private _DBFieldName As String
    Private _PrimaryKey As Boolean

#End Region

#Region "Constaructors"

    Public Sub New(ByVal strDBFieldName As String, ByVal bPrimaryKey As Boolean)
        _DBFieldName = strDBFieldName
        _PrimaryKey = bPrimaryKey
    End Sub

#End Region

#Region "Properties"

    Public ReadOnly Property DBFieldName() As String
        Get
            Return _DBFieldName
        End Get
    End Property

    Public ReadOnly Property PrimaryKey() As Boolean
        Get
            Return _PrimaryKey
        End Get
    End Property

#End Region

End Class
