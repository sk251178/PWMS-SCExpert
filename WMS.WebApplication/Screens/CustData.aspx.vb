Public Class CustData
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents TECompMan As Made4Net.WebControls.TableEditor
    Protected WithEvents hdCompId As System.Web.UI.WebControls.TextBox
    Protected WithEvents hdCompConsignee As System.Web.UI.WebControls.TextBox
    Protected WithEvents hdCompType As System.Web.UI.WebControls.TextBox
    Protected WithEvents hdPointId As System.Web.UI.WebControls.TextBox

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
        Return WMS.WebCtrls.WebCtrls.Screen.LoadPageStateFromPersistenceMedium()
    End Function

    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        WMS.WebCtrls.WebCtrls.Screen.SavePageStateToPersistenceMedium(viewState, Me.Page, Screen1.ScreenID)
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not IsPostBack Then
            TECompMan.Visible = False
        Else
            showCustomer()
            Dim tds As DataTable = TECompMan.CreateDataTableForSelectedRecord()
            Dim vals As New Specialized.NameValueCollection
            vals.Add("NEWPOINTID", hdPointId.Text)
            TECompMan.PreDefinedValues = vals
        End If
    End Sub

    Private Sub showCustomer()
        TECompMan.Visible = True
        TECompMan.FilterExpression = "CONSIGNEE='" & hdCompConsignee.Text & "' AND COMPANY='" & hdCompId.Text & "' AND COMPANYTYPE='" & hdCompType.Text & "'"
        TECompMan.Restart()
    End Sub

    Private Sub TECompMan_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TECompMan.CreatedChildControls
        With TECompMan.ActionBar
            .AddSpacer()
            .AddExecButton("PointComp", "pin", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnMultiEdit"))
            With .Button("PointComp")
                .ObjectDLL = "WMS.Logic.dll"
                .ObjectName = "WMS.Logic.Company"
                .CommandName = "pin"
                .ConfirmRequired = True
                .ConfirmMessage = "Are you sure you want to pin company?"
                .EnabledInMode = Made4Net.WebControls.TableEditorMode.View
            End With
        End With

    End Sub

    Private Sub TECompMan_AfterItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles TECompMan.AfterItemCommand
        If e.CommandName = "pin" Then
            TECompMan.RefreshData()
        End If
    End Sub
End Class