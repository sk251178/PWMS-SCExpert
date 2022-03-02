Public Class SignatureView
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
        'Dim bol, orderid, shipdoc As String
        Dim dt As New DataTable
        Dim confDocID As String = Request.Params.Get("confdocid")
        'orderid = Request.Params.Get("ORDERID")
        'shipdoc = Request.Params.Get("SHIPDOC")
        Dim sql As String = String.Format("select documentdata from attachment where documentid='{0}'", confDocID)
        'If orderid = "" Then
        '    Dim CustKey = Request.Params.Get("CUSTOMERKEY")
        '    SQL = String.Format("Select podimage FROM vCustomerDocsSignature where bol = '{0}' and customerkey = '{1}' and shipdoc = '{2}'", bol, CustKey, shipdoc)
        'Else
        '    SQL = String.Format("Select podimage FROM Deliverydocs where bol = '{0}' and Orderid = '{1}' and shipdoc = '{2}'", bol, orderid, shipdoc)
        'End If

        Made4Net.DataAccess.DataInterface.FillDataset(sql, dt)
        If dt.Rows.Count = 0 Then
            CloseWindow()
        ElseIf IsDBNull(dt.Rows(0)("documentdata")) Then
            CloseWindow()
        Else
            'Response.BinaryWrite(dt.Rows(0)("podimage"))
            Response.Write("<script language=javascript type=text/javascript>" & vbCrLf)

            Response.Write("view_image = new Image();" & vbCrLf)
            Response.Write("view_image.onload = open_image;" & vbCrLf)
            Response.Write("view_image.onerror = image_size_failed;" & vbCrLf)
            'Response.Write("view_image.src = ""SignatureImage.aspx?BOL=" + Request.Params.Get("BOL") + "&ORDERID=" + Request.Params.Get("ORDERID") + "&SHIPDOC=" + Request.Params.Get("SHIPDOC") + "&CUSTOMERKEY=" + Request.Params.Get("CUSTOMERKEY") + """;" & vbCrLf)
            Response.Write("view_image.src = ""SignatureImage.aspx?CONFDOCID=" + confDocID & """;" & vbCrLf)

            Response.Write("function open_image()   {" & vbCrLf)
            Response.Write("var w = view_image.width;" & vbCrLf)
            'Response.Write("if (!(w==480)) {" & vbCrLf)
            'Response.Write("w=480;" & vbCrLf)
            Response.Write("          //window.screen.width=w+20;" & vbCrLf)
            Response.Write("          //window.screen.height=view_image.height+20;" & vbCrLf)
            Response.Write("          window.resizeTo(w+50,view_image.height+200);" & vbCrLf)
            'Response.Write("} " & vbCrLf)


            Response.Write("document.getElementById('signimg').innerHTML='<img align=center border=0  src = ""SignatureImage.aspx?CONFDOCID=" + confDocID & """/>';" & vbCrLf)
            Response.Write("view_image=null;" & vbCrLf)
            Response.Write(" }" & vbCrLf)
            Response.Write("function image_size_failed() {" & vbCrLf)
            Response.Write("alert('could not load image');" & vbCrLf)
            Response.Write("}" & vbCrLf)
            Response.Write("</script>" & vbCrLf)
            Response.Write("<a style=""FONT-SIZE: 12px;MARGIN: 1px 0px 2px;COLOR: #315173;FONT-FAMILY: arial,ms Sans serif"" id=""HyperLink1"" href=""javascript:window.print()"">הדפסה</a>" & vbCrLf)
            Response.Write("<div align=""center"" name=""signimg"" id=""signimg""></div>")
        End If
    End Sub

    Private Sub CloseWindow()
        Response.Write("<script language=""javascript"">" & vbCrLf)
        Response.Write("window.close();" & vbCrLf)
        Response.Write("</script>" & vbCrLf)
    End Sub


End Class