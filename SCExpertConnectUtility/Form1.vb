Imports System.Configuration
Imports ExpertObjectMapper
Imports System.Xml
Imports Made4Net.Shared
Imports SCExpertConnect
Imports System

Public Class Form1

    Private strFile As String = ""

    Private Sub btnExportOutbound_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExportOutbound.Click

        Dim oOutgoing As OutgoingEventProcessor = New OutgoingEventProcessor()
        Try
            If CheckValues(UtilityOperation.OutboundExport) Then
                oOutgoing.ProcessRequest(CreateOutboundExportMessage())
                MessageBox.Show("Export successful")
            End If
        Catch ex As Exception
            MessageBox.Show(ex.ToString())
        End Try
    End Sub

    'Added for RWMS-1383,RWMS-1452 and RWMS-1450

    Private Function CreateOutboundExportMessage() As QMsgSender
        Dim qSender As QMsgSender = New QMsgSender()
        qSender.Add("WAREHOUSE", txtWarehouse1.Text)
        qSender.Add("CONSIGNEE", cbConsignee1.SelectedValue)
        qSender.Add("DOCUMENT", txtOrder1.Text)
        qSender.Add("Action", "")
        'OrderShipped Event is being raised   
        qSender.Add("EVENT", 21)
        Return qSender
    End Function


    Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        'check whether multiple warehouses are available   
        'if multiple warehouses are available then redirect to WHSelect   
        'else Stay in the same page and get the warehouse and consignee from the database.
        ShowWareHouseSelection()
    End Sub

    Private Sub btnNext2_Click(sender As Object, e As EventArgs) Handles btnNext2.Click
        'show panel3 and bind the consignee   
        Panel1.Visible = True
        Panel2.Visible = False

        txtWarehouse1.Text = lbWarehouse2.SelectedItem(0).ToString()
        SetSelectedWareHouse(txtWarehouse1.Text)
        ShowConsigneeList()
    End Sub

#Region "Methods for warehouse and consignee"

    Private Sub ShowWareHouseSelection()
        Panel1.Visible = False
        Panel2.Visible = False
        Dim dt As DataTable = GetWareHouses()
        If dt.Rows.Count > 1 Then
            'show warehouses for user selection
            Panel2.Visible = True
            ShowWHList(dt)
        ElseIf dt.Rows.Count = 1 Then
            Panel1.Visible = True

            'show warehouse in the textbox and bind the consignee dropdown
            txtWarehouse1.Text = dt.Rows(0)("warehouseid")

            SetSelectedWareHouse(txtWarehouse1.Text)
            ShowConsigneeList()

        End If
        'Else
        '    Panel1.Visible = True
        '    Panel2.Visible = False

        '    cbConsignee1.DisplayMember = "Text"
        '    cbConsignee1.ValueMember = "Value"

        '    'get the consignee
        '    Dim dtConsignee As New DataTable
        '    Dim SqlConsignee As String = String.Format("select * from CONSIGNEE")
        '    Made4Net.DataAccess.DataInterface.FillDataset(SqlConsignee, dtConsignee)

        '    Dim tb As New DataTable
        '    tb.Columns.Add("Text", GetType(String))
        '    tb.Columns.Add("Value", GetType(String))

        '    If dtConsignee.Rows.Count > 0 Then
        '        For Each drConsignee In dtConsignee.Rows
        '            tb.Rows.Add(drConsignee("consignee"), drConsignee("consignee"))
        '        Next
        '        cbConsignee1.DataSource = tb
        '    Else

        '    End If
        'End If
    End Sub

    Private Sub SetSelectedWareHouse(ByVal strWareHouseId As String)
        Warehouse.setCurrentWarehouse(strWareHouseId)
        Made4Net.DataAccess.DataInterface.ConnectionName = Warehouse.WarehouseConnection
    End Sub

    Private Sub ShowConsigneeList()

        Dim dtConsignee As DataTable = GetConsigneesForSelectedWareHouse()

        cbConsignee1.DisplayMember = "Text"
        cbConsignee1.ValueMember = "Value"

        Dim tb As New DataTable
        tb.Columns.Add("Text", GetType(String))
        tb.Columns.Add("Value", GetType(String))

        If dtConsignee.Rows.Count > 0 Then
            For Each drConsignee In dtConsignee.Rows
                tb.Rows.Add(drConsignee("consignee"), drConsignee("consignee"))
            Next
            cbConsignee1.DataSource = tb
        Else

        End If
        dtConsignee.Dispose()
    End Sub

    Private Sub ShowWHList(ByVal dtWarehouses As DataTable)
        Dim dr As DataRow
        lbWarehouse2.DisplayMember = "Text"
        lbWarehouse2.ValueMember = "Value"

        'show all the warehouses in a listbox
        Dim tb As New DataTable
        tb.Columns.Add("Text", GetType(String))
        tb.Columns.Add("Value", GetType(String))
        For Each dr In dtWarehouses.Rows
            tb.Rows.Add(dr("warehouseid"))
        Next
        lbWarehouse2.DataSource = tb
    End Sub

    Private Function GetWareHouses() As DataTable
        Dim SQL As String = String.Format("select * from warehouse")
        Dim dt As New DataTable
        Made4Net.DataAccess.DataInterface.FillDataset(SQL, dt, False, "Made4NetSchema")
        Return dt
    End Function

    Private Function GetConsigneesForSelectedWareHouse() As DataTable
        'get the consignee
        Dim dtConsignee As New DataTable
        Dim SqlConsignee As String = String.Format("select * from CONSIGNEE")
        Made4Net.DataAccess.DataInterface.FillDataset(SqlConsignee, dtConsignee, False, Made4Net.DataAccess.DataInterface.ConnectionName)
        Return dtConsignee
    End Function

#End Region

    Private Function CheckValues(ByVal enOperation As UtilityOperation) As Boolean
        Dim blnReturn As Boolean = True
        If (txtWarehouse1.Text.Trim.Length = 0) Or (cbConsignee1.Items.Count = 0) Then
            MessageBox.Show("Warehouse/Consignee cannot be blank")
            blnReturn = False
        Else
            Select Case enOperation
                Case UtilityOperation.OutboundExport
                    txtOrder1.Text = txtOrder1.Text.Trim
                    If (txtOrder1.Text.Length = 0) Then
                        MessageBox.Show("OrderID cannot be blank")
                        blnReturn = False
                    End If
                Case UtilityOperation.ReceiptClose
                    txtReceipt1.Text = txtReceipt1.Text.Trim
                    If (txtReceipt1.Text.Length = 0) Then
                        MessageBox.Show("ReceiptID cannot be blank")
                        blnReturn = False
                    End If
                Case Else
                    MessageBox.Show("Invalid Operation")
                    blnReturn = False
            End Select
        End If
        Return blnReturn
    End Function

    Private Sub btnCloseReceipt_Click(sender As Object, e As EventArgs) Handles btnCloseReceipt.Click
        Try
            If CheckValues(UtilityOperation.ReceiptClose) Then
                WMS.Logic.Warehouse.setCurrentWarehouse(txtWarehouse1.Text)
                Dim oReceipt As WMS.Logic.ReceiptHeader = New WMS.Logic.ReceiptHeader(txtReceipt1.Text)
                If oReceipt.STATUS = WMS.Lib.Statuses.Receipt.CLOSED Then
                    oReceipt.RaiseReceiptCloseEvent("scexpertutil")
                    MessageBox.Show("Receipt close event generated successfully")
                Else
                    MessageBox.Show("Receipt status should be closed to raise receipt close event")
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.ToString())
        End Try
    End Sub

    Private Enum UtilityOperation
        OutboundExport = 0
        ReceiptClose = 1
    End Enum

    Private Sub btnExportCompany_Click(sender As Object, e As EventArgs) Handles btnExportCompany.Click
        If String.IsNullOrEmpty(txtCompany.Text) Or String.IsNullOrEmpty(txtConsinee.Text) Or String.IsNullOrEmpty(txtCompanyType.Text) Then
            Return
        End If
        Dim obj As ObjectProcessor = New ObjectProcessor()
        Dim qSender As QMsgSender = New QMsgSender()
        qSender.Add("CONSIGNEE", txtConsinee.Text)
        qSender.Add("DOCUMENT", txtCompany.Text)
        qSender.Add("NOTES", txtCompanyType.Text)
        qSender.Add("Action", "")
        Dim comp As XmlDocument = obj.ExportCompany("", qSender, Nothing)
        comp.Save(IO.Path.Combine(IO.Path.GetDirectoryName(Reflection.Assembly.GetExecutingAssembly.Location), txtCompany.Text + ".txt"))
    End Sub
End Class
