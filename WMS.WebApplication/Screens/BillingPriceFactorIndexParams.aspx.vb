Public Partial Class BillingPriceFactorIndexParams
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub TS_SelectedIndexChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles TS.SelectedIndexChange
        If Tabstrip1.SelectedIndex = 0 Then
            TEDailyParamHeader.Restart()
            TEDailyParamValue.Restart()
            TEDailyParamHeader.RefreshData()
            TEDailyParamValue.RefreshData()
            DC1.Shake()
        End If
    End Sub
End Class