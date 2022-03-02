Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports Made4Net.DataAccess
Imports Made4Net.Shared
Imports Made4Net.General
Imports Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
Imports Microsoft.Practices.EnterpriseLibrary.Logging
Imports WMS.Logic
Imports System.Collections.Generic

<CLSCompliant(False)> Public Class MPPW
    Inherits PWMSRDTBase

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.MobileWebApp.WebCtrls.Screen
    Protected WithEvents DO1 As Made4Net.Mobile.WebCtrls.DataObject

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region "ViewState"

    Protected Overrides Function LoadPageStateFromPersistenceMedium() As Object
        Return Session("_ViewState")
    End Function

    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        Session("_ViewState") = viewState
    End Sub
#End Region

#Region "Page_Load"
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If IsPostBack Then
            Dim list As List(Of String) = Session("PayLoadId")
            Dim SequenceList As List(Of String) = Session("Sequence")

        ElseIf Session("PayLoadId") IsNot Nothing Then

            Dim list As List(Of String) = Session("PayLoadId")
            Dim SequenceList As List(Of String) = Session("Sequence")
            'list.Add(DO1.Value("LOADID"))
            Session("PayLoadId") = list
            DO1.Value("Numberofpayloads") = list.Count.ToString()
            Session("Sequence") = SequenceList
            'SequenceList.Add(DO1.Value("Numberofpayloads"))
            If list.Count = 0 Then
                DO1.Value("LastPayload") = Nothing
                DO1.Value("Numberofpayloads") = Nothing
            Else
                DO1.Value("LastPayload") = list(list.Count - 1)
            End If
        Else
            Dim list As New List(Of String)
            Dim SequenceList As New List(Of String)
            Session("PayLoadId") = list
            Session("Sequence") = SequenceList
        End If

        If Session("SKUMULTISEL_LOADID") IsNot Nothing Then

            Session("PayLoadId") = Session("SKUMULTISEL_LOADID")


        End If


    End Sub
#End Region

#Region "DO1_CreatedChildControls"
    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        DO1.AddTextboxLine("LOADID")
        DO1.AddSpacer()
        DO1.AddLabelLine("LastPayload")
        DO1.AddLabelLine("Numberofpayloads")
        DO1.AddSpacer()
        DO1.FocusField = "LOADID"
    End Sub

#End Region

#Region "doNext()"
    Private Sub doNext()
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

        If Session("PayLoadId") IsNot Nothing Then
            Dim listPayloads As List(Of String) = Session("PayLoadId")
            If listPayloads.Count > 0 Then
                Dim strTask As String = getFirstAssignedTask(listPayloads) '' why only first load, it sould be any load from sequence
                If strTask <> "" Then
                    Dim oTask As WMS.Logic.Task = New WMS.Logic.Task(strTask)
                    oTask.AssignUser(WMS.Logic.Common.GetCurrentUser)
                    If oTask.TASKTYPE = WMS.Lib.TASKTYPE.PARTREPL Or oTask.TASKTYPE = WMS.Lib.TASKTYPE.FULLREPL Or oTask.TASKTYPE = WMS.Lib.TASKTYPE.NEGTREPL Then
                        Dim ReplTask As New WMS.Logic.ReplenishmentTask(strTask)
                        Session("REPLTSKTaskId") = ReplTask
                        Dim ReplTaskDetail As New WMS.Logic.Replenishment(ReplTask.Replenishment)
                        Session("REPLTSKTDetail") = ReplTaskDetail
                        If MobileUtils.ShowGoalTimeOnTaskAssignment() Then
                            Session("TMTask") = ReplTask
                            Session("TargetScreen") = "Screens/REPL2.aspx?sourcescreen=LDPW"
                            Response.Redirect(MapVirtualPath(WMS.Lib.SCREENS.TASKMANAGER))
                        Else
                            Response.Redirect(MapVirtualPath("Screens/REPL2.aspx?sourcescreen=LDPW"))
                        End If
                    Else
                        'Session("TASKMANAGERSOURCESCREENID") = "LDPW"
                        'Session("TASKMANAGERSHOULDREDIRECTTOSRC") = "0"
                        Response.Redirect(MapVirtualPath("Screens/RPK2Multi.aspx?sourcescreen=MPPW"))
                    End If
                Else

                    If listPayloads IsNot Nothing And listPayloads.Count > 0 Then
                        Try
                            Dim pwProc As New Multiputaway
                            Dim destLoc, destWHArea, prepopulateLocation As String 'RWMS-1277
                            prepopulateLocation = "" 'RWMS-1277
                            pwProc.RequestDestinationForMultiLoad(listPayloads.ToArray(), destLoc, destWHArea, 0, prepopulateLocation) 'RWMS-1277
                            Session("SKUMULTISEL_LOADID") = listPayloads
                            Session.Remove("PayLoadId")
                            Session.Remove("Sequence")
                            If MobileUtils.ShowGoalTimeOnTaskAssignment() Then
                                Dim pwtask As PutawayTask
                                Dim tm As New WMS.Logic.TaskManager
                                pwtask = tm.getMultiPutAwayFirstAssignedTask(listPayloads, WMS.Logic.Common.GetCurrentUser)
                                Session("TMTask") = pwtask
                                Session("TargetScreen") = "Screens/RPK2Multi.aspx?sourcescreen=MPPW"
                                Session("LoadPUSrcScreen") = "MPPW"
                                Session("MobileSourceScreen") = "MPPW"
                                Session("SkipLoadScanScreen") = 1
                                Response.Redirect(MapVirtualPath(WMS.Lib.SCREENS.TASKMANAGER))
                            Else
                                Session("MobileSourceScreen") = "MPPW"
                                Session("LoadPUSrcScreen") = "MPPW"
                                Dim tm As New WMS.Logic.TaskManager
                                Dim oTask As Task = tm.getMultiPutAwayFirstAssignedTask(listPayloads, WMS.Logic.Common.GetCurrentUser)
                                If MultiPayloadPutawayHelper.IsMultiPayLoadPutAwayTask(oTask.TASK) Then
                                    Response.Redirect(MapVirtualPath("Screens/RPK2Multi.aspx?sourcescreen=MPPW"))
                                Else
                                    If Not String.IsNullOrEmpty(Session("SkipLoadScanScreen")) Then

                                        If Session("SkipLoadScanScreen").ToString = "2" Then
                                            Response.Redirect(MapVirtualPath("Screens/RPK2.aspx?sourcescreen=RPK"))
                                        Else
                                            Response.Redirect(MapVirtualPath("Screens/RPK2.aspx?sourcescreen=LDPW")) 'RPK"))
                                        End If

                                        Session.Remove("SkipLoadScanScreen")

                                    Else
                                        Response.Redirect(MapVirtualPath("Screens/LDPWCNF.aspx"))
                                    End If
                                End If
                            End If


                        Catch ex As Threading.ThreadAbortException
                        Catch m4nEx As Made4Net.Shared.M4NException
                            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  m4nEx.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
                        Catch ex As ApplicationException
                            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate(ex.Message))
                        End Try
                    End If
                End If
            End If
        End If
    End Sub
#End Region

#Region "getFirstAssignedTask"
    Private Function getFirstAssignedTask(listPayloads As List(Of String)) As String

        Dim loadList As String = String.Join(",", listPayloads.ToArray())

        Dim strSql As String = String.Format("SELECT top(1) task from tasks inner join  dbo.Split('{0}', ',') as loadIds on tasks.FROMLOAD = loadIds.Data where (STATUS <> 'COMPLETE' AND STATUS <> 'CANCELED') order by loadIds.id asc", loadList)
        Dim dt As DataTable = New DataTable
        DataInterface.FillDataset(strSql, dt)
        If dt.Rows.Count > 0 Then
            Return Convert.ToString(dt.Rows(0)("TASK"))
        Else
            Return ""
        End If
    End Function
#End Region

#Region "DO1_ButtonClick"

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        DO1.FocusField = "LOADID"
        Select Case e.CommandText.ToLower

            Case "scan"
                doScan()
            Case "menu"
                doMenu()
            Case "next"
                doNext()
            Case "cancel all"
                Try
                    If Session("SKUMULTISEL_LOADID") Is Nothing Then
                        Response.Redirect(Made4Net.Shared.Web.MapVirtualPath("Screens/VERIFYCANCELALL.aspx"))
                    Else
                        Throw New M4NException(New Exception, "Multi-payload putaway is already in progress. Please complete by clicking next.", "Multi-payload putaway is already in progress. Please complete by clicking next.")
                    End If
                Catch ex As Exception
                    If TypeOf ex Is M4NException Then
                        HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  "Multi-payload putaway is already in progress. Please complete by clicking next.")
                    End If
                End Try


        End Select
    End Sub
#End Region

#Region "doScan()"
    Private Sub doScan()
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

        If Session("SKUMULTISEL_LOADID") Is Nothing Then
            If IsPostBack Then
                Try
                    Dim list As List(Of String) = Session("PayLoadId")
                    Dim SequenceList As List(Of String) = Session("Sequence")


                    If DO1.Value("LOADID") <> "" Then
                        ' SQL = String.Format("select * from invload inner join sku on invload.consignee = sku.consignee and invload.sku = sku.sku where loadid = '{0}'", _
                        '  DO1.Value("LOADID"))
                        Dim dtmultipayload As New DataTable
                        Dim multipayloadsdao As MultipayloadsDao = New MultipayloadsDao
                        dtmultipayload = multipayloadsdao.GetMultipayloads(DO1.Value("LOADID"))


                        ' DataInterface.FillDataset(SQL, dtmultipayload)
                        If dtmultipayload.Rows.Count = 0 Then
                            Throw New M4NException(New Exception, t.Translate("No Loads were found"), t.Translate("No Loads were found"))

                        ElseIf dtmultipayload.Rows.Count >= 1 Then
                            If Not list.Contains(DO1.Value("LOADID")) Then
                                list.Add(DO1.Value("LOADID"))
                                Session("PayLoadId") = list
                                DO1.Value("Numberofpayloads") = list.Count.ToString()
                                Session("Sequence") = SequenceList
                                SequenceList.Add(DO1.Value("Numberofpayloads"))
                                DO1.Value("LastPayload") = list(list.Count - 1)
                                DO1.Value("LOADID") = Nothing
                            Else
                                Throw New M4NException(New Exception, t.Translate("Loads already exists"), t.Translate("Loads already exists"))
                                DO1.Value("LOADID") = Nothing
                            End If
                        End If
                    End If

                Catch m4nEx As Made4Net.Shared.M4NException
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  m4nEx.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
                    DO1.Value("LOADID") = Nothing
                Catch ex As Exception
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate(ex.Message))
                End Try
            ElseIf Session("PayLoadId") IsNot Nothing Then

                Dim list As List(Of String) = Session("PayLoadId")
                Dim SequenceList As List(Of String) = Session("Sequence")
                'list.Add(DO1.Value("LOADID"))
                Session("PayLoadId") = list
                DO1.Value("Numberofpayloads") = list.Count.ToString()
                Session("Sequence") = SequenceList
                'SequenceList.Add(DO1.Value("Numberofpayloads"))
                If list.Count = 0 Then
                    DO1.Value("LastPayload") = Nothing
                    DO1.Value("Numberofpayloads") = Nothing
                Else
                    DO1.Value("LastPayload") = list(list.Count - 1)
                End If
            Else
                Dim list As New List(Of String)
                Dim SequenceList As New List(Of String)
                Session("PayLoadId") = list
                Session("Sequence") = SequenceList
            End If
        Else
            Try
                Throw New M4NException(New Exception, "Multi-payload putaway is already in progress. Please complete by clicking next.", "Multi-payload putaway is already in progress. Please complete by clicking next.")
            Catch ex As Exception
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  "Multi-payload putaway is already in progress. Please complete by clicking next.")
            End Try
        End If
    End Sub
#End Region

#Region "doMenu()"
    Private Sub doMenu()
        Session.Remove("PayLoadId")
        Session.Remove("Sequence")
        Made4Net.Mobile.Common.GoToMenu()
    End Sub
#End Region


End Class