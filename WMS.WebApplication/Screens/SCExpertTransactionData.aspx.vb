Imports System.IO
Imports System.Xml
Imports System.Text

Partial Public Class SCExpertTransactionData
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request.QueryString("TransactionId") Is Nothing OrElse Request.QueryString("TransactionId") = String.Empty Then
            Me.Dispose()
        End If
        Dim oDoc As New XmlDocument()
        oDoc.LoadXml("<?xml version=""1.0"" encoding=""utf-8"" ?><TransactionData></TransactionData>")
        Try
            Dim sSql As String = String.Format("select TRANSACTIONDATA from SCEXPERTCONNECTTRANSACTION where TRANSACTIONID = {0}", Request.QueryString("TransactionId"))
            Dim sTransactionData As String = Made4Net.DataAccess.DataInterface.ExecuteScalar(sSql, Made4Net.Schema.CONNECTION_NAME)
            oDoc.SelectSingleNode("TransactionData").InnerXml = sTransactionData
            Response.ContentType = "text/xml"
            Response.Write(oDoc.InnerXml)
        Catch ex As Exception
            oDoc.SelectSingleNode("TransactionData").InnerXml = ex.ToString()
            Response.Write(oDoc.InnerXml)
        End Try
    End Sub

End Class