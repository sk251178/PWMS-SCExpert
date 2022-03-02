Public Class CreateLoad
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents TECL As Made4Net.WebControls.TableEditor
    Protected WithEvents lblLoadId As Made4Net.WebControls.FieldLabel
    Protected WithEvents txtLoadId As Made4Net.WebControls.TextBoxValidated
    Protected WithEvents lblLoc As Made4Net.WebControls.FieldLabel
    Protected WithEvents txtLocation As Made4Net.WebControls.TextBoxValidated
    Protected WithEvents lblStatus As Made4Net.WebControls.FieldLabel
    Protected WithEvents lblUnits As Made4Net.WebControls.FieldLabel
    Protected WithEvents txtUnits As Made4Net.WebControls.TextBoxValidated
    Protected WithEvents pnlCld As System.Web.UI.WebControls.Panel
    Protected WithEvents lblUOM As Made4Net.WebControls.FieldLabel
    Protected WithEvents ddUOM As Made4Net.WebControls.DropDownList
    Protected WithEvents lblHoldRc As Made4Net.WebControls.FieldLabel
    Protected WithEvents TEAB As Made4Net.WebControls.TableEditorActionBar
    <CLSCompliant(False)> Protected WithEvents TS As Made4Net.WebControls.TabStrip
    Protected WithEvents DTC As Made4Net.WebControls.DataTabControl
    Protected WithEvents Table1 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents Tbl As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents ddStatus As Made4Net.WebControls.DropDownList
    Protected WithEvents ddHold As Made4Net.WebControls.DropDownList
    Protected WithEvents AttribTbl As Made4Net.WebControls.Table
    Protected WithEvents lblNumOfLoads As Made4Net.WebControls.FieldLabel
    Protected WithEvents txtNumLoads As Made4Net.WebControls.TextBoxValidated


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
            ViewState.Add("_receipt", "")
            ViewState.Add("_receiptline", "")
            ViewState.Add("_sku", "")
            ViewState.Add("_consignee", "")
            setStatuses()
            setHoldReasonCode()
        End If
        setActionBarButtons()
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

    Private Sub TEAB_ItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles TEAB.ItemCommand
        Dim ds As New DataSet
        Dim dt As DataTable
        Dim ResponseMessage As String
        dt = CreateLoadTable()
        ds.Tables.Add(dt)
        dt = CreateAttributeTable()
        ds.Tables.Add(dt)

        Try
            If Convert.ToInt32(txtNumLoads.Value) = 1 Then
                Dim oRec As New Logic.Receiving(1, "CreateLoad", Made4Net.Shared.Util.DataSetToXMLSchema(ds), Made4Net.Shared.Util.DataSetToXMLData(ds), ResponseMessage)
            Else
                Dim oRec As New Logic.Receiving(Convert.ToInt32(txtNumLoads.Value), "CreateMultipleLoads", Made4Net.Shared.Util.DataSetToXMLSchema(ds), Made4Net.Shared.Util.DataSetToXMLData(ds), ResponseMessage)
            End If

            doClear(False)
            If ResponseMessage.Trim <> "" Then
                Screen1.Notify(ResponseMessage)
            End If
            TECL.RefreshData()

        Catch ex As Made4Net.Shared.M4NException
            Screen1.Warn(ex.GetErrMessage)
        Catch ex As WMS.Logic.LogicException
            Screen1.Warn(ex.Message)
        Catch ex As ApplicationException
            Screen1.Warn(ex.Message)
        End Try
        
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
            dr("LOADID") = txtLoadId.Value
            dr("ATTCAPTYPE") = WMS.Lib.CAPTURETYPES.INBOUND
            dr("DOCUMENT") = viewstate("_receipt")
            dr("DOCUMENTLINE") = viewstate("_receiptline")
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
                Case "int32"
                    dr("INTATTRIBUTE") = CType(AttribTbl.FindControl(attribrow("ATTRIBUTENAME")), Made4Net.WebControls.TextBoxValidated).Value
            End Select
            dt.Rows.Add(dr)
        Next
        Return dt
    End Function

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

        Dim dr As DataRow = dt.NewRow()
        dr("RECEIPT") = viewstate("_receipt")
        dr("RECEIPTLINE") = viewstate("_receiptline")
        dr("LOCATION") = txtLocation.Value
        dr("LOADID") = txtLoadId.Value
        dr("STATUS") = ddStatus.Value
        dr("HOLDRC") = ddHold.Value
        dr("UNITS") = txtUnits.Value
        dr("LOADUOM") = ddUOM.Value

        dt.Rows.Add(dr)
        Return dt
    End Function

    Private Sub doClear(Optional ByVal ClearAll As Boolean = True)
        txtLoadId.Value = ""
        txtNumLoads.Value = "1"
        If ClearAll Then
            txtLocation.Value = ""
            txtUnits.Value = ""
            ddStatus.Value = "Available"
            ddStatus_SelectedIndexChanged(Nothing, Nothing)
        End If
    End Sub

    Private Sub TECL_RecordSelected(ByVal sender As Made4Net.WebControls.TableEditor, ByVal e As Made4Net.WebControls.TableEditorEventArgs) Handles TECL.RecordSelected
        Dim tds As DataTable = TECL.CreateDataTableForSelectedRecord(False)
        doClear()
        ViewState.Item("_receipt") = tds.Rows(0)("RECEIPT")
        ViewState.Item("_receiptline") = tds.Rows(0)("RECEIPTLINE")
        ViewState.Item("_sku") = tds.Rows(0)("SKU")
        Try
            ViewState.Item("_consignee") = tds.Rows(0)("CONSIGNEE")
        Catch ex As Exception
            ViewState.Item("_consignee") = tds.Rows(0)("DEFAULT")
        End Try
        If WMS.Logic.Consignee.AutoGenerateLoadID(ViewState.Item("_consignee")) Then
            txtLoadId.Visible = False
            lblLoadId.Visible = False
        End If
        setUOM()
        SetInitialStatus()
        setAttributes()
    End Sub

    Private Sub setUOM()
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim Sql As String
        Sql = String.Format("Select UOM,DESCRIPTION from SKUUOM join CODELISTDETAIL on SKUUOM.UOM = CODELISTDETAIL.CODE where consignee = '{0}' and sku = '{1}' and CODELISTCODE = 'UOM'", ViewState.Item("_consignee"), ViewState.Item("_sku"))
        Made4Net.DataAccess.DataInterface.FillDataset(Sql, dt)
        ddUOM.Items.Clear()
        For Each dr In dt.Rows
            ddUOM.Items.Add(New ListItem(dr("DESCRIPTION"), dr("UOM")))
        Next
        dt.Dispose()
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

    Private Sub SetInitialStatus()
        Dim sql As String
        sql = String.Format("Select InitialStatus from SKU where consignee = '{0}' and sku = '{1}'", ViewState.Item("_consignee"), ViewState.Item("_sku"))
        Try
            ddStatus.SelectedValue = Convert.ToString(Made4Net.DataAccess.DataInterface.ExecuteScalar(sql))
            ddStatus_SelectedIndexChanged(Nothing, Nothing)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub setAttributes()
        Dim oSku As String = ViewState.Item("_sku")
        Dim oConsignee As String = ViewState.Item("_consignee")
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

    Protected Overrides Sub CreateChildControls()
        setAttributes()
    End Sub
End Class
