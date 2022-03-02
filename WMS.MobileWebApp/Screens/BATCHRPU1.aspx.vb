Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports WMS.Logic
Imports WLTaskManager = WMS.Logic.TaskManager
Imports WMS.Lib

<CLSCompliant(False)> Public Class BATCHRPU1
    Inherits PWMSRDTBase

     Public Const CURRENTUNLOADINDEX As String = "currentUnloadIndex"

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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If WMS.Logic.GetCurrentUser = "" Then
            WMS.Logic.GotoLogin()
        End If
        If Not IsPostBack Then
            Session.Add(CURRENTUNLOADINDEX, 0)
            SetScreen(GetDetailLine())
        End If
    End Sub

    Private Sub SetScreen(ByVal batchReplenDetail As BatchReplenDetail)
        Dim task As Task = Session(WMS.Lib.SESSION.BATCHREPLENUNLOADTASK)
        DO1.Value("TASKTYPE") = task.TASKTYPE

        Dim brh As BatchReplenHeader = Session(WMS.Lib.SESSION.BATCHREPLENUNLOADHEADER)
        DO1.Value("CONTAINER") = brh.REPLCONTAINER
        DO1.Value("PICKREGION") = brh.PICKREGION
        DO1.Value("LOCATION") = batchReplenDetail.TOLOCATION
        DO1.Value("TOSKU") = batchReplenDetail.TOSKU
        DO1.Value("EANUPC") = GetEanUPC(batchReplenDetail)
        Dim ld As New Load(batchReplenDetail.FROMLOAD)
        DO1.Value("SKUUOM") = ld.LOADUOM
        Dim sku As New SKU(batchReplenDetail.CONSIGNEE, batchReplenDetail.FROMSKU)
        DO1.Value("QTY") = sku.ConvertUnitsToUom(ld.LOADUOM, batchReplenDetail.LETDOWNQTY)
        DO1.Value("TOLOCATION") = String.Empty
    End Sub

    Private Sub doNext()
        Dim batchReplenDetail As BatchReplenDetail = GetDetailLine()
        If IsValidated(batchReplenDetail) Then
            ' Update the detail record
            batchReplenDetail.Unload(batchReplenDetail.LETDOWNQTY)
            Dim task As Task = CType(Session(WMS.Lib.SESSION.BATCHREPLENUNLOADTASK), Task)
            task.TOLOCATION = batchReplenDetail.TOLOCATION.Trim()
            task.Update()
            ' If more details to process, then get next detail record
            Session.Add(CURRENTUNLOADINDEX, Session(CURRENTUNLOADINDEX) + 1)
            If CType(Session(WMS.Lib.SESSION.BATCHREPLENDETAILCOLLLECTION), BatchReplenDetailCollection).Count > Session(CURRENTUNLOADINDEX) Then
                SetScreen(GetDetailLine())
            Else
                ' No more details to process
                ' Update the header and go back to the select container
                CType(Session(WMS.Lib.SESSION.BATCHREPLENUNLOADHEADER), BatchReplenHeader).Unload()
                ' Complete the Task
                task.USERID = WMS.Logic.Common.GetCurrentUser()
                task.Complete((WMS.Logic.LogHandler.GetRDTLogger))
                Response.Redirect(MapVirtualPath(WMS.Lib.SCREENS.BATCHREPLENUNLOAD))
            End If
        End If
    End Sub

    Private Function GetTranslator() As Made4Net.Shared.Translation.Translator
        Return New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
    End Function

    Private Function IsValidated(ByVal batchReplenDetail As BatchReplenDetail) As Boolean
        Dim t = GetTranslator()
        ' Validate the Location
        If DO1.Value("TOLOCATION").Trim() <> batchReplenDetail.TOLOCATION.Trim() Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, t.Translate("Wrong Location Confirmation"))
            Return False
        End If
        Return True
    End Function
    Private Function GetEanUPC(ByRef brd As BatchReplenDetail) As String
        Dim eanupc As String = String.Empty
        Dim sku As SKU = New SKU(brd.CONSIGNEE, brd.TOSKU)
        If Not String.IsNullOrEmpty(sku.UOM(brd.TOSKUUOM).EANUPC) Then
            eanupc = sku.UOM(brd.TOSKUUOM).EANUPC
        End If
        Return eanupc
    End Function
    Private Function GetDetailLine() As BatchReplenDetail
        Return CType(Session(WMS.Lib.SESSION.BATCHREPLENDETAILCOLLLECTION), BatchReplenDetailCollection).Item(Session(CURRENTUNLOADINDEX))
    End Function

    Private Sub doMenu()
        CType(Session(WMS.Lib.SESSION.BATCHREPLENUNLOADTASK), Task).ExitTask()
        Session.Remove(WMS.Lib.SESSION.BATCHREPLENUNLOADTASK)
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        If (Session(WMS.Lib.SESSION.BATCHREPLENUNLOADTASK) Is Nothing) Then
            Response.Redirect(Made4Net.Shared.Web.MapVirtualPath(WMS.Lib.SCREENS.LOGIN))
        End If
        DO1.AddLabelLine("TASKTYPE")
        DO1.AddLabelLine("CONTAINER")
        DO1.AddLabelLine("PICKREGION")
        DO1.AddLabelLine("LOCATION")
        DO1.AddLabelLine("TOSKU", "SKU")
        DO1.AddLabelLine("EANUPC")
        DO1.AddLabelLine("SKUUOM", "UOM")
        DO1.AddSpacer()
        DO1.AddLabelLine("QTY", "UOM Quantity")
        DO1.AddTextboxLine("TOLOCATION", False, "next")
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "next"
                doNext()
            Case "menu"
                doMenu()
        End Select
    End Sub
End Class