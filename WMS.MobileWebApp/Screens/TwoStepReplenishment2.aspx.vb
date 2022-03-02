Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports Made4Net.Shared
Imports Made4Net.DataAccess

Imports Wms.Logic

Partial Public Class TwoStepReplenishment2
    Inherits PWMSRDTBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not IsPostBack Then

            Dim r As RWMS.Logic.TwoStepReplenishment = Session("CLSREPL")
            DOVerify.Value("Trolley") = r.trolley

            DOVerify.Value("TotalTasks") = r.alistRepl.Count + r.TaskDeliveryIndex 'r.dictRepl.Values.Count

            DOVerify.Value("TotalCases") = r.TotalCases

            DOVerify.Value("TotalRemainingTasks") = r.alistRepl.Count - r.TaskIndex
        End If
    End Sub

    <CLSCompliant(False)>
    Protected Sub DOVerify_ButtonClick(ByVal sender As System.Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DOVerify.ButtonClick
        Select Case e.CommandText.ToLower
            Case "next"
                doNext()
            Case "back"
                doback()
            Case "deliver"
                doDeliver()
        End Select
    End Sub


    Private Sub doDeliver()
        Dim clsRep As RWMS.Logic.TwoStepReplenishment = Session("CLSREPL")
        Dim msg As String
        If Not clsRep.deliver(msg) Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  msg)
            doback()
        Else
            Session("CLSREPL") = clsRep
            Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("Screens/TwoStepReplenishment4.aspx"))

        End If

    End Sub

    Protected Sub DOVerify_CreatedChildControls(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DOVerify.CreatedChildControls
        DOVerify.AddLabelLine("Trolley")

        DOVerify.AddLabelLine("TotalTasks", "Total Tasks")

        DOVerify.AddLabelLine("TotalCases", "Total Cases")

        DOVerify.AddLabelLine("TotalRemainingTasks", "Total Remaining Tasks")

        DOVerify.DefaultButton = "Next"

    End Sub

    Private Sub doNext()

        Dim clsRepl As RWMS.Logic.TwoStepReplenishment

        If Not IsNothing(Session("CLSREPL")) Then
            clsRepl = Session("CLSREPL")
            If clsRepl.TaskIndex > clsRepl.alistRepl.Count - 1 Then
                ''add one more
                clsRepl.LoadAvailable(True)
            End If
        End If

        Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("Screens/TwoStepReplenishment3.aspx"))



    End Sub

    Private Sub doback()
        'clear object + unassingn
        Dim clsRepl As RWMS.Logic.TwoStepReplenishment

        If Not IsNothing(Session("CLSREPL")) Then
            clsRepl = Session("CLSREPL")
            clsRepl.UnAssignTaskAllRepl()
            clsRepl.clear()
            Session.Remove("CLSREPL")
        End If

        Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("Screens/TwoStepReplenishment.aspx"))
    End Sub

    Private Sub doMenu()
        Made4Net.Mobile.Common.GoToMenu()
    End Sub


End Class