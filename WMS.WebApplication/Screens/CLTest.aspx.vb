Imports System.Text

Public Class CLTest
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents TETest As Made4Net.WebControls.TableEditor
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents MPTest As Made4Net.WebControls.Map

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
        'Dim a As New WMS.Logic.Agreement("ZIMM", "ACB")
        'a.process("1", New WMS.Logic.LogHandler("D:\Billing\", "blog.txt"), "SYSTEM")
        'Response.End()
        'Dim att As New WMS.Logic.SkuClass("TSTCLASS")
        'Dim Att As New WMS.Logic.InventoryAttributeBase(Logic.InventoryAttributeBase.AttributeKeyType.Load, "00000000000000000014")
        'Att.Save("NIR")

        'Dim skcls As New WMS.Logic.SkuClass("WEIGHT")
        'skcls = New WMS.Logic.SkuClass("SIZENCOLOR")

        'Response.Write(Att("ExpiryDate") & "<BR>")
        'Response.Write(Att("KOSHER") & "<BR>")
    End Sub

End Class