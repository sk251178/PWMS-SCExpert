Public Class LabelPrinters
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
    Private Sub TELabelPrinters_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TELabelPrinters.CreatedChildControls
        If TELabelPrinters.CurrentMode = Made4Net.WebControls.TableEditorMode.Insert Then
            With TELabelPrinters.ActionBar.Button("Save")
                .ObjectDLL = "WMS.Logic.dll"
                .ObjectName = "WMS.Logic.LabelPrinter"
                .CommandName = "createnew"
            End With
        ElseIf TELabelPrinters.CurrentMode = Made4Net.WebControls.TableEditorMode.Edit Then
            With TELabelPrinters.ActionBar.Button("Save")
                .ObjectDLL = "WMS.Logic.dll"
                .ObjectName = "WMS.Logic.LabelPrinter"
                .CommandName = "update"
            End With
        End If
    End Sub
End Class