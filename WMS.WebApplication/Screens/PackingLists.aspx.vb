Imports Made4Net.Shared.Conversion.Convert
Imports WMS.Logic

Partial Public Class PackingLists
    Inherits System.Web.UI.Page

#Region "ViewState"

    Protected Overrides Function LoadPageStateFromPersistenceMedium() As Object
        Return WMS.WebCtrls.WebCtrls.Screen.LoadPageStateFromPersistenceMedium()
    End Function

    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        WMS.WebCtrls.WebCtrls.Screen.SavePageStateToPersistenceMedium(viewState, Me.Page, Screen1.ScreenID)
    End Sub

#End Region

#Region "Ctor"

    Public Sub New()

    End Sub

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As New DataSet
        Dim dr As DataRow
        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        Select Case CommandName
            Case "CreatPL"
                dr = ds.Tables(0).Rows(0)
                Dim oPl As New WMS.Logic.PackingListHeader
                oPl.Create(ReplaceDBNull(dr("packinglistid")), ReplaceDBNull(dr("consignee")), ReplaceDBNull(dr("company")), ReplaceDBNull(dr("companytype")), ReplaceDBNull(dr("contactid")), 0, WMS.Logic.Common.GetCurrentUser)
            Case "UpdatePL"
                dr = ds.Tables(0).Rows(0)
                Dim oPl As New WMS.Logic.PackingListHeader(dr("packinglistid"))
                oPl.Update(ReplaceDBNull(dr("consignee")), ReplaceDBNull(dr("company")), ReplaceDBNull(dr("companytype")), ReplaceDBNull(dr("contactid")), 0, WMS.Logic.Common.GetCurrentUser)
            Case "PackLoad"
                Dim oPl As New WMS.Logic.PackingListHeader(ds.Tables(0).Rows(0)("packinglistid"))
                For Each dr In ds.Tables(0).Rows
                    oPl.PackLoad(dr("loadid"), WMS.Logic.Common.GetCurrentUser)
                Next
            Case "UnpackLoad"
                Dim oPl As New WMS.Logic.PackingListHeader(ds.Tables(0).Rows(0)("packinglistid"))
                For Each dr In ds.Tables(0).Rows
                    oPl.UnPackLoad(dr("loadid"), WMS.Logic.Common.GetCurrentUser)
                Next
            Case "Ship"
                For Each dr In ds.Tables(0).Rows
                    Dim oPl As New WMS.Logic.PackingListHeader(dr("packinglistid"))
                    oPl.Ship(WMS.Logic.Common.GetCurrentUser)
                Next
            Case "CancelPL"
                For Each dr In ds.Tables(0).Rows
                    Dim oPl As New WMS.Logic.PackingListHeader(dr("packinglistid"))
                    oPl.Cancel(WMS.Logic.Common.GetCurrentUser)
                Next
            Case "PrintPL"
                For Each dr In ds.Tables(0).Rows
                    Dim oPl As New WMS.Logic.PackingListHeader(dr("packinglistid"))
                    oPl.PrintPackingList("", Made4Net.Shared.Translation.Translator.CurrentLanguageID, WMS.Logic.Common.GetCurrentUser)
                Next
            Case "PrintLabels"
                For Each dr In ds.Tables(0).Rows
                    Dim oPl As New WMS.Logic.PackingListHeader(dr("packinglistid"))
                    oPl.PrintLabel("")
                Next
        End Select
    End Sub

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub TEPackList_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEPackList.CreatedChildControls
        With TEPackList
            With .ActionBar
                .AddSpacer()
                .AddExecButton("Ship", "Ship Loads", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarShipOrders"))
                With .Button("Ship")
                    .ObjectDLL = "WMS.WebApp.dll"
                    .ObjectName = "WMS.WebApp.PackingLists"
                    .CommandName = "Ship"
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                    .ConfirmRequired = True
                    .ConfirmMessage = "Are you sure you want to ship the selected packing lists?"
                End With
                .AddSpacer()
                .AddExecButton("PrintPL", "Print Packing List", Made4Net.WebControls.SkinManager.GetImageURL("ReportPrint"))
                With .Button("PrintPL")
                    .ObjectDLL = "WMS.WebApp.dll"
                    .ObjectName = "WMS.WebApp.PackingLists"
                    .CommandName = "PrintPL"
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                End With
                .AddExecButton("PrintLabels", "Print Packing List Labels", Made4Net.WebControls.SkinManager.GetImageURL("LabelPrint"))
                With .Button("PrintLabels")
                    .ObjectDLL = "WMS.WebApp.dll"
                    .ObjectName = "WMS.WebApp.PackingLists"
                    .CommandName = "PrintLabels"
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                End With
                .AddSpacer()
                .AddExecButton("CancelPL", "Cancel PackingList", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarCancelOrders"))
                With .Button("CancelPL")
                    .ObjectDLL = "WMS.WebApp.dll"
                    .ObjectName = "WMS.WebApp.PackingLists"
                    .CommandName = "CancelPL"
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                    .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
                    .ConfirmRequired = True
                    .ConfirmMessage = "Are you sure you want to cancel the packing list?"
                End With
            End With
            With .ActionBar.Button("Save")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.PackingLists"
                If TEPackList.CurrentMode = Made4Net.WebControls.TableEditorMode.Insert Then
                    .CommandName = "CreatPL"
                ElseIf TEPackList.CurrentMode = Made4Net.WebControls.TableEditorMode.Edit Then
                    .CommandName = "UpdatePL"
                End If
            End With
        End With
    End Sub

    Private Sub TEPackListDetail_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEPackListDetail.CreatedChildControls
        With TEPackListDetail
            With .ActionBar
                .AddSpacer()
                .AddExecButton("UnpackLoad", "Unpack Load", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarCancelPicks"))
                With .Button("UnpackLoad")
                    .ObjectDLL = "WMS.WebApp.dll"
                    .ObjectName = "WMS.WebApp.PackingLists"
                    .CommandName = "UnpackLoad"
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                    .ConfirmRequired = True
                    .ConfirmMessage = "Are you sure you want to unpack the selected loads?"
                End With
            End With
        End With
    End Sub

    Private Sub TELoadsSelect_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TELoadsSelect.CreatedChildControls
        With TELoadsSelect
            With .ActionBar
                .AddSpacer()
                .AddExecButton("PackLoad", "Pack Load", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnPlanWave"))
                With .Button("PackLoad")
                    .ObjectDLL = "WMS.WebApp.dll"
                    .ObjectName = "WMS.WebApp.PackingLists"
                    .CommandName = "PackLoad"
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                    .ConfirmRequired = True
                    .ConfirmMessage = "Are you sure you want to pack the selected loads?"
                End With
            End With
            Dim tds As DataTable = TEPackList.CreateDataTableForSelectedRecord()
            Dim vals As New Specialized.NameValueCollection
            vals.Clear()
            vals.Add("packinglistid", tds.Rows(0)("packinglistid"))
            .PreDefinedValues = vals
        End With
    End Sub

End Class