Imports WMS.Logic
Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports Made4Net.DataAccess

<CLSCompliant(False)> Public Class RCVTRNS1
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
        If WMS.Logic.GetCurrentUser = "" Then
            WMS.Logic.GotoLogin()
        End If
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        '----------------------------
        DO1.AddLabelLine("CONSIGNEE")
        DO1.AddLabelLine("TRANSHIPMENT")
        DO1.AddLabelLine("TARGETCOMPANY")
        DO1.AddLabelLine("SOURCECOMPANY")
        DO1.AddLabelLine("EXPECTEDARRIVALDATE")
        DO1.AddLabelLine("EXPECTEDDELIVERYDATE")
        DO1.AddLabelLine("QTY")
        DO1.AddLabelLine("WEIGHT")

        Dim oTrans As New WMS.Logic.TransShipment
        oTrans = Session("TRANSHIPMENTOBJ")
        Dim oTargetCompany As New WMS.Logic.Company(oTrans.CONSIGNEE, oTrans.TARGETCOMPANY, oTrans.TARGETCOMPANYTYPE)
        Dim oSourceCompany As New WMS.Logic.Company(oTrans.CONSIGNEE, oTrans.SOURCECOMPANY, oTrans.SOURCECOMPANYTYPE)

        DO1.Value("CONSIGNEE") = oTrans.CONSIGNEE
        DO1.Value("TRANSHIPMENT") = oTrans.TRANSSHIPMENT
        DO1.Value("TARGETCOMPANY") = oTargetCompany.COMPANYNAME
        DO1.Value("SOURCECOMPANY") = oSourceCompany.COMPANYNAME
        DO1.Value("EXPECTEDARRIVALDATE") = oTrans.SCHEDULEDARRIVALDATE.ToString(Made4Net.Shared.Util.GetSystemParameterValue("RDTDateFormat"))
        DO1.Value("EXPECTEDDELIVERYDATE") = oTrans.SCHEDULEDDELIVERYDATE.ToString(Made4Net.Shared.Util.GetSystemParameterValue("RDTDateFormat"))
        DO1.Value("QTY") = oTrans.EXPECTEDQTY
        DO1.Value("WEIGHT") = oTrans.EXPECTEDWEIGHT
        '----------------------------
        DO1.AddTextboxLine("TXTQTY")
        DO1.AddTextboxLine("TXTWEIGHT")
        DO1.Value("TXTQTY") = oTrans.EXPECTEDQTY
        DO1.Value("TXTWEIGHT") = oTrans.EXPECTEDWEIGHT

    End Sub

    Private Sub doMenu()
        Session.Remove("TRANSHIPMENTOBJ")
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub doBack()
        Session.Remove("TRANSHIPMENTOBJ")
        Response.Redirect(MapVirtualPath("Screens/RCVTRNS.aspx"))
    End Sub

    Public Sub doReceive()
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Dim oTrans As New WMS.Logic.TransShipment
        oTrans = Session("TRANSHIPMENTOBJ")
        If Not CheckQty(DO1.Value("TXTQTY")) Or Not CheckWgt(DO1.Value("TXTWEIGHT")) Or Not CheckQty(DO1.Value("ACTUALPACKNUM")) Or Not CheckWgt(DO1.Value("ACTUALCAPACITY")) Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Some numeric fields has bad formating"))
            Exit Sub
        End If
        ' If DO1.Value("TXTQTY") > oTrans.EXPECTEDQTY Or DO1.Value("TXTWEIGHT") > oTrans.EXPECTEDWEIGHT Then
        'HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  "Quantity or Weight is greater than expected")
        'Exit Sub
        'End If




        If DO1.Value("TXTQTY") >= 0 Or DO1.Value("TXTWEIGHT") >= 0 Then
            Try
                oTrans.Receive(WMS.Logic.GetCurrentUser)
            Catch ex As Exception
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Transshipment Status Incorrect"))
                Exit Sub
            End Try
            oTrans.SetReceivedQty(DO1.Value("TXTQTY"), WMS.Logic.GetCurrentUser)
            oTrans.SetReceivedWeight(DO1.Value("TXTWEIGHT"), WMS.Logic.GetCurrentUser)
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Transshipment Received"))
            Response.Redirect(MapVirtualPath("Screens/RCVTRNS.aspx"))
        End If

    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "receive"
                doReceive()
            Case "back"
                doBack()
            Case "menu"
                doMenu()
        End Select
    End Sub

#Region "Methods"

    Private Function CheckQty(ByVal pQty As String) As Boolean
        Try
            If pQty = "" Then
                Return False
            End If
            Convert.ToDecimal(pQty)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Function CheckWgt(ByVal pWgt As String) As Boolean
        Try
            If pWgt = "" Then
                Return False
            End If
            Convert.ToDecimal(pWgt)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

#End Region

End Class
