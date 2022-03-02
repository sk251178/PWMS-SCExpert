Imports system.xml
Imports system.Text
Imports System.IO
Imports System.Data.SqlClient
Imports Made4Net.DataAccess
Imports Made4Net.WebControls.SGPR

<CLSCompliant(False)> _
Partial Public Class SGImport
    Inherits System.Web.UI.Page

    Protected WithEvents _ImportTable As Made4Net.WebControls.Table
    Protected WithEvents _ImportInput As Made4Net.WebControls.FileUploadInput
    Protected WithEvents _ImportBtn As Made4Net.WebControls.Button
    Protected WithEvents _ImportLabel As Made4Net.WebControls.Label
    Protected WithEvents _ImportLogPlaceHolder As PlaceHolder
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        _ImportInput = New Made4Net.WebControls.FileUploadInput
        _ImportInput.ID = "SGImport"

        _ImportInput.Style.Item("width") = "300px"

        _ImportBtn = New Made4Net.WebControls.Button
        _ImportBtn.CausesValidation = False
        _ImportBtn.Text = Made4Net.WebControls.TranslationManager.Translate("Import")
        _ImportBtn.ID = "btnSGImport"

        AddHandler _ImportBtn.Click, AddressOf ImportBtnclick

        _ImportLabel = New Made4Net.WebControls.Label
        _ImportLabel.ID = "SGImportLbl"
        _ImportLabel.Text = "<b>" & Made4Net.WebControls.TranslationManager.Translate("Import Data Template") & "</b>"

        _ImportTable = New Made4Net.WebControls.Table
        _ImportTable.ID = "SGImportTbl"

        _ImportLogPlaceHolder = New PlaceHolder
        _ImportLogPlaceHolder.ID = "PH"



        With _ImportTable
            .Add(_ImportLabel)
            .Add(New LiteralControl("<br>"))

            .AddRow()
            .AddCell(_ImportInput)


            .AddRow()
            .AddCell(_ImportBtn)

            .AddRow()
            .AddCell(_ImportLogPlaceHolder)


        End With
        Page.Form.Controls.Add(_ImportTable)


    End Sub

    Protected Sub ImportBtnclick(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim st As System.IO.Stream = _ImportInput.PostedFile.InputStream
            If st.Length > 0 Then

                Dim ds As New DataSet
                ds.ReadXml(st)
                ImportDSData(ds)
            End If

            Dim jscript As String = "alert('Done.');" & vbCrLf & _
                   "window.close();" & vbCrLf
            ClientScript.RegisterStartupScript(Form.GetType(), "close", jscript, True)

        Catch ex As Exception
            Dim jscript As String = "alert('Error: " & ex.Message & ".');" & vbCrLf & _
                   "window.close();" & vbCrLf
            ClientScript.RegisterStartupScript(Form.GetType(), "close", jscript, True)
        End Try

    End Sub

End Class