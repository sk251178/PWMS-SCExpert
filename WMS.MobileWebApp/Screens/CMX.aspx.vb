Imports WMS.Logic
<CLSCompliant(False)> Public Class CMX
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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If WMS.Logic.GetCurrentUser Is Nothing Then
            WMS.Logic.GotoLogin()
        End If
        If WMS.Logic.GetCurrentUser = "" Then
            WMS.Logic.GotoLogin()
        End If

        If Not IsPostBack Then
            Dim dd As Made4Net.WebControls.MobileDropDown
            dd = DO1.Ctrl("HUTYPE")

            dd.AllOption = False
            dd.TableName = "HANDELINGUNITTYPE"
            dd.ValueField = "CONTAINER"
            dd.TextField = "CONTAINERDESC"
            dd.DataBind()

            dd = DO1.Ctrl("USAGETYPE")
            dd.AllOption = False

            dd.TableName = "CODELISTDETAIL"
            dd.ValueField = "CODE"
            dd.TextField = "DESCRIPTION"
            dd.Where = "CODELISTCODE = 'CONTUSAGE'"
            dd.DataBind()
        End If
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddDropDown("HUTYPE")
        DO1.AddDropDown("USAGETYPE")
        DO1.AddTextboxLine("CONTAINERID")
        DO1.AddTextboxLine("SERIAL")
        DO1.AddTextboxLine("LOCATION", True, "placeloads")
        DO1.AddSpacer()
    End Sub

    Private Sub doCreateContainer()
        Create()
    End Sub

    Private Sub doPlaceLoads()
        Session("CONTAINERID") = Create()
        If Not Session("ContainerID") Is Nothing Then
            Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("Screens/CMX1.aspx"))
        End If
    End Sub

    Private Function Create() As String
        Dim cntr As New WMS.Logic.Container
        If DO1.Value("CONTAINERID") <> "" Then
            cntr.ContainerId = DO1.Value("CONTAINERID")
        End If
        cntr.HandlingUnitType = DO1.Value("HUTYPE")
        cntr.UsageType = DO1.Value("USAGETYPE")
        cntr.Serial = DO1.Value("SERIAL")
        cntr.Location = DO1.Value("LOCATION")
        cntr.Warehousearea = Warehouse.getUserWarehouseArea()
        Try
            'cntr.Post(WMS.Logic.Common.GetCurrentUser)
            cntr.Create(WMS.Logic.Common.GetCurrentUser)
        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Return Nothing
        Catch ex As Exception
            Return Nothing
        End Try
        Dim trans As New Made4Net.Shared.Translation.Translator
        Return cntr.ContainerId
    End Function

    Private Sub doMenu()
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            'Case "create"
            '    doCreateContainer()
            Case "placeloads"
                doPlaceLoads()
            Case "menu"
                doMenu()
        End Select
    End Sub
End Class
