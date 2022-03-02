Public Class ExecClass
    Implements Made4Net.Shared.IDataActions


	Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
		'''''''''''''''''''''''''''''''''''''
		'to create a dataset:

		Dim srSchema As New IO.StringReader(XMLSchema)
		Dim srData As New IO.StringReader(XMLData)

		Dim DS As New DataSet
		DS.ReadXmlSchema(srSchema)
		DS.ReadXml(srData)
		'''''''''''''''''''''''''''''''''''''

		Dim xml As String
		Try
			xml = Web.HttpContext.Current.Server.HtmlEncode(XMLData)
		Catch ex As Exception
		End Try

		Dim str As String = String.Format("Sender: {0}<br><br>Command: {1}<br><br>XML: {2}...<br><br>" _
		, Sender.ID, CommandName, xml)

		System.Web.HttpContext.Current.Response.Write(str)

		'Throw New ApplicationException("Test Exception")

		If Now.Second / 2 = Math.Floor(Now.Second / 2) Then
			Message = String.Format("The '{0}' Command executed successfully", CommandName)
		End If

	End Sub
End Class