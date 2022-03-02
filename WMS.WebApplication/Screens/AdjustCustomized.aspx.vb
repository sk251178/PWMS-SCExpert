Public Class AdjustCustomized
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents Button1 As System.Web.UI.WebControls.Button

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
        'Put user code to initialize the page here
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim sql As String = "select * from dt"
        Dim strArr() As String
        Dim counter As Integer = 0
        Try
            Made4Net.DataAccess.DataInterface.FillDataset(sql, dt, False, Made4Net.Schema.Constants.CONNECTION_NAME)
        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try
        For Each dr In dt.Rows
            strArr = Split(dr("dt_name"), "__")
            If strArr.Length > 1 Then
                sql = "update dt set dt_type = 'CUSTOMIZED', dt_name = '{0}', screen_id = '{1}' , object_id = '{2}' ,username = '{3}'" & _
                    ",mode='{4}' where dt_id = {5} "
                sql = String.Format(sql, strArr(0), strArr(1), strArr(2), strArr(4), strArr(3), dr("dt_id"))
                Made4Net.DataAccess.DataInterface.ExecuteScalar(sql, Made4Net.Schema.Constants.CONNECTION_NAME)
                counter += 1
            End If
        Next
        Response.Write("DONE! number of rows affected: " & counter)
    End Sub
End Class
