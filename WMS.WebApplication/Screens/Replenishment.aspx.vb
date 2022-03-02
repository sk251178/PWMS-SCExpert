Imports WMS.Logic

Public Class Replenishment
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents TEReplenish As Made4Net.WebControls.TableEditor

    Protected WithEvents txtMinFactor As Made4Net.WebControls.TextBoxValidated
    Protected WithEvents txtTimeLimit As Made4Net.WebControls.TextBoxValidated

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

#Region "CTor"

    Public Sub New()

    End Sub

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As New DataSet
        Dim dr As DataRow
        Dim UserId As String = Common.GetCurrentUser()
        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        Select Case CommandName.ToLower
            Case "normalreplenishoverrideminqty"
                Dim iVal As Int32 = CType(CType(Sender, WebControl).Page.FindControl("txtMinFactor"), Made4Net.WebControls.TextBoxValidated).Value
                If iVal <= 0 Then
                    Throw New Made4Net.Shared.M4NException(New Exception, "Invalid minimum qty for replenishment override, use normal replenishment instead", "Invalid minimum qty for replenishment override, use normal replenishment instead")
                End If
                For Each dr In ds.Tables(0).Rows
                    Dim oRepl As New NormalReplenish()
                    oRepl.NormalReplenish(dr("Location"), dr("Warehousearea"), dr("consignee"), dr("sku"), "", False, iVal, -1)
                Next
            Case "normalreplenishoverridetimelimit"
                Dim iVal As Int32 = CType(CType(Sender, WebControl).Page.FindControl("txtTimeLimit"), Made4Net.WebControls.TextBoxValidated).Value
                If iVal <= 0 Then
                    Throw New Made4Net.Shared.M4NException(New Exception, "Invalid time interval for replenishment override", "Invalid time interval for replenishment override")
                End If
                For Each dr In ds.Tables(0).Rows
                    Dim oRepl As New NormalReplenish()
                    oRepl.NormalReplenish(dr("Location"), dr("Warehousearea"), dr("consignee"), dr("sku"), "", False, -1, iVal)
                Next
        End Select
    End Sub

#End Region


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
    End Sub

    Private Sub TEReplenish_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEReplenish.CreatedChildControls
        With TEReplenish.ActionBar
            .AddSpacer()
            .AddExecButton("normalreplenish", "Replenish", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnPlanWave"))
            With .Button("normalreplenish")
                .ObjectDLL = "WMS.Logic.dll"
                .ObjectName = "WMS.Logic.NormalReplenish"
                .CommandName = "Replenish"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
            End With

            .AddSpacer()
            .AddExecButton("normalreplenishoverrideminqty", "Override Replenishment with minimum quantity", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarReplLoc"))
            With .Button("normalreplenishoverrideminqty")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.Replenishment"
                .CommandName = "normalreplenishoverrideminqty"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
            End With

            .AddSpacer()
            .AddExecButton("normalreplenishoverridetimelimit", "Override Replenishment with time limits", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnSoftPlanWave"))
            With .Button("normalreplenishoverridetimelimit")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.Replenishment"
                .CommandName = "normalreplenishoverridetimelimit"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
            End With
        End With
    End Sub

    'Protected Sub txtMinFactor_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMinFactor.TextChanged

    'End Sub
End Class
