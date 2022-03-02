Public Class ShowPoint
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents TEPoint As Made4Net.WebControls.TableEditor
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen

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
        Dim eCommand, FirstPoint, SecondPoint As String
        eCommand = Request.QueryString.Item("action")
        FirstPoint = Request.QueryString.Item("fpoint")
        SecondPoint = Request.QueryString.Item("spoint")
        If eCommand = "showroad" Then
            ShowRoad(FirstPoint, SecondPoint)
        Else
            ShowPoint(FirstPoint)
        End If
    End Sub

    Private Sub ShowRoad(ByVal fPoint As String, ByVal sPoint As String)
        TEPoint.DefaultDT = "DTNETWORK"
        TEPoint.FilterExpression = String.Format("POINTID={0} and NEXTPOINTID={1}", fPoint, sPoint)
        TEPoint.Restart()
    End Sub

    Private Sub ShowPoint(ByVal fPoint As String)
        TEPoint.DefaultDT = "DTPOINT"
        TEPoint.FilterExpression = String.Format("POINTID={0}", fPoint)
        TEPoint.Restart()
    End Sub
End Class
