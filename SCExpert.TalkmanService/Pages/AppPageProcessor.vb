Public MustInherit Class AppPageProcessor
    Inherits Made4Net.AppSessionManagement.AppClient

    Protected _msg As ClientMessage
    Protected _resp As ClientMessage
    Protected _responseCode As String
    Protected _responseText As String
    Protected _messageDate As String = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")

    Public Sub New()
        MyBase.New()
        _msg = CType(Request, AppRequest).GetClientMessage
        If Not IsNothing(MessagesInfo.GetMessageConfig(_msg.Name).ResponseMessage) AndAlso MessagesInfo.GetMessageConfig(_msg.Name).ResponseMessage <> String.Empty Then
            _resp = MessagesInfo.GetMessageConfig(MessagesInfo.GetMessageConfig(_msg.Name).ResponseMessage).CreateMessage
        End If
    End Sub

    Public Overridable Function Process(ByVal oLogger As WMS.Logic.LogHandler) As ClientMessage
        Return (Nothing)
    End Function

    Protected Sub FillSingleRecord(ByVal oLogger As WMS.Logic.LogHandler)
        Dim _resprecord As MessageRecord = _resp.CreateRecord
        _resprecord("Device ID").FieldValue = _msg(0)("Device ID").FieldValue
        _resprecord("Response Code").FieldValue = _responseCode
        _resprecord("Response Text").FieldValue = _responseText
        _resprecord("Message Date").FieldValue = _messageDate
        If Not oLogger Is Nothing Then
            oLogger.Write(String.Format("Resonse message row data:"))
            oLogger.Write(String.Format("Device Id: {0}", _resprecord("Device ID").FieldValue))
            oLogger.Write(String.Format("Response Code: {0}", _resprecord("Response Code").FieldValue))
            oLogger.Write(String.Format("Response Text: {0}", _resprecord("Response Text").FieldValue))
            oLogger.Write(String.Format("Message Date: {0}", _resprecord("Message Date").FieldValue))
        End If
        _resp.AddRecord(_resprecord)
    End Sub

    Protected Sub FillRecordsFromView(ByVal Where As String, ByVal oLogger As WMS.Logic.LogHandler)
        Dim odt As Data.DataTable = GetDT(Where, oLogger)
        For Each odr As Data.DataRow In odt.Rows
            Dim Record As MessageRecord = _resp.CreateRecord
            Record("Device ID").FieldValue = _msg(0)("Device ID").FieldValue
            Record("Response Code").FieldValue = _responseCode
            Record("Message Date").FieldValue = _messageDate
            If Not oLogger Is Nothing Then
                oLogger.Write(String.Format("Response message row data:"))
                oLogger.Write(String.Format("Device Id: {0}", Record("Device ID").FieldValue))
                oLogger.Write(String.Format("Response Code: {0}", Record("Response Code").FieldValue))
                oLogger.Write(String.Format("Message Date: {0}", Record("Message Date").FieldValue))
            End If
            FillRecordFromDR(Record, odr, oLogger)
            _resp.AddRecord(Record)
        Next
    End Sub

    Protected Function GetDT(ByVal Where As String, ByVal oLogger As WMS.Logic.LogHandler) As Data.DataTable
        Dim oTable As String = MessagesInfo.GetMessageConfig(_resp.Name).ViewName
        Dim oDT As New Data.DataTable
        Dim oSQL As String = String.Format("select * from {0} where {1}", oTable, Where)
        If Not oLogger Is Nothing Then
            oLogger.Write("Trying to load data for current action...")
            oLogger.Write(String.Format("SQL for retrieving data: {0}", oSQL))
        End If
        Made4Net.DataAccess.DataInterface.FillDataset(oSQL, oDT, False, MessagesInfo.GetMessageConfig(_resp.Name).Connection)
        If Not oLogger Is Nothing Then
            oLogger.Write(String.Format("Data Extracted and contains {0} Rows...", oDT.Rows.Count))
        End If
        Return oDT
    End Function

    Protected Sub FillRecordFromDR(ByVal Record As MessageRecord, ByVal oDR As Data.DataRow, ByVal oLogger As WMS.Logic.LogHandler)
        Dim _dynfields As Collection = MessagesInfo.GetMessageConfig(_resp.Name).DynamicFields
        If Not oLogger Is Nothing Then
            oLogger.Write(String.Format("Trying to fill the response record according to data retrieved..."))
        End If
        For Each oField As MessagesInfo.FieldInfo In _dynfields
            Try
                Record(oField.Name).FieldValue = Convert.ToString(oDR(oField.Name))
                If Not oLogger Is Nothing Then
                    oLogger.Write(String.Format("Value: {0} set to Field: {1}", Convert.ToString(oDR(oField.Name)), oField.Name))
                End If
            Catch ex As Exception
                If Not oLogger Is Nothing Then
                    oLogger.Write(String.Format("Exception occured while setting the value for field: {0}", oField.Name))
                    oLogger.Write(ex.ToString())
                End If
            End Try
        Next
    End Sub

End Class