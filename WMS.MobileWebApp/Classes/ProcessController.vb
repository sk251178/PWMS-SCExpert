
Imports System.Collections

Public NotInheritable Class ProcessController
    Private Sub New()
    End Sub


    Private Shared _results As New Hashtable()


    Public Shared Function GetResult(ByVal id As Guid) As String

        If _results.Contains(id) Then

            Return Convert.ToString(_results(id))
        Else

            Return [String].Empty
        End If

    End Function

    Public Shared Sub Add(ByVal id As Guid, ByVal value As String)

        _results(id) = value

    End Sub



    Public Shared Sub Remove(ByVal id As Guid)

        _results.Remove(id)

    End Sub

End Class
