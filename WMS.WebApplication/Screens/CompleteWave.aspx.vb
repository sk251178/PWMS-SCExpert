Imports WMS.Logic
Imports RWMS.Logic

Public Class CompleteWave
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    <CLSCompliant(False)> Protected WithEvents TS As Made4Net.WebControls.TabStrip
    Protected WithEvents DTC As Made4Net.WebControls.DataTabControl
    Protected WithEvents Table1 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents Tbl As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents TECompleteWave As Made4Net.WebControls.TableEditor
    Protected WithEvents TECompleteShip As Made4Net.WebControls.TableEditor
    Protected WithEvents TECompleteOrder As Made4Net.WebControls.TableEditor

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
        Dim ts As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Dim UserId As String = Common.GetCurrentUser()
        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        Select Case CommandName.ToLower
            Case "completeorder"
                Dim outorheader As RWMS.Logic.OutboundOrderHeader
                'Dim outorheader As WMS.Logic.OutboundOrderHeader
                'CancelException
                For Each dr In ds.Tables(0).Rows
                    WebAppUtil.processOrderExceptionLines(dr("CONSIGNEE"), dr("ORDERID"))
                    If dr("STATUS").ToString.ToUpper = "LOADING" Then
                        outorheader = New RWMS.Logic.OutboundOrderHeader(dr("CONSIGNEE"), dr("ORDERID"))
                        outorheader.Complete(UserId)
                        'Else
                        '    outorheader = New WMS.Logic.OutboundOrderHeader(dr("CONSIGNEE"), dr("ORDERID"))
                        '    outorheader.Complete(UserId)
                    End If
                Next
                'completeorder
                Dim obj As WMS.Logic.OutboundOrderHeader = New WMS.Logic.OutboundOrderHeader(Sender, CommandName, XMLSchema, XMLData, Message)
            Case "complete"
                Dim wave As String
                For Each dr In ds.Tables(0).Rows
                    wave = dr("WAVE")
                    WebAppUtil.processOrderExceptionLines(dr("WAVE"))
                    ' CompleteOrderLines(wave)
                    CompletePicklist(wave)
                    CompleteOrder(wave)
                Next
                Dim obj As WMS.Logic.Wave = New WMS.Logic.Wave(Sender, CommandName, XMLSchema, XMLData, Message)

        End Select
    End Sub

    Public Shared Sub CompleteOrder(ByVal wave As String)
        Dim UserId As String = Common.GetCurrentUser()
        Try
            'Dim sql As String = "select CONSIGNEE, ORDERID, sum(QTYMODIFIED - QTYPICKED - QTYALLOCATED - QTYLOADED -  QTYPACKED - QTYSHIPPED -  QTYSTAGED - QTYVERIFIED) sumqty from OUTBOUNDORDETAIL where  consignee + orderid in (select distinct consignee + orderid from WAVEDETAIL where WAVE  = '{0}') group by CONSIGNEE , ORDERID,CONSIGNEE + ORDERID having sum(QTYMODIFIED - QTYPICKED - QTYALLOCATED - QTYLOADED -  QTYPACKED - QTYSHIPPED -  QTYSTAGED - QTYVERIFIED)=0"
            Dim sql As String = "select distinct consignee, orderid from WAVEDETAIL where WAVE  = '{0}'"
            sql = String.Format(sql, wave)
            Dim dt As New DataTable
            Dim dr As DataRow

            Made4Net.DataAccess.DataInterface.FillDataset(sql, dt)

            For Each dr In dt.Rows
                Dim outorheader As RWMS.Logic.OutboundOrderHeader

                outorheader = New RWMS.Logic.OutboundOrderHeader(dr("CONSIGNEE"), dr("ORDERID"))
                outorheader.Complete(UserId)

                'Dim oh As New WMS.Logic.OutboundOrderHeader(dr("CONSIGNEE"), dr("ORDERID"), True)
                'oh.Complete(UserId)
                'Dim nextStatus As String = oh.getOrderStatusByActivity()
                'If Not String.IsNullOrEmpty(nextStatus) Then
                '    oh.setStatus(nextStatus, WMS.Logic.GetCurrentUser)
                'End If
            Next

        Catch ex As Exception
        End Try
    End Sub

    Public Shared Sub CompletePicklist(ByVal wave As String)
        Dim UserId As String = Common.GetCurrentUser()
        Try
            Dim sql As String = "select distinct picklist,picklistline,status from pickdetail where (status = 'PARTPICKED' or status = 'PLANNED' or status = 'RELEASED') and consignee + orderid in (select distinct  consignee + orderid from WAVEDETAIL where WAVE  = '{0}')"
            sql = String.Format(sql, wave)
            Dim dt As New DataTable
            Dim dr As DataRow

            Made4Net.DataAccess.DataInterface.FillDataset(sql, dt)

            For Each dr In dt.Rows
                Dim pDet As New WMS.Logic.PicklistDetail(dr("picklist"), dr("picklistline"), True)
                pDet.CompleteDetail(UserId)
            Next

            sql = "select distinct picklist from pickdetail where consignee + orderid in (select distinct  consignee + orderid from WAVEDETAIL where WAVE  = '{0}')"
            sql = String.Format(sql, wave)
            dt = New DataTable

            Made4Net.DataAccess.DataInterface.FillDataset(sql, dt)

            For Each dr In dt.Rows
                Dim pl As New WMS.Logic.Picklist(dr("picklist"))
                If (pl.isCanceled Or pl.isCompleted) Then pl.CompleteList(UserId, Nothing)
            Next

        Catch ex As Exception
        End Try
    End Sub

    Public Sub CompleteOrderLines(ByVal wave As String)
        Dim UserId As String = Common.GetCurrentUser()
        Try
            Dim sql As String = "select consignee,orderid,ORDERLINE from OUTBOUNDORDETAIL  where consignee + orderid in (select distinct  consignee + orderid from WAVEDETAIL where WAVE  = '{0}')"
            sql = String.Format(sql, wave)
            Dim dt As New DataTable
            Dim dr As DataRow

            Made4Net.DataAccess.DataInterface.FillDataset(sql, dt)

            For Each dr In dt.Rows
                Dim oDet As New WMS.Logic.OutboundOrderHeader.OutboundOrderDetail(dr("CONSIGNEE"), dr("ORDERID"), dr("ORDERLINE"))
                oDet.Complete(UserId)
            Next

        Catch ex As Exception
        End Try
    End Sub

    'Public Function CancelOrderLines2(ByVal wave As String)
    '    Dim UserId As String = Common.GetCurrentUser()
    '    Try
    '        Dim W As New WMS.Logic.Wave(wave)
    '        For Each wDet As WMS.Logic.WaveDetail In W.WaveDetails
    '            Dim oDet As New WMS.Logic.OutboundOrderHeader.OutboundOrderDetail(wDet.CONSIGNEE, wDet.ORDERID, wDet.ORDERLINE)
    '            oDet.Complete(UserId)
    '        Next
    '    Catch ex As Exception
    '    End Try
    'End Function
    <CLSCompliant(False)>
    Public Shared Sub CancelExceptionWave(ByVal outorheader As WMS.Logic.OutboundOrderHeader)
        Dim UserId As String = Common.GetCurrentUser()
        Try
            outorheader.CancelExceptions(UserId)
        Catch ex As Exception
        End Try
    End Sub

    <CLSCompliant(False)>
    Public Shared Sub CancelExceptionOrder(ByVal outorheader As WMS.Logic.OutboundOrderHeader)
        Dim UserId As String = Common.GetCurrentUser()
        Try
            outorheader.CancelExceptions(UserId)
        Catch ex As Exception
        End Try
    End Sub

#End Region


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub TECompleteWave_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TECompleteWave.CreatedChildControls
        With TECompleteWave.ActionBar
            .AddSpacer()

            .AddExecButton("CompleteWave", "CompleteWave", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnMultiEdit"))
            With .Button("CompleteWave")
                '.ObjectDLL = "WMS.Logic.dll"
                '.ObjectName = "WMS.Logic.Wave"
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.CompleteWave"
                .CommandName = "Complete"   'there is already a method called complete,so use it.
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .ConfirmRequired = True
                .ConfirmMessage = "Are you sure?"
            End With
        End With
    End Sub

    Private Sub TECompleteWave_AfterItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles TECompleteWave.AfterItemCommand
        If e.CommandName = "CompleteWave" Then
            TECompleteWave.RefreshData()
        End If
    End Sub

    Private Sub TECompleteShip_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TECompleteShip.CreatedChildControls
        With TECompleteShip.ActionBar
            .AddSpacer()

            .AddExecButton("CompleteShip", "CompleteShip", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnMultiEdit"))
            With .Button("CompleteShip")
                .ObjectDLL = "WMS.Logic.dll"
                .ObjectName = "WMS.Logic.Shipment"
                .CommandName = "CompleteShip"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .ConfirmRequired = True
                .ConfirmMessage = "Are you sure?"
            End With
        End With
    End Sub

    Private Sub TECompleteShip_AfterItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles TECompleteShip.AfterItemCommand
        If e.CommandName = "CompleteShip" Then
            TECompleteShip.RefreshData()
        End If
    End Sub

    Private Sub TECompleteOrder_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TECompleteOrder.CreatedChildControls
        With TECompleteOrder.ActionBar
            .AddSpacer()

            .AddExecButton("CompleteOrder", "CompleteOrder", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnMultiEdit"))
            With .Button("CompleteOrder")
                '.ObjectDLL = "WMS.Logic.dll"
                '.ObjectName = "WMS.Logic.OutboundOrderHeader"
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.CompleteWave"
                .CommandName = "CompleteOrder"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .ConfirmRequired = True
                .ConfirmMessage = "Are you sure?"
            End With
        End With
    End Sub

    Private Sub TECompleteOrder_AfterItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles TECompleteOrder.AfterItemCommand
        If e.CommandName = "CompleteOrder" Then
            TECompleteOrder.RefreshData()
        End If
    End Sub

End Class