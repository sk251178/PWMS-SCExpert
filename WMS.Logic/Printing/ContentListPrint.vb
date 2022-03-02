Imports Made4Net.DataAccess
Imports Made4Net.Shared
Public Class ContentListPrinter
    Public Shared Sub PrintContainerContentList(ByVal contentContainer As IContainerContentList,
                                                ByVal lblPrinter As String,
                                                ByVal Language As Int32,
                                                ByVal pPicklistID As String,
                                                ByVal pUser As String,
                                                Optional ByVal pReportName As String = "ContentList")
        Dim oQsender As IQMsgSender = QMsgSender.Factory.Create()
        Dim repType As String
        Dim dt As New DataTable
        repType = pReportName
        DataInterface.FillDataset(String.Format("SELECT * FROM SYS_REPORTPARAMS WHERE REPORTID = '{0}'", repType), dt, False)
        oQsender.Add("REPORTNAME", repType)
        oQsender.Add("REPORTID", repType)
        Try
            oQsender.Add("DATASETID", dt.Select("ParamName='DATASETNAME'")(0)("ParamValue")) ' "repContentList")
        Catch ex As Exception
            oQsender.Add("DATASETID", "repContentList")
        End Try

        oQsender.Add("FORMAT", "PDF")
        oQsender.Add("WAREHOUSE", Warehouse.CurrentWarehouse)
        oQsender.Add("USERID", pUser)
        oQsender.Add("LANGUAGE", Language)
        Try
            oQsender.Add("PRINTER", lblPrinter)
            Try
                oQsender.Add("COPIES", dt.Select("ParamName='Copies'")(0)("ParamValue"))
            Catch ex As Exception
                oQsender.Add("COPIES", 1)
            End Try

            If Made4Net.Shared.Util.GetSystemParameterValue("ReportServicePrintMode") = "SAVE" Then
                oQsender.Add("ACTION", WMS.Lib.REPORTACTIONTYPE.SAVE)
            Else
                oQsender.Add("ACTION", WMS.Lib.REPORTACTIONTYPE.PRINT)
            End If
        Catch ex As Exception
        End Try
        oQsender.Add("WHERE", contentContainer.ContentContainerWhereClause(pPicklistID))
        oQsender.Send("Report", repType)

    End Sub
End Class
