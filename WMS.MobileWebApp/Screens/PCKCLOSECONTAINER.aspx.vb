Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports Made4Net.Shared
Imports Made4Net.DataAccess
Imports WMS.Logic


Partial Public Class PCKCLOSECONTAINER
    Inherits PWMSRDTBase

#Region "ViewState"

    Protected Overrides Function LoadPageStateFromPersistenceMedium() As Object
        Return Session("_ViewState")
    End Function

    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        Session("_ViewState") = viewState
    End Sub
#End Region

    Public Shared activCount As Integer = 0
    Public Shared activCont As String
    Public picklistComplete As Boolean = False

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Session("PCKPicklist") Is Nothing Then
            Dim pcklist As Picklist = Session("PCKPicklist")
            picklistComplete = pcklist.isCompleted
        End If
        If Not IsPostBack() Then
            Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
            activCount = 0
            activCont = ""

            'If activCount = 2 Then
            DO1.Value("Warning") = trans.Translate("Are you sure you want to close container?") ' "Which container to open/close?"
            DO1.setVisibility("CONTAINER", True)
            DO1.Value("CONTAINER") = Session("PCKPicklistActiveContainerID")
            'Added for RWMS-1643 and RWMS-745 logs


                MyBase.WriteToRDTLog(" Pck closing container page. ")
            'rdtLogger.writeSeperator("-", 60)

            'End for RWMS-1643 and RWMS-745


        End If
    End Sub

    Private Function IsActive(ByVal cont As String) As Boolean
        Dim pcklist As Picklist = Session("PCKPicklist")
        Dim sql As String = String.Format("select COUNT(1) from PICKDETAIL where PICKLIST='{0}' and TOCONTAINER='{1}'", pcklist.PicklistID, cont)
        If Made4Net.DataAccess.DataInterface.ExecuteScalar(sql) > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "close"
                close(True)
            Case "open new"
                openNew()
            Case "close and deliver"
                close(False)
            Case "back"
                doBack()
                'Case "leave open"
                '    leaveOpen()

        End Select


    End Sub



    Private Sub leaveOpen()
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        If activCount = 1 Then
            'If the user presses “leave open” the system will return to picking and have both containers as active.
            'Added for RWMS-1643 and RWMS-745  logs


                MyBase.WriteToRDTLog(" If the user presses leave open the system will return to picking and have both containers as active. Active Container: " & activCont)
            'rdtLogger.writeSeperator("-", 60)

            'Ended  for RWMS-1643 and RWMS-745

            Session("PCKPicklistActiveContainerID") = activCont
            If DO1.Value("NEWCONTAINER") <> "" Then
                Session("PCKPicklistActiveContainerIDSecond") = DO1.Value("NEWCONTAINER")
            End If

            doBack()
        Else
            doBack()
        End If

    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As EventArgs) Handles DO1.CreatedChildControls
        DO1.AddLabelLine("Warning")
        DO1.AddSpacer()
        DO1.AddLabelLine("CONTAINER")
        '        DO1.AddTextboxLine("NEWCONTAINER")
        'New Container
    End Sub

    Private Sub doBack()
        Try
            'Commented for RWMS-1643 and RWMS-745
            'Response.Redirect(MapVirtualPath("screens/PCKPART.aspx"))
            'End Commented for RWMS-1643 and RWMS-745
            'Added for RWMS-1643 and RWMS-745
            If Not Request.QueryString("sourcescreen") Is Nothing Then
                Dim src As String = Request.QueryString("sourcescreen")
                Response.Redirect(MapVirtualPath("screens/" & src & ".aspx"))
            Else
                Response.Redirect(MapVirtualPath("screens/PCKPART.aspx"))
            End If
            'Ended  for RWMS-1643 and RWMS-745


        Catch ex As System.Threading.ThreadAbortException

        End Try

    End Sub

    Private Sub openNew()
        Dim errMsg As String
        Dim pcklist As WMS.Logic.Picklist = Session("PCKPicklist")
        'Added for RWMS-1643 and RWMS-745  logs


            MyBase.WriteToRDTLog(" Starting of Open New container in the PKCLOSEContainaer page")
        'rdtLogger.writeSeperator("-", 60)

        'Ended for RWMS-1643 and RWMS-745

        If Not MobileUtils.CheckContainerID(pcklist.PicklistID, DO1.Value("CONTAINER"), errMsg) Then
            'Added for RWMS-1643 and RWMS-745  logs

                MyBase.WriteToRDTLog(" Already container is assigned for other picklist and empty the container field and show an error message. ")
            'rdtLogger.writeSeperator("-", 60)

            'Ended for RWMS-1643 and RWMS-745

            DO1.Value("CONTAINER") = ""
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  errMsg)
            Return
        Else

            If Not MultiContManage.ActiveContainerContain(Session("PCKPicklistActiveContainerID")) Then
                'Added for RWMS-1643 and RWMS-745  logs

                MyBase.WriteToRDTLog(" If it is not an Active container and it is new container then add to the AddContainer method ")
                'rdtLogger.writeSeperator("-", 60)

                'Ended for RWMS-1643 and RWMS-745

                MultiContManage.AddContainer(Session("PCKPicklistActiveContainerID"))
            End If

            MultiContManage.AddContainer(DO1.Value("CONTAINER"))
            'Added for RWMS-1643 and RWMS-745 logs

            MyBase.WriteToRDTLog(" Adding the new container value from text field to the add container method of MultiContManage class. Cont " & DO1.Value("CONTAINER"))
            'rdtLogger.writeSeperator("-", 60)

            'Ended for RWMS-1643 and RWMS-745

            doBack()
        End If
    End Sub

    Private Sub doDeliver()
        Dim pcklist As WMS.Logic.Picklist = Session("PCKPicklist") 'New Picklist(pck.picklist)

        Dim relStrat As ReleaseStrategyDetail
        relStrat = pcklist.getReleaseStrategy()
        Dim pck As PickJob = Session("PCKPicklistPickJob")
        'Added for RWMS-1643 and RWMS-745 logs

        'Ended for RWMS-1643 and RWMS-745
        Try

            If Not relStrat Is Nothing Then
                If relStrat.DeliverContainerOnClosing Then
                    'Should deliver the container now
                    'Added for RWMS-1643 and RWMS-745 logs

                    MyBase.WriteToRDTLog("If DeliverContaineronClosing is true then call the close container method from Picklist class object")
                    'rdtLogger.writeSeperator("-", 60)

                    'Ended for RWMS-1643 and RWMS-745

                    pcklist.CloseContainer(DO1.Value("CONTAINER"), True, WMS.Logic.GetCurrentUser)

                    If Not Session("DefaultPrinter") = "" AndAlso Not Session("DefaultPrinter") Is Nothing Then
                        Dim prntr As LabelPrinter = New LabelPrinter(Session("DefaultPrinter"))
                        MobileUtils.getDefaultPrinter(Session("DefaultPrinter"), LogHandler.GetRDTLogger())
                        If Not Session("PCKPicklist") Is Nothing Then
                            Dim oPicklist As WMS.Logic.Picklist = Session("PCKPicklist")
                            If oPicklist.ShouldPrintShipLabel Then
                                oPicklist.PrintShipLabels(prntr.PrinterQName)
                            End If
                            If oPicklist.isCompleted Then
                                PickTask.UpdateCompletionTime(oPicklist)
                            End If
                        ElseIf Not Session("PARPCKPicklist") Is Nothing Then
                            Dim pck2 As ParallelPicking = Session("PARPCKPicklist")
                            pck2.PrintShipLabels(prntr.PrinterQName)
                        End If
                        If Not Session("PCKPicklist") Is Nothing Then
                            Dim oPicklist As WMS.Logic.Picklist = Session("PCKPicklist")
                            If oPicklist.GetPrintContentList() And Not oPicklist.GetContentListReoprtName() Is Nothing Then
                                Response.Redirect(MapVirtualPath("Screens/DELCLPRNT.aspx?sourcescreen=" & Session("MobileSourceScreen") & "&printed=1"))
                            End If
                        End If
                        Response.Redirect(MapVirtualPath("Screens/DEL.aspx?sourcescreen=" & Session("MobileSourceScreen") & "&printed=1"))
                    Else
                        Response.Redirect(MapVirtualPath("screens/DELLBLPRNT.aspx?sourcescreen=PCK"))
                    End If

                Else
                    'Should close the container - go back to PCK to open a new one
                    'Added for RWMS-1643 and RWMS-745 logs

                    MyBase.WriteToRDTLog(" call the close container method from Picklist class object and go back to the PCK page to opne new new container ")
                    'rdtLogger.writeSeperator("-", 60)

                    'Ended for RWMS-1643 and RWMS-745

                    pcklist.CloseContainer(DO1.Value("CONTAINER"), False, WMS.Logic.GetCurrentUser)
                    'Session.Remove("PCKPicklistActiveContainerID")
                    ' Response.Redirect(MapVirtualPath("screens/PCK.aspx"))
                    doBack()
                End If
            End If

        Catch ex As System.Threading.ThreadAbortException

        End Try

    End Sub

    Private Sub close(ByVal fDeliver As Boolean)

        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        'Added for RWMS-1643 and RWMS-745 logs


            MyBase.WriteToRDTLog(" Close method call")
        'rdtLogger.writeSeperator("-", 60)

        'Ended for RWMS-1643 and RWMS-745

        If DO1.Value("CONTAINER").Trim = "" Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Cannot Close Cotnainer - Container is blank"))
            Exit Sub
        End If
        Dim pcklist As WMS.Logic.Picklist = Session("PCKPicklist")
        'Session("PCKListToResume") = pcklist.PicklistID
        If picklistComplete = False Then
            Session("PCKListToResume") = pcklist.PicklistID
        Else
            Session.Remove("PCKListToResume")
        End If

        'load picl list
        pcklist = New WMS.Logic.Picklist(pcklist.PicklistID)
        If pcklist.NoOfToLocations > 1 Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Cannot close container - Picklist has more than one staging lane"))
            Exit Sub
        End If
        If Not IsActive(DO1.Value("CONTAINER")) Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("can not deliver container, nothing was picked"))
            Exit Sub
        End If

        If Not WMS.Logic.Container.Exists(DO1.Value("CONTAINER")) Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("container does not exist"))
            Exit Sub
        End If

        Dim oCont As New WMS.Logic.Container(DO1.Value("CONTAINER"), True)
        If oCont.Status <> WMS.Lib.Statuses.Container.STATUSNEW Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("container have illegal status"))
            Exit Sub
        End If

        Dim ret As Boolean = MultiContManage.CloseActiveContainer(DO1.Value("CONTAINER"))
        If fDeliver Then
            doDeliver()
            Exit Sub

        Else
            'Should deliver the container now
            pcklist.CloseContainer(DO1.Value("CONTAINER"), True, WMS.Logic.GetCurrentUser)
            Dim pck2 As ParallelPicking = Session("PARPCKPicklist")

            If Not Session("DefaultPrinter") = "" AndAlso Not Session("DefaultPrinter") Is Nothing Then
                Dim prntr As LabelPrinter = New LabelPrinter(Session("DefaultPrinter"))
                MobileUtils.getDefaultPrinter(Session("DefaultPrinter"), LogHandler.GetRDTLogger())
                If Not Session("PCKPicklist") Is Nothing Then
                    Dim oPicklist As WMS.Logic.Picklist = Session("PCKPicklist")
                    If oPicklist.ShouldPrintShipLabel Then
                        oPicklist.PrintShipLabels(prntr.PrinterQName)
                    End If
                    If oPicklist.isCompleted Then
                        PickTask.UpdateCompletionTime(oPicklist)
                    End If
                ElseIf Not Session("PARPCKPicklist") Is Nothing Then
                    pck2.PrintShipLabels(prntr.PrinterQName)
                End If
                If Not Session("PCKPicklist") Is Nothing Then
                    Dim oPicklist As WMS.Logic.Picklist = Session("PCKPicklist")
                    If oPicklist.GetPrintContentList() And Not oPicklist.GetContentListReoprtName() Is Nothing Then
                        Response.Redirect(MapVirtualPath("Screens/DELCLPRNT.aspx?sourcescreen=" & Session("MobileSourceScreen") & "&printed=1"))
                    End If
                End If
                Response.Redirect(MapVirtualPath("Screens/DEL.aspx?sourcescreen=" & Session("MobileSourceScreen") & "&printed=1"))
            Else
                Response.Redirect(MapVirtualPath("screens/DELLBLPRNT.aspx?sourcescreen=PCK"))
            End If
            'Response.Redirect(MapVirtualPath("screens/DELLBLPRNT.aspx?sourcescreen=PCK"))
            Exit Sub

        End If



    End Sub


End Class