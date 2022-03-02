Imports WMS.Logic
Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports Made4Net.DataAccess
Imports System.Collections.Generic
<CLSCompliant(False)> Public Class LOADING2DiffShip
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
        If Not IsPostBack Then

        End If
    End Sub

    Private Function LoadDictPallet(ByVal dr As DataRow, ByVal type As String) As Dictionary(Of String, String)
        Dim SQL As String
        
        Dim dict As New Dictionary(Of String, String)

        If type = "CONTAINER" Then
            dict.Add("TYPE", "CONTAINER")

            dict.Add("PALLETID", dr("CONTAINERID"))

            SQL = String.Format("SELECT count(distinct TARGETCOMPANY) FROM vLoadingContainer where SHIPMENT<>'' and CONTAINERID  = '{0}'", dr("CONTAINERID"))

            SQL = Made4Net.DataAccess.DataInterface.ExecuteScalar(SQL)
            If SQL = 1 Then
                dict.Add("TARGETCOMPANY", dr("TARGETCOMPANY").ToString)
                dict.Add("COMPANYNAME", dr("COMPANYNAME").ToString)
            Else
                dict.Add("TARGETCOMPANY", "")
                dict.Add("COMPANYNAME", "")
            End If
        Else 'type=LOAD
            dict.Add("TYPE", "LOAD")

            dict.Add("PALLETID", dr("LOADID"))
            dict.Add("TARGETCOMPANY", dr("TARGETCOMPANY").ToString)
            dict.Add("COMPANYNAME", dr("COMPANYNAME").ToString)
        End If

        dict.Add("SHIPMENT", dr("SHIPMENT").ToString)
        dict.Add("CARRIER", dr("CARRIER").ToString)
        dict.Add("CARRIERNAME", dr("CARRIERNAME").ToString)
        dict.Add("DOOR", dr("DOOR").ToString)
        dict.Add("VEHICLE", dr("VEHICLE").ToString)


        Return dict
    End Function

    Private Function ContainerExists(ByVal Container As String) As Boolean
        Dim ret As Boolean = True
        Dim SQL As String

        SQL = String.Format("select COUNT(*) from INVLOAD where HANDLINGUNIT = '{0}'", Container)
        SQL = Made4Net.DataAccess.DataInterface.ExecuteScalar(SQL)
        If SQL = "0" Then
            ret = False
        End If
        Return ret
    End Function

    Private Function LoadExists(ByVal load As String) As Boolean
        Dim ret As Boolean = True
        Dim SQL As String

        SQL = String.Format("select COUNT(*) from INVLOAD where loadid = '{0}'", load)
        SQL = Made4Net.DataAccess.DataInterface.ExecuteScalar(SQL)
        If SQL = "0" Then
            ret = False
        End If
        Return ret
    End Function

    Private Function getPalletID() As Boolean
        ' dim ls as New lis
        Dim SQL As String
        Dim SQLbasic As String
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Dim dict As New Dictionary(Of String, String)
        Dim dt As New DataTable


        If ContainerExists(DO1.Value("PALLETID")) Then
            'vLoadedContainers
            SQL = SQLbasic & " SELECT * FROM vLoadedContainers WHERE CONTAINER = '{0}'"
            SQL = String.Format(SQL, DO1.Value("PALLETID"))
            dt = New DataTable
            Made4Net.DataAccess.DataInterface.FillDataset(SQL, dt)
            If dt.Rows.Count > 0 Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Container already loaded"))
                Return False
            Else
                SQLbasic = "SELECT top 1 CONTAINERID, LOADID, SHIPMENT, COMPANYNAME, TARGETCOMPANY,CARRIER, CARRIERNAME, DOOR, VEHICLE,  ORDERID   FROM vLoadingContainer "

                SQL = SQLbasic & " WHERE SHIPMENT<>'' and CONTAINERID = '{0}'"
                SQL = String.Format(SQL, DO1.Value("PALLETID"))
                dt = New DataTable
                Made4Net.DataAccess.DataInterface.FillDataset(SQL, dt)

                If dt.Rows.Count > 0 Then
                    dict = LoadDictPallet(dt.Rows(0), "CONTAINER")
                    Session("LoadingPalletDict") = dict
                Else
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Container is not ready for loading"))
                    Return False
                End If
            End If
        ElseIf LoadExists(DO1.Value("PALLETID")) Then
            Dim l As New WMS.Logic.Load(DO1.Value("PALLETID"))
            If l.ACTIVITYSTATUS = "LOADED" Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Payload already loaded"))
                Return False
            Else
                dt = New DataTable
                SQLbasic = " SELECT top 1 '' as CONTAINERID, toload as LOADID, SHIPMENT, COMPANYNAME, TARGETCOMPANY,CARRIER, CARRIERNAME, DOOR, VEHICLE,  ORDERID   FROM vloadingloads "
                SQL = SQLbasic & "  WHERE SHIPMENT<>'' and toload = '{0}'"
                SQL = String.Format(SQL, DO1.Value("PALLETID"))
                Made4Net.DataAccess.DataInterface.FillDataset(SQL, dt)

                If dt.Rows.Count > 0 Then
                    dict = LoadDictPallet(dt.Rows(0), "LOAD")
                    Session("LoadingPalletDict") = dict
                Else
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Payload  is not ready for loading"))
                    Return False
                End If
            End If
        Else
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Pallet does not exist"))
            Return False
        End If

        'SQLbasic = "SELECT top 1 CONTAINERID, LOADID, SHIPMENT, COMPANYNAME, TARGETCOMPANY,CARRIER, CARRIERNAME, DOOR, VEHICLE,  ORDERID   FROM vLoadingContainer "

        'SQL = SQLbasic & " WHERE SHIPMENT<>'' and CONTAINERID = '{0}'"
        'SQL = String.Format(SQL, DO1.Value("PALLETID"))
        'dt = New DataTable
        'Made4Net.DataAccess.DataInterface.FillDataset(SQL, dt)

        'If dt.Rows.Count > 0 Then
        '    dict = LoadDictPallet(dt.Rows(0), "CONTAINER")
        '    Session("LoadingPalletDict") = dict
        'Else
        '    dt = New DataTable
        '    SQLbasic = " SELECT top 1 '' as CONTAINERID, toload as LOADID, SHIPMENT, COMPANYNAME, TARGETCOMPANY,CARRIER, CARRIERNAME, DOOR, VEHICLE,  ORDERID   FROM vloadingloads"
        '    SQL = SQLbasic & "  WHERE SHIPMENT<>'' and toload = '{0}'"
        '    SQL = String.Format(SQL, DO1.Value("PALLETID"))
        '    Made4Net.DataAccess.DataInterface.FillDataset(SQL, dt)

        '    If dt.Rows.Count > 0 Then
        '        dict = LoadDictPallet(dt.Rows(0), "LOAD")
        '        Session("LoadingPalletDict") = dict
        '    Else
        '         HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Pallet was not found"))
        '        Return False
        '    End If
        'End If

        Return True
    End Function

    'Added for RWMS-2343 RWMS-2314
    Private Function ContainerBelong2SingleShipment(ByVal container As String, ByRef err As String) As Boolean
        Dim ret As Boolean = True
        Dim SQL As String
        Dim dt As New DataTable
        Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

        SQL = String.Format("SELECT * FROM vShipmentContainers WHERE CONTAINERID = '{0}' ", container)

        Made4Net.DataAccess.DataInterface.FillDataset(SQL, dt)

        If dt.Rows.Count > 1 Then
            err = trans.Translate("Container belong to more than one shipment")
            ret = False
        End If
        Return ret
    End Function
    'End Added for RWMS-2343 RWMS-2314

    Private Sub doNext()
        Try
            Dim trans As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
            If DO1.Value("PALLETID") = "" Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me, trans.Translate("Pallet cannot be empty"))
                Return
            End If

            'Added for RWMS-2343 RWMS-2314 - validate for container belongs to single shipment
            Dim err As String
            If ContainerExists(DO1.Value("PALLETID")) Then
                If Not ContainerBelong2SingleShipment(DO1.Value("PALLETID"), err) Then
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me, err)
                    DO1.Value("PALLETID") = ""
                    Return
                End If
            End If
            'End Added for RWMS-2343 RWMS-2314

            If Not getPalletID() Then
                DO1.Value("PALLETID") = ""
                Return
            Else
                Response.Redirect(MapVirtualPath("Screens/LOADING2DiffShip1.aspx"))
            End If
        Catch ex As Threading.ThreadAbortException
            'Do Nothing
        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Return
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.Message)
            Return
        End Try
    End Sub

    Private Sub doMenu()
        Session.Remove("LoadingPallet")
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddSpacer()
        DO1.AddTextboxLine("PALLETID", True, "next")
        DO1.AddSpacer()
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

