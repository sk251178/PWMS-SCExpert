Imports WMS.Logic
Imports WMS.Lib
Imports Made4Net.Shared

Public Class AppPageVoiceList
    Inherits AppPageProcessor

    Private Enum ResponseCode
        OK
        UnknownError
    End Enum

    Public Sub New()
        MyBase.New()
    End Sub

    Public Overrides Function Process(ByVal oLogger As WMS.Logic.LogHandler) As ClientMessage
        PrintMessageContent(oLogger)
        _messageDate = Now.ToString("yyyy-MM-dd HH:mm:ss")

        If Not oLogger Is Nothing Then
            oLogger.Write("Processing voice list request...")
        End If

        Try
            _responseCode = ResponseCode.OK
            _responseText = ""
            Me.FillRecordsFromView("", oLogger)
            AddPrinters(oLogger)
        Catch ex As Made4Net.Shared.M4NException
            _responseCode = ResponseCode.UnknownError
            _responseText = ex.Description
            If Not oLogger Is Nothing Then
                oLogger.Write("Error Occured: " & ex.GetErrMessage(0))
            End If
            Me.FillSingleRecord(oLogger)
        Catch ex As ApplicationException
            _responseCode = ResponseCode.UnknownError
            _responseText = ex.Message
            If Not oLogger Is Nothing Then
                oLogger.Write("Error Occured: " & ex.ToString)
            End If
            Me.FillSingleRecord(oLogger)
        Catch ex As Exception
            _responseCode = ResponseCode.UnknownError
            _responseText = ex.Message
            If Not oLogger Is Nothing Then
                oLogger.Write("Error Occured: " & ex.ToString)
            End If
            Me.FillSingleRecord(oLogger)
        End Try
        Return _resp
    End Function

    Private Sub AddPrinters(ByVal oLogger As WMS.Logic.LogHandler)
        Dim sSql As String = String.Format("select 'REPORTPRINTERS' as ListCode,PRINTQNAME as ListValue, PHYSICALPATH as ListDescription  from REPORTPRINTERS union select 'LABELPRINTERS' as ListCode,PRINTQNAME as ListValue, PHYSICALPATH as ListDescription  from LABELPRINTERS")
        Dim oDt As New DataTable
        Made4Net.DataAccess.DataInterface.FillDataset(sSql, oDt, False)
        Me.AddAdditionalRecordsToResponse(oDt, oLogger)

    End Sub

End Class
