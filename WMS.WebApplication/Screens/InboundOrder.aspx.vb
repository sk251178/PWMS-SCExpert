Imports WMS.Logic
Imports System.Data
Imports RWMS.Logic

Public Class InboundOrder
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents TEMasterInboundOrders As Made4Net.WebControls.TableEditor
    <CLSCompliant(False)> Protected WithEvents TS As Made4Net.WebControls.TabStrip
    Protected WithEvents DTC As Made4Net.WebControls.DataTabControl
    Protected WithEvents Table1 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents Tbl As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents TEInboundOrderLines As Made4Net.WebControls.TableEditor
    Protected WithEvents DC1 As Made4Net.WebControls.DataConnector
    Protected WithEvents TEReceiptLines As Made4Net.WebControls.TableEditor
    Protected WithEvents DC2 As Made4Net.WebControls.DataConnector
    Protected WithEvents TEContactDetail As Made4Net.WebControls.TableEditor
    Protected WithEvents DC3 As Made4Net.WebControls.DataConnector
    Protected WithEvents TEHUTrans As Made4Net.WebControls.TableEditor
    Protected WithEvents DC4 As Made4Net.WebControls.DataConnector

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region "ViewState"

    Protected Overrides Function LoadPageStateFromPersistenceMedium() As Object
        Return WMS.WebCtrls.WebCtrls.Screen.LoadPageStateFromPersistenceMedium()
    End Function

    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        WMS.WebCtrls.WebCtrls.Screen.SavePageStateToPersistenceMedium(viewState, Me.Page, Screen1.ScreenID)
    End Sub

#End Region

#Region "CTor"

    Public Sub New()

    End Sub

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As New DataSet
        Dim dr As DataRow
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser()

        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)


        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        Select Case CommandName.ToLower
            Case "createnewcontact"
                dr = ds.Tables(0).Rows(0)
                Dim oContact As WMS.Logic.Contact = New WMS.Logic.Contact
                Dim oInOrd As WMS.Logic.InboundOrderHeader = New WMS.Logic.InboundOrderHeader(dr("CONSIGNEE"), dr("ORDERID"))
                oContact.CONTACTID = Made4Net.Shared.Util.getNextCounter("CONTACTID")
                oContact.CONTACT1EMAIL = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("CONTACT1EMAIL"))
                oContact.CITY = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("CONTACT1EMAIL"))
                oContact.CONTACT1FAX = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("CONTACT1FAX"))
                oContact.CONTACT1NAME = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("CONTACT1NAME"))
                oContact.CONTACT1PHONE = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("CONTACT1PHONE"))
                oContact.CONTACT2EMAIL = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("CONTACT2EMAIL"))
                oContact.CONTACT2FAX = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("CONTACT2FAX"))
                oContact.CONTACT2NAME = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("CONTACT2NAME"))
                oContact.CONTACT2PHONE = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("CONTACT2PHONE"))
                oContact.STATE = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("STATE"))
                oContact.STREET1 = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("STREET1"))
                oContact.STREET2 = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("STREET2"))
                oContact.ZIP = Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("ZIP"))
                oContact.Save(UserId)
                oInOrd.SetReceivedFrom(oContact.CONTACTID)
            Case "addhu"
                dr = ds.Tables(0).Rows(0)
                Dim cons, ordid As String
                cons = Convert.ToString(dr("consignee")).Split(",")(0)
                ordid = convert.ToString(dr("orderid")).Split(",")(0)
                Dim oOrd As New WMS.Logic.InboundOrderHeader(cons, ordid)
                oOrd.AddHandlingUnit(dr("HUTYPE"), dr("HUQTY"), UserId)
            Case "edithu"
                dr = ds.Tables(0).Rows(0)
                Dim cons, ordid As String
                cons = Convert.ToString(dr("consignee")).Split(",")(0)
                ordid = Convert.ToString(dr("orderid")).Split(",")(0)
                Dim oOrd As New WMS.Logic.InboundOrderHeader(cons, ordid)
                oOrd.UpdateHandlingUnit(dr("TRANSACTIONID"), dr("HUQTY"), UserId)
            Case "deleteio"
                dr = ds.Tables(0).Rows(0)
                Dim oOrd As New WMS.Logic.InboundOrderHeader(dr("CONSIGNEE"), dr("ORDERID"))
                If oOrd.STATUS = WMS.Lib.Statuses.InboundOrderHeader.CLOSED Then
                    Throw New ApplicationException(t.Translate("Cannot delete a closed order"))
                End If
                If Not AppUtil.deleteInbound(dr("CONSIGNEE"), dr("ORDERID"), Message) Then
                    Throw New ApplicationException(Message)
                End If

                oOrd.Delete()


            Case "deleteline"
                For Each dr In ds.Tables(0).Rows
                    Dim oOrd As New WMS.Logic.InboundOrderHeader(dr("CONSIGNEE"), dr("ORDERID"))
                    If oOrd.STATUS = WMS.Lib.Statuses.InboundOrderHeader.CLOSED Then
                        Throw New ApplicationException(t.Translate("Cannot delete line. Order is closed"))
                    End If
                    If Not AppUtil.deleteInboundLine(dr("CONSIGNEE"), dr("ORDERID"), dr("ORDERLINE"), Message) Then
                        Throw New ApplicationException(Message)
                    End If
                    Dim oOrdLine As New WMS.Logic.InboundOrderDetail(dr("CONSIGNEE"), dr("ORDERID"), dr("ORDERLINE"))
                    oOrdLine.Delete(UserId)
                Next
            Case "createline", "updateline"
                'Case
                For Each dr In ds.Tables(0).Rows
                    If WMS.Logic.InboundOrderHeader.Exists(dr("CONSIGNEE"), dr("ORDERID")) Then
                        Dim inh As New WMS.Logic.InboundOrderHeader(dr("CONSIGNEE"), dr("ORDERID"))
                        If inh.STATUS = WMS.Lib.Statuses.InboundOrderHeader.CLOSED Then
                            Message = t.Translate("Cannot update a closed order")
                            Throw New ApplicationException(Message)
                        End If
                        If inh.STATUS = WMS.Lib.Statuses.InboundOrderHeader.CANCELED Then
                            Message = t.Translate("Cannot update a cancelled order")
                            Throw New ApplicationException(Message)
                        End If
                    End If

                    Dim inb As New WMS.Logic.InboundOrderHeader(Sender, CommandName, XMLSchema, XMLData, Message)
                    'Dim referenceordline As Integer = 0
                    'Try
                    '    referenceordline = dr("REFERENCEORDLINE")
                    'Catch ex As Exception
                    'End Try

                    'Dim strsql As String = String.Format("UPDATE INBOUNDORDDETAIL SET REFERENCEORDLINE = {0} EDITDATE =GETDATE(),EDITUSER = {1} Where CONSIGNEE ='{2}' AND ORDERID ='{3}' AND ORDERLINE='{4}'", _
                    'referenceordline, WMS.Logic.GetCurrentUser, dr("CONSIGNEE"), dr("ORDERID"), dr("ORDERLINE"))
                    'Made4Net.DataAccess.DataInterface.RunSQL(strsql)
                Next
            Case "createheader", "updateheader"
                'Case "updateheader"
                For Each dr In ds.Tables(0).Rows


                    If WMS.Logic.InboundOrderHeader.Exists(dr("CONSIGNEE"), dr("ORDERID")) Then
                        Dim inh As New WMS.Logic.InboundOrderHeader(dr("CONSIGNEE"), dr("ORDERID"))
                        If inh.STATUS = WMS.Lib.Statuses.InboundOrderHeader.CLOSED Then
                            Message = t.Translate("Cannot update a closed order")
                            Throw New ApplicationException(Message)
                        End If
                        If inh.STATUS = WMS.Lib.Statuses.InboundOrderHeader.CANCELED Then
                            Message = t.Translate("Cannot update a cancelled order")
                            Throw New ApplicationException(Message)
                        End If
                    End If
                Next

                Dim inb As New WMS.Logic.InboundOrderHeader(Sender, CommandName, XMLSchema, XMLData, Message)

                'For Each dr In ds.Tables(0).Rows

                '    Dim referenceordline As Integer = 0
                '    Try
                '        referenceordline = dr("REFERENCEORD")
                '    Catch ex As Exception
                '    End Try

                '    Dim strsql As String = String.Format("UPDATE INBOUNDORDHEADER SET REFERENCEORD = {0} EDITDATE =GETDATE(),EDITUSER = {1} Where CONSIGNEE ='{2}' AND ORDERID ='{3}' ", _
                '    referenceordline, WMS.Logic.GetCurrentUser, dr("CONSIGNEE"), dr("ORDERID"))
                '    Made4Net.DataAccess.DataInterface.RunSQL(strsql)
                'Next
            Case "creatercn"
                Dim strsql As String

                For Each dr In ds.Tables(0).Rows
                    If WMS.Logic.InboundOrderHeader.Exists(dr("CONSIGNEE"), dr("ORDERID")) Then
                        Dim inh As New WMS.Logic.InboundOrderHeader(dr("CONSIGNEE"), dr("ORDERID"))
                        If inh.STATUS = WMS.Lib.Statuses.InboundOrderHeader.CLOSED Then
                            Message = t.Translate("Cannot export order lines to receipt, closed order")
                            Throw New ApplicationException(Message)
                        End If
                        If inh.STATUS = WMS.Lib.Statuses.InboundOrderHeader.CANCELED Then
                            Message = t.Translate("Cannot export order lines to receipt, cancelled order")
                            Throw New ApplicationException(Message)
                        End If
                    End If
                Next

                'Commented for RWMS-2543 START
                'Dim inb As New WMS.Logic.InboundOrderHeader(Sender, CommandName, XMLSchema, XMLData, Message)
                'Dim receipt As String = Message.Substring(Message.LastIndexOf(" ")).Trim()
                'Commented for RWMS-2543 END

                'RWMS-2543 START
                Dim receipt As String
                Dim inb As New WMS.Logic.InboundOrderHeader(Sender, CommandName, XMLSchema, XMLData, Message, receipt)
                'RWMS-2543 END

                For Each dr In ds.Tables(0).Rows

                    Dim ORDERLINE As String = dr("ORDERLINE")


                    strsql = "update ReceiptDetail " _
                     & " set AVGWEIGHT=0,REFORD = ih.REFERENCEORD, REFORDLINE = id.REFERENCEORDLINE " _
                     & " FROM         dbo.INBOUNDORDHEADER AS ih INNER JOIN " _
                     & " dbo.INBOUNDORDDETAIL AS id ON ih.CONSIGNEE = id.CONSIGNEE AND ih.ORDERID = id.ORDERID INNER JOIN " _
                     & " dbo.RECEIPTDETAIL AS rd INNER JOIN " _
                     & " dbo.RECEIPTHEADER AS rh ON rd.RECEIPT = rh.RECEIPT ON id.ORDERID = rd.ORDERID AND id.ORDERLINE = rd.ORDERLINE AND  " _
                     & " ID.CONSIGNEE = rd.CONSIGNEE " _
                     & " WHERE    rd.CONSIGNEE='{0}' and rd.ORDERID = '{1}'  AND rd.RECEIPT = '{2}' and rd.ORDERLINE = '{3}'"
                    strsql = String.Format(strsql, inb.CONSIGNEE, inb.ORDERID, receipt, ORDERLINE)

                    Made4Net.DataAccess.DataInterface.RunSQL(strsql)

                Next

                Dim rec As New WMS.Logic.ReceiptHeader(receipt)
                WMS.Logic.ReceiptHeader.PrintReceivingDoc(receipt, Made4Net.Shared.Translation.Translator.CurrentLanguageID, WMS.Logic.GetCurrentUser, False)

            Case "closeinbound"
                For Each dr In ds.Tables(0).Rows

                    If Not AppUtil.closeInbound(dr("CONSIGNEE"), dr("ORDERID"), Message) Then
                        Throw New ApplicationException(Message)
                    End If

                Next

            Case "cancelinbound"
                For Each dr In ds.Tables(0).Rows

                    If Not AppUtil.cancelInbound(dr("CONSIGNEE"), dr("ORDERID"), Message) Then
                        Throw New ApplicationException(Message)
                        'Dim inH As New WMS.Logic.InboundOrderHeader(dr("CONSIGNEE"), dr("ORDERID"))
                        'inH.CancelInbound(WMS.Logic.GetCurrentUser)
                    End If
                Next

                Dim inb As New WMS.Logic.InboundOrderHeader(Sender, CommandName, XMLSchema, XMLData, Message)

        End Select
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub TEInboundOrderLines_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEInboundOrderLines.CreatedChildControls
        With TEInboundOrderLines.ActionBar
            With .Button("save")
                '.ObjectDLL = "WMS.Logic.dll"
                '.ObjectName = "WMS.Logic.InboundOrderHeader"

                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.InboundOrder"

                Select Case TEInboundOrderLines.CurrentMode
                    Case Made4Net.WebControls.TableEditorMode.Edit
                        .CommandName = "updateline"
                    Case Made4Net.WebControls.TableEditorMode.Insert
                        .CommandName = "createline"
                End Select
            End With
            .AddSpacer()
            .AddExecButton("CreateRCN", "Export To Receipt", Made4Net.WebControls.SkinManager.GetImageURL("Receipt"))
            With .Button("CreateRCN")
                '.ObjectDLL = "WMS.Logic.dll"
                '.ObjectName = "WMS.Logic.InboundOrderHeader"
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.InboundOrder"

                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
            End With
            With .Button("Delete")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.InboundOrder"
                .CommandName = "DeleteLine"
            End With
        End With
    End Sub

    Private Sub TEMasterInboundOrders_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEMasterInboundOrders.CreatedChildControls
        With TEMasterInboundOrders.ActionBar
            With .Button("save")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.InboundOrder"
                '.ObjectDLL = "WMS.Logic.dll"
                '.ObjectName = "WMS.Logic.InboundOrderHeader"
                Select Case TEMasterInboundOrders.CurrentMode
                    Case Made4Net.WebControls.TableEditorMode.Edit
                        .CommandName = "updateheader"
                    Case Made4Net.WebControls.TableEditorMode.Insert
                        .CommandName = "createheader"
                End Select
            End With

            'Added by priel
            .AddExecButton("CancelInbound", "Cancel Order", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarCancelOrders"))
            With .Button("CancelInbound")
                '.ObjectDLL = "WMS.Logic.dll"
                '.ObjectName = "WMS.Logic.InboundOrderHeader"
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.InboundOrder"
                .CommandName = "CancelInbound"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .ConfirmRequired = True
                .ConfirmMessage = "Are you sure you want to cancel the order?"
            End With

            .AddExecButton("CloseInbound", "Close Inbound Order", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnCloseReceipt"))
            With .Button("CloseInbound")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.InboundOrder"
                .CommandName = "CloseInbound"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .ConfirmRequired = True
                .ConfirmMessage = "Are you sure you want to close the order?"
            End With

            If TEMasterInboundOrders.CurrentMode = Made4Net.WebControls.TableEditorMode.Edit Then
                With .Button("Delete")
                    .ObjectDLL = "WMS.WebApp.dll"
                    .ObjectName = "WMS.WebApp.InboundOrder"
                    .CommandName = "DeleteIO"
                End With

            End If


        End With
    End Sub

    Private Sub TEContactDetail_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEContactDetail.CreatedChildControls
        With TEContactDetail
            With .ActionBar.Button("Save")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.InboundOrder"
                If TEContactDetail.CurrentMode = Made4Net.WebControls.TableEditorMode.Insert Then
                    .CommandName = "createnewcontact"
                End If
            End With
        End With
    End Sub

    Private Sub TEMasterInboundOrders_RecordSelected(ByVal sender As Made4Net.WebControls.TableEditor, ByVal e As Made4Net.WebControls.TableEditorEventArgs) Handles TEMasterInboundOrders.RecordSelected
        Dim tds As DataTable = TEMasterInboundOrders.CreateDataTableForSelectedRecord()
        Dim vals As New Specialized.NameValueCollection
        vals.Add("ORDERID", tds.Rows(0)("ORDERID"))
        vals.Add("CONSIGNEE", tds.Rows(0)("CONSIGNEE"))
        If TEContactDetail.PreDefinedValues Is Nothing Then
            TEContactDetail.PreDefinedValues = vals
        End If
        If TEHUTrans.PreDefinedValues Is Nothing Then
            TEHUTrans.PreDefinedValues = vals
        End If
    End Sub

    Private Sub TEContactDetail_AfterItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles TEContactDetail.AfterItemCommand
        If e.CommandName = "createnewcontact" Then
            TEMasterInboundOrders.RefreshData()
            TEContactDetail.RefreshData()
        End If
    End Sub

    Private Sub TEHUTrans_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEHUTrans.CreatedChildControls
        With TEHUTrans
            With .ActionBar.Button("Save")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.InboundOrder"
                If TEHUTrans.CurrentMode = Made4Net.WebControls.TableEditorMode.Insert Then
                    .CommandName = "addhu"
                Else
                    .CommandName = "edithu"
                End If
            End With
        End With
    End Sub

End Class