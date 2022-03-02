Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports Made4Net.Shared
Imports Made4Net.DataAccess

Imports Wms.Logic

Partial Public Class vrfi
    Inherits PWMSRDTBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not IsPostBack Then

            If Not IsNothing(Session("VRFILD")) Then
                Dim ld As WMS.Logic.Load = Session("VRFILD")
                DO1.Value("LABELID") = ld.LOADID
            End If

            If Not IsNothing(Session("VRFICONT")) Then
                Dim cont As WMS.Logic.Container = Session("VRFICONT")
                DO1.Value("LABELID") = cont.ContainerId
            End If
            'Added for RWMS-559
            If Not IsNothing(Session("VRFIDICT")) Then
                Session.Remove("VRFIDICT")
            End If
            If Not IsNothing(Session("VRFISKU")) Then
                Session.Remove("VRFISKU")
            End If
            If Not IsNothing(Session("objMultiUOMUnits")) Then
                Session.Remove("objMultiUOMUnits")
            End If
            If Not IsNothing(Session("VRFIHUID")) Then
                Session.Remove("VRFIHUID")
            End If
            'End Added for RWMS-559
        End If
    End Sub

    <CLSCompliant(False)>
    Protected Sub DO1_ButtonClick(ByVal sender As System.Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "next"
                doNext()
            Case "menu"
                doMenu()
        End Select
    End Sub

    Protected Sub DO1_CreatedChildControls(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        'DO1.AddTextboxLine("LoadID")
        'DO1.AddTextboxLine("ContainerID")
        DO1.AddTextboxLine("LABELID")
        DO1.DefaultButton = "Next"
        DO1.FocusField = "LABELID"
    End Sub
    ''' <summary>
    ''' Method is modified for RWMS-705, RWMS-816
    ''' Checking the Container Status initially and then Loads status for that container.
    ''' For partial pick we have to consider the Container id and for Full Pick we have to use the Loadid while verifiying
    ''' JIRA Items - RWMS-705, RWMS-816
    ''' </summary>
    ''' <remarks></remarks>

    Private Sub doNext()
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        If String.IsNullOrEmpty(DO1.Value("LABELID")) Then
            'HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Both load id and container id field are filled."))
            Return
        End If
        'Added for RWMS-1293 and RWMS-705,RWMS-816
        Dim pPickType As String

        If WMS.Logic.Container.Exists(DO1.Value("LABELID")) Then
            'TODO Check for the container status staged.
            Dim cont As New WMS.Logic.Container(DO1.Value("LABELID"), True)

            'If cont.Status <> WMS.Lib.Statuses.Container.STAGED Then
            '     HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Container has an incorrect activity status for verifying."))
            '    DO1.Value("LABELID") = ""
            '    Return
            'End If
            'RWMS-705, RWMS-816
            'If cont.Status <> WMS.Lib.Statuses.Container.STAGED Then
            '     HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Container has an incorrect activity status for verifying."))
            '    DO1.Value("LABELID") = ""
            '    Return
            'End If

            'For i As Integer = 0 To cont.Loads.Count - 1
            '    If cont.Loads(i).ACTIVITYSTATUS.ToLower() <> "picked" AndAlso cont.Loads(i).ACTIVITYSTATUS.ToLower() <> "staged" _
            '     AndAlso cont.Loads(i).ACTIVITYSTATUS.ToLower() <> "verified" Then
            '         HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Container loads have an incorrect activity status for verifying."))
            '        DO1.Value("LABELID") = ""
            '        Return
            '    End If
            'Next
            ''If Not ValidateContainerOrderStatus(DO1.Value("LABELID")) Then
            ''     HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Container belong to an order with incorrect status for verifying"))
            ''    DO1.Value("LABELID") = ""
            ''    Return
            ''End If
            'RWMS-705, RWMS-816
            If cont.Status <> WMS.Lib.Statuses.Container.STAGED Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Container has an incorrect activity status for verifying."))
                DO1.Value("LABELID") = ""
                Return
            End If

            Dim arrList As New ArrayList()
            If cont.Loads.Count > 0 Then
                For Each addLoads As WMS.Logic.Load In cont.Loads
                    If addLoads.ACTIVITYSTATUS.ToLower() = "verified" Then
                        arrList.Add(addLoads)
                    End If
                Next
            End If

            If arrList.Count > 0 Then
                For Each removeLoads As WMS.Logic.Load In arrList
                    cont.Loads.Remove(removeLoads)
                Next
            End If

            If cont.Loads.Count > 0 Then
                For i As Integer = 0 To cont.Loads.Count - 1
                    If cont.Loads(i).ACTIVITYSTATUS.ToLower() <> "picked" AndAlso cont.Loads(i).ACTIVITYSTATUS.ToLower() <> "staged" Then
                        HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Container of the loads have an incorrect activity status for verifying."))
                        DO1.Value("LABELID") = ""
                        Return
                    End If
                Next
            Else
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Loads are unvailable for the container to verify."))
                DO1.Value("LABELID") = ""
                Return
            End If
            'RWMS-705
            Session("VRFICONT") = cont

        ElseIf WMS.Logic.Load.Exists(DO1.Value("LABELID")) Then
            'Dim ld As New WMS.Logic.Load(DO1.Value("LABELID"))
            ''Commented for RWMS-558
            ''If Not String.IsNullOrEmpty(ld.ContainerId) Then
            ''    Dim cont As New WMS.Logic.Container(ld.ContainerId, True)
            ''    For i As Integer = 0 To cont.Loads.Count - 1
            ''        If cont.Loads(i).ACTIVITYSTATUS.ToLower() <> "picked" AndAlso cont.Loads(i).ACTIVITYSTATUS.ToLower() <> "staged" Then
            ''             HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Container loads have an incorrect activity status for verifying."))
            ''            DO1.Value("LABELID") = ""
            ''            Return
            ''        End If
            ''    Next
            ''    'If Not ValidateContainerOrderStatus(DO1.Value("LABELID")) Then
            ''    '     HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Container belong to an order with incorrect status for verifying"))
            ''    '    DO1.Value("LABELID") = ""
            ''    '    Return
            ''    'End If
            ''    Session("VRFICONT") = cont
            ''End Commented for RWMS-558
            ''Added for RWMS-558
            'If Not String.IsNullOrEmpty(ld.LOADID) Then
            '    Dim cont As New WMS.Logic.Load(ld.LOADID, True)
            '    'Added for RWMS-637
            '    If cont.ACTIVITYSTATUS.ToLower() = "verified" Then
            '         HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Already Verified."))
            pPickType = GetPickType(DO1.Value("LABELID"))
            If pPickType = "FULLPICK" Then
                Dim ld As New WMS.Logic.Load(DO1.Value("LABELID"))
                'Added for RWMS-558
                If Not String.IsNullOrEmpty(ld.LOADID) Then
                    Dim cont As New WMS.Logic.Load(ld.LOADID, True)
                    'Added for RWMS-637
                    If cont.ACTIVITYSTATUS.ToLower() = "verified" Then
                        HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Already Verified."))
                        DO1.Value("LABELID") = ""
                        Return
                    ElseIf cont.ACTIVITYSTATUS.ToLower() <> "picked" AndAlso cont.ACTIVITYSTATUS.ToLower() <> "staged" Then
                        HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Load has an incorrect activity status for verifying."))
                        DO1.Value("LABELID") = ""
                        Return
                    End If
                    'End RWMS-637
                    Session("VRFILD") = cont
                    'End Added for RWMS-558
                Else
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Load does not exist."))
                    DO1.Value("LABELID") = ""
                    Return
                    Session("VRFILD") = ld
                End If
            Else
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Entered Load must be of FULLPICK type to verify."))
                DO1.Value("LABELID") = ""
                Return
            End If
        Else
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("No load or container with entered label exists."))
            DO1.Value("LABELID") = ""
            Return
        End If

        Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("Screens/VRFI1.aspx"))

    End Sub
    'Ended for RWMS-1293 and RWMS-705,RWMS-816
    'Added for RWMS-1293 and RWMS-705,RWMS-816
    ''' <summary>
    ''' 'New Method to get the Picktype from Pickheader by joining with pickdetail and passing the TOLOADID - RWMS-705
    ''' </summary>
    ''' <param name="pLabelId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetPickType(ByVal pLabelId As String) As String
        Dim retPickType As String = ""
        Dim Sql As String
        Sql = String.Format("SELECT distinct PH.PICKTYPE FROM PICKHEADER PH INNER JOIN PICKDETAIL PD ON PH.PICKLIST=PD.PICKLIST WHERE TOLOAD='{0}'", pLabelId)
        Dim dt As New DataTable
        DataInterface.FillDataset(Sql, dt)
        If dt.Rows.Count > 0 Then
            retPickType = dt.Rows(0)("PICKTYPE")
        End If
        Return retPickType
    End Function
    'END RWMS-705
    'Ended for RWMS-1293 and RWMS-705,RWMS-816

    'Private Sub doNext()
    '    If Page.IsValid Then
    '        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
    '        If DO1.Value("LoadID") <> "" And DO1.Value("ContainerID") <> "" Then
    '            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Both load id and container id field are filled."))
    '            Return
    '        End If

    '        If DO1.Value("LoadID") <> "" Then
    '            If Not WMS.Logic.Load.Exists(DO1.Value("LoadID")) Then
    '                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("The load id entered does not exist."))
    '                Return
    '            End If
    '            Dim lObj As New WMS.Logic.Load(DO1.Value("LoadID"))
    '            lObj.Verify(WMS.Logic.Common.GetCurrentUser())
    '            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Load verified"))
    '        ElseIf DO1.Value("ContainerID") <> "" Then
    '            If Not WMS.Logic.Container.Exists(DO1.Value("ContainerID")) Then
    '                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("The container id entered does not exist."))
    '                Return
    '            End If
    '            Dim contaObj As New WMS.Logic.Container(DO1.Value("ContainerID"), True)
    '            For Each load As WMS.Logic.Load In contaObj.Loads
    '                load.Verify(WMS.Logic.Common.GetCurrentUser)
    '            Next
    '            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Container verified"))
    '        End If
    '    End If

    '    DO1.Value("ContainerID") = ""
    '    DO1.Value("LoadID") = ""
    'End Sub

    Private Sub doMenu()
        Session.Remove("VRFILD")
        Session.Remove("VRFICONT")
        Session.Remove("VRFISKU")
        Session.Remove("VRFICONSIGNEE")
        Session.Remove("objMultiUOMUnits")
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Function ValidateContainerOrderStatus(ByVal pContId As String) As Boolean
        Dim Sql As String = String.Format("select distinct oh.STATUS from INVLOAD il inner join ORDERLOADS ol on ol.LOADID = il.LOADID inner join OUTBOUNDORHEADER oh on oh.CONSIGNEE = ol.CONSIGNEE and oh.ORDERID = ol.ORDERID where il.HANDLINGUNIT = '{0}'", pContId)
        Dim dt As New DataTable
        DataInterface.FillDataset(Sql, dt)
        If dt.Rows.Count > 1 Or dt.Rows.Count = 0 Then
            'More than one order found / No orders were found -> not valid
            Return False
        Else
            Dim status As String = dt.Rows(0)("STATUS")
            If Not String.Equals(status, WMS.Lib.Statuses.OutboundOrderHeader.STAGED, StringComparison.OrdinalIgnoreCase) Then
                Return False
            End If
        End If
        Return True
    End Function

    Private Function ValidateLoadOrderStatus(ByVal pLoadid As String) As Boolean
        Dim Sql As String = String.Format("select distinct oh.STATUS from ORDERLOADS ol inner join OUTBOUNDORHEADER oh on oh.CONSIGNEE = ol.CONSIGNEE and oh.ORDERID = ol.ORDERID where ol.LOADID = '{0}'", pLoadid)
        Dim dt As New DataTable
        DataInterface.FillDataset(Sql, dt)
        If dt.Rows.Count > 1 Or dt.Rows.Count = 0 Then
            'More than one order found / No orders were found -> not valid
            Return False
        Else
            Dim status As String = dt.Rows(0)("STATUS")
            If Not String.Equals(status, WMS.Lib.Statuses.OutboundOrderHeader.STAGED, StringComparison.OrdinalIgnoreCase) Then
                Return False
            End If
        End If
        Return True
    End Function

End Class