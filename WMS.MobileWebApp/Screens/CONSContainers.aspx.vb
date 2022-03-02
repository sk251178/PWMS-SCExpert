Imports Made4Net.Shared.Web
Imports Made4Net.Mobile
Imports Made4Net.Shared
Imports Made4Net.DataAccess
Imports Wms.Logic

<CLSCompliant(False)> Public Class CONSContainers
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

        DO1.FocusField = "CONTAINER"
    End Sub

    Sub CLEARFIELDS()
        DO1.Value("CONTAINER") = ""
        DO1.Value("ONCONTAINER") = ""
    End Sub

    Private Sub DO1_CreatedChildControls(ByVal sender As Object, ByVal e As System.EventArgs) Handles DO1.CreatedChildControls
        DO1.AddTextboxLine("CONTAINER", True, "next")
        DO1.AddTextboxLine("ONCONTAINER", True, "next")
        DO1.AddTextboxLine("PRINTER")
        DO1.AddSpacer()
    End Sub

    Private Function VerifyForm() As Boolean
        Dim Verified As Boolean = False
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Try
            If DO1.Value("CONTAINER") <> "" Then
                If verifyContainer() Then
                    Verified = True
                Else
                    Return Verified
                End If
            Else
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  "Container of load id not scanned")
                Return Verified
            End If

            If DO1.Value("ONCONTAINER") <> "" Then
                If verifyOnContainer() Then
                    Verified = True
                Else
                    Return Verified
                End If
            Else
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  "ONContainer of load id not scanned")
                Return Verified
            End If

        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Return Verified
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate(ex.Message))
            Return Verified

        End Try
        Return Verified
    End Function

    Private Function verifyOnContainer() As Boolean
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Dim CON As WMS.Logic.Container
        Try
            If Not WMS.Logic.Container.Exists(DO1.Value("ONCONTAINER")) Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("ONContainer does not exist."))
                DO1.Value("ONCONTAINER") = ""
                DO1.FocusField = "ONCONTAINER"
                Return False
            Else
                CON = New WMS.Logic.Container(DO1.Value("ONCONTAINER"), True)
                If WMS.Logic.Container.Exists(CON.OnContainer) Then
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("ONCONTAINER cannot be a base container, because it already defined as a subcontainer."))
                    DO1.Value("ONCONTAINER") = ""
                    DO1.FocusField = "ONCONTAINER"
                    Return False
                End If
                'Dim LoadCount As Int32 = Made4Net.DataAccess.DataInterface.ExecuteScalar(String.Format("select count(1) from INVLOAD where HANDLINGUNIT = '{0}'", DO1.Value("CONTAINER")))
                'If LoadCount = 0 Then
                '    HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("From Container Is Empty."))
                '    DO1.Value("ONCONTAINER") = ""
                '    Return False
                'End If
            End If

        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            DO1.FocusField = "ONCONTAINER"
            DO1.Value("ONCONTAINER") = ""
            Return False
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate(ex.Message))
            DO1.FocusField = "ONCONTAINER"
            DO1.Value("ONCONTAINER") = ""
            Return False
        End Try
        Return True
    End Function


    Private Function verifyContainer() As Boolean
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Try
            If Not WMS.Logic.Container.Exists(DO1.Value("CONTAINER")) Then
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Container does not exist."))
                DO1.Value("CONTAINER") = ""
                Return False
            Else
                Dim LoadCount As Int32 = Made4Net.DataAccess.DataInterface.ExecuteScalar(String.Format("select count(1) from INVLOAD where HANDLINGUNIT = '{0}'", DO1.Value("CONTAINER")))
                If LoadCount = 0 Then
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Container does not have any payloads"))
                    DO1.Value("CONTAINER") = ""
                    Return False
                End If
            End If

            'Dim dtOnContainer As DataTable = New DataTable
            'Dim SQLOnContainer As String = "SELECT * FROM INVLOAD WHERE HANDLINGUNIT='" & DO1.Value("ONCONTAINER") & "'" & _
            '                                " UNION " & _
            '                                "SELECT * FROM INVLOAD WHERE HANDLINGUNIT IN (SELECT CONTAINER FROM CONTAINER WHERE ONCONTAINER='" & DO1.Value("ONCONTAINER") & "')"
            'DataInterface.FillDataset(SQLOnContainer, dtOnContainer)

            'Dim dt As DataTable = New DataTable
            'Dim SQL As String

            'If dtOnContainer.Rows.Count > 0 Then
            '    If Not ValidateConsolidationParams() Then
            '        HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Consolidation params is not the same in all loads"))
            '        Return False
            '    Else
            '        Return True
            '    End If
            '    ' Than we need to check SHIPTO
            '    ' ''SQL = "SELECT SHIPMENT,SHIPTO FROM OUTBOUNDORHEADER WHERE CONSIGNEE+ORDERID IN (" & _
            '    ' ''      "SELECT CONSIGNEE+ORDERID FROM ORDERLOADS WHERE LOADID IN " & _
            '    ' ''      "(SELECT LOADID FROM CONTAINERLOADS WHERE CONTAINERID='" & DO1.Value("ONCONTAINER") & "' " & _
            '    ' ''      " UNION " & _
            '    ' ''      "SELECT LOADID FROM CONTAINERLOADS WHERE CONTAINERID IN (SELECT CONTAINER FROM CONTAINER WHERE ONCONTAINER='" & DO1.Value("ONCONTAINER") & "') )) " & _
            '    ' ''      "GROUP BY SHIPMENT,SHIPTO"
            '    ' ''DataInterface.FillDataset(SQL, dt)
            '    ' ''For Each dr As DataRow In dt.Rows
            '    ' ''    ' Check the shipto
            '    ' ''    Dim CheckSQL As String = "SELECT COUNT(1) FROM OUTBOUNDORHEADER WHERE CONSIGNEE+ORDERID IN (" & _
            '    ' ''                             "SELECT CONSIGNEE+ORDERID FROM ORDERLOADS WHERE LOADID IN (SELECT LOADID FROM CONTAINERLOADS WHERE CONTAINERID='" & DO1.Value("CONTAINER") & "'))" & _
            '    ' ''                             "AND (SHIPTO='" & dr("SHIPTO") & "' OR SHIPMENT='" & dr("SHIPMENT") & "')"
            '    ' ''    If DataInterface.ExecuteScalar(CheckSQL) = 0 Then
            '    ' ''        HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("SHIPTO is not the same in all loads"))
            '    ' ''        Return False
            '    ' ''    End If
            '    ' ''Next
            'Else
            '    Return True
            'End If
        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            DO1.Value("CONTAINER") = ""
            Return False
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate(ex.Message))
            DO1.Value("CONTAINER") = ""
            Return False
        End Try
        Return True
    End Function

    Private Sub doNext()

        Dim tocnt As WMS.Logic.Container
        Dim fromcont As WMS.Logic.Container
        'Dim fromload As WMS.Logic.Load
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

        If String.IsNullOrEmpty(DO1.Value("CONTAINER")) Or String.IsNullOrEmpty(DO1.Value("ONCONTAINER")) Or DO1.Value("CONTAINER") = DO1.Value("ONCONTAINER") Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Illegal Container"))
            CLEARFIELDS()
            Return
        End If

        If Not VerifyForm() Then
            CLEARFIELDS()
            Return
        End If
        Try
            'If WMS.Logic.Container.Exists(DO1.Value("ONCONTAINER")) Then
            tocnt = New WMS.Logic.Container(DO1.Value("ONCONTAINER"), True)
            'ElseIf WMS.Logic.Load.Exists(DO1.Value("ONCONTAINER")) Then
            '    Dim loadObj As New WMS.Logic.Load(DO1.Value("ONCONTAINER"))
            '    If loadObj.ACTIVITYSTATUS = WMS.Lib.Statuses.ActivityStatus.PICKED OrElse _
            '        loadObj.ACTIVITYSTATUS = WMS.Lib.Statuses.ActivityStatus.STAGED OrElse _
            '        loadObj.ACTIVITYSTATUS = WMS.Lib.Statuses.ActivityStatus.VERIFIED Then
            '        If Not String.IsNullOrEmpty(loadObj.ContainerId) Then
            '            tocnt = New WMS.Logic.Container(loadObj.ContainerId, True)
            '        Else
            '            tocnt.ContainerId = DO1.Value("ONCONTAINER")
            '            tocnt.Save(WMS.Logic.GetCurrentUser)
            '        End If
            '    End If
            'Else
            '    HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("No load or container with entered ONCONTAINER exists."))
            '    Return
            'End If
        Catch ex As Made4Net.Shared.M4NException
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
            Return
        Catch ex As Exception
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate(ex.Message))
            Return
        End Try

        'If Not DO1.Value("FROMLOAD") = String.Empty Then
        '    Try
        '        fromload = New WMS.Logic.Load(DO1.Value("FROMLOAD"))
        '    Catch ex As Made4Net.Shared.M4NException
        '        HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
        '        Return
        '    Catch ex As Exception
        '        HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate(ex.Message))
        '        Return
        '    End Try

        '    Try
        '        cons = New WMS.Logic.Consolidation
        '        cons.Consolidate(fromload, tocnt, WMS.Logic.Common.GetCurrentUser)
        '        HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Load Consolidated"))
        '        Dim aq As New EventManagerQ
        '        aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
        '        aq.Add("ACTIVITYTIME", "0")
        '        aq.Add("USERID", WMS.Logic.GetCurrentUser)
        '        aq.Add("ADDUSER", WMS.Logic.GetCurrentUser)
        '        aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        '        aq.Add("EDITUSER", WMS.Logic.GetCurrentUser)
        '        aq.Add("LASTSTATUSDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        '        aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
        '        aq.Add("EVENT", "8006")
        '        aq.Add("ACTIVITYTYPE", "CONTCONS")
        '        aq.Add("FROMLOAD", DO1.Value("FROMLOAD"))
        '        aq.Add("ONCONTAINER", DO1.Value("ONCONTAINER"))
        '        aq.Send("CONTCONS")
        '    Catch ex As Exception
        '        Return
        '    End Try
        'Else
        If Not DO1.Value("CONTAINER") Is String.Empty Then
            Try
                fromcont = New WMS.Logic.Container(DO1.Value("CONTAINER"), True)
            Catch ex As Made4Net.Shared.M4NException
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
                Return
            Catch ex As Exception
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate(ex.Message))
                CLEARFIELDS()
                Return
            End Try

            Try
                'cons = New WMS.Logic.Consolidation
                'cons.Consolidate(fromcont, tocnt, WMS.Logic.Common.GetCurrentUser)
                Dim sql As String = String.Format("update container set oncontainer='{0}' where container='{1}'", tocnt.ContainerId, fromcont.ContainerId)
                Made4Net.DataAccess.DataInterface.ExecuteScalar(sql)
                DO1.Value("CONTAINER") = ""

                'Dim aq As New EventManagerQ
                'aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
                'aq.Add("ACTIVITYTIME", "0")
                'aq.Add("USERID", WMS.Logic.GetCurrentUser)
                'aq.Add("ADDUSER", WMS.Logic.GetCurrentUser)
                'aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
                'aq.Add("EDITUSER", WMS.Logic.GetCurrentUser)
                'aq.Add("LASTSTATUSDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
                'aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
                'aq.Add("EVENT", "8006")
                'aq.Add("ACTIVITYTYPE", "CONTCONS")
                'aq.Add("CONTAINER", fromcont.ContainerId)
                'aq.Add("ONCONTAINER", tocnt.ContainerId)
                'aq.Send("CONTCONS")
                'HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Container Consolidated"))

                Dim MSG As String = "PLACECONT"
                Dim UserId As String = WMS.Logic.Common.GetCurrentUser

                Dim aq As EventManagerQ = New EventManagerQ
                aq.Add("EVENT", "2009")
                aq.Add("ACTIVITYTYPE", MSG)
                aq.Add("ACTIVITYDATE", Made4Net.Shared.Util.DateTimeToWMSString(DateTime.Now))
                aq.Add("ACTIVITYTIME", "0")

                aq.Add("FROMCONTAINER", fromcont.ContainerId)
                aq.Add("TOCONTAINER", tocnt.ContainerId)
                'aq.Add("CONTAINER", fromcont.ContainerId)
                'aq.Add("ONCONTAINER", tocnt.ContainerId)

                aq.Add("USERID", WMS.Logic.GetCurrentUser)
                aq.Add("ADDDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
                aq.Add("ADDUSER", WMS.Logic.GetCurrentUser)
                aq.Add("EDITDATE", Made4Net.[Shared].Util.DateTimeToWMSString(DateTime.Now))
                aq.Add("EDITUSER", WMS.Logic.GetCurrentUser)
                aq.Add("FROMWAREHOUSEAREA", WMS.Logic.Warehouse.getUserWarehouseArea())
                aq.Add("TOWAREHOUSEAREA", WMS.Logic.Warehouse.getUserWarehouseArea())

                aq.Send(MSG)

                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("container placed on container"))

            Catch ex As Made4Net.Shared.M4NException
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
                Return
            Catch ex As Exception
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate(ex.Message))
                CLEARFIELDS()
                Return
            End Try
        Else
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Load or Container not entered"))
            CLEARFIELDS()
            Return
        End If
        'DO1.Value("FROMLOAD") = ""
    End Sub

    Private Sub PrintPackingLabel(ByVal lblPrinter As String, ByVal LblType As String, ByVal ContainerID As String)
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)
        Dim con As WMS.Logic.Container

        If WMS.Logic.Container.Exists(DO1.Value("CONTAINER")) Then
            con = New WMS.Logic.Container(DO1.Value("CONTAINER"), True)
            con.PrintShipLabel()
        ElseIf WMS.Logic.Load.Exists(DO1.Value("CONTAINER")) Then
            Dim ld As New WMS.Logic.Load(DO1.Value("CONTAINER"))
            If Not String.IsNullOrEmpty(ld.ContainerId) Then
                If WMS.Logic.Container.Exists(ld.ContainerId) Then
                    con = New WMS.Logic.Container(ld.ContainerId, True)
                    con.PrintShipLabel()
                Else
                    DO1.Value("CONTAINER") = ""
                    HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Container does not exist"))
                End If
            Else
                DO1.Value("CONTAINER") = ""
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Container does not exist"))
            End If
        Else
            DO1.Value("CONTAINER") = ""
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("Container does not exist"))
        End If

        'Dim qSender As New QMsgSender
        'qSender.Add("LABELNAME", "PACKLABEL")
        'qSender.Add("LabelType", "PACKLABEL")
        'qSender.Add("CONTAINERID", ContainerID)
        'Dim ht As Made4Net.General.Serialization.HashTableWrapper = New Made4Net.General.Serialization.HashTableWrapper
        'ht.Hash.Add("CONTAINERID", ContainerID)
        ''ht.Hash.Add("LABELTYPES", LblType)
        'qSender.Add("LabelDataParameters", GetHashTableXMLString(ht))
        'qSender.Add("PRINTER", "")
        'qSender.Send("Label", "Container Pack Label")
    End Sub

    Private Sub doMenu()
        Made4Net.Mobile.Common.GoToMenu()
    End Sub

    Private Sub DO1_ButtonClick(ByVal sender As Object, ByVal e As Made4Net.Mobile.WebCtrls.ButtonClickEventArgs) Handles DO1.ButtonClick
        Select Case e.CommandText.ToLower
            Case "place"
                doNext()
            Case "menu"
                doMenu()
            Case "print label"
                PrintContainerLabel()
                ' PrintPackingLabel("", "", DO1.Value("ONCONTAINER"))
        End Select
    End Sub


    Public Sub PrintContainerLabel()
        Dim strcont As String
        Dim lblPrinter As String
        Dim fromcont As WMS.Logic.Container
        Dim t As New Made4Net.Shared.Translation.Translator(Made4Net.Shared.Translation.Translator.CurrentLanguageID)

        If String.IsNullOrEmpty(DO1.Value("ONCONTAINER")) Then
            HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate("illegal ONCONTAINER"))
            Return
        Else
            Try
                strcont = DO1.Value("ONCONTAINER")
                fromcont = New WMS.Logic.Container(strcont, True)
            Catch ex As Made4Net.Shared.M4NException
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  ex.GetErrMessage(Made4Net.Shared.Translation.Translator.CurrentLanguageID))
                Return
            Catch ex As Exception
                HandheldPopupNAlertMessageHandler.DisplayMessage(Me,  t.Translate(ex.Message))
                Return
            End Try
        End If


        If lblPrinter Is Nothing Then
            If String.IsNullOrEmpty(DO1.Value("PRINTER")) Then
                lblPrinter = ""
            Else
                lblPrinter = DO1.Value("PRINTER")
            End If
        End If
        Dim qSender As New QMsgSender
        qSender.Add("LABELNAME", "SHIPCONT")
        qSender.Add("LabelType", "SHIPCONT")
        qSender.Add("PRINTER", lblPrinter)
        qSender.Add("WAREHOUSE", Made4Net.Shared.Warehouse.CurrentWarehouse)
        qSender.Add("CONTAINERID", strcont)
        Dim ht As Made4Net.General.Serialization.HashTableWrapper = New Made4Net.General.Serialization.HashTableWrapper
        ht.Hash.Add("CONTAINERID", strcont)
        qSender.Add("LabelDataParameters", WMS.Logic.Common.GetHashTableXMLString(ht))
        qSender.Send("LABEL", String.Format("SHIPCONT Container Label ({0})", strcont))
    End Sub

End Class