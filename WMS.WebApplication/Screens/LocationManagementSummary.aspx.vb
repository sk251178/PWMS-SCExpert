Imports Made4Net.WebControls.AppComponents

Partial Public Class LocationManagementSummary
    Inherits System.Web.UI.Page


#Region "ViewState"

    Protected Overrides Function LoadPageStateFromPersistenceMedium() As Object
        Return WMS.WebCtrls.WebCtrls.Screen.LoadPageStateFromPersistenceMedium()
    End Function

    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        WMS.WebCtrls.WebCtrls.Screen.SavePageStateToPersistenceMedium(viewState, Me.Page, Screen1.ScreenID)
    End Sub

#End Region

    Private WithEvents _ACControl As AppComponentControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub BuildAC()
        _ACControl = New AppComponentControl
        _ACControl.AppComponentCode = "LocManag"
        _ACControl.Width = System.Web.UI.WebControls.Unit.Pixel(600)
        _ACControl.Height = System.Web.UI.WebControls.Unit.Pixel(350)
        ph.Controls.Add(_ACControl)
    End Sub

    Protected Overrides Sub CreateChildControls()
        MyBase.CreateChildControls()
        Me.BuildAC()
    End Sub

End Class