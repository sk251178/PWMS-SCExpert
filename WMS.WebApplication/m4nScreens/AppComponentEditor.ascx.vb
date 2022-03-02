Imports Made4Net.WebControls
Imports Made4Net.WebControls.AppComponents
Imports Made4Net.WebControls.TabStripControl

Public Class AppComponentEditor
    Inherits System.Web.UI.UserControl

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents TEAC As Made4Net.WebControls.TableEditor
    Protected WithEvents TEACVal As Made4Net.WebControls.TableEditor
    Protected WithEvents ph As System.Web.UI.WebControls.PlaceHolder
    <CLSCompliant(False)> Protected WithEvents TS As Made4Net.WebControls.TabStripControl
    Protected WithEvents TabStripControl1 As Made4Net.WebControls.TabStripControl
    Protected WithEvents lblWidth As System.Web.UI.WebControls.Label
    Protected WithEvents lblHeight As System.Web.UI.WebControls.Label
    Protected WithEvents tbWidth As System.Web.UI.WebControls.TextBox
    Protected WithEvents tbHeight As System.Web.UI.WebControls.TextBox
    Protected WithEvents btnApplySize As System.Web.UI.WebControls.Button
    Protected WithEvents T As System.Web.UI.HtmlControls.HtmlTable

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region "const"

    Private Const APP_COMPONENT_DLL As String = "Made4Net.Schema.dll"
    Private Const APP_COMPONENT_CLASS As String = "Made4Net.Schema.AppComponents.AppComponent"

    Private Const DEFAULT_PREVIEW_WIDTH As String = "100%"
    Private Const DEFAULT_PREVIEW_HEIGHT As String = "400px"

#End Region

#Region "var"

    Private WithEvents _ACControl As AppComponentControl
    Private _PreviewCreated As Boolean

#End Region

#Region "property"

    Private Property SelectedAC() As String
        Get
            Return viewstate("SelectedAC")
        End Get
        Set(ByVal Value As String)
            Dim rebuild As Boolean
            If Value <> Me.SelectedAC Then
                rebuild = True
            End If

            viewstate("SelectedAC") = Value

            If rebuild Then
                Me.CreateControls()
            End If
        End Set
    End Property

    Private Property PreviewWidth() As System.Web.UI.WebControls.Unit
        Get
            Try
                Return System.Web.UI.WebControls.Unit.Parse(tbWidth.Text)
            Catch ex As Exception
            End Try
        End Get
        Set(ByVal Value As System.Web.UI.WebControls.Unit)
            Try
                tbWidth.Text = Value.ToString()
            Catch ex As Exception
            End Try
        End Set
    End Property

    Private Property PreviewHeight() As System.Web.UI.WebControls.Unit
        Get
            Try
                Return System.Web.UI.WebControls.Unit.Parse(tbHeight.Text)
            Catch ex As Exception
            End Try
        End Get
        Set(ByVal Value As System.Web.UI.WebControls.Unit)
            Try
                tbHeight.Text = Value.ToString()
            Catch ex As Exception
            End Try
        End Set
    End Property

    Private Property ShouldClearPanelBarState() As Boolean
        Get
            Return ViewState("ShouldClearPanelBarState")
        End Get
        Set(ByVal Value As Boolean)
            ViewState("ShouldClearPanelBarState") = Value
        End Set
    End Property

#End Region

#Region "handler"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If tbWidth.Text = String.Empty Then
            Me.SetDefaultWidth()
        End If
        If tbHeight.Text = String.Empty Then
            Me.SetDefaultHeight()
        End If
    End Sub

    Private Sub TEAC_RecordSelected(ByVal sender As Made4Net.WebControls.TableEditor, ByVal e As Made4Net.WebControls.TableEditorEventArgs) Handles TEAC.RecordSelected
        Dim dt As DataTable = sender.CurrentRecordDataTable
        Dim dr As DataRow = dt.Rows(0)
        Me.SelectedAC = dr("code")
    End Sub

    Private Sub TEAC_RecordUnSelected(ByVal sender As Made4Net.WebControls.TableEditor, ByVal e As Made4Net.WebControls.TableEditorEventArgs) Handles TEAC.RecordUnSelected
        Me.SelectedAC = Nothing
        Me.ShouldClearPanelBarState = True
    End Sub

    Private Sub TEAC_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEAC.CreatedChildControls
        With TEAC.ActionBar
            With .Button("Save")
                .ObjectDLL = APP_COMPONENT_DLL
                .ObjectName = APP_COMPONENT_CLASS
                .CommandName = "Save"

                If TEAC.CurrentMode = Made4Net.WebControls.TableEditorMode.Insert Then
                    .MethodName = "Exec_CreateNew"
                Else
                    .MethodName = "Exec_Update"
                End If
            End With

            With .Button("Delete")
                .ObjectDLL = APP_COMPONENT_DLL
                .ObjectName = APP_COMPONENT_CLASS
                .CommandName = "Delete"
                .MethodName = "Exec_Delete"
            End With

            Dim sm As Skins.SkinManager = Skins.SkinManager.GetInstance(Me.Page)

            .AddSpacer()
            .AddExecButton("Clone", "Clone", sm.GetImageURL("Clone"), Nothing, False)
            With .Button("Clone")
                .ObjectDLL = APP_COMPONENT_DLL
                .ObjectName = APP_COMPONENT_CLASS
                .MethodName = "Exec_Clone"
                .CommandName = "Clone"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Normal
                .EnabledInMode = TableEditorMode.View Or Made4Net.WebControls.TableEditorMode.Edit

                Dim url As String = ScreenList.GetScreenURL("-acco")
                url = Made4Net.Shared.Web.MapVirtualPath(url)
                Dim r As New Random
                url = Made4Net.Shared.Web.AddParamToURL(url, "r", r.Next())
                Dim onclick As String = String.Format("var w = window.radopen(null,null);w.SetUrl('{0}'); w.SetModal(true); w.SetSize(400,300); w.MoveTo(80,120); return false;", url)
                .InnerButton.OnClickClientSide = onclick
            End With
        End With
    End Sub

    Private Sub TEACVal_AfterItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles TEACVal.AfterItemCommand
        If e.CommandName = "Update" Then
            _ACControl.Rebuild()
        End If
    End Sub

    Private Sub btnApplySize_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnApplySize.Click
        Try
            Me.PreviewWidth = System.Web.UI.WebControls.Unit.Parse(tbWidth.Text)
        Catch ex As Exception
            Me.SetDefaultWidth()
        End Try

        Try
            Me.PreviewHeight = System.Web.UI.WebControls.Unit.Parse(tbHeight.Text)
        Catch ex As Exception
            Me.SetDefaultHeight()
        End Try

        Me.BuildPreview()
    End Sub

    Private Sub TEACVal_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEACVal.CreatedChildControls
        If Me.ShouldClearPanelBarState AndAlso Not TEACVal.RecordEditor.Form Is Nothing Then
            TEACVal.RecordEditor.Form.ClearPanelBarState()
            Me.ShouldClearPanelBarState = False
        End If
    End Sub

#End Region

#Region "method"

    Protected Overrides Sub CreateChildControls()
        MyBase.CreateChildControls()
        CreateControls()
    End Sub

    Private Sub CreateControls()
        If Me.SelectedAC Is Nothing Then
            TEACVal.DefaultDT = Nothing
            TEACVal.Restart()
            ph.Controls.Clear()
        Else
            Dim dtName As String = Made4Net.Schema.AppComponents.AppComponent.GetDataTemplateName(Me.SelectedAC)
            If TEACVal.DefaultDT <> dtName Then
                TEACVal.DefaultDT = dtName
                TEACVal.Restart()
            End If

            Me.BuildPreview()
        End If
    End Sub

    Private Sub BuildPreview()
        ph.Controls.Clear()
        If Not Me.SelectedAC Is Nothing Then
            _ACControl = New AppComponentControl
            _ACControl.ID = "ACCtrl"
            _ACControl.AppComponentCode = Me.SelectedAC

            Try
                _ACControl.Width = Me.PreviewWidth
            Catch ex As Exception
            End Try

            Try
                _ACControl.Height = Me.PreviewHeight
            Catch ex As Exception
            End Try

            ph.Controls.Add(_ACControl)
        End If
    End Sub

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        If Me.SelectedAC Is Nothing Then
            T.Visible = False
        Else
            T.Visible = True
        End If

        MyBase.Render(writer)
    End Sub

    Sub SetDefaultWidth()
        Me.PreviewWidth = System.Web.UI.WebControls.Unit.Parse(DEFAULT_PREVIEW_WIDTH)
    End Sub

    Sub SetDefaultHeight()
        Me.PreviewHeight = System.Web.UI.WebControls.Unit.Parse(DEFAULT_PREVIEW_HEIGHT)
    End Sub
#End Region

End Class
