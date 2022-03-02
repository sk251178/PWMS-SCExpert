Imports WMS.Logic
Imports Made4Net.Shared
Imports Made4Net.Shared.Conversion

Public Class YardEntry
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    <CLSCompliant(False)> Protected WithEvents TS As Made4Net.WebControls.TabStrip
    Protected WithEvents DC1 As Made4Net.WebControls.DataConnector
    Protected WithEvents DTC As Made4Net.WebControls.DataTabControl
    Protected WithEvents Table1 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents Tbl As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents TEAssignShipments As Made4Net.WebControls.TableEditor
    Protected WithEvents TEYardEntry As Made4Net.WebControls.TableEditor
    Protected WithEvents TEAssignReceipts As Made4Net.WebControls.TableEditor
    Protected WithEvents TEYardEntryShipments As Made4Net.WebControls.TableEditor
    Protected WithEvents TEYardEntryReceipts As Made4Net.WebControls.TableEditor
    Protected WithEvents DC2 As Made4Net.WebControls.DataConnector
    Protected WithEvents Tableeditor1 As Made4Net.WebControls.TableEditor

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
        Return Screen1.LoadPageStateFromPersistenceMedium()
    End Function

    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        Screen1.SavePageStateToPersistenceMedium(viewState, Me.Page, Screen1.ScreenID)
    End Sub

#End Region


#Region "CTor"

    Public Sub New()

    End Sub

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As New DataSet
        Dim dr As DataRow
        Dim UserId As String = Common.GetCurrentUser()
        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        Select Case CommandName
            Case "CreateNewEntry"
                dr = ds.Tables(0).Rows(0)
                Dim oYardEntry As New WMS.Logic.YardEntry
                oYardEntry.Create(Convert.ReplaceDBNull(dr("yardentryid")), Convert.ReplaceDBNull(dr("carrier")), Convert.ReplaceDBNull(dr("vehicle")), Convert.ReplaceDBNull(dr("trailer")), Convert.ReplaceDBNull(dr("yardlocation")), Convert.ReplaceDBNull(dr("scheduledate")), UserId)
            Case "EditEntry"
                dr = ds.Tables(0).Rows(0)
                Dim oYardEntry As New WMS.Logic.YardEntry(Convert.ReplaceDBNull(dr("yardentryid")))
                oYardEntry.Update(Convert.ReplaceDBNull(dr("carrier")), Convert.ReplaceDBNull(dr("vehicle")), Convert.ReplaceDBNull(dr("trailer")), Convert.ReplaceDBNull(dr("yardlocation")), Convert.ReplaceDBNull(dr("checkindate")), Convert.ReplaceDBNull(dr("checkoutdate")), Convert.ReplaceDBNull(dr("scheduledate")), UserId)
            Case "AssignReceipt"
                For Each dr In ds.Tables(0).Rows
                    Dim oReceipt As New WMS.Logic.ReceiptHeader(dr("receipt"))
                    oReceipt.AssignToYardEntry(Convert.ReplaceDBNull(dr("yardentryid")), UserId)
                Next
            Case "AssignShipment"
                For Each dr In ds.Tables(0).Rows
                    Dim oShipment As New WMS.Logic.Shipment(dr("shipment"))
                    oShipment.AssignToYardEntry(Convert.ReplaceDBNull(dr("yardentryid")), UserId)
                Next
            Case "UnAssignReceipt"
                For Each dr In ds.Tables(0).Rows
                    Dim oReceipt As New WMS.Logic.ReceiptHeader(dr("receipt"))
                    oReceipt.UnAssignFromYardEntry(UserId)
                Next
            Case "UnAssignShipment"
                For Each dr In ds.Tables(0).Rows
                    Dim oShipment As New WMS.Logic.Shipment(dr("shipment"))
                    oShipment.UnAssignFromYardEntry(UserId)
                Next
                'Case "CheckIn"
                '    For Each dr In ds.Tables(0).Rows
                '        Dim oYardEntry As New WMS.Logic.YardEntry(Convert.ReplaceDBNull(dr("yardentryid")))
                '        oYardEntry.CheckIn(
                '    Next
                'Case "CheckOut"
                '    For Each dr In ds.Tables(0).Rows
                '        Dim oShipment As New WMS.Logic.Shipment(dr("shipment"))
                '        oShipment.UnAssignFromYardEntry(UserId)
                '    Next
        End Select
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub TEYardEntry_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEYardEntry.CreatedChildControls
        With TEYardEntry.ActionBar.Button("Save")
            .ObjectDLL = "WMS.WebApp.dll"
            .ObjectName = "WMS.WebApp.YardEntry"
            If TEYardEntry.CurrentMode = Made4Net.WebControls.TableEditorMode.Insert Then
                .CommandName = "CreateNewEntry"
            Else
                .CommandName = "EditEntry"
            End If
        End With

        'TEYardEntry.ActionBar.AddSpacer()
        'TEYardEntry.ActionBar.AddExecButton("CheckIn", "Check In", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnPlanWave"))
        'With TEAssignReceipts.ActionBar.Button("CheckIn")
        '    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
        '    .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
        '    .ObjectDLL = "WMS.WebApp.dll"
        '    .ObjectName = "WMS.WebApp.YardEntry"
        '    .CommandName = "CheckIn"
        'End With

        'TEYardEntry.ActionBar.AddSpacer()
        'TEYardEntry.ActionBar.AddExecButton("CheckOut", "Check Out", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnCompleteWave"))
        'With TEAssignReceipts.ActionBar.Button("CheckOut")
        '    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
        '    .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
        '    .ObjectDLL = "WMS.WebApp.dll"
        '    .ObjectName = "WMS.WebApp.YardEntry"
        '    .CommandName = "CheckOut"
        'End With
    End Sub

    Private Sub TEAssignReceipts_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEAssignReceipts.CreatedChildControls
        TEAssignReceipts.ActionBar.AddSpacer()
        TEAssignReceipts.ActionBar.AddExecButton("AssignReceipt", "Assign Receipt/s to Yard Entry", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnMultiEdit"))
        With TEAssignReceipts.ActionBar.Button("AssignReceipt")
            .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
            .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
            .ObjectDLL = "WMS.WebApp.dll"
            .ObjectName = "WMS.WebApp.YardEntry"
            .CommandName = "AssignReceipt"
        End With
    End Sub

    Private Sub TEAssignShipments_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEAssignShipments.CreatedChildControls
        TEAssignShipments.ActionBar.AddSpacer()
        TEAssignShipments.ActionBar.AddExecButton("AssignShipment", "Assign Shipment/s to Yard Entry", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnMultiEdit"))
        With TEAssignShipments.ActionBar.Button("AssignShipment")
            .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
            .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
            .ObjectDLL = "WMS.WebApp.dll"
            .ObjectName = "WMS.WebApp.YardEntry"
            .CommandName = "AssignShipment"
        End With
    End Sub

    Private Sub TEYardEntryReceipts_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEYardEntryReceipts.CreatedChildControls
        TEYardEntryReceipts.ActionBar.AddSpacer()
        TEYardEntryReceipts.ActionBar.AddExecButton("UnAssignReceipt", "UnAssign Receipt/s From Yard Entry", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnMultiEdit"))
        With TEYardEntryReceipts.ActionBar.Button("UnAssignReceipt")
            .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
            .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
            .ObjectDLL = "WMS.WebApp.dll"
            .ObjectName = "WMS.WebApp.YardEntry"
            .CommandName = "UnAssignReceipt"
        End With
    End Sub

    Private Sub TEYardEntryShipments_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEYardEntryShipments.CreatedChildControls
        TEYardEntryShipments.ActionBar.AddSpacer()
        TEYardEntryShipments.ActionBar.AddExecButton("UnAssignShipment", "UnAssign Shipment/s From Yard Entry", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnMultiEdit"))
        With TEYardEntryShipments.ActionBar.Button("UnAssignShipment")
            .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
            .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
            .ObjectDLL = "WMS.WebApp.dll"
            .ObjectName = "WMS.WebApp.YardEntry"
            .CommandName = "UnAssignShipment"
        End With
    End Sub

    Private Sub TEYardEntry_RecordSelected(ByVal sender As Made4Net.WebControls.TableEditor, ByVal e As Made4Net.WebControls.TableEditorEventArgs) Handles TEYardEntry.RecordSelected
        Dim tds As DataTable = TEYardEntry.CreateDataTableForSelectedRecord()
        Dim vals As New Specialized.NameValueCollection
        vals.Add("YardEntryId", tds.Rows(0)("YardEntryId"))
        TEAssignShipments.PreDefinedValues = vals
        TEAssignReceipts.PreDefinedValues = vals
    End Sub

End Class
