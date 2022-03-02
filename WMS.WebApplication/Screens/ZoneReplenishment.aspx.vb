Imports WMS.Logic
Imports System.Data
Imports System.Xml
Imports Made4Net.Shared

Public Class ZoneReplenishment
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents TEReplenish As Made4Net.WebControls.TableEditor

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
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser()
        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        Select Case CommandName.ToLower
            Case "replenish"
                For Each dr In ds.Tables(0).Rows
                    Dim oZoneRepl As New WMS.Logic.ZoneReplenish(dr("PUTREGION"), dr("SKU"), dr("CONSIGNEE"))
                    oZoneRepl.Execute()
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
            .AddExecButton("zonereplenish", "Replenish", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnPlanWave"))
            With .Button("zonereplenish")
                .ObjectDLL = "WMS.WebApp.dll"
                .ObjectName = "WMS.WebApp.ZoneReplenishment"
                .CommandName = "Replenish"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
            End With
        End With
    End Sub

End Class