Imports System.Data
Imports System.Collections
Imports Made4Net.DataAccess
Imports Made4Net.Shared.Conversion
Imports Made4Net.Shared
Imports WMS.Logic

Public Class GelsonsReceiptHeader
    Inherits WMS.Logic.ReceiptHeader

    Public Sub New(ByVal receipt As String, ByVal loadObj As Boolean)
        MyBase.New(receipt, loadObj)

    End Sub


    Public Sub closeReceiptHeader(ByVal pUser As String)

        If _status = WMS.Lib.Statuses.Receipt.CLOSED OrElse _status = WMS.Lib.Statuses.Receipt.CANCELLED Then
            Throw New Made4Net.Shared.M4NException(New Exception(), "Can not close receipt. Incorrect status.", "Can not close receipt. Incorrect status.")
        End If

        Dim _prValRes As String

        _prValRes = ValidateReceiptClose(Me.RECEIPT)
        If _prValRes.Equals("-1") Then
            Throw New ApplicationException("Cannot close receipt.Process validation failed.")
        End If

        _status = WMS.Lib.Statuses.Receipt.CLOSED
        _closereceiptdate = DateTime.Now
        _edituser = pUser
        _editdate = DateTime.Now
        DataInterface.RunSQL(String.Format("update receiptheader set status = {0},edituser={1},editdate={2},CLOSERECEIPTDATE={3} where {4}", Made4Net.Shared.Util.FormatField(_status), Made4Net.Shared.Util.FormatField(_edituser), Made4Net.Shared.Util.FormatField(_editdate), Made4Net.Shared.Util.FormatField(_closereceiptdate), WhereClause))
        For Each rl As ReceiptDetail In _lines
            rl.CalculateAverageWeight()
            Select Case rl.DOCUMENTTYPE
                Case WMS.Lib.DOCUMENTTYPES.FLOWTHROUGH
                    Dim oFlowthrough As New Flowthrough(rl.CONSIGNEE, rl.ORDERID)
                    oFlowthrough.SetReceived(pUser)
            End Select
        Next
        Try
            'Dim em As New EventManagerQ
            'Dim EventType As Int32 = WMS.Logic.WMSEvents.EventType.ReceiptClose
            'em.Add("EVENT", EventType)
            'em.Add("USERID", pUser)
            'em.Add("DOCUMENT", _receipt)
            'em.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
            'em.Send(WMSEvents.EventDescription(EventType))
            Dim aq As EventManagerQ = New EventManagerQ
            aq.Add("EVENT", WMS.Logic.WMSEvents.EventType.ReceiptClose)
            aq.Add("ACTIVITYTYPE", WMS.Lib.Actions.Audit.RECEIPTCLOSE)
            aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ACTIVITYTIME", "0")
            aq.Add("CONSIGNEE", "")
            aq.Add("DOCUMENT", _receipt)
            aq.Add("DOCUMENTLINE", 0)
            aq.Add("FROMLOAD", "")
            aq.Add("FROMLOC", "")
            aq.Add("FROMWAREHOUSEAREA", "")
            aq.Add("FROMQTY", 0)
            aq.Add("FROMSTATUS", _status)
            aq.Add("NOTES", "")
            aq.Add("SKU", "")
            aq.Add("TOLOAD", "")
            aq.Add("TOLOC", "")
            aq.Add("TOWAREHOUSEAREA", "")
            aq.Add("TOQTY", "")
            aq.Add("TOSTATUS", _status)
            aq.Add("USERID", pUser)
            aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("ADDUSER", pUser)
            aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
            aq.Add("EDITUSER", pUser)
            aq.Send(WMS.Lib.Actions.Audit.RECEIPTCLOSE)
        Catch ex As Exception
        End Try
    End Sub



    Public Shared Function ValidateReceiptClose(ByVal ReceiptID As String) As String
        Dim _receipt As New WMS.Logic.ReceiptHeader(ReceiptID)

        Dim dtException As New DataTable
        Dim sSql As String = String.Format("select RECEIPTLINE,SUM(qty) as qty from RECEIVINGEXCEPTION where RECEIPT = '{0}' group by RECEIPT,RECEIPTLINE", ReceiptID)
        DataInterface.FillDataset(sSql, dtException)

        Dim sFilter As String = String.Empty
        Dim dExceptionQty As Decimal = 0
        Dim drArray() As DataRow
        For Each _recline As WMS.Logic.ReceiptDetail In _receipt.LINES
            sFilter = String.Format("RECEIPTLINE = {0}", _recline.RECEIPTLINE)
            drArray = dtException.Select(sFilter)
            If drArray.Length = 0 Then
                dExceptionQty = 0
            Else
                dExceptionQty = drArray(0)("qty")
            End If
            If _recline.QTYRECEIVED + dExceptionQty < _recline.QTYEXPECTED Then
                Return "-1"
            End If
        Next

        Return "0"
    End Function

End Class
