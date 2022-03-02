Imports WMS.Logic
<CLSCompliant(False)> Public Class BLKDRCV0
    Inherits PWMSRDTBase

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.MobileWebApp.WebCtrls.Screen
    Protected WithEvents DO1 As Made4Net.Mobile.WebCtrls.DataObject

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

            Dim dd1 As Made4Net.WebControls.MobileDropDown
            dd1 = DO1.Ctrl("CONSIGNEE")
            dd1.AllOption = False
            dd1.TableName = "CONSIGNEE"
            dd1.ValueField = "CONSIGNEE"
            dd1.TextField = "CONSIGNEENAME"
            dd1.DataBind()

            'If Session("CreateLoadReceiptLocation") = "" Then
            '    If Session("CreateLoadConsignee") <> "" Then
            '        Dim oCon As New WMS.Logic.Consignee(Session("CreateLoadConsignee"))
            '        DO1.Value("LOCATION") = oCon.DEFAULTRECEIVINGLOCATION
            '    Else
            '        DO1.Value("LOCATION") = ""
            '    End If
            'Else
            '    DO1.Value("LOCATION") = Session("CreateLoadReceiptLocation")
            'End If
        End If

    End Sub

    Private Sub doCreateContainer()
        Dim cntrid As String = Create()

        Try
            Dim oLoc As WMS.Logic.Location = New WMS.Logic.Location(DO1.Value("LOCATION"), Warehouse.getUserWarehouseArea()) '  DO1.Value("WAREHOUSEAREA"))
            Session("CreateLoadReceiptLocation") = oLoc.Location
            Session("CreateLoadReceiptWarehousearea") = oLoc.WAREHOUSEAREA
            Session("CreateLoadConsignee") = DO1.Value("CONSIGNEE")
        Catch ex As Exception
            Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Location not found"))
            Return
        End Try
        Session("CreateLoadContainerID") = cntrid
        Session("HandelingUnitType") = DO1.Value("HUTYPE")

        Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("Screens/BLKDRCV1.aspx"))
    End Sub

    Private Function Create() As String
        Dim cntr As New WMS.Logic.Container
        ' check if Container Should be created 
        If DO1.Value("CONTAINERID") <> "" Then
            cntr.ContainerId = DO1.Value("CONTAINERID")
            cntr.HandlingUnitType = DO1.Value("HUTYPE")
            cntr.Location = DO1.Value("LOCATION")
            cntr.Warehousearea = Warehouse.getUserWarehouseArea() 'DO1.Value("WAREHOUSEAREA")
            cntr.Serial = DO1.Value("CONTAINERID")
        Else
            cntr.HandlingUnitType = DO1.Value("HUTYPE")
            cntr.Location = DO1.Value("LOCATION")
            cntr.Warehousearea = Warehouse.getUserWarehouseArea()
            cntr.Serial = DO1.Value("CONTAINERID")
        End If
        Try
            cntr.Post(WMS.Logic.Common.GetCurrentUser)
        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Return Nothing
        Catch ex As Exception
            Return Nothing
        End Try
        Return cntr.ContainerId
    End Function

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddDropDown("HUTYPE")
        DO1.AddTextboxLine("CONTAINERID")
        DO1.AddDropDown("CONSIGNEE")
        DO1.AddTextboxLine("LOCATION")
        'DO1.AddTextbox Line("WAREHOUSEAREA")
        DO1.AddSpacer()
    End Sub

    Private Sub doMenu()
        MobileUtils.ClearCreateLoadProcessSession(True)
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "create"
                doCreateContainer()
            Case "menu"
                doMenu()
        End Select
    End Sub

End Class
