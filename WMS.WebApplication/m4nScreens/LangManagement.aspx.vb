Imports Made4Net.WebControls

Public Class LangManagement
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents TS As Made4Net.WebControls.TabStrip
    Protected WithEvents DC1 As Made4Net.WebControls.DataConnector
    Protected WithEvents DTC As Made4Net.WebControls.DataTabControl
    Protected WithEvents Table1 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents Tbl As System.Web.UI.HtmlControls.HtmlTable
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents TEMaster As Made4Net.WebControls.TableEditor
    Protected WithEvents TEDetail1 As Made4Net.WebControls.TableEditor

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
        'Put user code to initialize the page here
    End Sub

    Private Sub TEMaster_CreatedGrid(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEMaster.CreatedGrid
        With TEMaster
            If Not .Grid Is Nothing Then
                .Grid.DisableTranslation = True

                Dim DLL As String = "Made4Net.WebControls.dll"
                Dim ClassName As String = "Made4Net.WebControls.LanguageManagement"

                .Grid.AddExecButton("Refresh", "Refresh", DLL _
                 , ClassName, 0, Screen1.SkinManager.GetImageURL("GridColumnEdit"))

                .Grid.AddExecButton("Clone", "Clone", DLL _
                 , ClassName, 0, Screen1.SkinManager.GetImageURL("GridColumnEdit"))
            End If
        End With
    End Sub

    Private Sub TEMaster_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEMaster.CreatedChildControls
        With TEMaster.ActionBar
            .AddSpacer()

            .AddExecButton("ReloadVocab", "Reload Vocabulary", Screen1.SkinManager.GetImageURL("ActionBarBtnEdit"))
            With .Button("ReloadVocab")
                .ObjectDLL = "Made4Net.WebControls.dll"
                .ObjectName = "Made4Net.WebControls.LanguageManagement"
                .CommandName = "ReloadVocab"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Normal
                .EnabledInMode = Made4Net.WebControls.TableEditorMode.Edit Or Made4Net.WebControls.TableEditorMode.Grid Or Made4Net.WebControls.TableEditorMode.Insert Or Made4Net.WebControls.TableEditorMode.MultilineEdit Or Made4Net.WebControls.TableEditorMode.Search Or Made4Net.WebControls.TableEditorMode.View
            End With
        End With
    End Sub

    Private Sub TEMaster_GridItemCommand(ByVal source As Object, ByVal e As Made4Net.WebControls.DataGridCommandEventArgs2) Handles TEMaster.GridItemCommand
        Select Case e.CommandName
            Case "Clone"
                TEMaster.RefreshData()
            Case "Refresh"
                Try
                    TEDetail1.RefreshData()
                Catch ex As Exception
                End Try
        End Select
    End Sub

    Private Sub TEDetail1_CreatedGrid(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEDetail1.CreatedGrid
        If Not TEDetail1.Grid Is Nothing Then
            TEDetail1.Grid.DisableTranslation = True
            Dim saveButton As ExecutionButton = TEDetail1.ActionBar.Button("Save")

            Dim AllPageSelected As String = ViewState("chkMultiSelect_SelectAllPagesID")
            If Not saveButton Is Nothing Then
                If Not String.IsNullOrEmpty(AllPageSelected) And Not String.IsNullOrEmpty(ID) Then
                    saveButton.CustomConfirmValidationMessage = "Are you sure want to update all records in from all pages?"
                    saveButton.CustomConfirmValidationRequired = True
                    saveButton.Script = "<script language=""JavaScript"">function m4nButton_CustomConfirm(t){var e=""" + AllPageSelected + """;return document.getElementById(e)&&document.getElementById(e).checked?confirm(t)?!0:!1:!0}</script>"

                End If
            End If
        End If
    End Sub

    Protected Sub TEDetail1_BeforeModeChange(sender As TableEditor, e As TableEditorEventArgs)
        If Not TEDetail1.Grid Is Nothing Then
            Dim saveButton As ExecutionButton = TEDetail1.ActionBar.Button("Save")
            If ViewState("chkMultiSelect_SelectAllPagesID") Is Nothing Then
                If Not String.IsNullOrEmpty(TEDetail1.MultiSelectSelectAllPagesID) Then
                    ViewState("chkMultiSelect_SelectAllPagesID") = TEDetail1.MultiSelectSelectAllPagesID
                End If
            End If
        End If
    End Sub

End Class
