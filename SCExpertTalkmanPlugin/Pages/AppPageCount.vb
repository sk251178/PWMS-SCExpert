Imports WMS.Logic
Imports WMS.Lib

Public Class AppPageCount
    Inherits AppPageProcessor

    Private Enum ResponseCode
        Counted
        [Error]
        NoUserLoggedIn = 99
    End Enum

    Public Sub New()
        MyBase.New()
    End Sub

    Public Overrides Function Process(ByVal oLogger As WMS.Logic.LogHandler) As ClientMessage
        If Not ValidateUserLoggedIn() Then
            If Not oLogger Is Nothing Then
                oLogger.Write(String.Format("User is not logged in. Please sign in again."))
            End If
            Me._responseCode = ResponseCode.NoUserLoggedIn
            Me._responseText = "User is not logged in"
            Me.FillSingleRecord(oLogger)
            Return _resp
        End If
        Dim oLoad As Load
        Dim CountQty As Decimal
        Dim CountUom As String
        Dim userid As String = Made4Net.Shared.Authentication.User.GetCurrentUser.UserName
        Try
            CountQty = _msg(0)("Count Quantity").FieldValue
            CountUom = _msg(0)("Count UOM").FieldValue
            oLoad = New Load(_msg(0)("LoadId").FieldValue)
            If Not oLogger Is Nothing Then
                oLogger.Write("Processing Count Request...")
                oLogger.Write("Load id received: " & oLoad.LOADID)
                oLogger.Write("Received Qty: " & CountQty)
                oLogger.Write("Received UOM: " & CountUom)
            End If
            oLoad.Count(CountQty, CountUom, oLoad.LOCATION, Nothing, userid)
        Catch ex As Made4Net.Shared.M4NException
            If Not oLogger Is Nothing Then
                oLogger.Write("Error Occured while counting: " & ex.Description)
            End If
            Me._responseCode = ResponseCode.Error
            Me._responseText = ex.Description
            Me.FillSingleRecord(oLogger)
            Return _resp
        Catch ex As Exception
            If Not oLogger Is Nothing Then
                oLogger.Write("Error Occured while counting: " & ex.ToString)
            End If
            Me._responseCode = ResponseCode.Error
            Me._responseText = ex.Message
            Me.FillSingleRecord(oLogger)
            Return _resp
        End Try
        If Not oLogger Is Nothing Then
            oLogger.Write("Counting Process completed successfully.")
        End If
        Me._responseCode = ResponseCode.Counted
        Me.FillSingleRecord(oLogger)
        Return _resp
    End Function

End Class
