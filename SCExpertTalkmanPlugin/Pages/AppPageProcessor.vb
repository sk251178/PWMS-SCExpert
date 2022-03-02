Imports System.Text
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
        If Not IsNothing(MessagesInfo.GetMessageConfig(_msg.Name, Nothing).ResponseMessage) AndAlso MessagesInfo.GetMessageConfig(_msg.Name, Nothing).ResponseMessage <> String.Empty Then
            _resp = MessagesInfo.GetMessageConfig(MessagesInfo.GetMessageConfig(_msg.Name, Nothing).ResponseMessage, Nothing).CreateMessage
        End If
    End Sub

    Public Overridable Function Process(ByVal oLogger As WMS.Logic.LogHandler) As ClientMessage
        Return (Nothing)
    End Function


    Protected Sub FillSingleRecord(ByVal oLogger As WMS.Logic.LogHandler, Optional ByVal pAddDeviceInfoToResponse As Boolean = True)
        Dim _resprecord As MessageRecord = _resp.CreateRecord(oLogger)
        If Not oLogger Is Nothing Then oLogger.Write(String.Format("Response message row data:"))
        If pAddDeviceInfoToResponse Then
            _resprecord("Device ID").FieldValue = _msg(0)("Device ID").FieldValue
            _resprecord("Message Date").FieldValue = _messageDate
            If Not oLogger Is Nothing Then
                oLogger.Write(String.Format("Device Id: {0}", _resprecord("Device ID").FieldValue))
                oLogger.Write(String.Format("Message Date: {0}", _resprecord("Message Date").FieldValue))
            End If
        End If
        _resprecord("Response Code").FieldValue = _responseCode
        _resprecord("Response Text").FieldValue = _responseText
        If Not oLogger Is Nothing Then
            oLogger.Write(String.Format("Response Code: {0}", _resprecord("Response Code").FieldValue))
            oLogger.Write(String.Format("Response Text: {0}", _resprecord("Response Text").FieldValue))
        End If
        _resp.AddRecord(_resprecord)
    End Sub

    Protected Sub FillRecordsFromView(ByVal Where As String, ByVal oLogger As WMS.Logic.LogHandler, Optional ByVal pAddDeviceInfoToResponse As Boolean = True)
        Dim odt As Data.DataTable = GetDT(Where, oLogger)
        If Not oLogger Is Nothing Then oLogger.Write(String.Format("Response message row data:"))
        For Each odr As Data.DataRow In odt.Rows
            Dim Record As MessageRecord = _resp.CreateRecord(oLogger)
            If pAddDeviceInfoToResponse Then
                Record("Device ID").FieldValue = _msg(0)("Device ID").FieldValue
                Record("Message Date").FieldValue = _messageDate
                If Not oLogger Is Nothing Then
                    oLogger.Write(String.Format("Device Id: {0}", Record("Device ID").FieldValue))
                    oLogger.Write(String.Format("Message Date: {0}", Record("Message Date").FieldValue))
                End If
            End If
            Record("Response Code").FieldValue = _responseCode
            Record("Response Text").FieldValue = _responseText
            If Not oLogger Is Nothing Then
                oLogger.Write(String.Format("Response Code: {0}", Record("Response Code").FieldValue))
                oLogger.Write(String.Format("Response Text: {0}", Record("Response Text").FieldValue))
            End If
            FillRecordFromDR(Record, odr, oLogger)
            _resp.AddRecord(Record)
        Next
    End Sub

    Protected Function GetDT(ByVal Where As String, ByVal oLogger As WMS.Logic.LogHandler) As Data.DataTable
        Dim oTable As String = MessagesInfo.GetMessageConfig(_resp.Name, oLogger).ViewName
        Dim oDT As New Data.DataTable
        Dim oSQL As String
        If Where Is Nothing OrElse Where = String.Empty Then
            oSQL = String.Format("select * from {0}", oTable)
        Else
            oSQL = String.Format("select * from {0} where {1}", oTable, Where)
        End If
        If Not oLogger Is Nothing Then
            oLogger.Write("Trying to load data for current action...")
            oLogger.Write(String.Format("SQL for retrieving data: {0}", oSQL))
        End If
        Made4Net.DataAccess.DataInterface.FillDataset(oSQL, oDT, False, MessagesInfo.GetMessageConfig(_resp.Name, oLogger).Connection)
        If Not oLogger Is Nothing Then
            oLogger.Write(String.Format("Data Extracted and contains {0} Rows...", oDT.Rows.Count))
        End If
        Return oDT
    End Function

    Protected Sub FillRecordFromDR(ByVal Record As MessageRecord, ByVal oDR As Data.DataRow, ByVal oLogger As WMS.Logic.LogHandler)
        Dim _dynfields As Collection = MessagesInfo.GetMessageConfig(_resp.Name, oLogger).DynamicFields
        If Not oLogger Is Nothing Then
            oLogger.Write(String.Format("Trying to add additional records to the response message..."))
        End If
        Dim oField As MessagesInfo.FieldInfo = Nothing
        For idx As Int32 = 1 To _dynfields.Count
            Try
                oField = _dynfields(idx)
                Record(oField.Name).FieldValue = Convert.ToString(oDR(oField.Name))
                If Not oLogger Is Nothing Then
                    oLogger.Write(String.Format("Value: {0} set to Field: {1}", Convert.ToString(oDR(oField.Name)), oField.Name))
                End If
            Catch ex As Exception
                If Not oLogger Is Nothing Then
                    If Not oField Is Nothing Then
                        oLogger.Write(String.Format("Exception occured while setting the value for field: {0}", oField.Name))
                        oLogger.Write(ex.ToString())
                    Else
                        oLogger.Write(String.Format("Exception occured while setting the value for field number: {0}", (idx + 1).ToString))
                        oLogger.Write(ex.ToString())
                    End If
                Else
                    Dim msg As String
                    If Not oField Is Nothing Then
                        msg = String.Format("Exception occured while setting the value for field: {0}", oField.Name)
                        Throw New ApplicationException(msg)
                    Else
                        msg = String.Format("Exception occured while setting the value for field number: {0}", (idx + 1).ToString)
                        Throw New ApplicationException(msg)
                    End If
                End If
            End Try
        Next
    End Sub

    Protected Sub AddAdditionalRecordsToResponse(ByVal oDataTable As DataTable, ByVal oLogger As WMS.Logic.LogHandler, Optional ByVal pAddDeviceInfoToResponse As Boolean = True)
        If Not oLogger Is Nothing Then oLogger.Write(String.Format("Response message row data:"))
        For Each odr As Data.DataRow In oDataTable.Rows
            Dim Record As MessageRecord = _resp.CreateRecord(oLogger)
            If pAddDeviceInfoToResponse Then
                Record("Device ID").FieldValue = _msg(0)("Device ID").FieldValue
                Record("Message Date").FieldValue = _messageDate
                If Not oLogger Is Nothing Then
                    oLogger.Write(String.Format("Device Id: {0}", Record("Device ID").FieldValue))
                    oLogger.Write(String.Format("Message Date: {0}", Record("Message Date").FieldValue))
                End If
            End If
            Record("Response Code").FieldValue = _responseCode
            Record("Response Text").FieldValue = _responseText
            If Not oLogger Is Nothing Then
                oLogger.Write(String.Format("Response Code: {0}", Record("Response Code").FieldValue))
                oLogger.Write(String.Format("Response Text: {0}", Record("Response Text").FieldValue))
            End If
            FillRecordFromDR(Record, odr, oLogger)
            _resp.AddRecord(Record)
        Next
    End Sub

    Protected Sub PrintMessageContent(ByVal oLogger As WMS.Logic.LogHandler)
        If Not oLogger Is Nothing Then
            oLogger.Write(String.Format("{0}", "*".PadLeft(80, "*")))
            oLogger.Write(String.Format("Message {0} received by the talkman server. Message content:", _msg(0).Name))
            oLogger.Write(String.Format("{0}", "-".PadLeft(70, "-")))
            '' Dim CharsToRemove As String = MessagesInfo.EOF
            Dim sb As New StringBuilder()

            For index As Integer = 0 To _msg.ReceivedMessage.Length - 1
                If Not Char.IsWhiteSpace(_msg.ReceivedMessage(index)) Then
                    sb.Append(_msg.ReceivedMessage(index))
                End If
            Next

            oLogger.Write("received message's data : " & (sb.ToString()))

            For Each oField As RecordField In _msg(0).MessageFields
                oLogger.Write(String.Format("{0}: {1}", oField.Name, _msg(0)(oField.Name).FieldValue))
            Next
            oLogger.Write(String.Format("{0}", "-".PadLeft(70, "-")))
            oLogger.Write(String.Format("{0}", "*".PadLeft(80, "*")))
            oLogger.Write(String.Format("{0}", " ".PadLeft(80, " ")))
        End If
    End Sub

    Protected Sub PrintResponseMessageContent(ByVal oLogger As WMS.Logic.LogHandler)
        If Not oLogger Is Nothing Then
            oLogger.Write(String.Format("{0}", " ".PadLeft(80, " ")))
            oLogger.Write(String.Format("{0}", "*".PadLeft(80, "*")))
            oLogger.Write(String.Format("Message {0} was built according to {1}. Message content:", _resp.Name, MessagesInfo.GetMessageConfig(_resp.Name, oLogger).ViewName))
            oLogger.Write(String.Format("{0}", "-".PadLeft(80, "-")))
            For Each oRecord As MessageRecord In _resp.RecordsList
                For Each oField As RecordField In oRecord.MessageFields
                    oLogger.Write(String.Format("Field {0} value: {1}", oField.Name, oRecord(oField.Name).FieldValue))
                Next
            Next
            oLogger.Write(String.Format("{0}", "*".PadLeft(80, "*")))
            oLogger.Write(String.Format("{0}", " ".PadLeft(80, " ")))
        End If
    End Sub

    Protected Function ValidateUserLoggedIn() As Boolean
        If WMS.Logic.Common.GetCurrentUser Is Nothing OrElse WMS.Logic.Common.GetCurrentUser = String.Empty Then
            Return False
        End If
        Return True
    End Function

End Class