Imports System.Messaging

<Serializable()> <CLSCompliant(False)> Public Class InventoryTransactionQ
    Inherits Made4Net.Shared.QMsgSender
    Implements IInventoryTransactionQ
    Private Shared inventoryTransactionQFactory As IInventoryTransactionQFactory = Nothing
    Public Overloads Shared Property Factory() As IInventoryTransactionQFactory
        Get
            If inventoryTransactionQFactory Is Nothing Then
                inventoryTransactionQFactory = New InventoryTransactionQFactory()
            End If
            Return inventoryTransactionQFactory
        End Get
        Set(ByVal value As IInventoryTransactionQFactory)
            inventoryTransactionQFactory = value
        End Set
    End Property
    Public Shadows Sub Send(ByVal pLbl As String) Implements IInventoryTransactionQ.Send
        Dim oQ As MessageQueue
        Dim oMsg As New Message
        Dim qPath As String
        Dim IsRemoteQueue As Boolean = False
        Dim _Logger As System.IO.StreamWriter
        Try


            Try
                _Logger = PrepareLogFile()
                If IsHttpContext() Then
                    WriteToLog(_Logger, System.Web.HttpContext.Current.Request.HttpMethod + " " + System.Web.HttpContext.Current.Request.RawUrl)
                End If
                WriteToLog(_Logger, " Send InventoryTransactionQ Request for qName:InventoryTransaction ,Label: " & pLbl)

            Catch ex As Exception

            End Try
            'qPath = Made4Net.DataAccess.DataInterface.ExecuteScalar _
            '("SELECT QUEUEPATH FROM MESSAGEQUEUES WHERE QUEUENAME = 'InventoryTransaction'", Made4Net.Schema.CONNECTION_NAME)
            qPath = Made4Net.Shared.M4NMessageQueueCollection.GetQueue("InventoryTransaction") ''PWMS-817
            WriteToLog(_Logger, " Queue Path found :" & qPath)
            If qPath.ToLower.IndexOf("formatname") <> -1 Then
                IsRemoteQueue = True
            End If
            If Not IsRemoteQueue Then
                If Not MessageQueue.Exists(qPath) Then
                    oQ = MessageQueue.Create(qPath)
                    oQ.SetPermissions("Administrators", MessageQueueAccessRights.FullControl)
                    oQ.SetPermissions("Everyone", MessageQueueAccessRights.FullControl)
                Else
                    oQ = New MessageQueue(qPath)
                End If
            Else
                oQ = New MessageQueue(qPath)
            End If
            WriteToLog(_Logger, " Current Process Infrastructure WAREHOUSE=" & Made4Net.Shared.Warehouse.CurrentWarehouse)
            WriteToLog(_Logger, " Current Process Logic WAREHOUSE=" & WMS.Logic.Warehouse.CurrentWarehouse)
            If _cs("WAREHOUSE") Is Nothing Or _cs("WAREHOUSE") = "" Then
                WriteToLog(_Logger, " Warehouse Nothing or Empty , adding WAREHOUSE=" & Warehouse.CurrentWarehouse)
                Add("WAREHOUSE", Warehouse.CurrentWarehouse)
            Else
                WriteToLog(_Logger, " Message already contains WAREHOUSE=" & _cs("WAREHOUSE"))
            End If
            WriteToLog(_Logger, PrintMessageContent)
            If _cs("WAREHOUSE") Is Nothing Or _cs("WAREHOUSE") = "" Then
                WriteToLog(_Logger, " Warehouse is Still Empty. Exit Sender")
                Throw New Exception("Msmq Message Warehouse field cannot be empty ")
            End If
            CloseLogFile(_Logger)
            oMsg.BodyStream = Serialize()
            If Not pLbl Is Nothing And pLbl <> "" Then
                oMsg.Label = pLbl
            End If
            oQ.Send(oMsg)
        Catch ex As Exception
            Throw ex
        Finally
            CloseLogFile(_Logger)
            oMsg.Dispose()
            oQ.Dispose()
        End Try

    End Sub


    Private Shadows Sub Add(key As String, value As String) Implements IInventoryTransactionQ.Add
        MyBase.Add(key, value)
    End Sub


End Class

