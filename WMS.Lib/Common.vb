Imports System.Web

Public Class Common

    Public Shared Function GetCurrentUser() As String
        If IsLocalhost() Then
            Return "Admin"
        Else
            Return HttpContext.Current.Session("user")
        End If
    End Function

    Public Shared Function GetCurrentUserDisplayName() As String
        Dim fullname As String = Nothing
        Dim curuser As String = GetCurrentUser()
        Try
            curuser = GetCurrentUser()
            fullname = Made4Net.DataAccess.DataInterface.ExecuteScalar(String.Format("Select fullname from {0} where UserID = '{1}')", TABLES.USERPROFILE, curuser))
        Catch ex As Exception

        End Try
        If fullname Is Nothing OrElse fullname.Length = 0 Then
            Return curuser
        Else
            Return fullname
        End If
    End Function

    Public Shared Function IsLocalhost() As Boolean
        If HttpContext.Current.Request.Url.Host = "localhost" Then
            Return True
        End If
        Return False
    End Function

End Class


