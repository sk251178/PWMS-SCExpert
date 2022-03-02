Imports WMS.Logic
Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports Made4Net.DataAccess

<CLSCompliant(False)> Public Class ShipTransShipment1
    Inherits PWMSRDTBase

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object
    <CLSCompliant(False)> Protected WithEvents Screen1 As WMS.MobileWebApp.WebCtrls.Screen
    Protected WithEvents DO1 As Made4Net.Mobile.WebCtrls.DataObject

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
        If Not IsPostBack Then
            DO1.Value("QTYSCANED") = Session("TRNCOUNT")
        End If
    End Sub

    Private Sub doMenu()
        Session.Remove("CONSINGEE")
        Session.Remove("TRANSSHIPMENT")
        Session.Remove("REFERENCEORD")
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub doBack()
        Session.Remove("CONSINGEE")
        Session.Remove("TRANSSHIPMENT")
        Session.Remove("REFERENCEORD")
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub doNext()
        If Not IsPostBack Then
            Session("TRNCOUNT") = 0
        End If
        Dim oTrans As New WMS.Logic.TransShipment
        oTrans = Session("TRANSHIPMENTOBJ")
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        If DO1.Value("TXTTRANSHIPMENT") = oTrans.TRANSSHIPMENT Then
            Session("TRNCOUNT") = Session("TRNCOUNT") + 1
            If oTrans.RECEIVEDQTY = Session("TRNCOUNT") Then
                oTrans.Ship(WMS.Logic.GetCurrentUser)
                Session("TRNCOUNT") = 0
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Transshipment shipped"))
                Response.Redirect(MapVirtualPath("Screens/ShipTransShipment.aspx"))
            Else
                Response.Redirect(MapVirtualPath("Screens/ShipTransShipment1.aspx"))
            End If
        Else
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Transshipment scaned is not valid"))
            Exit Sub
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
        DO1.AddLabelLine("QTYSCANED")

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
        DO1.Value("QTYSCANED") = Session("TRNCOUNT")
        '----------------------------
        DO1.AddTextboxLine("TXTTRANSHIPMENT")

    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick

        Session("CONSINGEE") = DO1.Value("CONSINGEE")
        Session("TRANSSHIPMENT") = DO1.Value("TRANSSHIPMENT")
        Session("REFERENCEORD") = DO1.Value("REFERENCEORD")

        Select Case e.CommandText.ToLower
            Case "ship"
                doNext()
            Case "menu"
                doMenu()
            Case "back"
                doBack()
        End Select
    End Sub

End Class
