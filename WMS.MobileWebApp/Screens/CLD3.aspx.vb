Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports Made4Net.DataAccess

<CLSCompliant(False)> Public Class CLD3
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
            Dim dd As Made4Net.WebControls.MobileDropDown = DO1.Ctrl("UOM")
            dd.AllOption = False
            dd.TableName = "SKUUOMDESC"
            dd.ValueField = "UOM"
            dd.TextField = "DESCRIPTION"
            dd.Where = String.Format("CONSIGNEE = '{0}' and SKU = '{1}'", Session("CreateLoadConsignee"), Session("CreateLoadSKU"))
            dd.DataBind()
            If Not Session("CreateLoadUOM") Is Nothing Then
                dd.SelectedValue = Session("CreateLoadUOM")
            Else
                Dim dfuom As String = DataInterface.ExecuteScalar("SELECT ISNULL(DEFAULTUOM,'') FROM SKU WHERE CONSIGNEE = '" & Session("CreateLoadConsignee") & "' and SKU = '" & Session("CreateLoadSKU") & "'")
                If dfuom <> "" Then
                    dd.SelectedValue = dfuom
                End If
            End If

            DO1.Value("UNITS") = Session("CreateLoadUnits")
            DO1.Value("NUMLOADS") = 1
        End If
    End Sub

    Private Sub doNext(ByVal doPutAway As Boolean)
        If Page.IsValid() Then
            Try
                Session("CreateLoadUOM") = DO1.Value("UOM")
            Catch ex As Exception

            End Try
            Session("CreateLoadUnits") = DO1.Value("UNITS")
            Session("CreateLoadNumLoads") = DO1.Value("NUMLOADS")
            Session("CreateLoadDoPutAway") = doPutAway
            Dim url As String = DataInterface.ExecuteScalar("select url from sys_screen where  screen_id = 'RDTCLD4'", "Made4NetSchema")
            Response.Redirect(MapVirtualPath(url))
        End If
    End Sub

    Private Sub doMenu()
        'Session.Remove("CreateLoadRCN")
        'Session.Remove("CreateLoadRCNLine")
        'Session.Remove("CreateLoadConsignee")
        'Session.Remove("CreateLoadSKU")
        'Session.Remove("CreateLoadContainerID")
        MobileUtils.ClearCreateLoadProcessSession()
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub doChangeLine()
        MobileUtils.ClearCreateLoadChangeLineSession()
        Response.Redirect(MapVirtualPath("Screens/CreateLoadSelectRCNLine.aspx"))
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddTextboxLine("UNITS")
        DO1.AddDropDown("UOM")
        DO1.AddTextboxLine("NUMLOADS")
        DO1.AddSpacer()
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "next"
                doNext(False)
            Case "create & pickup"
                doNext(True)
            Case "changeline"
                doChangeLine()
        End Select
    End Sub
End Class
