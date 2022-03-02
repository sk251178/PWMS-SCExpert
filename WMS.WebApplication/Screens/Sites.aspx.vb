Imports WMS.Logic
Imports Made4Net.DataAccess
Imports Made4Net.Shared.Conversion.Convert

Public Class Sites
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents TESites As Made4Net.WebControls.TableEditor
    <CLSCompliant(False)> Protected WithEvents TS As Made4Net.WebControls.TabStrip
    Protected WithEvents DTC As Made4Net.WebControls.DataTabControl
    Protected WithEvents Table1 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents Tbl As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents TESiteComp As Made4Net.WebControls.TableEditor
    Protected WithEvents TEAssignComp As Made4Net.WebControls.TableEditor
    Protected WithEvents DC1 As Made4Net.WebControls.DataConnector

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


#Region "Ctors"

    Public Sub New()

    End Sub

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As New DataSet
        Dim dr As DataRow
        Dim UserId As String = Common.GetCurrentUser()
        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        Select Case CommandName
            Case "Assign"
                Dim sitePID, contId As String
                For Each dr In ds.Tables(0).Rows
                    sitePID = dr("SITEPOINTID")
                    contId = dr("CONTACTID")
                    AssignCompanyToSite(sitePID, contId)
                Next
            Case "DeAssign"
                Dim sitePID, contId As String
                For Each dr In ds.Tables(0).Rows
                    sitePID = ""
                    contId = dr("CONTACTID")
                    DeAssignCompanyToSite(sitePID, contId)
                Next
        End Select
    End Sub

#End Region

#Region "Methods"

    Private Sub AssignCompanyToSite(ByVal SitePointId As String, ByVal pContactId As String)
        Dim ocont As New Contact(pContactId)
        ocont.setPointId(SitePointId, Common.GetCurrentUser)
        ocont = Nothing
    End Sub

    Private Sub DeAssignCompanyToSite(ByVal SitePointId As String, ByVal pContactId As String)
        Dim ocont As New Contact(pContactId)
        ocont.setPointId(SitePointId, Common.GetCurrentUser)
        ocont = Nothing
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
    End Sub

    Private Sub TEAssignComp_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEAssignComp.CreatedChildControls
        TEAssignComp.ActionBar.AddSpacer()
        TEAssignComp.ActionBar.AddExecButton("Assign", "Assign Company To Site", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnMultiEdit"))
        With TEAssignComp.ActionBar.Button("Assign")
            .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
            .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
            .ObjectDLL = "WMS.WebApp.dll"
            .ObjectName = "WMS.WebApp.Sites"
        End With
        Dim tds As DataTable = TESites.CreateDataTableForSelectedRecord()
        Dim vals As New Specialized.NameValueCollection
        vals.Add("SITEPOINTID", tds.Rows(0)("pointid"))
        TEAssignComp.PreDefinedValues = vals
    End Sub

    Private Sub TESiteComp_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TESiteComp.CreatedChildControls
        TESiteComp.ActionBar.AddSpacer()
        TESiteComp.ActionBar.AddExecButton("DeAssign", "DeAssign Company To Site", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnMultiEdit"))
        With TESiteComp.ActionBar.Button("DeAssign")
            .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
            .EnabledInMode = Made4Net.WebControls.TableEditorMode.Grid
            .ObjectDLL = "WMS.WebApp.dll"
            .ObjectName = "WMS.WebApp.Sites"
        End With

    End Sub


End Class