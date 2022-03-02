Imports System.Collections.Generic
Imports System.Linq
Imports Made4Net.Shared
Class UserSession

    Public Property SessionId() As String
    Public Property IpAddress() As String
    Public Property MHEType() As String
    Public Property MHEId() As String

    Sub New(ByVal sessionId As String, ByVal ipAddress As String)
        Me.SessionId = sessionId
        Me.IpAddress = ipAddress
    End Sub

    Sub New(ByVal sessionId As String, ByVal ipAddress As String, ByVal mheType As String, ByVal mheID As String)
        Me.SessionId = sessionId
        Me.IpAddress = ipAddress
        Me.MHEType = mheType
        Me.MHEId = mheID
    End Sub
    Public Overrides Function Equals(obj As Object) As Boolean
        Dim session As UserSession = CType(obj, UserSession)
        If (session Is Nothing) Then
            Return False
        End If
        If ReferenceEquals(Me, obj) Then
            Return True
        End If

        Return Me.SessionId.Equals(session.SessionId) AndAlso Me.IpAddress.Equals(session.IpAddress)

    End Function

    Public Overrides Function GetHashCode() As Integer
        Return SessionId.GetHashCode() Xor IpAddress.GetHashCode()
    End Function
End Class

Public Delegate Function UserHasBeenRemovedDelegate(userId As String) As Boolean


Public Class UserSessionManager

    Shared _sessionManager As UserSessionManager
    Public UserHasBeenRemoved As UserHasBeenRemovedDelegate
    Dim sessions_ As Dictionary(Of String, HashSet(Of UserSession))

    Public Sub New()
        sessions_ = New Dictionary(Of String, HashSet(Of UserSession))(StringComparer.InvariantCultureIgnoreCase)
    End Sub

    Public Sub AddSession(userId As String, sessionId As String, ipAddress As String, mheType As String, mheID As String, logger As ILogHandler)
        If Not sessions_.ContainsKey(userId) Then
            sessions_.Add(userId, New HashSet(Of UserSession)())
        End If
        sessions_(userId).Add(New UserSession(sessionId, ipAddress, mheType, mheID))
        logger.SafeWrite("User '{0}' with session Id '{1}' and IP address '{2}' has been added to the Session manager.", userId, sessionId, ipAddress)
    End Sub

    Public Sub RemoveSession(userId As String, sessionId As String, ipAddress As String, logger As ILogHandler)

        If String.IsNullOrEmpty(userId) Or String.IsNullOrEmpty(sessionId) Or String.IsNullOrEmpty(ipAddress) Then
            Return
        End If

        If Not sessions_.ContainsKey(userId) Then
            logger.SafeWrite("Login is not found for the user '{0}'", userId)
            Return
        End If
        Dim session As UserSession = New UserSession(sessionId, ipAddress)

        If sessions_(userId).Contains(session) Then
            sessions_(userId).Remove(session)
            logger.SafeWrite("Session Id '{0}' and IP address '{1}' has been removed from the Session manager for user {2}.",
                              sessionId, ipAddress, userId)
        End If

        If Not sessions_(userId).Any() Then
            sessions_.Remove(userId)
            logger.SafeWrite("User '{0}' has been removed from Session manager", userId)
            If Not UserHasBeenRemoved Is Nothing Then
                UserHasBeenRemoved(userId)
            End If
        Else
            logger.SafeWrite("Other sessions found for User '{0}'. User cannot be removed from the Session manager.", userId)
        End If

    End Sub

    Public Function GetMHEType(ByVal sessionID As String) As String
        Dim MHEType As String
        Dim userSession As UserSession
        If String.IsNullOrEmpty(sessionID) Then
            Return String.Empty
        End If

        Dim userSessions As HashSet(Of UserSession) = sessions_.Values.Where(Function(x) x.Where(Function(y) y.SessionId.Equals(sessionID)).Count() <> 0).FirstOrDefault()
        userSession = userSessions.Where(Function(x) x.SessionId.Equals(sessionID)).FirstOrDefault()
        If Not userSession Is Nothing Then
            MHEType = userSession.MHEType
        End If

        Return MHEType

    End Function

    Public Function GetMHEId(ByVal sessionID As String) As String
        Dim MHEId As String
        Dim userSession As UserSession
        If String.IsNullOrEmpty(sessionID) Then
            Return String.Empty
        End If

        Dim userSessions As HashSet(Of UserSession) = sessions_.Values.Where(Function(x) x.Where(Function(y) y.SessionId.Equals(sessionID)).Count() <> 0).FirstOrDefault()
        userSession = userSessions.Where(Function(x) x.SessionId.Equals(sessionID)).FirstOrDefault()
        If Not userSession Is Nothing Then
            MHEId = userSession.MHEId
        End If

        Return MHEId

    End Function


End Class
