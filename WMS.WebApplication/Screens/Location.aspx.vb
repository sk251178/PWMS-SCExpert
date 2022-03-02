Public Class Location
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


#Region "CTor"

    Public Sub New()

    End Sub

    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As New DataSet
        Dim dr As DataRow
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser()
        Dim Location, FRONTRACKLOCATION As String

        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)

        'Select Case CommandName.ToLower
        '    Case "approvelocation"
        Dim loc As New WMS.Logic.Location(Sender, CommandName, XMLSchema, XMLData, Message)

        For Each dr In ds.Tables(0).Rows
            If Not IsDBNull(dr("FRONTRACKLOCATION")) Then
                Location = dr("LOCATION")
                FRONTRACKLOCATION = dr("FRONTRACKLOCATION")
                'FRONTRACKLOCATION
                Dim SQL As String = String.Format("update location set FRONTRACKLOCATION = '{0}' where LOCATION='{1}'", FRONTRACKLOCATION, Location)
                Made4Net.DataAccess.DataInterface.RunSQL(SQL)
            End If

        Next
        'End Select
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
            '.ObjectDLL = "WMS.Logic.dll"
            '.ObjectName = "WMS.Logic.Location"
            .ObjectDLL = "WMS.WebApp.dll"
            .ObjectName = "WMS.WebApp.Location"
            If TEMaster.CurrentMode = Made4Net.WebControls.TableEditorMode.Insert Then
                .CommandName = "createlocation"
            ElseIf TEMaster.CurrentMode = Made4Net.WebControls.TableEditorMode.Edit Then
                .CommandName = "updatelocation"
            ElseIf TEMaster.CurrentMode = Made4Net.WebControls.TableEditorMode.MultilineEdit Then
                .CommandName = "multieditlocation"
            End If
        End With

        With TEMaster.ActionBar
            .AddExecButton("PrintLabels", "Print Label", Made4Net.WebControls.SkinManager.GetImageURL("ReportPrint"))
            With .Button("PrintLabels")
                .ObjectDLL = "WMS.Logic.dll"
                .ObjectName = "WMS.Logic.Location"
                .CommandName = "PrintLabels"
                .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                .ConfirmRequired = True
                .ConfirmMessage = "Are you sure you want to Print Labels?"

            End With
        End With

        If TEMaster.CurrentMode = Made4Net.WebControls.TableEditorMode.Grid Or TEMaster.CurrentMode = Made4Net.WebControls.TableEditorMode.MultilineEdit Then
            With TEMaster.ActionBar
                .AddExecButton("LocaltionEdgeCalc", "Location Edge Calculation", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnPlanWave"))
                With .Button("LocaltionEdgeCalc")
                    .ObjectDLL = "WMS.Logic.dll"
                    .ObjectName = "WMS.Logic.Location"
                    .CommandName = "calculateedges"
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Multiline
                    .ConfirmRequired = True
                    .ConfirmMessage = "Are you sure you want to calculate edges for selected location(s)?"

                End With
            End With
        End If

        If TEMaster.CurrentMode = Made4Net.WebControls.TableEditorMode.Edit Then
            With TEMaster.ActionBar
                .AddExecButton("LocaltionEdgeCalc", "Location Edge Calculation", Made4Net.WebControls.SkinManager.GetImageURL("ActionBarBtnPlanWave"))
                With .Button("LocaltionEdgeCalc")
                    .ObjectDLL = "WMS.Logic.dll"
                    .ObjectName = "WMS.Logic.Location"
                    .CommandName = "calculateedges"
                    .Mode = Made4Net.WebControls.ExecutionButtonMode.Normal
                    .ConfirmRequired = True
                    .ConfirmMessage = "Are you sure you want to calculate edges for selected location(s)?"

                End With
            End With
        End If

    End Sub
    Private Sub TEMaster_AfterItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles TEMaster.AfterItemCommand
        If e.CommandName = "calculateedges" Then
            TEMaster.RefreshData()
        End If
    End Sub
End Class