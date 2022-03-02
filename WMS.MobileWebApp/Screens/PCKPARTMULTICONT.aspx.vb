Imports WMS.Logic
Imports Made4Net.Shared.Web
Imports Made4Net.Mobile

Partial Public Class PCKPARTMULTICONT
    Inherits PWMSRDTBase

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not IsPostBack Then

            'loadContainers(Session("PCKPicklist"))

            setScreen(Session("PCKPicklist"))


        End If
    End Sub

    Private Sub loadContainers(ByVal pcklst As WMS.Logic.Picklist)
        
        Dim cnt As New WMS.Logic.Container

        'at the beginning add all active containers
        MultiContManage.AddContainers(pcklst.Containers)

        Dim oCont As MultiContPick = MultiContManage.GetMultiContPick()

        'need to be edit, when session is already been loaded
        If Not IsNothing(Session("PCKPicklistActiveContainerID")) Then
            DO1.Value("Container_1") = Session("PCKPicklistActiveContainerID")
            If Not IsNothing(Session("PCKPicklistActiveContainerIDSecond")) Then                
                'if we have more then 2 active containes go to pckpart
                If MultiContManage.ContainerCount > 2 Then
                    Response.Redirect(MapVirtualPath("Screens/PCKPART.aspx"))
                Else
                    DO1.Value("Container_2") = MultiContManage.GetSecondContainer 'Session("PCKPicklistActiveContainerIDSecond")
                End If
            End If
        ElseIf oCont.dictCont.Count = 1 Then
            '  cnt = oCont.dictCont.Item(0)
            DO1.Value("Container_1") = oCont.dictCont.Item(0)
            Session("PCKPicklistActiveContainerID") = oCont.dictCont.Item(0)
        ElseIf oCont.dictCont.Count > 1 Then
            Dim ContainerId As String
            ContainerId = oCont.dictCont.Item(oCont.dictCont.Count - 1)
            DO1.Value("Container_2") = ContainerId
            'Session("PCKPicklistActiveContainerIDSecond") = cnt.ContainerId
            MultiContManage.AddContainer(cnt.ContainerId)
            ContainerId = oCont.dictCont.Item(oCont.dictCont.Count - 2)
            DO1.Value("Container_1") = ContainerId
            Session("PCKPicklistActiveContainerID") = ContainerId
            'if we have more then 2 active containes go to pckpart
            If oCont.dictCont.Count > 2 Then
                Response.Redirect(MapVirtualPath("Screens/PCKPART.aspx"))
            End If
        End If


    End Sub

    Private Sub setScreen(ByVal pcklst As WMS.Logic.Picklist)

        DO1.Value("PickType") = pcklst.PickType
        DO1.Value("Picklist") = pcklst.PicklistID
        Dim Numberoflines As Integer
        Dim Numberofcases As Decimal
        NumberoflinesNumberofcases(pcklst, Numberoflines, Numberofcases)

        DO1.Value("Numberoflines") = Numberoflines.ToString

        DO1.Value("Numberofcases") = Numberofcases.ToString
        

    End Sub

    Private Sub NumberoflinesNumberofcases(ByVal pcklst As WMS.Logic.Picklist, ByRef Numberoflines As Integer, ByRef Numberofcases As Decimal)
        Dim sql As String
        sql = String.Format("select Numberofcases, Numberoflines from NumberoflinesNumberofcases where PICKLIST='{0}'", pcklst.PicklistID)
        Dim dt As New DataTable
        Made4Net.DataAccess.DataInterface.FillDataset(sql, dt)
        For Each dr As DataRow In dt.Rows
            Numberoflines = dr("Numberoflines")
            Numberofcases = Math.Round(dr("Numberofcases"), 2)
        Next
        'Dim s As WMS.Logic.SKU

        'For Each pline As WMS.Logic.PicklistDetail In pcklst.Lines
        '    If pline.Status = WMS.Lib.Statuses.Picklist.PLANNED Or _
        '       pline.Status = WMS.Lib.Statuses.Picklist.RELEASED Or _
        '       pline.Status = WMS.Lib.Statuses.Picklist.PARTPICKED Then

        '        Numberoflines = Numberoflines + 1

        '        s = New WMS.Logic.SKU(pline.Consignee, pline.SKU)
        '        Try
        '            Numberofcases = Numberofcases + pline.Quantity / s.ConvertToUnits(pline.UOM)
        '        Catch ex As Exception
        '            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  "sku " & pline.SKU & " case uom is missing")
        '        End Try

        '    End If

        'Next
        'Numberofcases = Math.Round(Numberofcases, 2)
    End Sub


    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
       

        DO1.AddLabelLine("PickType")
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        DO1.AddLabelLine("NOTES", "NOTES", t.Translate("Two pallets are needed"))
        DO1.AddLabelLine("Picklist")
        DO1.AddLabelLine("Numberoflines", "Number of lines")
        DO1.AddLabelLine("Numberofcases", "Number of cases")
        DO1.AddTextboxLine("Container_1", True, "")
        DO1.AddTextboxLine("Container_2")

        DO1.AddSpacer()


    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick

        Select Case e.CommandText.ToLower
            Case "1_container"
                ONEcontainer()
            Case "2_containers"
                ' doFinish()
                TWOcontainer()
            Case "back"
                Response.Redirect(MapVirtualPath("Screens/PCK.aspx"))
        End Select

    End Sub

    Private Sub ONEcontainer()
        If DO1.Value("Container_1") = "" Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  "Illegal Container_1")
            Return
        Else
            Dim errMsg As String
            If Not MobileUtils.CheckContainerID(DO1.Value("Picklist"), DO1.Value("Container_1"), errMsg) Then
                DO1.Value("Container_1") = ""
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  errMsg)
                Return
            End If
            Session("PCKPicklistActiveContainerID") = DO1.Value("Container_1")
            Session.Remove("PCKPicklistActiveContainerIDSecond")
        End If
        Response.Redirect(MapVirtualPath("Screens/PCKPART.aspx"))
    End Sub

    Private Sub TWOcontainer()
        If DO1.Value("Container_1") = DO1.Value("Container_2") And (DO1.Value("Container_1") <> "" And DO1.Value("Container_2") <> "") Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  "Two containers must be different")
            Return
        End If

        If DO1.Value("Container_1") = "" Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  "Illegal Container_1")
            Return
        Else
            Dim errMsg As String
            If Not MobileUtils.CheckContainerID(DO1.Value("Picklist"), DO1.Value("Container_1"), errMsg) Then
                DO1.Value("Container_1") = ""
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  errMsg)
                Return
            End If
            Session("PCKPicklistActiveContainerID") = DO1.Value("Container_1")
            MultiContManage.AddContainer(DO1.Value("Container_1"))
        End If

        If DO1.Value("Container_2") = "" Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  "Illegal Container_2")
            Return
        Else
            Dim errMsg As String
            If Not MobileUtils.CheckContainerID(DO1.Value("Picklist"), DO1.Value("Container_2"), errMsg) Then
                DO1.Value("Container_2") = ""
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  errMsg)
                Return
            End If

            MultiContManage.AddContainer(DO1.Value("Container_2"))
            'Session("PCKPicklistActiveContainerIDSecond") = DO1.Value("Container_2")
        End If

        Response.Redirect(MapVirtualPath("Screens/PCKPART.aspx"))

    End Sub

End Class