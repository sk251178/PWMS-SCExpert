Imports System.Collections.Specialized
Imports Made4Net.Schema
Imports Made4Net.WebControls

Namespace ASCX

    Public Class SavedSearch
        Inherits System.Web.UI.UserControl
        Implements IScreenErrorEvents

#Region " Web Form Designer Generated Code "

        'This call is required by the Web Form Designer.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

        End Sub
        Protected WithEvents pnl As System.Web.UI.WebControls.Panel
        Protected WithEvents pnlSave As System.Web.UI.WebControls.Panel
        Protected WithEvents tbSearchName As System.Web.UI.WebControls.TextBox
        Protected WithEvents btnSave As System.Web.UI.WebControls.Button
        Protected WithEvents TE As Made4Net.WebControls.TableEditor
        Protected WithEvents lblName As System.Web.UI.WebControls.Label

        'NOTE: The following placeholder declaration is required by the Web Form Designer.
        'Do not delete or move it.
        Private designerPlaceholderDeclaration As System.Object

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
            'CODEGEN: This method call is required by the Web Form Designer
            'Do not modify it using the code editor.
            InitializeComponent()
        End Sub

#End Region

#Region "Properties"
        Private ReadOnly Property Mode() As String
            Get
                Dim m As String = Request.QueryString("mode")
                If m Is Nothing Then m = ""
                If m = "save" Then
                    Return "save"
                Else
                    Return "manage"
                End If
            End Get
        End Property

        Private ReadOnly Property SCID() As String
            Get
                Return Request.QueryString("scid")
            End Get
        End Property

        Private ReadOnly Property ObjID() As String
            Get
                Return Request.QueryString("objid")
            End Get
        End Property

        Private ReadOnly Property DTID() As Int32
            Get
                Return Convert.ToInt32(Request.QueryString("dtid"))
            End Get
        End Property
#End Region

#Region "Events"
        Public Event StandardErrorEvent(ByVal sender As Object, ByVal e As StandardErrorEventArgs) Implements IScreenErrorEvents.StandardErrorEvent

        Public Event StandardSuccessEvent(ByVal sender As Object, ByVal e As StandardSuccessEventArgs) Implements IScreenErrorEvents.StandardSuccessEvent

        Public Sub HandleException(ByVal ex As Exception)
            RaiseEvent StandardErrorEvent(Me, New StandardErrorEventArgs(ex.Message, ex))
        End Sub
#End Region

#Region "Handlers"
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            If Me.Mode = "save" Then
                pnl.Visible = False
                pnlSave.Visible = True
                lblName.Text = Made4Net.WebControls.TranslationManager.Translate("Name this search") & ":"
            Else
                pnl.Visible = True
                pnlSave.Visible = False

                Dim u As String = Made4Net.Shared.Authentication.User.GetCurrentUser().UserName

                Dim filter As String = String.Format( _
                    "dt_type IN ('USS','SSS') AND username='{0}' AND screen_id='{1}' AND object_id='{2}' AND parent_dt_id={3}", _
                    u, Request.QueryString("scid"), Request.QueryString("objid"), Request.QueryString("parentdtid"))

                TE.FilterExpression = filter

                TE.SortExperssion = "dt_type"
            End If
        End Sub

        Private _RenderCloseScript As Boolean

        Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
            Try
                If tbSearchName.Text = Nothing OrElse tbSearchName.Text = String.Empty Then
                    Throw New ApplicationException("Please enter a name for the search.")
                End If

                Dim uid As String = Made4Net.Shared.Authentication.User.GetCurrentUser().UserName
                If uid Is Nothing OrElse uid = String.Empty Then
                    Throw New ApplicationException("Cannot find logged in user.")
                End If

                'clone the last search
                Dim dt As DataTemplate = DataTemplate.Load(0, Me.DTID, True)
                Made4Net.WebControls.SavedSearch.Clone(dt, tbSearchName.Text, Made4Net.Schema.DT_TYPES.USER_SAVED_SEARCH)

                _RenderCloseScript = True

            Catch ex As Exception
                Me.HandleException(ex)
            End Try
        End Sub

        Private Sub TE_CreatedGrid(ByVal sender As Object, ByVal e As System.EventArgs) Handles TE.CreatedGrid
            Dim sm As Made4Net.WebControls.Skins.SkinManager = Skins.SkinManager.GetInstance(Me.Page)
            With TE.Grid
                .AddExecButton("Del", "Delete", "Made4Net.WebControls.dll", "Made4Net.WebControls.SavedSearchExec", 0, SkinManager.GetImageURL("GridColumnDelete"), "Are you sure?", "if ('[0]'=='SSS') {'0'} else {'1'};field:dt_type")
                .AddExecButton("Load", "Load", "Made4Net.WebControls.dll", "Made4Net.WebControls.SavedSearchExec", 0, SkinManager.GetImageURL("GridColumnSelect"))
            End With
        End Sub

        Private Sub TE_AfterItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles TE.AfterItemCommand
            TE.RefreshData()
        End Sub
#End Region

#Region "Overrides"
        Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
            MyBase.Render(writer)

            If _RenderCloseScript Then
                writer.Write("<script language=jscript>CloseAfterSave();</script>")
            End If
        End Sub
#End Region

    End Class

End Namespace