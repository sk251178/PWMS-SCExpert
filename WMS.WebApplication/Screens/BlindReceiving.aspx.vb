Imports Made4Net.DataAccess
Imports Made4Net.Shared
Imports WMS.Logic

Public Class BlindReceiving
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents TEBlindRec As Made4Net.WebControls.TableEditor
    <CLSCompliant(False)> Protected WithEvents TS As Made4Net.WebControls.TabStrip
    Protected WithEvents TEAB As Made4Net.WebControls.TableEditorActionBar
    Protected WithEvents lblLoadId As Made4Net.WebControls.FieldLabel
    Protected WithEvents txtLoadId As Made4Net.WebControls.TextBoxValidated
    Protected WithEvents lblLoc As Made4Net.WebControls.FieldLabel
    Protected WithEvents txtLocation As Made4Net.WebControls.TextBoxValidated
    Protected WithEvents lblUnits As Made4Net.WebControls.FieldLabel
    Protected WithEvents txtUnits As Made4Net.WebControls.TextBoxValidated
    Protected WithEvents lblNumOfLoads As Made4Net.WebControls.FieldLabel
    Protected WithEvents txtNumLoads As Made4Net.WebControls.TextBoxValidated
    Protected WithEvents lblUOM As Made4Net.WebControls.FieldLabel
    Protected WithEvents ddUOM As Made4Net.WebControls.DropDownList
    Protected WithEvents lblStatus As Made4Net.WebControls.FieldLabel
    Protected WithEvents ddStatus As Made4Net.WebControls.DropDownList
    Protected WithEvents lblHoldRc As Made4Net.WebControls.FieldLabel
    Protected WithEvents ddHold As Made4Net.WebControls.DropDownList
    Protected WithEvents AttribTbl As Made4Net.WebControls.Table
    Protected WithEvents pnlCld As System.Web.UI.WebControls.Panel
    Protected WithEvents DTC As Made4Net.WebControls.DataTabControl
    Protected WithEvents Tbl As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents Fieldlabel1 As Made4Net.WebControls.FieldLabel
    Protected WithEvents Textboxvalidated1 As Made4Net.WebControls.TextBoxValidated
    Protected WithEvents Fieldlabel2 As Made4Net.WebControls.FieldLabel
    Protected WithEvents Textboxvalidated2 As Made4Net.WebControls.TextBoxValidated
    Protected WithEvents lblReceipt As Made4Net.WebControls.FieldLabel
    Protected WithEvents txtReceipt As Made4Net.WebControls.TextBoxValidated
    Protected WithEvents lblConsignee As Made4Net.WebControls.FieldLabel
    Protected WithEvents txtConsignee As Made4Net.WebControls.TextBoxValidated
    Protected WithEvents lblSku As Made4Net.WebControls.FieldLabel
    Protected WithEvents txtSku As Made4Net.WebControls.TextBoxValidated
    Protected WithEvents tblDetails As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents btnOK As Made4Net.WebControls.Button

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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not IsPostBack Then
            ViewState.Add("ReceiptId", "")
            ViewState.Add("_attributesdt", Nothing)
            setStatuses()
            setHoldReasonCode()
            setUOM()
        End If
        setActionBarButtons()
    End Sub

    Private Sub btnSendReceipt_ServerClick(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim ReceiptId As String = txtReceipt.Value
        If ReceiptId.Trim <> "" Then
            Dim SQL As String = String.Format("select count(*) from RECEIPTHEADER where receipt = '{0}'", ReceiptId)
            Dim count As Integer = DataInterface.ExecuteScalar(SQL)
            If count < 1 Then
                Screen1.Warn("Receipt does not exist")
            Else
                ViewState.Add("ReceiptId", ReceiptId)
                DTC.Visible = True
            End If
        Else
            'create new receiptId
            ReceiptId = ""
            ViewState.Add("ReceiptId", ReceiptId)
            DTC.Visible = True
        End If
        ddStatus_SelectedIndexChanged(Nothing, Nothing)
    End Sub

    Private Sub TEAB_ItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles TEAB.ItemCommand
        Dim ReceiptId As String = txtReceipt.Value
        If ReceiptId.Trim <> "" Then
            ViewState("ReceiptId") = ReceiptId
        End If
        Dim ds As New DataSet
        Dim dt As DataTable
        Dim ResponseMessage As String
        setAttributes()
        dt = CreateLoadTable()
        ds.Tables.Add(dt)
        dt = CreateAttributeTable()
        ds.Tables.Add(dt)
        Dim arr() As String

        Try
            Dim oRec As New Logic.ReceiptHeader(sender, "CreateBlindReceiving", Made4Net.Shared.Util.DataSetToXMLSchema(ds), Made4Net.Shared.Util.DataSetToXMLData(ds), ResponseMessage)

            arr = ResponseMessage.Split("@")
            ResponseMessage = arr(0)
            If ResponseMessage.Trim <> "" Then
                Screen1.Notify(ResponseMessage)
            End If
            ds.Tables("LOADS").Rows(0)("RECEIPT") = arr(1)
            txtReceipt.Value = ds.Tables("LOADS").Rows(0)("RECEIPT")
            doClear()
        Catch ex As Made4Net.Shared.M4NException
            Screen1.Warn(ex.GetErrMessage)
        Catch ex As WMS.Logic.LogicException
            Screen1.Warn(ex.Message)
        Catch ex As ApplicationException
            Screen1.Warn(ex.Message)
        End Try
    End Sub

    Private Sub setActionBarButtons()
        With TEAB
            .Button("New").Enabled = False
            .Button("Edit").Enabled = False
            .Button("MultiEdit").Enabled = False
            .Button("View").Enabled = False
            .Button("Find").Enabled = False
            .Button("Cancel").Enabled = False
            .Button("Delete").Enabled = False
        End With
    End Sub

    Private Sub setStatuses()
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim Sql As String
        Sql = String.Format("Select CODE,DESCRIPTION from CODELISTDETAIL WHERE CODELISTCODE = '{0}'", "INVHOLDSTT")
        Made4Net.DataAccess.DataInterface.FillDataset(Sql, dt)
        ddStatus.Items.Add("Available")
        For Each dr In dt.Rows
            ddStatus.Items.Add(New ListItem(dr("DESCRIPTION"), dr("CODE")))
        Next
        ddStatus.Items.Add("Limbo")
        dt.Dispose()
    End Sub

    Private Sub setHoldReasonCode()
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim Sql As String
        Sql = String.Format("Select CODE,DESCRIPTION from CODELISTDETAIL WHERE CODELISTCODE = '{0}'", "INVHOLDRC")
        Made4Net.DataAccess.DataInterface.FillDataset(Sql, dt)
        For Each dr In dt.Rows
            ddHold.Items.Add(New ListItem(dr("DESCRIPTION"), dr("CODE")))
        Next
        dt.Dispose()
    End Sub

    Private Sub setUOM()
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim Sql As String
        Sql = "Select distinct UOM,DESCRIPTION from SKUUOM join CODELISTDETAIL on SKUUOM.UOM = CODELISTDETAIL.CODE where CODELISTCODE = 'UOM'"
        Made4Net.DataAccess.DataInterface.FillDataset(Sql, dt)
        ddUOM.Items.Clear()
        For Each dr In dt.Rows
            ddUOM.Items.Add(New ListItem(dr("DESCRIPTION"), dr("UOM")))
        Next
        dt.Dispose()
    End Sub

    Private Sub ddStatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddStatus.SelectedIndexChanged
        If ddStatus.SelectedValue = "Available" Then
            Try
                ddHold.SelectedValue = ""
            Catch ex As Exception
                ddHold.Items.Insert(0, New ListItem("", ""))
                ddHold.SelectedValue = ""
            End Try
            ddHold.Enabled = False
        Else
            ddHold.Enabled = True
            ddHold.Items.Remove("")
        End If
    End Sub

    Private Function CreateAttributeTable() As DataTable
        Dim dt As New DataTable("LOADATT")
        dt.Columns.Add("LOADID", System.Type.GetType("System.String"))
        dt.Columns.Add("ATTCAPTYPE", System.Type.GetType("System.String"))
        dt.Columns.Add("DOCUMENT", System.Type.GetType("System.String"))
        dt.Columns.Add("DOCUMENTLINE", System.Type.GetType("System.Int32"))
        dt.Columns.Add("ATTRIBUTENAME", System.Type.GetType("System.String"))
        dt.Columns.Add("TXTATTRIBUTE", System.Type.GetType("System.String"))
        dt.Columns.Add("INTATTRIBUTE", System.Type.GetType("System.Int32"))
        dt.Columns.Add("DATEATTRIBUTE", System.Type.GetType("System.String"))
        Dim dr As DataRow
        Dim attribrow As DataRow
        For Each attribrow In ViewState.Item("_attributesdt").Rows
            dr = dt.NewRow()
            dr("LOADID") = ""
            dr("ATTCAPTYPE") = WMS.Lib.CAPTURETYPES.INBOUND
            dr("DOCUMENT") = viewstate("ReceiptId")
            dr("DOCUMENTLINE") = ""
            dr("ATTRIBUTENAME") = attribrow("ATTRIBUTENAME")
            dr("TXTATTRIBUTE") = System.DBNull.Value
            dr("INTATTRIBUTE") = System.DBNull.Value
            dr("DATEATTRIBUTE") = System.DBNull.Value
            Select Case Convert.ToString(attribrow("ATTRIBUTETYPE")).ToLower()
                Case "string"
                    dr("TXTATTRIBUTE") = CType(AttribTbl.FindControl(attribrow("ATTRIBUTENAME")), Made4Net.WebControls.TextBoxValidated).Value
                Case "datetime"
                    dr("DATEATTRIBUTE") = Convert.ToDateTime(CType(AttribTbl.FindControl(attribrow("ATTRIBUTENAME")), Made4Net.WebControls.DateBox).Value)
                Case "boolean"
                    dr("INTATTRIBUTE") = Convert.ToInt32(CType(AttribTbl.FindControl(attribrow("ATTRIBUTENAME")), Made4Net.WebControls.CheckBox).Checked)
                Case "int"
                    dr("INTATTRIBUTE") = CType(AttribTbl.FindControl(attribrow("ATTRIBUTENAME")), Made4Net.WebControls.TextBoxValidated).Value
            End Select
            dt.Rows.Add(dr)
        Next
        Return dt
    End Function

    Private Sub setAttributes()
        Dim oSku As String = txtSku.Value
        Dim oConsignee As String = txtConsignee.Value
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim sql As String = String.Format("Select skuclsloadatt.attributename, skuclsloadatt.attributetype " & _
                "From skuclsloadatt join sku on sku.classname = skuclsloadatt.classname " & _
                "where attcapin = 1 and sku.sku = '{0}' and sku.consignee = '{1}'", oSku, oConsignee)
        Made4Net.DataAccess.DataInterface.FillDataset(sql, dt)
        ViewState.Item("_attributesdt") = dt
        AttribTbl.Rows.Clear()
        AttribTbl.EnableViewState = True
        If dt.Rows.Count > 0 Then
            AttribTbl.AddRow()
            AttribTbl.AddCell(New Made4Net.WebControls.FieldLabel("Load Attributes"))
            AttribTbl.AddedCell.ColumnSpan = 4
        End If
        Dim Even As Boolean = False
        'Dim tblCell As TableCell
        For Each dr In dt.Rows
            Even = Not (Even)
            If Even Then
                AttribTbl.AddRow()
                AttribTbl.AddedRow.EnableViewState = True
            End If
            AttribTbl.AddCell(New Made4Net.WebControls.FieldLabel(dr("attributename")))
            Select Case Convert.ToString(dr("attributetype")).ToLower()
                Case "datetime"
                    Dim db As New Made4Net.WebControls.DateBox
                    db.Required = True
                    db.ID = dr("attributename")
                    db.EnableViewState = True
                    AttribTbl.AddCell(db)
                Case "string"
                    Dim tb As New Made4Net.WebControls.TextBoxValidated
                    tb.Required = True
                    tb.EnableViewState = True
                    tb.ID = dr("attributename")
                    AttribTbl.AddCell(tb)
                Case "boolean"
                    Dim bt As New Made4Net.WebControls.CheckBox
                    bt.ID = dr("attributename")
                    bt.EnableViewState = True
                    AttribTbl.AddCell(bt)
                Case "int32"
                    Dim it As New Made4Net.WebControls.TextBoxValidated
                    it.CheckDataType = True
                    it.EnableViewState = True
                    it.DataType = ValidationDataType.Integer
                    it.Required = True
                    it.ID = dr("attributename")
                    AttribTbl.AddCell(it)
            End Select
            AttribTbl.AddedCell.EnableViewState = True
        Next
    End Sub

    Private Function CreateLoadTable() As DataTable
        Dim dt As New DataTable("LOADS")
        dt.Columns.Add("RECEIPT", System.Type.GetType("System.String"))
        dt.Columns.Add("RECEIPTLINE", System.Type.GetType("System.Int32"))
        dt.Columns.Add("LOCATION", System.Type.GetType("System.String"))
        dt.Columns.Add("LOADID", System.Type.GetType("System.String"))
        dt.Columns.Add("STATUS", System.Type.GetType("System.String"))
        dt.Columns.Add("HOLDRC", System.Type.GetType("System.String"))
        dt.Columns.Add("UNITS", System.Type.GetType("System.Int32"))
        dt.Columns.Add("LOADUOM", System.Type.GetType("System.String"))
        dt.Columns.Add("NUMLOADS", System.Type.GetType("System.Int32"))
        dt.Columns.Add("SKU", System.Type.GetType("System.String"))
        dt.Columns.Add("CONSIGNEE", System.Type.GetType("System.String"))

        Dim dr As DataRow = dt.NewRow()
        dr("RECEIPT") = viewstate("ReceiptId")
        'dr("RECEIPTLINE") = ""
        dr("LOCATION") = txtLocation.Value
        dr("SKU") = txtSku.Value
        dr("CONSIGNEE") = txtConsignee.Value
        dr("LOADID") = ""
        dr("STATUS") = ddStatus.Value
        dr("HOLDRC") = ddHold.Value
        dr("UNITS") = txtUnits.Value
        dr("LOADUOM") = ddUOM.Value
        dr("NUMLOADS") = Convert.ToInt32(txtNumLoads.Value)

        dt.Rows.Add(dr)
        Return dt
    End Function

    Private Sub doClear(Optional ByVal ClearAll As Boolean = True)
        txtNumLoads.Value = "1"
        If ClearAll Then
            txtLocation.Value = ""
            txtUnits.Value = ""
            txtSku.Value = ""
            txtConsignee.Value = ""
            ddStatus.Value = "Available"
            ddStatus_SelectedIndexChanged(Nothing, Nothing)
        End If
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Dim ReceiptId As String = txtReceipt.Value
        If ReceiptId.Trim <> "" Then
            Dim SQL As String = String.Format("select count(*) from RECEIPTHEADER where receipt = '{0}'", ReceiptId)
            Dim count As Integer = DataInterface.ExecuteScalar(SQL)
            If count < 1 Then
                Screen1.Warn("Receipt does not exist")
            Else
                ViewState.Add("ReceiptId", ReceiptId)
                DTC.Visible = True
            End If
        Else
            'create new receiptId
            ReceiptId = ""
            ViewState.Add("ReceiptId", ReceiptId)
            DTC.Visible = True
        End If
        ddStatus_SelectedIndexChanged(Nothing, Nothing)
    End Sub
End Class