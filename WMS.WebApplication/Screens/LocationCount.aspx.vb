Public Class LocationCount
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents TS As Made4Net.WebControls.TabStrip
    Protected WithEvents DC1 As Made4Net.WebControls.DataConnector
    Protected WithEvents DC2 As Made4Net.WebControls.DataConnector
    Protected WithEvents DC3 As Made4Net.WebControls.DataConnector
    Protected WithEvents DTC As Made4Net.WebControls.DataTabControl
    Protected WithEvents Table1 As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents Tbl As System.Web.UI.HtmlControls.HtmlTable
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.WebCtrls.WebCtrls.Screen
    Protected WithEvents TEMaster As Made4Net.WebControls.TableEditor
    Protected WithEvents TEDetail1 As Made4Net.WebControls.TableEditor
    Protected WithEvents TEDetail2 As Made4Net.WebControls.TableEditor
    Protected WithEvents TEDetail3 As Made4Net.WebControls.TableEditor
    Protected WithEvents TEDetail4 As Made4Net.WebControls.TableEditor
    Protected WithEvents DC4 As Made4Net.WebControls.DataConnector

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

    End Sub

    Private Sub TEMaster_CreatedGrid(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEMaster.CreatedGrid
        TEMaster.Grid.AddExecButton("PrintLabels", "Print Label", "WMS.Logic.dll", "WMS.Logic.Location", 4, Made4Net.WebControls.SkinManager.GetImageURL("LabelPrint"))
    End Sub

    Protected Sub TEMaster_CreatedChildControls(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TEMaster.CreatedChildControls
        If TEMaster.CurrentMode = Made4Net.WebControls.TableEditorMode.Edit Then
            With TEMaster.ActionBar.Button("Delete")
                .ObjectDLL = "WMS.Logic.dll"
                .ObjectName = "WMS.Logic.Location"
                .CommandName = "delete"
            End With
        End If
        With TEMaster.ActionBar.Button("Save")
            .ObjectDLL = "WMS.Logic.dll"
            .ObjectName = "WMS.Logic.Location"
            If TEMaster.CurrentMode = Made4Net.WebControls.TableEditorMode.Insert Then
                .CommandName = "createlocation"
            ElseIf TEMaster.CurrentMode = Made4Net.WebControls.TableEditorMode.Edit Then
                .CommandName = "updatelocation"
            ElseIf TEMaster.CurrentMode = Made4Net.WebControls.TableEditorMode.MultilineEdit Then
                .CommandName = "multieditlocation"
            End If
        End With
    End Sub
End Class
