Imports WMS.Logic
Imports WMS.Lib

Public Class AppPagePrint
    Inherits AppPageProcessor

#Region "Enums"

    Private Enum ResponseCode
        Printed
        [Error]
        NoUserLoggedIn = 99
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
        PrintMessageContent(oLogger)
        If Not ValidateUserLoggedIn() Then
            If Not oLogger Is Nothing Then
                oLogger.Write(String.Format("User is not logged in. Please sign in again."))
            End If
            Me._responseCode = ResponseCode.NoUserLoggedIn
            Me._responseText = "User is not logged in"
            Me.FillSingleRecord(oLogger)
            Return _resp
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
        Return Nothing
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
            If Not Session("PCKPicklist") Is Nothing Then 'AndAlso CType(Session("PCKPicklist"), WMS.Logic.Picklist).PicklistID.Equals(_msg(0).Item("PRINT ID").FieldValue, StringComparison.OrdinalIgnoreCase) Then
                Dim oPicklist As WMS.Logic.Picklist = Session("PCKPicklist")
                If pLabelType = LabelType.PickingLabel Then
                    oPicklist.PrintPickLabels(prntr.PrinterQName)
                ElseIf pLabelType = LabelType.ShippingLabel Then
                    oPicklist.PrintShipLabels(prntr.PrinterQName)
                End If
            ElseIf Not Session("PARPCKPicklist") Is Nothing Then 'AndAlso CType(Session("PARPCKPicklist"), WMS.Logic.ParallelPicking).ParallelPickId.Equals(_msg(0).Item("PRINT ID").FieldValue, StringComparison.OrdinalIgnoreCase) Then
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
            Me._responseText = ex.Description
            Me.FillSingleRecord(oLogger)
            Return _resp
        Catch ex As Exception
            If Not oLogger Is Nothing Then
                oLogger.Write("Error while Printing: " & ex.ToString)
            End If
            Me._responseCode = ResponseCode.Error
            Me._responseText = ex.Message
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
            Dim oCont As New WMS.Logic.Container(_msg(0).Item("PRINT ID").FieldValue, True)
            Dim sContentListFormat As String = GetContentListReportFormat(oCont.ContainerId, oLogger)
            oCont.PrintContentList(prntr, 0, Made4Net.Shared.Authentication.User.GetCurrentUser.UserName, sContentListFormat)
        Catch ex As Made4Net.Shared.M4NException
            If Not oLogger Is Nothing Then
                oLogger.Write("Error while Printing: " & ex.GetErrMessage(0))
            End If
            Me._responseCode = ResponseCode.Error
            Me._responseText = ex.Description
            Me.FillSingleRecord(oLogger)
            Return _resp
        Catch ex As Exception
            If Not oLogger Is Nothing Then
                oLogger.Write("Error while Printing: " & ex.ToString)
            End If
            Me._responseCode = ResponseCode.Error
            Me._responseText = ex.Message
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
            Me._responseText = ex.Description
            Me.FillSingleRecord(oLogger)
            Return _resp
        Catch ex As Exception
            If Not oLogger Is Nothing Then
                oLogger.Write("Error while Printing: " & ex.ToString)
            End If
            Me._responseCode = ResponseCode.Error
            Me._responseText = ex.Message
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
        Dim contid As String = String.Empty
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

    Private Function GetContentListReportFormat(ByVal sContaienrId As String, ByVal oLogger As WMS.Logic.LogHandler)
        Dim sDefaultFormat As String = "ContentList"
        Dim sPicklistId = String.Empty, sContentListFormat As String = String.Empty
        Dim sSql As String = String.Format("select top 1 isnull(PICKLIST,'') from PICKDETAIL where TOCONTAINER = '{0}'", sContaienrId)
        sPicklistId = Made4Net.DataAccess.DataInterface.ExecuteScalar(sSql)
        If sPicklistId = String.Empty Then
            Return sDefaultFormat
        End If
        Dim oPicklist As New WMS.Logic.Picklist(sPicklistId)
        If oPicklist Is Nothing Then
            Return sDefaultFormat
        End If
        sContentListFormat = oPicklist.GetContentListReoprtName()
        If sContentListFormat <> String.Empty Then
            If Not oLogger Is Nothing Then
                oLogger.Write(String.Format("Contect list document format {0} will be used for current container.", sContentListFormat))
            End If
            Return sContentListFormat
        End If
        Return sDefaultFormat
    End Function

End Class