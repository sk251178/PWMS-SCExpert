Public Class Territory
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents TETerritoryDetail As Made4Net.WebControls.TableEditor
    Protected WithEvents TETerritoryList As Made4Net.WebControls.TableEditor
    Protected WithEvents DC1 As Made4Net.WebControls.DataConnector
    Protected WithEvents DTC As Made4Net.WebControls.DataTabControl
    Protected WithEvents MapTerritory As Made4Net.WebControls.Map
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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
    End Sub

    Private Sub TERouteList_RecordSelected(ByVal sender As Made4Net.WebControls.TableEditor, ByVal e As Made4Net.WebControls.TableEditorEventArgs)
        Dim tempds As DataTable = TETerritoryList.CreateDataTableForSelectedRecord(False)
        Response.Write("<Script langauge=""vbscript"">" & vbCrLf)
        Response.Write("try {" & vbCrLf)
        Response.Write("window.parent.frames(""frmViewData"").document.getElementById(""hdValCommand"").value='show';" & vbCrLf)
        Response.Write("window.parent.frames(""frmViewData"").document.getElementById(""hdVal1"").value='" & tempds.Rows(0)("ROUTEID") & "';" & vbCrLf)
        Response.Write("window.parent.frames(""frmViewData"").document.Form1.submit();" & vbCrLf)
        Response.Write("window.parent.document.getElementById(""command"").value='showroute';" & vbCrLf)
        Response.Write("window.parent.document.getElementById(""args"").value='" & tempds.Rows(0)("ROUTEID") & "';" & vbCrLf)
        Response.Write("window.parent.document.getElementById(""btnAct"").click();" & vbCrLf)
        Response.Write("} catch (ex){}" & vbCrLf)
        Response.Write("</Script>" & vbCrLf)
    End Sub

    Private Sub TERouteList_CreatedChildControls(ByVal sender As System.Object, ByVal e As System.EventArgs)
        With TETerritoryList.ActionBar
            .AddSpacer()
            .AddExecButton("confirm", "Confirm Route", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnApprove"))
            With .Button("confirm")
                .ObjectDLL = "WMS.Logic.dll"
                .ObjectName = "WMS.Logic.Route"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
            End With
        End With
    End Sub

    Private Sub TERouteList_AfterItemCommand(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs)
        If e.CommandName.ToLower = "confirm" Then
            TETerritoryList.RefreshData()
        End If
    End Sub
End Class
