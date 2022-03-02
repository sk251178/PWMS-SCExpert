Imports Made4Net.DataAccess
Imports Made4Net.Shared
Imports Made4Net.WebControls
Imports Made4Net.WebControls.DynamicForms
Imports Made4Net.Schema
Imports System.Data
Imports System.Reflection

Public Class ImportManagerRun
    Inherits System.Web.UI.UserControl

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents ph As System.Web.UI.WebControls.PlaceHolder
    Protected WithEvents LinkButton1 As System.Web.UI.WebControls.LinkButton
    Protected WithEvents TE As Made4Net.WebControls.TableEditor

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()

        If Not Page.IsPostBack Then
            Me.SetDT(False)
        End If
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    End Sub

    Private Sub SetDT(ByVal allowRestart As Boolean)
        EnsureChildControls()
        TE.DefaultDTObject = Me.BuildDT()
        If allowRestart AndAlso Page.IsPostBack Then
            TE.Restart()
        End If
    End Sub

    Public ReadOnly Property ImportCode() As String
        Get
            Return Request.QueryString("ImportCode")
        End Get
    End Property

    Public ReadOnly Property IsRadWindow() As Boolean
        Get
            Return False
            'Dim b As String = Request.QueryString("IsRadWindow")
            'If Not b Is Nothing Then
            '    Return Made4Net.Shared.Conversion.Convert.ParseBoolean(b)
            'End If
        End Get
    End Property

    Private Sub TE_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TE.CreatedChildControls
        AddHandler TE.ScreenObject.StandardErrorEvent, AddressOf Me.Screen_StandardErrorEvent
        AddHandler TE.ScreenObject.StandardSuccessEvent, AddressOf Me.Screen_StandardSuccessEvent

        With TE.ActionBar
            Dim attr As New ExecutionButtonAttributes
            With attr
                .Behavior = ExecutionButtonBehavior.Execute
                .CausesValidation = True
                .EnabledInMode = TableEditorMode.All
                .ImageURL = TE.ScreenObject.SkinManager.GetImageURL("ActionBarBtnImport")
                .Name = "RunImport"
                .Text = "Import"

                .ObjectInvokeAttributes = New ObjectInvokeAttributes
                With .ObjectInvokeAttributes
                    .ButtonMode = ExecutionButtonMode.Normal
                    .DLL = "Made4Net.Shared.dll"
                    .ClassName = "Made4Net.Shared.DataImport.DataImportHeader"
                    .CommandName = "RunImport"
                    .MethodName = "Exec_RunImport"
                End With

            End With
            .AddExecButton(attr)

            If Me.IsRadWindow Then
                Dim radClose As New ExecutionButtonAttributes
                With radClose
                    .Behavior = ExecutionButtonBehavior.CloseRadWindow
                    .EnabledInMode = TableEditorMode.All
                    .ImageURL = TE.ScreenObject.SkinManager.GetImageURL("ActionBarBtnClose")
                    .Name = "radClose"
                    .Text = "Close"
                End With
                .AddExecButton(radClose)
            End If

            'Dim cancelAttr As New ExecutionButtonAttributes
            'With cancelAttr
            '    .Behavior = ExecutionButtonBehavior.CloseRadWindow
            '    .CausesValidation = False
            '    .ImageURL = TE.ScreenObject.SkinManager.GetImageURL("ActionBarBtnCancel")
            '    .Name = "CancelImport"
            '    .Text = "Cancel"
            'End With
            '.AddExecButton(cancelAttr)

        End With

        Me.ImportTypeSelector.AutoPostBack = True
        AddHandler Me.ImportTypeSelector.SelectedIndexChanged, AddressOf HandleImportTypeChange

        Dim s As String = "<script>var ImportTypeId = '{0}';</script>"
        s = String.Format(s, Me.ImportTypeSelector.UniqueID)
        Page.ClientScript.RegisterClientScriptBlock(Nothing, "ImportTypeScriptBlock", s)

        If Not Page.IsPostBack Then
            'simulate selection change
            HandleImportTypeChange(Me.ImportTypeSelector, New EventArgs)
        End If
    End Sub

    Property SelectedImport() As String
        Get
            Return ViewState("SelectedImport")
        End Get
        Set(ByVal Value As String)
            ViewState("SelectedImport") = Value
        End Set
    End Property

    Private Sub HandleImportTypeChange(ByVal sender As Object, ByVal args As EventArgs)
        ChangeImportType()
    End Sub

    Private Sub ChangeImportType()
        Me.SelectedImport = Me.ImportTypeSelector.Value
        Me.SetDT(True)
    End Sub

    Private Function BuildDT() As DataTemplate
        Dim paramDt As DataTemplate
        If Not Me.ImportCode Is Nothing Then
            paramDt = Me.GetImportTypeDT(Me.ImportCode)
        Else
            If Not Me.SelectedImport Is Nothing AndAlso Not Me.SelectedImport = String.Empty Then
                paramDt = Me.GetImportTypeDT(Me.SelectedImport)
            End If
        End If

        Dim dt As DataTemplate
        If paramDt Is Nothing Then
            dt = DataTemplate.CreateBlank()
            dt.FormTemplate = "{TOP:n1}"
        Else
            dt = paramDt
        End If

        Dim displayId As String = "TOP"

        Dim importTypeDefaultVal As String
        Dim importTypeReadOnly As Boolean
        If Me.ImportCode Is Nothing Then
            importTypeDefaultVal = Me.SelectedImport
            importTypeReadOnly = False
        Else
            importTypeDefaultVal = Me.ImportCode
            importTypeReadOnly = True
        End If

        Dim dtfImport As New DTField(dt, "ImportType", False, GetType(String).FullName, importTypeDefaultVal,
            DisplayType.SYSTEM_DISPLAY_TYPES.DROP_DOWN, "Import", 255, 10, importTypeReadOnly, True, DTFlagCollection.DTFlagTrue,
            Nothing, displayId, Nothing, LabelPosition.Top, LabelAlign.Left, False, False,
            False, Nothing, 0, 0, DTFlagCollection.DTFlagFalse, False, False, 0, Nothing, Nothing, Nothing, False)

        With dtfImport
            .SetProperty("connection", CONNECTION_NAME)
            .SetProperty("table", Made4Net.Shared.TABLES.IMPORT_HEADER)
            .SetProperty("value_field", "code")
            .SetProperty("text_field", "name")
            .SetProperty("all", "true")
        End With

        dt.Fields.Add(dtfImport)

        Dim dtfFile As New DTField(dt, "File", False, GetType(String).FullName, Nothing,
            DisplayType.SYSTEM_DISPLAY_TYPES.FILE, "File", 255, 20, False, True, DTFlagCollection.DTFlagTrue, Nothing,
            displayId, Nothing, LabelPosition.Top, LabelAlign.Left, False, False, False, Nothing,
            0, 0, DTFlagCollection.DTFlagFalse, False, False, 0, Nothing, Nothing, Nothing, False)

        dt.Fields.Add(dtfFile)

        Return dt
    End Function

    ReadOnly Property ImportTypeSelector() As Made4Net.WebControls.DropDownList
        Get
            Return CType(TE.RecordEditor.Form.Field("ImportType"), DisplayTypes.DropDownList).ValueCtrl
        End Get
    End Property

    Private Function GetImportTypeDT(ByVal importCode As String) As DataTemplate
        Dim b As New Made4Net.Schema.DTBuilders.DataImportDTBuilder(importCode)
        Dim dtName As String = b.GetDTName()

        Dim dt As DataTemplate = DataTemplate.Load(0, dtName, True)
        Return dt
    End Function

    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        'Me.ImportTypeSelector.Value = Me.SelectedImport
        MyBase.Render(writer)
    End Sub

    Protected Overrides Sub LoadViewState(ByVal savedState As Object)
        MyBase.LoadViewState(savedState)

        If Page.IsPostBack Then
            Me.SetDT(False)
        End If
    End Sub

    Private Sub Screen_StandardSuccessEvent(ByVal sender As Object, ByVal e As Made4Net.WebControls.StandardSuccessEventArgs)
        ShowImportLog(e.Argument)
    End Sub

    Private Sub Screen_StandardErrorEvent(ByVal sender As Object, ByVal e As Made4Net.WebControls.StandardErrorEventArgs)
        ShowImportLog(e.Argument)
    End Sub

    Private Sub ShowImportLog(ByVal results As Object)
        If Not results Is Nothing Then
            Try
                Dim r As DataImport.DataImportResults = results

                Dim sb As New System.Text.StringBuilder
                sb.AppendFormat("<a href=""#"" onclick=""javascript:var a = document.getElementById('taImportResults'); a.style.display=''; this.style.display='none'; return false;"">{0}</a><br />", TranslationManager.Translate("Show Log"))
                sb.Append("<textarea style=""display:none; font-family:Courier New; font-size:12px"" id=taImportResults rows=17 cols=70>" & results.Log & "</textarea><br /><br />")

                ph.Controls.Add(New LiteralControl(sb.ToString()))

            Catch ex As InvalidCastException
            End Try
        End If
    End Sub

End Class
