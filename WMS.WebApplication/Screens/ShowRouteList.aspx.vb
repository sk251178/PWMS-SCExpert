Public Class ShowRouteList
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents DTRoutes As Made4Net.WebControls.TableEditor
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents TEROUTE As Made4Net.WebControls.TableEditor

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

    Private Sub TEROUTE_RecordSelected(ByVal sender As Made4Net.WebControls.TableEditor, ByVal e As Made4Net.WebControls.TableEditorEventArgs) Handles TEROUTE.RecordSelected
        Dim dt As DataTable = TEROUTE.CreateDataTableForSelectedRecord()
        Dim dr As DataRow = dt.Rows(0)
        Response.Write("<Script langauge=""vbscript"">" & vbCrLf)
        Response.Write("try {" & vbCrLf)
        Response.Write("window.parent.document.getElementById(""command"").value='showRoute';" & vbCrLf)
        Response.Write("window.parent.document.getElementById(""args"").value='" & dr("VEHICLEROUTEID") & "';" & vbCrLf)
        Response.Write("window.parent.document.getElementById(""btnAct"").click();" & vbCrLf)
        Response.Write("} catch (ex){}" & vbCrLf)
        Response.Write("</Script>" & vbCrLf)
    End Sub
End Class
