Imports System.Web
Imports System.Web.UI
Imports System.ComponentModel
Imports Made4Net.WebControls.DynamicForms
Imports Made4Net.Schema
Imports Made4Net.DataAccess
Imports Made4Net.Shared.Web

<Designer(GetType(Made4Net.WebControls.DefaultDesigner)), ToolboxData("<{0}:ReportGeneratorUI runat=server></{0}:ReportGeneratorUI>")> _
Public Class ReportGeneratorUI
    Inherits Made4Net.WebControls.ReportGeneratorUI

    Protected Overloads Sub DoAction(ByVal pWhere As WhereCondition)
        EnsureChildControls()

        Dim Producer As Made4Net.Shared.Reporting.ReportProducer

        Dim ContentType As String
        Dim Path As String
        Try

            Select Case _CurrentAction

                Case Made4Net.WebControls.ReportGeneratorAction.Print
                    Dim oQsender As New Made4Net.Shared.QMsgSender
                    oQsender.Add("REPORTNAME", ReportName)
                    oQsender.Add("WAREHOUSE", WMS.Logic.Warehouse.CurrentWarehouse)
                    oQsender.Add("PRINTQPATH", _ActionBar.PrintQPath)
                    oQsender.Add("COPIES", _ActionBar.Copies)
                    oQsender.Add("LANGUAGE", Made4Net.Shared.Translation.Translator.CurrentLanguageID())
                    oQsender.Add("USERID", WMS.Logic.Common.GetCurrentUser())
                    oQsender.Add("WHERE", pWhere.WhereClause)
                    oQsender.Send("Report", ReportName)

                Case Made4Net.WebControls.ReportGeneratorAction.ViewPDF
                    ContentType = Made4Net.Shared.ContentType.PDF
                    Dim AssemblyDllPath As String
                    AssemblyDllPath = getReportsDllPath()
                    Producer = Made4Net.Shared.Reflection.CreateInstance(AssemblyDLL, ClassFullName, Nothing, AssemblyDllPath)
                    Path = Producer.GeneratePDF(pWhere)

            End Select

            If Not _CurrentAction = Made4Net.WebControls.ReportGeneratorAction.Print Then

                With HttpContext.Current.Server
                    Path = .UrlEncode(Path)
                    ContentType = .UrlEncode(ContentType)
                End With

                Dim target As String = "_blank"
                Dim URL As String = MapVirtualPath(String.Format("{0}?ct={1}&path={2}", Made4Net.Shared.Web.M4N_SCREENS.DOC_VIEWER, ContentType, Path))
                If _CurrentAction = Made4Net.WebControls.ReportGeneratorAction.Generate Then
                    Controls.Add(_IFrame)
                    _IFrame.Attributes("src") = URL
                Else
                    _WO.Redirect(URL, target)
                End If

            End If

        Catch ex As Exception
            Me.OnStandarErrorEvent(New Made4Net.WebControls.StandardErrorEventArgs(ex.Message, ex))
        End Try
    End Sub

    Protected Function getReportsDllPath() As String
        Dim path As String
        path = getLanguageReportDll(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        If path Is Nothing Then
            path = getLanguageReportDll(DataInterface.ExecuteScalar(String.Format("SELECT LANGUAGE FROM USERPROFILE WHERE USERID='{0}'", WMS.Logic.GetCurrentUser), Made4Net.Schema.CONNECTION_NAME))
        End If
        Return path
    End Function

    Protected Function getLanguageReportDll(ByVal LanguageId As Int32) As String
        Dim path As String
        Try
            path = DataInterface.ExecuteScalar(String.Format("SELECT ASSEMBLYFILEPATH FROM REPORTLANGUAGE WHERE LANGUAGEID={0}", LanguageId), Made4Net.Schema.CONNECTION_NAME)
        Catch ex As Exception
            Return Nothing
        End Try
        Return path
    End Function
End Class
