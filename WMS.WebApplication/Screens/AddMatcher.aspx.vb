Public Class AddMatcher
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents TEAddMatcher As Made4Net.WebControls.TableEditor
    Protected WithEvents hdVal1 As System.Web.UI.WebControls.TextBox
    Protected WithEvents hdVal2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents hdVal3 As System.Web.UI.WebControls.TextBox
    Protected WithEvents hdObjectType As System.Web.UI.WebControls.TextBox
    Protected WithEvents hdValCommand As System.Web.UI.WebControls.TextBox
    Protected WithEvents hdPointId As System.Web.UI.WebControls.TextBox

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
        Return WMS.WebCtrls.WebCtrls.Screen.LoadPageStateFromPersistenceMedium()
    End Function

    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        WMS.WebCtrls.WebCtrls.Screen.SavePageStateToPersistenceMedium(viewState, Me.Page, Screen1.ScreenID)
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If hdValCommand.Text = "hide" Then
            TEAddMatcher.Visible = False
        ElseIf hdValCommand.Text = "show" Then
            'TEAddMatcher.Visible = True
            If hdObjectType.Text = "COMP" Then
                showCompany()
            ElseIf hdObjectType.Text = "DEPOT" Then
                'showDepo()
            ElseIf hdObjectType.Text = "DRIVER" Then
                'showDriver()
            End If
        ElseIf hdValCommand.Text = "pin" Then
            Dim tds As DataTable = TEAddMatcher.CreateDataTableForSelectedRecord()
            Dim vals As New Specialized.NameValueCollection
            vals.Add("NEWPOINTID", hdPointId.Text)
            vals.Add("OBJECTTYPE", hdObjectType.Text)
            TEAddMatcher.PreDefinedValues = vals
        End If
    End Sub

    Private Sub showCompany()
        TEAddMatcher.DefaultDT = "DTAddMatch"
        TEAddMatcher.Visible = True
        Session("cntctid") = hdVal1.Text
        Session("cntcttype") = "COMPANY"
        Dim fltexp As String = String.Format("CONTACTID='{0}' AND CONTACTTYPE='COMPANY'", hdVal1.Text)
        If Not TEAddMatcher.FilterExpression = fltexp Then
            TEAddMatcher.FilterExpression = fltexp
            TEAddMatcher.Restart()
        End If
    End Sub

    Private Sub TEAddMatcher_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEAddMatcher.CreatedChildControls
        With TEAddMatcher.ActionBar

            '.AddExecButton("pin", "Pin", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnApprove"))
            'With .Button("pin")
            '    .ObjectDLL = "WMS.Logic.dll"
            '    .ObjectName = "WMS.Logic.TMSWebSupport"
            '    .EnabledInMode = Made4Net.WebControls.TableEditorMode.Insert
            'End With

            With .Button("save")
                .ObjectDLL = "WMS.Logic.dll"
                .ObjectName = "WMS.Logic.TMSWebSupport"
                '.EnabledInMode = Nothing
                '.Visible = False
            End With
        End With
    End Sub

    Private Sub TEAddMatcher_AfterItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles TEAddMatcher.AfterItemCommand
        If e.CommandName.ToLower = "save" Then
            Response.Write("<script language=""javascript"">" & vbCrLf)
            Response.Write("try {" & vbCrLf)
            Response.Write("window.parent.frames(""frmPinGrid"").document.getElementById(""hdValCommand"").value='refresh';" & vbCrLf)
            Response.Write("window.parent.frames(""frmPinGrid"").document.Form1.submit();" & vbCrLf)
            Response.Write("} catch (ex){}" & vbCrLf)
            Response.Write("</script>" & vbCrLf)
        End If
    End Sub
End Class