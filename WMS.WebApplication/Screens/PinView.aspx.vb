Public Class PinView
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents TEPinView As Made4Net.WebControls.TableEditor
    Protected WithEvents hdVal1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents hdVal2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents hdVal3 As System.Web.UI.WebControls.TextBox
    Protected WithEvents hdObjectType As System.Web.UI.WebControls.TextBox
    Protected WithEvents hdValCommand As System.Web.UI.WebControls.TextBox
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
        Return Screen1.LoadPageStateFromPersistenceMedium()
    End Function

    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        Screen1.SavePageStateToPersistenceMedium(viewState, Me.Page, Screen1.ScreenID)
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If hdValCommand.Text = "hide" Then
            TEPinView.Visible = False
        ElseIf hdValCommand.Text = "show" Then
            TEPinView.Visible = True
            If hdObjectType.Text = "COMP" Then
                showCompany()
            ElseIf hdObjectType.Text = "DEPOT" Then
                showDepo()
            ElseIf hdObjectType.Text = "DRIVER" Then
                showDriver()
            End If
        ElseIf hdValCommand.Text = "pin" Then
            Dim tds As DataTable = TEPinView.CreateDataTableForSelectedRecord()
            Dim vals As New Specialized.NameValueCollection
            vals.Add("NEWPOINTID", hdPointId.Text)
            vals.Add("OBJECTTYPE", hdObjectType.Text)
            TEPinView.PreDefinedValues = vals
        End If
    End Sub

    Private Sub showCompany()
        TEPinView.DefaultDT = "DTShowComp"
        TEPinView.Visible = True
        TEPinView.FilterExpression = "CONSIGNEE='" & hdVal1.Text & "' AND COMPANY='" & hdVal2.Text & "' AND COMPANYTYPE='" & hdVal3.Text & "'"
        TEPinView.Restart()
    End Sub

    Private Sub showDepo()
        TEPinView.DefaultDT = "DTShowDepot"
        TEPinView.Visible = True
        TEPinView.FilterExpression = "DEPOTNAME='" & hdVal1.Text & "'"
        TEPinView.Restart()
    End Sub

    Private Sub showDriver()
        TEPinView.DefaultDT = "DTShowDriver"
        TEPinView.Visible = True
        TEPinView.FilterExpression = "DRIVERID='" & hdVal1.Text & "'"
        TEPinView.Restart()
    End Sub

    Private Sub TEPinView_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEPinView.CreatedChildControls
        With TEPinView.ActionBar
            .AddSpacer()
            .AddExecButton("PointObject", "pin", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnMultiEdit"))
            With .Button("PointObject")
                .ObjectDLL = "WMS.Logic.dll"
                .ObjectName = "WMS.Logic.Pin"
                .CommandName = "pin"
                .ConfirmRequired = True
                .ConfirmMessage = "Are you sure?"
                .EnabledInMode = Made4Net.WebControls.TableEditorMode.View
            End With
        End With
    End Sub

    Private Sub TEPinView_AfterItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles TEPinView.AfterItemCommand
        If e.CommandName = "pin" Then
            TEPinView.RefreshData()
        End If
    End Sub

End Class
