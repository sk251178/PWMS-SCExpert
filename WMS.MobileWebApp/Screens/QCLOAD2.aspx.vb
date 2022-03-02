Imports Made4Net.Mobile.WebCtrls
Imports Made4Net.Shared.Web
Imports Made4Net.Shared.Translation
Imports Made4Net.Mobile
Imports Made4Net.Shared
Imports Made4Net.DataAccess
Imports WMS.Logic

Partial Public Class QCLOAD2
    Inherits PWMSRDTBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Me.IsPostBack Then
            Me.DO1.Value("LOADID") = Session.Item("LoadId")
            Me.DO1.Value("TEST") = DataInterface.ExecuteScalar("SELECT QCDESC FROM QCHEADER WHERE QCCODE='" & Me.Session.Item("QCCode") & "'")
            Me.setTestList()
        End If
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "ok"
                Me.doNext()
                Exit Select
            Case "menu"
                Me.doMenu()
                Exit Select
            Case "status change"
                Me.StatusChange()
                Exit Select
        End Select
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As EventArgs) Handles DO1.CreatedChildControls
        DO1.AddLabelLine("LOADID")
        DO1.AddLabelLine("TEST")
        DO1.AddSpacer()
        Dim dt As New DataTable
        DataInterface.FillDataset("SELECT * FROM QCDETAIL WHERE QCCODE='" & Session.Item("QCCode") & "'", dt)
        For Each dr As DataRow In dt.Rows
            If Not IsDBNull(dr.Item("CODELIST")) Then
                Me.DO1.AddDropDown(dr.Item("TEST"))
            Else
                Me.DO1.AddTextboxLine(dr.Item("TEST"))
            End If
        Next
    End Sub

    Private Sub doMenu()
        Me.Session.Remove("LoadId")
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub doNext()
        'If Me.Page.IsValid Then
        Dim trans As New Translation.Translator(Translation.Translator.CurrentLanguageID)
        Try
            Me.SaveLoadTest()
        Catch exception1 As M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  exception1.GetErrMessage(Translator.CurrentLanguageID))
            Return
        Catch exception2 As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  exception2.Message)
            Return
        End Try
        HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  trans.Translate("QC Test for load updated", Nothing))
        'End If
    End Sub

    Private Sub SaveLoadTest()
        Dim dt As New DataTable
        DataInterface.FillDataset("SELECT * FROM QCDETAIL WHERE QCCODE='" & Me.Session.Item("QCCode") & "'", dt)
        Try
            For Each dr As DataRow In dt.Rows
                Dim QCCOUNTER As String = Made4Net.Shared.Util.getNextCounter("QCCOUNTER")
                Dim value As String = ""
                If (Not DO1.Value(dr.Item("TEST")) Is Nothing) Then
                    value = DO1.Value(dr.Item("TEST"))
                End If
                DataInterface.RunSQL("INSERT INTO [QCLOADS]([QCID],[QCCODE],[LOADID],[TEST],[VALUE])VALUES('" & QCCOUNTER & "','" _
                    & Session.Item("QCCode") & "','" & Session.Item("LoadId") & "','" & dr.Item("TEST") & "','" & value & "')")
            Next
        Catch exception1 As M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  exception1.GetErrMessage(Translator.CurrentLanguageID))
            Return
        Catch exception2 As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  exception2.Message)
            Return
        End Try
    End Sub

    Private Sub setTestList()
        Dim dt As New DataTable
        DataInterface.FillDataset("SELECT * FROM QCDETAIL WHERE QCCODE='" & Session.Item("QCCode") & "'", dt)
        Try
            For Each dr As DataRow In dt.Rows
                If Not IsDBNull(dr.Item("CODELIST")) Then
                    Dim dd As Made4Net.WebControls.MobileDropDown = DO1.Ctrl(dr.Item("TEST"))
                    dd.AllOption = False
                    dd.TableName = "CODELISTDETAIL"
                    dd.ValueField = "CODE"
                    dd.TextField = "DESCRIPTION"
                    dd.Where = "CODELISTCODE = '" & dr.Item("CODELIST") & "'"
                    dd.DataBind()
                End If
            Next
        Catch exception1 As M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  exception1.GetErrMessage(Translator.CurrentLanguageID))
            Return
        Catch exception2 As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  exception2.Message)
            Return
        End Try
    End Sub

    Private Sub StatusChange()
        Me.Response.Redirect(MapVirtualPath("Screens/QCLOAD3.aspx", False))
    End Sub

End Class