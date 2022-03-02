Imports Made4Net.Shared.Web
Imports Made4Net.Mobile

<CLSCompliant(False)> Public Class RePrintLbl
    Inherits PWMSRDTBase

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents DO1 As Made4Net.Mobile.WebCtrls.DataObject

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
        If WMS.Logic.GetCurrentUser Is Nothing Then
            WMS.Logic.GotoLogin()
        End If
        If WMS.Logic.GetCurrentUser = "" Then
            WMS.Logic.GotoLogin()
        End If
        If Not IsPostBack Then
            Dim dd As Made4Net.WebControls.MobileDropDown
            dd = DO1.Ctrl("DOCTYPE")
            dd.AllOption = False
            dd.TableName = "CODELISTDETAIL"
            dd.ValueField = "CODE"
            dd.TextField = "DESCRIPTION"
            dd.Where = "CODELISTCODE='REPRNDOCTYPE'"
            dd.DataBind()

        End If
    End Sub

    Private Sub doMenu()
        Session.Remove("CONSINGEE")
        Session.Remove("DOCID")
        Session.Remove("DOCTYPE")
        Session.Remove("PRINTER")
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub doBack()
        Session.Remove("CONSINGEE")
        Session.Remove("DOCID")
        Session.Remove("DOCTYPE")
        Session.Remove("PRINTER")
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub doNext()
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        If Session("CONSINGEE") <> "" Then
            Try
                Dim oConsignee As New WMS.Logic.Consignee(Session("CONSINGEE"))
            Catch ex As Exception
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Consignee not found"))
            End Try
        Else
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Consignee not found"))
            Exit Sub
        End If

        If Session("DOCID") <> "" Then
            If Session("PRINTER") <> "" Then
                If Session("DOCTYPE") <> "" Then
                    Select Case Convert.ToString(Session("DOCTYPE"))
                        Case "FLOWTHRH"
                            Dim oTS As WMS.Logic.TransShipment
                            Try
                                oTS = New WMS.Logic.TransShipment(Session("CONSINGEE"), Session("DOCID"))
                                oTS.PrintLabel(Session("PRINTER"))
                                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Lables printed"))
                                Response.Redirect(MapVirtualPath("Screens/RePrintLbl.aspx"))
                            Catch ex As Exception
                                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("TransShipment entered inncorrect"))
                                Exit Sub
                            End Try
                        Case "TRNSHPMNT"
                            Dim oFT As WMS.Logic.Flowthrough
                            Try
                                oFT = New WMS.Logic.Flowthrough(Session("CONSINGEE"), Session("DOCID"))
                                oFT.PrintLabel(Session("PRINTER"))
                                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Lables printed"))
                                Response.Redirect(MapVirtualPath("Screens/RePrintLbl.aspx"))
                            Catch ex As Exception
                                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Flowthrough entered inncorrect"))
                                Exit Sub
                            End Try
                    End Select
                Else
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Docid should not be empty"))
                    Exit Sub
                End If
            Else
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Printer name should not be empty"))
                Exit Sub
            End If
        Else
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Doctype should not be empty"))
            Exit Sub
        End If
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddTextboxLine("CONSINGEE")
        DO1.AddTextboxLine("DOCID")
        DO1.AddDropDown("DOCTYPE")
        DO1.AddTextboxLine("PRINTER")
        DO1.AddSpacer()
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick

        Session("CONSINGEE") = DO1.Value("CONSINGEE")
        Session("DOCID") = DO1.Value("DOCID")
        Session("DOCTYPE") = DO1.Value("DOCTYPE")
        Session("PRINTER") = DO1.Value("PRINTER")

        Select Case e.CommandText.ToLower
            Case "print"
                doNext()
            Case "menu"
                doMenu()
            Case "back"
                doBack()
        End Select
    End Sub

End Class
