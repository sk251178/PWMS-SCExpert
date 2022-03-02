Imports System
Imports System.Collections.Generic
Imports System.Text

<AttributeUsage(AttributeTargets.Class)> _
Public Class DBTableAttribute
    Inherits Attribute

#Region "Private data"

    Private _DBTableName As String

#End Region

#Region "Constructors"

    Public Sub New(ByVal strDBTableName As String)
        _DBTableName = strDBTableName
    End Sub

#End Region

#Region "Properties"

    Public ReadOnly Property DBTableName() As String
        Get
            Return _DBTableName
        End Get
    End Property

#End Region

End Class
