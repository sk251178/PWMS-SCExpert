Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports WMS.Logic
<CLSCompliant(False)> Partial Public Class CNTTASKVERFICATION
    Inherits PWMSRDTBase

#Region "ViewState"

    Protected Overrides Function LoadPageStateFromPersistenceMedium() As Object
        Return Session("_ViewState")
    End Function

    Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal viewState As Object)
        Session("_ViewState") = viewState
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not Page.IsPostBack Then
            Dim oLoad As WMS.Logic.Load
            Dim SrcScreen As String = Request.QueryString("SRCSCREEN").ToString
            If SrcScreen.ToUpper = "CNTTASK" Then
                If Not Session("TaskLoadCNTLoadId") Is Nothing Then
                    oLoad = New WMS.Logic.Load(Session("TaskLoadCNTLoadId").ToString())
                End If
            Else
                If Not Session("LocationCNTLoadId") Is Nothing Then
                    oLoad = New WMS.Logic.Load(Session("LocationCNTLoadId").ToString())
                End If
            End If
            'If Not Session("TaskLoadCNTLoadId") Is Nothing Then
            '    oLoad = New WMS.Logic.Load(Session("TaskLoadCNTLoadId").ToString())
            'ElseIf Not Session("LocationCNTLoadId") Is Nothing Then
            '    oLoad = New WMS.Logic.Load(Session("LocationCNTLoadId").ToString())
            'End If

            DO1.Value("LoadId") = oLoad.LOADID
            DO1.Value("SKU") = oLoad.SKU
            Dim oSku As New WMS.Logic.SKU(oLoad.CONSIGNEE, oLoad.SKU)
            DO1.Value("SKUDESC") = oSku.SKUDESC
            DO1.Value("UOM") = oLoad.LOADUOM ' Session("UOMForVerification")
            'DO1.Value("CountedUnits") = Session("CountedUnitsForVerification")

            'If SrcScreen = "CNTTASK" Then
            DO1.Value("CountedUnits") = ManageMutliUOMUnits.GetTotal()
            'End If

        End If
    End Sub

    Private Sub doBack()
        Try
        
            If Request.QueryString("SRCSCREEN") = "CNTTASK" Then
                Response.Redirect(MapVirtualPath("Screens/CNTTASK1.aspx"))
            ElseIf Request.QueryString("SRCSCREEN") = "LOCCONTTASK" Then
                Response.Redirect(MapVirtualPath("Screens/LOCCONTTASK2.aspx"))
            Else
                Response.Redirect(MapVirtualPath("Screens/" & Request.QueryString("SRCSCREEN") & ".aspx"))
            End If
        Catch ex As System.Threading.ThreadAbortException
        Catch ex As Exception

        End Try
    End Sub

    Private Sub doApprove()
        'If Page.IsValid Then
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Dim oCntTask As WMS.Logic.CountTask
        Try
            Dim ld As WMS.Logic.Load
            If Not Session("TaskLoadCNTLoadId") Is Nothing Then
                ld = New WMS.Logic.Load(Session("TaskLoadCNTLoadId").ToString())
            ElseIf Not Session("LocationCNTLoadId") Is Nothing Then
                ld = New WMS.Logic.Load(Session("LocationCNTLoadId").ToString())
            End If

            If Not Session("LocationBulkCountTask") Is Nothing Then
                oCntTask = Session("LocationBulkCountTask")
            ElseIf Not Session("LocationCountTask") Is Nothing Then
                oCntTask = Session("LocationCountTask")
            ElseIf Not Session("LoadCountTask") Is Nothing Then
                oCntTask = Session("LoadCountTask")
            End If
            Session("TaskID") = oCntTask.TASK
            Dim oCounting As New WMS.Logic.Counting(oCntTask.COUNTID)
            'Build and fill count job object
            Dim oCountJob As WMS.Logic.CountingJob = oCntTask.getCountJob(oCounting)
            If String.IsNullOrEmpty(DO1.Value("CountedUnits")) OrElse Convert.ToDecimal(DO1.Value("CountedUnits")) < 0 Then
                Throw New Made4Net.Shared.M4NException(New Exception, "Please enter a positive number of units.", "Please enter a positive number of units.")
            End If
            Dim SK As New WMS.Logic.SKU(ld.CONSIGNEE, ld.SKU)
            'if ManageMutliUOMUnits.GetTotalEachUnits()
            If (DO1.Value("CountedUnits") = ManageMutliUOMUnits.GetTotal()) Then
                oCountJob.CountedQty = ManageMutliUOMUnits.GetTotalEachUnits() ' DO1.Value("CountedUnits") 'ManageMutliUOMUnits.GetTotal()
                oCountJob.UOM = SK.LOWESTUOM ' ld.LOADUOM ' DO1.Value("UOM")
            Else
                oCountJob.CountedQty = DO1.Value("CountedUnits") 'ManageMutliUOMUnits.GetTotal()
                oCountJob.UOM = ld.LOADUOM ' DO1.Value("UOM")
            End If
            

            oCountJob.Location = ld.LOCATION

            'RWMS-66 : 5/12/14 set the Location from Sesson only when the control comes to this page from the SRCSCREEN = CNTTASK1, 
            'not when the control comes to this page from LOCCONTTASK2:
            'Added Code for RWMS-598
            If (Request.QueryString("SRCSCREEN") = "CNTTASK1" Or Request.QueryString("SRCSCREEN") = "CNTTASK") Then
                'RWMS-58 : Tolocation fix 05/03/14
                'End RWMS-598
                oCountJob.Location = Session("ToLocation")
            End If

            oCountJob.LoadId = ld.LOADID
            oCountJob.LoadCountVerified = True
            oCountJob.LocationCountingLoadCounted = True
            oCountJob.Warehousearea = ld.WAREHOUSEAREA
            oCountJob.CountingAttributes = Session("AttributesForVerification")

            '************************

            ' oCountJob.CountedLoads = Session("TaskLocationCNTLoadsDT")
            '******************
            oCntTask.Count(oCounting, oCountJob, WMS.Logic.GetCurrentUser)
            UpdateLoadCount(ld.LOADID, DO1.Value("CountedUnits")) 'ManageMutliUOMUnits.GetTotal()) ' DO1.Value("CountedUnits")) ' ld.UNITS)
        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.Message)
            Return
        End Try
        Dim SrcScreen As String = Request.QueryString("SRCSCREEN").ToString

        'If SrcScreen = "CNTTASK" Then
        ManageMutliUOMUnits.Clear(True)
        'End If
        Try
            If oCntTask.STATUS = WMS.Lib.Statuses.Task.ASSIGNED AndAlso oCntTask.TASKTYPE = WMS.Lib.TASKTYPE.LOCATIONCOUNTING Then
                Response.Redirect(MapVirtualPath("Screens/LOCCONTTASK1.aspx"))
            End If
            If MobileUtils.ShouldRedirectToTaskSummary() Then
                MobileUtils.RedirectToTaskSummary(Request.QueryString("SRCSCREEN"))
            Else
                Response.Redirect(MapVirtualPath("Screens/" & Request.QueryString("SRCSCREEN") & ".aspx"))
            End If
        Catch ex As System.Threading.ThreadAbortException
        Catch ex As Exception

        End Try
        'doBack()
        'End If
    End Sub

    Private Sub UpdateLoadCount(ByVal pLoadId As String, ByVal pToQty As Decimal)
        If Not Session("TaskLocationCNTLoadsDT") Is Nothing Then
            Dim dt As DataTable = Session("TaskLocationCNTLoadsDT")
            For Each dr As DataRow In dt.Rows
                If dr("loadid") = pLoadId Then
                    dr("counted") = 1
                    dr("toqty") = pToQty
                End If
            Next
            Session("TaskLocationCNTLoadsDT") = dt
        End If
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddLabelLine("LoadId")
        DO1.AddLabelLine("SKU")
        DO1.AddLabelLine("SKUDESC")
        DO1.AddLabelLine("UOM")
        '        DO1.AddLabelLine("CountedUnits")
        DO1.AddTextboxLine("CountedUnits", True, "approve")
        DO1.AddSpacer()
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "back"
                doBack()
            Case "approve"
                doApprove()
        End Select
    End Sub

End Class
