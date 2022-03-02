Public Class ReportPrinters
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
    Private Sub TEReportPrinters_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles TEReportPrinters.CreatedChildControls
        If TEReportPrinters.CurrentMode = Made4Net.WebControls.TableEditorMode.Insert Then
            With TEReportPrinters.ActionBar.Button("Save")
                .ObjectDLL = "WMS.Logic.dll"
                .ObjectName = "WMS.Logic.ReportPrinter"
                .CommandName = "createnew"
            End With
        ElseIf TEReportPrinters.CurrentMode = Made4Net.WebControls.TableEditorMode.Edit Then
            With TEReportPrinters.ActionBar.Button("Save")
                .ObjectDLL = "WMS.Logic.dll"
                .ObjectName = "WMS.Logic.ReportPrinter"
                .CommandName = "update"
            End With
        End If
    End Sub
End Class