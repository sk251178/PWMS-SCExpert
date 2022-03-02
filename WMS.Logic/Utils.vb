Imports Made4Net.DataAccess

<CLSCompliant(False)> Public Class Utils

    Public Shared Function GetWarehouseConnectionId(ByVal WarehouseName As String) As Int32
        Dim con_id As Int32 = -1
        Try
            con_id = Convert.ToInt32(DataInterface.ExecuteScalar("select connection from warehouses where warehouse = '" & WarehouseName & "'", Made4Net.Schema.Constants.CONNECTION_NAME))
        Catch ex As Exception
            Throw New ApplicationException("Warehouse " & WarehouseName & " Connection does not exists")
        End Try
        Return con_id
    End Function

    Public Shared Function TranslateMessage(ByVal pMessageID As String, ByVal pParams As Made4Net.DataAccess.Collections.GenericCollection) As String
        Dim strTrans As String = String.Empty
        Dim searchMessageID = pMessageID.Replace("'", "''")
        Dim iLang As Int32 = Made4Net.Shared.Translation.Translator.CurrentLanguageID()
        Dim SQL As String = "SELECT * FROM vocabulary where phrase_id = '" & searchMessageID & "' and language_id=" & iLang
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim i As Integer

        DataInterface.FillDataset(SQL, dt, True, "Made4NetSchema")
        If dt.Rows.Count = 0 And pParams Is Nothing Then
            Return pMessageID
        ElseIf dt.Rows.Count = 0 And Not pParams Is Nothing Then
            If pParams.Count > 0 Then
                For i = 0 To pParams.Count - 1
                    pMessageID = pMessageID.Replace("{" & i & "}", pParams.Item(i))
                Next
            End If
            Return pMessageID
        End If
        dr = dt.Rows(0)
        If Not dr.IsNull("PHRASE_TRANSLATION") Then strTrans = dr.Item("PHRASE_TRANSLATION")
        If Not pParams Is Nothing Then
            If pParams.Count > 0 Then
                For i = 0 To pParams.Count - 1
                    strTrans = strTrans.Replace("{" & i & "}", pParams.Item(i))
                Next
            End If
        End If
        Return strTrans
    End Function

    'Added for PWMS-419 Start
    Public Shared Function deleteWaveassignment(ByVal templateName As String, ByRef Message As String) As Boolean
        Dim ret As Boolean = True

        Dim strsql As String
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

        strsql = String.Format("select COUNT(*) from WAVEASSIGNMENTDETAIL where TEMPLATENAME = '{0}' ", templateName)
        strsql = Made4Net.DataAccess.DataInterface.ExecuteScalar(strsql)
        If strsql > 0 Then
            Message = t.Translate("Detail records and/or Scheduling records exist for this Template Name Header and must be deleted before the Template Name Header can be deleted")
            Return False
        End If

        Return ret
    End Function

    'Added for PWMS-419 End

    'Added for PWMS-418 Start
    Public Shared Function deleteHandlingUnitStorage(ByVal hustoragetemplateid As String, ByRef Message As String) As Boolean
        Dim ret As Boolean = True

        Dim strsql As String
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

        strsql = String.Format("select COUNT(*) from HANDLINGUNITSTORAGETEMPLATEDETAIL where HUSTORAGETEMPLATEID = '{0}' ", hustoragetemplateid)
        strsql = Made4Net.DataAccess.DataInterface.ExecuteScalar(strsql)
        If strsql > 0 Then
            Message = t.Translate("Detail records exist for this header and must be deleted before the header can be deleted")
            Return False
        End If

        Return ret
    End Function

    'Added for PWMS-418 End


    'Added for PWMS-351 Start

    Public Shared Function deleteSKU(ByVal pCONSIGNEE As String, ByVal pSKU As String, ByRef pMessage As String) As Boolean
        Dim ret As Boolean = True

        Dim strsql As String
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

        strsql = String.Format("select COUNT(*) from INVLOAD where CONSIGNEE = '{0}' and SKU='{1}'", pCONSIGNEE, pSKU)
        strsql = Made4Net.DataAccess.DataInterface.ExecuteScalar(strsql)
        If strsql > 0 Then
            pMessage = t.Translate("Can not delete SKU as it has on-hand Inventry")
            Return False
        End If

        Return ret
    End Function

    'Added for PWMS-351 End

End Class
