Imports Made4Net.DataAccess
Imports Made4Net.Shared.Evaluation
Imports WMS.Logic

Public Class PriceFactorCalc
    Protected BillingCalcParameters As New Hashtable

    Sub New()
    End Sub

    Public Function CalculatePriceFactor(ByVal pPriceFactorIndex As String, _
                                    ByVal pDate As String, _
                                    ByVal logger As WMS.Logic.LogHandler) As Double
        Try
            Dim SQL As String

            ''load fixed  params
            SQL = String.Format("SELECT PARAMID,PARAMVALUE FROM BILLINGFIXEDPRICEFACTORPARAMS" & _
                " union SELECT bd.PARAMID,PARAMVALUE FROM BILLINGDAILYPRICEFACTORPARAMS bd  join BILLINGDAILYPRICEFACTORPARAMSVALUES bv on bd.PARAMID=bv.PARAMID " & _
                " where convert(DATETIME,PARAMDATE,103)=convert(DATETIME,'{0}',103) ", pDate)

            Dim dt As New DataTable
            DataInterface.FillDataset(SQL, dt)
            For Each dr As DataRow In dt.Rows
                addCalcParameters(dr("PARAMID").ToString(), dr("PARAMVALUE").ToString())
            Next

            '' get formula
            SQL = String.Format("select isnull(CALCULATIONEQUATION,1) as CALCULATIONEQUATION from BILLINGPRICEFACTORINDEX where PRICEFACTORINDEX='{0}' ", _
                pPriceFactorIndex)

            Dim dtEq As New DataTable
            DataInterface.FillDataset(SQL, dtEq)
            If dtEq.Rows.Count = 0 Then
                Return 1D
            End If

            ''run calculation
            Dim LogPath As String = String.Empty
            If Not logger Is Nothing Then LogPath = logger.LogFilePath

            Dim SourceEquation As String = dtEq.Rows(0)("CALCULATIONEQUATION").ToString()
            Dim oEvalEquation As EvalEquation = New EvalEquation(Nothing, BillingCalcParameters, LogPath)
            Dim TargetEquation As String = oEvalEquation.Bind(SourceEquation, 0)
            Dim res As Double = oEvalEquation.EvalEquation(SourceEquation, TargetEquation)

            If res = 0D Then res = 1D
            Return res
        Catch ex As Exception
            Return 1D
        End Try

    End Function

    Protected Sub addCalcParameters(ByVal key As String, ByVal value As String)
        Dim isnewValue As Boolean = True
        If Not BillingCalcParameters.Contains(key) Then
            BillingCalcParameters.Add(key, value)
        Else
            If BillingCalcParameters(key) = value Then
                isnewValue = False
            Else
                BillingCalcParameters(key) = value
            End If
        End If
    End Sub

End Class