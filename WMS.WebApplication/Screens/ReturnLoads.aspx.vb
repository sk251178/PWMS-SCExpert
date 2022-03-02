Public Partial Class ReturnLoads
    Inherits System.Web.UI.Page

#Region "ViewState"

    Protected Overrides Function LoadPageStateFromPersistenceMedium() As Object
        Return WMS.WebCtrls.WebCtrls.Screen.LoadPageStateFromPersistenceMedium()
    End Function

    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        WMS.WebCtrls.WebCtrls.Screen.SavePageStateToPersistenceMedium(viewState, Me.Page, Screen1.ScreenID)
    End Sub

#End Region

    Public Sub New()

    End Sub

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As New DataSet
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser()
        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        Select Case CommandName.ToLower
            Case "save"
                If ds.Tables(0).Rows.Count = 0 Then
                    Throw New Made4Net.Shared.M4NException(New Exception, "No rows selected.", "No rows selected.")
                End If
                For Each dr As DataRow In ds.Tables(0).Rows
                    Dim oLoad As New WMS.Logic.Load(dr("loadid").ToString())
                    oLoad.ReturnShippedLoadsToInventory(dr("status"), dr("Location"), dr("warehousearea"), dr("laststatusrc"), WMS.Logic.Common.GetCurrentUser())
                Next
        End Select
    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub TEReturnLoads_CreatedChildControls(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TEReturnLoads.CreatedChildControls
        With TEReturnLoads.ActionBar.Button("Save")
            .ObjectDLL = "WMS.WebApp.dll"
            .ObjectName = "WMS.WebApp.ReturnLoads"
            .CommandName = "save"
        End With
    End Sub
End Class