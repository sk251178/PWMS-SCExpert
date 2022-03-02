Public Partial Class HUStacking2
    Inherits PWMSRDTBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Session()("TopHU") Is Nothing Then
            Response.Redirect(Made4Net.Shared.Web.Common.MapVirtualPath("Screens/HUSTACKING.aspx"))
        End If
        Dim oCont As WMS.Logic.Container = CType(Session()("TopHU"), WMS.Logic.Container)
        DO1.Value("HUID") = oCont.ContainerId
        DO1.Value("NumberOfLoads") = oCont.Loads.Count
        DO1.Value("Location") = oCont.Location
        DO1.Value("WarehouseArea") = oCont.Warehousearea
        If Not IsPostBack Then
            DO1.Value("LOWERHUID") = ""
        End If
    End Sub

    Protected Sub DO1_CreatedChildControls(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddLabelLine("HUID")
        DO1.AddLabelLine("NumberOfLoads")
        DO1.AddLabelLine("Location")
        DO1.AddLabelLine("WarehouseArea")

        DO1.AddTextboxLine("LOWERHUID")
    End Sub

    <CLSCompliant(False)>
    Protected Sub DO1_ButtonClick(ByVal sender As System.Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        If e.CommandText.ToLower() = "back" Then
            doBack()
        ElseIf e.CommandText.ToLower() = "next" Then
            doNext()
        End If
    End Sub

    Private Sub doBack()
        Response.Redirect(Made4Net.Shared.Web.Common.MapVirtualPath("Screens/HUSTACKING.aspx"))
    End Sub

    Private Sub doNext()
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        If WMS.Logic.Container.Exists(DO1.Value("LOWERHUID")) Then

            Dim contObj As WMS.Logic.Container = CType(Session()("TopHU"), WMS.Logic.Container)
            If DO1.Value("LOWERHUID") = contObj.ContainerId Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Can not place handling unit on itself."))
                Return
            End If
            contObj.OnContainer = DO1.Value("LOWERHUID")
            contObj.Save(WMS.Logic.GetCurrentUser)
            Session().Remove("TOPHU")
            Response.Redirect(Made4Net.Shared.Web.Common.MapVirtualPath("Screens/HUSTACKING.aspx"))
        Else
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Lower Handling Unit ID does not exist."))
        End If
    End Sub
End Class