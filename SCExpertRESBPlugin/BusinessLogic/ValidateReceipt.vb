Imports System.Xml
Imports Made4Net.DataAccess
Imports WMS.Logic

Public Class ValidateReceipt
    Public Logger As Made4Net.Shared.Logging.LogFile

    Public Sub New(ByRef Logger As Made4Net.Shared.Logging.LogFile)
        Me.Logger = Logger

    End Sub

    Public Function ValidateReceipt(ByVal outDoc As XmlDocument, ByRef errorMessage As String) As Boolean
        Dim RET As Boolean = True
        Try
            Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

            Logger.WriteLine("start run on xml: DATACOLLECTION/DATA/LINES/LINE")

            For Each oDetailLine As XmlNode In outDoc.SelectNodes("DATACOLLECTION/DATA/LINES/LINE")
                Dim CONSIGNEE As String = oDetailLine.SelectSingleNode("CONSIGNEE").InnerText
                Dim COMPANY As String = oDetailLine.SelectSingleNode("COMPANY").InnerText
                Dim COMPANYTYPE As String = oDetailLine.SelectSingleNode("COMPANYTYPE").InnerText
                Dim QTYORIGINAL As String = oDetailLine.SelectSingleNode("QTYORIGINAL").InnerText
                Dim QTYEXPECTED As String = oDetailLine.SelectSingleNode("QTYEXPECTED").InnerText
                Dim INPUTQTY As String = oDetailLine.SelectSingleNode("INPUTQTY").InnerText

                Logger.WriteLine("CONSIGNEE:" & CONSIGNEE)
                Logger.WriteLine("COMPANY:" & COMPANY)
                Logger.WriteLine("COMPANYTYPE:" & COMPANYTYPE)
                Logger.WriteLine("INPUTQTY:" & INPUTQTY)
                Logger.WriteLine("QTYORIGINAL:" & QTYORIGINAL)
                Logger.WriteLine("QTYEXPECTED:" & QTYEXPECTED)

                Dim d As Double
                If Not WMS.Logic.Company.Exists(CONSIGNEE, COMPANY, COMPANYTYPE) Then
                    errorMessage = t.Translate("Company does not exist") & " " & COMPANY
                    Return False               
                End If
                If Not Double.TryParse(INPUTQTY, d) Then
                    errorMessage = t.Translate("INPUTQTY must be numeric")
                    Return False
                End If
                If Not Double.TryParse(QTYORIGINAL, d) Then
                    errorMessage = t.Translate("QTYORIGINAL must be numeric")
                    Return False
                End If
                If Not Double.TryParse(QTYEXPECTED, d) Then
                    errorMessage = t.Translate("QTYEXPECTED must be numeric")
                    Return False
                End If
            Next

        Catch
        End Try

        Return RET
    End Function

End Class
