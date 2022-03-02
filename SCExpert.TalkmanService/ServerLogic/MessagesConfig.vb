Imports System.Xml
Public Class MessagesInfo
    Private _messages As New Hashtable
    Private Shared _messageeof As String
    Private Shared _messageformat As String
    Private Shared _messagedelimiter As String
    Private Shared _config As MessagesInfo
    Private Sub Init()
        'Dim conf As New XmlDocument
        'conf.Load(System.Configuration.ConfigurationManager.AppSettings.Get("MessagesConfig"))
        '_messageeof = conf.SelectSingleNode(".//MessageEOF").InnerText
        '_messageformat = conf.SelectSingleNode(".//MessageFormat").InnerText
        '_messagedelimiter = conf.SelectSingleNode(".//MessageDelimiter").InnerText
        'For Each msgelement As XmlElement In conf.SelectNodes(".//Message")
        'Dim newMsg As New MessageInfo(msgelement)
        '_messages.Add(newMsg.Name, newMsg)
        'Next
        'Dim ParamsDT As New DataTable
        'Made4Net.DataAccess.DataInterface.FillDataset("select * from ScExpertTalkManParams", ParamsDT, False, "Made4NetSchema")
        _messageeof = Made4Net.Shared.AppConfig.GetSystemParameter("TalkManMessageEOF", "CRLFCRLF")
        _messageformat = Made4Net.Shared.AppConfig.GetSystemParameter("TalkManMessageFormat", "Delimited")
        _messagedelimiter = Made4Net.Shared.AppConfig.GetSystemParameter("TalkManMessageDelimiter", ",")
        Dim MsgDT As New DataTable
        Made4Net.DataAccess.DataInterface.FillDataset("select * from ScExpertTalkManMessages", MsgDT, False, "Made4NetSchema")
        For Each _msgDR As DataRow In MsgDT.Rows
            Dim newMsg As New MessageInfo(_msgDR)
            _messages.Add(newMsg.Name, newMsg)
        Next
    End Sub
    Public Shared Sub CreateConfig()
        _config = New MessagesInfo
        _config.Init()
    End Sub
    Public Shared ReadOnly Property Delimiter() As String
        Get
            Return _messagedelimiter
        End Get
    End Property
    Public Shared ReadOnly Property Format() As String
        Get
            Return _messageformat
        End Get
    End Property
    Public Shared ReadOnly Property EOF() As String
        Get
            Return _messageeof
        End Get
    End Property
    Public Shared Function GetMessageConfig(ByVal MessageName As String) As MessageInfo
        Return _config._messages(MessageName)
    End Function
    Public Shared Function GetMessageInstance(ByVal MessageData As String) As ClientMessage
        Dim Firstline As String = (MessageData.Split(vbCrLf))(0)
        Dim clmsg As ClientMessage
        If MessagesInfo.Format.Equals(MessagesInfo.RecordFormat.Delimited) Then
            clmsg = New ClientMessage(Firstline.Split(Delimiter)(0))
            clmsg.Parse(MessageData)
        Else

        End If
        Return clmsg
    End Function

    Public Class MessageInfo
        Private _fields As New Collection
        Private _dynfields As New Collection
        Private _dynfieldsview As String
        Private _dynfieldsviewconn As String
        Private _name As String
        Private _type As String
        Private _responsemessage As String
        Private _messagehandler As String
        Private _confirmation As String
        Public Sub New(ByVal MessageXML As XmlElement)
            Me.Parse(MessageXML)
        End Sub
        Public Sub New(ByVal MessageDR As DataRow)
            Me.Parse(MessageDR)
        End Sub
        Public ReadOnly Property Name() As String
            Get
                Return _name
            End Get
        End Property
        Public ReadOnly Property ViewName() As String
            Get
                Return _dynfieldsview
            End Get
        End Property
        Public ReadOnly Property Connection() As String
            Get
                Return _dynfieldsviewconn
            End Get
        End Property
        Public ReadOnly Property Type() As String
            Get
                Return _type
            End Get
        End Property
        Public ReadOnly Property ResponseMessage() As String
            Get
                Return _responsemessage
            End Get
        End Property
        Public ReadOnly Property MessageHandler() As String
            Get
                Return _messagehandler
            End Get
        End Property
        Public ReadOnly Property Confirmation() As String
            Get
                Return _confirmation
            End Get
        End Property
        Public ReadOnly Property Fields() As Collection
            Get
                Return _fields
            End Get
        End Property
        Public ReadOnly Property DynamicFields() As Collection
            Get
                Return _dynfields
            End Get
        End Property
        Public ReadOnly Property Field(ByVal FieldName As String) As FieldInfo
            Get
                'Return _fields(FieldName)
                If _fields.Contains(FieldName) Then
                    Return _fields(FieldName)
                ElseIf _dynfields.Contains(FieldName) Then
                    Return _dynfields(FieldName)
                Else
                    Return Nothing
                End If
            End Get
        End Property
        Public ReadOnly Property DynamicField(ByVal FieldName As String) As FieldInfo
            Get
                Return _dynfields(FieldName)
            End Get
        End Property
        Private Sub SaveToDb()
            Dim _conf As String
            If _confirmation.Equals("Y") Then
                _conf = "1"
            Else
                _conf = "0"
            End If
            Dim sql As String = "INSERT INTO SCEXPERTTALKMANMESSAGES(MESSAGENAME,MESSAGETYPE,CONFIRMATION,RESPONSEMESSAGE,DYNAMICFIELDSVIEWNAME,FUNCTIONNAME) VALUES ('" & _name & "','" & _type & "','" & _conf & "','" & _responsemessage & "','" & _dynfieldsview & "','')"
            Made4Net.DataAccess.DataInterface.RunSQL(sql, "Made4NetSchema")
            For Each _field As FieldInfo In Me._fields
                sql = "INSERT INTO SCEXPERTTALKMANMESSAGEFIELDS (MESSAGENAME,FIELDNAME,FIELDVALUE,FIELDLENGTH) VALUES ('" & _name & "','" & _field.Name & "','" & _field.DefaultValue & "','" & _field.Length & "')"
                Made4Net.DataAccess.DataInterface.RunSQL(sql, "Made4NetSchema")
            Next
            For Each _field As FieldInfo In Me._dynfields
                sql = "INSERT INTO SCEXPERTTALKMANMESSAGEDYNAMICFIELDS (MESSAGENAME,FIELDNAME,FIELDLENGTH) VALUES ('" & _name & "','" & _field.Name & "','" & _field.Length & "')"
                Made4Net.DataAccess.DataInterface.RunSQL(sql, "Made4NetSchema")
            Next
        End Sub
        Private Sub Parse(ByVal MessageXML As XmlElement)
            _name = MessageXML.SelectSingleNode(".//MessageName").InnerText
            If Not IsNothing(MessageXML.SelectSingleNode(".//MessageType")) Then
                _type = MessageXML.SelectSingleNode(".//MessageType").InnerText
            Else
                _type = ""
            End If
            If Not IsNothing(MessageXML.SelectSingleNode(".//ResponseMessage")) Then
                _responsemessage = MessageXML.SelectSingleNode(".//ResponseMessage").InnerText
            Else
                _responsemessage = ""
            End If
            If Not IsNothing(MessageXML.SelectSingleNode(".//Confirmation")) Then
                _confirmation = MessageXML.SelectSingleNode(".//Confirmation").InnerText
            Else
                _confirmation = "Y"
            End If
            For Each msgfield As XmlElement In MessageXML.SelectNodes(".//Fields/Field")
                Dim NewField As New FieldInfo(msgfield)
                _fields.Add(NewField, NewField.Name)
            Next
            If Not IsNothing(MessageXML.SelectSingleNode(".//DynamicFields")) Then
                _dynfieldsview = MessageXML.SelectSingleNode(".//DynamicFields").Attributes("viewname").InnerText
                Try
                    _dynfieldsviewconn = MessageXML.SelectSingleNode(".//DynamicFields").Attributes("connection").InnerText
                Catch ex As Exception
                    _dynfieldsviewconn = ""
                End Try
                For Each msgfield As XmlElement In MessageXML.SelectNodes(".//DynamicFields/Field")
                    Dim NewField As New FieldInfo(msgfield)
                    _dynfields.Add(NewField, NewField.Name)
                Next
            End If
            SaveToDb()
        End Sub
        Private Sub Parse(ByVal MessageDR As DataRow)
            _name = MessageDR("MessageName")
            If Not IsDBNull(MessageDR("MessageType")) Then
                _type = MessageDR("MessageType")
            Else
                _type = ""
            End If
            If Not IsDBNull(MessageDR("ResponseMessage")) Then
                _responsemessage = MessageDR("ResponseMessage")
            Else
                _responsemessage = ""
            End If
            If Not IsDBNull(MessageDR("Confirmation")) Then
                _confirmation = MessageDR("Confirmation")
            Else
                _confirmation = "Y"
            End If
            If Not IsDBNull(MessageDR("DYNAMICFIELDSVIEWNAME")) Then
                _dynfieldsview = MessageDR("DYNAMICFIELDSVIEWNAME")
            Else
                _dynfieldsview = ""
            End If
            _messagehandler = MessageDR("FunctionName")
            'For Each msgfield As XmlElement In MessageXML.SelectNodes(".//Fields/Field")
            'Dim NewField As New FieldInfo(msgfield)
            '_fields.Add(NewField, NewField.Name)
            'Next
            Dim MsgFieldsDT As New DataTable
            Made4Net.DataAccess.DataInterface.FillDataset("select * from ScExpertTalkManMessageFields where MessageName='" & Me.Name & "'", MsgFieldsDT, False, "Made4NetSchema")
            For Each _msgFieldsDR As DataRow In MsgFieldsDT.Rows
                Dim NewField As New FieldInfo(_msgFieldsDR)
                _fields.Add(NewField, NewField.Name)
            Next
            
            If Not _dynfieldsview.Equals("") Then
                Dim MsgDynFieldsDT As New DataTable
                Made4Net.DataAccess.DataInterface.FillDataset("select * from ScExpertTalkManMessageDynamicFields where MessageName='" & Me.Name & "'", MsgDynFieldsDT, False, "Made4NetSchema")

                For Each _dynfield As DataRow In MsgDynFieldsDT.Rows
                    Dim NewField As New FieldInfo(_dynfield)
                    _dynfields.Add(NewField, NewField.Name)
                Next
            End If
        End Sub
        Public Function CreateMessage() As ClientMessage
            Return New ClientMessage(Me.Name)
        End Function
    End Class
    Public Class FieldInfo
        Private _name As String
        Private _length As String
        Private _defaultvalue As String
        Public Sub New(ByVal MessageXML As XmlElement)
            Parse(MessageXML)
        End Sub
        Public Sub New(ByVal MessageDR As DataRow)
            Parse(MessageDR)
        End Sub
        Private Sub Parse(ByVal MessageXML As XmlElement)
            _name = MessageXML.Attributes("name").InnerText
            If Not IsNothing(MessageXML.Attributes("length")) Then
                _length = MessageXML.Attributes("length").InnerText
            Else
                _length = "0"
            End If
            If Not IsNothing(MessageXML.Attributes("value")) Then
                _defaultvalue = MessageXML.Attributes("value").InnerText
            Else
                _defaultvalue = ""
            End If
        End Sub
        Private Sub Parse(ByVal MessageDR As DataRow)
            _name = MessageDR("fieldname")
            If Not IsDBNull(MessageDR("fieldlength")) Then
                _length = MessageDR("fieldlength")
            Else
                _length = "0"
            End If
            If MessageDR.Table.Columns.Contains("fieldvalue") Then
                If Not IsDBNull(MessageDR("fieldvalue")) Then
                    _defaultvalue = MessageDR("fieldvalue")
                Else
                    _defaultvalue = ""
                End If
            Else
                _defaultvalue = ""
            End If
        End Sub
        Public ReadOnly Property Name() As String
            Get
                Return _name
            End Get
        End Property
        Public ReadOnly Property Length() As String
            Get
                Return _length
            End Get
        End Property
        Public ReadOnly Property DefaultValue() As String
            Get
                Return _defaultvalue
            End Get
        End Property
    End Class
    Public Class MessageType
        Public Shared LUT = "LUT"
        Public Shared ORD = "ORD"
    End Class
    Public Class RecordFormat
        Public Shared Fixed = "Fixed"
        Public Shared Delimited = "Delimited"
    End Class

End Class
