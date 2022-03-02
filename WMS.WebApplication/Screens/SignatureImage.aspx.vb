Public Class Signature
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

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
        Dim dt As New DataTable
        Dim confDocID As String = Request.Params.Get("CONFDOCID")
        'bol = Request.Params.Get("BOL")
        'orderid = Request.Params.Get("ORDERID")
        'shipdoc = Request.Params.Get("SHIPDOC")
        'Dim SQL As String
        'If orderid = "" Then
        '    Dim CustKey = Request.Params.Get("CUSTOMERKEY")
        '    SQL = String.Format("Select podimage FROM vCustomerDocsSignature where bol = '{0}' and customerkey = '{1}' and shipdoc = '{2}'", bol, CustKey, shipdoc)
        'Else
        '    SQL = String.Format("Select podimage FROM Deliverydocs where bol = '{0}' and Orderid = '{1}' and shipdoc = '{2}'", bol, orderid, shipdoc)
        'End If
        Dim sql As String = String.Format("select documentdata from attachment where documentid='{0}'", confDocID)
        Made4Net.DataAccess.DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count = 0 Then
            'CloseWindow()
        ElseIf IsDBNull(dt.Rows(0)("documentdata")) Then
            'CloseWindow()
        Else
            Response.BinaryWrite(dt.Rows(0)("documentdata"))
        End If
    End Sub

    Private Sub CloseWindow()
        Response.Write("<script language=""javascript"">" & vbCrLf)
        Response.Write("window.close();" & vbCrLf)
        Response.Write("</script>" & vbCrLf)
    End Sub


End Class