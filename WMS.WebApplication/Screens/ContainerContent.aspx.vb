Imports WMS.Logic

Public Class ContainerContent
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    <CLSCompliant(False)> Protected WithEvents TS As Made4Net.WebControls.TabStrip
    Protected WithEvents TEContainerLoads As Made4Net.WebControls.TableEditor
    Protected WithEvents DC1 As Made4Net.WebControls.DataConnector
    Protected WithEvents DTC As Made4Net.WebControls.DataTabControl
    Protected WithEvents Table1 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents Tbl As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents TEContContent As Made4Net.WebControls.TableEditor

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
        'Dim dr As DataRow
        Dim UserId As String = Common.GetCurrentUser()
        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        Select Case CommandName.ToLower
            Case "printpackinglist"
                For Each dr As DataRow In ds.Tables(0).Rows
                    Dim cntr As New WMS.Logic.Container(dr("Container"), True)
                    cntr.PrintContentList("", Made4Net.Shared.Translation.Translator.CurrentLanguageID, WMS.Logic.Common.GetCurrentUser)
                Next
            Case "printshiplabel"
                Dim cntr As New WMS.Logic.Container(ds.Tables(0).Rows(0)("Container"), True)
                cntr.PrintContainerLabel()
        End Select
    End Sub

#End Region

    Private Sub TEContContent_CreatedGrid(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEContContent.CreatedGrid
        TEContContent.Grid.AddExecButton("printshiplabel", "Print Label", "WMS.WebApp.dll", "WMS.WebApp.ContainerContent", 1, Made4Net.WebControls.SkinManager.GetImageURL("LabelPrint"))
    End Sub


    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
    End Sub

    Private Sub TEContContent_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEContContent.CreatedChildControls
        With TEContContent
            With .ActionBar
                .AddSpacer()
                .AddExecButton("printpackinglist", "Print Packing List", Made4Net.WebControls.SkinManager.GetImageURL("ReportPrint"))
                With .Button("printpackinglist")
                    .ObjectDLL = "WMS.WebApp.dll"
                    .ObjectName = "WMS.WebApp.ContainerContent"
                    .CommandName = "printpackinglist"
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                End With
            End With
        End With
    End Sub

End Class