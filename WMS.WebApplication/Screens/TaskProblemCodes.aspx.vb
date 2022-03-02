Imports Made4Net.Shared.Conversion.Convert
Partial Public Class TaskProblemCodes
    Inherits System.Web.UI.Page
    Private Const ExecDLL As String = "WMS.WebApp.dll"
    Private Const ExecClassName As String = "WMS.WebApp.TaskProblemCodes"
#Region "ViewState"

    Protected Overrides Function LoadPageStateFromPersistenceMedium() As Object
        Return WMS.WebCtrls.WebCtrls.Screen.LoadPageStateFromPersistenceMedium()
    End Function

    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        WMS.WebCtrls.WebCtrls.Screen.SavePageStateToPersistenceMedium(viewState, Me.Page, Screen1.ScreenID)
    End Sub

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
    'RWMS-1747 Start
    Public Sub New() 'Empty default Ctor
    End Sub
    Public Sub New(ByVal Sender As Object, ByVal CommandName As String, ByVal XMLSchema As String, ByVal XMLData As String, ByRef Message As String)
        Dim ds As New DataSet
        Dim dr As DataRow
        ds = Made4Net.Shared.Util.XmlToDS(XMLSchema, XMLData)
        If CommandName = "newTaskProblemCode" Then
            dr = ds.Tables(0).Rows(0)
            Dim objTaskProblemCodes As New WMS.Logic.TaskProblemCodes
            objTaskProblemCodes.CreateTaskProblemCode(dr("PROBLEMCODEID"), ReplaceDBNull(dr("PROBLEMCODEDESC")), ReplaceDBNull(dr("LOCATIONPROBLEM")), ReplaceDBNull(dr("COMPLETETASK")), ReplaceDBNull(dr("COUNTTYPE")), ReplaceDBNull(dr("LOCATIONPROBLEMRC")))
        End If
        If CommandName = "editTaskProblemCode" Then
            dr = ds.Tables(0).Rows(0)
            Dim objTaskProblemCodes As New WMS.Logic.TaskProblemCodes(dr("PROBLEMCODEID"))
            objTaskProblemCodes.UpdateTaskProblemCode(dr("PROBLEMCODEID"), ReplaceDBNull(dr("PROBLEMCODEDESC")), ReplaceDBNull(dr("LOCATIONPROBLEM")), ReplaceDBNull(dr("COMPLETETASK")), ReplaceDBNull(dr("COUNTTYPE")), ReplaceDBNull(dr("LOCATIONPROBLEMRC")))
        End If
    End Sub
    Protected Sub TETaskProblemCodes_CreatedChildControls(sender As Object, e As EventArgs) Handles TETaskProblemCodes.CreatedChildControls
        With TETaskProblemCodes
            With .ActionBar
                With .Button("Save")
                    .ObjectDLL = ExecDLL
                    .ObjectName = ExecClassName
                    If TETaskProblemCodes.CurrentMode = Made4Net.WebControls.TableEditorMode.Insert Then
                        .CommandName = "newTaskProblemCode"
                    ElseIf TETaskProblemCodes.CurrentMode = Made4Net.WebControls.TableEditorMode.Edit Then
                        .CommandName = "editTaskProblemCode"
                    End If
                End With
            End With
        End With
    End Sub
    'RWMS-1747 End
End Class