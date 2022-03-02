Imports System.Xml
Imports Made4Net.DataAccess
Imports wms.Logic

Public Class ValidateOutbound
    Public Logger As Made4Net.Shared.Logging.LogFile

    Public Sub New(ByRef Logger As Made4Net.Shared.Logging.LogFile)
        Me.Logger = Logger

    End Sub

    Public Function ValidateOutbound(ByVal outDoc As XmlDocument, ByRef errorMessage As String) As Boolean
        Dim RET As Boolean = True
        Try
            Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

            Logger.WriteLine("start run on xml: DATACOLLECTION/DATA")

            For Each oDetailLine As XmlNode In outDoc.SelectNodes("DATACOLLECTION/DATA")
                Dim CONSIGNEE As String = oDetailLine.SelectSingleNode("CONSIGNEE").InnerText
                Dim ORDERID As String = oDetailLine.SelectSingleNode("ORDERID").InnerText
                ' Dim ORDERLINE As String = oDetailLine.SelectSingleNode("ORDERLINE").InnerText

                Logger.WriteLine("CONSIGNEE:" & CONSIGNEE)
                Logger.WriteLine("ORDERID:" & ORDERID)
                ' Logger.WriteLine("ORDERLINE:" & ORDERLINE)

                Dim Sql As String
                'Sql = " select COUNT(1) from PICKDETAIL where CONSIGNEE='{0}' and ORDERID='{1}' and ORDERLINE= '{2}'"
                'Sql = String.Format(Sql, CONSIGNEE, ORDERID, ORDERLINE)

                'Sql = Made4Net.DataAccess.DataInterface.ExecuteScalar(Sql)
                ''                            xmlFilled.SelectSingleNode("DATACOLLECTION").RemoveChild(xmlFilled.SelectSingleNode(string.Format("DATACOLLECTION/DATA[FLOWTHROUGH='{0}']", OrderId)));// removing the node inOrder to send it to SCExpertConnect

                'If Sql <> "0" Then
                '    errorMessage = t.Translate("Cannot edit line - there is an open picklist for the line")
                '    Return False
                'End If
                '
                Sql = "select COUNT(1) from vEditOrderDetail where CONSIGNEE='{0}' and ORDERID='{1}' "
                Sql = String.Format(Sql, CONSIGNEE, ORDERID)

                Sql = Made4Net.DataAccess.DataInterface.ExecuteScalar(Sql)

                If Sql <> "0" Then
                    errorMessage = t.Translate("Cannot edit line - order status is incorrect")
                    Return False
                End If

            Next

        Catch
        End Try

        Return RET
    End Function

End Class
