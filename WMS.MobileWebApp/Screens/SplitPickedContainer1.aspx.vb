Imports WMS.Logic

Partial Public Class SplitPickedContainer2
    Inherits PWMSRDTBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not IsPostBack Then

            Dim dd1 As Made4Net.WebControls.MobileDropDown
            dd1 = DO1.Ctrl("CONSIGNEE")
            dd1.AllOption = False
            dd1.TableName = "CONSIGNEE"
            dd1.ValueField = "CONSIGNEE"
            dd1.TextField = "CONSIGNEENAME"
            dd1.DataBind()
            Try
                dd1.SelectedValue = "FSA"
            Catch ex As Exception
            End Try

            If Not IsNothing(Session("SplitContainerID")) Then DO1.Value("SplitContainerID") = Session("SplitContainerID")

            'If Not IsNothing(Session("SELECTEDSKU")) Then
            '    DO1.Value("SKU") = Session("SELECTEDSKU")
            Session.Remove("SELECTEDSKU")
            'ElseIf Not IsNothing(Session("SplitItem")) Then
            '    DO1.Value("SKU") = Session("SplitItem")
            'End If
            DO1.Value("SKU") = ""
            DO1.Value("SplitContainerID") = ""
            'If Not IsNothing(Session("SplitConsignee")) Then DO1.Value("Consignee") = Session("SplitConsignee")
        End If
    End Sub

    <CLSCompliant(False)>
    Protected Sub DO1_ButtonClick(ByVal sender As System.Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        If e.CommandText.ToLower = "menu" Then
            doMenu()
        ElseIf e.CommandText.ToLower = "next" Then
            doNext()
        End If
    End Sub

    Protected Sub DO1_CreatedChildControls(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls

        DO1.AddTextboxLine("SplitContainerID", True, "Next", "Split container ID")
        DO1.AddTextboxLine("SKU", True, "Next")
        DO1.AddDropDown("Consignee")

    End Sub

    Private Sub doMenu()
        Session.Remove("SplitContainerID")
        Session.Remove("SplitItem")
        Session.Remove("SplitConsignee")
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub doNext()
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Dim sku As String

        If (DO1.Value("SplitContainerID") <> "") Then
            If ContainerExists(DO1.Value("SplitContainerID")) Then

                sku = MobileUtils.getSKU(DO1.Value("Consignee"), DO1.Value("SKU"), "SplitPickedContainer1")

                If SKUExists(DO1.Value("SplitContainerID"), DO1.Value("Consignee"), sku) Then
                Else
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("SKU not found on the container"))
                    Return
                End If
            Else
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Container not found"))
                Return
            End If
        Else
            '             HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Please fill at least one of the fields"))
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Container not found")) 'Illigal input"))
            Return
        End If


        Session("SplitContainerID") = DO1.Value("SplitContainerID")
        Session("SplitItem") = sku
        Session("SplitConsignee") = DO1.Value("Consignee")
        Response.Redirect(Made4Net.Shared.Web.Common.MapVirtualPath("Screens/SplitPickedContainer2.aspx"))
    End Sub


    Private Function ContainerExists(ByVal Container As String) As Boolean
        Dim ret As Boolean = True
        Dim SQL As String

        SQL = String.Format("select count(1) from container where container = '{0}'", Container)
        SQL = Made4Net.DataAccess.DataInterface.ExecuteScalar(SQL)
        If SQL = "0" Then
            ret = False
        End If
        Return ret
    End Function

    Private Function SKUExists(ByVal Container As String, ByVal consignee As String, ByVal sku As String) As Boolean
        Dim ret As Boolean = True
        Dim SQL As String

        SQL = String.Format("select count(1) from invload where handlingunit = '{0}' and CONSIGNEE='{1}' and SKU='{2}' and activitystatus in ('PICKED', 'STAGED','LOADED', 'VERIFIED')", Container, consignee, sku)
        SQL = Made4Net.DataAccess.DataInterface.ExecuteScalar(SQL)
        If SQL = "0" Then
            ret = False
        End If
        Return ret
    End Function
End Class