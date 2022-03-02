Imports WMS.Logic
Public Class VRFIReverse
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        DO1.Value("NOTES") = t.Translate("Would you like to reverse the location picking order?")
    End Sub

    Protected Sub DO1_CreatedChildControls(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddLabelLine("NOTES")
    End Sub

    <CLSCompliant(False)>
    Protected Sub DO1_ButtonClick(ByVal sender As System.Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "yes"
                doUpdReverse()
            Case "no"
                If Not Session("PCKBagOutPicking") Is Nothing Then
                    Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("Screens/PCKBagOut.aspx"))
                Else
                    Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("Screens/PCK.aspx"))
                End If
        End Select
    End Sub

    Public Sub doUpdReverse()
        Dim pcklst As Picklist = Session("PCKPicklist")
        Dim statusval As String = "REVERSE"
        If Not pcklst Is Nothing Then
            Dim sql As String = "update PICKHEADER set PICKORDERSTATUS='{0}' where PICKLIST= '{1}'"
            sql = String.Format(sql, statusval, pcklst.PicklistID)
            Made4Net.DataAccess.DataInterface.RunSQL(sql)
            Session("ReversePickon") = 1
            If Not Session("PCKBagOutPicking") Is Nothing Then
                Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("Screens/PCKBagOut.aspx"))
            Else
                Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("Screens/PCK.aspx"))
            End If

        End If
    End Sub

End Class