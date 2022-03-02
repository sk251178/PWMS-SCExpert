Imports Made4Net.DataAccess

Public Class SQLQuery
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents DropDownListValidated1 As Made4Net.WebControls.DropDownListValidated
    Protected WithEvents btnRun As System.Web.UI.WebControls.Button
    Protected WithEvents ddConn As System.Web.UI.WebControls.DropDownList
    Protected WithEvents tbSQL As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblInfo As System.Web.UI.WebControls.Label

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
        Try
            If Not IsPostBack Then
                Me.BindConnection()
            End If

        Catch ex As Exception
            Screen1.HandleException(ex)
        End Try
    End Sub

    Protected Sub BindConnection()
        ddConn.Items.Clear()

        Dim sql As String = "select * from conn"
        Dim d As New Data(sql, Made4Net.Schema.CONNECTION_NAME)
        Dim dt As DataTable = d.CreateDataTable(False, False)
        For Each dr As DataRow In dt.Rows
            ddConn.Items.Add(New System.Web.UI.WebControls.ListItem(String.Format("{0} ({1})", dr("conn_label"), dr("conn_dsn")), dr("record_id")))
        Next
    End Sub

    Private Sub btnRun_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRun.Click
        Try
            lblInfo.Text = String.Empty

            Dim sql As String = tbSQL.Text
            sql = sql.Trim()
            If sql = String.Empty Then
                Throw New ApplicationException("Query is empty.")
            End If
            Dim conn As New Made4Net.Schema.Connection(Convert.ToInt32(ddConn.SelectedValue))

            Dim time1 As DateTime = DateTime.Now
            Dim result As Int32 = DataInterface.RunSQL(sql, conn.GetODBCConnectionInfo())
            Dim time2 As DateTime = DateTime.Now

            Dim duration As TimeSpan = DateTime.op_Subtraction(time2, time1)
            lblInfo.Text = String.Format("{0:0.00} Seconds. ", duration.TotalSeconds)
            If result >= 0 Then
                lblInfo.Text &= String.Format("{0} Records Affected.", result)
            End If

        Catch ex As Exception
            Screen1.HandleException(ex)
        End Try
    End Sub
End Class
