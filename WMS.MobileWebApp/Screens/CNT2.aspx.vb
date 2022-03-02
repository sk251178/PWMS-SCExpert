Imports Made4Net.Shared.Web
Imports Made4Net.Mobile

<CLSCompliant(False)> Public Class CNT2
    Inherits PWMSRDTBase

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.MobileWebApp.WebCtrls.Screen
    Protected WithEvents DO1 As Made4Net.Mobile.WebCtrls.DataObject

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
        Return Session("_ViewState")
    End Function

    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        Session("_ViewState") = viewState
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not IsPostBack Then
            Dim ld As New WMS.Logic.Load(Session("LoadCNTLoadId").ToString())
            DO1.Value("CONSIGNEE") = ld.CONSIGNEE
            DO1.Value("SKU") = ld.SKU

            DO1.Value("LOCATION") = ld.LOCATION
            DO1.Value("TOLOCATION") = ld.LOCATION

            DO1.Value("WAREHOUSEAREA") = ld.WAREHOUSEAREA
            DO1.Value("TOWAREHOUSEAREA") = ld.WAREHOUSEAREA
            DO1.Value("LOADID") = ld.LOADID

            Dim dduom As Made4Net.WebControls.MobileDropDown = DO1.Ctrl("UOM")
            ddUOM.AllOption = False
            ddUOM.TableName = "SKUUOMDESC"
            ddUOM.ValueField = "UOM"
            ddUOM.TextField = "DESCRIPTION"
            ddUOM.Where = String.Format("CONSIGNEE = '{0}' and SKU = '{1}'", ld.CONSIGNEE, ld.SKU)
            ddUOM.DataBind()

            Try
                ddUOM.SelectedValue = ld.LOADUOM
            Catch ex As Exception

            End Try

            Try
                DO1.Value("SKUDESC") = Made4Net.DataAccess.DataInterface.ExecuteScalar(String.Format("select skudesc from sku where sku = '{0}' and consignee = '{1}'", ld.SKU, ld.CONSIGNEE))
            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Sub doMenu()
        Session.Remove("LoadCNTLoadId")
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub doBack()
        Session.Remove("LoadCNTLoadId")
        Response.Redirect(MapVirtualPath("Screens/CNT.aspx"))
    End Sub

    Private Sub doNext()
        'If Page.IsValid Then
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Try
            Dim ld As New WMS.Logic.Load(Session("LoadCNTLoadId").ToString())
            If ld.UNITS <> DO1.Value("TOUNITS") Then
                If Session("UnitsDiffApproved") Then
                    ld.Count(DO1.Value("TOUNITS"), DO1.Value("UOM"), DO1.Value("TOLOCATION"), DO1.Value("TOWAREHOUSEAREA"), Nothing, WMS.Logic.GetCurrentUser)
                Else
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Quantity Mismatch. Please approve again."))
                    Session("UnitsDiffApproved") = True
                    Return
                End If
            Else
                ld.Count(DO1.Value("TOUNITS"), DO1.Value("UOM"), DO1.Value("TOLOCATION"), DO1.Value("TOWAREHOUSEAREA"), Nothing, WMS.Logic.GetCurrentUser)
            End If
        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate(ex.Message))
            Return
        End Try
        Session.Remove("LoadCNTLoadId")
        'If we are in a scope of low limit count -> go back to picking
        If Session("LoadCountingSourceScreen") Is Nothing Then
            Response.Redirect(MapVirtualPath("Screens/CNT.aspx"))
        Else
            Response.Redirect(MapVirtualPath("Screens/" & Session("LoadCountingSourceScreen") & ".aspx"))
        End If
        'End If
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "next"
                doNext()
            Case "back"
                doBack()
            Case "menu"
                doMenu()
        End Select
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddLabelLine("LOADID")
        DO1.AddLabelLine("CONSIGNEE")
        DO1.AddLabelLine("SKU")
        DO1.AddLabelLine("SKUDESC")
        DO1.AddLabelLine("LOCATION")
        DO1.AddLabelLine("WAREHOUSEAREA")
        DO1.AddSpacer()
        DO1.AddDropDown("UOM")
        DO1.AddTextboxLine("TOUNITS")
        DO1.AddTextboxLine("TOLOCATION")
        DO1.AddTextboxLine("TOWAREHOUSEAREA")
    End Sub
End Class
