Imports WMS.WebCtrls
Partial Public Class ViewStateManager
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsNothing(HttpContext.Current.Request.Params("ViewStateCmd")) Then
            Dim oCmd As String = HttpContext.Current.Request.Params("ViewStateCmd").ToLower
            Select Case oCmd
                Case "changeid"
                    Dim ViewStateID As String = HttpContext.Current.Request.Params("ViewStateId")
                    ViewStateID = WMS.WebCtrls.WebCtrls.Screen.ChangeViewStateID(ViewStateID)
                    Response.Write(ViewStateID)
                    Response.End()
                Case Else
                    Response.Write("Not Supported")
                    Response.End()
            End Select


        End If
    End Sub

End Class