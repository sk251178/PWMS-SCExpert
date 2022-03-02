Imports WMS.Logic
Imports Made4Net.Shared.Web
Imports Made4Net.Mobile

Partial Public Class CLD1_OVERRIDEWEIGHT
    Inherits PWMSRDTBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not IsPostBack Then
            Session("PICKINGSRCSCREEN") = Request("sourcescreen")
            Dim osku As New WMS.Logic.SKU(Session("CreateLoadConsignee"), Session("CreateLoadSKU"))

            setScreen(osku)
        End If
    End Sub

    Private Sub clear()
        Session.Remove("PICKINGSRCSCREEN")
        Session.Remove("ERROROVERRIDE")
        Session.Remove("WeightOverridConfirm")
        Session.Remove("CreateLoadUnitsEach")

    End Sub

    Private Sub setScreen(ByVal osku As WMS.Logic.SKU)
       
        DO1.Value("SKU") = osku.SKU
        DO1.Value("SKUDESC") = osku.SKUDESC
        DO1.Value("CapturedWeight") = Session("WeightOverridConfirm")

        Dim WGT As Decimal = 0, TOLPCT As Decimal = 0
        Dim sql As String = String.Format("select isnull(WGT,0) WGT, isnull(TOLPCT,0) TOLPCT from SKUATTRIBUTE where CONSIGNEE = '{0}' and SKU = '{1}'", osku.CONSIGNEE, osku.SKU)

        Dim dt As New DataTable()
        Try
            Made4Net.DataAccess.DataInterface.FillDataset(sql, dt)
            'Added for PWMS-879 and RWMS-948  
            'WGT = Math.Truncate(Decimal.Parse(dt.Rows(0)("WGT").ToString()))
            WGT = Decimal.Parse(dt.Rows(0)("WGT").ToString())
            'Ended for PWMS-879 and RWMS-948  
            TOLPCT = Math.Truncate(Decimal.Parse(dt.Rows(0)("TOLPCT").ToString()))
        Catch
            WGT = 0
            TOLPCT = 0
        End Try

        Try
            DO1.Value("AverageCapturedWeight") = GetWeightPerCase(osku, Session("CreateLoadUnitsEach").ToString) ' Session("CreateLoadUnits").ToString)
        Catch ex As Exception
        End Try
        Dim Err As String = Session("ERROROVERRIDE")
        DO1.Value("ERROR") = Err
        Try
            DO1.Value("ERROR") = Err.Replace(" 0 ", " " & DO1.Value("AverageCapturedWeight") & " ")
        Catch ex As Exception
        End Try

        DO1.Value("WGT") = WGT
        DO1.Value("TOLPCT") = TOLPCT

    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddLabelLine("ERROR")
        DO1.AddLabelLine("SKU")
        DO1.AddLabelLine("SKUDESC")
        Dim scrn As String
        scrn = Session("PICKINGSRCSCREEN")
        'AVERAGEWEIGHT
        DO1.AddLabelLine("AverageCapturedWeight")

        DO1.AddLabelLine("CapturedWeight")

        DO1.AddLabelLine("WGT")
        DO1.AddLabelLine("TOLPCT")

    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "override"
                doOverride()
                clear()
            Case "back"
                doBack()
                clear()
        End Select
    End Sub

    Private Sub doBack()
        Try
            Dim oAttributes As WMS.Logic.AttributesCollection = Session("CreateLoadAttributes")
            oAttributes.Item("WEIGHT") = ""
        Catch ex As Exception
        End Try

        Dim screenid As String = Session("PICKINGSRCSCREEN")

        Dim url As String = Made4Net.DataAccess.DataInterface.ExecuteScalar(String.Format("select url from sys_screen where  screen_id = '{0}'", screenid), "Made4NetSchema")
        Try
            Session("LoadPreviewsVals") = "cld1overrideWGTBACK"
            Response.Redirect(MapVirtualPath(url)) ' & "?sourcescreen=cld1overrideWGTBACK")
        Catch ex As System.Threading.ThreadAbortException

        Catch ex As Exception

        End Try


    End Sub


    Private Sub doOverride()

        '  Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

        Dim screenid As String = Session("PICKINGSRCSCREEN")

        Dim url As String = Made4Net.DataAccess.DataInterface.ExecuteScalar(String.Format("select url from sys_screen where  screen_id = '{0}'", screenid), "Made4NetSchema")
        Try
            Session("LoadPreviewsVals") = "cld1overrideWGT"
            Response.Redirect(MapVirtualPath(url)) '& "?sourcescreen=cld1overrideWGT")
        Catch ex As System.Threading.ThreadAbortException

        Catch ex As Exception

        End Try

    End Sub


    Private Function GetWeightPerCase(ByVal oSku As WMS.Logic.SKU, ByVal units As Integer) As Decimal
        Dim d As Decimal = 0
        Dim dWeight As Decimal = Session("WeightOverridConfirm")

        Try
            d = Math.Round(dWeight / oSku.ConvertUnitsToUom("CASE", units), 2)
        Catch ex As Exception
            d = 0
        End Try
        Return d
    End Function

End Class