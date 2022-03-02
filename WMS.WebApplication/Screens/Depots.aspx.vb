Public Class Depots
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents TEDepot As Made4Net.WebControls.TableEditor
    Protected WithEvents TEConsigneeContact As Made4Net.WebControls.TableEditor
    Protected WithEvents Dataconnector3 As Made4Net.WebControls.DataConnector
    Protected WithEvents DTC As Made4Net.WebControls.DataTabControl
    Protected WithEvents Tbl As System.Web.UI.HtmlControls.HtmlTable
    <CLSCompliant(False)> Protected WithEvents TS As Made4Net.WebControls.TabStrip
    Protected WithEvents Table1 As System.Web.UI.HtmlControls.HtmlTable

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

#Region "Ctor"

    Public Sub New()

    End Sub

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As DataSet = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        Dim user As String = WMS.Logic.Common.GetCurrentUser
        Dim dr As DataRow
        If CommandName.ToLower = "insertdepot" Then
            dr = ds.Tables(0).Rows(0)
            Dim oDepot As New WMS.Logic.Depots
            oDepot.Insert(dr("depotname"), dr("depottype"), Made4Net.Shared.Conversion.Convert.ReplaceDBNull("description"), user)
        ElseIf CommandName.ToLower = "editdepot" Then
            dr = ds.Tables(0).Rows(0)
            Dim oDepot As New WMS.Logic.Depots(dr("depotname"))
            oDepot.Edit(dr("depottype"), Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dr("description")), user)
        End If
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub TEDepot_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEDepot.CreatedChildControls
        With TEDepot.ActionBar.Button("Save")
            .ObjectDLL = "WMS.WebApp.dll"
            .ObjectName = "WMS.WebApp.Depots"
            If TEDepot.CurrentMode = Made4Net.WebControls.TableEditorMode.Insert Then
                .CommandName = "insertdepot"
            Else
                .CommandName = "editdepot"
            End If
        End With
    End Sub

    'Private Sub TEConsigneeContact_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEConsigneeContact.CreatedChildControls
    '    With TEConsigneeContact.ActionBar
    '        .AddSpacer()
    '        .AddExecButton("setpoint", "Point Depot", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarApprovePicks"))
    '        With .Button("setpoint")
    '            .ObjectDLL = "WMS.WebApp.dll"
    '            .ObjectName = "WMS.WebApp.Depots"
    '            .CommandName = "setpoint"
    '        End With
    '    End With
    'End Sub

End Class
