Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports Made4Net.Shared
Imports Made4Net.DataAccess
Imports WMS.Logic

Partial Public Class WeightCaptureNeeded
    Inherits PWMSRDTBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub goPalletized()
        Response.Redirect(MapVirtualPath("Screens/WeightCaptureNeededCLD1.aspx"))
    End Sub

    Private Sub goReady()
        If Request("sourcescreen") = "cld1overrideWGT" Then
            gotoOverrideWeight()
        Else
            Session("LoadPreviewsVals") = "cld1overrideWGT"

            Response.Redirect("CLD1.aspx")

        End If

        '  Response.Redirect(MapVirtualPath("Screens/WeightCaptureNeeded2.aspx"))
    End Sub


    Public Sub gotoOverrideWeight()

        Dim url As String = DataInterface.ExecuteScalar("select url from sys_screen where  screen_id = 'cld1overrideWGT'", "Made4NetSchema")
        Response.Redirect(MapVirtualPath(url) & "?sourcescreen=RDTCLD1")

    End Sub
    'Private Sub goFull()
    '    Response.Redirect(MapVirtualPath("Screens/WeightCaptureNeeded3.aspx"))
    'End Sub
    Private Sub doBack()
        Response.Redirect(MobileUtils.GetURLByScreenCode("RDTCLD1"))
    End Sub


    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "palletized"
                goPalletized()
            Case "ready"
                goReady()
                'Case "full pallet"
                '    goFull()
            Case "back"
                doBack()
        End Select
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        'DO1.AddLabelLine("Weight Capture Is Needed")
    End Sub
End Class