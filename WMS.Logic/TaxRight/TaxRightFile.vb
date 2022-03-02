Imports System.Collections.Generic
Imports Made4Net.DataAccess
Imports Made4Net.Shared
Imports System.Collections.Specialized
Imports System.Collections.ObjectModel

Public Class TaxRightFile
    Inherits Made4Net.Shared.FixedWidthFile

    Private Const DOCUMENTKey As String = "DOCUMENT"
    Private ReadOnly header As TaxRightHeader

    Public Sub New(ByVal valueCollection As NameValueCollection)
        If valueCollection Is Nothing OrElse valueCollection(DOCUMENTKey) Is Nothing Then
            Throw New ArgumentException($"Invalid message content. {NameOf(valueCollection)} is NULL, or does not contain a value for 'DOCUMENT'")
        End If

        header = New TaxRightHeader(valueCollection(DOCUMENTKey))
    End Sub

    Public Overrides ReadOnly Iterator Property FixedWidthLines As IEnumerable(Of FixedWidthLine)
        Get
            For Each line As FixedWidthLine In header.FixedWidthLines
                Yield line
            Next
        End Get
    End Property

    Public Overrides ReadOnly Property SignificantFields As ReadOnlyDictionary(Of String, String)
        Get
            Return header.SignificantFields
        End Get
    End Property
End Class