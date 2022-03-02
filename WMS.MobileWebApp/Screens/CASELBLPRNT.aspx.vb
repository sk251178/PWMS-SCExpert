Imports WMS.Logic
Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports WL = WMS.Logic
Imports WLTaskManager = WMS.Logic.TaskManager

<CLSCompliant(False)> Public Class CASELBLPRNT
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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If WMS.Logic.GetCurrentUser Is Nothing Then
            WMS.Logic.GotoLogin()
        End If
        If Not IsPostBack Then

        End If
    End Sub

    Private Sub doNext()
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser
        Dim prntr As LabelPrinter
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Try
            prntr = New LabelPrinter(DO1.Value("Printer"))
        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Return
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate(ex.ToString()))
            Return
        End Try
        Dim tm As WLTaskManager = WLTaskManager.NewTaskManagerForUserAndTaskType(UserId, WMS.Lib.TASKTYPE.PICKING, WL.LogHandler.GetRDTLogger())
        Dim pcklst As New Picklist(tm.Task.Picklist)
        ' Do for individual or for every case. Check the quantity, in loop
        Dim largeQty As Int32 = pcklst.GetMinimumQty()
        Dim pckdt As PicklistDetail = GetNextPickLine(pcklst)
        Dim qty As Int32 = ConvertToCaseQty(pckdt)
        ' Chech for -1
        If largeQty > 0 Then
            If qty > largeQty Then
                ' Print only one label for large qty
                pcklst.PrintPickLabels(prntr.PrinterQName)
            Else
                ' Get number of cases
                For index As Integer = 1 To qty
                    pcklst.PrintPickLabels(prntr.PrinterQName)
                Next
            End If
        Else
            For index As Integer = 1 To qty
                pcklst.PrintPickLabels(prntr.PrinterQName)
            Next
        End If
        Response.Redirect(MapVirtualPath("Screens/PCK.aspx"))
    End Sub

    Private Function ConvertToCaseQty(ByRef pcklstdt As PicklistDetail) As Int32
        Dim sql As String = String.Format("select UNITSPERMEASURE from SKUUOM where SKU ='{0}' AND UOM = (select LINEOUM as UOM from OUTBOUNDORDETAIL where ORDERID='{1}' AND ORDERLINE='{2}')", pcklstdt.SKU, pcklstdt.OrderId, pcklstdt.OrderLine)
        Dim factor As Int32 = Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)

        Return pcklstdt.Quantity / factor
    End Function

    Private Function GetNextPickLine(ByRef pcklst As Picklist) As PicklistDetail
        Dim pckdet As PicklistDetail
        Dim lineIdx As Int32 = 0
        pckdet = pcklst.Lines(0)
        While Not pckdet Is Nothing
            If pckdet.Status = WMS.Lib.Statuses.Picklist.PLANNED Or pckdet.Status = WMS.Lib.Statuses.Picklist.RELEASED Then
                Return pckdet
            End If
            lineIdx += 1
        End While
        Return Nothing
    End Function

    Private Sub doSkip()
        Response.Redirect(MapVirtualPath("Screens/DELCLPRNT.aspx?sourcescreen=" & Session("MobileSourceScreen") & "&printed=1"))
    End Sub

    Private Sub doMenu()
        Dim UserId As String = WMS.Logic.Common.GetCurrentUser
        If WMS.Logic.TaskManager.isAssigned(UserId, WMS.Lib.TASKTYPE.PICKING, WMS.Logic.LogHandler.GetRDTLogger()) Then
            Dim tm As WLTaskManager = WLTaskManager.NewTaskManagerForUserAndTaskType(UserId, WMS.Lib.TASKTYPE.PICKING, WL.LogHandler.GetRDTLogger())
            tm.ExitTask()
        End If
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddSpacer()
        DO1.AddTextboxLine("Printer", True, "next")
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "menu"
                doMenu()
            Case "next"
                doNext()
            Case "skip"
                doSkip()
        End Select
    End Sub

End Class