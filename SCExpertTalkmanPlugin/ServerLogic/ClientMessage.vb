Imports System.Text

Public Class ClientMessage

    Private _records As New ArrayList
    Private _messagename As String
    Private _deviceid As String
    Private _receivedMessage As String

    Public Sub New(ByVal MessageName As String)
        _messagename = MessageName
    End Sub

    Public ReadOnly Property Name() As String
        Get
            Return _messagename
        End Get
    End Property

    Public ReadOnly Property DeviceID() As String
        Get
            Return _deviceid
        End Get
    End Property
    ''' <summary>
    ''' the received message before manipulating
    ''' </summary>
    Public ReadOnly Property ReceivedMessage() As String
        Get
            Return _receivedMessage
        End Get
    End Property

    Public ReadOnly Property Count() As Integer
        Get
            Return _records.Count
        End Get
    End Property

    Public ReadOnly Property RecordsList() As ArrayList
        Get
            Return _records
        End Get
    End Property


    Default Public ReadOnly Property Record(ByVal Index As Int32) As MessageRecord
        Get
            Return _records(Index)
        End Get
    End Property

    Public Function CreateRecord(ByVal oLogger As WMS.Logic.LogHandler) As MessageRecord
        Dim newRec As MessageRecord
        If MessagesInfo.Format.Equals(MessagesInfo.RecordFormat.Delimited) Then
            newRec = New MessageRecordDelimited(Me._messagename, oLogger)
        Else
            newRec = New MessageRecordFixed(Me._messagename)
        End If
        Return newRec
    End Function

    Public Sub AddRecord(ByVal NewRecord As MessageRecord)
        _records.Add(NewRecord)

    End Sub

    Public Sub RemoveRecord(ByVal pIndex As Integer)
        _records.RemoveAt(pIndex)
    End Sub

    Public Sub Parse(ByVal MessageData As String)
        _receivedMessage = MessageData
        Dim memio As New System.IO.MemoryStream
        Dim bytes() As Byte = System.Text.UTF8Encoding.UTF8.GetBytes(MessageData)
        memio.Write(bytes, 0, bytes.Length)
        memio.Seek(0, IO.SeekOrigin.Begin)
        Parse(memio)
    End Sub

    Public Sub Parse(ByVal MessageData As System.IO.Stream)
        Dim reader As New System.IO.StreamReader(MessageData)
        Dim Line As String
        Dim firstline As Boolean = True
        Line = reader.ReadLine
        While Not IsNothing(Line)

            If Not Trim(Line).Equals("") Then
                Dim record As MessageRecord = Me.CreateRecord(Nothing)
                record.Parse(Line)
                Me.AddRecord(record)
                If firstline Then
                    firstline = False
                    Me._messagename = record.Name
                    Me._deviceid = record.Item("Device ID").FieldValue
                End If
            End If
            Line = reader.ReadLine
        End While
    End Sub

    Public Function Export() As String
        Dim exportstring As String = ""
        For i As Int32 = 0 To _records.Count - 1
            exportstring = exportstring & CType(_records(i), MessageRecord).Export & vbCrLf
        Next
        Return exportstring & MessagesInfo.EOF.Replace("CR", vbCr).Replace("LF", vbLf)
    End Function
End Class

Public Class MessageRecord

    Protected _fields As New Collection
    Protected _fieldsConfig As Collection
    Protected _dynfieldsConfig As Collection
    Protected _messageName As String

    Public Sub New(ByVal MessageName As String, ByVal oLogger As WMS.Logic.LogHandler)
        '_messageName = MessageName
        '_fieldsConfig = MessagesInfo.GetMessageConfig(MessageName).Fields
        '_dynfieldsConfig = MessagesInfo.GetMessageConfig(MessageName).DynamicFields
        'For Each oField As MessagesInfo.FieldInfo In _fieldsConfig
        '    _fields.Add(New RecordField(oField), oField.Name)
        'Next
        'For Each oField As MessagesInfo.FieldInfo In _dynfieldsConfig
        '    _fields.Add(New RecordField(oField), oField.Name)
        'Next

        Try
            BuildMessageRecord(MessageName, oLogger)
        Catch ex As Exception
            If Not oLogger Is Nothing Then
                oLogger.Write(String.Format("Error has occured when building message record. Error details:"))
                oLogger.Write(ex.ToString())
                oLogger.Write("Trying to reload messages configuration...")
            End If
            'MessagesInfo.CreateConfig()
            BuildMessageRecord(MessageName, oLogger)
        End Try
    End Sub

    Private Sub BuildMessageRecord(ByVal MessageName As String, ByVal oLogger As WMS.Logic.LogHandler)
        If Not oLogger Is Nothing Then
            oLogger.Write("Looking for message : " & MessageName)
        End If
        _messageName = MessageName
        Dim _message As MessagesInfo.MessageInfo
        _message = MessagesInfo.GetMessageConfig(_messageName, oLogger)
        Try
            _fieldsConfig = _message.Fields 'MessagesInfo.GetMessageConfig(MessageName, oLogger).Fields
        Catch ex As Exception
            If Not oLogger Is Nothing Then
                oLogger.Write(String.Format("Error has occured when getting fields from message info. Error details:"))
                oLogger.Write(ex.ToString())
            End If
            Throw ex
        End Try
        Try
            _dynfieldsConfig = _message.DynamicFields 'MessagesInfo.GetMessageConfig(MessageName, oLogger).DynamicFields
        Catch ex As Exception
            If Not oLogger Is Nothing Then
                oLogger.Write(String.Format("Error has occured when getting dynamic fields from message info. Error details:"))
                oLogger.Write(ex.ToString())
            End If
            Throw ex
        End Try
        Try
            For i As Int32 = 1 To _fieldsConfig.Count 'oField As MessagesInfo.FieldInfo In _fieldsConfig
                Try
                    Dim oField As MessagesInfo.FieldInfo = _fieldsConfig.Item(i)
                    _fields.Add(New RecordField(oField), oField.Name)
                Catch ex As Exception
                    If Not oLogger Is Nothing Then
                        'oLogger.Write(String.Format("Error has occured when adding field {0} to the message. Error details:", oField.Name))
                        oLogger.Write(ex.ToString())
                    End If
                End Try
            Next
            'For Each oField As MessagesInfo.FieldInfo In _fieldsConfig
            '    Try
            '        _fields.Add(New RecordField(oField), oField.Name)
            '    Catch ex As Exception
            '        If Not oLogger Is Nothing Then
            '            oLogger.Write(String.Format("Error has occured when adding field {0} to the message. Error details:", oField.Name))
            '            oLogger.Write(ex.ToString())
            '        End If
            '    End Try
            'Next
        Catch ex As Exception
            If Not oLogger Is Nothing Then
                oLogger.Write(String.Format("Error has occured when iterating message fields from config. Error details:"))
                oLogger.Write(ex.ToString())
            End If
            Throw ex
        End Try
        Try
            'For Each oField As MessagesInfo.FieldInfo In _dynfieldsConfig
            'Try
            '_fields.Add(New RecordField(oField), oField.Name)
            'Catch ex As Exception
            For i As Int32 = 1 To _dynfieldsConfig.Count 'oField As MessagesInfo.FieldInfo In _fieldsConfig
                Try
                    Dim oField As MessagesInfo.FieldInfo = _dynfieldsConfig.Item(i)
                    _fields.Add(New RecordField(oField), oField.Name)
                Catch ex As Exception
                    If Not oLogger Is Nothing Then
                        'oLogger.Write(String.Format("Error has occured when adding field {0} to the message. Error details:", oField.Name))
                        oLogger.Write(ex.ToString())
                    End If
                End Try
            Next
        Catch ex As Exception
            If Not oLogger Is Nothing Then
                oLogger.Write(String.Format("Error has occured when iterating message dynamic fields from config. Error details:"))
                oLogger.Write(ex.ToString())
            End If
            Throw ex
        End Try
    End Sub

    Public ReadOnly Property Name() As String
        Get
            Return _messageName
        End Get
    End Property

    Default Public ReadOnly Property Item(ByVal FieldName As String) As RecordField
        Get
            If Not _fields(FieldName) Is Nothing Then
                Return _fields(FieldName)
            Else
                Throw New Made4Net.Shared.M4NException(New Exception, String.Format("Field {0} not found", FieldName), String.Format("Field {0} not found", FieldName))
            End If
        End Get
    End Property

    Public ReadOnly Property MessageFields() As Collection
        Get
            Return _fields
        End Get
    End Property

    Public Overridable Sub Parse(ByVal RecordData As String)

    End Sub

    Public Overridable Function Export() As String
        Return String.Empty
    End Function

    Public Overrides Function ToString() As String
        Dim oStringBuilder As New StringBuilder()
        oStringBuilder.AppendLine()
        For Each oRecField As RecordField In _fields
            oStringBuilder.AppendLine(String.Format("Field Name: {0}, Field Value: {1}", oRecField.Name.PadRight(12), oRecField.FieldValue.PadRight(15)))
        Next
        Return oStringBuilder.ToString()
    End Function

End Class

Public Class MessageRecordFixed
    Inherits MessageRecord

    Public Sub New(ByVal MessageName As String)
        MyBase.New(MessageName, Nothing)
    End Sub

    Public Overrides Sub Parse(ByVal RecordData As String)

    End Sub

    Public Overrides Function Export() As String

        Return ""
    End Function

End Class

Public Class MessageRecordDelimited
    Inherits MessageRecord

    Public Sub New(ByVal MessageName As String, ByVal oLogger As WMS.Logic.LogHandler)
        MyBase.New(MessageName, oLogger)
    End Sub

    Public Overrides Sub Parse(ByVal RecordData As String)
        Dim textsplitted() As String = RecordData.Split("'")
        For i As Integer = 1 To textsplitted.Length - 1 Step 2
            If textsplitted(i).IndexOf(MessagesInfo.Delimiter) > -1 Then textsplitted(i) = textsplitted(i).Replace(MessagesInfo.Delimiter, "<SEP>")
        Next
        Dim datasplitted() As String = Join(textsplitted, "").Split(MessagesInfo.Delimiter)
        For i As Integer = 1 To _fieldsConfig.Count
            Dim oField As MessagesInfo.FieldInfo = _fieldsConfig(i)
            CType(Me._fields(oField.Name), RecordField).FieldValue = datasplitted(i - 1).Replace("<SEP>", MessagesInfo.Delimiter)
        Next
    End Sub

    Public Overrides Function Export() As String
        Dim recordstring As String = ""
        For i As Int32 = 1 To _fields.Count
            Dim FieldValue As String = CType(_fields(i), RecordField).FieldValue
            If FieldValue.IndexOf(MessagesInfo.Delimiter) > -1 Then FieldValue = "'" & FieldValue & "'"
            If i <> _fields.Count Then
                recordstring = recordstring & FieldValue & MessagesInfo.Delimiter
            Else
                recordstring = recordstring & FieldValue
            End If
        Next
        Return recordstring
    End Function

End Class

Public Class RecordField

    Private _name As String
    Private _value As String = ""

    Public Sub New(ByVal Conf As MessagesInfo.FieldInfo)
        Me.Name = Conf.Name
        Me.FieldValue = ""
        If Not Conf.DefaultValue.Equals("") Then
            Me.FieldValue = Conf.DefaultValue
        End If
    End Sub

    Public Property Name() As String
        Get
            Return _name
        End Get
        Set(ByVal value As String)
            _name = value
        End Set
    End Property

    Public Property FieldValue() As String
        Get
            Return _value
        End Get
        Set(ByVal value As String)
            If IsNothing(value) Then
                _value = ""
            Else
                _value = value
            End If
        End Set
    End Property

End Class