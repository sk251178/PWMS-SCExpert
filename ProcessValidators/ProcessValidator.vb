Imports Made4Net.DataAccess
Imports Made4Net.Shared
Imports System.Globalization

Public Class ProcessValidator

    Public Shared Function ValidateReceiptClose(ByVal ReceiptID As String) As String
        Dim _receipt As New WMS.Logic.ReceiptHeader(ReceiptID)

        Dim dtException As New DataTable
        Dim sSql As String = String.Format("select RECEIPTLINE,SUM(qty) as qty from RECEIVINGEXCEPTION where RECEIPT = '{0}' group by RECEIPT,RECEIPTLINE", ReceiptID)
        DataInterface.FillDataset(sSql, dtException)

        Dim sFilter As String = String.Empty
        Dim dExceptionQty As Decimal = 0
        Dim drArray() As DataRow
        For Each _recline As WMS.Logic.ReceiptDetail In _receipt.LINES
            sFilter = String.Format("RECEIPTLINE = {0}", _recline.RECEIPTLINE)
            drArray = dtException.Select(sFilter)
            If drArray.Length = 0 Then
                dExceptionQty = 0
            Else
                dExceptionQty = drArray(0)("qty")
            End If
            If _recline.QTYRECEIVED + dExceptionQty < _recline.QTYEXPECTED Then
                Return "-1"
            End If
        Next

        Return "0"
    End Function



    Public Shared Function ValidateExpiryDate(EXPIRYDATE As String, DAYSTORECEIVE As String) As String
        Try
            Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.[Shared].Translation.Translator.CurrentLanguageID)

            If DAYSTORECEIVE = "0" Then
                Return "1"
            End If
            Dim dEXPIRYDATE As DateTime
            ' = DateTime.Parse(EXPIRYDATE);
            If Not DateTime.TryParseExact(EXPIRYDATE, Made4Net.[Shared].SysParam.[Get]("System_dateformat"), CultureInfo.InvariantCulture, DateTimeStyles.None, dEXPIRYDATE) Then
                Return "-1;" + ("Illegal EXPIRYDATE format")
            End If


            Dim dExpired As DateTime = DateTime.Today.AddDays(Double.Parse(DAYSTORECEIVE))



            If dExpired.CompareTo(dEXPIRYDATE) < 0 Then
                Return "1;"
            End If
            '  Made4Net.DataAccess.Collections.GenericCollection cParam = new Made4Net.DataAccess.Collections.GenericCollection();

            'cParam.Add("#param0#", dEXPIRYDATE.ToString(Made4Net.Shared.Util.GetSystemParameterValue("System_dateformat")));
            'cParam.Add("#param1#", dExpired.ToString(Made4Net.Shared.Util.GetSystemParameterValue("System_dateformat")));
            Dim retMsg As String = t.Translate("payload expiry date #param0# is sooner than allowed to receive for this product (#param1#)", Nothing)
            retMsg = retMsg.Replace("#param0#", dEXPIRYDATE.ToString(Made4Net.[Shared].SysParam.[Get]("System_dateformat")))
            retMsg = retMsg.Replace("#param1#", dExpired.ToString(Made4Net.[Shared].SysParam.[Get]("System_dateformat")))
            ' return "0;" +  t.Translate("payload expiry date #param0# is sooner than allowed to receive for this product (#param1#)", cParam);

            '                return "0;" + string.Format("payload expiry date #param0# is sooner than allowed to receive for this product (#param1#)", dEXPIRYDATE.ToString(Made4Net.Shared.Util.GetSystemParameterValue("System_dateformat")), dExpired.ToString(Made4Net.Shared.Util.GetSystemParameterValue("System_dateformat")));
            Return Convert.ToString("0;") & retMsg
        Catch ex As Exception
            Return (Convert.ToString((Convert.ToString("-1;") & EXPIRYDATE) + " ") & DAYSTORECEIVE) + " " + ex.Message
        End Try

    End Function

    Public Shared Function ValidateTestW(CONSIGNEE As String, SKU As String) As String
        Dim WGT As Decimal = 0, TOLPCT As Decimal = 10
        Dim WEIGHT As Decimal = 12
        If WEIGHT > WGT AndAlso WEIGHT < TOLPCT Then
            Return "1;"
        End If
        Return Convert.ToString("1;work proper") & SKU
    End Function



    'validate weight for rdt.fullpick/rdt.partialpick/rdt.parallel pick
    Public Shared Function ValidateWeightCase(CONSIGNEE As String, SKU As String, CASEWEIGHT As String) As String
        'string SkuClassName = new SKU(CONSIGNEE, SKU, true).SKUClass.ClassName;
        Try

            Dim WGT As Decimal = 80, TOLPCT As Decimal = 10
            Dim WEIGHT As Decimal = System.Convert.ToDecimal(CASEWEIGHT)

            Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.[Shared].Translation.Translator.CurrentLanguageID)

            Dim sql As String = String.Format("select count(1) from SKUATTRIBUTE where CONSIGNEE = '{0}' and SKU = '{1}'", CONSIGNEE, SKU)

            Try
                If CInt(Made4Net.DataAccess.DataInterface.ExecuteScalar(sql, Made4Net.DataAccess.DataInterface.ConnectionName)) = 0 Then
                    Return "1;"
                End If
            Catch ex As Exception
                '  WGT = 0; TOLPCT = 0; 
                Return (Convert.ToString((Convert.ToString((Convert.ToString("-1;") & CONSIGNEE) + " ") & SKU) + " ") & CASEWEIGHT) + " " + ex.Message
            End Try


            sql = String.Format("select isnull(WGT,0) WGT, isnull(TOLPCT,0) TOLPCT from SKUATTRIBUTE where CONSIGNEE = '{0}' and SKU = '{1}'", CONSIGNEE, SKU)

            Dim dt As New DataTable()
            Try
                Made4Net.DataAccess.DataInterface.FillDataset(sql, dt, False, Made4Net.DataAccess.DataInterface.ConnectionName)
                WGT = Decimal.Parse(dt.Rows(0)("WGT").ToString())
                TOLPCT = Decimal.Parse(dt.Rows(0)("TOLPCT").ToString())
            Catch ex As Exception
                '  WGT = 0; TOLPCT = 0; 
                Return (Convert.ToString((Convert.ToString((Convert.ToString("-1;") & CONSIGNEE) + " ") & SKU) + " ") & CASEWEIGHT) + " " + ex.Message
            End Try

            If WGT = 0 OrElse TOLPCT = 0 Then
                Return "1;"
            End If

            Dim borderUnits As Decimal = (WGT / 100) * TOLPCT
            Dim min As Decimal, max As Decimal
            min = WGT - borderUnits
            max = WGT + borderUnits

            If WEIGHT >= min AndAlso WEIGHT <= max Then
                Return "1;"
            Else
                Dim retMsg As String = t.Translate("Case weight #param0# out of tolerance", Nothing)
                retMsg = retMsg.Replace("#param0#", CASEWEIGHT)
                Return Convert.ToString("0;") & retMsg
            End If
        Catch ex As Exception
            Return (Convert.ToString((Convert.ToString((Convert.ToString("-1;") & CONSIGNEE) + " ") & SKU) + " ") & CASEWEIGHT) + " " + ex.Message
        End Try

    End Function


End Class
