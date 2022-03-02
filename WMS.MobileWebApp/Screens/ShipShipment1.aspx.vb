Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports Made4Net.DataAccess

<CLSCompliant(False)> Public Class ShipShipment1
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
            If Not Session("SHIPMENTSHPOBJ") Is Nothing Then
                SetScreen()
            Else
                doBack()
            End If
        End If
    End Sub

    Private Sub doMenu()
        Session.Remove("SHIPMENTSHPOBJ")
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub doBack()
        Response.Redirect(MapVirtualPath("Screens/ShipShipment.aspx"))
    End Sub

    Private Sub SetScreen()
        Dim oShip As WMS.Logic.Shipment = Session("SHIPMENTSHPOBJ")
        DO1.Value("SHIPMENT") = oShip.SHIPMENT
        DO1.Value("SCHEDDATE") = oShip.SCHEDDATE
        DO1.Value("STATUS") = DataInterface.ExecuteScalar(String.Format("select [description] from codelistdetail where codelistcode = 'SHPSTAT' and code = '{0}'", oShip.STATUS))
        DO1.Value("VEHICLE") = oShip.Vehicle
    End Sub

    Private Sub doNext()
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Try
            Dim oShip As WMS.Logic.Shipment = Session("SHIPMENTSHPOBJ")
            'Added for RWMS-1710 Start   
            ''Validation Rules for SHIPMENT   
            '1. If QtyModified = 0 then ignore those outboundordetails   
            '2. If Qtyallocated or qtypicked or qtypacked or qtyverified or qtystaged > 0  If true, then display there are still open picks or inventory that has not been loaded   
            '3. If qtymodified > 0 AND (qtystaged + qtyloaded + qtyshipped + Qtyallocated + qtypicked + qtypacked + qtyverified = 0) display message "Some order lines have not been allocated"   
            Dim shipId As String = oShip.SHIPMENT
            Dim dt As New DataTable
            'RWMS-2369 RWMS-2368 - Added the condition in the sql - AND WE.ORDERLINE=OD.ORDERLINE
            Dim SQL As String = String.Format(" SELECT SUM(ISNULL(QTYALLOCATED, 0)) AS QTYALLOCATED,SUM(ISNULL(QTYPICKED, 0)) AS QTYPICKED, " & _
            " SUM(ISNULL(QTYPACKED, 0)) AS QTYPACKED,SUM(ISNULL(QTYVERIFIED, 0)) AS QTYVERIFIED, " & _
            " SUM(ISNULL(OD.QTYMODIFIED, 0)) AS QTYMODIFIED,SUM(ISNULL(OD.QTYSTAGED, 0)) AS QTYSTAGED, " & _
            " SUM(ISNULL(OD.QTYLOADED, 0)) AS QTYLOADED,SUM(ISNULL(OD.QTYSHIPPED, 0)) AS QTYSHIPPED, " & _
            " SUM(ISNULL(QTYSTAGED, 0) + ISNULL(QTYLOADED, 0) + ISNULL(OD.QTYSHIPPED, 0) + ISNULL(OD.QTYALLOCATED, 0) + ISNULL(OD.QTYPICKED, 0) + ISNULL(OD.QTYPACKED, 0) + ISNULL(OD.QTYVERIFIED, 0)) AS SUMQTY, " & _
            " SUM(ISNULL(ExceptionQty,0)) AS EXCEPTIONQTY " & _
            " FROM OUTBOUNDORDETAIL OD LEFT JOIN WAVEEXCEPTION WE ON OD.ORDERID=WE.ORDERID AND OD.CONSIGNEE=WE.CONSIGNEE AND WE.ORDERLINE=OD.ORDERLINE " & _
            " INNER JOIN SHIPMENTDETAIL SH ON SH.ORDERID = OD.ORDERID AND SH.ORDERLINE = OD.ORDERLINE AND SH.CONSIGNEE = OD.CONSIGNEE " & _
            " WHERE OD.QTYMODIFIED > 0 AND SH.SHIPMENT='{0}'", shipId)

            DataInterface.FillDataset(SQL, dt)

            If dt.Rows.Count > 0 Then
                If (Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dt.Rows(0).Item("QTYMODIFIED")) > 0) And (Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dt.Rows(0).Item("SUMQTY")) = 0) Then
                    'Throw New Made4Net.Shared.M4NException(New Exception, "Some order lines have not been allocated", "Some order lines have not been allocated")   
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Some order lines have not been allocated"))
                    doBack()
                    'Commented for RWMS-2340 RWMS-2339 RWMS-2369 RWMS-2368 Start
                    'ElseIf Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dt.Rows(0).Item("QTYALLOCATED")) > 0 Or Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dt.Rows(0).Item("QTYPICKED")) > 0 Or Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dt.Rows(0).Item("QTYPACKED")) > 0 Or Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dt.Rows(0).Item("QTYVERIFIED")) > 0 Or Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dt.Rows(0).Item("QTYSTAGED")) > 0 Then
                    'Commented for RWMS-2340 RWMS-2339 RWMS-2369 RWMS-2368 End
                    'Throw New Made4Net.Shared.M4NException(New Exception, "There are still open picks or inventory that has not been loaded", "There are still open picks or inventory that has not been loaded")   

                    'Added for RWMS-2340 RWMS-2339 RWMS-2369 RWMS-2368 Start
                ElseIf Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dt.Rows(0).Item("QTYALLOCATED")) > 0 Or Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dt.Rows(0).Item("QTYPICKED")) > 0 Or Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dt.Rows(0).Item("QTYSTAGED")) > 0 Then
                    'Added for RWMS-2340 RWMS-2339 RWMS-2369 RWMS-2368 End
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("There are still open picks or inventory that has not been loaded"))
                    doBack()
                ElseIf Made4Net.Shared.Conversion.Convert.ReplaceDBNull(dt.Rows(0).Item("EXCEPTIONQTY")) <> 0 Then
                    'Throw New Made4Net.Shared.M4NException(New Exception, "Cancel exceptions first", "Cancel exceptions first")   
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Cancel exceptions first"))
                    doBack()
                End If
            End If

            'Added for RWMS-1710 End   

            oShip.Ship(WMS.Logic.Common.GetCurrentUser)
            'Go Back To Scan another orderid
            doBack()
        Catch ex As System.Threading.ThreadAbortException
            'Do nothing
        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate(ex.Message))
        End Try
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddLabelLine("SHIPMENT")
        DO1.AddLabelLine("SCHEDDATE")
        DO1.AddLabelLine("STATUS")
        DO1.AddLabelLine("VEHICLE")
        DO1.AddSpacer()
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "next"
                doNext()
            Case "menu"
                doMenu()
            Case "back"
                doBack()
        End Select
    End Sub

End Class
