Imports Made4Net.Shared
Imports Made4Net.Scheduler

Partial Public Class OrderAutomationWaveAssign
    Inherits System.Web.UI.Page


#Region "ViewState"

    Protected Overrides Function LoadPageStateFromPersistenceMedium() As Object
        Return WMS.WebCtrls.WebCtrls.Screen.LoadPageStateFromPersistenceMedium()
    End Function

    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        WMS.WebCtrls.WebCtrls.Screen.SavePageStateToPersistenceMedium(viewState, Me.Page, Screen1.ScreenID)
    End Sub

#End Region

    'Added for PWMS-419 Start

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

            Case "deleteoaw"
                dr = ds.Tables(0).Rows(0)
                If Not WMS.Logic.Utils.deleteWaveassignment(dr("TEMPLATENAME"), Message) Then
                    Throw New ApplicationException(Message)
                End If

                WMS.Logic.Wave.Delete(dr("TEMPLATENAME"))

        End Select
    End Sub

#End Region
    ' Added for PWMS-419 End

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub FillControl()
        SheduleTrigger.ApplicationConnection = Warehouse.WarehouseConnection
        SheduleTrigger.ApplicationToRun = "OrdersAutomationWaveAssignment"
        SheduleTrigger.ScheduleID = "OrdersAutomationWaveAssignment_" & Session("TEMPLATE")
        Dim sap As SchedulerApplicationParam = New SchedulerApplicationParam
        sap.Argument = Session("TEMPLATE")
        sap.ArgumentOrdinal = 1
        sap.ArgumentSystemType = "System.String"
        Dim arr As New ArrayList
        arr.Add(sap)

        SheduleTrigger.SetApplicationParams(arr)
        SheduleTrigger.ReLoad()
        'SheduleTrigger.refresh()

    End Sub

    Private Sub TEWAVEASSIGNPOLICY_RecordSelected(ByVal sender As Made4Net.WebControls.TableEditor, ByVal e As Made4Net.WebControls.TableEditorEventArgs) Handles TEWAVEASSIGNPOLICY.RecordSelected
        Dim tds As DataTable = TEWAVEASSIGNPOLICY.CreateDataTableForSelectedRecord()
        Session("TEMPLATE") = tds.Rows(0)("TEMPLATENAME")

        FillControl()
    End Sub

    'Added for PWMS-419 Start
    Private Sub TEWAVEASSIGNPOLICY_CreatedChildControls(sender As Object, e As EventArgs) Handles TEWAVEASSIGNPOLICY.CreatedChildControls
        With TEWAVEASSIGNPOLICY.ActionBar
            If TEWAVEASSIGNPOLICY.CurrentMode = Made4Net.WebControls.TableEditorMode.Edit Then
                With .Button("Delete")
                    .ObjectDLL = "WMS.WebApp.dll"
                    .ObjectName = "WMS.WebApp.OrderAutomationWaveAssign"
                    .CommandName = "deleteoaw"
                End With

            End If


        End With
    End Sub
    ' Added for PWMS-419 End

   
End Class