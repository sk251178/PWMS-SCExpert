Imports Made4Net.Shared.Web

<CLSCompliant(False)> Public Class CLD2
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
            If Session("CreateLoadLabelPrinter") Is Nothing Or Session("CreateLoadLabelPrinter") = "" Then
                Try
                    Session("CreateLoadLabelPrinter") = Convert.ToString(Made4Net.DataAccess.DataInterface.ExecuteScalar("SELECT DEFAULTPRINTER FROM LABELS WHERE LABELNAME = 'LOAD'"))
                Catch ex As Exception
                End Try
            End If

            Dim dd As Made4Net.WebControls.MobileDropDown
            dd = DO1.Ctrl("STATUS")

            dd.AllOption = False
            dd.TableName = "INVSTATUSES"
            dd.ValueField = "CODE"
            dd.TextField = "DESCRIPTION"
            dd.DataBind()
            If Not Session("CreateLoadStatus") Is Nothing Then
                dd.SelectedValue = Session("CreateLoadStatus")
            End If

            Try
                Dim oSku As New WMS.Logic.SKU(Session("CreateLoadConsignee"), Session("CreateLoadSKU"))
                dd.SelectedValue = oSku.INITIALSTATUS
            Catch ex As Exception

            End Try

            dd = DO1.Ctrl("REASONCODE")
            dd.AllOption = True
            dd.TableName = "CODELISTDETAIL"
            dd.ValueField = "CODE"
            dd.TextField = "DESCRIPTION"
            dd.Where = "CODELISTCODE = 'INVHOLDRC'"
            dd.DataBind()
            If Not Session("CreateLoadHoldRC") Is Nothing Then
                dd.SelectedValue = Session("CreateLoadHoldRC")
            End If

            If WMS.Logic.Consignee.AutoPrintLoadIdOnReceiving(Session("CreateLoadConsignee")) Then
                DO1.setVisibility("PRINTER", True)
                DO1.Value("PRINTER") = Session("CreateLoadLabelPrinter")
            Else
                DO1.setVisibility("PRINTER", False)
            End If
        End If
    End Sub

    Private Sub doMenu()
        'Session.Remove("CreateLoadRCN")
        'Session.Remove("CreateLoadRCNLine")
        'Session.Remove("CreateLoadConsignee")
        'Session.Remove("CreateLoadSKU")
        MobileUtils.ClearCreateLoadProcessSession()
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub doNext()
        Session("CreateLoadStatus") = DO1.Value("STATUS")
        Session("CreateLoadHoldRC") = DO1.Value("REASONCODE")
        If DO1.Ctrl("PRINTER").Visible = True Then
            Session("CreateLoadLabelPrinter") = DO1.Value("PRINTER")
        End If
        Response.Redirect(MapVirtualPath("Screens/CLD3.aspx"))
    End Sub

    Private Sub doChangeLine()
        MobileUtils.ClearCreateLoadChangeLineSession()
        Response.Redirect(MapVirtualPath("Screens/CreateLoadSelectRCNLine.aspx"))
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddDropDown("STATUS")
        DO1.AddDropDown("ReasonCode")
        DO1.AddTextboxLine("Printer")
        DO1.AddSpacer()
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "next"
                doNext()
            Case "menu"
                doMenu()
            Case "changeline"
                doChangeLine()
        End Select
    End Sub

End Class
