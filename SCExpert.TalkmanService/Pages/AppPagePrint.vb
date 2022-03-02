Imports WMS.Logic
Imports WMS.Lib

Public Class AppPagePrint
    Inherits AppPageProcessor

#Region "Enums"

    Private Enum ResponseCode
        Printed
        [Error]
    End Enum

    Private Enum PrintTypes
        PickingLabel = 1
        ShippingLabel = 2
        ContentList = 3
        PickList = 4
    End Enum

    Private Enum LabelType
        PickingLabel
        ShippingLabel
    End Enum

#End Region

    Public Sub New()
        MyBase.New()
    End Sub

    Public Overrides Function Process(ByVal oLogger As WMS.Logic.LogHandler) As ClientMessage
        If Not oLogger Is Nothing Then
            oLogger.Write(String.Format("Processing Printing Request..."))
            oLogger.Write(String.Format("Print Type from message: {0}", _msg(0).Item("PRINT TYPE").FieldValue))
            oLogger.Write(String.Format("Print Id from message: {0}", _msg(0).Item("PRINT ID").FieldValue))
            oLogger.Write(String.Format("Printer Name from message: {0}", _msg(0).Item("PRINTER").FieldValue))
        End If
        Dim printType As Int32 = _msg(0).Item("PRINT TYPE").FieldValue
        Select Case printType
            Case PrintTypes.PickingLabel
                Return PrintLabels(LabelType.PickingLabel, oLogger)
            Case PrintTypes.ShippingLabel
                Return PrintLabels(LabelType.ShippingLabel, oLogger)
            Case PrintTypes.ContentList
                Return PrintContentList(oLogger)
            Case PrintTypes.PickList
                Return PrintPickList(oLogger)
        End Select
    End Function

    Private Function PrintLabels(ByVal pLabelType As LabelType, ByVal oLogger As WMS.Logic.LogHandler) As ClientMessage
        If Not oLogger Is Nothing Then
            If pLabelType = LabelType.PickingLabel Then
                oLogger.Write(String.Format("Request Type - {0} Label...", "Picking"))
            ElseIf pLabelType = LabelType.ShippingLabel Then
                oLogger.Write(String.Format("Request Type - {0} Label...", "Shipping"))
            End If
        End If
        Try
            Dim prntr As LabelPrinter
            If Not oLogger Is Nothing Then
                oLogger.Write("Label Printer extracted from message: " & _msg(0).Item("PRINTER").FieldValue)
            End If
            prntr = New LabelPrinter(_msg(0).Item("PRINTER").FieldValue)
            If Not Session("PCKPicklist") Is Nothing AndAlso CType(Session("PCKPicklist"), WMS.Logic.Picklist).PicklistID.Equals(_msg(0).Item("PRINT ID").FieldValue, StringComparison.OrdinalIgnoreCase) Then
                Dim oPicklist As WMS.Logic.Picklist = Session("PCKPicklist")
                If pLabelType = LabelType.PickingLabel Then
                    oPicklist.PrintPickLabels(prntr.PrinterQName)
                ElseIf pLabelType = LabelType.ShippingLabel Then
                    oPicklist.PrintShipLabels(prntr.PrinterQName)
                End If
            ElseIf Not Session("PARPCKPicklist") Is Nothing AndAlso CType(Session("PARPCKPicklist"), WMS.Logic.ParallelPicking).ParallelPickId.Equals(_msg(0).Item("PRINT ID").FieldValue, StringComparison.OrdinalIgnoreCase) Then
                Dim pck As ParallelPicking = Session("PARPCKPicklist")
                If pLabelType = LabelType.PickingLabel Then
                    pck.PrintPickLabels(prntr.PrinterQName)
                ElseIf pLabelType = LabelType.ShippingLabel Then
                    'pck.PrintShipLabels(prntr.PrinterQName)
                    For Each oPcklst As WMS.Logic.Picklist In pck.PickLists
                        oPcklst.PrintShipLabels(prntr.PrinterQName)
                    Next
                End If
            Else
                If IsPicklistExists(_msg(0).Item("PRINT ID").FieldValue) Then
                    Dim oPicklist As New WMS.Logic.Picklist(_msg(0).Item("PRINT ID").FieldValue)
                    If pLabelType = LabelType.PickingLabel Then
                        oPicklist.PrintPickLabels(prntr.PrinterQName)
                    ElseIf pLabelType = LabelType.ShippingLabel Then
                        oPicklist.PrintShipLabels(prntr.PrinterQName)
                    End If
                Else    'Its a parallel picklist
                    Dim pck As New ParallelPicking(_msg(0).Item("PRINT ID").FieldValue)
                    If pLabelType = LabelType.PickingLabel Then
                        pck.PrintPickLabels(prntr.PrinterQName)
                    ElseIf pLabelType = LabelType.ShippingLabel Then
                        'pck.PrintShipLabels(prntr.PrinterQName)
                        For Each oPcklst As WMS.Logic.Picklist In pck.PickLists
                            oPcklst.PrintShipLabels(prntr.PrinterQName)
                        Next
                    End If
                End If
            End If
        Catch ex As Made4Net.Shared.M4NException
            If Not oLogger Is Nothing Then
                oLogger.Write("Error while Printing: " & ex.GetErrMessage(0))
            End If
            Me._responseCode = ResponseCode.Error
            Me.FillSingleRecord(oLogger)
            Return _resp
        Catch ex As Exception
            If Not oLogger Is Nothing Then
                oLogger.Write("Error while Printing: " & ex.ToString)
            End If
            Me._responseCode = ResponseCode.Error
            Me.FillSingleRecord(oLogger)
            Return _resp
        End Try
        If Not oLogger Is Nothing Then
            oLogger.Write("Printing completed successfully...")
        End If
        Me._responseCode = ResponseCode.Printed
        Me.FillSingleRecord(oLogger)
        Return _resp
    End Function

    Private Function PrintContentList(ByVal oLogger As WMS.Logic.LogHandler) As ClientMessage
        If Not oLogger Is Nothing Then
            oLogger.Write("Request Type - Content List...")
        End If
        Try
            Dim prntr As String
            Dim userid As String = Made4Net.Shared.Authentication.User.GetCurrentUser.UserName
            If Not oLogger Is Nothing Then
                oLogger.Write("Printer extracted from message: " & _msg(0).Item("PRINTER").FieldValue)
            End If
            prntr = _msg(0).Item("PRINTER").FieldValue
            If IsPicklistExists(_msg(0).Item("PRINT ID").FieldValue) Then
                'Dim oPicklist As New WMS.Logic.Picklist(_msg(0).Item("PRINT ID").FieldValue)
                PrintPicklistContentList(_msg(0).Item("PRINT ID").FieldValue, prntr, oLogger)
            Else    'Its a parallel picklist
                Dim pck As New ParallelPicking(_msg(0).Item("PRINT ID").FieldValue)
                For Each oPcklst As WMS.Logic.Picklist In pck.PickLists
                    PrintPicklistContentList(oPcklst.PicklistID, prntr, oLogger)
                Next
            End If
        Catch ex As Made4Net.Shared.M4NException
            If Not oLogger Is Nothing Then
                oLogger.Write("Error while Printing: " & ex.GetErrMessage(0))
            End If
            Me._responseCode = ResponseCode.Error
            Me.FillSingleRecord(oLogger)
            Return _resp
        Catch ex As Exception
            If Not oLogger Is Nothing Then
                oLogger.Write("Error while Printing: " & ex.ToString)
            End If
            Me._responseCode = ResponseCode.Error
            Me.FillSingleRecord(oLogger)
            Return _resp
        End Try
        If Not oLogger Is Nothing Then
            oLogger.Write("Printing completed successfully...")
        End If
        Me._responseCode = ResponseCode.Printed
        Me.FillSingleRecord(oLogger)
        Return _resp
    End Function

    Private Function PrintPickList(ByVal oLogger As WMS.Logic.LogHandler) As ClientMessage
        If Not oLogger Is Nothing Then
            oLogger.Write("Request Type - Content List...")
        End If
        Try
            Dim prntr As String
            Dim userid As String = Made4Net.Shared.Authentication.User.GetCurrentUser.UserName
            If Not oLogger Is Nothing Then
                oLogger.Write("Printer extracted from message: " & _msg(0).Item("PRINTER").FieldValue)
            End If
            prntr = _msg(0).Item("PRINTER").FieldValue
            If IsPicklistExists(_msg(0).Item("PRINT ID").FieldValue) Then
                Dim oPicklist As New WMS.Logic.Picklist(_msg(0).Item("PRINT ID").FieldValue)
                oPicklist.Print(prntr)
            Else    'Its a parallel picklist
                Dim pck As New ParallelPicking(_msg(0).Item("PRINT ID").FieldValue)
                For Each oPcklst As WMS.Logic.Picklist In pck.PickLists
                    oPcklst.Print(prntr)
                Next
            End If
        Catch ex As Made4Net.Shared.M4NException
            If Not oLogger Is Nothing Then
                oLogger.Write("Error while Printing: " & ex.GetErrMessage(0))
            End If
            Me._responseCode = ResponseCode.Error
            Me.FillSingleRecord(oLogger)
            Return _resp
        Catch ex As Exception
            If Not oLogger Is Nothing Then
                oLogger.Write("Error while Printing: " & ex.ToString)
            End If
            Me._responseCode = ResponseCode.Error
            Me.FillSingleRecord(oLogger)
            Return _resp
        End Try
        If Not oLogger Is Nothing Then
            oLogger.Write("Printing completed successfully...")
        End If
        Me._responseCode = ResponseCode.Printed
        Me.FillSingleRecord(oLogger)
        Return _resp
    End Function

    Private Function IsPicklistExists(ByVal pPicklistId As String) As Boolean
        Dim SQL As String = String.Format("select count(1) from pickheader where picklist = '{0}'", pPicklistId)
        Return Convert.ToBoolean(Made4Net.DataAccess.DataInterface.ExecuteScalar(SQL))
    End Function

    Private Sub PrintPicklistContentList(ByVal pPicklistId As String, ByVal pPrinter As String, ByVal oLogger As WMS.Logic.LogHandler)
        Dim SQL As String = String.Format("select distinct tocontainer from pickdetail where picklist = '{0}'", pPicklistId)
        Dim dt As New DataTable
        Made4Net.DataAccess.DataInterface.FillDataset(SQL, dt)
        Dim contid As String
        For Each dr As DataRow In dt.Rows
            Try
                contid = dr("tocontainer")
                Dim oCont As New WMS.Logic.Container(contid, True)
                oCont.PrintContentList(pPrinter, 0, Made4Net.Shared.Authentication.User.GetCurrentUser.UserName)
            Catch ex As Exception
                If Not oLogger Is Nothing Then
                    oLogger.Write(String.Format("Error while Printing content list for container id: {0}", contid))
                    oLogger.Write(String.Format("Error Details: {0}", ex.ToString))
                End If
            End Try
        Next
    End Sub

End Class
