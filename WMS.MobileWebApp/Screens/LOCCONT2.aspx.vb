Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports Made4Net.DataAccess
Imports Made4Net.Shared

<CLSCompliant(False)> Partial Class LOCCONT2
    Inherits PWMSRDTBase

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.MobileWebApp.WebCtrls.Screen

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
        Return Session("_ViewState")
    End Function

    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        Session("_ViewState") = viewState
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Try

            If Session("SKU") <> "" And Session("CONSIGNEE") <> "" Then
                Dim oSku As WMS.Logic.SKU = New WMS.Logic.SKU(Session("CONSIGNEE"), Session("SKU"))
                Dim oLoc As WMS.Logic.Location
                oLoc = New WMS.Logic.Location(Session("LOCATION"), Session("WAREHOUSEAREA"))
                Dim SQLStr As String = "SELECT INVLOAD.LOADID, UNITSAVAILABLE FROM INVLOAD WHERE LOCATION='" & oLoc.Location & "' AND WAREHOUSEAREA='" & oLoc.Warehousearea & "' AND CONSIGNEE='" & oSku.CONSIGNEE & "' AND SKU='" & oSku.SKU & "'"
                Dim dt As DataTable = New DataTable
                DataInterface.FillDataset(SQLStr, dt)
                DO1.AddLabelLine("SKU")
                DO1.AddLabelLine("SKUDESC")
                DO1.AddLabelLine("LOADID")
                DO1.Value("LOADID") = "QTY".PadLeft(13)
                DO1.Value("SKU") = oSku.SKU
                DO1.Value("SKUDESC") = oSku.SKUDESC

                For Each dr As DataRow In dt.Rows
                    DO1.AddLabelLine(dr("LOADID"))
                    DO1.Value(dr("LOADID")) = dr("UNITSAVAILABLE")
                    DO1.AddSpacer()
                Next
            Else
                Dim oLoc As WMS.Logic.Location
                oLoc = New WMS.Logic.Location(Session("LOCATION"), Session("WAREHOUSEAREA"))
                Dim SQLStr As String = "SELECT SKU.CONSIGNEE, dbo.SKU.SKU,dbo.SKU.SKUDESC  FROM dbo.SKU INNER JOIN dbo.INVLOAD ON dbo.SKU.CONSIGNEE = dbo.INVLOAD.CONSIGNEE AND dbo.SKU.SKU = dbo.INVLOAD.SKU WHERE LOCATION='" & oLoc.Location & "' and WAREHOUSEAREA='" & oLoc.Warehousearea & "' GROUP BY SKU.CONSIGNEE, SKU.SKU, dbo.SKU.SKUDESC "
                Dim dt As DataTable = New DataTable
                DataInterface.FillDataset(SQLStr, dt)

                If dt.Rows.Count > 0 Then
                    Dim contstr As String
                    contstr = "<table width=100% class=RDTContentTable cellspacing=0 cellpadding=0 border=0><tr><td>Location Content</td><tr><td width=100%>"
                    For Each dr As DataRow In dt.Rows

                        contstr = contstr & "<table>"
                        contstr = contstr & "<tr><td>LOCATION : " & Session("LOCATION") & "</td></tr>"
                        contstr = contstr & "<tr><td>WAREHOUSEAREA : " & Session("WAREHOUSEAREA") & "</td></tr>"
                        contstr = contstr & "<tr><td>CONSIGNEE : " & dr("CONSIGNEE") & "</td></tr>"
                        contstr = contstr & "<tr><td>SKU : " & dr("SKU") & "</td></tr>"
                        contstr = contstr & "<tr><td>DESC : " & dr("SKUDESC") & "</td></tr>"
                        contstr = contstr & "</table>"
                        contstr = contstr & "</td></tr>"
                        Dim Loads As DataTable = New DataTable
                        DataInterface.FillDataset("SELECT LOADID, UNITSAVAILABLE AS QTY FROM INVLOAD WHERE LOCATION='" & oLoc.Location & "' and WAREHOUSEAREA='" & oLoc.Warehousearea & "' AND CONSIGNEE='" & dr("CONSIGNEE") & "' AND SKU='" & dr("SKU") & "' ORDER BY LOADID ", Loads)

                        contstr = contstr & "<tr><td>"
                        contstr = contstr & "<table border=1>"
                        contstr = contstr & "<tr>"
                        contstr = contstr & "<td width=220>LOADID</td>"
                        contstr = contstr & "<td width=70>QTY</td>"
                        contstr = contstr & "</tr>"
                        For Each ld As DataRow In Loads.Rows
                            contstr = contstr & "<tr>"
                            contstr = contstr & "<td width=220>" & ld("LOADID") & "</td>"
                            contstr = contstr & "<td width=70>" & ld("QTY") & "</td>"
                            contstr = contstr & "</tr>"
                        Next
                        contstr = contstr & "</table>"
                        contstr = contstr & "</td></tr>"
                        contstr = contstr & "<tr><td width=300>"
                    Next
                    contstr = contstr & "</td></tr></table>"

                    DO1.Title = contstr

                End If
            End If

        Catch m4nEx As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  m4nEx.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Return
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate(ex.Message))
            Return
        End Try

    End Sub

    Private Sub doMenu()
        Session.Remove("SKU")
        Session.Remove("CONSIGNEE")
        Session.Remove("LOCATION")
        Session.Remove("WAREHOUSEAREA")
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "menu"
                doMenu()
        End Select
    End Sub

End Class
